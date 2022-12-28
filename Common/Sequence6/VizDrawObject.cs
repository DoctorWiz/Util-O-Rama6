using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using FileHelper;

namespace LOR4
{

	public class LOR4VizDrawObject : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>
	{
		public bool isRGB = false;
		public int BulbSize = LOR4Admin.UNDEFINED;
		public int BulbSpacing = LOR4Admin.UNDEFINED;
		//public string Comment = ""; // Now included in base class
		public int BulbShape = LOR4Admin.UNDEFINED;  //? Enum?
		public int ZOrder = 0;
		public int AssignedItem = LOR4Admin.UNDEFINED;
		public bool Locked = false;
		public int FixtureType = LOR4Admin.UNDEFINED; //? Enum?
		public int ChannelType = LOR4Admin.UNDEFINED; //? Enum?
		public int MaxOpacity = 0; // Percent?
		public LOR4VizChannel redChannel = null;
		public LOR4VizChannel grnChannel = null;
		public LOR4VizChannel bluChannel = null;

		private static readonly string FIELDbulbSpacing = " BulbSpacing";
		private static readonly string FIELDcomment = " Comment";
		private static readonly string FIELDbulbShape = " BulbShape";
		private static readonly string FIELDzOrder = " ZOrder";
		private static readonly string FIELDassignedItem = " AssignedItem";
		private static readonly string FIELDlocked = " Locked";
		private static readonly string FIELDfixtureType = " Fixture_Type";
		private static readonly string FIELDchannelType = " Channel_Type";
		private static readonly string FIELDmaxOpacity = " Max_Opacity";

		public LOR4VizDrawObject(iLOR4Member theParent, string lineIn)
		{
			myParent = theParent;
			Parse(lineIn);
		}

		public LOR4VizChannel subChannel
		{
			get
			{
				return redChannel;
			}
			set
			{
				redChannel = value;
			}
		}

		public int DrawObjectID
		{ get { return myID; } }
		public void SetDrawObjectID(int newObjectID)
		{ myID = newObjectID; }
		public int AltDrawObjectID
		{ get { return myAltID; } set { myAltID = value; } }
		public override LOR4MemberType MemberType
		{ get { return LOR4MemberType.VizDrawObject; } }

		public override int UniverseNumber
		{
			get
			{
				int ret = LOR4Admin.UNDEFINED;
				if (redChannel == null)
				{
					string m1 = "WTF does DrawObject '" + myName + "' not have a red channel?!";
					Fyle.BUG(m1);
				}
				else
				{
					string redName = redChannel.Name;
					if (redChannel.output == null)
					{
						string m2 = "WTF does " + myName + "'s red channel '" + redName + "' not have an output?!?";
						//Fyle.BUG(m2);
					}
					else
					{
						ret = redChannel.output.UniverseNumber;
					}
				}
				return ret;
			}
		}

		public override int DMXAddress
		{
			get
			{
				int ret = LOR4Admin.UNDEFINED;
				if (redChannel == null)
				{
					string m1 = "WTF does DrawObject '" + myName + "' not have a red channel?!";
					Fyle.BUG(m1);
				}
				else
				{
					string redName = redChannel.Name;
					if (redChannel.output == null)
					{
						string m2 = "WTF does " + myName + "'s red channel '" + redName + "' not have an output?!?";
						//Fyle.BUG(m2);
					}
					else
					{
						ret = redChannel.output.DMXAddress;
					}
				}
				return ret;
			}
		}

