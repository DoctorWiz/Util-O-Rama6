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


	public class LOR4Track : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>
	{
		// Tracks are the ultimate top-level groups.  Level 2 and up are handled by 'ChannelGroups'
		// Channel groups are optional, a sequence many not have any groups, but it will always have at least one track
		// Tracks do not have savedIndexes.  They are just numbered instead.
		// Tracks can contain regular Channels, RGB Channels, and Channel Groups, but not other tracks
		// (ie: Tracks cannot be nested like Channel Groups (which can be nested many levels deep))
		// All Channel Groups, regular Channels, and RGB Channels must directly or indirectly belong to one or more tracks.
		// Channels, RGB Channels, and channel groups will not be displayed and will not be accessible unless added to one or
		// more tracks, directly or subdirectly (a subitem of a group in a track).
		// A LOR4Track may not contain more than one copy of the same item-- directly.  Within a subgroup is OK.
		// (See related notes in the LOR4ChannelGroup class)

		public LOR4Membership Members; // = new LOR4Membership(null);
		public List<LOR4LoopLevel> loopLevels = new List<LOR4LoopLevel>();
		public LOR4Timings timingGrid = null;

		//! CONSTRUCTOR
		public LOR4Track(iLOR4Member theParent, string lineIn)
		{
			myParent = theParent;
			Members = new LOR4Membership(this);
			Parse(lineIn);
		}



		//! PROPERTIES, METHODS, ETC.
		public override int Centiseconds
		{
			get
			{
				//return myCentiseconds;
				int cs = 0;
				for (int idx = 0; idx < Members.Count; idx++)
				{
					iLOR4Member mbr = Members[idx];
					if (Members.Items[idx].Centiseconds > cs)
					{
						cs = Members.Items[idx].Centiseconds;
						myCentiseconds = cs;
					}
				}
				return cs;
			}
			set
			{
				if (value != myCentiseconds)
				{
					if (value > LOR4Admin.MAXCentiseconds)
					{
						string m = "WARNING!! Setting Centiseconds to more than 60 minutes!  Are you sure?";
						Fyle.WriteLogEntry(m, "Warning");
						if (Fyle.DebugMode)
						{
							System.Diagnostics.Debugger.Break();
						}
					}
					else
					{
						if (value < LOR4Admin.MINCentiseconds)
						{
							string m = "WARNING!! Setting Centiseconds to less than 1 second!  Are you sure?";
							Fyle.WriteLogEntry(m, "Warning");
							if (Fyle.DebugMode)
							{
								//System.Diagnostics.Debugger.Break();
							}
						}
						else
						{
							myCentiseconds = value;
							for (int idx = 0; idx < Members.Count; idx++)
							{
								iLOR4Member mbr = Members[idx];
								if (Members.Items[idx].Centiseconds > value)
								{
									Members.Items[idx].Centiseconds = value;
								}
							}

							if (myParent != null)
							{
								if (myParent.Centiseconds < value)
								{
									myParent.Centiseconds = value;
								}
							}
							if (myParent != null) myParent.MakeDirty(true);
						}
					}
				}
			}
		}

		public override CheckState SelectedState
		{
			get
			{
				return Members.SelectedState;
			}
			set
			{
				if (value != CheckState.Indeterminate)
				{
					base.SelectedState = value;
					Members.SelectedState = value;
				}
			}
		}


		public override LOR4MemberType MemberType
		{
			get
			{
				return LOR4MemberType.Track;
			}
		}

		public override string Name
		{
			get
			{
				string s = myName;
				// If no name has been set, make one up temporarily.
				//   But do not alter the original blank name
				if (s.Length < 1)
				{
					s = "Track " + TrackNumber.ToString();
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
			return LineOut(false);
		}


		public override void Parse(string lineIn)
		{
			string seek = LOR4Admin.STFLD + LOR4Sequence.TABLEtrack + LOR4Admin.FIELDtotalCentiseconds;
			//int pos = lineIn.IndexOf(seek);
			int pos = LOR4Admin.ContainsKey(lineIn, seek);
			if (pos > 0)
			{
				myName = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDname));
				//mySavedIndex = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDsavedIndex);
				myCentiseconds = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDtotalCentiseconds);
			}
			else
			{
				myName = lineIn;
			}
			int tempGridSaveID = LOR4Admin.getKeyValue(lineIn, LOR4Sequence.TABLEtimingGrid);
			if (tempGridSaveID < 0)
			{
				// For Channel Configs, there will be no timing grid
				timingGrid = null;
			}
			else
			{
				// Assign the LOR4Timings based on the SaveID
				//iLOR4Member member = myParent.Members.bySaveID[tempGridSaveID];
				LOR4Timings tg = null;
				LOR4Sequence mySeq = (LOR4Sequence)myParent;
				for (int i = 0; i < mySeq.TimingGrids.Count; i++)
				{
					if (mySeq.TimingGrids[i].SaveID == tempGridSaveID)
					{
						tg = mySeq.TimingGrids[i];
						i = mySeq.TimingGrids.Count; // Loopus Interruptus
					}
				}
				if (tg == null)
				{
					string msg = "ERROR: Timing Grid with SaveID of " + tempGridSaveID.ToString() + " not found!";
					//System.Diagnostics.Debugger.Break();
					tg = mySeq.TimingGrids[0];
				}
				timingGrid = tg;
			}
			//if (myParent != null) myParent.MakeDirty(true);
		}


		public override iLOR4Member Clone()
		{
			LOR4Track tr = (LOR4Track)Clone();
			tr.timingGrid = (LOR4Timings)timingGrid.Clone();
			return tr;
		}

		public override iLOR4Member Clone(string newName)
		{
			LOR4Track tr = (LOR4Track)this.Clone();
			ChangeName(newName);
			return tr;
		}

		public int TrackNumber
		{
			// Read-Only!
			get
			{
				// LOR4Track numbers are one based, the index is zero based, so just add 1 to the index for the track number
				return myID + 1;
			}
		}

		public int TrackID
		{ get { return myID; } }

		public void SetTrackID(int newID)
		{ myID = newID; }

		public int AltTrackID
		{ get { return myAltID; } set { myAltID = value; } }


		public string LineOut(bool selectedOnly)
		{
			StringBuilder ret = new StringBuilder();
			// Write info about track
			ret.Append(LOR4Admin.LEVEL2);
			ret.Append(LOR4Admin.STFLD);
			ret.Append(LOR4Sequence.TABLEtrack);
			//! LOR writes it with the Name last
			// In theory, it shouldn't matter
			//if (Name.Length > 1)
			//{
			//	ret += LOR4Admin.SPC + FIELDname + LOR4Admin.FIELDEQ + Name + LOR4Admin.ENDQT;
			//}
			ret.Append(LOR4Admin.FIELDtotalCentiseconds);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(myCentiseconds.ToString());
			ret.Append(LOR4Admin.ENDQT);

			int altID = timingGrid.AltSaveID;
			ret.Append(LOR4Admin.SPC);
			ret.Append(LOR4Sequence.TABLEtimingGrid);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(altID.ToString());
			ret.Append(LOR4Admin.ENDQT);
			// LOR writes it with the Name last
			if (myName.Length > 1)
			{
				ret.Append(LOR4Admin.FIELDname);
				ret.Append(LOR4Admin.FIELDEQ);
				ret.Append(LOR4Admin.XMLifyName(myName));
				ret.Append(LOR4Admin.ENDQT);
			}
			ret.Append(LOR4Admin.FINFLD);

			ret.Append(LOR4Admin.CRLF);
			ret.Append(LOR4Admin.LEVEL3);
			ret.Append(LOR4Admin.STFLD);
			ret.Append(LOR4Admin.TABLEchannel);
			ret.Append(LOR4Admin.PLURAL);
			ret.Append(LOR4Admin.FINFLD);

			// LOR4Loop thru all items in this track
			foreach (iLOR4Member subMember in Members.Items)
			{
				bool sel = (subMember.SelectedState == CheckState.Checked);
				if (!selectedOnly || sel)
				{
					// Write out the links to the items
					//destSI = updatedTracks[trackIndex].newSavedIndexes[iti];

					//if (subMember.Name.IndexOf("lyphonic") > 0) System.Diagnostics.Debugger.Break();

					int siAlt = subMember.AltID;
					if (siAlt > LOR4Admin.UNDEFINED)
					{
						ret.Append(LOR4Admin.CRLF);
						ret.Append(LOR4Admin.LEVEL4);
						ret.Append(LOR4Admin.STFLD);
						ret.Append(LOR4Admin.TABLEchannel);

						ret.Append(LOR4Admin.FIELDsavedIndex);
						ret.Append(LOR4Admin.FIELDEQ);
						ret.Append(siAlt.ToString());
						ret.Append(LOR4Admin.ENDQT);
						ret.Append(LOR4Admin.ENDFLD);
					}
				}
			}

			// Close the list of items
			ret.Append(LOR4Admin.CRLF);
			ret.Append(LOR4Admin.LEVEL3);
			ret.Append(LOR4Admin.FINTBL);
			ret.Append(LOR4Admin.TABLEchannel);
			ret.Append(LOR4Admin.PLURAL);
			ret.Append(LOR4Admin.FINFLD);

			// Write out any LoopLevels in this track
			//writeLoopLevels(trackIndex);
			if (loopLevels.Count > 0)
			{
				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.LEVEL3);
				ret.Append(LOR4Admin.STFLD);
				ret.Append(LOR4Sequence.TABLEloopLevels);
				ret.Append(LOR4Admin.FINFLD);
				foreach (LOR4LoopLevel ll in loopLevels)
				{
					ret.Append(LOR4Admin.CRLF);
					ret.Append(ll.LineOut());
				}
				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.LEVEL3);
				ret.Append(LOR4Admin.FINTBL);
				ret.Append(LOR4Sequence.TABLEloopLevels);
				ret.Append(LOR4Admin.FINFLD);
			}
			else
			{
				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.LEVEL3);
				ret.Append(LOR4Admin.STFLD);
				ret.Append(LOR4Sequence.TABLEloopLevels);
				ret.Append(LOR4Admin.ENDFLD);
			}
			ret.Append(LOR4Admin.CRLF);
			ret.Append(LOR4Admin.LEVEL2);
			ret.Append(LOR4Admin.FINTBL);
			ret.Append(LOR4Sequence.TABLEtrack);
			ret.Append(LOR4Admin.FINFLD);


			return ret.ToString();
		}

		public int AddItem(iLOR4Member newPart)
		{
			int retSI = LOR4Admin.UNDEFINED;
			bool alreadyAdded = false;
			for (int i = 0; i < Members.Count; i++)
			{
				if (newPart.ID == Members.Items[i].ID)
				{
					//TODO: Using saved index, look up Name of item being added
					string sMsg = newPart.Name + " has already been added to this LOR4Track '" + myName + "'.";
					//DialogResult rs = MessageBox.Show(sMsg, "Channel Groups", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					if (System.Diagnostics.Debugger.IsAttached)
						//System.Diagnostics.Debugger.Break();
						//TODO: Make this just a warning, put "add" code below into an else block
						//TODO: Do the same with Tracks
						alreadyAdded = true;
					retSI = newPart.ID;
					i = Members.Count; // Break out of loop
				}
			}
			if (!alreadyAdded)
			{
				retSI = Members.Add(newPart);
				if (myParent != null) myParent.MakeDirty(true);
			}
			return retSI;
		}

		public int AddItem(int itemSavedIndex)
		{
			int ret = LOR4Admin.UNDEFINED;
			if (myParent != null)
			{
				LOR4Sequence mySeq = (LOR4Sequence)myParent;
				iLOR4Member newItem = mySeq.AllMembers.FindBySavedIndex(itemSavedIndex);
				if (newItem != null)
				{
					ret = AddItem(newItem);
					myParent.MakeDirty(true);
				}
				else
				{
					if (Fyle.DebugMode)
					{
						// Trying to add a member which does not exist!
						System.Diagnostics.Debugger.Break();
					}
				}
			}
			return ret;
		}

		public LOR4LoopLevel AddLoopLevel(string lineIn)
		{
			LOR4LoopLevel newLL = new LOR4LoopLevel(lineIn);
			AddLoopLevel(newLL);
			if (myParent != null) myParent.MakeDirty(true);
			return newLL;
		}

		public int AddLoopLevel(LOR4LoopLevel newLL)
		{
			loopLevels.Add(newLL);
			if (myParent != null) myParent.MakeDirty(true);
			return loopLevels.Count - 1;
		}

		public override int UniverseNumber
		{
			get
			{
				if (Members.Count > 0)
				{
					return Members[0].UniverseNumber;
				}
				else
				{
					return 0;
				}
			}
		}
		public override int DMXAddress
		{
			get
			{
				if (Members.Count > 0)
				{
					return Members[0].DMXAddress;
				}
				else
				{
					return 0;
				}
			}
		}

		public override int color
		{
			get
			{
				if (Members.Count > 0)
				{
					return Members[0].color;
				}
				else
				{
					return 0;
				}
			}
			set
			{
				int ignore = value;
			}
		}

		public override Color Color
		{
			get { return LOR4Admin.Color_LORtoNet(this.color); }
			set { Color ignore = value; }
		}

		//TODO: add RemoveItem procedure
	} // end class track
}
