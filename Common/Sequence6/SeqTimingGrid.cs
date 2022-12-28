using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using FileHelper;

namespace LOR4
{
	public class LOR4Timings : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>
	{
		public LOR4.LOR4TimingGridType TimingGridType = LOR4.LOR4TimingGridType.None;
		public int spacing = LOR4Admin.UNDEFINED;
		public List<int> timings = new List<int>();
		public const string FIELDsaveID = " saveID";
		public const string TABLEtiming = "timing";
		public const string FIELDspacing = " spacing";

		//! CONSTRUCTORS
		public LOR4Timings(iLOR4Member theParent, string lineIn)
		{
			myParent = theParent;
			Parse(lineIn);


#if DEBUG
			//string msg = "Timings.LOR4Timings(" + lineIn + ") // Constructor";
			//Debug.WriteLine(msg);
#endif
		}


		//! PROPERTIES, METHODS, ETC.

		public override LOR4MemberType MemberType
		{
			get
			{
				return LOR4MemberType.Timings;
			}
		}

		public override string Name
		{
			get
			{
				string s = myName;
				// If no name has been set, make one up temporarily.
				//   But do not alter the original blank name.
				if (s.Length < 1)
				{
					if (TimingGridType == LOR4TimingGridType.Freeform)
					{
						s = "Freeform Timing Grid " + (SaveID + 1).ToString();
					}
					if (TimingGridType == LOR4TimingGridType.FixedGrid)
					{
						// Note: Not guaranteed to be unique, if more than one fixed grid has the same spacing.
						s = "Fixed Timing Grid " + spacing.ToString() + "cs";
					}
				}
				return s;
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public override string LineOut()
		{
			StringBuilder ret = new StringBuilder();

			ret.Append(LOR4Admin.LEVEL2);
			ret.Append(LOR4Admin.STFLD);
			ret.Append(LOR4Sequence.TABLEtimingGrid);

			ret.Append(LOR4Admin.SetKey(FIELDsaveID, myAltID));
			if (myName.Length > 1)
			{
				ret.Append(LOR4Admin.SetKey(LOR4Admin.FIELDname, LOR4Admin.XMLifyName(myName)));
			}
			ret.Append(LOR4Admin.SetKey(LOR4Admin.FIELDtype, LOR4SeqEnums.TimingName(this.TimingGridType)));
			if (spacing > 1)
			{
				ret.Append(LOR4Admin.SetKey(FIELDspacing, spacing));
			}
			if (this.TimingGridType == LOR4.LOR4TimingGridType.FixedGrid)
			{
				ret.Append(LOR4Admin.ENDFLD);
			}
			else if (this.TimingGridType == LOR4.LOR4TimingGridType.Freeform)
			{
				ret.Append(LOR4Admin.FINFLD);

				//foreach (int tm in timings)
				for (int tm = 0; tm < timings.Count; tm++)
				{
					ret.Append(LOR4Admin.CRLF);
					ret.Append(LOR4Admin.LEVEL3);
					ret.Append(LOR4Admin.STFLD);
					ret.Append(TABLEtiming);

					ret.Append(LOR4Admin.SetKey(LOR4Admin.FIELDcentisecond, timings[tm]));
					ret.Append(LOR4Admin.ENDFLD);
				}

				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.LEVEL2);
				ret.Append(LOR4Admin.FINTBL);
				ret.Append(LOR4Sequence.TABLEtimingGrid);
				ret.Append(LOR4Admin.FINFLD);
			}

			return ret.ToString();
		}

		public override void Parse(string lineIn)
		{
			string seek = LOR4Admin.STFLD + LOR4Sequence.TABLEtimingGrid + FIELDsaveID;
			//int pos = lineIn.IndexOf(seek);
			int pos = LOR4Admin.ContainsKey(lineIn, seek);
			if (pos > 0)
			{
				myName = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDname));
				myID = LOR4Admin.getKeyValue(lineIn, FIELDsaveID);
				Centiseconds = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDcentiseconds);
				this.TimingGridType = LOR4SeqEnums.EnumGridType(LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDtype));
				spacing = LOR4Admin.getKeyValue(lineIn, FIELDspacing);
			}
			else
			{
				myName = lineIn;
			}
			//if (myParent != null) myParent.MakeDirty(true);
		}


		public override iLOR4Member Clone()
		{
			LOR4Timings grid = (LOR4Timings)Clone();
			grid.TimingGridType = this.TimingGridType;
			grid.spacing = spacing;
			if (this.TimingGridType == LOR4.LOR4TimingGridType.Freeform)
			{
				foreach (int time in timings)
				{
					grid.timings.Add(time);
				}
			}
			return grid;
		}

		public override iLOR4Member Clone(string newName)
		{
			LOR4Timings grid = (LOR4Timings)this.Clone();
			ChangeName(newName);
			return grid;
		}


		public int SaveID
		{
			get
			{
				return myID;
			}
		}

		public void SetSaveID(int newSaveID)
		{
			myID = newSaveID;
			myIndex = newSaveID;
		}

		public int AltSaveID
		{ get { return myAltID; } set { myAltID = value; } }


		public void AddTiming(int time)
		{
			if (timings.Count == 0)
			{
				timings.Add(time);
				if (myParent != null) myParent.MakeDirty(true);
			}
			else
			{
				if (timings[timings.Count - 1] < time)
				{
					timings.Add(time);
					if (myParent != null) myParent.MakeDirty(true);
				}
				else
				{
					if (timings.Count > 1)
					{
						if (timings[timings.Count - 2] > timings[timings.Count - 1])
						{
							// Array.Sort uses QuickSort, which is not the most efficient way to do this
							// Most efficient way is a (sort of) one-pass backwards bubble sort
							for (int n = timings.Count - 1; n > 0; n--)
							{
								if (timings[n] < timings[n - 1])
								{
									// Swap
									int temp = timings[n];
									timings[n] = timings[n - 1];
									timings[n - 1] = temp;
								}
							} // end shifting loop

							// Check for, and remove, duplicates
							int offset = 0;
							for (int n = 1; n < timings.Count; n++)
							{
								if (timings[n - 1] == timings[n])
								{
									offset++;
								}
								if (offset > 0)
								{
									timings[n - offset] = timings[n];
								}
							}
							if (offset > 0)
							{
								//itemCount -= offset;
							}
							// end duplicate check/removal
						} // end comparison
					} // end more than one
					if (myParent != null) myParent.MakeDirty(true);
				}
			}
			if (time > myCentiseconds)
			{
				Centiseconds = time;
			}
		}// end addTiming function

		public void Clear()
		{
			timings = new List<int>();
			if (myParent != null) myParent.MakeDirty(true);
		}

		public int CopyTimings(List<int> newTimes, bool merge)
		{
			int count = 0;
			if (!merge)
			{
				timings = new List<int>();
			}
			foreach (int tm in newTimes)
			{
				AddTiming(tm);
				count++;
			}
			if (myParent != null) myParent.MakeDirty(true);
			return count;
		}
		public new int UniverseNumber
		{
			get
			{
				return LOR4Admin.UNDEFINED;
			}
		}
		public override int DMXAddress
		{
			get
			{
				return LOR4Admin.UNDEFINED;
			}
		}


	} // end timingGrid class
}

