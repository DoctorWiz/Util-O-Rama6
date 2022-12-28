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

namespace LOR4
{
  // Most-Recently-Used Files
  // This is a File specific and Util-O-Rama specific version!
  // The MRU list is saved both in an application-specific registry key
  // and in a Util-O-Rama shared registry key.
  // [Non Util-O-Rama specific version named simply 'MRU' in Windows/COMMON folder]
  // File000 is the most recently used, 001, 002, 003. etc after that
  // If new item was already in the list it is moved to the top (index 0)

  // Three lists are maintained in memory:
  //   1 Files opened in this app
  //   2 Files opened in any Util-O-Rama app (including this one)
  //   3 Combined list, with app specific files first
  // Only the app and suite lists get saved to the registry
  //   the combined list is created on the fly when opening

  public class MRUoRama
  {
    private List<string> appFiles = new List<string>(); // From this App
    private List<string> suiteFiles = new List<string>(); // From all apps in the suite (including this one)
    private List<string> recentFiles = new List<string>(); // Combined, this app first, other apps next
    private string appName = "";
    private string keyNameApp = "SOFTWARE\\Util-O-Rama\\MRU";
    private string keyNameSuite = "SOFTWARE\\Util-O-Rama\\MRU";
    private RegistryKey myAppKey;
    private RegistryKey mySuiteKey;
    // Maximum number of entries to save in the registry, for App specific and Suite -- EACH
    private int _maxFilesStore = 30; // Note: Max of 999
                                     // Maximum number of entries to populate the combo box or menu, for App specific and Suite -- EACH
                                     //   Therefore max number of entries in combo box or menu will be _maxFilesShow * 2
    private int _maxFilesShow = 10;
    public bool AutoSave = true;
    private bool _dirty = false;
    // True if list contains just paths (no file names)
    // False (default) if list contains full file names with path and extension
    public bool Paths = false;
    public enum FileType { Undefined, Sequences, Selections, Maps, Audio, Visualizations, xml, xSequence, Timings }
    public FileType myfiletype = FileType.Undefined;
    private bool fillingCombo = false;


    // Constructor
    public MRUoRama(FileType fileType, string utilName, bool autoWrite = true, string subName = "")
    {
      myfiletype = fileType;
      appName = utilName;
      AutoSave = autoWrite;
      keyNameApp = "SOFTWARE\\Util-O-Rama\\" + utilName + "\\MRU\\" + FileTypeName(myfiletype);
      if (subName.Length > 0)
      {
        keyNameApp += "\\" + subName;
      }
      keyNameSuite = "SOFTWARE\\Util-O-Rama\\MRU\\" + FileTypeName(myfiletype);

      myAppKey = Registry.CurrentUser.CreateSubKey(keyNameApp);
      mySuiteKey = Registry.CurrentUser.CreateSubKey(keyNameSuite);

      // Get files most recently used in THIS APP first
      for (int i = 1; i <= _maxFilesStore; i++)
      {
        string entryName = "File" + i.ToString("000");
        string fileName = "";
        try
        {
          fileName = (string)myAppKey.GetValue(entryName);
        }
        catch (Exception ex)
        {
          // Ignore the error, key probably does not exist
          //! TEMP see what the error is, make sure we are not getting unexpected behaviour
          //! Remove this one we know it is stable
          string s = ex.Message;
          System.Diagnostics.Debugger.Break();
        }
        if (fileName != null)
        {
          if (fileName.Length > 0)
          {
            // Add to our app specific list of recents
            appFiles.Add(fileName);
          }
        }
      }

      // Get files most recently used in ALL APPS next
      //    (which will include this app)
      for (int i = 1; i <= _maxFilesStore; i++)
      {
        string entryName = "File" + i.ToString("000");
        string fileName = "";
        try
        {
          fileName = (string)mySuiteKey.GetValue(entryName);
        }
        catch (Exception ex)
        {
          // Ignore the error, key probably does not exist
          //! TEMP see what the error is, make sure we are not getting unexpected behaviour
          //! Remove this one we know it is stable
          string s = ex.Message;
          System.Diagnostics.Debugger.Break();
        }
        if (fileName != null)
        {
          if (fileName.Length > 0)
          {
            //readCount++;
            suiteFiles.Add(fileName);
          }
        }
      }

      BuildCombinedList();
    }

