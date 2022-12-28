namespace RecentlyUsedTest
{
	partial class frmTester
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTester));
			this.mnuStrip = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileNew = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileRecent = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFileSave = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFileClose = new System.Windows.Forms.ToolStripMenuItem();
			this.dlgFileOpen = new System.Windows.Forms.OpenFileDialog();
			this.dlgFileSave = new System.Windows.Forms.SaveFileDialog();
			this.picImage = new System.Windows.Forms.PictureBox();
			this.mnuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
			this.SuspendLayout();
			// 
			// mnuStrip
			// 
			this.mnuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
						this.mnuFile});
			this.mnuStrip.Location = new System.Drawing.Point(0, 0);
			this.mnuStrip.Name = "mnuStrip";
			this.mnuStrip.Size = new System.Drawing.Size(638, 24);
			this.mnuStrip.TabIndex = 0;
			this.mnuStrip.Text = "menuStrip1";
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
						this.mnuFileNew,
						this.toolStripSeparator1,
						this.mnuFileOpen,
						this.mnuFileRecent,
						this.toolStripSeparator2,
						this.mnuFileSave,
						this.mnuFileSaveAs,
						this.toolStripSeparator3,
						this.mnuFileClose});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(37, 20);
			this.mnuFile.Text = "&File";
			this.mnuFile.DropDownOpening += new System.EventHandler(this.mnuFile_DropDownOpening);
			this.mnuFile.Click += new System.EventHandler(this.mnuFile_Click);
			// 
			// mnuFileNew
			// 
			this.mnuFileNew.Enabled = false;
			this.mnuFileNew.Name = "mnuFileNew";
			this.mnuFileNew.Size = new System.Drawing.Size(180, 22);
			this.mnuFileNew.Text = "&New";
			this.mnuFileNew.Click += new System.EventHandler(this.mnuFileNew_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
			// 
			// mnuFileOpen
			// 
			this.mnuFileOpen.Name = "mnuFileOpen";
			this.mnuFileOpen.Size = new System.Drawing.Size(180, 22);
			this.mnuFileOpen.Text = "&Open...";
			this.mnuFileOpen.Click += new System.EventHandler(this.mnuFileOpen_Click);
			// 
			// mnuFileRecent
			// 
			this.mnuFileRecent.Enabled = false;
			this.mnuFileRecent.Name = "mnuFileRecent";
			this.mnuFileRecent.Size = new System.Drawing.Size(180, 22);
			this.mnuFileRecent.Text = "Open &Recent";
			this.mnuFileRecent.Click += new System.EventHandler(this.mnuFileRecent_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
			// 
			// mnuFileSave
			// 
			this.mnuFileSave.Enabled = false;
			this.mnuFileSave.Name = "mnuFileSave";
			this.mnuFileSave.Size = new System.Drawing.Size(180, 22);
			this.mnuFileSave.Text = "&Save";
			this.mnuFileSave.Click += new System.EventHandler(this.mnuFileSave_Click);
			// 
			// mnuFileSaveAs
			// 
			this.mnuFileSaveAs.Enabled = false;
			this.mnuFileSaveAs.Name = "mnuFileSaveAs";
			this.mnuFileSaveAs.Size = new System.Drawing.Size(180, 22);
			this.mnuFileSaveAs.Text = "Save &As...";
			this.mnuFileSaveAs.Click += new System.EventHandler(this.mnuFileSaveAs_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
			// 
			// mnuFileClose
			// 
			this.mnuFileClose.Name = "mnuFileClose";
			this.mnuFileClose.Size = new System.Drawing.Size(180, 22);
			this.mnuFileClose.Text = "&Close";
			this.mnuFileClose.Click += new System.EventHandler(this.mnuFileClose_Click);
			// 
			// dlgFileOpen
			// 
			this.dlgFileOpen.FileName = "openFileDialog1";
			// 
			// picImage
			// 
			this.picImage.Location = new System.Drawing.Point(0, 27);
			this.picImage.Name = "picImage";
			this.picImage.Size = new System.Drawing.Size(640, 480);
			this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picImage.TabIndex = 1;
			this.picImage.TabStop = false;
			// 
			// frmTester
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(638, 504);
			this.Controls.Add(this.picImage);
			this.Controls.Add(this.mnuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.mnuStrip;
			this.Name = "frmTester";
			this.Text = "Most Recently Used (MRU) Tester";
			this.mnuStrip.ResumeLayout(false);
			this.mnuStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MenuStrip mnuStrip;
		private ToolStripMenuItem mnuFile;
		private ToolStripMenuItem mnuFileNew;
		private ToolStripMenuItem mnuFileOpen;
		private ToolStripMenuItem mnuFileRecent;
		private ToolStripMenuItem mnuFileSave;
		private ToolStripMenuItem mnuFileSaveAs;
		private ToolStripMenuItem mnuFileClose;
		private OpenFileDialog dlgFileOpen;
		private SaveFileDialog dlgFileSave;
		private PictureBox picImage;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripSeparator toolStripSeparator3;
	}
}