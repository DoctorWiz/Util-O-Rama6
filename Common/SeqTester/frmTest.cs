using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using LORUtils4; using FileHelper;

namespace TestORama
{
	public partial class frmTest : Form
	{
		private string fileName = "";
		private LORSequence4 seq = new LORSequence4();
		private string applicationName = "TEST-O-Rama";
		private string tempPath = "C:\\Windows\\Temp\\";  // Gets overwritten with X:\\Username\\AppData\\Roaming\\Util-O-Rama\\Split-O-Rama\\
		private string thisEXE = "TEST-O-Rama.exe";
		private string[] commandArgs = null;
		private bool firstShown = false;
		private string[] batch_fileList = null;
		private int batch_fileCount = 0;
		private bool batchMode = false;
		const char DELIM1 = '⬖';
		const char DELIM4 = '⬙';


		public frmTest()
		{
			InitializeComponent();
		}

		private void InitForm()
		{
			//this.Cursor = Cursors.WaitCursor;
			RestoreFormPosition();
			string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string mySubDir = "\\UtilORama\\";
			tempPath = appDataDir + mySubDir;
			if (!Directory.Exists(tempPath))
			{
				Directory.CreateDirectory(tempPath);
			}
			string appName = applicationName.Replace("-", "");
			mySubDir	+= appName + "\\";
			tempPath = appDataDir + mySubDir;
			if (!Directory.Exists(tempPath))
			{
				Directory.CreateDirectory(tempPath);
			}
			ProcessCommandLine();
			//this.Cursor = DefaultCursor;
			chkAutoLaunch.Checked = Properties.Settings.Default.AutoLaunch;
			chkAutoLoad.Checked = Properties.Settings.Default.AutoLoad;
			chkAutoSave.Checked = Properties.Settings.Default.AutoSave;
			fileName = Properties.Settings.Default.LastFile;
		}

		private void ProcessCommandLine()
		{
			commandArgs = Environment.GetCommandLineArgs();
			string arg;
			for (int f = 0; f < commandArgs.Length; f++)
			{
				arg = commandArgs[f];
				// Does it LOOK like a file?
				int isFile = 0;
				if (arg.Substring(1, 2).CompareTo(":\\") == 0) isFile = 1;  // Local File
				if (arg.Substring(0, 2).CompareTo("\\\\") == 0) isFile = 1; // UNC file
				if (arg.Substring(4).IndexOf(".") > lutils.UNDEFINED) isFile++;  // contains a period
				if (Fyle.InvalidCharacterCount(arg) == 0) isFile++;
				if (isFile == 3)
				{
					if (File.Exists(arg))
					{
						string ext = Path.GetExtension(arg).ToLower();
						if (ext.CompareTo(".exe") == 0)
						{
							if (f == 0)
							{
								thisEXE = arg;
							}
								
						}
						if ((ext.CompareTo(".lms") == 0) ||
							  (ext.CompareTo(".las") == 0) ||
							  (ext.CompareTo(".lcc") == 0) ||
							  (ext.CompareTo(".lcb") == 0))
						{
							Array.Resize(ref batch_fileList, batch_fileCount + 1);
							batch_fileList[batch_fileCount] = arg;
							batch_fileCount++;
						}
					}
				}
				else
				{
					// Not a file, is it an argument
					if (arg.Substring(0, 1).CompareTo("/") == 0)
					{
						//TODO: Process any commands
					}
				}
			} // foreach argument
			if (batch_fileCount == 1)
			{


			}
			else
			{
				if (batch_fileCount > 1)
				{
					ProcessFileBatch(batch_fileList);
				}
			}


		}

		private void SaveFormPosition()
		{
			// Get current location, size, and state
			Point myLoc = this.Location;
			Size mySize = this.Size;
			FormWindowState myState = this.WindowState;
			// if minimized or maximized
			if (myState != FormWindowState.Normal)
			{
				// override with the restore location and size
				myLoc = new Point(this.RestoreBounds.X, this.RestoreBounds.Y);
				mySize = new Size(this.RestoreBounds.Width, this.RestoreBounds.Height);
			}

			// Save it for later!
			Properties.Settings.Default.Location = myLoc;
			Properties.Settings.Default.Size = mySize;
			Properties.Settings.Default.WindowState = (int)myState;
			Properties.Settings.Default.Save();
		} // End SaveFormPostion

