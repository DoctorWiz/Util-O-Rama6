using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Media;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using System.DirectoryServices;
using Microsoft.Win32;
using FileHelper;
//using FuzzORama;

namespace LOR4
{
  public static class LOR4Admin
  {
    #region Constants
    public const string COPYRIGHT = "Copyright © 2021+ by Doctor 🧙 Wizard and W⚡zlights Software";
    public const int UNDEFINED = -1;
    // Note: LOR colors not in the same order as .Net or Web colors, Red and Blue are reversed (In BGR order)
    public const Int32 LORCOLOR_RED = 0x0000FF; // 255;      // 0x0000FF
    public const Int32 LORCOLOR_GRN = 0x00FF00; // 65280;    // 0x00FF00
    public const Int32 LORCOLOR_BLU = 0xFF0000; // 16711680; // 0xFF0000
    public const Int32 LORCOLOR_BLK = 0;
    public const Int32 LORCOLOR_WHT = 0xFFFFFF;
    public const int LORCOLOR_RGB = 0x000040;
    public const int LORCOLOR_MULTI = 0x404080;

    public const int ADDshimmer = 0x200;
    public const int ADDtwinkle = 0x400;

    // Trigger warnings in debug mode if centiseconds is set above or below normal values
    //   Warnings triggered only in debug mode, and are non-fatal
    // Warn if over 30 minutes in length, or under 0.98 seconds
    //                                mins * secs * cs
    public const int MAXCentiseconds = (30 * 60 * 100); // 30 minutes
    public const int MINCentiseconds = 98;             // 0.98 seconds

    public const string LOG_Error = "Error";
    public const string LOG_Info = "SeqInfo";
    public const string LOG_Debug = "Debug";
    //private const string FORMAT_DATETIME = "MM/dd/yyyy hh:mm:ss tt";
    //private const string FORMAT_FILESIZE = "###,###,###,###,##0";

    public static int nodeIndex = UNDEFINED;

    public const string TABLEchannel = "channel";
    public const string FIELDname = " name";
    public const string FIELDtype = " type";
    public const string FIELDsavedIndex = " savedIndex";
    public const string FIELDcentisecond = " centisecond";
    public const string FIELDcentiseconds = FIELDcentisecond + PLURAL;
    public const string FIELDtotalCentiseconds = " totalCentiseconds";
    public const string FIELDstartCentisecond = " startCentisecond";
    public const string FIELDendCentisecond = " endCentisecond";
    //! EXPERIMENTAL, MAY CRASH SHOWTIME
    public const string FIELDcomment = " comment";
    public const string XMLINFO = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";

    public const string FILTER_SEQ = "All Sequences *.las, *.lms|*.las;*.lms";
    public const string FILTER_CFG = "All Sequences *.las, *.lms, *.lcc|*.las;*.lms;*.lcc";
    public const string FILTER_ANY = FILTER_CFG; //? ??
    public const string FILTER_LMS = "Musical Sequence *.lms|*.lms";
    public const string FILTER_LAS = "Animated Sequence *.las|*.las";
    public const string FILTER_LCC = "Channel Configuration *.lcc|*.lcc";
    public const string FILTER_LEE = "Visualization *.lee|*.lee";
    public const string FILTER_CHLIST = "Channel List *.chList|*.chList";
    public const string FILTER_CHMAP = "Util-O-Rama Channel Map *.chMap|*.chMap";
    public const string FILTER_ALL = "All Files *.*|*.*";
    public const string FILEDESCR_CHLIST = "Util-O-Rama Channel List";
    public const string FILEDESCR_CHMAP = "Util-O-Rama Channel Map";
    public const string CSVHEAD_CHLIST = "Type, Name, SavedIndex, Universe, Channel";
    //public const string FILTER_OPEN_ANY = FILTER_SEQ + "|" + FILTER_LMS + "|" + FILTER_LAS;
    //public const string FILTER_OPEN_CFG = FILTER_CFG + "|" + FILTER_LMS + "|" + FILTER_LAS + "|" + FILTER_LCC;
    //public const string FILTER_SAVE_EITHER = FILTER_LMS + "|" + FILTER_LAS;
    public const string EXT_CHMAP = ".chmap";
    public const string EXT_CHLIST = ".chlist";
    public const string EXT_LAS = ".las";
    public const string EXT_LMS = ".lms";
    public const string EXT_LEE = ".lee";
    public const string EXT_LCC = ".lcc";


    public const string SPC = " ";
    public const string LEVEL0 = "";
    //public const string LEVEL1 = "  ";
    //public const string LEVEL2 = "    ";
    //public const string LEVEL3 = "      ";
    //public const string LEVEL4 = "        ";
    //public const string LEVEL5 = "          ";
    public const string LEVEL1 = "\t";
    public const string LEVEL2 = "\t\t";
    public const string LEVEL3 = "\t\t\t";
    public const string LEVEL4 = "\t\t\t\t";
    public const string LEVEL5 = "\t\t\t\t\t";
    public const string CRLF = "\r\n";
    // Or, if you prefer tabs instead of spaces...
    //public const string LEVEL1 = "\t";
    //public const string LEVEL2 = "\t\t";
    //public const string LEVEL3 = "\t\t\t";
    //public const string LEVEL4 = "\t\t\t\t";
    public const string PLURAL = "s";
    public const string FIELDEQ = "=\"";
    public const string ENDQT = "\"";
    public const string STFLD = "<";
    public const string ENDFLD = "/>";
    public const string FINFLD = ">";
    public const string STTBL = "<";
    public const string FINTBL = "</";
    public const string ENDTBL = ">";

    public const string COMMA = ",";
    public const string SLASH = "/";
    public const char DELIM1 = '⬖';
    public const char DELIM2 = '⬘';
    public const char DELIM3 = '⬗';
    public const char DELIM4 = '⬙';
    public const char DELIM5 = '֍';
    public const string DELIMA = "🗲";
    public const string DELIMB = "🧙";
    public const string DELIMC = "👍";
    public const string DELIMD = "🐕";
    public const string DELIME = "💡";
    private const char DELIM_Map = (char)164;  // ¤
    private const char DELIM_SID = (char)177;  // ±
    private const char DELIM_Name = (char)167;  // §
    private const char DELIM_X = (char)182;  // ¶
    private const string ROOT = "C:\\";
    private const string LOR_REGKEY = "HKEY_CURRENT_USER\\SOFTWARE\\Light-O-Rama\\Shared";
    private const string LOR_DIR = "Light-O-Rama\\";
    //private static string noisePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Noises\\";
    //private static bool gotWiz = false;
    //private static bool isWiz = false;

    private static Dictionary<int, String> colorMap = new();
    #endregion // Constants

    #region Get and Set XML Tagged Fields
    public static string HumanizeName(string XMLizedName)
    {
      // Takes a name from XML and converts symbols back to the real thing
      string ret = XMLizedName;
      ret = ret.Replace("&amp;", "&");
      ret = ret.Replace("&apos;", "'");
      ret = ret.Replace("&quot;", "\"");
      ret = ret.Replace("&lt;", "<");
      ret = ret.Replace("&gt;", ">");
      ret = ret.Replace("&#34;", "\"");
      ret = ret.Replace("&#38;", "&");
      ret = ret.Replace("&#39;", "'");
      ret = ret.Replace("&#44", ",");
      ret = ret.Replace("&#47", "/");
      ret = ret.Replace("&#60;", "<");
      ret = ret.Replace("&#62;", ">");
      ret = ret.Replace("&#9837;", "♭");
      ret = ret.Replace("&#9839;", "♯");
      return ret;
    }

    public static string XMLifyName(string HumanizedName)
    {
      // Takes a human friendly name, possibly with illegal symbols in it
      // And replaces the illegal symbols with codes to make it XML friendly
      // Also works to make things CSV and HTML friendly
      string ret = HumanizedName;
      ret = ret.Replace("\"", "&#34;");
      ret = ret.Replace(",", "&#44;");
      ret = ret.Replace("/", "&#47;");
      ret = ret.Replace("<", "&#60;");
      ret = ret.Replace(">", "&#62;");
      ret = ret.Replace("&", "&#38;");
      ret = ret.Replace("'", "&#39;");
      ret = ret.Replace("♭", "&#9837;");
      ret = ret.Replace("♯", "&#9839;");
      return ret;
    }

    // Use the EndSubstring function in Fyle - FileHelper
    /*
    public static string EndSubstring(this string s, int length)
    {
      string r = "";
      if (s.Length >= length)
      { r = s.Substring(s.Length - length); }
      else
      { r = s; }
      return r;

    }
    */

    public static int ContainsKey(string lineIn, string keyWord)
    {
      int pos = -1;
      try
      {
        if (lineIn != null)
        {
          if (lineIn.Length > 1)
          {
            string lowerLine = lineIn.ToLower();
            string lowerWord = keyWord.ToLower(); // + "="; += is handled eariler
            pos = lowerLine.IndexOf(lowerWord);
          }
        }
      }
      catch (Exception q)
      { // Ignore, return -1
      }
      return pos;
    }

    public static Int32 getKeyValue(string lineIn, string keyWord)
    {
      int p = ContainsKey(lineIn, keyWord + LOR4Admin.FIELDEQ);
      if (p >= 0)
      {
        return getKeyValue(p, lineIn, keyWord);
      }
      else
      {
        return LOR4Admin.UNDEFINED;
      }
    }

    public static int getKeyValue(int pos1, string lineIn, string keyWord)
    {
      int valueOut = UNDEFINED;
      int pos2 = UNDEFINED;
      string fooo = "";

      fooo = lineIn.Substring(pos1 + keyWord.Length + 2);
      pos2 = fooo.IndexOf("\"");
      fooo = fooo.Substring(0, pos2);
      //valueOut = Convert.ToInt32(fooo);
      bool itWorked = int.TryParse(fooo, out valueOut);
      if (!itWorked)
      {
        if (Fyle.DebugMode)
        {
          System.Diagnostics.Debugger.Break();
        }
      }

      return valueOut;
    }

