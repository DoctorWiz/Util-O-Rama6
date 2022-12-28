using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
//using LOR4;
using FileHelper;

namespace UtilORama4
{
	partial class frmAbout : Form
	{

		string applicationName = "Util-O-Rama";

		public frmAbout()
		{
			InitializeComponent();

			applicationName = Fyle.AssemblyTitle;

			this.Text = String.Format("About {0}", applicationName);
			this.labelProductName.Text = applicationName;
			//string ver = String.Format("Version {0}", AssemblyVersion);
			//string[] vparts = AssemblyVersion.Split('.');
			//if (Int16.Parse(vparts[1]) < 1)
			//{
			//	ver = "Alpha " + ver + " α";
			//}
			//else
			//{
			//	if (Int16.Parse(vparts[1]) < 2)
			//	{
			//		ver = "Beta " + ver + " β";
			//	}
			//}
			string ver = "Version " + Fyle.Version();

			this.labelVersion.Text = ver;
			//this.labelCopyright.Text = AssemblyCopyright;
			//this.labelCompanyName.Text = AssemblyCompany;
			this.textBoxDescription.Text = AssemblyDescription;


			labelSuite.Text = applicationName + "\r\n" + labelSuite.Text;
			labelFreeware.Text = applicationName + labelFreeware.Text;

			int major = 4;
			int minor = 0;
			int patch = 0;
			int build = 0;
			Fyle.VersionNumbers(ref major, ref minor, ref patch, ref build);
			if (patch < 2) // Patch version 0 and 1 are alpha releases
			{
				lblAlpha.Top = lblDisclaimer.Top;
				lblDisclaimer.Top = lblAlpha.Top + lblAlpha.Height + 8;
				this.Height = this.Height + lblAlpha.Height + 8;
				lblAlpha.Visible = true;
			}
			else
			{
				if (patch < 5) // Patch versions 2, 3, and 4 are beta releases
				{
					lblBeta.Top = lblDisclaimer.Top;
					lblDisclaimer.Top = lblBeta.Top + lblBeta.Height + 8;
					this.Height = this.Height + lblBeta.Height + 8;
					lblBeta.Visible = true;
				}
			}



		}

		public frmAbout(string utilityName)
		{
			InitializeComponent();

			applicationName = utilityName;

			this.Text = String.Format("About {0}", applicationName);
			this.labelProductName.Text = applicationName;
			string ver = String.Format("Version {0}", AssemblyVersion);
			string[] vparts = AssemblyVersion.Split('.');
			if (Int16.Parse(vparts[1]) < 1)
			{
				ver = "Alpha " + ver + " α";
			}
			else
			{
				if (Int16.Parse(vparts[1]) < 2)
				{
					ver = "Beta " + ver + " β";
				}
			}

			this.labelVersion.Text = ver;
			//this.labelCopyright.Text = AssemblyCopyright;
			//this.labelCompanyName.Text = AssemblyCompany;
			this.textBoxDescription.Text = AssemblyDescription;


			labelSuite.Text = applicationName + labelSuite.Text;
			labelFreeware.Text = applicationName + labelFreeware.Text;
		}

		public void SetIcon(Image icon)
		{
			picIcon.Image = icon;
		}

		public Image AboutIcon
		{
			get { return picIcon.Image; }
			set { picIcon.Image = value; }
		}

		private void SetGraphic(string utilityName)
		{
			string uname = utilityName.ToLower();
			uname.Replace("-", "");
			uname.Replace("orama", "");
			switch (uname)
			{
				case "vamperizer":
					// Set graphic to...
					break;
				case "vamp":  // Vamp-O-Rama
											// Set graphic to...
					break;



			}


		}




		#region Assembly Attribute Accessors

		// Moved to LORutils
		/*
		public string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "")
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}
		*/

		public string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string AssemblyDescription
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}
		#endregion

		private void frmAbout_Load(object sender, EventArgs e)
		{
			//labelAuthorName.Links.Add("http://drwiz.net");
			//labelCompanyName.Links.Add("http://wizster.com");
			//labelAuthorName.Links.Add(6, 4, "http://drwiz.net");
			//labelCompanyName.Links.Add(6, 4, "http://wizster.com");
		}

		public void InitForm()
		{
			//logoPictureBox.Image = this.Icon.ToBitmap();
		}

		private void frmAbout_Shown(object sender, EventArgs e)
		{
			//logoPictureBox.Image = this.Icon.ToBitmap();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			this.Hide();
			this.Close();
		}

		private void labelAuthorName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://drwiz.net/");
		}

		private void labelCompanyName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://wizster.com/");
		}

		private void labelLOR_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www1.lightorama.com/");
		}

		private void labelGPL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.gnu.org/licenses/gpl-3.0.en.html");
		}

		private void labelBugs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("mailto:dev.utilorama@wizster.com");
		}

		private void label4_Click(object sender, EventArgs e)
		{

		}

		private void okButton_Click_1(object sender, EventArgs e)
		{
			this.Hide();
			this.Close();
		}

		private void picGPL_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.gnu.org/licenses/gpl-3.0.en.html");
		}

		private void picLOR_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www1.lightorama.com/");
		}

		private void labeLOR4Admin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://wizlights.com/utilorama/");
		}

		private void picLOR_MouseEnter(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Hand;
		}

		private void picLOR_MouseLeave(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Default;
		}

		private void picGPL_MouseEnter(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Hand;
		}

		private void picGPL_MouseLeave(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Default;
		}

		private void picIcon_MouseEnter(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Hand;
		}

		private void picIcon_MouseLeave(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Default;
		}

		private void picIcon_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://wizlights.com/utilorama/" + applicationName.ToLower() + "/");
		}
	}
}
