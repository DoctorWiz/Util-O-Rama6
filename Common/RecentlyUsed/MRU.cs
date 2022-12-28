using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using FileHelper; // Since the MRU may likely be holding file names
using Microsoft.Win32;

namespace RecentlyUsed

{
	// Most-Recently-Used
	// Holds 10 or more strings, could be files, paths, or ???
	//    Validate function must be customized for string type/purpose
	// Item 0 is the most recently used, 1, 2, 3. etc after that
	// If new item was already in the list it is moved to the top (index 0)

	// Note: This is a generic version good for any type of string (including files).
	// It is Application agnostic.
	// There is also a version called MRUoRama which is specifically for files and
	// specifically for the Util-O-Rama suite.
	//    (In the UtilORama4\Common\RecentlyUsed folder)

	public class MRU
	{
		private List<string> usedItems = new List<string>();
		private string myName = "";
		private string myFullName = "";
		private RegistryKey myKey;
		private int _maxItems = 20; // Note: Max of 999
		private int _itemCount = 0;
		public bool AutoSave = true;
		private bool _dirty = false;
		public bool CaseSensitive = false;

		// Constructor
		public MRU(string theName, int maxEntries = 20, bool autoWrite = true)
		{
			int readCount = 0;
			if (maxEntries > 999) maxEntries = 999;
			if (theName.Length == 0) theName = Application.ProductName;
			if (theName.Substring(theName.Length - 4).ToLower() == "rama")
			{ theName = "UtilORama\\" + theName; }
			theName = theName.Replace("-", "");
			AutoSave = autoWrite;
			myName = theName;
			myFullName = "SOFTWARE\\" + myName + "\\MRU";

			myKey = Registry.CurrentUser.CreateSubKey(myFullName);

			for (int i = 0; i < _maxItems; i++)
			{
				string entryName = "Item" + i.ToString("000");
				string itemText = "";
				try
				{
					itemText = (string)myKey.GetValue(entryName);
				}
				catch (Exception ex)
				{
					// Ignore the error, key probably does not exist
					//! TEMP see what the error is, make sure we are not getting unexpected behaviour
					//! Remove this one we know it is stable
					string s = ex.Message;
					System.Diagnostics.Debugger.Break();
				}
				if (itemText != null)
				{
					if (itemText.Length > 0)
					{
						readCount++;
						usedItems.Add(itemText);
					}
				}
			}
			_itemCount = Math.Min(_maxItems, usedItems.Count);

		}

		public int MaxItems
		{
			get { return _maxItems; }
			set
			{
				if ((value > 0) && (value < 1000))
				{ _maxItems = value; }
			}
		}

		public int ItemCount
		{ get { return _itemCount; } }

		public bool Dirty
		{ get { return _dirty; } }

		public string GetItem(int index)
		{
			string s = "";
			if (index < _itemCount)
			{
				s = usedItems[index];
			}
			return s;
		}

		public void UseItem(string itemName)
		{
			int foundAt = WhereIsItem(itemName);
			if (foundAt < 0)
			{
				// Not in the list, insert it at the top
				usedItems.Insert(0, itemName);
				_dirty = true;
			}

			if (foundAt == 0)
			{
				// This item is also already the most recently used
				// We don't have to do anything!
				// _dirty = no change
			}

			if (foundAt > 0)
			{
				// Found in the list, but not at the top, need to move it to the top
				usedItems.RemoveAt(foundAt);
				usedItems.Insert(0, itemName);
				_dirty = true;
			}
			_itemCount = Math.Min(_maxItems, usedItems.Count);

			if (_dirty && AutoSave)
			{
				Save();
			}
		}

		public bool Remove(string itemName)
		{
			StringComparison comparison = StringComparison.OrdinalIgnoreCase;
			if (CaseSensitive)
			{
				comparison = StringComparison.Ordinal;
			}
			bool success = false;
			try
			{
				usedItems.Remove(itemName);
				success = true;
				_dirty = true;
			}
			catch { }
			if (_dirty && AutoSave)
			{
				Save();
			}
			return success;
		}

		public bool Remove(int index)
		{
			bool success = false;
			if (index < usedItems.Count)
			{
				usedItems.RemoveAt(index);
				success = true;
				_dirty = true;
			}
			if (_dirty && AutoSave)
			{
				Save();
			}
			return success;
		}

		public void Save()
		{
			if (_dirty)
			{
				//appSettings.MRPath0 = files[0];
				for (int i = 0; i < usedItems.Count; i++)
				{
					if (i < _maxItems)
					{
						if (usedItems[i].Length > 0)
						{
							string entryName = "Item" + i.ToString("000");
							try
							{
								myKey.SetValue(entryName, usedItems[i]);
							}
							catch (Exception e)
							{
								//!EXCEPTION HERE
							}
						} // itemName.Length > 0
					} // i < _maxItems
				} // for loop, used items
				_dirty = false;
			} // was _dirty
			_itemCount = Math.Min(_maxItems, usedItems.Count);
		}