    public static string getKeyWord(string lineIn, string keyWord)
    {
      int p = ContainsKey(lineIn, keyWord + LOR4Admin.FIELDEQ);
      if (p >= 0)
      {
        return getKeyWord(p, lineIn, keyWord);
      }
      else
      {
        return "";
      }

    }

    public static string getKeyWord(int pos1, string lineIn, string keyWord)
    {
      string valueOut = "";
      int pos2 = UNDEFINED;
      string fooo = "";

      fooo = lineIn.Substring(pos1 + keyWord.Length + 2);
      pos2 = fooo.IndexOf("\"");
      fooo = fooo.Substring(0, pos2);
      valueOut = fooo;

      return valueOut;
    }

    public static bool getKeyState(string lineIn, string keyWord)
    {
      int p = ContainsKey(lineIn, keyWord + LOR4Admin.FIELDEQ);
      if (p >= 0)
      {
        return getKeyState(p, lineIn, keyWord);
      }
      else
      {
        return false;
      }
    }

    public static bool getKeyState(int pos1, string lineIn, string keyWord)
    {
      bool stateOut = false;
      int pos2 = UNDEFINED;
      string fooo = "";

      fooo = lineIn.Substring(pos1 + keyWord.Length + 2);
      pos2 = fooo.IndexOf("\"");
      fooo = fooo.Substring(0, pos2).ToLower();
      if (fooo == "true") stateOut = true;
      if (fooo == "yes") stateOut = true;
      if (fooo == "1") stateOut = true;
      return stateOut;
    }

    public static string SetKey(string fieldName, string value)
    {
      StringBuilder ret = new();

      ret.Append(fieldName);
      ret.Append(FIELDEQ);
      ret.Append(value);
      ret.Append(ENDQT);

      return ret.ToString();
    }

    public static string SetKey(string fieldName, int value)
    {
      StringBuilder ret = new();

      ret.Append(fieldName);
      ret.Append(FIELDEQ);
      ret.Append(value);
      ret.Append(ENDQT);

      return ret.ToString();
    }

    public static string StartTable(string tableName, int level)
    {
      StringBuilder ret = new();

      for (int l = 0; l < level; l++)
      {
        ret.Append(LEVEL1);
      }
      ret.Append(STFLD);
      ret.Append(tableName);
      return ret.ToString();
    }

    public static string EndTable(string tableName, int level)
    {
      StringBuilder ret = new();

      for (int l = 0; l < level; l++)
      {
        ret.Append(LEVEL1);
      }
      ret.Append(LOR4Admin.FINTBL);
      ret.Append(tableName);
      ret.Append(LOR4Admin.ENDTBL);
      return ret.ToString();
    }

    #endregion // Get and Set Tagged XML Fields

    #region DisplayOrder
    public static int DisplayOrderBuildLists(LOR4Sequence seq, ref int[] savedIndexes, ref int[] levels, bool selectedOnly, bool includeRGBchildren)
    {
      //TODO: 'Selected' not implemented yet

      int count = 0;
      int c = 0;
      int level = 0;
      //int tot = seq.Tracks.Count + seq.Channels.Count + seq.ChannelGroups.Count;
      int tot = seq.Channels.Count + seq.ChannelGroups.Count;
      if (includeRGBchildren)
      {
        tot += seq.RGBchannels.Count;
      }
      else
      {
        // Why * 2? 'cus we won't show the 3 Members, but will show the parent.  3-1=2.
        tot -= (seq.RGBchannels.Count * 2);
      }
      //Array.Resize(ref savedIndexes, tot);
      //Array.Resize(ref levels, tot);




      // TEMPORARY, FOR DEBUGGING
      // int tcount = 0;
      // int gcount = 0;
      // int rcount = 0;
      // int ccount = 0;

      //const string ERRproc = " in TreeFillChannels(";
      // const string ERRtrk = "), in LOR4Track #";
      // const string ERRitem = ", Items #";
      // const string ERRline = ", Line #";

      for (int t = 1; t < seq.Tracks.Count; t++)
      {
        level = 0;
        LOR4Track theTrack = seq.Tracks[t];
        // Note the double negative here!  include it If it is selected or indeterminate
        if (!selectedOnly || (theTrack.SelectedState != CheckState.Unchecked))
        {
          DisplayOrderBuildTrack(seq, seq.Tracks[t], level, ref count, ref savedIndexes, ref levels, selectedOnly, includeRGBchildren);
          //Array.Resize(ref savedIndexes, count + 1);
          //Array.Resize(ref levels, count + 1);
          //savedIndexes[count] = theTrack.SavedIndex;
          //levels[count] = level;
          //count++;
          //c++;
        }
      }
      #region catch2 
      /*
				} // end try
				catch (System.NullReferenceException ex)
				{
					StackTrace st = new StackTrace(ex, true);
					StackFrame sf = st.GetFrame(st.FrameCount - 1);
					string emsg = ex.ToString();
					emsg += ERRproc + seq.filename + ERRtrk + t.ToString();
					emsg += ERRline + sf.GetFileLineNumber();
					#if DEBUG
						System.Diagnostics.Debugger.Break();
					#endif
					Fyle.WriteLogEntry(emsg, LOR4Admin.LOG_Error, Application.ProductName);
				}
				catch (System.InvalidCastException ex)
				{
					StackTrace st = new StackTrace(ex, true);
					StackFrame sf = st.GetFrame(st.FrameCount - 1);
					string emsg = ex.ToString();
					emsg += ERRproc + seq.filename + ERRtrk + t.ToString();
					emsg += ERRline + sf.GetFileLineNumber();
					#if DEBUG
						System.Diagnostics.Debugger.Break();
					#endif
					Fyle.WriteLogEntry(emsg, LOR4Admin.LOG_Error, Application.ProductName);
				}
				catch (Exception ex)
				{
					StackTrace st = new StackTrace(ex, true);
					StackFrame sf = st.GetFrame(st.FrameCount - 1);
					string emsg = ex.ToString();
					emsg += ERRproc + seq.filename + ERRtrk + t.ToString();
					emsg += ERRline + sf.GetFileLineNumber();
					#if DEBUG
						System.Diagnostics.Debugger.Break();
					#endif
					Fyle.WriteLogEntry(emsg, LOR4Admin.LOG_Error, Application.ProductName);
				}
				*/
      #endregion


      // Integrity Checks for debugging
      if (c != count)
      {
        string msg = "Houston, we have a problem!";
      }
      if (!selectedOnly)
      {
        if (c != tot)
        {
          string msg = "Houston, we have another problem!";
        }
      }
      return c;
    } // end fillOldChannels

    public static int DisplayOrderBuildTrack(LOR4Sequence seq, LOR4Track theTrack, int level, ref int count, ref int[] savedIndexes, ref int[] levels, bool selectedOnly, bool includeRGBchildren)
    {
      int c = 0;
      for (int ti = 0; ti < theTrack.Members.Count; ti++)
      {
        iLOR4Member member = theTrack.Members.Items[ti];
        if (member != null)
        {
          int si = member.ID;
          if (member.MemberType == LOR4MemberType.ChannelGroup)
          {
            c += DisplayOrderBuildGroup(seq, (LOR4ChannelGroup)member, level + 1, ref count, ref savedIndexes, ref levels, selectedOnly, includeRGBchildren);
          }
          if (member.MemberType == LOR4MemberType.Cosmic)
          {
            c += DisplayOrderBuildCosmic(seq, (LOR4Cosmic)member, level + 1, ref count, ref savedIndexes, ref levels, selectedOnly, includeRGBchildren);
          }
          if (member.MemberType == LOR4MemberType.RGBChannel)
          {
            c += DisplayOrderBuildRGBchannel(seq, (LOR4RGBChannel)member, level + 1, ref count, ref savedIndexes, ref levels, selectedOnly, includeRGBchildren);
          }
          if (member.MemberType == LOR4MemberType.Channel)
          {
            c += DisplayOrderBuildChannel(seq, (LOR4Channel)member, level + 1, ref count, ref savedIndexes, ref levels, selectedOnly);
          }
        } // end not null
      }
      return c;
    }

    public static int DisplayOrderBuildGroup(LOR4Sequence seq, LOR4ChannelGroup theGroup, int level, ref int count, ref int[] savedIndexes, ref int[] levels, bool selectedOnly, bool includeRGBchildren)
    {
      int c = 0;
      string nodeText = theGroup.Name;

      // Note double negative!  Include it if selected or indeterminate
      if (!selectedOnly || theGroup.SelectedState != CheckState.Unchecked)
      {
        Array.Resize(ref savedIndexes, count + 1);
        Array.Resize(ref levels, count + 1);
        savedIndexes[count] = theGroup.SavedIndex;
        levels[count] = level;
        count++;
        c++;
      }

      for (int gi = 0; gi < theGroup.Members.Count; gi++)
      {
        //try
        //{
        iLOR4Member member = theGroup.Members.Items[gi];
        int si = member.ID;
        if (member.MemberType == LOR4MemberType.ChannelGroup)
        {
          c += DisplayOrderBuildGroup(seq, (LOR4ChannelGroup)member, level + 1, ref count, ref savedIndexes, ref levels, selectedOnly, includeRGBchildren);
        }
        if (member.MemberType == LOR4MemberType.Cosmic)
        {
          c += DisplayOrderBuildCosmic(seq, (LOR4Cosmic)member, level + 1, ref count, ref savedIndexes, ref levels, selectedOnly, includeRGBchildren);
        }
        if (member.MemberType == LOR4MemberType.Channel)
        {
          c += DisplayOrderBuildChannel(seq, (LOR4Channel)member, level + 1, ref count, ref savedIndexes, ref levels, selectedOnly);
        }
        if (member.MemberType == LOR4MemberType.RGBChannel)
        {
          c += DisplayOrderBuildRGBchannel(seq, (LOR4RGBChannel)member, level + 1, ref count, ref savedIndexes, ref levels, selectedOnly, includeRGBchildren);
        }
        #region catch
        /*
	} // end try
		catch (Exception ex)
		{
			StackTrace st = new StackTrace(ex, true);
			StackFrame sf = st.GetFrame(st.FrameCount - 1);
			string emsg = ex.ToString();
			emsg += ERRproc + seq.filename + ERRgrp + groupIndex.ToString() + ERRitem + gi.ToString();
			emsg += ERRline + sf.GetFileLineNumber();
			#if DEBUG
				Debugger.Break();
			#endif
			Fyle.WriteLogEntry(emsg, LOR4Admin.LOG_Error, Application.ProductName);
		} // end catch
		*/
        #endregion

      } // End loop thru items
      return c;
    } // end AddGroup

