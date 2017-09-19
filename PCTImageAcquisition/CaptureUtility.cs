using System;


namespace PCTImageAcquisition
{
	/// <summary>
	/// Simple wrapper for easier use
	/// </summary>
	public static class CaptureUtility
	{
		public static int __numInvalidFrames;   //only for debug

		private static NetAcquisition capturer;		// network layer for capturing
		private static volatile DataManager dataManager;	// data storage manager

		// Send a stop request
		public static void StopCapture()
		{
			if (capturer == null)
				capturer = new NetAcquisition();
			if (!capturer.IsRunning)
				return;
			capturer.StopAcquistion();
		}

		// Start capturing and write it to DataManager
		public static bool StartCaptureAsync(DataManager dataMan)
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
			uint[] imageInts = Raw2Image.ExtractImageData(rawData, out modId);

			dataManager.Add(modId, imageInts);
		}
	}
}
