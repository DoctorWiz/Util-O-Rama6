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

	public class LOR4Channel : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>
	{
		private const string STARTchannel = LOR4Admin.STFLD + LOR4Admin.TABLEchannel + LOR4Admin.FIELDname;
		//public int color = 0;
		public LOR4Output output = null;
		public LOR4RGBChild rgbChild = LOR4RGBChild.None;
		public LOR4RGBChannel rgbParent = null;
		public List<LOR4Effect> effects = new List<LOR4Effect>();
		public const string FIELDcolor = " color";

		//! CONSTRUCTORS
		/*public LOR4Channel(string theName, int theSavedIndex)
		{
			myName = theName;
			output = new LOR4Output(this);
			mySavedIndex = theSavedIndex;
		}
		*/
		public LOR4Channel(iLOR4Member theParent, string lineIn)
		{
			myParent = theParent;
			output = new LOR4Output(this);
			Parse(lineIn);
		}


		/*
		public LOR4Channel(string lineIn)
		{
			output = new LOR4Output(this);
			string seek = LOR4Admin.STFLD + LOR4Admin.TABLEchannel + LOR4Admin.FIELDname;
			//int pos = lineIn.IndexOf(seek);
			int pos = LOR4Admin.ContainsKey(lineIn, seek);
			if (pos > 0)
			{
				Parse(lineIn);
			}
			else
			{
				if (lineIn.Length > 0)
				{
					myName = lineIn;
				}
			}
		}
		*/

		//! METHODS, PROPERTIES, ETC.

		public override LOR4MemberType MemberType
		{ get { return LOR4MemberType.Channel; } }
		public override int UniverseNumber
		{ get { return output.UniverseNumber; } }
		public override int DMXAddress
		{ get { return output.DMXAddress; } }

		//public int SavedIndex
		//{ get { return myID; } }
		//public void SetSavedIndex(int newSavedIndex)
		//{ myID = newSavedIndex; }
		//public int AltSavedIndex
		//{ get { return myAltID;} set { myAltID = value; } }

		public CheckState SelectedState
		{
			get { return base.SelectedState; }
			set { base.SelectedState = value; }

		}

		public override int CompareTo(iLOR4Member other)
		{
			int result = 0;
			if (LOR4Membership.sortMode == LOR4Membership.SORTbyOutput)
			{
				Type t = other.GetType();
				if (t == typeof(LOR4Channel))
				{
					LOR4Channel ch = (LOR4Channel)other;
					result = output.ToString().CompareTo(ch.output.ToString());
				}
				if (t == typeof(LOR4VizChannel))
				{
					LOR4VizChannel ch = (LOR4VizChannel)other;
					result = output.ToString().CompareTo(ch.output.ToString());
				}
			}
			else
			{
				result = base.CompareTo(other);
			}
			return result;
		}

		// I'm not a big fan of case sensitivity, but I'm gonna take advantage of it here
		// color with lower c is the LOR color, a 32 bit int in BGR order
		// Color with capital C is the .Net Color object (aka Web Color)(In ARGB order)

		public override string LineOut()
		{
			return LineOut(false);
		}

		public override void Parse(string lineIn)
		{
			string seek = LOR4Admin.STFLD + LOR4Admin.TABLEchannel + LOR4Admin.FIELDname;
			int pos = LOR4Admin.ContainsKey(lineIn, seek);
			if (pos > 0)
			{
				myName = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDname));
				myID = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDsavedIndex);
				color = (int)LOR4Admin.getKeyValue(lineIn, FIELDcolor);
				myCentiseconds = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDcentiseconds);

				//! NOT supported by ShowTime.  Will not exist in sequence files saved from ShowTime (return blank "")
				// Preserved only in sequence files saved in Util-O-Rama
				// Only intended for temporary use within Util-O-Rama anyway so no big deal
				myComment = LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDcomment);

				output.Parse(lineIn);
			}
			else
			{
				if (lineIn.Length > 0)
				{
					myName = lineIn;
				}
			}
			//LOR4Sequence Parent = ID.Parent;
			//if (myParent != null) myParent.MakeDirty(true);
		}

		public override iLOR4Member Clone()
		{
			// See Also: Clone(), CopyTo(), and CopyFrom()
			LOR4Channel chan = (LOR4Channel)base.Clone();
			chan.color = color;
			chan.output = output.Clone();
			chan.rgbChild = this.rgbChild;
			chan.rgbParent = rgbParent; // should be changed/overridden
			chan.effects = CloneEffects();
			return chan;
		}

		public override iLOR4Member Clone(string newName)
		{
			//LOR4Channel chan = new LOR4Channel(newName, LOR4Admin.UNDEFINED);
			LOR4Channel chan = (LOR4Channel)base.Clone();
			chan.ChangeName(newName);
			return chan;
		}

		public List<LOR4Effect> CloneEffects()
		{
			List<LOR4Effect> newList = new List<LOR4Effect>();
			foreach (LOR4Effect ef in effects)
			{
				if (ef.endCentisecond > ef.startCentisecond)
				{
					LOR4Effect F = ef.Clone();
					newList.Add(F);
				}
				else
				{
					Fyle.MakeNoise(Fyle.Noises.Pop);
				}
			}
			return newList;
		}

		public string LineOut(bool noEffects)
		{
			StringBuilder ret = new StringBuilder();
			ret.Append(LOR4Admin.LEVEL2);
			ret.Append(LOR4Admin.STFLD);
			ret.Append(LOR4Admin.TABLEchannel);

			ret.Append(LOR4Admin.FIELDname);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(LOR4Admin.XMLifyName(myName));
			ret.Append(LOR4Admin.ENDQT);

			ret.Append(FIELDcolor);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(color.ToString());
			ret.Append(LOR4Admin.ENDQT);

			ret.Append(LOR4Admin.FIELDcentiseconds);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(myCentiseconds.ToString());
			ret.Append(LOR4Admin.ENDQT);

			ret.Append(output.LineOut());

			ret.Append(LOR4Admin.FIELDsavedIndex);
			ret.Append(LOR4Admin.FIELDEQ);
			ret.Append(myAltID.ToString());
			ret.Append(LOR4Admin.ENDQT);


			//! EXPERIMENTAL-- MAY CRASH SHOWTIME
			//ret.Append(LOR4Admin.FIELDcomment);
			//ret.Append(LOR4Admin.FIELDEQ);
			//ret.Append(myComment);
			//ret.Append(LOR4Admin.ENDQT);
			//! Yup!  Error "Unexpected save file information found" when trying to open the file.
			//! So much for being able to save any extraneous data.


			// Are there any effects for this channel?
			if (!noEffects && (effects.Count > 0))
			{
				// complete channel line with regular '>' then do effects
				ret.Append(LOR4Admin.FINFLD);
				foreach (LOR4Effect thisEffect in effects)
				{
					ret.Append(LOR4Admin.CRLF);
					ret.Append(thisEffect.LineOut());
				}
				ret.Append(LOR4Admin.CRLF);
				ret.Append(LOR4Admin.LEVEL2);
				ret.Append(LOR4Admin.FINTBL);
				ret.Append(LOR4Admin.TABLEchannel);
				ret.Append(LOR4Admin.FINFLD);
			}
			else // NO effects for this channal
			{
				// complete channel line with field end '/>'
				ret.Append(LOR4Admin.ENDFLD);
			}

			return ret.ToString();
		}


		public void CopyTo(LOR4Channel destination, bool withEffects)
		{
			if (destination.color == 0) destination.color = color;
			if (destination.Name.Length == 0) destination.ChangeName(myName);
			if (destination.myParent == null) destination.myParent = this.myParent;
			//if (destination.output.deviceType == LOR4DeviceType.None)
			//{
			destination.output.deviceType = output.deviceType;
			destination.output.circuit = output.circuit;
			destination.output.network = output.network;
			destination.output.unit = output.unit;
			//}
			if (withEffects)
			{
				//destination.Centiseconds = myCentiseconds;
				//foreach (LOR4Effect thisEffect in effects)
				//{
				//	destination.effects.Add(thisEffect.Clone());
				//}
				destination.effects = CloneEffects();
			}
		}

		public void CopyFrom(LOR4Channel source, bool withEffects)
		{
			if (color == 0) color = source.color;
			if (myName.Length == 0) ChangeName(source.Name);
			if (this.myParent == null) this.myParent = source.myParent;
			//if (output.deviceType == LOR4DeviceType.None)
			//{
			output.deviceType = source.output.deviceType;
			output.circuit = source.output.circuit;
			output.network = source.output.network;
			output.unit = source.output.unit;
			//}
			if (withEffects)
			{
				//myCentiseconds = source.Centiseconds;
				//foreach (LOR4Effect theEffect in source.effects)
				//{
				//effects.Add(theEffect.Clone());
				//	AddEffect(theEffect.Clone());
				//}
				effects = source.CloneEffects();
			}
		}

		public LOR4Channel Clone(bool withEffects)
		{
			// See Also: Duplicate()
			//int nextSI = ID.Parent.Members.highestSavedIndex + 1;
			LOR4Channel ret = new LOR4Channel(this, myName);
			ret.color = color;
			ret.output = output;
			ret.rgbChild = rgbChild;
			ret.rgbParent = rgbParent;
			ret.SetParent(Parent);
			List<LOR4Effect> newEffects = new List<LOR4Effect>();
			if (withEffects)
			{
				//foreach (LOR4Effect thisEffect in effects)
				//{
				//	newEffects.Add(thisEffect.Clone());
				//}
				//ret.effects = newEffects;
				ret.effects = CloneEffects();
			}
			return ret;
		}

		public void AddEffect(LOR4Effect theEffect)
		{
			theEffect.parent = this;
			theEffect.myIndex = effects.Count;
			effects.Add(theEffect);
			if (theEffect.endCentisecond > myCentiseconds)
			{
				Centiseconds = theEffect.endCentisecond;
			}
			if (myParent != null) myParent.MakeDirty(true);
			//ID.Parent.dirty = true;
		}

		public void AddEffect(string lineIn)
		{
			LOR4Effect theEffect = new LOR4Effect(lineIn);
			AddEffect(theEffect);
			if (myParent != null) myParent.MakeDirty(true);
		}

		public int CopyEffects(List<LOR4Effect> effectList, bool Merge)
		{
			LOR4Effect newEffect = null;
			if (!Merge)
			{
				//! Note: clears any pre-existing effects!
				effects.Clear();
			}
			foreach (LOR4Effect thisEffect in effectList)
			{
				newEffect = thisEffect.Clone();
				newEffect.parent = this;
				newEffect.myIndex = effects.Count;
				effects.Add(newEffect);
				Parent.MakeDirty(true);
			}
			if (Merge)
			{
				effects.Sort();
			}
			// Return how many effects were copied
			if (myParent != null) myParent.MakeDirty(true);
			return effects.Count;
		}

		//TODO: add RemoveEffect procedure
		//TODO: add SortEffects procedure (by startCentisecond)







	} // end channel
}