    public static int DisplayOrderBuildCosmic(LOR4Sequence seq, LOR4Cosmic theDevice, int level, ref int count, ref int[] savedIndexes, ref int[] levels, bool selectedOnly, bool includeRGBchildren)
    {
      int c = 0;
      string nodeText = theDevice.Name;

      // const string ERRproc = " in TreeFillChannels-AddGroup(";
      // const string ERRgrp = "), in Group #";
      // const string ERRitem = ", Items #";
      // const string ERRline = ", Line #";

      if (!selectedOnly || theDevice.SelectedState != CheckState.Unchecked)
      {
        Array.Resize(ref savedIndexes, count + 1);
        Array.Resize(ref levels, count + 1);
        savedIndexes[count] = theDevice.SavedIndex;
        levels[count] = level;
        count++;
        c++;
      }

      for (int gi = 0; gi < theDevice.Members.Count; gi++)
      {
        //try
        //{
        iLOR4Member member = theDevice.Members.Items[gi];
        int si = member.ID;
        if (member.MemberType == LOR4MemberType.ChannelGroup)
        {
          c += DisplayOrderBuildGroup(seq, (LOR4ChannelGroup)member, level + 1, ref count, ref savedIndexes, ref levels, selectedOnly, includeRGBchildren);
        }
        if (member.MemberType == LOR4MemberType.Cosmic)
        {
          c += DisplayOrderBuildCosmic(seq, (LOR4Cosmic)member, level + 1, ref count, ref savedIndexes, ref levels, selectedOnly, includeRGBchildren);
        }
        if (member.MemberType == LOR4MemberType.Channel)
        {
          c += DisplayOrderBuildChannel(seq, (LOR4Channel)member, level + 1, ref count, ref savedIndexes, ref levels, selectedOnly);
        }
        if (member.MemberType == LOR4MemberType.RGBChannel)
        {
          c += DisplayOrderBuildRGBchannel(seq, (LOR4RGBChannel)member, level + 1, ref count, ref savedIndexes, ref levels, selectedOnly, includeRGBchildren);
        }
        #region catch
        /*
	} // end try
		catch (Exception ex)
		{
			StackTrace st = new StackTrace(ex, true);
			StackFrame sf = st.GetFrame(st.FrameCount - 1);
			string emsg = ex.ToString();
			emsg += ERRproc + seq.filename + ERRgrp + groupIndex.ToString() + ERRitem + gi.ToString();
			emsg += ERRline + sf.GetFileLineNumber();
			#if DEBUG
				Debugger.Break();
			#endif
			Fyle.WriteLogEntry(emsg, LOR4Admin.LOG_Error, Application.ProductName);
		} // end catch
		*/
        #endregion

      } // End loop thru items
      return c;
    } // end AddGroup

    public static int DisplayOrderBuildChannel(LOR4Sequence seq, LOR4Channel theChannel, int level, ref int count, ref int[] savedIndexes, ref int[] levels, bool selectedOnly)
    {
      int c = 0;
      // Note double negative!  Include it if selected or indeterminate*
      //    *(Groups may be indeterminate, but channels never should)
      if (!selectedOnly || theChannel.SelectedState != CheckState.Unchecked)
      {
        Array.Resize(ref savedIndexes, count + 1);
        Array.Resize(ref levels, count + 1);
        savedIndexes[count] = theChannel.SavedIndex;
        levels[count] = level;
        count++;
        c++;
      }
      return c;
    }

    public static int DisplayOrderBuildRGBchannel(LOR4Sequence seq, LOR4RGBChannel theRGB, int level, ref int count, ref int[] savedIndexes, ref int[] levels, bool selectedOnly, bool includeRGBchildren)
    {
      int c = 0;

      if (!selectedOnly || theRGB.SelectedState != CheckState.Unchecked)
      {
        Array.Resize(ref savedIndexes, count + 1);
        Array.Resize(ref levels, count + 1);
        savedIndexes[count] = theRGB.SavedIndex;
        levels[count] = level;
        count++;
        c++;
        if (includeRGBchildren)
        {
          Array.Resize(ref savedIndexes, count + 3);
          Array.Resize(ref levels, count + 3);
          // * * R E D   S U B  C H A N N E L * *
          savedIndexes[count] = theRGB.redChannel.SavedIndex;
          levels[count] = level + 1;
          count++;

          // * * G R E E N   S U B  C H A N N E L * *
          savedIndexes[count] = theRGB.grnChannel.SavedIndex;
          levels[count] = level + 1;
          count++;

          // * * B L U E   S U B  C H A N N E L * *
          savedIndexes[count] = theRGB.bluChannel.SavedIndex;
          levels[count] = level + 1;
          count++;

          c += 3;
        } // end includeRGBchildren
      }

      return c;
    }

    #endregion // Display Order

    #region ColorFunctions (Some are generic, some are LOR specific)
    public static Int32 ColorInt(Color theColor)
    {
      return theColor.ToArgb();
    }

    public static string ColorToHex(Color color)
    // Returns 7 characters in typical web color format starting with a #
    {
      return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
    }

    public static Color HexToColor(string hexCode)
    {
      Color c = Color.White;
      // Eliminate any characters before the last 6
      // This gets rid of "#" or "0x" or even "0x??" or "#??" if alpha is specified
      if (hexCode.Length > 5)
      {
        // Yes, this looks absurdly complicated.
        // I could combine most of this into one longer line of code but it wouldn't speed it
        // up because it basically breaks down to this anyway.
        // Might as well go for the obvious understandable way.
        hexCode = hexCode.Substring(hexCode.Length - 6, 6);
        string rs = hexCode.Substring(0, 2);
        string gs = hexCode.Substring(2, 2);
        string gb = hexCode.Substring(4, 2);
        int r = Convert.ToInt32(rs, 16);
        int g = Convert.ToInt32(gs, 16);
        int b = Convert.ToInt32(gb, 16);
        c = Color.FromArgb(r, g, b);
      }
      return c;
    }

    public static int ColorIcon(ImageList icons, int LORcolorVal)
    {
      return ColorIcon(icons, Color_LORtoNet(LORcolorVal));
    }

    public static int ColorIcon(ImageList icons, Color color)
    {
      int ret = -1;
      // LOR's Color Format is in BGR format, so have to reverse the Red and the Blue
      string colorID = ColorToHex(color);
      ret = icons.Images.IndexOfKey(colorID);
      if (ret < 0)
      {
        // Create a temporary working bitmap
        Bitmap bmp = new(16, 16);
        // get the graphics handle from it
        Graphics gr = Graphics.FromImage(bmp);
        // A colored solid brush to fill the middle
        SolidBrush b = new(color);
        // define a rectangle for the middle
        Rectangle r = new(2, 2, 12, 12);
        // Fill the middle rectangle with color
        gr.FillRectangle(b, r);
        // Draw a 3D button around it
        Pen p = new(Color.Black);
        gr.DrawLine(p, 1, 15, 15, 15);
        gr.DrawLine(p, 15, 1, 15, 14);
        p = new Pen(Color.DarkGray);
        gr.DrawLine(p, 2, 14, 14, 14);
        gr.DrawLine(p, 14, 2, 14, 13);
        p = new Pen(Color.White);
        gr.DrawLine(p, 0, 0, 15, 0);
        gr.DrawLine(p, 0, 1, 0, 15);
        p = new Pen(Color.LightGray);
        gr.DrawLine(p, 1, 1, 14, 1);
        gr.DrawLine(p, 1, 2, 1, 14);

        // Add it to the image list, using it's hex color code as the key
        icons.Images.Add(colorID, bmp);
        // get it's numeric index
        ret = icons.Images.Count - 1;
      }
      // Return the numeric index of the new image
      return ret;
    }

    public static string ColorName(Color color)
    {
      string name = "";

      foreach (Color c in Enum.GetValues(typeof(KnownColor)))
      {
        if (c == color)
        {
          name = c.Name;
        }
      }
      return name;
    }

    public static string NearestColorName(Color color)
    {
      string name = "";
      //TODO Fix 'Specified Cast is not valid' error!!
      /*
			if (color != null)
			{
				Int32 myARGB = color.ToArgb();
				int myR = (myARGB >> 0) & 255;
				int myG = (myARGB >> 8) & 255;
				int myB = (myARGB >> 16) & 255;

				Color match = Color.Black;
				Int32 diff = 9999999;
				foreach (Color c in Enum.GetValues(typeof(KnownColor)))
				{
					Int32 cARGB = c.ToArgb();
					int cR = (cARGB >> 0) & 255;
					int cG = (cARGB >> 8) & 255;
					int cB = (cARGB >> 16) & 255;
					int d = Math.Abs(myR - cR);
					d += Math.Abs(myG - cG);
					d += Math.Abs(myB - cB);
					if (d < diff)
					{
						match = c;
						diff = d;
						name = c.Name;
					}
				}
			}
			*/
      return name;
    }

