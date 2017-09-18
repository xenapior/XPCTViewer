using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TempHexDisplay
{
	class DataManager
	{
		public const int NumDetectorModules = 5;
		public const int PixelBytes = 3;
		public const int ImageCol = 24;
		public const int ImageRow = 16;
		public const int BufferLength=100;

		public int[] Data;
		public int[] ModID;
		private int nextReadLoc;


		public int NextCaptureLoc { get; private set; }

		public int NextReadLoc
		{
			get { return nextReadLoc++; }
			private set { nextReadLoc = value; }
		}

		public DataManager()
		{
			NextCaptureLoc = 0;
		}

		public void abst()
		{
			NextCaptureLoc = 3;
		}
	}
}
