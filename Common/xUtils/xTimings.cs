using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xLights22
{
  public class xTimings
  {
    public string Name = "";
    public string sourceVersion = "2022.1";
    public List<xMarker> Markers = new List<xMarker>();
    //public int effectCount = 0;
    public readonly static string SourceVersion = "2019.32";
    public readonly static string XMLinfo = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
    private int maxMillis = 0;

    public static string TABLE_timing = "timing";
    public static string TABLE_Effect = "Effect";
    public static string FIELD_name = "name";
    public static string FIELD_source = "SourceVersion";
    public static string TABLE_layers = "EffectLayer";
    public static string FIELD_version = "version";
    public static string TABLE_Grids = "TimingGrids";
    public static string TABLE_grid = "timingGrid";
    public static string TABLE_FreeGrid = "TimingGridFree";
    public static string TABLE_BeatChans = "BeatChannels";
    public static string FIELD_millisecond = "millisecond";
    public static string FIELD_saveID = "saveID";
    public static string FIELD_type = "type";
    public static string FIELD_label = "label";
    public static string FIELD_start = "starttime";
    public static string FIELD_end = "endtime";
    public static string FIELD_time = "time";
    public static string TYPE_freeform = "freeform";
    public static string PLURAL = "s";
    public static string SAVEID_X = "X";
    public static string LEVEL0 = "";
    public static string LEVEL1 = "  ";
    public static string LEVEL2 = "    ";
    public static string RECORD_start = "<";
    public static string RECORD_end = ">";
    public static string RECORD_done = "/>";
    public static string SPC = " ";
    public static string CRLF = "\r\n";
    public static string VALUE_start = "=\"";
    public static string VALUE_end = "\"";
    public static string RECORDS_done = "</";

    //public enum LabelType {  None=0, Numbers=1, NoteNames=2, MidiNumbers=3, KeyNames=4, KeyNumbers=5, Frequency=6, Letters=7 }

    //public static readonly string[] availableLabels = {"None", "Numbers", "Note Names", "MIDI Note Numbers", "Key Names",
    //"Key Numbers", "Frequency", "Letters" };


    public xTimings(string theName)
    {
      Name = theName;
      //xSequence.TimingTracks.Add(this);
    }

    public xMarker Add(string lineIn)
    {
      // In theory you could create new marker with just a name, but
      // that would be pretty dumb.  When adding a marker you should
      // at least provide a start time.
      // THUS trying to create a new marker from just a string indicates
      // it is importing a line from a timing file
      xMarker newMark = new xMarker(lineIn);
      Add(newMark);
      return newMark;
    }

    public void Add(xMarker newMark)
    {
      if (Markers.Count > 0)
      {
        if (newMark.starttime < Markers[Markers.Count - 1].endtime)
        {
          // Is this truly an error?  How will xLights respond?
          //System.Diagnostics.Debugger.Break();
          // Raise Exception
        }
        else
        {
          Markers.Add(newMark);
          maxMillis = newMark.endtime;
          //					effectCount++;
          //Array.Resize(ref Markers, effectCount);
          //Markers[effectCount - 1] = newMarkect;
        }
      }
      else
      {
        Markers.Add(newMark);
        maxMillis = newMark.endtime;
        //effectCount = 1;
        //Array.Resize(ref Markers, 1);
        //Markers[0] = newMarkect;
      }
      //Annotator.HighTime = newMarkect.endtime;

    }

    public xMarker Add(string theLabel, int startTime, int endTime, int number)
    {
      xMarker newMark = new xMarker(theLabel, startTime, endTime);
      newMark.Number = number;
      Add(newMark);
      return newMark;
    }

    public xMarker Add(string theLabel, int startTime, int endTime)
    {
      xMarker newMark = new xMarker(theLabel, startTime, endTime);
      Add(newMark);
      return newMark;
    }

    public xMarker Add(int startTime, int endTime)
    {
      xMarker newMark = new xMarker(startTime, endTime);
      Add(newMark);
      return newMark;
    }

    public xMarker Add(int endTime)
    {
      xMarker newMark = new xMarker(endTime);
      Add(newMark);
      return newMark;
    }

    public xMarker Add(string theLabel, int endTime)
    {
      xMarker newMark = new xMarker(theLabel, endTime);
      Add(newMark);
      return newMark;
    }

    public void Clear()
    {
      Name = "";
      sourceVersion = "2019.32";
      //Markers = null;
      Markers = new List<xMarker>();
      //effectCount = 0;
    }

    public int Import(string fileName, string trackName = "")
    {
      // Behavior if no name specified: will import the first timing track of a multi-track timing file
      //   or the [only] track in a single-track file.
      // If a name is specified it will not import the timing track unless the name matches
      //   regardless of whether or not it is a single or multi-tracked file
      int err = 0;
      int trackNo = 0;
      string lineIn = "";
      StreamReader reader = new StreamReader(fileName);
      while (!reader.EndOfStream)
      {

      }

      return err;
    }

    public int Import(string fileName, int whichTrack = 1)
    {
      // In case of a multi-track timing file, this will import only the first one
      //    unless otherwise specified
      int err = 0;
      int trackNo = 0;
      string lineIn = "";
      StreamReader reader = new StreamReader(fileName);
      while (!reader.EndOfStream)
      {
        lineIn = reader.ReadLine();
        // If 2nd line of file is <timings> it is a multi-track file
        // If 2nd line of file starts with <timing name= it is either a single-track file
        // or it is a lyric track (which has 3 timing tracks)
        if (lineIn == "<timings>")
        {
          trackNo++;
          if (whichTrack == trackNo)
          {
            lineIn = reader.ReadLine();
            Parse(lineIn);
            bool go = true;
            while ((!reader.EndOfStream) && go)
            {
              lineIn = reader.ReadLine();
              if (lineIn.IndexOf("</EffectLayer>") > 0)
              {
                go = false;
              }
              else
              {
                xMarker marker = new xMarker(lineIn);

              }
            }

          }
        }
      }
      reader.Close();
      return err;
    }

    public int Export(string fileName)
    {
      int err = 0;
      StreamWriter writer = new StreamWriter(fileName);
      writer.WriteLine(xAdmin.XML_HEADER);
      string lineOut = LineOut();
      writer.WriteLine(lineOut);
      writer.Close();
      return err;
    }

    public void Parse(string lineIn)
    {
      Name = xAdmin.getKeyWord(lineIn, "name");
    }

    public string LineOut()
    {
      StringBuilder ret = new StringBuilder();
      //  <timing
      ret.Append(LEVEL0);
      ret.Append(RECORD_start);
      ret.Append(TABLE_timing);
      ret.Append(SPC);
      //  name="the Name"
      ret.Append(FIELD_name);
      ret.Append(VALUE_start);
      ret.Append(Name);
      ret.Append(VALUE_end);
      ret.Append(SPC);
      //  SourceVersion="2022.1">
      ret.Append(FIELD_source);
      ret.Append(VALUE_start);
      ret.Append(sourceVersion);
      ret.Append(VALUE_end);
      ret.Append(RECORD_end);
      ret.Append(CRLF);
      //    <EffectLayer>
      ret.Append(LEVEL1);
      ret.Append(RECORD_start);
      ret.Append(TABLE_layers);
      ret.Append(RECORD_end);
      ret.Append(CRLF);

      for (int i = 0; i < Markers.Count; i++)
      {
        ret.Append(Markers[i].LineOut());
        ret.Append("\r\n");
      }

      //     </EffectLayer>
      ret.Append(LEVEL1);
      ret.Append(RECORDS_done);
      ret.Append(TABLE_layers);
      ret.Append(RECORD_end);
      ret.Append(CRLF);
      //  </timing>
      ret.Append(LEVEL0);
      ret.Append(RECORDS_done);
      ret.Append(TABLE_timing);
      ret.Append(RECORD_end);

      return ret.ToString();
    }

    public int Milliseconds
    {
      get
      {
        return maxMillis;
      }
    }
  }
  //! END xTimings class


  //!/////////////////////
  //!  xMarkers Class  //
  //!///////////////////
  public class xMarker : IComparable<xMarker>
  {
    public string Label = "";
    public object Tag = null;
    public int Number = -1;  // For many annotations, this will be the MIDI number
    private int _starttime = 0;
    private int _endtime = 0; //999999999;
    private static int lastEnd = 0;

    public xMarker(string lineIn)
    {
      // In theory you could create new marker with just a name, but
      // that would be pretty dumb.  When adding a marker you should
      // at least provide a start time.
      // THUS trying to create a new marker from just a string indicates
      // it is importing a line from a timing file
      Parse(lineIn);
    }

    public xMarker(string theLabel, int startTime, int endTime)
    {
      if (startTime >= endTime)
      {
        // Raise Exception
        //System.Diagnostics.Debugger.Break();
      }
      else
      {
        Label = theLabel;
        _starttime = startTime;
        _endtime = endTime;
        lastEnd = endTime;
      }
    }

    public xMarker(string theLabel, int startTime, int endTime, int number)
    {
      if (startTime >= endTime)
      {
        // Raise Exception
        //System.Diagnostics.Debugger.Break();
      }
      else
      {
        Label = theLabel;
        _starttime = startTime;
        _endtime = endTime;
        lastEnd = endTime;
        Number = number;
      }
    }

    public xMarker(int startTime, int endTime)
    {
      if (startTime >= endTime)
      {
        // Raise Exception
        System.Diagnostics.Debugger.Break();
      }
      else
      {
        _starttime = startTime;
        _endtime = endTime;
        lastEnd = endTime;
      }
    }

    public xMarker(int endTime)
    {
      if (lastEnd >= endTime)
      {
        // Raise Exception
        System.Diagnostics.Debugger.Break();
      }
      else
      {
        _starttime = lastEnd;
        _endtime = endTime;
        lastEnd = endTime;
      }
    }

    public xMarker(string theLabel, int endTime)
    {
      if (lastEnd >= endTime)
      {
        // Raise Exception
        System.Diagnostics.Debugger.Break();
      }
      else
      {
        Label = theLabel;
        _starttime = lastEnd;
        _endtime = endTime;
        lastEnd = endTime;
      }
    }

    public int starttime
    {
      get
      {
        return _starttime;
      }
      set
      {
        if (value >= _endtime)
        {
          System.Diagnostics.Debugger.Break();
          // Raise Exception
        }
        else
        {
          _starttime = value;
        }
      }
    }

    public int endtime
    {
      get
      {
        return _endtime;
      }
      set
      {
        if (_starttime >= value)
        {
          System.Diagnostics.Debugger.Break();
          // Raise Exception
        }
        else
        {
          _endtime = value;
        }
      }
    }

    public int CompareTo(xMarker otherEffect)
    {
      return _starttime.CompareTo(otherEffect.starttime);
    }

    public void Parse(string lineIn)
    {
      Label = xAdmin.getKeyWord(lineIn, "label");
      _starttime = xAdmin.getKeyValue(lineIn, "starttime");
      _endtime = xAdmin.getKeyValue(lineIn, "endtime");
      lastEnd = _endtime;
    }

    public string LineOut()
    {
      StringBuilder ret = new StringBuilder();
      ret.Append(xAdmin.LEVEL3);
      ret.Append("<Effect label=\"");
      ret.Append(Label);
      ret.Append("\" starttime=\"");
      ret.Append(_starttime.ToString());
      ret.Append("\" endtime=\"");
      ret.Append(_endtime.ToString());
      ret.Append("\" />");
      return ret.ToString();
    }

  }




}
