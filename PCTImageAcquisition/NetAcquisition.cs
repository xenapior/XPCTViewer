using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCTImageAcquisition
{
	public class NetAcquisition : IDisposable
	{
		public bool IsRunning;

		private int capturerPort = 8080;
		private Action<byte[]> callbackFunc;
		private IPEndPoint detector;
		private UdpClient local;
		private volatile bool requestStopCapture;

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);

		public NetAcquisition()
		{
			if (File.Exists(Resources.settingfilePath))
			{
				StringBuilder str = new StringBuilder(16);
				int port  = GetPrivateProfileInt("Network", "capturer port", 0, Resources.settingfilePath);
				if (port > 0)
					capturerPort = port;
			}
			detector = new IPEndPoint(IPAddress.Any, 0);
		}

		public void StartAcquisitionAsync(Action<byte[]> rawdataHandler)
		{
			callbackFunc = rawdataHandler;
			if (local == null)
			{
				local = new UdpClient(capturerPort);
				local.Client.ReceiveTimeout = 200;  //mininum response time ~200ms
			}
			Thread acqThread=new Thread(workerThread);
			acqThread.Priority=ThreadPriority.AboveNormal;
			acqThread.Start();
		}

		public void StopAcquistion()
		{
			requestStopCapture = true;
		}

		private void workerThread()
		{
			IsRunning = true;
			requestStopCapture = false;

			byte[] data = null;
			while (!requestStopCapture)
			{
				try
				{
					data = local.Receive(ref detector);
				}
				catch (SocketException)
				{}
				if (data != null)
				{
					callbackFunc(data);
					data = null;
				}
			}
			requestStopCapture = false;
			IsRunning = false;
		}

		public void Dispose()
		{
			local?.Close();
		}
	}
}