    private void BuildCombinedList()
    {
      int idx = 0;
      int c = 0;
      recentFiles.Clear(); // Reset

      // Go thru the list of app specific recent files
      // If less than size of list, AND less than the max # to show
      while ((idx < appFiles.Count) && (c <= _maxFilesShow))
      {
        string f = appFiles[idx];
        if (Fyle.Exists(f))
        {
          // If it exists, add it to the combined list, update the count
          recentFiles.Add(f);
          c++;
        }
        idx++;
      }


      // Next go thru the list of all-suite recents
      // We can add up to maxShow to the existing list
      int n = recentFiles.Count + _maxFilesShow;
      idx = 0; // reset
      while ((idx < suiteFiles.Count) && (c <= n))
      {
        bool found = false; // reset
                            // Get the name, see if it exists
        string f = suiteFiles[idx];
        if (Fyle.Exists(f))
        {
          // Is it already in the list? (pro'ly cuz it was used in this app)
          string fl = f.ToLower();
          for (int j = 0; j < recentFiles.Count; j++)
          {
            if (fl == recentFiles[j].ToLower())
            {
              // Yup, already in the list
              found = true;
              break;
            }
          }
          // Wasn't in the list, add it
          if (!found)
          {
            recentFiles.Add(f);
            c++;
          } // End found, or not
        } // End exists
        idx++;
      } // End loop thru suite recent list
    }


    // Controls how many files are stored in the registry for the app and the suite (each)
    public int MaxFilesStore
    {
      get { return _maxFilesStore; }
      set
      {
        if ((value > 0) && (value < 1000))
        { _maxFilesStore = value; }
      }
    }

    // Controls how many files are populated in the combo box or menu for the app and the suite (each)
    public int MaxFilesShow
    {
      get { return _maxFilesShow; }
      set
      {
        if ((value > 0) && (value < 1000))
        { _maxFilesShow = value; }
      }
    }

    public int FileCount
    { get { return recentFiles.Count; } }

    public bool Dirty
    { get { return _dirty; } }

    public string GetFile(int index)
    {
      string s = "";
      try
      {
        if (index < recentFiles.Count)
        {
          s = recentFiles[index];
        }
      }
      catch (Exception egf)
      {
        //string msg = egf.Message;
        Fyle.BUG("MRUORama.GetFile(" + index.ToString() + ")", egf);
      }
      return s;
    }

    public void UseFile(string fileName)
    {
      int foundAt = -1;
      try
      {
        // Check the App's list first
        foundAt = AppFileIndex(fileName);
        if (foundAt < 0)
        {
          // Not in the list, insert it at the top
          appFiles.Insert(0, fileName);
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
          appFiles.RemoveAt(foundAt);
          appFiles.Insert(0, fileName);
          _dirty = true;
        }
      }
      catch (Exception exa)
      {
        Fyle.BUG("MRUoRama.UseFile/App(" + fileName + ")", exa);
      }


      try
      {
        // Next, check the Suite's list
        foundAt = SuiteFileIndex(fileName);
        if (foundAt < 0)
        {
          // Not in the list, insert it at the top
          suiteFiles.Insert(0, fileName);
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
          suiteFiles.RemoveAt(foundAt);
          suiteFiles.Insert(0, fileName);
          _dirty = true;
        }
      }
      catch (Exception eus)
      {
        Fyle.BUG("MRUORama.UseFile/Suite(" + fileName + ")", eus);
      }


      try
      {
        // Third/Last check the combined list
        foundAt = FileIndex(fileName);
        if (foundAt < 0)
        {
          // Not in the list, insert it at the top
          recentFiles.Insert(0, fileName);
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
          recentFiles.RemoveAt(foundAt);
          recentFiles.Insert(0, fileName);
          _dirty = true;
        }
      }
      catch (Exception eur)
      {
        Fyle.BUG("MRUORama.UseFile/Recent(" + fileName + ")", eur);
      }

      // Save if necessary
      if (_dirty && AutoSave)
      {
        Save();
      }
    }

    public bool Remove(string fileName)
    {
      bool success = false;
      try
      {
        StringComparison comparison = StringComparison.OrdinalIgnoreCase;
        try
        {
          appFiles.Remove(fileName);
          suiteFiles.Remove(fileName);
          recentFiles.Remove(fileName);
          success = true;
          _dirty = true;
        }
        catch (Exception ec1)
        {
          string mc1 = ec1.Message;
          Fyle.BUG(mc1);
        }
        if (_dirty && AutoSave)
        {
          Save();
        }
      }
      catch (Exception exr)
      {
        Fyle.BUG("MRUORama.Remove(" + fileName + ")", exr);
      }
      return success;
    }

