using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Text;

namespace LOR4
{
	public class LOR4Music
	{
		public string Artist = "";
		public string Title = "";
		public string Album = "";
		public string File = "";

		public const string FIELDmusicAlbum = " musicAlbum";
		public const string FIELDmusicArtist = " musicArtist";
		public const string FIELDmusicFilename = " musicFilename";
		public const string FIELDmusicTitle = " musicTitle";
		// TODO Add properties/methods for these
		protected LOR4Sequence parentSequence = null;
		protected bool isDirty = false;

		public LOR4Music()
		{
			// Default Contructor
		}
		public LOR4Music(string lineIn)
		{
			Parse(lineIn);
		}

		public void Parse(string lineIn)
		{
			Artist = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, FIELDmusicArtist));
			Album = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, FIELDmusicAlbum));
			Title = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, FIELDmusicTitle));
			File = LOR4Admin.getKeyWord(lineIn, FIELDmusicFilename);
		}

		public string LineOut()
		{
			StringBuilder ret = new StringBuilder();
			ret.Append(LOR4Admin.SetKey(FIELDmusicAlbum, LOR4Admin.XMLifyName(Album)));
			ret.Append(LOR4Admin.SetKey(FIELDmusicArtist, LOR4Admin.XMLifyName(Artist)));
			ret.Append(LOR4Admin.SetKey(FIELDmusicFilename, File));
			ret.Append(LOR4Admin.SetKey(FIELDmusicTitle, LOR4Admin.XMLifyName(Title)));
			return ret.ToString();
		}

		public LOR4Music Clone()
		{
			LOR4Music newTune = new LOR4Music();
			newTune.Album = Album;
			newTune.Artist = Artist;
			newTune.File = File;
			newTune.Title = Title;
			return newTune;
		}

	} // End LOR4Music Class

	public class LOR4SavedIndex
	{
		public LOR4MemberType MemberType = LOR4MemberType.None;
		public int objIndex = LOR4Admin.UNDEFINED;

		public LOR4SavedIndex Clone()
		{
			LOR4SavedIndex newSI = new LOR4SavedIndex();
			newSI.MemberType = MemberType;
			newSI.objIndex = objIndex;
			return newSI;
		} // End SavedIndex class
	}

	public class LOR4Output : IComparable<LOR4Output>, IEquatable<LOR4Output>
	{
		public LOR4DeviceType deviceType = LOR4DeviceType.None;

		// for LOR, this is the unit, for DMX it's not used
		public int unit = LOR4Admin.UNDEFINED;

		// for LOR or DMX this is the channel
		public int circuit = LOR4Admin.UNDEFINED;

		// for LOR, this is the network, for DMX it is the universe
		public int network = LOR4Admin.UNDEFINED;

		public bool isViz = false;

		private const char DELIM4 = '⬙';
		private static readonly string FIELDdeviceType = " deviceType"; // For Sequence Files, all lower case
		private static readonly string FIELDnetwork = " network";
		private static readonly string FIELDcircuit = " circuit";
		private static readonly string FIELDunit = " unit";

		private static readonly string FIELDDeviceType = " DeviceType"; //for Visualizer Files, capitalized
		private static readonly string FIELDNetwork = " Network";
		private static readonly string FIELDChannel = " Channel";
		private static readonly string FIELDController = " Controller";

		// TODO Add properties and methods to access these
		protected iLOR4Member myOwner = null;
		protected bool isDirty = false;

		public LOR4Output(iLOR4Member theOwner)
		{
			// Default Constructor
			myOwner = theOwner;
		}
		public LOR4Output(iLOR4Member theOwner, string lineIn)
		{
			myOwner = theOwner;
			Parse(lineIn);
		}

		public int CompareTo(LOR4Output other)
		{
			int ret = 0;
			if (deviceType < other.deviceType)
			{
				ret = 1;
			}
			else
			{
				if (deviceType > other.deviceType)
				{
					ret = -1;
				}
				else
				{
					if (deviceType == LOR4DeviceType.LOR)
					{
						ret = network.CompareTo(other.network);
						if (ret == 0)
						{
							ret = unit.CompareTo(other.unit);
							if (ret == 0)
							{
								ret = circuit.CompareTo(other.circuit);
							}
						}
					}
					if (deviceType == LOR4DeviceType.Dasher)
					{
						ret = network.CompareTo(other.network);
						if (ret == 0)
						{
							ret = unit.CompareTo(other.unit);
							if (ret == 0)
							{
								ret = circuit.CompareTo(other.circuit);
							}
						}
					}
					if (deviceType == LOR4DeviceType.DMX)
					{
						ret = network.CompareTo(other.network);
						if (ret == 0)
						{
							ret = unit.CompareTo(other.circuit);
						}
					}
				}
			}
			return ret;
		}

		// For IEquatible
		public bool Equals(LOR4Output obj)
		{
			// Check for null values and compare run-time types.
			if (obj == null || GetType() != obj.GetType())
				return false;

			LOR4Output o = (LOR4Output)obj;
			return ((deviceType == o.deviceType) && (unit == o.unit) && (circuit == o.circuit) && (network == o.network));
		}

		public override bool Equals(Object obj)
		{
			// Check for null values and compare run-time types.
			if (obj == null || GetType() != obj.GetType())
				return false;

			LOR4Output o = (LOR4Output)obj;
			return ((deviceType == o.deviceType) && (unit == o.unit) && (circuit == o.circuit) && (network == o.network));
		}

		public override int GetHashCode()
		{
			var hashCode = 1137;
			hashCode += (int)deviceType;
			hashCode += unit * 0x10;
			hashCode += circuit * 0x1000;
			hashCode += network * 0x2000000;
			return hashCode;
		}

		public string SortableComparableString()
		{
			int dt = (int)deviceType;  // Enumerate deviceType
			dt += 1; // Add 1 'cuz None=Undefined=-1 and for comparison's sake, we want 0+ positive numbers

			// Short Version
			//string ret = dt.ToString() + network.ToString("00000");
			//ret += unit.ToString("000") + circuit.ToString("000");
			// Long Version
			string ret = "T" + dt.ToString() + DELIM4 + "N" + network.ToString("00000");
			ret += DELIM4 + "U" + unit.ToString("000") + DELIM4 + "C" + circuit.ToString("000");
			return ret;
			;
		}

		public void Parse(string lineIn)
		{
			//System.Diagnostics.Debugger.Break();
			//string keywd = LOR4Admin.getKeyWord(lineIn, LOR4VizChannel.FIELDsubParam);
			//string keywd = LOR4Admin.getKeyWord(lineIn, LOR4Channel.FIELDcolor);
			int idt = lineIn.IndexOf(FIELDdeviceType);  // Check for "deviceType" (lower case) (Sequences)
																									//if (keywd.Length == 0)
			if (idt > 0)
			{
				//isViz = false;
				string dev = LOR4Admin.getKeyWord(lineIn, FIELDdeviceType); // Note: deviceType is NOT capitalized and is a String
				deviceType = LOR4SeqEnums.EnumDevice(dev);
				// for LOR, this is the network, for DMX it is the UniverseNumber
				network = LOR4Admin.getKeyValue(lineIn, FIELDnetwork);
				// for LOR, this is the channel, for DMX this is the DMXAddress
				circuit = LOR4Admin.getKeyValue(lineIn, FIELDcircuit);
				// for LOR, this is the unit, for DMX it's not used
				unit = LOR4Admin.getKeyValue(lineIn, FIELDunit);
			}
			else
			{
				idt = lineIn.IndexOf(FIELDDeviceType);  // Check for "DeviceType" (upper case) (Visualizations)
				if (idt > 0)
				{
					isViz = true;
					int dt = LOR4Admin.getKeyValue(lineIn, FIELDDeviceType); // Note: DeviceType IS captialized and is an Int
					if (dt == 1) deviceType = LOR4DeviceType.LOR;
					if (dt == 7) deviceType = LOR4DeviceType.DMX;

					// for LOR, this is the unit, for DMX it's not used
					unit = LOR4Admin.getKeyValue(lineIn, FIELDController);

					// for LOR, this is the network, for DMX it is the universe
					network = LOR4Admin.getKeyValue(lineIn, FIELDNetwork);

					// for LOR or DMX this is the channel
					circuit = LOR4Admin.getKeyValue(lineIn, FIELDChannel);

				}
				else
				{
					// if neither "deviceType" or "DeviceType" was found
					// The Device Type is 'None'
					deviceType = LOR4DeviceType.None;
				}
			}

		}

		public string LineOut()
		{
			StringBuilder ret = new StringBuilder();
			if (isViz)  // Visualizer
			{
				if (deviceType == LOR4DeviceType.LOR)
				{
					ret.Append(LOR4Admin.SetKey(FIELDDeviceType, 1));
				}
				else if (deviceType == LOR4DeviceType.DMX)
				{
					ret.Append(LOR4Admin.SetKey(FIELDDeviceType, 7));
				}
				ret.Append(LOR4Admin.SetKey(FIELDNetwork, network));
				ret.Append(LOR4Admin.SetKey(FIELDController, unit));
				ret.Append(LOR4Admin.SetKey(FIELDChannel, circuit));
			}
			else  // Sequence (not visualizer)
			{
				if ((network < 0) || (circuit < 0))
				{
					// Assume DeviceType = None
					// So-- Don't append this info
					LOR4DeviceType ldt = deviceType;
				}
				else
				{
					if (deviceType == LOR4DeviceType.LOR)
					{
						ret.Append(LOR4Admin.SetKey(FIELDdeviceType, LOR4SeqEnums.DeviceName(deviceType)));
						ret.Append(LOR4Admin.SetKey(FIELDunit, unit));
						ret.Append(LOR4Admin.SetKey(FIELDcircuit, circuit));
						if (network != LOR4Admin.UNDEFINED)
						{
							ret.Append(LOR4Admin.SetKey(FIELDnetwork, network));
						}
					}
					else if (deviceType == LOR4DeviceType.DMX)
					{
						ret.Append(LOR4Admin.SetKey(FIELDdeviceType, LOR4SeqEnums.DeviceName(deviceType)));
						ret.Append(LOR4Admin.SetKey(FIELDcircuit, circuit));
						ret.Append(LOR4Admin.SetKey(FIELDnetwork, network));
					}
					else if (deviceType == LOR4DeviceType.Dasher)
					{
						ret.Append(LOR4Admin.SetKey(FIELDdeviceType, LOR4SeqEnums.DeviceName(deviceType)));
						ret.Append(LOR4Admin.SetKey(FIELDunit, unit));
						ret.Append(LOR4Admin.SetKey(FIELDcircuit, circuit));
					}
					else if (deviceType == LOR4DeviceType.None)
					{
						// Don't Append Anything!
					}
				}
			}
			return ret.ToString();
		}

		// An Alias: channel=circuit
		public int channel
		{
			get
			{
				return circuit;
			}
			set
			{
				circuit = value;
				//if (parentSequence != null) parentSequence.MakeDirty();
			}
		}

		// An Alias: universe=network for DMX
		public int UniverseNumber
		{
			get
			{
				return network;
			}
			set
			{
				network = value;
				//if (parentSequence != null) parentSequence.MakeDirty();
			}
		}

		public int DMXAddress
		{ get { return circuit; } set { circuit = value; } }

		public string networkName
		{
			get
			{
				string n = "";
				if (deviceType == LOR4DeviceType.DMX)
				{
					n = "DMX512";
				}
				else
				{
					if (deviceType == LOR4DeviceType.LOR)
					{
						if (network < 0)
						{
							n = "Regular";
						}
						else
						{
							n = "Aux" + (char)(64 + network);
						}
					}
					else
					{
						n = LOR4SeqEnums.DeviceName(deviceType);
					}
				}
				return n;
			}
		}

		public LOR4Output Clone()
		{
			LOR4Output oout = new LOR4Output(myOwner);
			oout.deviceType = deviceType;
			oout.circuit = circuit;
			oout.network = network;
			oout.unit = unit;
			oout.isViz = isViz;
			return oout;
		}


	} // End LOR4Output Class

	public class LOR4Effect
	{
		public LOR4.LOR4EffectType EffectType = LOR4EffectType.None;
		private int myStartCentisecond = LOR4Admin.UNDEFINED;
		private int myEndCentisecond = 360001;
		public int Intensity = LOR4Admin.UNDEFINED;
		public int startIntensity = LOR4Admin.UNDEFINED;
		public int endIntensity = LOR4Admin.UNDEFINED;
		public LOR4Channel parent = null;
		public int myIndex = LOR4Admin.UNDEFINED;

		public const string FIELDintensity = " intensity";
		public const string FIELDstartIntensity = " startIntensity";
		public const string FIELDendIntensity = " endIntensity";


		public LOR4Effect()
		{
		}
		public LOR4Effect(string lineIn)
		{
			Parse(lineIn);
		}

		public LOR4Effect(LOR4EffectType effectType, int startTime, int endTime)
		{
			if (startTime >= endTime)
			{
				// Raise Error to Debugger
				System.Diagnostics.Debugger.Break();
			}
			else
			{
				this.EffectType = effectType;
				myStartCentisecond = startTime;
				myEndCentisecond = endTime;
			}
		}
		public LOR4Effect(LOR4EffectType effectType, int startTime, int endTime, int intensity)
		{
			if (startTime >= endTime)
			{
				// Raise Error to Debugger
				System.Diagnostics.Debugger.Break();
			}
			else
			{
				this.EffectType = effectType;
				myStartCentisecond = startTime;
				myEndCentisecond = endTime;
				Intensity = intensity;
			}
		}

		public LOR4Effect(LOR4EffectType effectType, int startTime, int endTime, int start_Intensity, int end_Intensity)
		{
			if (startTime >= endTime)
			{
				// Raise Error to Debugger
				System.Diagnostics.Debugger.Break();
			}
			else
			{
				this.EffectType = effectType;
				myStartCentisecond = startTime;
				myEndCentisecond = endTime;
				startIntensity = start_Intensity;
				endIntensity = end_Intensity;
			}
		}


		public void Parse(string lineIn)
		{
			this.EffectType = LOR4SeqEnums.EnumEffectType(LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDtype));
			myStartCentisecond = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDstartCentisecond);
			myEndCentisecond = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDendCentisecond);
			Intensity = LOR4Admin.getKeyValue(lineIn, FIELDintensity);
			startIntensity = LOR4Admin.getKeyValue(lineIn, FIELDstartIntensity);
			endIntensity = LOR4Admin.getKeyValue(lineIn, FIELDendIntensity);

			if (parent != null)
			{
				if (parent.Parent != null)
				{
					parent.Parent.MakeDirty(true);
				}
			}
		}

		public string LineOut()
		{
			StringBuilder ret = new StringBuilder();

			ret.Append(LOR4Admin.StartTable(LOR4Sequence.TABLEeffect, 3));

			ret.Append(LOR4Admin.SetKey(LOR4Admin.FIELDtype, LOR4SeqEnums.EffectName(this.EffectType).ToLower()));
			ret.Append(LOR4Admin.SetKey(LOR4Admin.FIELDstartCentisecond, startCentisecond));
			ret.Append(LOR4Admin.SetKey(LOR4Admin.FIELDendCentisecond, myEndCentisecond));
			// For steady intensity (non-ramp/fade)
			if (Intensity > LOR4Admin.UNDEFINED)
			{
				ret.Append(LOR4Admin.SetKey(FIELDintensity, Intensity));
			}
			// For ramps/fades
			if (startIntensity > LOR4Admin.UNDEFINED)
			{
				ret.Append(LOR4Admin.SetKey(FIELDstartIntensity, startIntensity));
				ret.Append(LOR4Admin.SetKey(FIELDendIntensity, endIntensity));
			}
			ret.Append(LOR4Admin.ENDFLD);
			return ret.ToString();
		}

		public override string ToString()
		{
			StringBuilder ret = new StringBuilder();

			ret.Append(LOR4SeqEnums.EffectTypeName(this.EffectTypeEX));
			ret.Append(" From ");
			ret.Append(startCentisecond);
			ret.Append(" to ");
			ret.Append(myEndCentisecond);
			if (Intensity > LOR4Admin.UNDEFINED)
			{
				ret.Append(" at ");
				ret.Append(Intensity);
			}
			if (startIntensity > LOR4Admin.UNDEFINED)
			{
				ret.Append(" at ");
				ret.Append(startIntensity);
			}
			return ret.ToString();
		}

		public int startCentisecond
		{
			get
			{
				return myStartCentisecond;
			}
			set
			{
				myStartCentisecond = value;
				if (parent != null)
				{
					if (parent.Parent != null)
					{
						parent.Parent.MakeDirty(true);
					}
				}
				if (parent != null)
				{
					if (myStartCentisecond >= parent.Centiseconds)
					{
						// Raise Error
						parent.Centiseconds = myStartCentisecond + 1;
					}
				}
			}
		}

		public int endCentisecond
		{
			get
			{
				return myEndCentisecond;
			}
			set
			{
				myEndCentisecond = value;
				if (parent != null)
				{
					if (parent.Parent != null)
					{
						parent.Parent.MakeDirty(true);
					}
				}
				if (parent != null)
				{
					if (myEndCentisecond > parent.Centiseconds)
					{
						// Raise Error
						parent.Centiseconds = myEndCentisecond;
					}
				}
			}
		}

		public int Direction
		{

			get
			{
				int ret = 0;
				if (startIntensity < endIntensity) ret = 1;
				if (startIntensity > endIntensity) ret = -1;
				return ret;
			}
		}

		public bool Steady
		{
			get
			{
				if (Direction == 0) return true; else return false;
			}
		}


		public LOR4EffectType EffectTypeEX
		{
			get
			{
				LOR4EffectType levelOut = LOR4EffectType.None;
				if (this.EffectType == LOR4EffectType.Shimmer) levelOut = LOR4EffectType.Shimmer;
				if (this.EffectType == LOR4EffectType.Twinkle) levelOut = LOR4EffectType.Twinkle;
				if (this.EffectType == LOR4EffectType.DMX) levelOut = LOR4EffectType.DMX;
				if (this.EffectType == LOR4EffectType.Intensity)
				{
					if (endIntensity < 0)
					{
						levelOut = LOR4EffectType.Constant;
					}
					else
					{
						if (endIntensity > startIntensity)
						{
							levelOut = LOR4EffectType.FadeUp;
						}
						else
						{
							levelOut = LOR4EffectType.FadeDown;
						}
					}
				}
				return levelOut;
			}
		}


		public LOR4Effect Clone()
		{
			// See Also: Duplicate()
			LOR4Effect ret = new LOR4Effect(this.EffectType, this.startCentisecond, this.endCentisecond);
			ret.Intensity = Intensity;
			ret.startIntensity = startIntensity;
			ret.endIntensity = endIntensity;
			ret.parent = parent;
			//int pcs = parent.Parent.Centiseconds;
			//pcs = Math.Max(startCentisecond, pcs);
			//pcs = Math.Max(endCentisecond, pcs);
			//parent.Parent.Centiseconds = pcs;
			return ret;
		}


	} // End LOR4Effect Class

	public class LOR4Animation
	{
		//public int sections = 0;
		//public int columns = 0;
		public string image = "";
		public List<LOR4AnimationRow> animationRows = new List<LOR4AnimationRow>();
		public LOR4Sequence parentSequence = null;

		public const string FIELDrows = " sections";
		public const string FIELDcolumns = " columns";
		public const string FIELDimage = " image";

		public int sections
		{
			get
			{
				return animationRows.Count;
			}
		}

		public int columns
		{
			get
			{
				int c = 0;
				if (animationRows.Count > 0)
				{
					c = animationRows[0].animationColumns.Count;
				}
				return c;
			}
		}

		public LOR4Animation(LOR4Sequence myParent)
		{
			parentSequence = myParent;
		}
		public LOR4Animation(LOR4Sequence myParent, string lineIn)
		{
			parentSequence = myParent;
			Parse(lineIn);
		}

		public string LineOut()
		{
			StringBuilder ret = new StringBuilder();

			//if (sections > 0)
			//{
			ret.Append(LOR4Admin.StartTable(LOR4Sequence.TABLEanimation, 1));

			ret.Append(LOR4Admin.SetKey(FIELDrows, sections));
			ret.Append(LOR4Admin.SetKey(FIELDcolumns, columns));
			ret.Append(LOR4Admin.SetKey(FIELDimage, image));
			if (animationRows.Count > 0)
			{
				ret.Append(LOR4Admin.FINFLD);
				foreach (LOR4AnimationRow aniRow in animationRows)
				{
					ret.Append(LOR4Admin.CRLF);
					ret.Append(aniRow.LineOut());
				}
				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.LEVEL1);
				ret.Append(LOR4Admin.FINTBL);
				ret.Append(LOR4Sequence.TABLEanimation);
				ret.Append(LOR4Admin.FINFLD);
			}
			else
			{
				ret.Append(LOR4Admin.ENDFLD);
			}

			return ret.ToString();

		}

		public void Parse(string lineIn)
		{
			//sections = LOR4Admin.getKeyValue(lineIn, LOR4AnimationRow.FIELDrow + LOR4Admin.PLURAL);
			//columns = LOR4Admin.getKeyValue(lineIn, FIELDcolumns);
			image = LOR4Admin.getKeyWord(lineIn, FIELDimage);
			if (parentSequence != null) parentSequence.MakeDirty(true);

		}

		public LOR4AnimationRow AddRow(string lineIn)
		{
			LOR4AnimationRow newRow = new LOR4AnimationRow(lineIn);
			AddRow(newRow);
			return newRow;
		}

		public int AddRow(LOR4AnimationRow newRow)
		{
			animationRows.Add(newRow);
			if (parentSequence != null) parentSequence.MakeDirty(true);
			return animationRows.Count - 1;
		}

		public LOR4Animation Clone()
		{
			LOR4Animation dupOut = new LOR4Animation(null);
			dupOut.image = image;
			foreach (LOR4AnimationRow row in animationRows)
			{
				LOR4AnimationRow newRow = row.Clone();
				dupOut.animationRows.Add(newRow);
			}
			return dupOut;
		}
	} // end LOR4Animation class

	public class LOR4AnimationRow
	{
		public int rowIndex = LOR4Admin.UNDEFINED;
		public List<LOR4AnimationColumn> animationColumns = new();
		public const string FIELDrow = "row";
		public const string FIELDindex = "index";

		public LOR4AnimationRow()
		{
			// Default Constructor
		}
		public LOR4AnimationRow(string lineIn)
		{
			Parse(lineIn);
		}

		public void Parse(string lineIn)
		{
			rowIndex = LOR4Admin.getKeyValue(lineIn, FIELDrow + LOR4Admin.SPC + FIELDindex);

		}

		public string LineOut()
		{
			StringBuilder ret = new StringBuilder();

			ret.Append(LOR4Admin.CRLF);
			ret.Append(LOR4Admin.LEVEL2);
			ret.Append(LOR4Admin.STFLD);
			ret.Append(FIELDrow);
			ret.Append(LOR4Admin.SPC);
			ret.Append(LOR4Admin.SetKey(FIELDindex, rowIndex));
			ret.Append(LOR4Admin.FINFLD);
			foreach (LOR4AnimationColumn aniCol in animationColumns)
			{
				ret.Append(LOR4Admin.CRLF);
				ret.Append(aniCol.LineOut());
			} // end columns loop
			ret.Append(LOR4Admin.CRLF);
			ret.Append(LOR4Admin.LEVEL2);
			ret.Append(LOR4Admin.FINTBL);
			ret.Append(FIELDrow);
			ret.Append(LOR4Admin.FINFLD);

			return ret.ToString();
		} // end sections loop

		public LOR4AnimationColumn AddColumn(string lineIn)
		{
			LOR4AnimationColumn newColumn = new LOR4AnimationColumn(lineIn);
			AddColumn(newColumn);
			return newColumn;
		}

		public int AddColumn(LOR4AnimationColumn animationColumn)
		{
			animationColumns.Add(animationColumn);
			return animationColumns.Count - 1;
		}

		public LOR4AnimationRow Clone()
		{
			LOR4AnimationRow rowOut = new LOR4AnimationRow();
			rowOut.rowIndex = rowIndex;
			foreach (LOR4AnimationColumn column in animationColumns)
			{
				LOR4AnimationColumn newCol = column.Clone();
				rowOut.animationColumns.Add(newCol);
			}
			return rowOut;
		}
	} // end LOR4AnimationRow class

	public class LOR4AnimationColumn
	{
		public int columnIndex = LOR4Admin.UNDEFINED;
		public int channel = LOR4Admin.UNDEFINED;
		public const string FIELDcolumnIndex = "column index";

		public LOR4AnimationColumn()
		{
			//Default Constructor
		}

		public LOR4AnimationColumn(string lineIn)
		{
			Parse(lineIn);
		}

		public void Parse(string lineIn)
		{
			columnIndex = LOR4Admin.getKeyValue(lineIn, FIELDcolumnIndex);
			channel = LOR4Admin.getKeyValue(lineIn, LOR4Admin.TABLEchannel);

		}

		public string LineOut()
		{
			StringBuilder ret = new StringBuilder();

			ret.Append(LOR4Admin.LEVEL3);
			ret.Append(LOR4Admin.STFLD);
			ret.Append(LOR4Admin.SetKey(FIELDcolumnIndex, columnIndex));
			ret.Append(LOR4Admin.SPC);

			ret.Append(LOR4Admin.SetKey(LOR4Admin.TABLEchannel, channel));
			ret.Append(LOR4Admin.ENDFLD);


			return ret.ToString();
		}

		public LOR4AnimationColumn Clone()
		{
			LOR4AnimationColumn colOut = new LOR4AnimationColumn();
			colOut.columnIndex = columnIndex;
			colOut.channel = channel;
			return colOut;
		}
	} // end LOR4AnimationColumn class

	public class LOR4Loop
	{
		public int startCentsecond = LOR4Admin.UNDEFINED;
		public int endCentisecond = 360001;
		public int loopCount = LOR4Admin.UNDEFINED;
		public const string FIELDloopCount = "loopCount";
		public const string FIELDloop = "loop";

		public LOR4Loop()
		{
			// Default Constructor
		}
		public LOR4Loop(string lineIn)
		{
			Parse(lineIn);
		}

		public void Parse(string lineIn)
		{
			startCentsecond = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDstartCentisecond);
			endCentisecond = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDendCentisecond);
			loopCount = LOR4Admin.getKeyValue(lineIn, FIELDloopCount);
		}

		public string LineOut()
		{
			StringBuilder ret = new StringBuilder();

			ret.Append(LOR4Admin.StartTable(FIELDloop, 5));
			ret.Append(LOR4Admin.SPC);

			ret.Append(LOR4Admin.SetKey(LOR4Admin.FIELDstartCentisecond, startCentsecond));
			ret.Append(LOR4Admin.SPC);
			ret.Append(LOR4Admin.SetKey(LOR4Admin.FIELDendCentisecond, endCentisecond));

			ret.Append(LOR4Admin.StartTable(LOR4Sequence.TABLEloopLevel, 4));
			ret.Append(LOR4Admin.FINFLD);
			if (loopCount > 0)
			{
				ret.Append(LOR4Admin.SPC);
				ret.Append(LOR4Admin.SetKey(FIELDloopCount, loopCount));
			}
			ret.Append(LOR4Admin.ENDFLD);

			return ret.ToString();
		}

		public LOR4Loop Clone()
		{
			LOR4Loop dupLoop = new LOR4Loop();


			return dupLoop;
		}
	} // end LOR4Loop class

	public class LOR4LoopLevel
	{
		public List<LOR4Loop> loops = new List<LOR4Loop>();
		//public int loopsCount = LOR4Admin.UNDEFINED;

		public LOR4LoopLevel()
		{
			// Default Constructor
		}
		public LOR4LoopLevel(string lineIn)
		{
			Parse(lineIn);
		}

		public void Parse(string lineIn)
		{
			//loops.Count = LOR4Admin.getKeyValue(lineIn, LOR4Loop.FIELDloopCount);
		}

		public string LineOut()
		{
			StringBuilder ret = new StringBuilder();

			ret.Append(LOR4Admin.CRLF);
			ret.Append(LOR4Admin.StartTable(LOR4Sequence.TABLEloopLevel, 4));
			if (loops.Count > 0)
			{
				ret.Append(LOR4Admin.SPC);
				ret.Append(LOR4Admin.SetKey(LOR4Loop.FIELDloopCount, loops.Count));
			}
			if (loops.Count > 0)
			{
				foreach (LOR4Loop theLoop in loops)
				{
					ret.Append(LOR4Admin.CRLF);
					ret.Append(theLoop.LineOut());
				}
				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.EndTable(LOR4Sequence.TABLEloopLevel, 4));
			}
			else
			{
				ret.Append(LOR4Admin.ENDFLD);
			}
			return ret.ToString();
		}


		public LOR4Loop AddLoop(string lineIn)
		{
			LOR4Loop newLoop = new LOR4Loop(lineIn);
			AddLoop(newLoop);
			return newLoop;
		}

		public int AddLoop(LOR4Loop newLoop)
		{
			loops.Add(newLoop);
			return loops.Count - 1;
		}

		public LOR4LoopLevel Clone()
		{
			LOR4LoopLevel dupLevel = new LOR4LoopLevel();

			return dupLevel;
		}
	} // end LOR4LoopLevel class

	public class ErrInfo
	{
		public int fileLine = LOR4Admin.UNDEFINED;
		public int linePos = LOR4Admin.UNDEFINED;
		public int codeLine = LOR4Admin.UNDEFINED;
		public string errName = "";
		public string errMsg = "";
		public string lineIn = "";
	}


	public class LOR4SeqInfo
	{
		public string filename = "*_UNNAMED_FILE_*";
		public string xmlInfo = LOR4Admin.XMLINFO;
		public string infoLine = ""; // only populated when the info line is considered invalid, and saved only for debugging purposes.
		public int saveFileVersion = 14; // LOR4Admin.UNDEFINED;
		public string author = "Util-O-Rama";
		//public string modifiedBy = "Util-O-Rama / " + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
		public string modifiedBy = "";
		public DateTime file_created = DateTime.Now;
		public DateTime file_modified = DateTime.Now;
		public DateTime file_accessed = DateTime.Now;
		// Created At defaults to the file's Created At timestamp, but is overridden by what's in the file information header
		public string createdAt = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
		// Last defaults to the file's modified At timestamp, but is overridden by any data changes
		public DateTime lastModified = DateTime.Now; //.ToString("MM/dd/yyyy hh:mm:ss tt");
		public DateTime file_saved = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

		public LOR4Music music = new LOR4Music();
		public int videoUsage = 0;
		public string animationInfo = "";
		public LOR4SequenceType sequenceType = LOR4SequenceType.Undefined;
		// TODO Change this to protected and add properties / methods for it
		// Note: May be a Sequence or a Vizualization!
		public iLOR4Member Parent = null;
		protected bool isDirty = false;


		//public LOR4Visualization  ParentVisualization = null;
		public ErrInfo LastError = new ErrInfo();

		public const string FIELDsaveFileVersion = " saveFileVersion";
		public const string FIELDlvizSaveFileVersion = " lvizSaveFileVersion";
		public const string FIELDchannelConfigFileVersion = " channelConfigFileVersion";
		public const string FIELDauthor = " author";
		public const string FIELDcreatedAt = " createdAt";
		public const string FIELDmodifiedBy = " modifiedBy";
		public const string FIELDvideoUsage = " videoUsage";

		public LOR4SeqInfo(LOR4Sequence myParent)
		{
			Parent = myParent;
		}
		public LOR4SeqInfo(LOR4Sequence myParent, string lineIn)
		{
			Parent = myParent;
			Parse(lineIn);
		}

		//public LOR4SeqInfo(LOR4Visualization myParent, string lineIn)
		//{
		//	ParentVisualization = myParent;
		//	Parse(lineIn);
		//}

		public void Parse(string lineIn)
		{
			author = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, FIELDauthor));
			modifiedBy = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, FIELDmodifiedBy));
			createdAt = LOR4Admin.getKeyWord(lineIn, FIELDcreatedAt);
			music = new LOR4Music(lineIn);
			bool musical = (music.File.Length > 4);
			saveFileVersion = LOR4Admin.getKeyValue(lineIn, FIELDsaveFileVersion);
			if (saveFileVersion > 0)
			{
				if (saveFileVersion > 2)
				{
					if (musical)
					{
						sequenceType = LOR4SequenceType.Musical;
					}
					else
					{
						sequenceType = LOR4SequenceType.Animated;
					}
					videoUsage = LOR4Admin.getKeyValue(lineIn, FIELDvideoUsage);
				}
				else
				{
					saveFileVersion = LOR4Admin.getKeyValue(lineIn, FIELDchannelConfigFileVersion);
					if (saveFileVersion > 2)
					{
						sequenceType = LOR4SequenceType.ChannelConfig;
					}
				}
			}
			else // First fetch returned undefined
			{
				string sfv = LOR4Admin.getKeyWord(lineIn, FIELDlvizSaveFileVersion);
				if (sfv.Length > 0)
				{
					saveFileVersion = Int32.Parse(sfv.Substring(0, 1));
					sequenceType = LOR4SequenceType.Visualizer;
				}
			}

		} // end Parse

		public string LineOut()
		{
			StringBuilder ret = new StringBuilder();

			// Use sequence.sequenceInfo when using WriteSequenceFile (which matches original unmodified file)
			// Use sequence.fileInfo.sequenceInfo when using WriteSequenceInxxxxOrder (which creates a whole new file)
			ret.Append(LOR4Admin.STFLD);
			ret.Append(LOR4Sequence.TABLEsequence);

			ret.Append(FIELDsaveFileVersion);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(saveFileVersion);
			ret.Append(LOR4Admin.ENDQT);

			ret.Append(FIELDauthor);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(LOR4Admin.XMLifyName(author));
			ret.Append(LOR4Admin.ENDQT);

			// if Sequence's dirty flag is set, this returns a createdAt that is NOW
			// Whereas if Sequence is 'clean' this returns the createdAt of the original file
			ret.Append(FIELDcreatedAt);
			ret.Append(LOR4Admin.FIELDEQ);
			//if (Parent.dirty)
			//{
			//	string nowtime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
			//	ret += nowtime;
			//}
			//else
			//{
			//!NO! Always return THE ORIGINAL date/time of creation!
			ret.Append(createdAt);
			ret.Append(LOR4Admin.ENDQT);
			//}

			// if Sequence's dirty flag is set, this returns a modifiedBy that is NOW
			// Whereas if Sequence is 'clean' this returns the createdAt of the original file
			//if (modifiedBy.Length < 1)
			//{
			if (Parent.Dirty)
			{
				if (author.Length < 2)
				{
					author = LOR4Admin.DefaultAuthor;
				}
				modifiedBy = author + " / Util-O-Rama";
			}
			//}
			ret.Append(FIELDmodifiedBy);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(LOR4Admin.XMLifyName(modifiedBy));
			ret.Append(LOR4Admin.ENDQT);

			if (sequenceType == LOR4SequenceType.Musical)
			{
				ret.Append(music.LineOut());
			}
			ret.Append(FIELDvideoUsage);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(videoUsage);
			ret.Append(LOR4Admin.ENDQT);
			ret.Append(LOR4Admin.ENDTBL);

			return ret.ToString();
		}

		public string LastModified
		{
			get
			{
				return lastModified.ToString("MM/dd/yyyy hh:mm:ss tt");
			}
		}

		public string LineOut(string newCreatedAt)
		{
			StringBuilder ret = new StringBuilder();

			ret.Append(xmlInfo);
			ret.Append(LOR4Admin.CRLF);

			ret.Append(LOR4Admin.STFLD);
			ret.Append(LOR4Sequence.TABLEsequence);

			ret.Append(FIELDsaveFileVersion);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(saveFileVersion.ToString());
			ret.Append(LOR4Admin.ENDQT);

			ret.Append(FIELDauthor);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(LOR4Admin.XMLifyName(author));
			ret.Append(LOR4Admin.ENDQT);

			ret.Append(FIELDcreatedAt);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(newCreatedAt);
			ret.Append(LOR4Admin.ENDQT);
			if (sequenceType == LOR4SequenceType.Musical)
			{
				ret.Append(music.ToString());
			}
			ret.Append(FIELDvideoUsage);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(videoUsage.ToString());
			ret.Append(LOR4Admin.ENDQT);
			ret.Append(LOR4Admin.ENDTBL);

			return ret.ToString();
		} // end LineOut

		public LOR4SeqInfo Clone()
		{
			LOR4SeqInfo data = new LOR4SeqInfo(null);
			data.filename = this.filename;
			data.xmlInfo = xmlInfo;
			data.saveFileVersion = saveFileVersion;
			data.author = author;
			data.file_accessed = file_accessed;
			data.file_created = file_created;
			data.file_modified = file_modified;
			data.music = music.Clone();
			data.videoUsage = videoUsage;
			data.animationInfo = animationInfo;
			data.sequenceType = this.sequenceType;
			return data;
		}
	} // end LOR4SeqInfo class

	public class Err
	{
		// =0 means no errors or warnings
		// =1-100 are warnings, but not fatal errors

		// =101+ is a fatal error
		public const int ERR_WrongFileVersion = 114;

		public static string ErrName(int err)
		{
			string ret = "";

			switch (err)
			{
				case 114:
					ret = "Sequence is not version 4";
					break;
			}
			return ret;
		}

		public static string ErrInfo(int err)
		{
			string ret = "";

			switch (err)
			{
				case 114:
					ret = "Only Sequences and related files from Light-O-Rama Sequence Editor version 4.x are supported by this release ";
					ret += "of Util-O-Rama.  Version 5.x will be supported with a future release.";
					break;
			}
			return ret;
		}
	} // end Err class

}
