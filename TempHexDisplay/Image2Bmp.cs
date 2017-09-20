using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using PCTImageAcquisition;

namespace XPCTViewer
{
	internal class Image2Bmp
	{
		public Bitmap[] BmpImages;
		public uint PlotMax, PlotMin;
		public int Mag = 8;
		public bool bGrayscaling = true, bShowModuleBorder = true;
		public readonly uint[][] BufferInts;
		public uint DataMax, DataMin;

		public Image2Bmp()
		{
			BmpImages = new Bitmap[Raw2Image.NumDetectorModules];
			BufferInts = new uint[Raw2Image.NumDetectorModules][];

			for (int i = 0; i < BmpImages.Length; i++)
			{
				BmpImages[i] = new Bitmap(Raw2Image.ImageCol * Mag, Raw2Image.ImageRow * Mag, PixelFormat.Format24bppRgb);
				BufferInts[i] = new uint[Raw2Image.ImageCol * Raw2Image.ImageRow];
			}
		}

		/// <summary>
		/// Copy image data into target buffer
		/// </summary>
		/// <param name="imageInts">Source data in int32</param>
		/// <param name="position">0-based position of the bitmap</param>
		public void FillBufferData(uint[] imageInts, int position)
		{
			imageInts.CopyTo(BufferInts[position], 0);
		}

		private void UpdateBitmap(uint[] imageInts, int position)
		{
			int w = Raw2Image.ImageCol * Mag;
			int h = Raw2Image.ImageRow * Mag;
			if (w != BmpImages[0].Width || h != BmpImages[0].Height)
			{
				for (int i = 0; i < BmpImages.Length; i++)
				{
					BmpImages[i].Dispose();
					BmpImages[i] = new Bitmap(w, h, PixelFormat.Format24bppRgb);
				}
			}
			if (bGrayscaling)
			{
				PlotMax = DataMax;
				PlotMin = DataMin;
			}
			PlotMax = PlotMax > PlotMin ? PlotMax : PlotMin + 1;
			var pdata = BmpImages[position].LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
			int s = pdata.Stride;
			unsafe
			{
				byte* addr = (byte*)pdata.Scan0;
				for (int i = 0; i < h; i++)
				{
					byte* baseOffset = addr + i * s;
					for (int j = 0; j < w; j++)
					{
						uint pixeldata = imageInts[(i / Mag) * Raw2Image.ImageCol + j / Mag];
						pixeldata = pixeldata < PlotMax ? pixeldata : PlotMax;
						pixeldata = pixeldata > PlotMin ? pixeldata : PlotMin;
						byte grayscale = (byte)((pixeldata - PlotMin) * 255 / (PlotMax - PlotMin));
						byte* pxPos = baseOffset + j * 3;
						*(pxPos) = grayscale;
						*(pxPos + 1) = grayscale;
						*(pxPos + 2) = grayscale;
					}
					if (bShowModuleBorder)
					{
						*(baseOffset) = 0;
						*(baseOffset + 1) = 0xff;
						*(baseOffset + 2) = 0;
					}
				}
			}
			BmpImages[position].UnlockBits(pdata);
		}

		/// <summary>
		/// Update all bitmaps using buffered data
		/// </summary>
		public void UpdateAllBitmaps()
		{
			DataMax = 0;
			DataMin = 0xffffff; // not UInt32.MaxValue because it's 24-bit
			for (int i = 0; i < BufferInts.Length; i++)
			{
				for (int j = 0; j < BufferInts[i].Length; j++)
				{
					DataMax = BufferInts[i][j] > DataMax ? BufferInts[i][j] : DataMax;
					DataMin = BufferInts[i][j] < DataMin ? BufferInts[i][j] : DataMin;
				}
			}
			for (int i = 0; i < BmpImages.Length; i++)
				UpdateBitmap(BufferInts[i], i);
		}
	}
}
