using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCTImageAcquisition
{
	/// <summary>
	/// Buffer for extracted UInt32 frame data and history peeking
	/// </summary>
	public class DataManager
	{
		public int Length;		// frame counter
		public uint[] Data;		// pixel data in uint format
		public int[] ModID;		// corresponding module IDs
		public int FrameCapacity	//buffer capacity in num of frames
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
		private readonly uint[] blackImage;  // accelerate data peeking
		private Action callbackBufferfull;	// triggers when buffer is full

		public DataManager(int frameCapacity = 1000)
		{
			_frameCapacity = frameCapacity;
			Data = new uint[frameCapacity * ImageLength];
			ModID = new int[frameCapacity];
			blackImage = new uint[ImageLength];
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
		/// <summary>
		/// Find the most recent image in the buffer
		/// </summary>
		/// <param name="modId">ID of the module</param>
		/// <returns>Content of the image data. Returns all-0 image if not found</returns>
		public uint[] PeekMostRecentImage(int modId)
		{
			int pos = Length == 0 ? 0 : Length - 1;
			while (true)
			{
				if (ModID[pos] == modId)
					break;
				pos--;
				if (pos < 0)
					pos = FrameCapacity - 1;
				if (Length == pos)  //not found over a round search
					return blackImage;
			}
			uint[] im = new uint[ImageLength];
			Array.Copy(Data, pos * ImageLength, im, 0, ImageLength);
			return im;
		}

		public uint[] PeekImageAt(int index, out int modId)
		{
			uint[] im = new uint[ImageLength];
			Array.Copy(Data, index * ImageLength, im, 0, ImageLength);
			modId = ModID[index];
			return im;
		}
	}
}