		public override void Parse(string lineIn)
		{
			// <LOR4VizDrawObject ID="141"
			// Name ="Pixel 154 / S2.004 / U3.010-012" BulbSize="1"
			// BulbSpacing ="1"
			// Comment =""
			// BulbShape ="1"
			// ZOrder ="1"
			// AssignedItem ="0"
			// Locked ="False"
			// Fixture_Type ="3"
			// Channel_Type ="2"
			// Max_Opacity ="0">

			myName = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDname));
			myID = LOR4Admin.getKeyValue(lineIn, LOR4Visualization.FIELDvizID);
			myIndex = myID;
			BulbSpacing = LOR4Admin.getKeyValue(lineIn, FIELDbulbSpacing);
			Comment = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, FIELDcomment));
			BulbShape = LOR4Admin.getKeyValue(lineIn, FIELDbulbShape);
			ZOrder = LOR4Admin.getKeyValue(lineIn, FIELDzOrder);
			AssignedItem = LOR4Admin.getKeyValue(lineIn, FIELDassignedItem);
			Locked = LOR4Admin.getKeyState(lineIn, FIELDlocked);
			FixtureType = LOR4Admin.getKeyValue(lineIn, FIELDfixtureType);
			ChannelType = LOR4Admin.getKeyValue(lineIn, FIELDchannelType);
			MaxOpacity = LOR4Admin.getKeyValue(lineIn, FIELDmaxOpacity);

		}



		public override string LineOut()
		{
			StringBuilder ret = new StringBuilder();

			ret.Append(LOR4Admin.StartTable(LOR4Visualization.TABLEdrawObject, 2));

			ret.Append(LOR4Admin.SetKey(LOR4Visualization.FIELDvizID, DrawObjectID));
			ret.Append(LOR4Admin.SetKey(LOR4Visualization.FIELDvizName, LOR4Admin.XMLifyName(myName)));
			ret.Append(LOR4Admin.SetKey(FIELDbulbSpacing, BulbSpacing));
			ret.Append(LOR4Admin.SetKey(FIELDcomment, Comment));
			ret.Append(LOR4Admin.SetKey(FIELDbulbShape, BulbShape));
			ret.Append(LOR4Admin.SetKey(FIELDzOrder, ZOrder));
			ret.Append(LOR4Admin.SetKey(FIELDassignedItem, AssignedItem));

			ret.Append(FIELDlocked);
			ret.Append(LOR4Admin.FIELDEQ);
			// Would be nice if LOR used standard "true" (Lower Case) but NOOOooooooo
			if (Locked)
			{
				ret.Append("True");
			}
			else
			{
				ret.Append("False");
			}
			ret.Append(LOR4Admin.ENDQT);

			ret.Append(LOR4Admin.SetKey(FIELDfixtureType, FixtureType));
			ret.Append(LOR4Admin.SetKey(FIELDchannelType, ChannelType));
			ret.Append(LOR4Admin.SetKey(FIELDmaxOpacity, MaxOpacity));
			ret.Append(LOR4Admin.ENDFLD);

			return ret.ToString();
		}

		public override iLOR4Member Clone()
		{
			LOR4VizDrawObject newDO = (LOR4VizDrawObject)Clone();
			newDO.isRGB = isRGB;
			newDO.BulbSize = BulbSize;
			newDO.BulbSpacing = BulbSpacing;
			newDO.BulbShape = BulbShape;
			newDO.Comment = Comment;
			newDO.ZOrder = ZOrder;
			newDO.AssignedItem = AssignedItem;
			newDO.Locked = Locked;
			newDO.FixtureType = FixtureType;
			newDO.ChannelType = ChannelType;
			newDO.MaxOpacity = MaxOpacity;
			newDO.redChannel = redChannel;
			newDO.grnChannel = grnChannel;
			newDO.bluChannel = bluChannel;

			return newDO;
		}

		public override iLOR4Member Clone(string newName)
		{
			iLOR4Member newDO = (LOR4VizDrawObject)this.Clone();
			newDO.ChangeName(newName);
			return newDO;
		}

		public override int color
		{
			get
			{
				if (isRGB)
				{ return LOR4Admin.LORCOLOR_RGB; }
				else
				{
					if (redChannel != null)
					{
						return redChannel.color;
					}
					else
					{
						return 0;
					}
				}
			}
			set { int ignore = value; }
		}

		public override Color Color
		{
			get { return LOR4Admin.Color_LORtoNet(this.color); }
			set { Color ignore = value; }
		}






	} // End LOR4VizDrawObject class







}