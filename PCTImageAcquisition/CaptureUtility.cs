using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PCTImageAcquisition
{
	public static class CaptureUtility
	{
		public static int __numInvalidFrames;		//only for debug

		private static NetAcquisition capture;
		private static bool[] moduleOK;
		private static int[] fullImage;
		private static int[] data;
		private static readonly int imageLength;
		private static volatile bool requestStop;

		static CaptureUtility()
		{
			if (capture == null)
				capture = new NetAcquisition();
			moduleOK = new bool[5];
			imageLength = Raw2Image.ImageCol*Raw2Image.ImageRow;
			fullImage =new int[imageLength * 5];
		}
		public static int[] CaptureOneFullview(int timeout=100)
		{
			if (capture.IsRunning)
				return fullImage;
			requestStop = false;
			__numInvalidFrames = 0;
			Array.Clear(moduleOK, 0, 5);

			Stopwatch sw=new Stopwatch();
			sw.Start();
			capture.StartAcquisitionAsync(cbFillBuffer);
			while (!requestStop)
			{
				if (Array.TrueForAll(moduleOK, i => i))
					break;
				if (sw.ElapsedMilliseconds > timeout)
				{
					__numInvalidFrames = -1;
					break;
				}
			}
			sw.Reset();
			capture.StopAcquistion();
			requestStop = false;
			
			return fullImage;
		}

		public static void StopCapture()
		{
			if (!capture.IsRunning)
				return;
			capture.StopAcquistion();
			requestStop = true;
		}

		private static void cbFillBuffer(byte[] rawData)
		{
			if (!Raw2Image.IsValidFrame(rawData))
			{
				__numInvalidFrames++;
				return;
			}
			int modId;
			int[] modBytes = Raw2Image.ExtractImageData(rawData, out modId);
			modId--; //modId must be 0-based

			for (int row = 0; row < Raw2Image.ImageRow; row++)
				Array.Copy(modBytes, row*Raw2Image.ImageCol, fullImage, (row*5 + modId)*Raw2Image.ImageCol, Raw2Image.ImageCol);
			moduleOK[modId] = true;
		}

		public static bool CaptureAndStore(int timeout=50)
		{
			throw new NotImplementedException();
			if (capture.IsRunning)
				return false;
			requestStop = false;
			Array.Clear(moduleOK, 0, 5);
			capture.StartAcquisitionAsync(cbFillBuffer);
			while (!requestStop)
			{
				if (Array.TrueForAll(moduleOK, i => i))
					break;
			}
			capture.StopAcquistion();
			requestStop = false;

			return true;
		}

	}
}
