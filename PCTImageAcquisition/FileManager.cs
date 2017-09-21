using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PCTImageAcquisition
{
	public class FileManager : IDisposable
	{
		public int NumFrames { get; private set; } // Number of frames in the file

		private const int FileHeader = 16;
		private const int FrameHeader = 4;
		private const int ImageLength = Raw2Image.ImageCol * Raw2Image.ImageRow;
		private const int pb = Raw2Image.PixelBytes;
		private const int FrameLength = FrameHeader + ImageLength * pb;

		private uint[] imageBuffer;
		private int[] modIdBuffer;
		private byte[] binform;
		private FileStream fileStream;
		private DataManager dataMan;
		private int nFrames, segStartPos, segEndPos;

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

		public void BeginWrite()
		{
			byte[] header = new byte[FileHeader];
			header[0] = 0x58;   //'X'
			header[1] = 0x50;   //'P'
			header[2] = 0x43;   //'C'
			header[3] = 0x54;   //'T'
			header[8] = 0x84;
			header[9] = 0x4;    // 0x484 = 1156 is the FrameLength
			fileStream.Write(header, 0, FileHeader);
			nFrames = 0;
			dataMan.Clear();
		}

		public bool BeginRead()
		{
			byte[] header = new byte[FileHeader];
			fileStream.Read(header, 0, FileHeader);
			byte[] XPCT = { 0x58, 0x50, 0x43, 0x54 };
			for (int i = 0; i < 4; i++)
				if (header[i] != XPCT[i])
					return false;
			NumFrames = BitConverter.ToInt32(header, 4);
			segEndPos = -1; // trigger Loading file
			dataMan.Clear();
			return true;
		}

		public void EndWriting()
		{
			dataMan.Data.CopyTo(imageBuffer, 0);
			dataMan.ModID.CopyTo(modIdBuffer, 0);
			for (int i = 0; i < dataMan.Length; i++)
			{
				int frameStart = i * FrameLength;
				binform[frameStart] = (byte)dataMan.ModID[i];   //only the 1st byte used, among 4 bytes
				for (int j = 0; j < ImageLength; j++)
				{
					uint pxInt = dataMan.Data[i * ImageLength + j];
					int sub = FrameHeader + frameStart + j * pb;
					binform[sub] = (byte)(pxInt >> 16);     //Big-endian
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
							byte* sub = pbinform + FrameHeader + frameStart + j * pb;
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
		/// Load into DataManager the frames around pos.
		/// If the segment of data were already loaded, returns the relative position.
		/// </summary>
		/// <param name="pos"> the pa</param>
		/// <returns>0-based location of position in the DataManager. -1 for error</returns>
		public int LoadFileSegmentAtPos(int pos)
		{
			if (pos >= NumFrames)
				return -1;
			if (pos >= segStartPos && pos <= segEndPos - Raw2Image.NumDetectorModules)
				return pos - segStartPos;
			if (pos >= NumFrames - Raw2Image.NumDetectorModules && dataMan.Length != 0) // for the last 5 frames
				return pos - segStartPos;
			segStartPos = pos - dataMan.FrameCapacity / 2;
			segStartPos = segStartPos > 0 ? segStartPos : 0;
			segEndPos = pos + (dataMan.FrameCapacity - 1) / 2;
			segEndPos = segEndPos < NumFrames ? segEndPos : NumFrames - 1;

			fileStream.Seek(FileHeader + (long)segStartPos * FrameLength, SeekOrigin.Begin);
			fileStream.Read(binform, 0, binform.Length);
			for (int i = 0; i < segEndPos - segStartPos; i++)
			{
				modIdBuffer[i] = binform[FrameLength * i];    //only read the 1st byte as modId
				int frameDataStart = FrameLength * i + FrameHeader;
				for (int j = 0; j < ImageLength; j++)
				{
					int p = frameDataStart + j * pb;
					uint hi = binform[p];       // big-endian
					uint mid = binform[p + 1];
					uint low = binform[p + 2];
					imageBuffer[ImageLength * i + j] = (hi << 16) + (mid << 8) + low;
				}
			}
			imageBuffer.CopyTo(dataMan.Data, 0);
			modIdBuffer.CopyTo(dataMan.ModID, 0);
			dataMan.Length = segEndPos - segStartPos + 1;
			return pos - segStartPos;
		}
	}
}
