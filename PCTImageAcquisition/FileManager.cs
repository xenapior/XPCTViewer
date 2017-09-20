﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PCTImageAcquisition
{
	public class FileManager : IDisposable
	{
		public int NumFrames;

		private uint[] imageBuffer;
		private int[] modIdBuffer;
		private byte[] binform;
		private FileStream fileStream;
		private DataManager dataMan;
		private const int ImageLength = Raw2Image.ImageCol * Raw2Image.ImageRow;
		private const int pb = Raw2Image.PixelBytes;
		private const int FrameLength = 4 + ImageLength * pb;
		private int nFrames, lastGroup;

		public FileManager(FileStream fStream, DataManager dataManager)
		{
			fileStream = fStream;
			dataMan = dataManager;
			dataMan.SetBufferfullCallback(cbSaveToFile);
			imageBuffer = new uint[ImageLength * dataMan.FrameCapacity];
			modIdBuffer = new int[dataMan.FrameCapacity];
			binform = new byte[FrameLength * dataMan.FrameCapacity];
		}

		public void Dispose()
		{
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream = null;
			}
		}

		public void WriteHeader()
		{
			byte[] header = new byte[16];
			header[0] = 0x58;   //'X'
			header[1] = 0x50;   //'P'
			header[2] = 0x43;   //'C'
			header[3] = 0x54;   //'T'
			header[8] = 0x84;
			header[9] = 0x4;    // 0x484 = 1156 is the FrameLength
			fileStream.Write(header, 0, 16);
		}

		public bool ReadHeader()
		{
			byte[] header = new byte[16];
			fileStream.Read(header, 0, 16);
			byte[] XPCT = { 0x58, 0x50, 0x43, 0x54 };
			for (int i = 0; i < 4; i++)
			{
				if (header[i] != XPCT[i])
					return false;
			}
			NumFrames = BitConverter.ToInt32(header, 4);
			return true;
		}

		public void EndWriting()
		{
			dataMan.Data.CopyTo(imageBuffer, 0);
			dataMan.ModID.CopyTo(modIdBuffer, 0);
			for (int i = 0; i < dataMan.Length; i++)
			{
				int frameStart = i * FrameLength;
				binform[frameStart] = (byte)dataMan.ModID[i];
				for (int j = 0; j < ImageLength; j++)
				{
					uint pxInt = dataMan.Data[i * ImageLength + j];
					int sub = 4 + frameStart + j * pb;
					binform[sub] = (byte)(pxInt >> 16);
					binform[sub + 1] = (byte)(pxInt >> 8);
					binform[sub + 2] = (byte)pxInt;
				}
			}
			fileStream.Write(binform, 0, dataMan.Length * FrameLength);
			nFrames += dataMan.Length;
			fileStream.Seek(4, SeekOrigin.Begin);   // seek to 0x4 header position
			fileStream.Write(BitConverter.GetBytes(nFrames), 0, 4);
			dataMan.Clear();
		}

		private void cbSaveToFile()
		{
			dataMan.Data.CopyTo(imageBuffer, 0);
			dataMan.ModID.CopyTo(modIdBuffer, 0);
			unsafe
			{
				fixed (byte* pbinform = binform)
				{
					for (int i = 0; i < dataMan.FrameCapacity; i++)
					{
						int frameStart = i * FrameLength;
						*(pbinform + frameStart) = (byte)dataMan.ModID[i];
						for (int j = 0; j < ImageLength; j++)
						{
							uint pxInt = dataMan.Data[i * ImageLength + j];
							byte* sub = pbinform + 4 + frameStart + j * pb;
							*(sub) = (byte)(pxInt >> 16);
							*(sub + 1) = (byte)(pxInt >> 8);
							*(sub + 2) = (byte)pxInt;
						}
					}
				}
			}
			fileStream.WriteAsync(binform, 0, binform.Length);
			nFrames += dataMan.FrameCapacity;
			dataMan.Clear();
		}

		/// <summary>
		/// Load into DataManager the frames around position
		/// </summary>
		/// <param name="position"></param>
		/// <returns>0-based location of position in the DataManager. -1 for error</returns>
		public int LoadFileSegmentAtPos(int position)
		{
			if (position >= NumFrames)
				return -1;

			int 
		}
	}

}