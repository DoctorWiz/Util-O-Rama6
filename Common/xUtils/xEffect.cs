using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace xLights22
{

	public class xEffect : IComparable<xEffect>
	{
		public string EffectType = "";
		public int StartTime = 0;
		public int EndTime = 0; //999999999;
		public string Parameters = "";


		/*
		public static string TABLE_Effect = "Effect";
		public static string FIELD_label = "label";
		public static string FIELD_start = "starttime";
		public static string FIELD_end = "endtime";
		public static string TABLE_timing = "timing";
		public static string TABLE_LORtime5 = "<Timing version=\"1\">";
		public static string TABLE_Grids = "<TimingGrids>";
		public static string TABLE_FreeGrid = "";
		//public static string FIELD_centisecond = "centisecond";
		public static string FIELD_millisecond = "millisecond";
		public static string LEVEL2 = "    ";
		public static string RECORD_start = "<";
		public static string RECORD_end = "/>";
		public static string RECORD_final = ">";
		public static string SPC = " ";
		public static string CRLF = "\r\n";
		public static string VALUE_start = "=\"";
		public static string VALUE_end = "\"";
		*/

		public xEffect(string theType, int startTime, int endTime)
		{
			if (startTime >= endTime)
			{
				// Raise Exception
				//System.Diagnostics.Debugger.Break();
			}
			else
			{
				EffectType = theType;
				StartTime = startTime;
				EndTime = endTime;
			}
		}

		public xEffect(string theType, int startTime, int endTime, string parameters)
		{
			if (startTime >= endTime)
			{
				// Raise Exception
				//System.Diagnostics.Debugger.Break();
			}
			else
			{
				EffectType = theType;
				StartTime = startTime;
				EndTime = endTime;
				Parameters = parameters;
			}
		}

		/*
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
		*/

		public int CompareTo(xEffect otherEffect)
		{
			return StartTime.CompareTo(otherEffect.StartTime);
		}
		/*
		public string LineOutX()
		{
			StringBuilder ret = new StringBuilder();
			//    <LOR4Effect 
			ret.Append(LEVEL2);
			ret.Append(RECORD_start);
			ret.Append(TABLE_Effect);
			ret.Append(SPC);
			//  label="foo" 
			ret.Append(FIELD_label);
			ret.Append(VALUE_start);
			ret.Append(Label);
			ret.Append(VALUE_end);
			ret.Append(SPC);
			//  starttime="50" 
			ret.Append(FIELD_start);
			ret.Append(VALUE_start);
			ret.Append(_starttime.ToString());
			ret.Append(VALUE_end);
			ret.Append(SPC);
			//  endtime="350" />
			ret.Append(FIELD_end);
			ret.Append(VALUE_start);
			ret.Append(_endtime.ToString());
			ret.Append(VALUE_end);
			ret.Append(SPC);

			ret.Append(RECORD_end);
			ret.Append(CRLF);

			return ret.ToString();
		}

		public string LineOut4()
		{
			StringBuilder ret = new StringBuilder();
			int cs = _starttime / 10;
			//    <timing 
			ret.Append(LEVEL2);
			ret.Append(RECORD_start);
			ret.Append(TABLE_timing);
			ret.Append(SPC);
			//  label="foo" 
			ret.Append(FIELD_millisecond);
			ret.Append(VALUE_start);
			ret.Append(cs.ToString());
			ret.Append(VALUE_end);
			ret.Append(SPC);

			ret.Append(RECORD_end);
			ret.Append(CRLF);

			return ret.ToString();
		}

		public string LineOut5()
		{
			StringBuilder ret = new StringBuilder();
			int cs = _starttime / 10;
			//    <timing 
			ret.Append(LEVEL2);
			ret.Append(RECORD_start);
			ret.Append(TABLE_timing);
			ret.Append(SPC);
			//  label="foo" 
			ret.Append(FIELD_millisecond);
			ret.Append(VALUE_start);
			ret.Append(cs.ToString());
			ret.Append(VALUE_end);
			ret.Append(SPC);

			ret.Append(RECORD_end);
			ret.Append(CRLF);

			return ret.ToString();
		}
		*/
	}
}