		public int WhereIsItem(string itemName)
		{
			int foundAt = -1;
			if (itemName.Length > 0)
			{
				StringComparison comparison = StringComparison.OrdinalIgnoreCase;
				// Is it already in our list?
				for (int i = 0; i < _itemCount; i++)
				{
					if (foundAt < 0)
					{
						bool b = false;
						if (String.Compare(itemName, usedItems[i], comparison) == 0) b = true;
						if (b)
						{
							foundAt = i;
							i = _itemCount; // force early exit of for loop
						}
					}
				}
			}
			return foundAt;
		}

		private bool DoesItemExist(string itemName)
		{
			bool exists = (WhereIsItem(itemName) >= 0);
			return exists;
		}

		private int FillComboBox(ComboBox combo, bool clear = true)
		{
			// (ComboBox is passed by reference)
			// Agnostic to data type in the MRU-- could be filenames, could be ...?
			// See Also: FillFileComboBox
			int count = 0;  // Redundant.  But keeping it for now for debugging.
			if (clear)
			{ combo.Items.Clear(); }
			for (int i=0; i< usedItems.Count; i++)
			{
				combo.Items.Add(usedItems[i]);
				count++;
			}
			return count;
		}

		private int FillFileComboBox(ComboBox combo, bool clear = true)
		{
			// (ComboBox is passed by reference)
			// Melding of FillComboBox and ValidateFiles
			// Adds abbreviated filename to the combo box only if it currently exists, but does not remove
			// non-existing files from the internal MRU ItemList in case they reappear later.
			int count = 0;
			string fileName = "";
			// How many characters will fit?
			int avg = 0;  int avgW = 0;
			int maxChar = 0;
			for (int i=64; i< 91; i++)
			{
				Size txtSize = TextRenderer.MeasureText(((char)i).ToString(), combo.Font);
				avgW += txtSize.Width;
			}
			maxChar = ((26 * combo.Width) / avgW);

			if (clear)
			{ combo.Items.Clear(); }
			for (int i = 0; i < usedItems.Count; i++)
			{
				fileName = usedItems[i];
				if (Fyle.Exists(fileName))
				{
					string shortName = Fyle.ShortenLongPath(fileName, maxChar);
					combo.Items.Add(shortName);
					count++;
				}
			}
			return count;
		}

		public string FindFullFilename(string abbrevFileName)
		{
			// He-He, see what I did here?  I abbreviated the word abbreviated!
			string fullName = "";
			int p = abbrevFileName.IndexOf("\\…\\");
			int foundAt = -1;
			if (p > 0)
			{
				// Includes last backslash?  Includes elipsis?
				string part1 = abbrevFileName.Substring(0, p);
				int p1l = part1.Length;
				// Includes elipsis?  Includes next backslash?
				string part2 = abbrevFileName.Substring(p+2);
				int p2l = part2.Length;
				for (int i = 0; i < usedItems.Count; i++)
				{
					if (usedItems[i].Substring(0,p1l) == part1)
					{
						if (usedItems[i].Substring(usedItems[i].Length - p2l) == part2)
						{
							foundAt = i;
							fullName = usedItems[i];
							i = usedItems.Count; // Force exit of loop
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < usedItems.Count; i++)
				{
					if (usedItems[i]== abbrevFileName)
					{
						foundAt = i;
						fullName = usedItems[i];
						i = usedItems.Count; // Force exit of loop
					}
				}

			}
			if (fullName.Length == 0)
			{
				//! NOT FOUND!
				System.Diagnostics.Debugger.Break();
			}
			return fullName;
		}


		private static bool IsWizard
		{
			get
			{
				bool ret = false;
				string usr = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
				usr = usr.ToLower();
				int i = usr.IndexOf("wizard");
				if (i >= 0) ret = true;
				return ret;
			}
		}

		public int ValidateFiles()
		{
			// Go thru the list and remove any files which no longer exist
			// (see also: ValidatePaths)
			int valid = 0;
			for (int i = 0; i < usedItems.Count; i++)
			{
				try
				{
					if (Fyle.Exists(usedItems[i]))
					{
						valid++;
					}
					else
					{
						usedItems.RemoveAt(i);
					}
				}
				catch (Exception ex)
				{
					usedItems.RemoveAt(i);
				}
			}
			return valid;
		}

		public int ValidatePaths()
		{
			// Go thru the list and remove any folders which no longer exist
			// (see also: ValidateFiles)
			int valid = 0;
			for (int i = 0; i < usedItems.Count; i++)
			{
				try
				{
					if (Directory.Exists(usedItems[i]))
					{
						valid++;
					}
					else
					{
						usedItems.RemoveAt(i);
					}
				}
				catch (Exception ex)
				{
					usedItems.RemoveAt(i);
				}
			}
			return valid;
		}

		public int ValidateURLs()
		{
			// Go thru the list and remove any URLs which are no longer online
			// (see also: ValidateFiles)
			int valid = 0;
			for (int i = 0; i < usedItems.Count; i++)
			{
				try
				{
					//TODO: figure out how to validate a URL
					//if (URL.Exists(usedItems[i]))	
					if (7 == 7)
					{
						valid++;
					}
					else
					{
						usedItems.RemoveAt(i);
					}
				}
				catch (Exception ex)
				{
					usedItems.RemoveAt(i);
				}
			}
			return valid;
		}
	}
}
