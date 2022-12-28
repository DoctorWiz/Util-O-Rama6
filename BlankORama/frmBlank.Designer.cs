
namespace UtilORama4
{
	partial class frmBlank
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBlank));
			this.staStatus = new System.Windows.Forms.StatusStrip();
			this.pnlHelp = new System.Windows.Forms.ToolStripStatusLabel();
			this.pnlStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.pnlAbout = new System.Windows.Forms.ToolStripStatusLabel();
			this.btnOK = new System.Windows.Forms.Button();
			this.picAboutIcon = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.staStatus.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picAboutIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// staStatus
			// 
			this.staStatus.AllowDrop = true;
			this.staStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pnlHelp,
            this.pnlStatus,
            this.pnlAbout});
			this.staStatus.Location = new System.Drawing.Point(0, 350);
			this.staStatus.Name = "staStatus";
			this.staStatus.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.staStatus.Size = new System.Drawing.Size(560, 24);
			this.staStatus.TabIndex = 63;
			// 
			// pnlHelp
			// 
			this.pnlHelp.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.pnlHelp.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
			this.pnlHelp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
			this.pnlHelp.ForeColor = System.Drawing.SystemColors.Highlight;
			this.pnlHelp.IsLink = true;
			this.pnlHelp.Name = "pnlHelp";
			this.pnlHelp.Size = new System.Drawing.Size(45, 19);
			this.pnlHelp.Text = "Help...";
			this.pnlHelp.Click += new System.EventHandler(this.pnlHelp_Click);
			// 
			// pnlStatus
			// 
			this.pnlStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.pnlStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.pnlStatus.Name = "pnlStatus";
			this.pnlStatus.Size = new System.Drawing.Size(446, 19);
			this.pnlStatus.Spring = true;
			// 
			// pnlAbout
			// 
			this.pnlAbout.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.pnlAbout.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
			this.pnlAbout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
			this.pnlAbout.ForeColor = System.Drawing.SystemColors.Highlight;
			this.pnlAbout.Name = "pnlAbout";
			this.pnlAbout.Size = new System.Drawing.Size(52, 19);
			this.pnlAbout.Text = "About...";
			this.pnlAbout.Click += new System.EventHandler(this.pnlAbout_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(224, 306);
			this.btnOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(88, 27);
			this.btnOK.TabIndex = 67;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// picAboutIcon
			// 
			this.picAboutIcon.Image = ((System.Drawing.Image)(resources.GetObject("picAboutIcon.Image")));
			this.picAboutIcon.Location = new System.Drawing.Point(398, 162);
			this.picAboutIcon.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.picAboutIcon.Name = "picAboutIcon";
			this.picAboutIcon.Size = new System.Drawing.Size(128, 128);
			this.picAboutIcon.TabIndex = 68;
			this.picAboutIcon.TabStop = false;
			this.picAboutIcon.Visible = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(90, 37);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(283, 15);
			this.label1.TabIndex = 69;
			this.label1.Text = "This is a blank stub for future Util-O-Rama programs";
			// 
			// frmBlank
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(560, 374);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.staStatus);
			this.Controls.Add(this.picAboutIcon);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.MaximizeBox = false;
			this.Name = "frmBlank";
			this.Text = "Util-O-Rama Blank";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBlank_FormClosing);
			this.Load += new System.EventHandler(this.frmBlank_Load);
			this.staStatus.ResumeLayout(false);
			this.staStatus.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picAboutIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip staStatus;
		private System.Windows.Forms.ToolStripStatusLabel pnlHelp;
		private System.Windows.Forms.ToolStripStatusLabel pnlStatus;
		private System.Windows.Forms.ToolStripStatusLabel pnlAbout;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.PictureBox picAboutIcon;
		private System.Windows.Forms.Label label1;
	}
}

