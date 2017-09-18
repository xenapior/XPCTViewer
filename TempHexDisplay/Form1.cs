using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCTImageAcquisition;

namespace XPCTViewer
{
	public partial class Form1 : Form
	{
		private DataManager dataMan = new DataManager();
		private Image2Bmp i2b = new Image2Bmp();
		private PseudoDataSource pseu;

		public Form1()
		{
			InitializeComponent();
		}

		//		private void openK12Btn_Click(object sender, EventArgs e)
		//		{
		//			if (netAcqBtn.Checked)
		//				netAcqBtn.Checked = false;
		//			var ifOk = openFileDialog1.ShowDialog();
		//			if (ifOk != DialogResult.OK)
		//				return;
		//
		//			try
		//			{
		//				u.OpenK12file(openFileDialog1.OpenFile());
		//			}
		//			catch (Exception)
		//			{
		//				MessageBox.Show("不能打开数据文件");
		//				return;
		//			}
		//			updateAllImages();
		//			nGroupsLbl.Text = string.Format("共{0}帧数据", u.nFrames);
		//			maxLbl.Text = string.Format("最大值{0}/0x{0:X}", u.dataMax);
		//			minLbl.Text = string.Format("最小值{0}/0x{0:X}", u.dataMin);
		//		}

		private void exitBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			picBox.Invalidate();
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			i2b.UpdateAllBitmaps();
			for (int i = 0; i < Raw2Image.NumDetectorModules; i++)
				e.Graphics.DrawImage(i2b.BmpImages[i], new Point(Raw2Image.ImageCol * i2b.Mag * i, 0));
		}

		private void isGrayscaling_CheckedChanged(object sender, EventArgs e)
		{
			i2b.bGrayscaling = isGrayscaling.Checked;
			picBox.Invalidate();
		}

		private void frameScrl_Scroll(object sender, ScrollEventArgs e)
		{
		}

		private void displayBox_MouseMove(object sender, MouseEventArgs e)
		{
			const int w = Raw2Image.ImageCol, h = Raw2Image.ImageRow;
			int x = e.X / i2b.Mag, y = e.Y / i2b.Mag;

			if (x >= Raw2Image.NumDetectorModules * w || y >= h)
				return;
			int modLoc = x / w;
			pixelDataLbl.Text = string.Format("第{0}组,第{1}块,({2},{3}):{4}/0x{4:X}", 0, modLoc + 1, x % w + 1, y + 1,
				i2b.BufferInts[modLoc][y * Raw2Image.ImageCol + x % w]);
		}

		private void showSplitlineChk_CheckedChanged(object sender, EventArgs e)
		{
			i2b.bShowModuleBorder = showSplitline.Checked;
			picBox.Invalidate();
		}

		private void magBar_Scroll(object sender, EventArgs e)
		{
			int[] mag = { 1, 2, 4, 8 };
			i2b.Mag = mag[magBar.Value];
			magDisp.Text = i2b.Mag + "x";
			picBox.Invalidate();
		}

		private void updateTimer_Tick(object sender, EventArgs e)
		{
			for (int i = 0; i < Raw2Image.NumDetectorModules; i++)
				i2b.FillBufferData(CaptureUtility.PeekMostRecentImage(i + 1), i);
			picBox.Invalidate();
			maxLbl.Text = string.Format("最大值{0}/0x{0:X}", i2b.DataMax);
			minLbl.Text = string.Format("最小值{0}/0x{0:X}", i2b.DataMin);

			int stat = CaptureUtility.__numInvalidFrames;
			status1.Text = stat == -1 ? "超时" : "无效帧数" + CaptureUtility.__numInvalidFrames;
			Debug.WriteLine("Timer tick");
		}

		private void netCaptureBtn_CheckedChanged(object sender, EventArgs e)
		{
			updateTimer.Interval = int.Parse(updateIntevalBox.Text);
			if (netCaptureBtn.Checked)
			{
				CaptureUtility.CaptureAndStore(dataMan);
				netCaptureBtn.Text = "停止网络采集";
				acqGrp.Enabled = false;
				updateTimer.Start();
				return;
			}
			CaptureUtility.StopCapture();
			netCaptureBtn.Text = "开始网络采集";
			updateTimer.Stop();
			acqGrp.Enabled = true;
		}

		private void pseudoDataBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (pseudoDataBtn.Checked)
			{
				if (pseu == null)
					pseu = new PseudoDataSource();
				pseu.StartSendDataAsync();
				pseudoDataBtn.Text = "停止发送数据";
				return;
			}
			pseu.StopSendData();
			pseudoDataBtn.Text = "发送测试数据";
		}

		private void saveasBtn_Click(object sender, EventArgs e)
		{

		}

		private void autosaveBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (autosaveBtn.Checked)
			{
				//				saveFileDialog1.OpenFile();
			}
		}
	}
}