    //public bool Remove(int index)
    //{
    //	bool success = false;
    //	if (index < recentFiles.Count)
    //	{
    //		string fileName = recentFiles[index];
    //		success = Remove(fileName);
    //	}
    //	return success;
    //}

    public void Save()
    {
      if (_dirty)
      {
        int c = 0;
        for (int i = 0; i < appFiles.Count; i++)
        {
          if (i < _maxFilesStore)
          {
            if (appFiles[i].Length > 0)
            {
              c++;
              string entryName = "File" + c.ToString("000");
              try
              {
                myAppKey.SetValue(entryName, appFiles[i]);
              }
              catch (Exception e1)
              {
                //!EXCEPTION HERE
              }
            } // itemName.Length > 0
          } // i < _maxFiles
        } // for loop, used items

        c = 0;
        for (int i = 0; i < suiteFiles.Count; i++)
        {
          if (i < _maxFilesStore)
          {
            if (suiteFiles[i].Length > 0)
            {
              c++;
              string entryName = "File" + c.ToString("000");
              try
              {
                mySuiteKey.SetValue(entryName, suiteFiles[i]);
              }
              catch (Exception e2)
              {
                //!EXCEPTION HERE
              }
            } // itemName.Length > 0
          } // i < _maxFiles
        } // for loop, used items

        _dirty = false;
      } // was _dirty
        //_fileCount = Math.Min(_maxFiles, recentFiles.Count);
    }

    public int FileIndex(string fileName)
    {
      int foundAt = -1;
      if (fileName.Length > 0)
      {
        try
        {
          StringComparison comparison = StringComparison.OrdinalIgnoreCase;
          // Is it already in our list?
          for (int i = 0; i < recentFiles.Count; i++)
          {
            if (foundAt < 0)
            {
              bool b = false;
              if (String.Compare(fileName, recentFiles[i], comparison) == 0) b = true;
              if (b)
              {
                foundAt = i;
                //i = _fileCount; // force early exit of for loop
                break;
              }
            }
          }
        }
        catch (Exception efi)
        {
          Fyle.BUG("MRUORama.FileIndex(" + fileName + ")", efi);
        }
      }
      return foundAt;
    }

    private int AppFileIndex(string fileName)
    {
      int foundAt = -1;
      if (fileName.Length > 0)
      {
        StringComparison comparison = StringComparison.OrdinalIgnoreCase;
        // Is it already in our list?
        for (int i = 0; i < appFiles.Count; i++)
        {
          if (foundAt < 0)
          {
            bool b = false;
            if (String.Compare(fileName, appFiles[i], comparison) == 0) b = true;
            if (b)
            {
              foundAt = i;
              //i = appFiles.Count; // force early exit of for loop
              break;
            }
          }
        }
      }
      return foundAt;
    }

    private int SuiteFileIndex(string fileName)
    {
      int foundAt = -1;
      if (fileName.Length > 0)
      {
        StringComparison comparison = StringComparison.OrdinalIgnoreCase;
        // Is it already in our list?
        for (int i = 0; i < suiteFiles.Count; i++)
        {
          if (foundAt < 0)
          {
            bool b = false;
            if (String.Compare(fileName, suiteFiles[i], comparison) == 0) b = true;
            if (b)
            {
              foundAt = i;
              //i = suiteFiles.Count; // force early exit of for loop
              break;
            }
          }
        }
      }
      return foundAt;
    }

    public bool WasFileUsed(string fileName)
    {
      bool exists = (FileIndex(fileName) >= 0);
      return exists;
    }

    public int FillFileComboBox(ComboBox combo, bool clear = true)
    {
      // (ComboBox is passed by reference)
      // Melding of FillComboBox and ValidateFiles
      // Adds abbreviated filename to the combo box only if it currently exists, but does not remove
      // non-existing files from the internal MRU ItemList in case they reappear later.

      // Reset/Rebuild the list since it may have changed
      BuildCombinedList();

      int count = 0;
      string fileName = "";
      // How many characters will fit?
      int avg = 0; int avgW = 0;
      int maxChar = 0;
      fillingCombo = true;

      try
      {
        for (int i = 64; i < 91; i++)
        {
          Size txtSize = TextRenderer.MeasureText(((char)i).ToString(), combo.Font);
          avgW += txtSize.Width;
        }
        maxChar = ((26 * combo.Width) / avgW);
      }
      catch (Exception esz)
      {
        Fyle.BUG("FillFillCombo/Size", esz);
      }

      // Go thru the combined app/suite recent list
      try
      {
        if (clear)
        { combo.Items.Clear(); }
        int i = 0;
        // Keep going while less than size of list
        while (i < recentFiles.Count)
        {
          fileName = recentFiles[i];
          string shortName = Fyle.ShortenLongPath(fileName, maxChar);
          combo.Items.Add(shortName);
          count++;
          i++;
        }
      }
      catch (Exception effc)
      {
        Fyle.BUG("MRUORama.FillFileComboBox(...)", effc);
      }

      try
      {
        if (count > 0)
        { combo.SelectedIndex = 0; }
        fillingCombo = false;
      }
      catch (Exception eqq)
      {
        //...
      }
      return count;
    }

