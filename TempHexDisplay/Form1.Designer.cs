namespace XPCTViewer
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.picBox = new System.Windows.Forms.PictureBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.exitBtn = new System.Windows.Forms.Button();
			this.datapropGrp = new System.Windows.Forms.GroupBox();
			this.minLbl = new System.Windows.Forms.Label();
			this.pixelDataLbl = new System.Windows.Forms.Label();
			this.maxLbl = new System.Windows.Forms.Label();
			this.nGroupsLbl = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.updateIntevalBox = new System.Windows.Forms.TextBox();
			this.magBar = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.dispGrp = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.grayscaleMaxBox = new System.Windows.Forms.TextBox();
			this.grayscaleMinBox = new System.Windows.Forms.TextBox();
			this.showSplitline = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.isGrayscaling = new System.Windows.Forms.CheckBox();
			this.magDisp = new System.Windows.Forms.Label();
			this.frameScrl = new System.Windows.Forms.HScrollBar();
			this.updateTimer = new System.Windows.Forms.Timer(this.components);
			this.netCaptureBtn = new System.Windows.Forms.CheckBox();
			this.pseudoDataBtn = new System.Windows.Forms.CheckBox();
			this.saveasBtn = new System.Windows.Forms.Button();
			this.openfileBtn = new System.Windows.Forms.Button();
			this.acqGrp = new System.Windows.Forms.GroupBox();
			this.accumNumBox = new System.Windows.Forms.TextBox();
			this.autosaveBtn = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.status1 = new System.Windows.Forms.ToolStripStatusLabel();
			((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
			this.datapropGrp.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.magBar)).BeginInit();
			this.dispGrp.SuspendLayout();
			this.acqGrp.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// picBox
			// 
			this.picBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picBox.Location = new System.Drawing.Point(12, 12);
			this.picBox.Name = "picBox";
			this.picBox.Size = new System.Drawing.Size(962, 130);
			this.picBox.TabIndex = 0;
			this.picBox.TabStop = false;
			this.picBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
			this.picBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.displayBox_MouseMove);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "All files (*.*)|*.*";
			// 
			// exitBtn
			// 
			this.exitBtn.Location = new System.Drawing.Point(870, 364);
			this.exitBtn.Name = "exitBtn";
			this.exitBtn.Size = new System.Drawing.Size(104, 65);
			this.exitBtn.TabIndex = 6;
			this.exitBtn.Text = "退出";
			this.exitBtn.UseVisualStyleBackColor = true;
			this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
			// 
			// datapropGrp
			// 
			this.datapropGrp.Controls.Add(this.minLbl);
			this.datapropGrp.Controls.Add(this.pixelDataLbl);
			this.datapropGrp.Controls.Add(this.maxLbl);
			this.datapropGrp.Controls.Add(this.nGroupsLbl);
			this.datapropGrp.Location = new System.Drawing.Point(12, 195);
			this.datapropGrp.Name = "datapropGrp";
			this.datapropGrp.Size = new System.Drawing.Size(258, 163);
			this.datapropGrp.TabIndex = 17;
			this.datapropGrp.TabStop = false;
			this.datapropGrp.Text = "数据属性";
			// 
			// minLbl
			// 
			this.minLbl.AutoSize = true;
			this.minLbl.Location = new System.Drawing.Point(13, 77);
			this.minLbl.Name = "minLbl";
			this.minLbl.Size = new System.Drawing.Size(71, 12);
			this.minLbl.TabIndex = 18;
			this.minLbl.Text = "最小值0/0x0";
			// 
			// pixelDataLbl
			// 
			this.pixelDataLbl.AutoSize = true;
			this.pixelDataLbl.Location = new System.Drawing.Point(13, 103);
			this.pixelDataLbl.Name = "pixelDataLbl";
			this.pixelDataLbl.Size = new System.Drawing.Size(65, 12);
			this.pixelDataLbl.TabIndex = 18;
			this.pixelDataLbl.Text = "当前像素值";
			// 
			// maxLbl
			// 
			this.maxLbl.AutoSize = true;
			this.maxLbl.Location = new System.Drawing.Point(13, 51);
			this.maxLbl.Name = "maxLbl";
			this.maxLbl.Size = new System.Drawing.Size(71, 12);
			this.maxLbl.TabIndex = 17;
			this.maxLbl.Text = "最大值1/0x1";
			// 
			// nGroupsLbl
			// 
			this.nGroupsLbl.AutoSize = true;
			this.nGroupsLbl.Location = new System.Drawing.Point(13, 25);
			this.nGroupsLbl.Name = "nGroupsLbl";
			this.nGroupsLbl.Size = new System.Drawing.Size(59, 12);
			this.nGroupsLbl.TabIndex = 14;
			this.nGroupsLbl.Text = "共0帧数据";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 12);
			this.label2.TabIndex = 16;
			this.label2.Text = "刷新间隔(ms)";
			// 
			// updateIntevalBox
			// 
			this.updateIntevalBox.Location = new System.Drawing.Point(91, 20);
			this.updateIntevalBox.Name = "updateIntevalBox";
			this.updateIntevalBox.Size = new System.Drawing.Size(46, 21);
			this.updateIntevalBox.TabIndex = 15;
			this.updateIntevalBox.Text = "100";
			this.updateIntevalBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.updateIntevalBox.Validating += new System.ComponentModel.CancelEventHandler(this.updateIntevalBox_Validating);
			// 
			// magBar
			// 
			this.magBar.LargeChange = 2;
			this.magBar.Location = new System.Drawing.Point(73, 20);
			this.magBar.Maximum = 3;
			this.magBar.Name = "magBar";
			this.magBar.Size = new System.Drawing.Size(179, 45);
			this.magBar.TabIndex = 15;
			this.magBar.Value = 3;
			this.magBar.Scroll += new System.EventHandler(this.magBar_Scroll);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 12);
			this.label1.TabIndex = 16;
			this.label1.Text = "放大率";
			// 
			// dispGrp
			// 
			this.dispGrp.Controls.Add(this.label5);
			this.dispGrp.Controls.Add(this.grayscaleMaxBox);
			this.dispGrp.Controls.Add(this.grayscaleMinBox);
			this.dispGrp.Controls.Add(this.showSplitline);
			this.dispGrp.Controls.Add(this.label4);
			this.dispGrp.Controls.Add(this.isGrayscaling);
			this.dispGrp.Controls.Add(this.magDisp);
			this.dispGrp.Controls.Add(this.label1);
			this.dispGrp.Controls.Add(this.magBar);
			this.dispGrp.Location = new System.Drawing.Point(716, 195);
			this.dispGrp.Name = "dispGrp";
			this.dispGrp.Size = new System.Drawing.Size(258, 163);
			this.dispGrp.TabIndex = 18;
			this.dispGrp.TabStop = false;
			this.dispGrp.Text = "显示参数";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(172, 93);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(17, 12);
			this.label5.TabIndex = 31;
			this.label5.Text = "—";
			// 
			// grayscaleMaxBox
			// 
			this.grayscaleMaxBox.Enabled = false;
			this.grayscaleMaxBox.Location = new System.Drawing.Point(195, 90);
			this.grayscaleMaxBox.Name = "grayscaleMaxBox";
			this.grayscaleMaxBox.Size = new System.Drawing.Size(46, 21);
			this.grayscaleMaxBox.TabIndex = 30;
			this.grayscaleMaxBox.Text = "FFFFFF";
			this.grayscaleMaxBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.grayscaleMaxBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.grayscaleMaxBox_KeyPress);
			this.grayscaleMaxBox.Validating += new System.ComponentModel.CancelEventHandler(this.grayscaleMaxBox_Validating);
			// 
			// grayscaleMinBox
			// 
			this.grayscaleMinBox.Enabled = false;
			this.grayscaleMinBox.Location = new System.Drawing.Point(120, 90);
			this.grayscaleMinBox.Name = "grayscaleMinBox";
			this.grayscaleMinBox.Size = new System.Drawing.Size(46, 21);
			this.grayscaleMinBox.TabIndex = 28;
			this.grayscaleMinBox.Text = "0";
			this.grayscaleMinBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.grayscaleMinBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.grayscaleMinBox_KeyPress);
			this.grayscaleMinBox.Validating += new System.ComponentModel.CancelEventHandler(this.grayscaleMinBox_Validating);
			// 
			// showSplitline
			// 
			this.showSplitline.AutoSize = true;
			this.showSplitline.Checked = true;
			this.showSplitline.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showSplitline.Location = new System.Drawing.Point(110, 64);
			this.showSplitline.Name = "showSplitline";
			this.showSplitline.Size = new System.Drawing.Size(84, 16);
			this.showSplitline.TabIndex = 19;
			this.showSplitline.Text = "块分隔标记";
			this.showSplitline.UseVisualStyleBackColor = true;
			this.showSplitline.CheckedChanged += new System.EventHandler(this.showSplitlineChk_CheckedChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 93);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(113, 12);
			this.label4.TabIndex = 29;
			this.label4.Text = "指定灰度窗(16进制)";
			// 
			// isGrayscaling
			// 
			this.isGrayscaling.AutoSize = true;
			this.isGrayscaling.Checked = true;
			this.isGrayscaling.CheckState = System.Windows.Forms.CheckState.Checked;
			this.isGrayscaling.Location = new System.Drawing.Point(8, 64);
			this.isGrayscaling.Name = "isGrayscaling";
			this.isGrayscaling.Size = new System.Drawing.Size(96, 16);
			this.isGrayscaling.TabIndex = 14;
			this.isGrayscaling.Text = "自动灰度拉伸";
			this.isGrayscaling.UseVisualStyleBackColor = true;
			this.isGrayscaling.CheckedChanged += new System.EventHandler(this.isGrayscaling_CheckedChanged);
			// 
			// magDisp
			// 
			this.magDisp.AutoSize = true;
			this.magDisp.Location = new System.Drawing.Point(50, 26);
			this.magDisp.Name = "magDisp";
			this.magDisp.Size = new System.Drawing.Size(17, 12);
			this.magDisp.TabIndex = 17;
			this.magDisp.Text = "8x";
			// 
			// frameScrl
			// 
			this.frameScrl.Location = new System.Drawing.Point(12, 145);
			this.frameScrl.Maximum = 1023;
			this.frameScrl.Name = "frameScrl";
			this.frameScrl.Size = new System.Drawing.Size(962, 36);
			this.frameScrl.TabIndex = 19;
			this.frameScrl.Scroll += new System.Windows.Forms.ScrollEventHandler(this.frameScrl_Scroll);
			// 
			// updateTimer
			// 
			this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
			// 
			// netCaptureBtn
			// 
			this.netCaptureBtn.Appearance = System.Windows.Forms.Appearance.Button;
			this.netCaptureBtn.Location = new System.Drawing.Point(500, 364);
			this.netCaptureBtn.Name = "netCaptureBtn";
			this.netCaptureBtn.Size = new System.Drawing.Size(126, 65);
			this.netCaptureBtn.TabIndex = 22;
			this.netCaptureBtn.Text = "开始网络采集";
			this.netCaptureBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.netCaptureBtn.UseVisualStyleBackColor = true;
			this.netCaptureBtn.CheckedChanged += new System.EventHandler(this.netCaptureBtn_CheckedChanged);
			// 
			// pseudoDataBtn
			// 
			this.pseudoDataBtn.Appearance = System.Windows.Forms.Appearance.Button;
			this.pseudoDataBtn.ForeColor = System.Drawing.Color.Blue;
			this.pseudoDataBtn.Location = new System.Drawing.Point(760, 364);
			this.pseudoDataBtn.Name = "pseudoDataBtn";
			this.pseudoDataBtn.Size = new System.Drawing.Size(104, 65);
			this.pseudoDataBtn.TabIndex = 23;
			this.pseudoDataBtn.Text = "发送测试数据";
			this.pseudoDataBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.pseudoDataBtn.UseVisualStyleBackColor = true;
			this.pseudoDataBtn.CheckedChanged += new System.EventHandler(this.pseudoDataBtn_CheckedChanged);
			// 
			// saveasBtn
			// 
			this.saveasBtn.Location = new System.Drawing.Point(368, 364);
			this.saveasBtn.Name = "saveasBtn";
			this.saveasBtn.Size = new System.Drawing.Size(126, 65);
			this.saveasBtn.TabIndex = 24;
			this.saveasBtn.Text = "设定保存文件名";
			this.saveasBtn.UseVisualStyleBackColor = true;
			this.saveasBtn.Click += new System.EventHandler(this.saveasBtn_Click);
			// 
			// openfileBtn
			// 
			this.openfileBtn.Location = new System.Drawing.Point(12, 364);
			this.openfileBtn.Name = "openfileBtn";
			this.openfileBtn.Size = new System.Drawing.Size(126, 65);
			this.openfileBtn.TabIndex = 25;
			this.openfileBtn.Text = "打开文件";
			this.openfileBtn.UseVisualStyleBackColor = true;
			// 
			// acqGrp
			// 
			this.acqGrp.Controls.Add(this.accumNumBox);
			this.acqGrp.Controls.Add(this.autosaveBtn);
			this.acqGrp.Controls.Add(this.label3);
			this.acqGrp.Controls.Add(this.updateIntevalBox);
			this.acqGrp.Controls.Add(this.label2);
			this.acqGrp.Location = new System.Drawing.Point(368, 195);
			this.acqGrp.Name = "acqGrp";
			this.acqGrp.Size = new System.Drawing.Size(258, 163);
			this.acqGrp.TabIndex = 27;
			this.acqGrp.TabStop = false;
			this.acqGrp.Text = "采集参数";
			// 
			// accumNumBox
			// 
			this.accumNumBox.Location = new System.Drawing.Point(91, 44);
			this.accumNumBox.Name = "accumNumBox";
			this.accumNumBox.Size = new System.Drawing.Size(46, 21);
			this.accumNumBox.TabIndex = 23;
			this.accumNumBox.Text = "1";
			this.accumNumBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// autosaveBtn
			// 
			this.autosaveBtn.AutoSize = true;
			this.autosaveBtn.Location = new System.Drawing.Point(10, 77);
			this.autosaveBtn.Name = "autosaveBtn";
			this.autosaveBtn.Size = new System.Drawing.Size(96, 16);
			this.autosaveBtn.TabIndex = 21;
			this.autosaveBtn.Text = "自动保存数据";
			this.autosaveBtn.UseVisualStyleBackColor = true;
			this.autosaveBtn.CheckedChanged += new System.EventHandler(this.autosaveBtn_CheckedChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 51);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(77, 12);
			this.label3.TabIndex = 18;
			this.label3.Text = "累加平均帧数";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Filter = "XPCT数据文件|*.bin";
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status1});
			this.statusStrip.Location = new System.Drawing.Point(0, 437);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(987, 22);
			this.statusStrip.TabIndex = 28;
			this.statusStrip.Text = "状态";
			// 
			// status1
			// 
			this.status1.AutoSize = false;
			this.status1.Name = "status1";
			this.status1.Size = new System.Drawing.Size(200, 17);
			this.status1.Text = "就绪";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(987, 459);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.acqGrp);
			this.Controls.Add(this.openfileBtn);
			this.Controls.Add(this.saveasBtn);
			this.Controls.Add(this.pseudoDataBtn);
			this.Controls.Add(this.netCaptureBtn);
			this.Controls.Add(this.frameScrl);
			this.Controls.Add(this.dispGrp);
			this.Controls.Add(this.datapropGrp);
			this.Controls.Add(this.exitBtn);
			this.Controls.Add(this.picBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
			this.datapropGrp.ResumeLayout(false);
			this.datapropGrp.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.magBar)).EndInit();
			this.dispGrp.ResumeLayout(false);
			this.dispGrp.PerformLayout();
			this.acqGrp.ResumeLayout(false);
			this.acqGrp.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox picBox;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button exitBtn;
		private System.Windows.Forms.GroupBox datapropGrp;
		private System.Windows.Forms.TrackBar magBar;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox dispGrp;
		private System.Windows.Forms.Label magDisp;
        private System.Windows.Forms.CheckBox isGrayscaling;
		private System.Windows.Forms.HScrollBar frameScrl;
		private System.Windows.Forms.Label nGroupsLbl;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox updateIntevalBox;
		private System.Windows.Forms.Label pixelDataLbl;
		private System.Windows.Forms.CheckBox showSplitline;
		private System.Windows.Forms.Label minLbl;
		private System.Windows.Forms.Label maxLbl;
		private System.Windows.Forms.Timer updateTimer;
		private System.Windows.Forms.CheckBox netCaptureBtn;
		private System.Windows.Forms.CheckBox pseudoDataBtn;
		private System.Windows.Forms.Button saveasBtn;
		private System.Windows.Forms.Button openfileBtn;
		private System.Windows.Forms.GroupBox acqGrp;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox autosaveBtn;
		private System.Windows.Forms.TextBox accumNumBox;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox grayscaleMaxBox;
		private System.Windows.Forms.TextBox grayscaleMinBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel status1;
	}
}

