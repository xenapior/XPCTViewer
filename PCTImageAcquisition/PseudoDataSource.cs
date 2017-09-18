using System;
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
	public class PseudoDataSource:IDisposable
	{
		public const int NumDetectorModules = 5;
		public int PixelBytes = 3;
		public int FrameHeader = 5;
		public int FrameTrailer = 105;
		public const int ImageCol = 24;
		public const int ImageRow = 16;
		public bool IsRunning;

		private string targetIP = "127.0.0.1";
		private int sendingPort = 80;
		private int targetPort = 8080;
		private IPEndPoint targetEndPoint;
		private UdpClient local;
		public volatile bool requestStopCapture;
		private const string SettingfilePath = "./capture_settings.ini";

		private Random rm=new Random();

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);
		public PseudoDataSource()
		{
			if (File.Exists(SettingfilePath))
			{
				int result = GetPrivateProfileInt("Raw data format", "pixel bytes", 0, SettingfilePath);
				if (result > 0)
					PixelBytes = result;
				result = GetPrivateProfileInt("Raw data format", "header length", 0, SettingfilePath);
				if (result > 0)
					FrameHeader = result;
				result = GetPrivateProfileInt("Raw data format", "trailer length", 0, SettingfilePath);
				if (result > 0)
					FrameTrailer = result;
				int port = GetPrivateProfileInt("Network", "detector port", 0, SettingfilePath);
				if (port > 0)
					sendingPort = port;
				port = GetPrivateProfileInt("Network", "capturer port", 0, SettingfilePath);
				if (port > 0)
					targetPort = port;
			}
			targetEndPoint=new IPEndPoint(IPAddress.Parse(targetIP), targetPort);
		}

		public void StartSendDataAsync()
		{
			if (local == null)
			{
				local = new UdpClient(sendingPort);
			}
			if (IsRunning)
				return;
			Task t=new Task(senderThread);
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
			int counter = 1;

			while (!requestStopCapture)
			{
				byte[] datagm = PseudoFrameGenerator(counter++);
				if (counter > NumDetectorModules)
					counter = 1;
				local.Send(datagm, datagm.Length, targetEndPoint);
//				Thread.Sleep(200);
			}
			requestStopCapture = false;
			IsRunning = false;
		}

		private byte[] PseudoFrameGenerator(int modId)
		{
			byte[] data = new byte[ImageCol*ImageRow*PixelBytes+FrameHeader+FrameTrailer];

			data[0] = (byte)(0xf8*(rm.Next(10)>0?1:0));	//set chance to send Invalid data
			data[1] = (byte)modId;

			for (int i = 0; i < ImageCol*ImageRow; i++)
			{
				int p = FrameHeader + i*PixelBytes;
				data[p] = 0;
				data[p + 1] = (byte)rm.Next(255);
				data[p + 2] = (byte)rm.Next(255);
			}
			return data;
		}

		public void Dispose()
		{
			local?.Close();
		}
	}
}
