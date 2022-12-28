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

	//////////////////////////////////////////////
	// 
	//		RGB CHANNEL
	//
	////////////////////////////////////////////////

	public class LOR4RGBChannel : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>
	{
		public LOR4Channel redChannel = null;
		public LOR4Channel grnChannel = null;
		public LOR4Channel bluChannel = null;

		//! CONSTRUCTOR(s)
		/*
		public LOR4RGBChannel(string lineIn)
		{
			string seek = LOR4Admin.STFLD + LOR4Sequence.TABLErgbChannel + LOR4Admin.FIELDtotalCentiseconds;
			//int pos = lineIn.IndexOf(seek);
			int pos = LOR4Admin.ContainsKey(lineIn, seek);
			if (pos > 0)
			{
				Parse(lineIn);
			}
			else
			{
				myName = lineIn;
			}
		}
		*/
		public LOR4RGBChannel(iLOR4Member theParent, string lineIn)
		{
			myParent = theParent;
			Parse(lineIn);
		}

		/*
		public LOR4RGBChannel(string lineIn, int theSavedIndex)
		{
			string seek = LOR4Admin.STFLD + LOR4Sequence.TABLErgbChannel + LOR4Admin.FIELDtotalCentiseconds;
			//int pos = lineIn.IndexOf(seek);
			int pos = LOR4Admin.ContainsKey(lineIn, seek);
			if (pos > 0)
			{
				Parse(lineIn);
			}
			else
			{
				myName = lineIn;
			}
			SetSavedIndex(theSavedIndex);
		}
		*/

		//! OTHER PROPERTIES, METHODS, ETC.
		public override int Centiseconds
		{
			get
			{
				int cs = 0;
				if (redChannel != null)
				{
					cs = redChannel.Centiseconds;
				}
				if (grnChannel != null)
				{
					if (grnChannel.Centiseconds > cs) cs = grnChannel.Centiseconds;
				}
				if (bluChannel != null)
				{
					if (bluChannel.Centiseconds > cs) cs = bluChannel.Centiseconds;
				}
				myCentiseconds = cs;
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
								System.Diagnostics.Debugger.Break();
							}
						}
						else
						{
							myCentiseconds = value;
							if (redChannel != null)
							{
								if (redChannel.Centiseconds > value) redChannel.Centiseconds = value;
							}
							if (grnChannel != null)
							{
								if (grnChannel.Centiseconds > value) grnChannel.Centiseconds = value;
							}
							if (bluChannel != null)
							{
								if (bluChannel.Centiseconds > value) bluChannel.Centiseconds = value;
							}

							//if (parentSequence != null) parentSequence.MakeDirty(true);

							//if (myCentiseconds > ParentSequence.Centiseconds)
							//{
							//	ParentSequence.Centiseconds = value;
							//}
						}
					}
				}
			}
		}

		public override int color
		{
			get { return LOR4Admin.LORCOLOR_RGB; }
			set { int ignore = value; }
		}

		public override Color Color
		{
			get { return LOR4Admin.Color_LORtoNet(this.color); }
			set { Color ignore = value; }
		}

		public override CheckState SelectedState
		{
			get
			{
				// Default
				CheckState state = CheckState.Indeterminate;
				// Until we prove otherwise...
				bool all = true;
				bool some = false;
				// Are ANY of the 3 subcolors selected?
				if ((redChannel.SelectedState == CheckState.Checked) ||
				 (redChannel.SelectedState == CheckState.Checked) ||
				 (redChannel.SelectedState == CheckState.Checked))
				{
					// 'Some' must be true
					some = true;
				}
				// Are ANY of them unselected?
				if ((redChannel.SelectedState == CheckState.Unchecked) ||
				 (redChannel.SelectedState == CheckState.Unchecked) ||
				 (redChannel.SelectedState == CheckState.Unchecked))
				{
					// 'All' can't be true
					all = false;
				}
				if (all)
				{
					// All 3 subcolors selected, so RGB parent is selected
					state = CheckState.Checked;
				}
				else
				{
					if (!some)
					{
						// None of the subcolors selected so RGB parent is unselected
						state = CheckState.Unchecked;
					}
				}
				return state;
			}
			set
			{
				if (value != CheckState.Indeterminate)
				{
					base.SelectedState = value;
					redChannel.SelectedState = value;
					grnChannel.SelectedState = value;
					bluChannel.SelectedState = value;
				}
			}
		}

		public override LOR4MemberType MemberType
		{
			get
			{
				return LOR4MemberType.RGBChannel;
			}
		}

		public override string LineOut()
		{
			//! FullTrack??
			return LineOut(false, false, LOR4MemberType.FullTrack);
		}

		public override void Parse(string lineIn)
		{
			string seek = LOR4Admin.STFLD + LOR4Sequence.TABLErgbChannel + LOR4Admin.FIELDtotalCentiseconds;
			int pos = LOR4Admin.ContainsKey(lineIn, seek);
			if (pos > 0)
			{
				myName = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDname));
				myID = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDsavedIndex);
				myCentiseconds = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDtotalCentiseconds);
			}
			else
			{
				myName = lineIn;
			}
			//if (myParent != null) myParent.MakeDirty(true);
		}


		public override iLOR4Member Clone()
		{
			LOR4RGBChannel rgb = (LOR4RGBChannel)this.Clone();
			rgb.redChannel = (LOR4Channel)redChannel.Clone();
			rgb.grnChannel = (LOR4Channel)grnChannel.Clone();
			rgb.bluChannel = (LOR4Channel)bluChannel.Clone();
			return rgb;
		}

		public override iLOR4Member Clone(string newName)
		{
			LOR4RGBChannel rgb = (LOR4RGBChannel)this.Clone();  //   new LOR4RGBChannel(newName, LOR4Admin.UNDEFINED);
			rgb.ChangeName(newName);
			return rgb;
		}

		public string LineOut(bool selectedOnly, bool noEffects, LOR4MemberType itemTypes)
		{
			StringBuilder ret = new StringBuilder();

			//int redSavedIndex = LOR4Admin.UNDEFINED;
			//int grnSavedIndex = LOR4Admin.UNDEFINED;
			//int bluSavedIndex = LOR4Admin.UNDEFINED;

			int AltSavedIndex = LOR4Admin.UNDEFINED;

			if ((itemTypes == LOR4MemberType.Items) || (itemTypes == LOR4MemberType.Channel))
			// Type NONE actually means ALL in this case
			{
				// not checking .Selected flag 'cuz if parent LOR4RGBChannel is Selected 
				//redSavedIndex = ID.ParentSequence.WriteChannel(redChannel, noEffects);
				//grnSavedIndex = ID.ParentSequence.WriteChannel(grnChannel, noEffects);
				//bluSavedIndex = ID.ParentSequence.WriteChannel(bluChannel, noEffects);
			}

			if ((itemTypes == LOR4MemberType.Items) || (itemTypes == LOR4MemberType.RGBChannel))
			{
				ret.Append(LOR4Admin.LEVEL2);
				ret.Append(LOR4Admin.STFLD);
				ret.Append(LOR4Sequence.TABLErgbChannel);
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

				// Start SubChannels
				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.LEVEL3);
				ret.Append(LOR4Admin.STFLD);
				ret.Append(LOR4Admin.TABLEchannel);
				ret.Append(LOR4Admin.PLURAL);
				ret.Append(LOR4Admin.FINFLD);

				// RED subchannel
				AltSavedIndex = redChannel.AltSavedIndex;
				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.LEVEL4);
				ret.Append(LOR4Admin.STFLD);
				ret.Append(LOR4Admin.TABLEchannel);
				ret.Append(LOR4Admin.FIELDsavedIndex);
				ret.Append(LOR4Admin.FIELDEQ);
				ret.Append(AltSavedIndex.ToString());
				ret.Append(LOR4Admin.ENDQT);
				ret.Append(LOR4Admin.ENDFLD);

				// GREEN subchannel
				AltSavedIndex = grnChannel.AltSavedIndex;
				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.LEVEL4);
				ret.Append(LOR4Admin.STFLD);
				ret.Append(LOR4Admin.TABLEchannel);
				ret.Append(LOR4Admin.FIELDsavedIndex);
				ret.Append(LOR4Admin.FIELDEQ);
				ret.Append(AltSavedIndex.ToString());
				ret.Append(LOR4Admin.ENDQT);
				ret.Append(LOR4Admin.ENDFLD);

				// BLUE subchannel
				AltSavedIndex = bluChannel.AltSavedIndex;
				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.LEVEL4);
				ret.Append(LOR4Admin.STFLD);
				ret.Append(LOR4Admin.TABLEchannel);
				ret.Append(LOR4Admin.FIELDsavedIndex);
				ret.Append(LOR4Admin.FIELDEQ);
				ret.Append(AltSavedIndex.ToString());
				ret.Append(LOR4Admin.ENDQT);
				ret.Append(LOR4Admin.ENDFLD);

				// End SubChannels
				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.LEVEL3);
				ret.Append(LOR4Admin.FINTBL);
				ret.Append(LOR4Admin.TABLEchannel);
				ret.Append(LOR4Admin.PLURAL);
				ret.Append(LOR4Admin.FINFLD);

				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.LEVEL2);
				ret.Append(LOR4Admin.FINTBL);
				ret.Append(LOR4Sequence.TABLErgbChannel);
				ret.Append(LOR4Admin.FINFLD);
			} // end LOR4MemberType Channel or LOR4RGBChannel

			return ret.ToString();
		} // end LineOut

		public override int UniverseNumber
		{
			get
			{
				int ret = 0;
				if (redChannel != null)
				{
					ret = redChannel.output.network;
				}
				return ret;
			}
		}
		public override int DMXAddress
		{
			get
			{
				int ret = 0;
				if (redChannel != null)
				{
					ret = redChannel.output.channel;
				}
				return ret;
			}
		}

		//public int SavedIndex
		//{ get { return myID; } }
		//public void SetSavedIndex(int newSavedIndex)
		//{ myID = newSavedIndex; }
		//public int AltSavedIndex
		//{ get { return myAltID; } set { myAltID = value; } }



	} // end LOR4RGBChannel Class
}