		private void RestoreFormPosition()
		{
			// Multi-Monitor aware
			// with bounds checking
			// repositions as necessary
			// should(?) be able to handle an additional screen that is no longer there,
			// a repositioned taskbar or gadgets bar,
			// or a resolution change.

			// Note: If the saved position spans more than one screen
			// the form will be repositioned to fit all within the
			// screen containing the center point of the form.
			// Thus, when restoring the position, it will no longer
			// span monitors.
			// This is by design!
			// Alternative 1: Position it entirely in the screen containing
			// the top left corner

			Point savedLoc = Properties.Settings.Default.Location;
			Size savedSize = Properties.Settings.Default.Size;
			FormWindowState savedState = (FormWindowState)Properties.Settings.Default.WindowState;
			int x = savedLoc.X; // Default to saved postion and size, will override if necessary
			int y = savedLoc.Y;
			int w = savedSize.Width;
			int h = savedSize.Height;
			Point center = new Point(x + w / w, y + h / 2); // Find center point
			int onScreen = 0; // Default to primary screen if not found on screen 2+
			Screen screen = Screen.AllScreens[0];

			// Find which screen it is on
			for (int si = 0; si < Screen.AllScreens.Length; si++)
			{
				// Alternative 1: Change "Contains(center)" to "Contains(savedLoc)"
				if (Screen.AllScreens[si].WorkingArea.Contains(center))
				{
					screen = Screen.AllScreens[si];
					onScreen = si;
				}
			}
			Rectangle bounds = screen.WorkingArea;
			// Alternate 2:
			//Rectangle bounds = Screen.GetWorkingArea(center);

			// Test Horizontal Positioning, correct if necessary
			if (this.MinimumSize.Width > bounds.Width)
			{
				// Houston, we have a problem, monitor is too narrow
				System.Diagnostics.Debugger.Break();
				w = this.MinimumSize.Width;
				// Center it horizontally over the working area...
				//x = (bounds.Width - w) / 2 + bounds.Left;
				// OR position it on left edge
				x = bounds.Left;
			}
			else
			{
				// Should fit horizontally
				// Is it too far left?
				if (x < bounds.Left) x = bounds.Left; // Move over
																							// Is it too wide?
				if (w > bounds.Width) w = bounds.Width; // Shrink it
																								// Is it too far right?
				if ((x + w) > bounds.Right)
				{
					// Keep width, move it over
					x = (bounds.Width - w) + bounds.Left;
				}
			}

			// Test Vertical Positioning, correct if necessary
			if (this.MinimumSize.Height > bounds.Height)
			{
				// Houston, we have a problem, monitor is too short
				System.Diagnostics.Debugger.Break();
				h = this.MinimumSize.Height;
				// Center it vertically over the working area...
				//y = (bounds.Height - h) / 2 + bounds.Top;
				// OR position at the top edge
				y = bounds.Top;
			}
			else
			{
				// Should fit vertically
				// Is it too high?
				if (y < bounds.Top) y = bounds.Top; // Move it down
																						// Is it too tall;
				if (h > bounds.Height) h = bounds.Height; // Shorten it
																									// Is it too low?
				if ((y + h) > bounds.Bottom)
				{
					// Kepp height, raise it up
					y = (bounds.Height - h) + bounds.Top;
				}
			}

			// Position and Size should be safe!
			// Move and Resize the form
			this.SetDesktopLocation(x, y);
			this.Size = new Size(w, h);

			// Window State
			if (savedState == FormWindowState.Maximized)
			{
				if (this.MaximizeBox)
				{
					// Optional.  Personally, I think it should always be reloaded non-maximized.
					//this.WindowState = savedState;
				}
			}
			if (savedState == FormWindowState.Minimized)
			{
				if (this.MinimizeBox)
				{
					// Don't think it's right to reload to a minimized state (confuses the user),
					// but you can enable this if you want.
					//this.WindowState = savedState;
				}
			}

		} // End LoadFormPostion


		private void frmReport_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveFormPosition();
		}

		


