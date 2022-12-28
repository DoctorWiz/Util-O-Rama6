using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RecentlyUsed;

namespace RecentlyUsedTest
{
	public partial class frmTester : Form
	{
		private string fileName = "";
		private MRU recentList = new MRU("WizsterSoftware\\MRUtester");
		public frmTester()
		{
			InitializeComponent();
			try
			{
				string[] args = Environment.GetCommandLineArgs();
				if (args != null)
				{
					if (args.Length > 0)
					{
						string cmdFile = args[0];
						if (File.Exists(cmdFile))
						{
							LoadImage(cmdFile);
						}
					}
				}
			}
			catch (Exception ex)
			{
				// Ignore
			}
		}

		private void UpdateTitleBar()
		{
			if (fileName.Length > 0)
			{
				this.Text = "MRU Tester - " + fileName;
				mnuFileNew.Enabled = false;
				mnuFileSave.Enabled = false;
				mnuFileSaveAs.Enabled = false;
			}
			else
			{
				this.Text = "Most Recently Used (MRU) Tester - [New File]";
				mnuFileNew.Enabled = false;
				mnuFileSave.Enabled = false;
				mnuFileSaveAs.Enabled = false;
			}
		}

		private void UpdateRecentMenu()
		{
			mnuFileRecent.DropDownItems.Clear();
			if (recentList.ItemCount > 0)
			{
				for (int i = 0; i < recentList.ItemCount; i++)
				{
					string menuText = "";
					if (i < 9) { menuText += "&"; }
					string itemName = recentList.GetItem(i);
					menuText += (i + 1).ToString() + "  ";
					string[] paths = itemName.Split("\\");
					menuText += paths[0] + "\\" + paths[1] + "\\...\\";
					menuText += paths[paths.Length - 1];
					mnuFileRecent.DropDownItems.Add(menuText, null, mnuFileRecentItem_Click);
				}
				mnuFileRecent.Enabled = true;
			}
			else
			{
				mnuFileRecent.Enabled = false;
			}
		}

		private void LoadImage(string imageFile)
		{
			//TODO Scale the picture to fit and keep aspect ratio
			double boxRatio = picImage.Width / picImage.Height;
			Image img = Image.FromFile(imageFile);
			double imgRatio = img.Width / img.Height;
			int newW = Math.Max(img.Width, picImage.Width);
			int newH = Math.Max(img.Height, picImage.Height);
			if (imgRatio > boxRatio)
			{


			}
			else
			{

			}

			picImage.Width = newW;
			picImage.Height = newH;
			picImage.Image = img;
			fileName = imageFile;
			UpdateTitleBar();
			recentList.UseItem(fileName);
		}


		private void mnuFileOpen_Click(object sender, EventArgs e)
		{
			dlgFileOpen.Filter = "Image Files|*.jpg;*.png;*.gif";
			dlgFileOpen.Title = "Open Image File";
			dlgFileOpen.FileName = "";
			DialogResult dr = dlgFileOpen.ShowDialog();
			if (dr == DialogResult.OK)
			{
				fileName = dlgFileOpen.FileName;
				LoadImage(fileName);
			}
		}

		private void mnuFileNew_Click(object sender, EventArgs e)
		{
			fileName = "";
			picImage.Image = null;
			UpdateTitleBar();
		}

		private void mnuFileRecent_Click(object sender, EventArgs e)
		{

		}

		private void mnuFileRecentItem_Click(object sender, EventArgs e)
		{
			string menuText = sender.ToString();
			int tabAt = menuText.IndexOf("  ");
			string theNum = menuText.Substring(0, tabAt);
			theNum = theNum.Replace("&", "");
			int itemNum = -1;
			Int32.TryParse(theNum, out itemNum);
			if (itemNum > 0)
			{
				if (itemNum <= recentList.ItemCount)
				{
					string theFile = recentList.GetItem(itemNum - 1);
					LoadImage(theFile);
				}
			}
		}

		private void mnuFile_Click(object sender, EventArgs e)
		{
			//UpdateRecentMenu();
		}

		private void mnuFileSave_Click(object sender, EventArgs e)
		{
			recentList.UseItem(fileName);
			UpdateTitleBar();
		}

		private void mnuFileSaveAs_Click(object sender, EventArgs e)
		{
			dlgFileSave.Filter = "Image Files|*.jpg;*.png;*.gif";
			dlgFileSave.Title = "Save Image File As...";
			dlgFileOpen.FileName = fileName;
			DialogResult dr = dlgFileSave.ShowDialog();
			if (dr == DialogResult.OK)
			{
				fileName = dlgFileSave.FileName;
				recentList.UseItem(fileName);
				UpdateTitleBar();
			}

		}

		private void mnuFileClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void mnuFile_DropDownOpening(object sender, EventArgs e)
		{
			UpdateRecentMenu();
		}
	}
}
