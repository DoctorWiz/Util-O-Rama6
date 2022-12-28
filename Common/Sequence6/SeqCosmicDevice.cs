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



	public class LOR4Cosmic : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>
	{
		// Cosmic Color Devices are basically the same thing as ChannelGroups
		// Channel Groups and Cosmic Devices are Level 2 and Up, Level 1 is the Tracks (which are similar to a group)
		// Cosmic Devices can (?!?!) contain regular Channels, RGB Channels, and other groups.
		// Cosmic Devices can (?!?!) be nested many levels deep (limit?).
		// See additional notes in the LOR4ChannelGroup class

		public const string TABLEcosmicDeviceDevice = "colorCosmicDevice";
		private const string STARTcosmicDevice = LOR4Admin.STFLD + TABLEcosmicDeviceDevice + LOR4Admin.SPC;
		public LOR4Membership Members; // = new LOR4Membership(this);

		//! CONSTRUCTORS
		public LOR4Cosmic(string theName, int theSavedIndex)
		{
			myName = theName;
			myID = theSavedIndex;
			Members = new LOR4Membership(this);
		}

		public LOR4Cosmic(iLOR4Member theParent, string lineIn)
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
				return myCentiseconds;
			}
			set
			{
				if (value != myCentiseconds)
				{
					myCentiseconds = value;
					for (int idx = 0; idx < Members.Count; idx++)
					{
						Members.Items[idx].Centiseconds = value;
					}
					if (myParent != null) myParent.MakeDirty(true);

					//if (myCentiseconds > Parent.Centiseconds)
					//{
					//	Parent.Centiseconds = value;
					//}
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
				return LOR4MemberType.Cosmic;
			}
		}


		public override string LineOut()
		{
			return LineOut(false);
		}

		public override void Parse(string lineIn)
		{
			string seek = LOR4Admin.STFLD + LOR4Sequence.TABLEcosmicDevice + LOR4Admin.FIELDtotalCentiseconds;
			//int pos = lineIn.IndexOf(seek);
			int pos = LOR4Admin.ContainsKey(lineIn, seek);
			if (pos > 0)
			{
				myName = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDname));
				myID = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDsavedIndex);
				//Members = new LOR4Membership(this);
				myCentiseconds = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDtotalCentiseconds);
			}
			else
			{
				if (lineIn.Length > 1)
				{
					myName = lineIn;
				}
			}
			//if (myParent != null) myParent.MakeDirty(true);
		}

		public override iLOR4Member Clone()
		{
			LOR4Cosmic cosm = (LOR4Cosmic)Clone();
			return cosm;
		}

		public override iLOR4Member Clone(string newName)
		{
			// Returns an EMPTY group with same name, index, centiseconds, etc.
			LOR4Cosmic cosm = (LOR4Cosmic)this.Clone();
			ChangeName(newName);
			return cosm;
		}



		public string LineOut(bool selectedOnly)
		{
			StringBuilder ret = new StringBuilder();

			ret.Append(LOR4Admin.LEVEL2);
			ret.Append(LOR4Admin.STFLD);
			ret.Append(LOR4Sequence.TABLEcosmicDevice);

			ret.Append(LOR4Admin.FIELDtotalCentiseconds);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(myCentiseconds.ToString());
			ret.Append(LOR4Admin.ENDQT);

			ret.Append(LOR4Admin.FIELDname);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(LOR4Admin.XMLifyName(myName));
			ret.Append(LOR4Admin.ENDQT);

			ret.Append(LOR4Admin.FIELDsavedIndex);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(myAltID.ToString());
			ret.Append(LOR4Admin.ENDQT);
			ret.Append(LOR4Admin.FINFLD);

			ret.Append(LOR4Admin.CRLF); ;
			ret.Append(LOR4Admin.STFLD);
			ret.Append(LOR4Sequence.TABLEchannelGroup);
			ret.Append(LOR4Admin.PLURAL);
			ret.Append(LOR4Admin.FINFLD);

			foreach (iLOR4Member member in Members.Items)
			{
				int osi = member.ID;
				int asi = member.AltID;
				if (asi > LOR4Admin.UNDEFINED)
				{
					ret.Append(LOR4Admin.CRLF);
					ret.Append(LOR4Admin.LEVEL4);
					ret.Append(LOR4Admin.STFLD);
					ret.Append(LOR4Sequence.TABLEchannelGroup);

					ret.Append(LOR4Admin.FIELDsavedIndex);
					ret.Append(LOR4Admin.FIELDEQ);
					ret.Append(asi.ToString());
					ret.Append(LOR4Admin.ENDQT);
					ret.Append(LOR4Admin.ENDFLD);
				}
			}
			ret.Append(LOR4Admin.CRLF);
			ret.Append(LOR4Admin.LEVEL3);
			ret.Append(LOR4Admin.FINTBL);
			ret.Append(LOR4Sequence.TABLEchannelGroup);
			ret.Append(LOR4Admin.PLURAL);
			ret.Append(LOR4Admin.FINFLD);

			ret.Append(LOR4Admin.CRLF);
			ret.Append(LOR4Admin.LEVEL2);
			ret.Append(LOR4Admin.FINTBL);
			ret.Append(LOR4Sequence.TABLEcosmicDevice);
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
					string sMsg = newPart.Name + " has already been added to this Channel Group '" + myName + "'.";
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
			// Adds an EXISTING item to this Group's membership
			int ret = LOR4Admin.UNDEFINED;
			if (myParent != null)
			{
				LOR4Sequence mySeq = (LOR4Sequence)myParent;
				iLOR4Member newPart = mySeq.AllMembers.BySavedIndex[itemSavedIndex];
				ret = AddItem(newPart);
			}
			return ret;
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

		//public int SavedIndex
		//{ get { return myID; } }
		//public void SetSavedIndex(int newSavedIndex)
		//{ myID = newSavedIndex; }
		//public int AltSavedIndex
		//{ get { return myAltID; } set { myAltID = value; } }

		//TODO: add RemoveItem procedure
	}
}