    public static Color Color_LORtoNet(int LORcolorVal)
    {
      string colorID = Color_LORtoHTML(LORcolorVal);
      // Convert rearranged hex value a real color
      Color theColor = System.Drawing.ColorTranslator.FromHtml(colorID);
      return theColor;
    }

    public static string Color_LORtoHTML(int LORcolorVal)
    {
      string tempID = LORcolorVal.ToString("X6");
      // LOR's Color Format is in BGR format, so have to reverse the Red and the Blue
      string colorID = "#" + tempID.Substring(4, 2) + tempID.Substring(2, 2) + tempID.Substring(0, 2);
      return colorID;
    }

    public static int Color_NettoLOR(Color netColor)
    {
      int argb = netColor.ToArgb();
      string tempID = argb.ToString("X6");
      // LOR's Color Format is in BGR format, so have to reverse the Red and the Blue
      string colorID = tempID.Substring(4, 2) + tempID.Substring(2, 2) + tempID.Substring(0, 2);
      int c = int.Parse(colorID, System.Globalization.NumberStyles.HexNumber);
      return c;
    }

    public static string Color_NettoHTML(Color netColor)
    {
      int argb = netColor.ToArgb();
      string tempID = argb.ToString("X6");
      string colorID = "#" + tempID;
      return colorID;
    }

    public static Int32 Color_RGBtoLOR(int r, int g, int b)
    {
      int c = r;
      c += g * 0x100;
      c += b * 0x10000;
      return c;
    }

    public static int Color_HTMLtoLOR(string HTMLcolor)
    {
      // LOR's Color Format is in BGR format, so have to reverse the Red and the Blue
      string colorID = HTMLcolor.Substring(4, 2) + HTMLcolor.Substring(2, 2) + HTMLcolor.Substring(0, 2);
      int c = int.Parse(colorID, System.Globalization.NumberStyles.HexNumber);
      return c;
    }

    public struct RGB
    {
      private byte _r;
      private byte _g;
      private byte _b;

      public RGB(byte r, byte g, byte b)
      {
        this._r = r;
        this._g = g;
        this._b = b;
      }

      public byte R
      {
        get { return this._r; }
        set { this._r = value; }
      }

      public byte G
      {
        get { return this._g; }
        set { this._g = value; }
      }

      public byte B
      {
        get { return this._b; }
        set { this._b = value; }
      }

      public bool Equals(RGB rgb)
      {
        return (this.R == rgb.R) && (this.G == rgb.G) && (this.B == rgb.B);
      }
    }

    public struct HSV
    {
      private double _h;
      private double _s;
      private double _v;

      public HSV(double h, double s, double v)
      {
        this._h = h;
        this._s = s;
        this._v = v;
      }

      public double H
      {
        get { return this._h; }
        set { this._h = value; }
      }

      public double S
      {
        get { return this._s; }
        set { this._s = value; }
      }

      public double V
      {
        get { return this._v; }
        set { this._v = value; }
      }

      public bool Equals(HSV hsv)
      {
        return (this.H == hsv.H) && (this.S == hsv.S) && (this.V == hsv.V);
      }
    }

