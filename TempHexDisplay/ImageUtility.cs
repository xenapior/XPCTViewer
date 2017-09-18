using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace TempHexDisplay
{
	class ImageUtility
	{
		public const int ImageWidth = 24, ImageHeight = 16;
		public bool bNetCaptureMode;

		public int[] data;
		public int dataMax = 0xffffff, dataMin = 0;
		public int nFrames = 5;
		public Bitmap image;

		public int viewMag = 8, currentGroup = 0;
		public bool bGrayscaling = true, bShowSplitter = true;

		private bool filedataReady;

		public ImageUtility()
		{
			data = new int[ImageWidth * ImageHeight * 5];
			UpdateImage();
		}

		public void OpenK12file(Stream fileStream)
		{
			TextReader treader = new StreamReader(fileStream);
			string txt = treader.ReadToEnd();
			treader.Close();
			data = Str2IntArray(txt);
			UpdateImage();
		}

		private int[] Str2IntArray(string tData)
		{
			string[] byteTexts = tData.Split('|');
			int framehead = 49, frametail = 105;
			int frameLen = ImageWidth * ImageHeight;
			int dataPtr = 0, frameStride = frameLen * 3 + framehead + frametail;
			int tempDataMin = dataMin, tempDataMax = dataMax, tempnFrames = nFrames;

			nFrames = byteTexts.Length / frameStride;
			if (nFrames == 0)
			{
				nFrames = tempnFrames;
				filedataReady = false;
				return data;
			}
			int[] result = new int[nFrames * frameLen];

			dataMax = 0;
			dataMin = 0xffffff;

			for (int frame = 0; frame < nFrames; frame++)
			{
				int frameStart = frame * frameStride + framehead;
				for (int i = 0; i < frameLen; i++)
				{
					if (byteTexts[i * 3 + frameStart].Length != 2 && byteTexts[i * 3 + frameStart + 1].Length != 2 || byteTexts[i * 3 + frameStart + 2].Length != 2)
					{
						nFrames = tempnFrames;
						dataMin = tempDataMin;
						dataMax = tempDataMax;
						filedataReady = false;
						return data;
					}
					int pixelValue = Convert.ToInt32(byteTexts[i * 3 + frameStart] + byteTexts[i * 3 + frameStart + 1] + byteTexts[i * 3 + frameStart + 2], 16);
					dataMax = pixelValue > dataMax ? pixelValue : dataMax;
					dataMin = pixelValue < dataMin ? pixelValue : dataMin;
					result[dataPtr++] = pixelValue;
				}
			}
			if (dataMax == dataMin)
			{
				dataMax = dataMin + 1;
			}

			filedataReady = true;
			return result;
		}

		public void UpdateImage()
		{
			if (bNetCaptureMode)
				FillBitmap();
			else
				UpdateSegImage();
		}
		private void UpdateSegImage()
		{
			if (image == null || ImageWidth * viewMag != image.Width || ImageHeight * viewMag != image.Height)
			{
				image?.Dispose();
				image = new Bitmap(ImageWidth * viewMag * 5, ImageHeight * viewMag, PixelFormat.Format24bppRgb);
			}

			var pdata = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
			int minVal = bGrayscaling ? dataMin : 0;
			int s = pdata.Stride;
			int w = ImageWidth * viewMag;
			int h = ImageHeight * viewMag;
			int groupStart = currentGroup * 5 * ImageWidth * ImageHeight;
			unsafe
			{
				byte* addr = (byte*)pdata.Scan0;
				for (int k = 0; k < 5; k++)
				{
					int regionStart = k * ImageWidth * ImageHeight + groupStart;
					for (int i = 0; i < h; i++)
					{
						for (int j = 0; j < w; j++)
						{
							int dataPos = (i / viewMag) * ImageWidth + (j / viewMag) + regionStart;
							//							int dataPos = (i / viewMag) + (j / viewMag)*ImageHeight + regionStart;
							byte grayscale = (byte)((data[dataPos] - minVal) * 255 / (dataMax - minVal));
							addr[i * s + (k * w + j) * 3] = grayscale;
							addr[i * s + (k * w + j) * 3 + 1] = grayscale;
							addr[i * s + (k * w + j) * 3 + 2] = grayscale;
						}
						if (bShowSplitter)
						{
							addr[i * s + k * w * 3] = 0;
							addr[i * s + k * w * 3 + 1] = 0xff;
							addr[i * s + k * w * 3 + 2] = 0;
						}
					}
				}
			}
			image.UnlockBits(pdata);
		}

		private void FillBitmap()
		{
			int w = ImageWidth * 5 * viewMag;
			int h = ImageHeight * viewMag;

			if (image == null || w != image.Width || h != image.Height)
			{
				image?.Dispose();
				image = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			}

			dataMin = 0xffffff;
			dataMax = 0;
			for (int i = 0; i < data.Length; i++)
			{
				int temp = data[i];
				dataMax = temp > dataMax ? temp : dataMax;
				dataMin = temp < dataMin ? temp : dataMin;
			}
			if (dataMax == dataMin)
				dataMax = dataMin + 1;

			int minVal = bGrayscaling ? dataMin : 0;
			var pdata = image.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
			int s = pdata.Stride;

			unsafe
			{
				byte* addr = (byte*)pdata.Scan0;

				for (int i = 0; i < h; i++)
				{
					for (int j = 0; j < w; j++)
					{
						int dataPos = (i / viewMag) * ImageWidth * 5 + (j / viewMag);
						byte grayscale = (byte)((data[dataPos] - minVal) * 255 / (dataMax - minVal));
						addr[i * s + j * 3] = grayscale;
						addr[i * s + j * 3 + 1] = grayscale;
						addr[i * s + j * 3 + 2] = grayscale;
						if (bShowSplitter && j % (ImageWidth * viewMag) == 0)
						{
							addr[i * s + j * 3] = 0;
							addr[i * s + j * 3 + 1] = 0xff;
							addr[i * s + j * 3 + 2] = 0;
						}
					}
				}
			}

			image.UnlockBits(pdata);
			nFrames = 5;
		}

	}
}
