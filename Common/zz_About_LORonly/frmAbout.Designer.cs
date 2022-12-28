namespace UtilORama4
{
	partial class frmAbout
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
			this.labelVersion = new System.Windows.Forms.Label();
			this.labelCopyright = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.labelProductName = new System.Windows.Forms.Label();
			this.textBoxDescription = new System.Windows.Forms.TextBox();
			this.labelCompanyName = new System.Windows.Forms.LinkLabel();
			this.labelAuthorName = new System.Windows.Forms.LinkLabel();
			this.labelAnd = new System.Windows.Forms.Label();
			this.labelFreeware = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.labelGPL = new System.Windows.Forms.LinkLabel();
			this.label4 = new System.Windows.Forms.Label();
			this.labelBugs = new System.Windows.Forms.LinkLabel();
			this.picGPL = new System.Windows.Forms.PictureBox();
			this.picLOR = new System.Windows.Forms.PictureBox();
			this.labelSuite = new System.Windows.Forms.Label();
			this.labeLOR4Admin = new System.Windows.Forms.LinkLabel();
			this.label5 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblDisclaimer = new System.Windows.Forms.Label();
			this.lblAlpha = new System.Windows.Forms.Label();
			this.lblBeta = new System.Windows.Forms.Label();
			this.picWizard = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picGPL)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picLOR)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picWizard)).BeginInit();
			this.SuspendLayout();
			// 
			// labelVersion
			// 
			this.labelVersion.AutoSize = true;
			this.labelVersion.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelVersion.Location = new System.Drawing.Point(150, 50);
			this.labelVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelVersion.MaximumSize = new System.Drawing.Size(0, 17);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(57, 17);
			this.labelVersion.TabIndex = 25;
			this.labelVersion.Text = "Version";
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelCopyright
			// 
			this.labelCopyright.AutoSize = true;
			this.labelCopyright.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCopyright.Location = new System.Drawing.Point(150, 80);
			this.labelCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelCopyright.Name = "labelCopyright";
			this.labelCopyright.Size = new System.Drawing.Size(163, 19);
			this.labelCopyright.TabIndex = 28;
			this.labelCopyright.Text = "Copyright © 2021+... by";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new System.Drawing.Point(348, 318);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 31;
			this.okButton.Text = "&OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click_1);
			// 
			// picIcon
			// 
			this.picIcon.ErrorImage = null;
			this.picIcon.Image = ((System.Drawing.Image)(resources.GetObject("picIcon.Image")));
			this.picIcon.InitialImage = null;
			this.picIcon.Location = new System.Drawing.Point(12, 12);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(128, 128);
			this.picIcon.TabIndex = 32;
			this.picIcon.TabStop = false;
			this.picIcon.WaitOnLoad = true;
			this.picIcon.Click += new System.EventHandler(this.picIcon_Click);
			this.picIcon.MouseEnter += new System.EventHandler(this.picIcon_MouseEnter);
			this.picIcon.MouseLeave += new System.EventHandler(this.picIcon_MouseLeave);
			// 
			// labelProductName
			// 
			this.labelProductName.AutoSize = true;
			this.labelProductName.Font = new System.Drawing.Font("Calibri", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelProductName.Location = new System.Drawing.Point(149, 12);
			this.labelProductName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelProductName.Name = "labelProductName";
			this.labelProductName.Size = new System.Drawing.Size(144, 26);
			this.labelProductName.TabIndex = 27;
			this.labelProductName.Text = "Program Name";
			this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxDescription
			// 
			this.textBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxDescription.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxDescription.Location = new System.Drawing.Point(152, 88);
			this.textBoxDescription.Multiline = true;
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.ReadOnly = true;
			this.textBoxDescription.Size = new System.Drawing.Size(207, 57);
			this.textBoxDescription.TabIndex = 33;
			this.textBoxDescription.Visible = false;
			// 
			// labelCompanyName
			// 
			this.labelCompanyName.AutoSize = true;
			this.labelCompanyName.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCompanyName.Location = new System.Drawing.Point(280, 102);
			this.labelCompanyName.Name = "labelCompanyName";
			this.labelCompanyName.Size = new System.Drawing.Size(142, 19);
			this.labelCompanyName.TabIndex = 34;
			this.labelCompanyName.TabStop = true;
			this.labelCompanyName.Text = "W⚡zlights Software";
			this.labelCompanyName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelCompanyName_LinkClicked);
			// 
			// labelAuthorName
			// 
			this.labelAuthorName.AutoEllipsis = true;
			this.labelAuthorName.AutoSize = true;
			this.labelAuthorName.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAuthorName.Location = new System.Drawing.Point(150, 102);
			this.labelAuthorName.Name = "labelAuthorName";
			this.labelAuthorName.Size = new System.Drawing.Size(101, 19);
			this.labelAuthorName.TabIndex = 35;
			this.labelAuthorName.TabStop = true;
			this.labelAuthorName.Text = "Doctor Wizard";
			this.labelAuthorName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelAuthorName_LinkClicked);
			// 
			// labelAnd
			// 
			this.labelAnd.AutoSize = true;
			this.labelAnd.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAnd.Location = new System.Drawing.Point(248, 102);
			this.labelAnd.Name = "labelAnd";
			this.labelAnd.Size = new System.Drawing.Size(33, 19);
			this.labelAnd.TabIndex = 36;
			this.labelAnd.Text = "and";
			// 
			// labelFreeware
			// 
			this.labelFreeware.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelFreeware.Location = new System.Drawing.Point(12, 162);
			this.labelFreeware.Name = "labelFreeware";
			this.labelFreeware.Size = new System.Drawing.Size(380, 17);
			this.labelFreeware.TabIndex = 37;
			this.labelFreeware.Text = " is released as FREEWARE for the benefit";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(12, 210);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(314, 15);
			this.label3.TabIndex = 40;
			this.label3.Text = "Source Code is available under a General Public License";
			// 
			// labelGPL
			// 
			this.labelGPL.AutoSize = true;
			this.labelGPL.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelGPL.Location = new System.Drawing.Point(332, 210);
			this.labelGPL.Name = "labelGPL";
			this.labelGPL.Size = new System.Drawing.Size(35, 15);
			this.labelGPL.TabIndex = 41;
			this.labelGPL.TabStop = true;
			this.labelGPL.Text = "(GPL)";
			this.labelGPL.Visible = false;
			this.labelGPL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelGPL_LinkClicked);
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(12, 233);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(398, 39);
			this.label4.TabIndex = 42;
			this.label4.Text = "For more information, or to submit bug reports, ideas, suggestions, cool sequence" +
		"s, or good dirty jokes, please contact Doctor Wizard at:";
			this.label4.Click += new System.EventHandler(this.label4_Click);
			// 
			// labelBugs
			// 
			this.labelBugs.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBugs.Location = new System.Drawing.Point(23, 262);
			this.labelBugs.Name = "labelBugs";
			this.labelBugs.Size = new System.Drawing.Size(184, 16);
			this.labelBugs.TabIndex = 43;
			this.labelBugs.TabStop = true;
			this.labelBugs.Text = "dev.utilorama@wizster.com";
			this.labelBugs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelBugs_LinkClicked);
			// 
			// picGPL
			// 
			this.picGPL.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picGPL.BackgroundImage")));
			this.picGPL.Location = new System.Drawing.Point(332, 204);
			this.picGPL.Name = "picGPL";
			this.picGPL.Size = new System.Drawing.Size(64, 26);
			this.picGPL.TabIndex = 44;
			this.picGPL.TabStop = false;
			this.picGPL.Click += new System.EventHandler(this.picGPL_Click);
			this.picGPL.MouseEnter += new System.EventHandler(this.picGPL_MouseEnter);
			this.picGPL.MouseLeave += new System.EventHandler(this.picGPL_MouseLeave);
			// 
			// picLOR
			// 
			this.picLOR.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picLOR.BackgroundImage")));
			this.picLOR.Location = new System.Drawing.Point(58, 184);
			this.picLOR.Name = "picLOR";
			this.picLOR.Size = new System.Drawing.Size(90, 19);
			this.picLOR.TabIndex = 45;
			this.picLOR.TabStop = false;
			this.picLOR.Click += new System.EventHandler(this.picLOR_Click);
			this.picLOR.MouseEnter += new System.EventHandler(this.picLOR_MouseEnter);
			this.picLOR.MouseLeave += new System.EventHandler(this.picLOR_MouseLeave);
			// 
			// labelSuite
			// 
			this.labelSuite.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSuite.Location = new System.Drawing.Point(310, 10);
			this.labelSuite.Name = "labelSuite";
			this.labelSuite.Size = new System.Drawing.Size(113, 35);
			this.labelSuite.TabIndex = 46;
			this.labelSuite.Text = " is a member of the";
			// 
			// labeLOR4Admin
			// 
			this.labeLOR4Admin.AutoEllipsis = true;
			this.labeLOR4Admin.AutoSize = true;
			this.labeLOR4Admin.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labeLOR4Admin.Location = new System.Drawing.Point(309, 39);
			this.labeLOR4Admin.Name = "labeLOR4Admin";
			this.labeLOR4Admin.Size = new System.Drawing.Size(70, 14);
			this.labeLOR4Admin.TabIndex = 47;
			this.labeLOR4Admin.TabStop = true;
			this.labeLOR4Admin.Text = "Util-O-Rama";
			this.labeLOR4Admin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labeLOR4Admin_LinkClicked);
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(376, 39);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(35, 14);
			this.label5.TabIndex = 48;
			this.label5.Text = "Suite";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 181);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(380, 17);
			this.label1.TabIndex = 49;
			this.label1.Text = "of the                         community.";
			// 
			// lblDisclaimer
			// 
			this.lblDisclaimer.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDisclaimer.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.lblDisclaimer.Location = new System.Drawing.Point(9, 292);
			this.lblDisclaimer.Name = "lblDisclaimer";
			this.lblDisclaimer.Size = new System.Drawing.Size(333, 57);
			this.lblDisclaimer.TabIndex = 50;
			this.lblDisclaimer.Text = resources.GetString("lblDisclaimer.Text");
			// 
			// lblAlpha
			// 
			this.lblAlpha.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAlpha.ForeColor = System.Drawing.Color.Chocolate;
			this.lblAlpha.Location = new System.Drawing.Point(9, 360);
			this.lblAlpha.Name = "lblAlpha";
			this.lblAlpha.Size = new System.Drawing.Size(333, 82);
			this.lblAlpha.TabIndex = 51;
			this.lblAlpha.Text = resources.GetString("lblAlpha.Text");
			this.lblAlpha.Visible = false;
			// 
			// lblBeta
			// 
			this.lblBeta.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblBeta.ForeColor = System.Drawing.Color.Purple;
			this.lblBeta.Location = new System.Drawing.Point(9, 455);
			this.lblBeta.Name = "lblBeta";
			this.lblBeta.Size = new System.Drawing.Size(333, 72);
			this.lblBeta.TabIndex = 52;
			this.lblBeta.Text = resources.GetString("lblBeta.Text");
			this.lblBeta.Visible = false;
			// 
			// picWizard
			// 
			this.picWizard.Image = ((System.Drawing.Image)(resources.GetObject("picWizard.Image")));
			this.picWizard.Location = new System.Drawing.Point(146, 120);
			this.picWizard.Name = "picWizard";
			this.picWizard.Size = new System.Drawing.Size(48, 48);
			this.picWizard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picWizard.TabIndex = 53;
			this.picWizard.TabStop = false;
			// 
			// frmAbout
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(435, 353);
			this.Controls.Add(this.picWizard);
			this.Controls.Add(this.lblBeta);
			this.Controls.Add(this.lblAlpha);
			this.Controls.Add(this.lblDisclaimer);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.labeLOR4Admin);
			this.Controls.Add(this.labelSuite);
			this.Controls.Add(this.picLOR);
			this.Controls.Add(this.picGPL);
			this.Controls.Add(this.labelBugs);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.labelGPL);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.labelAnd);
			this.Controls.Add(this.labelAuthorName);
			this.Controls.Add(this.labelCompanyName);
			this.Controls.Add(this.picIcon);
			this.Controls.Add(this.labelProductName);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.labelCopyright);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.textBoxDescription);
			this.Controls.Add(this.labelFreeware);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.Padding = new System.Windows.Forms.Padding(9);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About Program";
			this.Load += new System.EventHandler(this.frmAbout_Load);
			this.Shown += new System.EventHandler(this.frmAbout_Shown);
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picGPL)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picLOR)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picWizard)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Label labelCopyright;
		private System.Windows.Forms.Button okButton;
		public System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.Label labelProductName;
		private System.Windows.Forms.TextBox textBoxDescription;
		private System.Windows.Forms.LinkLabel labelCompanyName;
		private System.Windows.Forms.LinkLabel labelAuthorName;
		private System.Windows.Forms.Label labelAnd;
		private System.Windows.Forms.Label labelFreeware;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel labelGPL;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.LinkLabel labelBugs;
		private System.Windows.Forms.PictureBox picGPL;
		private System.Windows.Forms.PictureBox picLOR;
		private System.Windows.Forms.Label labelSuite;
		private System.Windows.Forms.LinkLabel labeLOR4Admin;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblDisclaimer;
		private System.Windows.Forms.Label lblAlpha;
		private System.Windows.Forms.Label lblBeta;
		private System.Windows.Forms.PictureBox picWizard;
	}
}