		private void frmReport_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				ProcessFileList(files);
			}
		}

		private void ProcessFileList(string[] batchFilenames)
		{
			batch_fileCount = 0; // reset
			DialogResult dr = DialogResult.None;

			foreach (string file in batchFilenames)
			{
				string ext = Path.GetExtension(file).ToLower();
				if ((ext.CompareTo(".lms") == 0) ||
					  (ext.CompareTo(".las") == 0) ||
						(ext.CompareTo(".lcc") == 0) ||
					  (ext.CompareTo(".lcb") == 0))
				{
					Array.Resize(ref batch_fileList, batch_fileCount + 1);
					batch_fileList[batch_fileCount] = file;
					batch_fileCount++;
				}
			}
			if (batch_fileCount > 1)
			{
				batchMode = true;
				ProcessFileBatch(batch_fileList);
			}
			else
			{
				if (batch_fileCount == 1)
				{
					string thisFile = batch_fileList[0];
					string reportTempFile = tempPath + Path.GetFileNameWithoutExtension(thisFile) +" Report.htm";
					//CreateReport(thisFile, reportTempFile);
				}
			} // batch_fileCount-- Batch Mode or Not
		} // end ProcessFileList

		private void ProcessFileBatch(string[] batchFilenames)
		{
			string thisFile = batch_fileList[0];
			string reportTempFile = tempPath + Path.GetFileNameWithoutExtension(thisFile) + " Report.htm";
			//CreateReport(thisFile, reportTempFile);

			for (int f=1; f< batchFilenames.Length; f++)
			{
				thisFile = batch_fileList[f];
				Process.Start(thisEXE, thisFile);
			}
			batchMode = false;
		} // end ProcessFileBatch

		private void ImBusy(bool isBusy)
		{
			if (isBusy)
			{
				this.Cursor = Cursors.WaitCursor;
				this.Enabled = false;
			}
			else
			{
				this.Enabled = true;
				this.Cursor = Cursors.Default;
			}
		} // end ImBusy

		private void frmReport_Load(object sender, EventArgs e)
		{
			InitForm();
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			string initDir = lutils.DefaultSequencesPath;
			string initFile = "";

			dlgOpenFile.Filter = "Light-O-Rama Sequences|*.lms;*.las";
			dlgOpenFile.FilterIndex = 1;
			dlgOpenFile.DefaultExt = ".lms";
			dlgOpenFile.InitialDirectory = initDir;
			dlgOpenFile.FileName = initFile;
			dlgOpenFile.CheckFileExists = true;
			dlgOpenFile.CheckPathExists = true;
			dlgOpenFile.Multiselect = false;
			dlgOpenFile.Title = "Select a Sequence...";
			//pnlAll.Enabled = false;
			DialogResult result = dlgOpenFile.ShowDialog(this);

			if (result == DialogResult.OK)
			{
				ImBusy(true);
				string thisFile  = dlgOpenFile.FileName;
				this.Text = "Test-O-Rama - " + Path.GetFileName(thisFile);
				Properties.Settings.Default.LastFile = thisFile;
				Properties.Settings.Default.Save();
				int err = loadSequence(thisFile);
				//CreateReport(thisFile, thisRpt);

				//webReport.Navigate(thisRpt);
				btnSaveAs.Enabled = true;
				btnReload.Enabled = true;
				//btnBrowse.Text = "Analyze another Sequence...";
				//MessageBox.Show(this, seq.summary(), "Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
				ImBusy(false);

			} // end if (result = DialogResult.OK)
				//pnlAll.Enabled = true;

		}

		private int loadSequence(string seqFile)
		{
			int err = 0;
			txtFilename.Text = Path.GetFileName(seqFile);
			err = seq.ReadSequenceFile(seqFile);
			MessageBox.Show(this, seq.summary(), "Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
			fileName = seqFile;

			if (err == 0)
			{
				btnSaveAs.Enabled = true;
				if (chkAutoSave.Checked)
				{
					int f = 2;
					string autoSeqPath = lutils.DefaultSequencesPath;
					string autoSeqName = Path.GetFileNameWithoutExtension(fileName);
					string ext = Path.GetExtension(fileName);
					string tryFile = autoSeqPath + autoSeqName + " Rewrite" + ext;
					//while (System.IO.File.Exists(tryFile))
					//{
					//	tryFile = autoSeqPath + autoSeqName + " (" + f.ToString() + ")" + ext;
					//	f++;
					//}
					err = SaveSequence(tryFile);
					
				}
			}
			return err;
			 


		}

	

		private void chkAutoLoad_CheckedChanged(object sender, EventArgs e)
		{
			if (firstShown)
			{
				Properties.Settings.Default.AutoLoad = chkAutoLoad.Checked;
				Properties.Settings.Default.Save();
			}
		}

		private void chkAutoSave_CheckedChanged(object sender, EventArgs e)
		{
			if (firstShown)
			{
				Properties.Settings.Default.AutoSave = chkAutoSave.Checked;
				Properties.Settings.Default.Save();
				if (!chkAutoSave.Checked) chkAutoLaunch.Checked = false;
				chkAutoLaunch.Enabled = chkAutoSave.Checked;
			}
		}

		private void chkAutoLaunch_CheckedChanged(object sender, EventArgs e)
		{
			if (firstShown				)
			{
				Properties.Settings.Default.AutoLaunch = chkAutoLaunch.Checked;
				Properties.Settings.Default.Save();
			}
		}

		private void frmTest_Paint(object sender, PaintEventArgs e)
		{
			if (!firstShown)
			{
				firstShown = true;
				FirstShow();
			}
		} // end Paint event

		private void FirstShow()
		{
			if (chkAutoLoad.Checked)
			{
				if (File.Exists(fileName))
				{
					loadSequence(fileName);
					MessageBox.Show(this, seq.summary(), "Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		} // end FirstShow

		private void btnSaveAs_Click(object sender, EventArgs e)
		{
			string filt = "";
			string tit = "";
			//string ext = Path.GetExtension(fileName).ToLower();
			string ext = ".lms";
			//if (ext.CompareTo(".loredit") == 0)
			//{
			filt = "Light-O-Rama Musical Sequences|*.lms|Light-O-Rama Animated Sequence|*.las";
			tit = "Save Sequence As...";
			//}
			string initDir = "";
			string initFile = "";
			if (fileName.Length > 5)
			{
				if (Directory.Exists(Path.GetDirectoryName(fileName)))
				{
					initDir = Path.GetDirectoryName(fileName);
					initFile = Path.GetFileNameWithoutExtension(fileName) + " Rewrite"; // + ext;
				}
			}
			if (initDir.Length < 5)
			{
				// Can't imagine that we would ever make it this far, but, just in case...
				//if (ext.CompareTo(".lcc") == 0)
				//{
				//	initDir = lutils.DefaultChannelConfigsPath;
				//}
				//else
				//{
					initDir = lutils.DefaultSequencesPath;
				//}
			}

			dlgSaveFile.Filter = filt;
			dlgSaveFile.FilterIndex = 1;
			//dlgSaveFile.FileName = Path.GetFullPath(fileSeqCur) + Path.GetFileNameWithoutExtension(fileSeqCur) + " Part " + part.ToString() + ext;
			dlgSaveFile.CheckPathExists = true;
			dlgSaveFile.InitialDirectory = initDir;
			dlgSaveFile.DefaultExt = ext;
			dlgSaveFile.OverwritePrompt = true;
			dlgSaveFile.Title = tit;
			dlgSaveFile.SupportMultiDottedExtensions = true;
			dlgSaveFile.ValidateNames = true;
			//newFileIn = Path.GetFileNameWithoutExtension(fileSeqCur) + " Part " + part.ToString(); // + ext;
			//newFileIn = "Part " + part.ToString() + " of " + Path.GetFileNameWithoutExtension(fileSeqCur);
			//newFileIn = "Part Mother Fucker!!";
			dlgSaveFile.FileName = initFile;
			DialogResult result = dlgSaveFile.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				SaveSequence(dlgSaveFile.FileName);
			}

		} // end Save As...

		private int SaveSequence(string newName)
		{
			int err = seq.WriteSequenceFile(newName);
			if (err==0)
			{
				if (chkAutoLaunch.Checked)
				{
					System.Diagnostics.Process.Start(newName);
				}
			}
			return err;
		} // end SaveSequence

		private void btnReload_Click(object sender, EventArgs e)
		{
			if (File.Exists(fileName))
			{
				loadSequence(fileName);
			}
		}
	} // end form frmTest
} // end namespace InfoRama
