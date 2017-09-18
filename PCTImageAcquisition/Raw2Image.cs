using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PCTImageAcquisition
{
	public static class Raw2Image
	{
		public const int NumDetectorModules = 5;
		public const int PixelBytes = 3;
		public const int ImageCol = 24;
		public const int ImageRow = 16;

		public static bool BigEndian = true;
		public static int FrameHeader = 5;
		public static int FrameTrailer = 105;

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);
		static Raw2Image()
		{
			if (File.Exists(Resources.settingfilePath))
			{
				int result = GetPrivateProfileInt("RawFormat", "header length", 0, Resources.settingfilePath);
				if (result > 0)
					FrameHeader = result;
				result = GetPrivateProfileInt("RawFormat", "trailer length", 0, Resources.settingfilePath);
				if (result > 0)
					FrameTrailer = result;
				result = GetPrivateProfileInt("RawFormat", "big endian", 1, Resources.settingfilePath);
				BigEndian = (result == 1);
			}
		}

		public static bool IsValidFrame(byte[] data)
		{
			int standardLength = ImageCol * ImageRow * PixelBytes;
			if (data.Length != standardLength + FrameHeader + FrameTrailer)
				return false;
			if (data[0] != 0xf8)
				return false;
			if (data[1] > NumDetectorModules)
				return false;
			return true;
		}

		// Make sure to check for udpData validity beforehand using IsValidFrame(udpData)
		public static int[] ExtractImageData(byte[] udpData, out int moduleId)
		{
			int standardLength = ImageCol * ImageRow;
			int[] pxData = new int[standardLength];
			int hi, mid, low;
			if (BigEndian)
			{
				for (int i = 0; i < standardLength; i++)
				{
					int p = FrameHeader + i * PixelBytes;
					hi = udpData[p];
					mid = udpData[p + 1];
					low = udpData[p + 2];
					pxData[i] = (hi << 16) + (mid << 8) + low;
				}
			}
			else
			{
				for (int i = 0; i < standardLength; i++)
				{
					int p = FrameHeader + i * PixelBytes;
					hi = udpData[p + 2];
					mid = udpData[p + 1];
					low = udpData[p];
					pxData[i] = (hi << 16) + (mid << 8) + low;
				}

			}
			moduleId = udpData[1];
			return pxData;
		}
	}
}
