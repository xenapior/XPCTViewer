﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCTImageAcquisition
{
	public class PseudoDataSource : IDisposable
	{
		public int FrameHeader = 5;
		public int FrameTrailer = 105;
		public bool IsRunning;
		public string TargetIP = "127.0.0.1";
		public int TargetPort = 8080;
		public int SendingPort = 80;
		public int OffsetPeriod=800000;
		public int FramesSent;

		private IPEndPoint targetEndPoint;
		private UdpClient local;
		private volatile bool requestStopCapture;
		private Random rm = new Random();
		private const int ImageLength = Raw2Image.ImageCol*Raw2Image.ImageRow;

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);
		public PseudoDataSource()
		{
			if (File.Exists(Resources.settingfilePath))
			{
				int result = GetPrivateProfileInt("RawFormat", "header length", 0, Resources.settingfilePath);
				if (result > 0)
					FrameHeader = result;
				result = GetPrivateProfileInt("RawFormat", "trailer length", 0, Resources.settingfilePath);
				if (result > 0)
					FrameTrailer = result;
				int port = GetPrivateProfileInt("Network", "detector port", 0, Resources.settingfilePath);
				if (port > 0)
					SendingPort = port;
				port = GetPrivateProfileInt("Network", "capturer port", 0, Resources.settingfilePath);
				if (port > 0)
					TargetPort = port;
			}
		}

		public void StartSendDataAsync()
		{
			if (local == null)
			{
				targetEndPoint = new IPEndPoint(IPAddress.Parse(TargetIP), TargetPort);
				local = new UdpClient(SendingPort);
			}
			if (IsRunning)
				return;
			Task t = new Task(senderThread);
			t.Start();
		}

		public void StopSendData()
		{
			requestStopCapture = true;
		}

		private void senderThread()
		{
			IsRunning = true;
			requestStopCapture = false;
			FramesSent = 0;
			int modIdCounter = 1;

			while (!requestStopCapture)
			{
				byte[] datagm = PseudoFrameGenerator(modIdCounter++);
				if (modIdCounter > Raw2Image.NumDetectorModules)
					modIdCounter = 1;
				local.Send(datagm, datagm.Length, targetEndPoint);
				Thread.Sleep(10);
			}
			requestStopCapture = false;
			IsRunning = false;
		}

		private byte[] PseudoFrameGenerator(int modId)
		{
			byte[] data = new byte[ImageLength * Raw2Image.PixelBytes + FrameHeader + FrameTrailer];

			data[0] = 0xf8;
			if (FramesSent++ >= 40)
				data[0] = 0;
			data[1] = (byte)modId;

			for (int i = 0; i < ImageLength; i++)
			{
				uint pxVal=(uint)rm.Next(0x100);
				int p = FrameHeader + i * Raw2Image.PixelBytes;
				data[p] = (byte)modId;
				data[p + 1] = (byte)pxVal;
				data[p + 2] = 0;
			}
			return data;
		}

		public void Dispose()
		{
			local?.Close();
		}
	}
}
