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
			this.lblFreeware = new System.Windows.Forms.Label();
			this.lblSourceCode = new System.Windows.Forms.Label();
			this.labelGPL = new System.Windows.Forms.LinkLabel();
			this.lblInfo = new System.Windows.Forms.Label();
			this.lblEmail = new System.Windows.Forms.LinkLabel();
			this.picGPL = new System.Windows.Forms.PictureBox();
			this.picxLights = new System.Windows.Forms.PictureBox();
			this.labelSuite = new System.Windows.Forms.Label();
			this.labelUtils = new System.Windows.Forms.LinkLabel();
			this.lblSuite = new System.Windows.Forms.Label();
			this.lblCommunity = new System.Windows.Forms.Label();
			this.lblDisclaimer = new System.Windows.Forms.Label();
			this.picLOR = new System.Windows.Forms.PictureBox();
			this.lblAlpha = new System.Windows.Forms.Label();
			this.lblBeta = new System.Windows.Forms.Label();
			this.picDrWiz = new System.Windows.Forms.PictureBox();
			this.lblThanks = new System.Windows.Forms.Label();
			this.lblGitHub = new System.Windows.Forms.Label();
			this.lblGitIssues = new System.Windows.Forms.LinkLabel();
			this.linkCharity = new System.Windows.Forms.LinkLabel();
			this.lblProgram = new System.Windows.Forms.Label();
			this.lblCompiled = new System.Windows.Forms.Label();
			this.lblEXEfile = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picGPL)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picxLights)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picLOR)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picDrWiz)).BeginInit();
			this.SuspendLayout();
			// 
			// labelVersion
			// 
			this.labelVersion.AutoSize = true;
			this.labelVersion.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.labelVersion.Location = new System.Drawing.Point(175, 54);
			this.labelVersion.Margin = new System.Windows.Forms.Padding(7, 0, 4, 0);
			this.labelVersion.MaximumSize = new System.Drawing.Size(0, 20);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(57, 19);
			this.labelVersion.TabIndex = 25;
			this.labelVersion.Text = "Version";
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelCopyright
			// 
			this.labelCopyright.AutoSize = true;
			this.labelCopyright.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.labelCopyright.Location = new System.Drawing.Point(175, 98);
			this.labelCopyright.Margin = new System.Windows.Forms.Padding(7, 0, 4, 0);
			this.labelCopyright.Name = "labelCopyright";
			this.labelCopyright.Size = new System.Drawing.Size(151, 19);
			this.labelCopyright.TabIndex = 28;
			this.labelCopyright.Text = "Copyright © 2022+ by";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new System.Drawing.Point(391, 368);
			this.okButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(88, 27);
			this.okButton.TabIndex = 31;
			this.okButton.Text = "&OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click_1);
			// 
			// picIcon
			// 
			this.picIcon.ErrorImage = null;
			this.picIcon.Image = ((System.Drawing.Image)(resources.GetObject("picIcon.Image")));
			this.picIcon.InitialImage = null;
			this.picIcon.Location = new System.Drawing.Point(14, 14);
			this.picIcon.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(128, 128);
			this.picIcon.TabIndex = 32;
			this.picIcon.TabStop = false;
			this.picIcon.Click += new System.EventHandler(this.picIcon_Click);
			this.picIcon.MouseEnter += new System.EventHandler(this.picIcon_MouseEnter);
			this.picIcon.MouseLeave += new System.EventHandler(this.picIcon_MouseLeave);
			// 
			// labelProductName
			// 
			this.labelProductName.AutoSize = true;
			this.labelProductName.Font = new System.Drawing.Font("Calibri", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
			this.labelProductName.Location = new System.Drawing.Point(174, 14);
			this.labelProductName.Margin = new System.Windows.Forms.Padding(7, 0, 4, 0);
			this.labelProductName.Name = "labelProductName";
			this.labelProductName.Size = new System.Drawing.Size(144, 26);
			this.labelProductName.TabIndex = 27;
			this.labelProductName.Text = "Program Name";
			this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxDescription
			// 
			this.textBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxDescription.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.textBoxDescription.Location = new System.Drawing.Point(177, 102);
			this.textBoxDescription.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.textBoxDescription.Multiline = true;
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.ReadOnly = true;
			this.textBoxDescription.Size = new System.Drawing.Size(241, 66);
			this.textBoxDescription.TabIndex = 33;
			this.textBoxDescription.Visible = false;
			// 
			// labelCompanyName
			// 
			this.labelCompanyName.AutoSize = true;
			this.labelCompanyName.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.labelCompanyName.Location = new System.Drawing.Point(300, 118);
			this.labelCompanyName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
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
			this.labelAuthorName.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.labelAuthorName.Location = new System.Drawing.Point(175, 118);
			this.labelAuthorName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
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
			this.labelAnd.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.labelAnd.Location = new System.Drawing.Point(272, 118);
			this.labelAnd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labelAnd.Name = "labelAnd";
			this.labelAnd.Size = new System.Drawing.Size(33, 19);
			this.labelAnd.TabIndex = 36;
			this.labelAnd.Text = "and";
			// 
			// lblFreeware
			// 
			this.lblFreeware.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lblFreeware.Location = new System.Drawing.Point(135, 189);
			this.lblFreeware.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblFreeware.Name = "lblFreeware";
			this.lblFreeware.Size = new System.Drawing.Size(484, 20);
			this.lblFreeware.TabIndex = 37;
			this.lblFreeware.Text = "is released as Freeware / Charityware for the";
			// 
			// lblSourceCode
			// 
			this.lblSourceCode.AutoSize = true;
			this.lblSourceCode.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lblSourceCode.Location = new System.Drawing.Point(14, 242);
			this.lblSourceCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblSourceCode.Name = "lblSourceCode";
			this.lblSourceCode.Size = new System.Drawing.Size(314, 15);
			this.lblSourceCode.TabIndex = 40;
			this.lblSourceCode.Text = "Source Code is available under a General Public License";
			// 
			// labelGPL
			// 
			this.labelGPL.AutoSize = true;
			this.labelGPL.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.labelGPL.Location = new System.Drawing.Point(375, 242);
			this.labelGPL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labelGPL.Name = "labelGPL";
			this.labelGPL.Size = new System.Drawing.Size(35, 15);
			this.labelGPL.TabIndex = 41;
			this.labelGPL.TabStop = true;
			this.labelGPL.Text = "(GPL)";
			this.labelGPL.Visible = false;
			this.labelGPL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelGPL_LinkClicked);
			// 
			// lblInfo
			// 
			this.lblInfo.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lblInfo.Location = new System.Drawing.Point(14, 269);
			this.lblInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(464, 45);
			this.lblInfo.TabIndex = 42;
			this.lblInfo.Text = "For more information, or to submit bug reports, ideas, suggestions, cool sequence" +
		"s, or good dirty jokes, please contact Doctor Wizard at:";
			this.lblInfo.Click += new System.EventHandler(this.label4_Click);
			// 
			// lblEmail
			// 
			this.lblEmail.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
			this.lblEmail.Location = new System.Drawing.Point(27, 298);
			this.lblEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblEmail.Name = "lblEmail";
			this.lblEmail.Size = new System.Drawing.Size(215, 18);
			this.lblEmail.TabIndex = 43;
			this.lblEmail.TabStop = true;
			this.lblEmail.Text = "dev.utilorama@wizlights.com";
			this.lblEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblEmail_LinkClicked);
			// 
			// picGPL
			// 
			this.picGPL.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picGPL.BackgroundImage")));
			this.picGPL.Location = new System.Drawing.Point(347, 235);
			this.picGPL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.picGPL.Name = "picGPL";
			this.picGPL.Size = new System.Drawing.Size(64, 26);
			this.picGPL.TabIndex = 44;
			this.picGPL.TabStop = false;
			this.picGPL.Click += new System.EventHandler(this.picGPL_Click);
			this.picGPL.MouseEnter += new System.EventHandler(this.picGPL_MouseEnter);
			this.picGPL.MouseLeave += new System.EventHandler(this.picGPL_MouseLeave);
			// 
			// picxLights
			// 
			this.picxLights.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picxLights.BackgroundImage")));
			this.picxLights.InitialImage = ((System.Drawing.Image)(resources.GetObject("picxLights.InitialImage")));
			this.picxLights.Location = new System.Drawing.Point(238, 209);
			this.picxLights.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.picxLights.Name = "picxLights";
			this.picxLights.Size = new System.Drawing.Size(85, 26);
			this.picxLights.TabIndex = 45;
			this.picxLights.TabStop = false;
			this.picxLights.Click += new System.EventHandler(this.picxLights_Click);
			this.picxLights.MouseEnter += new System.EventHandler(this.picLOR_MouseEnter);
			this.picxLights.MouseLeave += new System.EventHandler(this.picLOR_MouseLeave);
			// 
			// labelSuite
			// 
			this.labelSuite.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
			this.labelSuite.Location = new System.Drawing.Point(362, 14);
			this.labelSuite.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labelSuite.Name = "labelSuite";
			this.labelSuite.Size = new System.Drawing.Size(132, 40);
			this.labelSuite.TabIndex = 46;
			this.labelSuite.Text = " is a member of the";
			// 
			// labelUtils
			// 
			this.labelUtils.AutoEllipsis = true;
			this.labelUtils.AutoSize = true;
			this.labelUtils.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
			this.labelUtils.Location = new System.Drawing.Point(362, 43);
			this.labelUtils.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labelUtils.Name = "labelUtils";
			this.labelUtils.Size = new System.Drawing.Size(70, 14);
			this.labelUtils.TabIndex = 47;
			this.labelUtils.TabStop = true;
			this.labelUtils.Text = "Util-O-Rama";
			this.labelUtils.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelUtils.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelUtils_LinkClicked);
			// 
			// lblSuite
			// 
			this.lblSuite.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
			this.lblSuite.Location = new System.Drawing.Point(430, 43);
			this.lblSuite.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblSuite.Name = "lblSuite";
			this.lblSuite.Size = new System.Drawing.Size(41, 16);
			this.lblSuite.TabIndex = 48;
			this.lblSuite.Text = "Suite";
			this.lblSuite.Click += new System.EventHandler(this.label5_Click);
			// 
			// lblCommunity
			// 
			this.lblCommunity.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lblCommunity.Location = new System.Drawing.Point(14, 209);
			this.lblCommunity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblCommunity.Name = "lblCommunity";
			this.lblCommunity.Size = new System.Drawing.Size(443, 20);
			this.lblCommunity.TabIndex = 49;
			this.lblCommunity.Text = "benefit of the                         and                         community.";
			// 
			// lblDisclaimer
			// 
			this.lblDisclaimer.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
			this.lblDisclaimer.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.lblDisclaimer.Location = new System.Drawing.Point(10, 337);
			this.lblDisclaimer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblDisclaimer.Name = "lblDisclaimer";
			this.lblDisclaimer.Size = new System.Drawing.Size(388, 66);
			this.lblDisclaimer.TabIndex = 50;
			this.lblDisclaimer.Text = resources.GetString("lblDisclaimer.Text");
			// 
			// picLOR
			// 
			this.picLOR.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picLOR.BackgroundImage")));
			this.picLOR.Location = new System.Drawing.Point(110, 210);
			this.picLOR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.picLOR.Name = "picLOR";
			this.picLOR.Size = new System.Drawing.Size(90, 19);
			this.picLOR.TabIndex = 51;
			this.picLOR.TabStop = false;
			// 
			// lblAlpha
			// 
			this.lblAlpha.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
			this.lblAlpha.ForeColor = System.Drawing.Color.OrangeRed;
			this.lblAlpha.Location = new System.Drawing.Point(655, 141);
			this.lblAlpha.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblAlpha.Name = "lblAlpha";
			this.lblAlpha.Size = new System.Drawing.Size(388, 66);
			this.lblAlpha.TabIndex = 52;
			this.lblAlpha.Text = resources.GetString("lblAlpha.Text");
			// 
			// lblBeta
			// 
			this.lblBeta.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
			this.lblBeta.ForeColor = System.Drawing.Color.OrangeRed;
			this.lblBeta.Location = new System.Drawing.Point(655, 242);
			this.lblBeta.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblBeta.Name = "lblBeta";
			this.lblBeta.Size = new System.Drawing.Size(388, 66);
			this.lblBeta.TabIndex = 53;
			this.lblBeta.Text = resources.GetString("lblBeta.Text");
			// 
			// picDrWiz
			// 
			this.picDrWiz.Image = ((System.Drawing.Image)(resources.GetObject("picDrWiz.Image")));
			this.picDrWiz.Location = new System.Drawing.Point(208, 132);
			this.picDrWiz.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.picDrWiz.Name = "picDrWiz";
			this.picDrWiz.Size = new System.Drawing.Size(48, 48);
			this.picDrWiz.TabIndex = 54;
			this.picDrWiz.TabStop = false;
			this.picDrWiz.Click += new System.EventHandler(this.picDrWiz_Click);
			// 
			// lblThanks
			// 
			this.lblThanks.AutoSize = true;
			this.lblThanks.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
			this.lblThanks.ForeColor = System.Drawing.Color.DarkGreen;
			this.lblThanks.Location = new System.Drawing.Point(391, 301);
			this.lblThanks.Margin = new System.Windows.Forms.Padding(7, 0, 4, 0);
			this.lblThanks.MaximumSize = new System.Drawing.Size(0, 20);
			this.lblThanks.Name = "lblThanks";
			this.lblThanks.Size = new System.Drawing.Size(66, 19);
			this.lblThanks.TabIndex = 55;
			this.lblThanks.Text = "Thanks...";
			this.lblThanks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblThanks.Click += new System.EventHandler(this.lblThanks_Click);
			// 
			// lblGitHub
			// 
			this.lblGitHub.AutoSize = true;
			this.lblGitHub.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lblGitHub.Location = new System.Drawing.Point(30, 317);
			this.lblGitHub.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblGitHub.Name = "lblGitHub";
			this.lblGitHub.Size = new System.Drawing.Size(196, 14);
			this.lblGitHub.TabIndex = 56;
			this.lblGitHub.Text = "Or you can report issues on GitHub";
			// 
			// lblGitIssues
			// 
			this.lblGitIssues.AutoEllipsis = true;
			this.lblGitIssues.AutoSize = true;
			this.lblGitIssues.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lblGitIssues.Location = new System.Drawing.Point(222, 317);
			this.lblGitIssues.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblGitIssues.Name = "lblGitIssues";
			this.lblGitIssues.Size = new System.Drawing.Size(33, 14);
			this.lblGitIssues.TabIndex = 57;
			this.lblGitIssues.TabStop = true;
			this.lblGitIssues.Text = "Here";
			this.lblGitIssues.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.lblGitIssues.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblGitIssues_LinkClicked);
			// 
			// linkCharity
			// 
			this.linkCharity.AutoEllipsis = true;
			this.linkCharity.AutoSize = true;
			this.linkCharity.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.linkCharity.Location = new System.Drawing.Point(307, 189);
			this.linkCharity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.linkCharity.Name = "linkCharity";
			this.linkCharity.Size = new System.Drawing.Size(87, 19);
			this.linkCharity.TabIndex = 58;
			this.linkCharity.TabStop = true;
			this.linkCharity.Text = "Charityware";
			this.linkCharity.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCharity_LinkClicked);
			// 
			// lblProgram
			// 
			this.lblProgram.AutoSize = true;
			this.lblProgram.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lblProgram.Location = new System.Drawing.Point(10, 189);
			this.lblProgram.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblProgram.Name = "lblProgram";
			this.lblProgram.Size = new System.Drawing.Size(135, 19);
			this.lblProgram.TabIndex = 59;
			this.lblProgram.Text = "Something-O-Rama";
			// 
			// lblCompiled
			// 
			this.lblCompiled.AutoSize = true;
			this.lblCompiled.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lblCompiled.ForeColor = System.Drawing.Color.Gray;
			this.lblCompiled.Location = new System.Drawing.Point(177, 72);
			this.lblCompiled.Margin = new System.Windows.Forms.Padding(7, 0, 4, 0);
			this.lblCompiled.MaximumSize = new System.Drawing.Size(0, 20);
			this.lblCompiled.Name = "lblCompiled";
			this.lblCompiled.Size = new System.Drawing.Size(104, 13);
			this.lblCompiled.TabIndex = 60;
			this.lblCompiled.Text = "Compiled 6/22/2022";
			this.lblCompiled.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblEXEfile
			// 
			this.lblEXEfile.AutoSize = true;
			this.lblEXEfile.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.lblEXEfile.ForeColor = System.Drawing.Color.Goldenrod;
			this.lblEXEfile.Location = new System.Drawing.Point(177, 86);
			this.lblEXEfile.Margin = new System.Windows.Forms.Padding(7, 0, 4, 0);
			this.lblEXEfile.MaximumSize = new System.Drawing.Size(0, 20);
			this.lblEXEfile.Name = "lblEXEfile";
			this.lblEXEfile.Size = new System.Drawing.Size(222, 13);
			this.lblEXEfile.TabIndex = 61;
			this.lblEXEfile.Text = "C:\\PortableApps\\UtilORama\\BlankORama.exe";
			this.lblEXEfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblEXEfile.Visible = false;
			// 
			// frmAbout
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(492, 408);
			this.Controls.Add(this.lblEXEfile);
			this.Controls.Add(this.lblCompiled);
			this.Controls.Add(this.lblProgram);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.linkCharity);
			this.Controls.Add(this.lblGitIssues);
			this.Controls.Add(this.lblGitHub);
			this.Controls.Add(this.lblThanks);
			this.Controls.Add(this.lblBeta);
			this.Controls.Add(this.lblAlpha);
			this.Controls.Add(this.picLOR);
			this.Controls.Add(this.lblDisclaimer);
			this.Controls.Add(this.lblSuite);
			this.Controls.Add(this.labelUtils);
			this.Controls.Add(this.labelSuite);
			this.Controls.Add(this.picxLights);
			this.Controls.Add(this.picGPL);
			this.Controls.Add(this.lblEmail);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.labelGPL);
			this.Controls.Add(this.lblSourceCode);
			this.Controls.Add(this.labelAnd);
			this.Controls.Add(this.labelAuthorName);
			this.Controls.Add(this.labelCompanyName);
			this.Controls.Add(this.picIcon);
			this.Controls.Add(this.labelProductName);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.labelCopyright);
			this.Controls.Add(this.lblFreeware);
			this.Controls.Add(this.lblCommunity);
			this.Controls.Add(this.picDrWiz);
			this.Controls.Add(this.textBoxDescription);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About Program";
			this.Load += new System.EventHandler(this.frmAbout_Load);
			this.Shown += new System.EventHandler(this.frmAbout_Shown);
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picGPL)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picxLights)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picLOR)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picDrWiz)).EndInit();
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
		private System.Windows.Forms.Label lblFreeware;
		private System.Windows.Forms.Label lblSourceCode;
		private System.Windows.Forms.LinkLabel labelGPL;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.LinkLabel lblEmail;
		private System.Windows.Forms.PictureBox picGPL;
		private System.Windows.Forms.PictureBox picxLights;
		private System.Windows.Forms.Label labelSuite;
		private System.Windows.Forms.LinkLabel labelUtils;
		private System.Windows.Forms.Label lblSuite;
		private System.Windows.Forms.Label lblCommunity;
		private System.Windows.Forms.Label lblDisclaimer;
		private System.Windows.Forms.PictureBox picLOR;
		private Label lblAlpha;
		private Label lblBeta;
		private PictureBox picDrWiz;
		private Label lblThanks;
		private Label lblGitHub;
		private LinkLabel lblGitIssues;
		private LinkLabel linkCharity;
		private Label lblProgram;
		private Label lblCompiled;
		private Label lblEXEfile;
	}
}
