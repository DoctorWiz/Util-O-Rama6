using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using FileHelper;

namespace LOR4
{
	public class LOR4VizItemGroup : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>
	// An "Item" in a Visualization file is basically a group!
	{
		protected LOR4Visualization parentVisualization = null;
		public bool Locked = false;
		public int[] AssignedObjectsNumbers = null;

		// Will Contain just DrawObjects (for now anyway)
		public LOR4Membership Members = null;

		private static readonly string FIELDLocked = " Locked";
		private static readonly string FIELDComment = " Comment";
		private static readonly string FIELDObjectID = " Object";
		private static readonly string FIELDAssignedID = "AssignedObject ID";

		// SuperStarStuff
		// Since (for now) I don't have SuperStar, and
		// Since (for now) We are not writing out Visualization files
		//   Ignore and throw away this stuff
		public int SSWU = 0;
		public string SSFF = "";
		public bool SSReverseOrder = false;
		public bool SSForceRowColumn = false;
		public int SSRow = 0;
		public int SSColumn = 0;
		public bool SSUseMyOrder = false;
		public bool SSStar = false;
		public int SSMatrixInd = 0;
		public int SSPropColorTemp = 0;
		public static readonly string FIELD_SSWU = " SSWU";
		public static readonly string FIELD_SSFF = " SSFF";
		public static readonly string FIELD_SSReverseOrder = " SSReverseOrder";
		public static readonly string FIELD_SSForceRowColumn = " SSForceRowColumn";
		public static readonly string FIELD_SSRow = " SSRow";
		public static readonly string FIELD_SSColumn = " SSColumn";
		public static readonly string FIELD_SSUseMyOrder = " SSUseMyOrder";
		public static readonly string FIELD_SSStar = " SSStar";
		public static readonly string FIELD_SSMatrixInd = " SSMatrixInd";
		public static readonly string FIELD_SSPropColorTemp = " SSPropColorTemp";
		// So when I get ready to start writing out Visualization Files, I probably won't yet be supporting
		// SuperStar since it is very expensive!  So I can just write out all these defaults in one go.
		public static readonly string FIELDS_SS_DEFAULTS = " SSWU=\"0\" SSFF=\"\" SSReverseOrder=\"False\" SSForceRowColumn=\"False\" SSRow=\"0\" SSColumn=\"0\" SSUseMyOrder=\"False\" SSStar=\"False\" SSMatrixInd=\"0\" SSPropColorTemp=\"0\"";

		public LOR4VizItemGroup(iLOR4Member parent, string lineIn)
		{

			base.SetParent(parent);
			myParent = parent;
			Members = new LOR4Membership(this);
			MakeDummies();
			Parse(lineIn);

		}

		private void MakeDummies()
		{
			if (Members.Count == 0)
			{
				// SavedIndices and SaveIDs in Sequences start at 0. Cool! Great! No Prob!
				// But Channels, Groups, and DrawObjects in Visualizations start at 1 (Grrrrr)
				// So add a dummy object at the [0] start of the lists
				LOR4Visualization vp = (LOR4Visualization)myParent;


				LOR4VizDrawObject lvdo = new LOR4VizDrawObject(myParent, "DUMMY");
				lvdo.SetIndex(0);
				lvdo.SetID(0);



				//LOR4VizDrawObject lvdo = vp.VizDrawObjects[0];
				Members.Add(lvdo);
			}
		}

		public int ItemID
		{ get { return myID; } }
		public void SetItemID(int newItemID)
		{ myID = newItemID; }
		public int AltItemID
		{ get { return myAltID; } set { myAltID = value; } }
		public override int UniverseNumber
		{
			get
			{
				int un = LOR4Admin.UNDEFINED;
				if (Members.Count > 1)
				{
					un = Members.Items[1].UniverseNumber;
				}
				return un;
			}
		}
		public override int DMXAddress
		{
			get
			{
				int da = LOR4Admin.UNDEFINED;
				if (Members.Count > 1)
				{
					da = Members.Items[1].DMXAddress;
				}
				return da;
			}
		}

		public override LOR4MemberType MemberType
		{
			get
			{
				return LOR4MemberType.VizItemGroup;
			}
		}

		public override int CompareTo(iLOR4Member other)
		{
			int result = 1; // By default I win!

			if (LOR4Membership.sortMode == LOR4Membership.SORTbyOutput)
			{
				if (Members != null)
				{
					result = Members.Items[0].UniverseNumber.CompareTo(other.UniverseNumber);
					if (result == 0)
					{
						result = Members.Items[0].DMXAddress.CompareTo(other.DMXAddress);
					}
				}
			}
			else
			{
				result = base.CompareTo(other);
			}

			return result;
		}

		public override iLOR4Member Clone()
		{
			LOR4VizItemGroup newGrp = (LOR4VizItemGroup)Clone();
			newGrp.parentVisualization = parentVisualization;
			newGrp.Locked = Locked;
			newGrp.Comment = newGrp.Comment;
			newGrp.Members = Members;
			//newGrp.AssignedObjects = AssignedObjects;
			// Use/Keep Defaults for SuperStarStuff
			return newGrp;
		}

		public override iLOR4Member Clone(string newName)
		{
			LOR4VizItemGroup newGrp = (LOR4VizItemGroup)this.Clone();
			newGrp.ChangeName(newName);
			return newGrp;
		}

		//public string LineOut()
		//{
		//TODO Add support for writing Visualization files
		//	return "";
		//}

		public override void Parse(string lineIn)
		{
			myID = LOR4Admin.getKeyValue(lineIn, LOR4Visualization.FIELDvizID);
			myIndex = myID - 1;
			myName = LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDname);
			Locked = LOR4Admin.getKeyState(lineIn, FIELDLocked);
			Comment = LOR4Admin.getKeyWord(lineIn, FIELDComment);
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


		public void ParseAssignedObjectNumbers(StreamReader reader)
		{
			bool keepGoing = true;
			int aoCount = 0;
			while (keepGoing)
			{
				if (reader.EndOfStream) keepGoing = false;
				if (keepGoing)
				{
					string lineIn = reader.ReadLine();
					//lineCount++;
					int iEnd = lineIn.IndexOf("</Item>");
					if (iEnd > 0) keepGoing = false;
					if (keepGoing)
					{
						int ox = LOR4Admin.getKeyValue(lineIn, FIELDAssignedID);
						int oid = LOR4Admin.getKeyValue(lineIn, FIELDObjectID);
						if (AssignedObjectsNumbers == null)
						{
							Array.Resize(ref AssignedObjectsNumbers, ox + 1);
							AssignedObjectsNumbers[ox] = oid;
						}
						else
						{
							int c = AssignedObjectsNumbers.Length;
							Array.Resize(ref AssignedObjectsNumbers, ox + 1);
							AssignedObjectsNumbers[ox] = oid;
						}


						aoCount++;
					} // End second KeepGoing test-- not end of <Item>
				} // End first KeepGoing test-- not EndOfStream
			} // End While KeepGoing
		} // End ParseAssignedObjectNumbers

		public override int color
		{
			get
			{
				if (Members.Count > 1)
				{
					return Members[1].color;
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


	} // End Class LOR4VizItemGroup
} // End Namespace

