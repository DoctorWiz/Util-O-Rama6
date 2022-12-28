namespace FuzzyTest
{
	partial class frmFuzzyTest
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFuzzyTest));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtOutput = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(143, 450);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 27);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "Test";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(431, 450);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 27);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Close";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtOutput
			// 
			this.txtOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
			this.txtOutput.CausesValidation = false;
			this.txtOutput.Font = new System.Drawing.Font("DejaVu Sans Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.txtOutput.ForeColor = System.Drawing.Color.LawnGreen;
			this.txtOutput.Location = new System.Drawing.Point(3, 1);
			this.txtOutput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.txtOutput.Multiline = true;
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.ReadOnly = true;
			this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtOutput.Size = new System.Drawing.Size(689, 424);
			this.txtOutput.TabIndex = 140;
			// 
			// frmFuzzyTest
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(695, 489);
			this.Controls.Add(this.txtOutput);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmFuzzyTest";
			this.Text = "Fuzzy Tester";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Button btnOK;
		private Button btnCancel;
		private TextBox txtOutput;
	}
}