    public static Int32 HSVToRGB(HSV hsv)
    {
      double r = 0, g = 0, b = 0;

      if (hsv.S == 0)
      {
        r = hsv.V;
        g = hsv.V;
        b = hsv.V;
      }
      else
      {
        int i;
        double f, p, q, t;

        if (hsv.H == 360)
          hsv.H = 0;
        else
          hsv.H = hsv.H / 60;

        i = (int)Math.Truncate(hsv.H);
        f = hsv.H - i;

        p = hsv.V * (1.0 - hsv.S);
        q = hsv.V * (1.0 - (hsv.S * f));
        t = hsv.V * (1.0 - (hsv.S * (1.0 - f)));

        switch (i)
        {
          case 0:
            r = hsv.V;
            g = t;
            b = p;
            break;

          case 1:
            r = q;
            g = hsv.V;
            b = p;
            break;

          case 2:
            r = p;
            g = hsv.V;
            b = t;
            break;

          case 3:
            r = p;
            g = q;
            b = hsv.V;
            break;

          case 4:
            r = t;
            g = p;
            b = hsv.V;
            break;

          default:
            r = hsv.V;
            g = p;
            b = q;
            break;
        }

      }

      RGB x = new RGB((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
      Int32 ret = x.R * 0x10000 + x.G * 0x100 + x.B;
      return ret;
    }
    #endregion // Color Functions

    #region RenderEffects

    public static void RenderEffects(iLOR4Member member, ref PictureBox preview, bool useRamps = false, bool dividers = true)
    {
      preview.Image = RenderEffects(member, 0, member.Centiseconds, preview.Width, preview.Height, useRamps, dividers);
    }


    public static Bitmap RenderEffects(iLOR4Member member, int startCentiseconds, int endCentiseconds, int width, int height, bool useRamps = false, bool dividers = true)
    {
      // Create a temporary working bitmap
      Bitmap bmp = new Bitmap(width, height);
      if (member.MemberType == LOR4MemberType.Channel)
      {
        LOR4Channel channel = (LOR4Channel)member;
        bmp = RenderEffects(channel, startCentiseconds, endCentiseconds, width, height, useRamps, dividers);
      }
      if (member.MemberType == LOR4MemberType.RGBChannel)
      {
        LOR4RGBChannel rgb = (LOR4RGBChannel)member;
        bmp = RenderEffects(rgb, startCentiseconds, endCentiseconds, width, height, useRamps, dividers);
      }

      return bmp;

    }

    public static Bitmap RenderEffects(LOR4Channel channel, int startCentiseconds, int endCentiseconds, int width, int height, bool useRamps = false, bool dividers = true)
    {
      // Create a temporary working bitmap
      Bitmap bmp = new Bitmap(width, height);
      // get the graphics handle from it
      Graphics gr = Graphics.FromImage(bmp);
      // Paint the entire 'background' black
      gr.FillRectangle(Brushes.Silver, 0, 0, width - 1, height - 1);
      Color c = Color_LORtoNet(channel.color);
      Pen p = new Pen(c, 1);
      Brush br = new SolidBrush(c);
      //Debug.WriteLine(""); Debug.WriteLine("");
      int divSec = -1;
      if (dividers)
      {
        divSec = RenderTimeDivisions(ref bmp, startCentiseconds, endCentiseconds);
      }

      // TODO: Fix!
      //^ Time Divsions are getting COMPLETELY overwritten by effects render, even
      //^ in areas that have no effects!

      int effectCount = channel.effects.Count;
      //effectCount = 0; //! FOR DEBUGGING
      if (effectCount > 0)
      {
        int[] levels = PlotLevels(channel, startCentiseconds, endCentiseconds, width);
        for (int x = 0; x < width; x++)
        {
          Debug.Write(levels[x].ToString() + " ");
          bool shimmer = ((levels[x] & LOR4Admin.ADDshimmer) > 0);
          bool twinkle = ((levels[x] & LOR4Admin.ADDtwinkle) > 0);
          levels[x] &= 0x0FF;
          if (useRamps)
          {
            //int lineLen = levels[x] * Convert.ToInt32((float)height / 100D);
            int ll = levels[x] * height;
            int lineLen = ll / 100 + 1;
            if (shimmer)
            {
              for (int n = 0; n < lineLen; n++)
              {
                int m = (n + x) % 6;
                if (m < 2)
                {
                  gr.FillRectangle(br, x, height - n, 1, 1);
                }
              }
            }
            else if (twinkle)
            {
              for (int n = 0; n < lineLen; n++)
              {
                int m = (n + x) % 6;
                if (m < 1)
                {
                  gr.FillRectangle(br, x, height - n, 1, 1);
                }
                m = (x - n + 25000) % 6;
                if (m < 1)
                {
                  gr.FillRectangle(br, x, height - n, 1, 1);
                }

              }
            }
            else
            {
              gr.DrawLine(p, x, height - 1, x, height - lineLen);
            }
          }
          else // use fades instead of ramps
          {
            int R = (Color.Silver.R * (100 - levels[x]) / 100) + (c.R * levels[x] / 100);
            int G = (Color.Silver.G * (100 - levels[x]) / 100) + (c.G * levels[x] / 100);
            int B = (Color.Silver.B * (100 - levels[x]) / 100) + (c.B * levels[x] / 100);
            Color d = Color.FromArgb(R, G, B);
            p = new Pen(d, 1);
            br = new SolidBrush(d);
            if (shimmer)
            {
              for (int n = 0; n < height; n++)
              {
                int m = (n + x) % 6;
                if (m < 2)
                {
                  gr.FillRectangle(br, x, height - n, 1, 1);
                }
              }
            }
            else if (twinkle)
            {
              for (int n = 0; n < height; n++)
              {
                int m = (n + x) % 6;
                if (m < 1)
                {
                  gr.FillRectangle(br, x, height - n, 1, 1);
                }
                m = (x - n + 25000) % 6;
                if (m < 1)
                {
                  gr.FillRectangle(br, x, height - n, 1, 1);
                }

              }
            }
            else
            {
              gr.DrawLine(p, x, 0, x, height - 1);
            }
          }
        }

        if (dividers)
        {
          // Label the division marks
          string msg = divSec.ToString() + " sec per div";
          // Switch to 'DimGray' for the text label
          //   (Which is darker than regular 'Gray', and darker than
          //    'DarkGray' used for the timings (go figure!))
          c = Color.DarkSlateGray;
          br = new SolidBrush(c);
          p = new Pen(c, 1);
          br = new SolidBrush(c);
          Rectangle boxrect = new Rectangle(0, 0, width, height);
          Font fnt = new Font("Arial", 8, FontStyle.Regular);
          //Font itfnt = new Font("Arial", 8, FontStyle.Italic);
          StringFormat strfmt = new StringFormat();
          strfmt.Alignment = StringAlignment.Near; // Left
          strfmt.LineAlignment = StringAlignment.Near; // Top
          gr.DrawString(msg, fnt, br, boxrect, strfmt);

        }

      }
      else // NO effects!
      {
        //Size boxsz = new Size(width, height);
        Rectangle boxrect = new Rectangle(0, 0, width, height);
        Font fnt = new Font("Arial", 8, FontStyle.Italic);
        string msg = "(No Effects)";
        //TextFormatFlags flags = TextFormatFlags.NoPadding;
        //Size txtsize = TextRenderer(gr, msg, fnt, boxsz);
        //int x = (width - txtsize.Width) / 2;
        //int y = (height - txtsize.Height) / 2;
        br = new SolidBrush(Color.Black);
        StringFormat strfmt = new StringFormat();
        strfmt.Alignment = StringAlignment.Center;
        strfmt.LineAlignment = StringAlignment.Center;
        gr.DrawString(msg, fnt, br, boxrect, strfmt);

        fnt.Dispose();

      } // end channel has effects, or not
      br.Dispose();
      p.Dispose();
      gr.Dispose();

      return bmp;
    }

    public static void RenderTimings(LOR4Timings grid, ref PictureBox preview)
    {
      preview.Image = RenderTimings(grid, 0, grid.Centiseconds, preview.Width, preview.Height);
    }

    public static Bitmap Original_RenderTimings(LOR4Timings grid, int startCentiseconds, int endCentiseconds, int width, int height)
    {
      //! I really need to rethink how this works!  Typically get a solid mess of tick marks
      int h12 = height / 2;
      int h13 = height / 3;
      // Create a temporary working bitmap
      Bitmap bmp = new Bitmap(width, height);
      // get the graphics handle from it
      Graphics gr = Graphics.FromImage(bmp);
      // Paint the entire 'background' pale yellow
      Brush br = new SolidBrush(Color.FromArgb(240, 240, 128));
      gr.FillRectangle(br, 0, 0, width - 1, height - 1);
      Color c = Color.Black;
      Pen p = new Pen(c, 1);
      br = new SolidBrush(c);
      int totalCS = endCentiseconds - startCentiseconds;
      float q = totalCS / width;
      //Debug.WriteLine(""); Debug.WriteLine("");
      int div = 1500;
      //? TODO: Do I need to change these magic numbers to something based on the preview size?!?!?
      if (totalCS < 3000) div = 100;
      if (totalCS > 30000) div = 3000;

      // Taller, thicker tick Markers every 30 seconds
      int n30 = totalCS / div;

      for (int cx = startCentiseconds; cx < endCentiseconds; cx += div)
      {
        float xf = (cx - startCentiseconds) / q;
        int x = (int)Math.Round(xf, 0);
        gr.DrawLine(p, x, h13, x, height - 1);
        gr.DrawLine(p, x + 1, h13, x + 1, height - 1);
      }

      if (grid.timings.Count > 0)
      {
        for (int i = 0; i < grid.timings.Count; i++)
        {
          int t = grid.timings[i];
          if ((t >= startCentiseconds) && (t <= endCentiseconds))
          {
            float xf = (t - startCentiseconds) / q;
            int x = (int)Math.Round(xf, 0);
            gr.DrawLine(p, x, h12, x, height - 1);
          } // End timing in range
        } // End loop thru timings
      } // End grid has timings

      return bmp;
    }

    public static Bitmap RenderTimings(LOR4Timings grid, int startCentiseconds, int endCentiseconds, int width, int height)
    {
      const int csPerSec = 100;
      int minPixPerMark = 3;
      int h13 = (int)Math.Round(height / 0.666666D);  // Start 2/3 down, One-Third length
      int h12 = height / 2;                           // Start 1/2 down, Half Length
      int h23 = height / 3;                           // Start 1/3 down, Two-Thirds length
      int h34 = (int)Math.Round(height / 0.25D);      // Start 1/4 down, Three-Quarters length

      // Create a temporary working bitmap
      Bitmap bmp = new Bitmap(width, height);
      // get the graphics handle from it
      Graphics gr = Graphics.FromImage(bmp);
      // Paint the entire 'background' pale yellow
      Brush br = new SolidBrush(Color.FromArgb(240, 240, 128));
      gr.FillRectangle(br, 0, 0, width - 1, height - 1);
      // Start with 'DarkGray' for the seconds divisions
      //   (Which is lighter than regular 'Gray', and lighter than
      //    'DimGray' used for the timings (go figure!))
      Color c = Color.DarkGray;
      Pen p = new Pen(c, 1);
      br = new SolidBrush(c);
      Rectangle boxrect = new Rectangle(0, 0, width, height);
      Font fnt = new Font("Arial", 8, FontStyle.Regular);
      Font itfnt = new Font("Arial", 8, FontStyle.Italic);
      StringFormat strfmt = new StringFormat();
      string msg = "";
      int divSec = 0;

      // Is it an Empty grid?  No timings, or only 1 timing at 0?
      bool empty = false;
      if (grid.timings.Count < 1)
      { empty = true; }
      else
      {
        if (grid.timings.Count == 1)
        {
          if (grid.timings[0] == 0)
          {
            empty = true;
          }
        }
      } // end is the grid empty?

      // Assuming fairly evenly spaced timings---
      // Dunno how the heck I would handle clusters!
      int totalCS = endCentiseconds - startCentiseconds;
      string info = FormatTime(totalCS);
      if (grid.timings.Count > (width / minPixPerMark))
      {
        // Toooo many timings to display without some magic fudging...
        // How much of it can we show without jamming them together

        // How many ticks can we show?
        int maxTicks = width / minPixPerMark;
        // What percentage of the song would that cover?
        double percentage = (double)maxTicks / (double)grid.timings.Count;
        // Round down to 2 decimal places
        int pp = (int)(percentage * 100);
        double pctRound = (pp / 100D);
        // How many centiseconds is that?
        int lengthCS = (int)(pctRound * totalCS);
        int newcs = lengthCS;
        // If more than 30 seconds
        if (lengthCS > 30 * csPerSec)
        {
          // Round that down to the nearest 10 seconds
          newcs = (lengthCS / (10 * csPerSec)) * 10 * csPerSec;
        }
        else // Less than 30 seconds
        {
          // Round that down to the nearest second
          newcs = (lengthCS / csPerSec) * csPerSec;
        }
        // Set our new end and length
        endCentiseconds = startCentiseconds + newcs;
        totalCS = endCentiseconds - startCentiseconds;
        info = FormatTime(totalCS);
      }

      // Next problem, how long is it and can we do 1 second dividers?
      //( works up to about 5 minutes)
      divSec = 1;
      int maxCount = width / minPixPerMark;
      int divCount = (totalCS / (divSec * csPerSec));
      if (divCount > maxCount)
      {
        // No?, Too many?, can we do 5 second dividers?
        divSec = 5;
        divCount = (totalCS / (divSec * csPerSec));
        if (divCount > maxCount)
        {
          // No?, Still too many?, how about 10 second dividers?
          divSec = 10;
          divCount = (totalCS / (divSec * csPerSec));
          if (divCount > maxCount)
          {
            // Really?, How long is this thing?, can we manage 30 second dividers?
            divSec = 30;
            divCount = (totalCS / (divSec * csPerSec));
            if (divCount > maxCount)
            {
              // Something is probably wrong...
              Fyle.BUG("Is this sequence really too long for 30 second dividers?  cs=" + totalCS.ToString());
              divSec = -1;
            }
          }
        }
      }

      // Valid spaces for dividers
      if (divSec > 0)
      {
        // Get Pixels-per-Centisecond and Centiseconds-per-Pixel Horizontal
        double CSperPixel = ((double)totalCS / width);
        double pixelsPerCS = ((double)width / totalCS);
        // How many pixels is that per timing division?
        double divSpace = divSec * pixelsPerCS * csPerSec;
        // Sanity check for debugging, remark out once known valid
        //if (divSpace < 2.5)
        //{ Fyle.BUG("Spacing Problem-- Too Small!"); }
        //if (divSpace > 30)
        //{ Fyle.BUG("Possible Spacing Problem-- Too Big!"); }
        // Draw the timing divisions!
        for (int t = 1; t < divCount; t++)
        {
          int tx = (int)Math.Round(t * divSpace);
          gr.DrawLine(p, tx, 0, tx, height - 1);
        }

        if (!empty)
        {
          // Now draw the timing marks
          // Switch from [Whatever]Gray to Black for the timing marks
          c = Color.Black;
          p = new Pen(c, 1);
          br = new SolidBrush(c);
          for (int t = 0; t < grid.timings.Count; t++)
          {
            int csx = grid.timings[t];
            if (csx > startCentiseconds)
            {
              if (csx <= endCentiseconds)
              {
                int tx = (int)Math.Round(csx * pixelsPerCS);
                //TODO: Get Divisions working the way I want first, then get this working
                // Start at halfway, and draw downwards
                gr.DrawLine(p, tx, h12, tx, height - 1);
              }
              else
              {
                // Break out of loop
                //t = grid.timings.Count;
                break;
              }
            }
          }
        }
        else // Empty!
        {
          // Grid is empty!
          msg = "[ No Timings ]";
          br = new SolidBrush(Color.Navy);
          strfmt.Alignment = StringAlignment.Center;
          strfmt.LineAlignment = StringAlignment.Far; // Bottom
          gr.DrawString(msg, itfnt, br, boxrect, strfmt);
        } // end empty, or not




        // Label the division marks
        msg = divSec.ToString() + " sec per div";
        // Switch to 'DimGray' for the text label
        //   (Which is darker than regular 'Gray', and darker than
        //    'DarkGray' used for the timings (go figure!))
        br = new SolidBrush(Color.DimGray);
        strfmt.Alignment = StringAlignment.Near; // Left
        strfmt.LineAlignment = StringAlignment.Near; // Top
        gr.DrawString(msg, fnt, br, boxrect, strfmt);

        // Label the end time
        msg = FormatTime(endCentiseconds) + " sec→";
        // If we are displaying less than a minute, but the sequence is over a minute
        if ((endCentiseconds < 60 * csPerSec) && (grid.Centiseconds >= 60 * csPerSec))
        {
          // add leading zero
          msg = "0:" + msg;
        }
        // Hmmmm, what color looks good here...
        br = new SolidBrush(Color.Indigo);
        strfmt.Alignment = StringAlignment.Far; // Right
        gr.DrawString(msg, fnt, br, boxrect, strfmt);
      }
      else // divSec < 1
      {
        // Error!  Could not figure out how to do it
        //   Possibly because the sequence is too long (?)
        msg = "[ Cannot display timings! ]";
        br = new SolidBrush(Color.DimGray);
        strfmt.Alignment = StringAlignment.Center;
        strfmt.LineAlignment = StringAlignment.Center;
        gr.DrawString(msg, itfnt, br, boxrect, strfmt);
      }// end Valid Divisions, or not

      return bmp;
    }

    private static int RenderTimeDivisions(ref Bitmap bmp, int startCentiseconds, int endCentiseconds)
    {
      const int csPerSec = 100;
      const int minDivWidth = 7;
      Graphics gr = Graphics.FromImage(bmp);
      // Start with 'DarkGray' for the divisions
      //   (Which is lighter than regular 'Gray' (go figure!))
      //Color c = Color.DimGray;
      Color c = Color.Magenta; // Temp, for debugging
      Pen p = new Pen(c, 1);
      int width = bmp.Width;
      int height = bmp.Height;
      int totalCS = endCentiseconds - startCentiseconds;
      int maxDivs = width / minDivWidth;
      // Start with 1 sec per div
      int divSec = 1;
      int divCount = totalCS / (divSec * csPerSec);
      if (divCount > maxDivs)
      {
        // Too dense eh?, lets try 5 seconds
        divSec = 5;
        divCount = totalCS / (divSec * csPerSec);
        if (divCount > maxDivs)
        {
          // Still too dense?, lets try 10 seconds
          divSec = 10;
          divCount = totalCS / (divSec * csPerSec);
          if (divCount > maxDivs)
          {
            // Still too dense?, how about 15 seconds
            divSec = 15;
            divCount = totalCS / (divSec * csPerSec);
            if (divCount > maxDivs)
            {
              // Lets try 30 seconds
              divSec = 30;
              divCount = totalCS / (divSec * csPerSec);
              if (divCount > maxDivs)
              {
                // Still too dense?, How about a minute?
                divSec = 60;
                divCount = totalCS / (divSec * csPerSec);
                if (divCount > maxDivs)
                {
                  // Really?  Is the grid actually that effing long?
                  Fyle.BUG("Is the song really that friggin' long?");
                }
              }
            }
          }
        }
      }

      // Get Pixels-per-Centisecond and Centiseconds-per-Pixel Horizontal
      double CSperPixel = ((double)totalCS / width);
      double pixelsPerCS = ((double)width / totalCS);
      // How many pixels is that per timing division?
      double divSpace = divSec * pixelsPerCS * csPerSec;

      for (int t = 1; t < divCount; t++)
      {
        int tx = (int)Math.Round(t * divSpace);
        gr.DrawLine(p, tx, 0, tx, height - 1);
      }
      return divSec;
    }

    public static Bitmap RenderEffects(LOR4RGBChannel rgb, int startCentiseconds, int endCentiseconds, int width, int height, bool useRamps = false, bool dividers = true)
    {
      // Create a temporary working bitmap
      Bitmap bmp = new Bitmap(width, height);
      // get the graphics handle from it
      Graphics gr = Graphics.FromImage(bmp);
      // Paint the entire 'background' black
      //gr.FillRectangle(Brushes.Black, 0, 0, width - 1, height - 1);

      int[] rLevels = null;
      Array.Resize(ref rLevels, width);
      int[] gLevels = null;
      Array.Resize(ref gLevels, width);
      int[] bLevels = null;
      Array.Resize(ref bLevels, width);
      int thirdHt = height / 3;
      int toteffcount = rgb.redChannel.effects.Count +
                        rgb.grnChannel.effects.Count +
                        rgb.bluChannel.effects.Count;
      int divSec = -1;
      if (dividers)
      {
        divSec = RenderTimeDivisions(ref bmp, startCentiseconds, endCentiseconds);
      }




      if (toteffcount > 0)
      {
        if (rgb.redChannel.effects.Count > 0)
        {
          rLevels = PlotLevels(rgb.redChannel, startCentiseconds, endCentiseconds, width);
        }
        if (rgb.grnChannel.effects.Count > 0)
        {
          gLevels = PlotLevels(rgb.grnChannel, startCentiseconds, endCentiseconds, width);
        }
        if (rgb.bluChannel.effects.Count > 0)
        {
          bLevels = PlotLevels(rgb.bluChannel, startCentiseconds, endCentiseconds, width);
        }

        for (int x = 0; x < width; x++)
        {
          //Debug.Write(levels[x].ToString() + " ");
          //bool shimmer = ((levels[x] & LOR4Admin.ADDshimmer) > 0);
          //bool twinkle = ((levels[x] & LOR4Admin.ADDtwinkle) > 0);
          //levels[x] &= 0x0FF;
          if (useRamps)
          {
            // * * R E D * *
            Pen p = new Pen(Color.Red, 1);
            Brush br = new SolidBrush(Color.Red);
            bool shimmer = ((rLevels[x] & LOR4Admin.ADDshimmer) > 0);
            bool twinkle = ((rLevels[x] & LOR4Admin.ADDtwinkle) > 0);
            rLevels[x] &= 0x0FF;
            int ll = rLevels[x] * thirdHt;
            int lineLen = ll / 100 + 1;
            if (shimmer)
            {
              for (int n = 0; n < lineLen; n++)
              {
                int m = (n + x) % 6;
                if (m < 2)
                {
                  gr.FillRectangle(br, x, height - n, 1, 1);
                }
              }
            }
            else if (twinkle)
            {
              for (int n = 0; n < lineLen; n++)
              {
                int m = (n + x) % 6;
                if (m < 1)
                {
                  gr.FillRectangle(br, x, height - n, 1, 1);
                }
                m = (x - n + 25000) % 6;
                if (m < 1)
                {
                  gr.FillRectangle(br, x, height - n, 1, 1);
                }

              }
            }
            else
            {
              gr.DrawLine(p, x, height - 1, x, height - lineLen);
            }
            // END RED

            // * * G R E E N * *
            p = new Pen(Color.Green, 1);
            br = new SolidBrush(Color.Green);
            shimmer = ((rLevels[x] & LOR4Admin.ADDshimmer) > 0);
            twinkle = ((rLevels[x] & LOR4Admin.ADDtwinkle) > 0);
            rLevels[x] &= 0x0FF;
            ll = rLevels[x] * thirdHt;
            lineLen = ll / 100 + 1;
            if (shimmer)
            {
              for (int n = 0; n < lineLen; n++)
              {
                int m = (n + x) % 6;
                if (m < 2)
                {
                  gr.FillRectangle(br, x, thirdHt + height - n, 1, 1);
                }
              }
            }
            else if (twinkle)
            {
              for (int n = 0; n < lineLen; n++)
              {
                int m = (n + x) % 6;
                if (m < 1)
                {
                  gr.FillRectangle(br, x, thirdHt + height - n, 1, 1);
                }
                m = (x - n + 25000) % 6;
                if (m < 1)
                {
                  gr.FillRectangle(br, x, thirdHt + height - n, 1, 1);
                }

              }
            }
            else
            {
              gr.DrawLine(p, x, height - 1, x, thirdHt + height - lineLen);
            }
            // END GREEN

            // * * B L U E * *
            p = new Pen(Color.Red, 1);
            br = new SolidBrush(Color.Red);
            shimmer = ((rLevels[x] & LOR4Admin.ADDshimmer) > 0);
            twinkle = ((rLevels[x] & LOR4Admin.ADDtwinkle) > 0);
            rLevels[x] &= 0x0FF;
            ll = rLevels[x] * thirdHt;
            lineLen = ll / 100 + 1;
            if (shimmer)
            {
              for (int n = 0; n < lineLen; n++)
              {
                int m = (n + x) % 6;
                if (m < 2)
                {
                  gr.FillRectangle(br, x, thirdHt + thirdHt + height - n, 1, 1);
                }
              }
            }
            else if (twinkle)
            {
              for (int n = 0; n < lineLen; n++)
              {
                int m = (n + x) % 6;
                if (m < 1)
                {
                  gr.FillRectangle(br, x, thirdHt + thirdHt + height - n, 1, 1);
                }
                m = (x - n + 25000) % 6;
                if (m < 1)
                {
                  gr.FillRectangle(br, x, height - n, 1, 1);
                }

              }
            }
            else
            {
              gr.DrawLine(p, x, height - 1, x, thirdHt + thirdHt + height - lineLen);
            }
            // END BLUE

          }
          else // use fades instead of ramps
          {
            int R = rLevels[x];
            int G = gLevels[x];
            int B = bLevels[x];

            // Shimmer and Twinkle
            if (R >= ADDtwinkle)
            {
              R -= ADDtwinkle;
              R /= 2;
            }
            if (R >= ADDshimmer)
            {
              R -= ADDshimmer;
              R /= 2;
            }
            if (R > 100)
            {
              R = 100;
            }
            if (G >= ADDtwinkle)
            {
              G -= ADDtwinkle;
              G /= 2;
            }
            if (G >= ADDshimmer)
            {
              G -= ADDshimmer;
              G /= 2;
            }
            if (G > 100)
            {
              G = 100;
            }
            if (B >= ADDtwinkle)
            {
              B -= ADDtwinkle;
              B /= 2;
            }
            if (B >= ADDshimmer)
            {
              B -= ADDshimmer;
              B /= 2;
            }
            if (B > 100)
            {
              B = 100;
            }

            int r = (int)((float)R * 2.55F);
            int g = (int)((float)G * 2.55F);
            int b = (int)((float)B * 2.55F);



            Color c = Color.FromArgb(r, g, b);
            Pen p = new Pen(c, 1);
            gr.DrawLine(p, x, 0, x, height - 1);
          }
        } // end For X Coord (Horiz)

        if (dividers)
        {
          // Label the division marks
          string msg = divSec.ToString() + " sec per div";
          // Switch to 'DimGray' for the text label
          //   (Which is darker than regular 'Gray', and darker than
          //    'DarkGray' used for the timings (go figure!))
          Color c = Color.DarkSlateGray;
          SolidBrush br = new SolidBrush(c);
          Pen p = new Pen(c, 1);
          br = new SolidBrush(c);
          Rectangle boxrect = new Rectangle(0, 0, width, height);
          Font fnt = new Font("Arial", 8, FontStyle.Regular);
          //Font itfnt = new Font("Arial", 8, FontStyle.Italic);
          StringFormat strfmt = new StringFormat();
          strfmt.Alignment = StringAlignment.Near; // Left
          strfmt.LineAlignment = StringAlignment.Near; // Top
          gr.DrawString(msg, fnt, br, boxrect, strfmt);

        }



      }
      else // NO effects on any of the 3 subchannels
      {
        Rectangle boxrect = new Rectangle(0, 0, width, height);
        Font fnt = new Font("Arial", 8, FontStyle.Italic);
        string msg = "(No Effects)";
        Brush br = new SolidBrush(Color.White);
        StringFormat strfmt = new StringFormat();
        strfmt.Alignment = StringAlignment.Center;
        strfmt.LineAlignment = StringAlignment.Center;
        gr.DrawString(msg, fnt, br, boxrect, strfmt);
      } // End effects count, or not
      return bmp;
    }

    public static int[] PlotLevels(LOR4Channel channel, int startCentiseconds, int endCentiseconds, int width)
    {
      int[] levels = null;
      Array.Resize(ref levels, width);

      int totalCentiseconds = endCentiseconds - startCentiseconds + 1;
      // centisecondsPerPixel = totalCentiseconds / width;
      float cspp = (float)totalCentiseconds / (float)width;
      // int curCs = 0;
      // int lastCs = 0;
      int curLevel = 0;
      int effectIdx = 0;
      bool keepGoing = true;
      int thisClik = 0;
      int nextClik = width;
      LOR4Effect curEffect = channel.effects[effectIdx];


      if (cspp >= 1.0F)
      {
        for (int x = 0; x < width; x++)
        {
          if (Fyle.DebugMode)
          {
            if (x == 42)
            {
              //System.Diagnostics.Debugger.Break();
            }
          }

          keepGoing = true;
          while (keepGoing)
          {
            // at how many centiseconds does this column represent
            //     Convert.ToInt provides rounding
            //      Whereas casting with (int) does not  (per StackExchange)
            thisClik = Convert.ToInt32(cspp * (float)x);
            // if not to the end
            if (x < width - 2)
            {
              // how many centiseconds does the next column represent
              nextClik = Convert.ToInt32(cspp * ((float)(x + 1)));
            }
            else // at the end
            {
              nextClik = width;
            }
            // does the current effect start at or before this time slice?
            if (thisClik >= curEffect.startCentisecond)
            {
              // does it end at or after this time slice?
              if (thisClik <= curEffect.endCentisecond)
              {
                // We Got One!
                keepGoing = false;
                // This is the current effect at this time
                if (curEffect.EffectTypeEX == LOR4EffectType.Constant)
                {
                  curLevel = curEffect.Intensity;
                }
                if (curEffect.EffectTypeEX == LOR4EffectType.Shimmer)
                {
                  curLevel = curEffect.Intensity | ADDshimmer;
                }
                if (curEffect.EffectTypeEX == LOR4EffectType.Twinkle)
                {
                  curLevel = curEffect.Intensity | ADDtwinkle;
                }
                if ((curEffect.EffectTypeEX == LOR4EffectType.FadeDown) ||
                    (curEffect.EffectTypeEX == LOR4EffectType.FadeUp))
                {
                  // Amount of difference in level, from start to end
                  int levelDiff = curEffect.endIntensity - curEffect.startIntensity;
                  // lenth of the effect
                  int effLen = curEffect.endCentisecond - curEffect.startCentisecond;
                  // how far we currently are from/past the start of the effect - in centiseconds
                  int csFromStart = thisClik - curEffect.startCentisecond;
                  // how far is that, as related to the length of the effect, expressed as 0 to 1
                  float amtThru = (float)csFromStart / (float)effLen;
                  // New relative level is the level difference times how relatively far we are thru it
                  float newLev = 0F;
                  // is it a fade-out/down?
                  if (levelDiff < 0)
                  {
                    // Get inverse of amount thru
                    //amtThru = 1F - amtThru;
                    // make difference positive
                    //levelDiff *= -1;
                  }
                  // New relative level is the level difference times how relatively far we are thru it
                  newLev = (float)levelDiff * amtThru;
                  // add the base starting level to get the current level at this slice in time
                  curLevel = curEffect.startIntensity + Convert.ToInt32(newLev);
                }
              }
              else // we are past the end of this effect
              {
                // until we figure out otherwise - assume we are BETWEEN effects
                curLevel = 0;
                // are there more effects?
                if (effectIdx < (channel.effects.Count - 1))
                {                   // move to next effect
                  effectIdx++;
                  curEffect = channel.effects[effectIdx];
                  // does it end before this time slice
                  while (curEffect.endCentisecond < cspp)
                  {
                    if (effectIdx < (channel.effects.Count - 1))
                    {
                      // move to next effect
                      effectIdx++;
                      curEffect = channel.effects[effectIdx];
                    }
                    else
                    {
                      curEffect = new LOR4Effect();
                      curEffect.startCentisecond = endCentiseconds + 1;
                      curEffect.endCentisecond = endCentiseconds + 2;
                    } // if more effects remain
                  } // end while(currentEffect End < this time slice)
                } // end there are more effects left
                else
                {
                  keepGoing = false;
                }
              } // end curent effect ends at or before this time slice
            }
            else
            {
              keepGoing = false;
            } // end current effect starts at or before this time slice
          } // end while(keepGoing) loop looking at effects
          if (curLevel > 100)
          {
            string msg = "WTF? More than 100%";
            if (Fyle.DebugMode)
            {
              //stem.Diagnostics.Debugger.Break();
            }
          }
          levels[x] = curLevel;
        } // end for loop (columns/pixels, horizontal x left to right)
      } // end muliple centiseconds per pixel
      else
      {
        // mulitple pixels per centisecond
        //TODO
      } // end pixels to centiseconds ratio
      return levels;
    }

    #endregion // Render Effects

    #region FastIndexOf
    public static int FastIndexOf(string source, string pattern)
    {
      if (pattern == null) throw new ArgumentNullException();
      if (pattern.Length == 0) return 0;
      if (pattern.Length == 1) return source.IndexOf(pattern[0]);
      bool found;
      int limit = source.Length - pattern.Length + 1;
      if (limit < 1) return -1;
      // Store the first 2 characters of "pattern"
      char c0 = pattern[0];
      char c1 = pattern[1];
      // Find the first occurrence of the first character
      int first = source.IndexOf(c0, 0, limit);
      while (first != -1)
      {
        // Check if the following character is the same like
        // the 2nd character of "pattern"
        if (source[first + 1] != c1)
        {
          first = source.IndexOf(c0, ++first, limit - first);
          continue;
        }
        // Check the rest of "pattern" (starting with the 3rd character)
        found = true;
        for (int j = 2; j < pattern.Length; j++)
          if (source[first + j] != pattern[j])
          {
            found = false;
            break;
          }
        // If the whole word was found, return its index, otherwise try again
        if (found) return first;
        first = source.IndexOf(c0, ++first, limit - first);
      }
      return -1;
    }
    #endregion // FastIndexOf

    #region LOR Specific File & Directory Functions

    public static string SequenceEditor
    {
      get
      {
        string exe = "";
        string root = ROOT;
        try
        {
          string ky = "HKEY_CLASSES_ROOT\\lms_auto_file\\shell\\open";
          exe = (string)Registry.GetValue(ky, "command", root);
          exe = exe.Replace(" \"%1\"", "");
          if (exe == null)
          {
            exe = "C:\\Program Files (x86)\\Light-O-Rama\\LORSequenceEditor.exe";
          }
          if (exe.Length < 10)
          {
            exe = "C:\\Program Files (x86)\\Light-O-Rama\\LORSequenceEditor.exe";
          }
          bool valid = Fyle.Exists(exe);
          if (!valid)
          {
            exe = "";
          }
        }
        catch
        {
          exe = "";
        }
        return exe;

      }
    }

    public static string DefaultUserDataPath
    {
      get
      {
        string fldr = "";
        string root = ROOT;
        string userDocs = Fyle.DefaultDocumentsPath;
        try
        {
          fldr = (string)Registry.GetValue(LOR_REGKEY, "UserDataPath", root);
          if (fldr.Length < 6)
          {
            fldr = userDocs + LOR_DIR;
          }
          bool valid = Directory.Exists(fldr); // Fyle.IsValidPath(fldr);
          if (!valid)
          {
            fldr = userDocs + LOR_DIR;
          }
          if (!Directory.Exists(fldr))
          {
            Directory.CreateDirectory(fldr);
          }
        }
        catch
        {
          fldr = userDocs;
        }
        return fldr;
      } // End get UserDataPath
    }

    public static string DefaultAuthor
    {
      get
      {
        string author = "[Unknown Author]";
        string root = ROOT;
        try
        {
          string ky = "HKEY_CURRENT_USER\\Software\\Light-O-Rama\\Editor\\NewSequence";
          author = (string)Registry.GetValue(ky, "Author", root);
          if (author == null)
          {
            // If key does not exist, string will be NULL instead of empty
            author = "";
          }
          if (author.Length < 2)
          {
            ky = LOR_REGKEY + "\\Licensing";
            author = (string)Registry.GetValue(ky, "LicenseName", root);
            if (author.Length < 2)
            {
              // Fallback Failsafe
              author = Fyle.WindowsUsername;
            }
          }
        }
        catch
        {
          author = Fyle.WindowsUsername;
        }
        return author;
      } // End get UserDataPath
    }

    public static string DefaultNonAudioPath
    {
      // AKA Sequences Folder
      get
      {
        string fldr = "";
        string root = ROOT;
        string userDocs = Fyle.DefaultDocumentsPath;
        try
        {
          fldr = (string)Registry.GetValue(LOR_REGKEY, "NonAudioPath", root);
          if (fldr.Length < 6)
          {
            fldr = DefaultUserDataPath + "Sequences\\";
          }
          bool valid = Fyle.IsValidPath(fldr);
          if (!valid)
          {
            fldr = DefaultUserDataPath + "Sequences\\";
          }
          if (!Directory.Exists(fldr))
          {
            Directory.CreateDirectory(fldr);
          }
        }
        catch
        {
          fldr = userDocs;
        }
        return fldr;
      } // End get NonAudioPath (Sequences)
    }

    public static string DefaultVisualizationsPath
    {
      get
      {
        string fldr = "";
        string root = ROOT;
        string userDocs = Fyle.DefaultDocumentsPath;
        try
        {
          fldr = (string)Registry.GetValue(LOR_REGKEY, "VisualizationsPath", root);
          if (fldr.Length < 6)
          {
            fldr = DefaultUserDataPath + "Visualizations\\";
          }
          bool valid = Fyle.IsValidPath(fldr);
          if (!valid)
          {
            fldr = DefaultUserDataPath + "Visualizations\\";
          }
          if (!Directory.Exists(fldr))
          {
            Directory.CreateDirectory(fldr);
          }
        }
        catch
        {
          fldr = userDocs;
        }
        return fldr;
      } // End get Visualizations Path
    }

    public static string DefaultSequencesPath
    {
      get
      {
        return DefaultNonAudioPath;
      }
    }

    public static string DefaultAudioPath
    {
      get
      {
        string fldr = "";
        string root = ROOT;
        string userDocs = Fyle.DefaultDocumentsPath;
        string userMusic = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        try
        {
          fldr = (string)Registry.GetValue(LOR_REGKEY, "AudioPath", root);
          if (fldr.Length < 6)
          {
            fldr = DefaultUserDataPath + "Audio\\";
          }
          bool valid = Fyle.IsValidPath(fldr);
          if (!valid)
          {
            fldr = DefaultUserDataPath + "Audio\\";
          }
          if (!Directory.Exists(fldr))
          {
            Directory.CreateDirectory(fldr);
          }
        }
        catch
        {
          fldr = userMusic;
        }
        return fldr;
      } // End get AudioPath
    }

    public static string DefaultClipboardsPath
    {
      get
      {
        string fldr = "";
        string root = ROOT;
        string userDocs = Fyle.DefaultDocumentsPath;
        try
        {
          fldr = (string)Registry.GetValue(LOR_REGKEY, "ClipboardsPath", root);
          if (fldr.Length < 6)
          {
            fldr = DefaultUserDataPath + "Clipboards\\";
          }
          bool valid = Fyle.IsValidPath(fldr);
          if (!valid)
          {
            fldr = DefaultUserDataPath + "Clipboards\\";
          }
          if (!Directory.Exists(fldr))
          {
            Directory.CreateDirectory(fldr);
          }
        }
        catch
        {
          fldr = userDocs;
        }
        return fldr;
      } // End get ClipboardsPath
    }

    public static string DefaultChannelConfigsPath
    {
      get
      {
        string fldr = "";
        // string root = ROOT;
        string userDocs = Fyle.DefaultDocumentsPath;
        try
        {
          fldr = (string)Registry.GetValue(LOR_REGKEY, "ChannelConfigsPath", "");
          if (fldr.Length < 6)
          {
            fldr = DefaultUserDataPath + "Sequences\\ChannelConfigs\\";
            Registry.SetValue(LOR_REGKEY, "ChannelConfigsPath", fldr, RegistryValueKind.String);
          }
          bool valid = Fyle.IsValidPath(fldr);
          if (!valid)
          {
            fldr = DefaultUserDataPath + "Sequences\\ChannelConfigs\\";
            Registry.SetValue(LOR_REGKEY, "ChannelConfigsPath", fldr, RegistryValueKind.String);
          }
          if (!Directory.Exists(fldr))
          {
            Directory.CreateDirectory(fldr);
          }
        }
        catch
        {
          fldr = userDocs;
        }
        return fldr;
      } // End get ChannelConfigsPath
    }
    #endregion LOR Specific File Functions

    #region TimeFunctions
    public static string FormatTime(int centiseconds)
    {
      string timeOut = "";

      int totsecs = (int)(centiseconds / 100);
      int centis = centiseconds % 100;
      int min = (int)(totsecs / 60);
      int secs = totsecs % 60;

      if (min > 0)
      {
        timeOut = min.ToString() + Fyle.CHAR_TIMESEP;
        timeOut += secs.ToString("00");
      }
      else
      {
        timeOut = secs.ToString();
      }
      timeOut += Fyle.CHAR_DECIMAL + centis.ToString("00");

      return timeOut;
    }

    public static int DecodeTime(string theTime)
    {
      // format mm:ss.cs
      int csOut = UNDEFINED;
      int csTmp = UNDEFINED;

      // Split time by :
      string[] tmpM = theTime.Split(Fyle.CHAR_TIMESEP);
      // Not more than 1 : ?
      if (tmpM.Length < 3)
      {
        // has a : ?
        if (tmpM.Length == 2)
        {
          // first part is minutes
          int min = 0;
          // try to parse minutes from first part of string
          int.TryParse(tmpM[0], out min);
          // each minute is 6000 centiseconds
          csTmp = min * 6000;
          // place second part of split into first part for next step of decoding
          tmpM[0] = tmpM[1];
        }
        // split seconds by . ?
        string[] tmpS = tmpM[0].Split('.');
        // not more than 1 . ?
        if (tmpS.Length < 3)
        {
          // has a . ?
          if (tmpS.Length == 2)
          {
            // next part is seconds
            int sec = 0;
            // try to parse seconds from first part of remaining string
            int.TryParse(tmpS[0], out sec);
            // each second is 100 centiseconds (duh!)
            csTmp += (sec * 100);
            // no more than 2 decimal places allowed
            if (tmpS[1].Length > 2)
            {
              tmpS[1] = tmpS[1].Substring(0, 2);
            }
            // place second part into first part for next step of decoding
            tmpS[0] = tmpS[1];
          }
          int cs = 0;
          int.TryParse(tmpS[0], out cs);
          csTmp += cs;
          csOut = csTmp;
        }
      }



      return csOut;
    }

    public static string Time_CentisecondsToMinutes(int centiseconds)
    {
      int mm = (int)(centiseconds / 6000);
      int ss = (int)((centiseconds - mm * 6000) / 100);
      int cs = (int)(centiseconds - mm * 6000 - ss * 100);
      string ret = mm.ToString("0") + Fyle.CHAR_TIMESEP + ss.ToString("00") + Fyle.CHAR_DECIMAL + cs.ToString("00");
      return ret;
    }

    public static int Time_MinutesToCentiseconds(string timeInMinutes)
    {
      // Time string must be formated as mm:ss.cs
      // Where mm is minutes.  Must be specified, even if zero.
      // Where ss is seconds 0-59.
      // Where cs is centiseconds 0-99.  Must be specified, even if zero.
      // Time string must contain one colon (:) and one period (.)
      // Maximum of 60 minutes  (Anything longer can result in unmanageable sequences)
      string newTime = timeInMinutes.Trim();
      int ret = UNDEFINED;
      int posColon = newTime.IndexOf(Fyle.CHAR_TIMESEP);
      if ((posColon > 0) && (posColon < 3))
      {
        int posc2 = newTime.IndexOf(Fyle.CHAR_TIMESEP, posColon + 1);
        if (posc2 < 0)
        {
          string min = newTime.Substring(0, posColon);
          string rest = newTime.Substring(posColon + 1);
          int posPer = rest.IndexOf(Fyle.CHAR_DECIMAL);
          if ((posPer == 2))
          {
            int posp2 = rest.IndexOf(Fyle.CHAR_DECIMAL, posPer + 1);
            if (posp2 < 0)
            {
              string sec = rest.Substring(0, posPer);
              string cs = rest.Substring(posPer + 1);
              int mn = LOR4Admin.UNDEFINED;
              int.TryParse(min, out mn);
              if ((mn >= 0) && (mn < 61))
              {
                int sc = LOR4Admin.UNDEFINED;
                int.TryParse(sec, out sc);
                if ((sc >= 0) && (sc < 60))
                {
                  int c = LOR4Admin.UNDEFINED;
                  int.TryParse(cs, out c);
                  if ((c >= 0) && (c < 100))
                  {
                    ret = mn * 6000 + sc * 100 + c;
                  }
                }
              }
            }
          }
        }
      }

      return ret;
    }

    #endregion // Time Functions

    public static int ExceptionLineNumber(Exception ex)
    {
      // Get stack trace for the exception with source file information
      StackTrace st = new StackTrace(ex, true);
      // Get the top stack frame
      StackFrame frame = st.GetFrame(0);
      // Get the line number from the stack frame
      int line = frame.GetFileLineNumber();
      return line;
    }



  } // end class LOR4Admin
} // end namespace LOR4Utils
