using System;


namespace PCTImageAcquisition
{
	public static class CaptureUtility
	{
		public static int __numInvalidFrames;   //only for debug

		private const int ImageLength = Raw2Image.ImageCol * Raw2Image.ImageRow;
		private static NetAcquisition capturer;
		private static volatile DataManager dataManager;
		private static readonly int[] blackImage;

		static CaptureUtility()
		{
			blackImage = new int[ImageLength];
		}
		public static int[] PeekMostRecentImage(int modId)
		{
			if (capturer == null)
				return blackImage;
			int pos = dataManager.Length == 0 ? 0 : dataManager.Length - 1;
			while (true)
			{
				if (dataManager.ModID[pos] == modId)
					break;
				pos--;
				if (pos < 0)
					pos = dataManager.FrameCapacity - 1;
				if (dataManager.Length - 1 == pos)  //not found over a round search
				{
					return blackImage;
				}
			}
			int[] im = new int[ImageLength];
			Array.Copy(dataManager.Data, pos * ImageLength, im, 0, ImageLength);
			return im;
		}

		public static void StopCapture()
		{
			if (capturer == null)
				capturer = new NetAcquisition();
			if (!capturer.IsRunning)
				return;
			capturer.StopAcquistion();
		}

		public static bool CaptureAndStore(DataManager dataMan)
		{
			if (capturer == null)
				capturer = new NetAcquisition();
			if (capturer.IsRunning)
				return false;
			dataManager = dataMan;
			capturer.StartAcquisitionAsync(cbFillDataManager);

			return true;
		}

		private static void cbFillDataManager(byte[] rawData)
		{
			if (!Raw2Image.IsValidFrame(rawData))
			{
				__numInvalidFrames++;
				return;
			}
			int modId;
			int[] imageInts = Raw2Image.ExtractImageData(rawData, out modId);

			dataManager.Add(modId, imageInts);
		}
	}
}
