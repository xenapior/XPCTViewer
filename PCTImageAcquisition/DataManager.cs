using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCTImageAcquisition
{
	public class DataManager
	{
		public int Length;
		public uint[] Data;
		public int[] ModID;
		public int FrameCapacity
		{
			get { return _frameCapacity; }
			set
			{
				if (value > _frameCapacity)
				{
					_frameCapacity = value;
					Data = new uint[value * ImageLength];
					ModID = new int[value];
				}
			}
		}
		private const int ImageLength = Raw2Image.ImageCol * Raw2Image.ImageRow;
		private int _frameCapacity;
		private Action callbackBufferfull;

		public DataManager(int frameCapacity = 50)
		{
			_frameCapacity = frameCapacity;
			Data = new uint[frameCapacity * ImageLength];
			ModID = new int[frameCapacity];
			callbackBufferfull = () => { };
		}

		public void SetBufferfullCallback(Action cbFull)
		{
			callbackBufferfull = cbFull;
		}

		public void Add(int modId, uint[] imageInts)
		{
			if (Length >= _frameCapacity)
				Length = 0;
			ModID[Length] = modId;
			Array.Copy(imageInts, 0, Data, Length * ImageLength, ImageLength);
			Length++;
			if (Length == _frameCapacity)
				callbackBufferfull();
		}

		public void Clear()
		{
			Length = 0;
		}
	}
}
