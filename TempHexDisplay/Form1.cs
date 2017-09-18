using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace TempHexDisplay
{
	public partial class Form1 : Form
	{
		private ImageUtility u = new ImageUtility();
		private PseudoDataSource pseudor;

		public Form1()
		{
			InitializeComponent();
		}

		private void openK12Btn_Click(object sender, EventArgs e)
		{
			if (netAcqBtn.Checked)
				netAcqBtn.Checked = false;
			var ifOk = openFileDialog1.ShowDialog();
			if (ifOk != DialogResult.OK)
				return;

			try
			{
				u.OpenK12file(openFileDialog1.OpenFile());
			}
			catch (Exception)
			{
				MessageBox.Show("不能打开数据文件");
				return;
			}
			updateImage();
			nGroupsLbl.Text = string.Format("共{0}帧数据", u.nFrames);
			maxLbl.Text = string.Format("最大值{0}/0x{0:X}", u.dataMax);
			minLbl.Text = string.Format("最小值{0}/0x{0:X}", u.dataMin);
		}

		private void exitBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			updateImage();
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawImage(u.image, new Point(0, 0));
		}

		private void updateImage()
		{
			u.UpdateImage();
			displayBox.Invalidate();
		}

		private void isGrayscaling_CheckedChanged(object sender, EventArgs e)
		{
			u.bGrayscaling = isGrayscaling.Checked;
			updateImage();
		}

		private void frameScrl_Scroll(object sender, ScrollEventArgs e)
		{
			int nGroups = u.nFrames / 5;
			int oldGroup = e.OldValue * nGroups / 1024;
			int newGroup = e.NewValue * nGroups / 1024;
			if (newGroup != oldGroup)
			{
				u.currentGroup = newGroup;
				this.Text = String.Format("第{0}-{1}帧数据", newGroup * 5 + 1, (newGroup + 1) * 5);
				updateImage();
			}
		}

		private void displayBox_MouseMove(object sender, MouseEventArgs e)
		{
			int x = e.X / u.viewMag, y = e.Y / u.viewMag, w = ImageUtility.ImageWidth, h = ImageUtility.ImageHeight;
			if (x >= 5 * w || y >= h)
				return;
			int subscript = (u.currentGroup * 5 + x / w) * w * h + y * w + x % w;
			pixelDataLbl.Text = string.Format("第{0}组,第{1}块,({2},{3}):{4}/0x{4:X}", u.currentGroup + 1, x / w + 1, x % w + 1, y + 1,
				u.data[subscript]);
		}

		private void showSplitlineChk_CheckedChanged(object sender, EventArgs e)
		{
			u.bShowSplitter = showSplitline.Checked;
			updateImage();
		}

		private void magBar_Scroll(object sender, EventArgs e)
		{
			int[] mag = { 1, 2, 4, 8 };
			u.viewMag = mag[magBar.Value];
			magDisp.Text = u.viewMag + "x";

			updateImage();
		}

		private void captureTimer_Tick(object sender, EventArgs e)
		{
			u.data = CaptureUtility.CaptureOneFullview(50);
			updateImage();
			maxLbl.Text = string.Format("最大值{0}/0x{0:X}", u.dataMax);
			minLbl.Text = string.Format("最小值{0}/0x{0:X}", u.dataMin);
			int stat = CaptureUtility.__numInvalidFrames;
			status1.Text = stat==-1?"超时":"无效帧数"+CaptureUtility.__numInvalidFrames;
		}

		private void netAcqBtn_CheckedChanged(object sender, EventArgs e)
		{
			autoUpdateTimer.Interval = int.Parse(autoUpdateIntevalBox.Text);
			if (netAcqBtn.Checked)
			{
				netAcqBtn.Text = "停止网络采集";
				u.bNetCaptureMode = true;
				acqGrp.Enabled = false;
				autoUpdateTimer.Start();
				return;
			}
			netAcqBtn.Text = "开始网络采集";
			u.bNetCaptureMode = false;
			autoUpdateTimer.Stop();
			acqGrp.Enabled = true;
		}

		private void pseudoDataBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (pseudoDataBtn.Checked)
			{
				if (pseudor == null)
					pseudor = new PseudoDataSource();
				pseudor.StartSendDataAsync();
				pseudoDataBtn.Text = "停止发送数据";
				return;
			}
			pseudor.StopSendData();
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