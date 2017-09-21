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
		private FileManager file;
		private PseudoDataSource pseu;

		public Form1()
		{
			InitializeComponent();
		}

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
			if (isGrayscaling.Checked)
			{
				grayscaleMaxBox.Enabled = false;
				grayscaleMinBox.Enabled = false;
			}
			else
			{
				i2b.PlotMax = Convert.ToUInt32(grayscaleMaxBox.Text, 16);
				i2b.PlotMin = Convert.ToUInt32(grayscaleMinBox.Text, 16);
				grayscaleMaxBox.Enabled = true;
				grayscaleMinBox.Enabled = true;
			}
			picBox.Invalidate();
		}

		private void frameScrl_Scroll(object sender, ScrollEventArgs e)
		{
			if (file == null || file.NumFrames == 0)
				return;
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
				i2b.SetBufferAtPos(dataMan.PeekMostRecentImage(i + 1), i);
			picBox.Invalidate();

			maxLbl.Text = string.Format("最大值{0}/0x{0:X}", i2b.DataMax);
			minLbl.Text = string.Format("最小值{0}/0x{0:X}", i2b.DataMin);
			if (i2b.bGrayscaling)
			{
				grayscaleMaxBox.Text = $"{i2b.PlotMax:X}";
				grayscaleMinBox.Text = $"{i2b.PlotMin:X}";
			}

			int stat = CaptureUtility.__numInvalidFrames;
			status1.Text = stat == -1 ? "超时" : "无效帧数" + CaptureUtility.__numInvalidFrames;
		}

		private void netCaptureBtn_CheckedChanged(object sender, EventArgs e)
		{
			updateTimer.Interval = int.Parse(updateIntevalBox.Text);
			if (netCaptureBtn.Checked)
			{
				if (autosaveBtn.Checked)
				{
					if (File.Exists(saveFileDialog1.FileName) && MessageBox.Show("要覆盖现有文件吗？", "文件已存在", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
					{
						netCaptureBtn.Checked = false;
						return;
					}
					file?.Dispose();
					file = new FileManager(File.Create(saveFileDialog1.FileName), dataMan);
					file.BeginWrite();
				}
				openfileBtn.Enabled = false;
				acqGrp.Enabled = false;
				netCaptureBtn.Text = "停止网络采集";
				updateTimer.Interval = Convert.ToInt32(updateIntevalBox.Text);

				CaptureUtility.StartCaptureAsync(dataMan);
				updateTimer.Start();
				return;
			}
			CaptureUtility.StopCapture();
			updateTimer.Stop();
			openfileBtn.Enabled = true;
			acqGrp.Enabled = true;
			netCaptureBtn.Text = "开始网络采集";
			if (autosaveBtn.Checked && file != null)
			{
				file.EndWriting();
				file.Dispose();
				file = null;
			}
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
			if (saveFileDialog1.ShowDialog() != DialogResult.OK)
				return;
			saveasBtn.Text = @"数据保存为

" + saveFileDialog1.FileName;
			autosaveBtn.Checked = true;
		}

		private void autosaveBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (autosaveBtn.Checked && saveFileDialog1.FileName == "")
			{
				MessageBox.Show("请先设定保存文件名");
				autosaveBtn.Checked = false;
			}
		}

		private void updateIntevalBox_Validating(object sender, CancelEventArgs e)
		{
			uint newVal = Convert.ToUInt32(updateIntevalBox.Text);
			updateIntevalBox.Text = Clamp(newVal, 15, 10000).ToString();
		}
		private uint Clamp(uint val, uint min, uint max)
		{
			if (val < min)
				return min;
			if (val > max)
				return max;
			return val;
		}

		private void grayscaleMaxBox_Validating(object sender, CancelEventArgs e)
		{
			uint max;
			uint min = Convert.ToUInt32(grayscaleMinBox.Text, 16);
			try
			{
				max = Convert.ToUInt32(grayscaleMaxBox.Text, 16);
			}
			catch (FormatException)
			{
				MessageBox.Show("数字格式不正确");
				if (e != null)
					e.Cancel = true;
				return;
			}
			uint newVal = Clamp(max, min + 1, 0xffffff);
			grayscaleMaxBox.Text = $"{newVal:X}";
			i2b.PlotMax = newVal;
			picBox.Invalidate();
		}

		private void grayscaleMaxBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			// if press Enter
			if (e.KeyChar == 13)
			{
				grayscaleMaxBox_Validating(sender, null);
				e.Handled = true;
			}
		}

		private void grayscaleMinBox_Validating(object sender, CancelEventArgs e)
		{
			uint max = Convert.ToUInt32(grayscaleMaxBox.Text, 16);
			uint min;
			try
			{
				min = Convert.ToUInt32(grayscaleMinBox.Text, 16);
			}
			catch (FormatException)
			{
				MessageBox.Show("数字格式不正确");
				if (e != null)
					e.Cancel = true;
				return;
			}
			uint newVal = Clamp(min, 0, max - 1);
			grayscaleMinBox.Text = $"{newVal:X}";
			i2b.PlotMin = newVal;
			picBox.Invalidate();
		}

		private void grayscaleMinBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			// if press Enter
			if (e.KeyChar == 13)
			{
				grayscaleMinBox_Validating(sender, null);
				e.Handled = true;
			}
		}

		private void openfileBtn_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() != DialogResult.OK)
				return;
			file?.Dispose();
			file=new FileManager(File.OpenRead(openFileDialog1.FileName),dataMan);
			if (!file.BeginRead())
			{
				MessageBox.Show("文件格式不正确");
				file.Dispose();
				file = null;
				return;
			}
			int pos=file.LoadFileSegmentAtPos(0);
			for (int i = 0; i < 5; i++)
			{
				int temp;
				i2b.SetBufferAtPos(dataMan.PeekImageAt(i, out temp), i);
			}
			picBox.Invalidate();
			frameScrl.Value = 0;
			nFramesLbl.Text = $"共{file.NumFrames}帧图像:第1-5帧";
		}

	}
}