    public bool FillingCombo
    { get { return fillingCombo; } }

    public string FindFullFilename(string abbrevFileName)
    {
      int foundAt = -1;
      string fullName = "";
      // He-He, see what I did here?  I abbreviated the word abbreviated!
      int p = abbrevFileName.IndexOf("\\…\\");

      try
      {
        if (p > 0)
        {
          // Includes last backslash?  Includes elipsis?
          string part1 = abbrevFileName.Substring(0, p);
          int p1l = part1.Length;
          // Includes elipsis?  Includes next backslash?
          string part2 = abbrevFileName.Substring(p + 2);
          int p2l = part2.Length;
          for (int i = 0; i < recentFiles.Count; i++)
          {
            if (recentFiles[i].Substring(0, p1l) == part1)
            {
              if (recentFiles[i].Substring(recentFiles[i].Length - p2l) == part2)
              {
                foundAt = i;
                fullName = recentFiles[i];
                i = recentFiles.Count; // Force exit of loop
              }
            }
          }
        }
        else
        {
          for (int i = 0; i < recentFiles.Count; i++)
          {
            if (recentFiles[i] == abbrevFileName)
            {
              foundAt = i;
              fullName = recentFiles[i];
              i = recentFiles.Count; // Force exit of loop
            }
          }

        }
      }
      catch (Exception efff)
      {
        Fyle.BUG("MRUORama.FindFullFilename(" + abbrevFileName + ")", efff);
      }
      if (fullName.Length == 0)
      {
        //! NOT FOUND!
        System.Diagnostics.Debugger.Break();
      }
      return fullName;
    }

    public int ValidateFiles()
    {
      // Go thru the list and remove any files which no longer exist
      // (see also: ValidatePaths)
      //! WARNING:
      // Any files that do not exist at the moment will be PERMENANTLY removed from the list.
      // This includes any files on a network path or removable drive which may be unavailable at the moment.
      // If that network path or drive should become available again later, those files will no longer
      // be in the list.  If you anticipate this situation occuring be wary of using this function.
      int valid = 0;
      for (int i = 0; i < recentFiles.Count; i++)
      {
        try
        {
          if (Fyle.Exists(recentFiles[i]))
          {
            valid++;
          }
          else
          {
            recentFiles.RemoveAt(i);
          }
        }
        catch (Exception ex)
        {
          recentFiles.RemoveAt(i);
        }
      }
      return valid;
    }

    public int ValidatePaths()
    {
      // Go thru the list and remove any folders which no longer exist
      // (see also: ValidateFiles)
      //! WARNING (See warning above under ValidateFiles)
      int valid = 0;
      for (int i = 0; i < recentFiles.Count; i++)
      {
        try
        {
          if (Directory.Exists(recentFiles[i]))
          {
            valid++;
          }
          else
          {
            recentFiles.RemoveAt(i);
          }
        }
        catch (Exception ex)
        {
          recentFiles.RemoveAt(i);
        }
      }
      return valid;
    }

    private string FileTypeName(FileType fileType)
    {
      string ret = "";
      switch (fileType)
      {
        case FileType.Undefined:
          ret = "Undefined";
          break;
        case FileType.Sequences: // *.lms, *.las
                                 // LOR Showtime
          ret = "Sequences";
          break;
        case FileType.Selections: // *.chlist
          ret = "Selections";
          break;
        case FileType.Maps: // *.chmap
          ret = "Maps";
          break;
        case FileType.Visualizations: // *.viz, *.lee
          ret = "Visualizations";
          break;
        case FileType.xml: // *.xml
                           // For now at least, the only xml files likely to be used by Util-O-Rama is the 
                           //   xLights 'rgbeffects.xml' file containing the model and layout data
          ret = "rgbeffects";
          break;
        case FileType.xSequence: // *.xsq
                                 // xLights
          ret = "xSequences";
          break;
      }
      return ret;
    }

  }
}
