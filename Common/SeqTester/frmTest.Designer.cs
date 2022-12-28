namespace TestORama
{
	partial class frmTest
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTest));
			this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
			this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnSaveAs = new System.Windows.Forms.Button();
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.chkAutoSave = new System.Windows.Forms.CheckBox();
			this.chkAutoLaunch = new System.Windows.Forms.CheckBox();
			this.chkAutoLoad = new System.Windows.Forms.CheckBox();
			this.btnReload = new System.Windows.Forms.Button();
			this.grpProcess = new System.Windows.Forms.GroupBox();
			this.cmdOK = new System.Windows.Forms.Button();
			this.grpProcess.SuspendLayout();
			this.SuspendLayout();
			// 
			// dlgOpenFile
			// 
			this.dlgOpenFile.FileName = "openFileDialog1";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(246, 20);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 20);
			this.btnBrowse.TabIndex = 63;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			this.btnBrowse.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmReport_DragDrop);
			// 
			// btnSaveAs
			// 
			this.btnSaveAs.Enabled = false;
			this.btnSaveAs.Location = new System.Drawing.Point(101, 250);
			this.btnSaveAs.Name = "btnSaveAs";
			this.btnSaveAs.Size = new System.Drawing.Size(139, 31);
			this.btnSaveAs.TabIndex = 64;
			this.btnSaveAs.Text = "Save As...";
			this.btnSaveAs.UseVisualStyleBackColor = true;
			this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
			this.btnSaveAs.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmReport_DragDrop);
			// 
			// txtFilename
			// 
			this.txtFilename.Location = new System.Drawing.Point(30, 20);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.ReadOnly = true;
			this.txtFilename.Size = new System.Drawing.Size(210, 20);
			this.txtFilename.TabIndex = 65;
			// 
			// chkAutoSave
			// 
			this.chkAutoSave.AutoSize = true;
			this.chkAutoSave.Checked = true;
			this.chkAutoSave.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAutoSave.Location = new System.Drawing.Point(101, 297);
			this.chkAutoSave.Name = "chkAutoSave";
			this.chkAutoSave.Size = new System.Drawing.Size(115, 17);
			this.chkAutoSave.TabIndex = 66;
			this.chkAutoSave.Text = "Save automatically";
			this.chkAutoSave.UseVisualStyleBackColor = true;
			this.chkAutoSave.CheckedChanged += new System.EventHandler(this.chkAutoSave_CheckedChanged);
			// 
			// chkAutoLaunch
			// 
			this.chkAutoLaunch.AutoSize = true;
			this.chkAutoLaunch.Checked = true;
			this.chkAutoLaunch.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAutoLaunch.Location = new System.Drawing.Point(101, 320);
			this.chkAutoLaunch.Name = "chkAutoLaunch";
			this.chkAutoLaunch.Size = new System.Drawing.Size(136, 17);
			this.chkAutoLaunch.TabIndex = 67;
			this.chkAutoLaunch.Text = "Launch LOR Sequecer";
			this.chkAutoLaunch.UseVisualStyleBackColor = true;
			this.chkAutoLaunch.CheckedChanged += new System.EventHandler(this.chkAutoLaunch_CheckedChanged);
			// 
			// chkAutoLoad
			// 
			this.chkAutoLoad.AutoSize = true;
			this.chkAutoLoad.Checked = true;
			this.chkAutoLoad.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAutoLoad.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.chkAutoLoad.Location = new System.Drawing.Point(30, 46);
			this.chkAutoLoad.Name = "chkAutoLoad";
			this.chkAutoLoad.Size = new System.Drawing.Size(107, 17);
			this.chkAutoLoad.TabIndex = 68;
			this.chkAutoLoad.Text = "Reload at startup";
			this.chkAutoLoad.UseVisualStyleBackColor = true;
			this.chkAutoLoad.CheckedChanged += new System.EventHandler(this.chkAutoLoad_CheckedChanged);
			// 
			// btnReload
			// 
			this.btnReload.Location = new System.Drawing.Point(166, 55);
			this.btnReload.Name = "btnReload";
			this.btnReload.Size = new System.Drawing.Size(139, 31);
			this.btnReload.TabIndex = 69;
			this.btnReload.Text = "Reload";
			this.btnReload.UseVisualStyleBackColor = true;
			this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
			// 
			// grpProcess
			// 
			this.grpProcess.Controls.Add(this.cmdOK);
			this.grpProcess.Location = new System.Drawing.Point(29, 107);
			this.grpProcess.Name = "grpProcess";
			this.grpProcess.Size = new System.Drawing.Size(337, 120);
			this.grpProcess.TabIndex = 70;
			this.grpProcess.TabStop = false;
			this.grpProcess.Text = " Process... ";
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(24, 36);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(139, 31);
			this.cmdOK.TabIndex = 70;
			this.cmdOK.Text = "Do...";
			this.cmdOK.UseVisualStyleBackColor = true;
			// 
			// frmTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 361);
			this.Controls.Add(this.grpProcess);
			this.Controls.Add(this.btnReload);
			this.Controls.Add(this.chkAutoLoad);
			this.Controls.Add(this.chkAutoLaunch);
			this.Controls.Add(this.chkAutoSave);
			this.Controls.Add(this.txtFilename);
			this.Controls.Add(this.btnSaveAs);
			this.Controls.Add(this.btnBrowse);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(400, 400);
			this.MinimumSize = new System.Drawing.Size(400, 400);
			this.Name = "frmTest";
			this.Text = "TEST-O-Rama";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmReport_FormClosing);
			this.Load += new System.EventHandler(this.frmReport_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmReport_DragDrop);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmTest_Paint);
			this.grpProcess.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.OpenFileDialog dlgOpenFile;
		private System.Windows.Forms.SaveFileDialog dlgSaveFile;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnSaveAs;
		private System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.CheckBox chkAutoSave;
		private System.Windows.Forms.CheckBox chkAutoLaunch;
		private System.Windows.Forms.CheckBox chkAutoLoad;
		private System.Windows.Forms.Button btnReload;
		private System.Windows.Forms.GroupBox grpProcess;
		private System.Windows.Forms.Button cmdOK;
	}
}

