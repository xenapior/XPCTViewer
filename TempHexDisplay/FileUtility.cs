using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCTImageAcquisition;
namespace XPCTViewer
{
	public class FileUtility : IDisposable
	{
		private uint[] memBuffer;
		private int[] modId;
		private Stream fileStream;
		private DataManager dataMan;

		private const int ImageLength = Raw2Image.ImageCol * Raw2Image.ImageRow;
		private const int pb = Raw2Image.PixelBytes;
		private const int FrameLength = 4 + ImageLength * pb;
		private byte[] binform;
		public FileUtility(Stream fStream, DataManager dMan)
		{
			fileStream = fStream;
			dataMan = dMan;
			dataMan.SetBufferfullCallback(cbSave2FileAsync);
			memBuffer = new uint[ImageLength * dataMan.FrameCapacity];
			modId = new int[dataMan.FrameCapacity];
			binform=new byte[FrameLength * dataMan.FrameCapacity];
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
			byte[] XPCT = Encoding.ASCII.GetBytes("XPCT");
			XPCT.CopyTo(header, 0);
			fileStream.Write(header, 0, 16);
		}

		public bool ReadHeader()
		{
			byte[] header = new byte[16];
			fileStream.Read(header, 0, 16);
			byte[] XPCT = Encoding.ASCII.GetBytes("XPCT");
			for (int i = 0; i < 4; i++)
			{
				if (header[i] != XPCT[i])
					return false;
			}
			return true;
		}

		public void Flush()
		{
			dataMan.Data.CopyTo(memBuffer, 0);
			dataMan.ModID.CopyTo(modId, 0);
			for (int i = 0; i < dataMan.FrameCapacity; i++)
			{
				binform[i * FrameLength] = (byte)dataMan.ModID[i];
				for (int j = 0; j < ImageLength; j++)
				{
					uint pxInt = dataMan.Data[i * ImageLength + j];
					binform[4 + j * pb] = (byte)(pxInt >> 16);
					binform[5 + j * pb] = (byte)(pxInt >> 8);
					binform[6 + j * pb] = (byte)pxInt;
				}
			}
			fileStream.WriteAsync(binform, 0, dataMan.Length*FrameLength);
			dataMan.Clear();
		}

		private void cbSave2FileAsync()
		{
			dataMan.Data.CopyTo(memBuffer, 0);
			dataMan.ModID.CopyTo(modId, 0);
			for (int i = 0; i < dataMan.FrameCapacity; i++)
			{
				binform[i * FrameLength] = (byte)dataMan.ModID[i];
				for (int j = 0; j < ImageLength; j++)
				{
					uint pxInt = dataMan.Data[i * ImageLength + j];
					int sub = i * FrameLength + j * pb + 4;
					binform[sub] = (byte)(pxInt >> 16);
					binform[sub + 1] = (byte)(pxInt >> 8);
					binform[sub + 2] = (byte)pxInt;
				}
			}
			fileStream.WriteAsync(binform, 0, binform.Length);
			dataMan.Clear();
		}
	}
}
