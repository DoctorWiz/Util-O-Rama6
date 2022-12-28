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
	public class LOR4Membership : IEnumerator, IEnumerable  //  IEnumerator<iLOR4Member>
	{
		#region Class-Level Global-Scope variables and objects
		private List<iLOR4Member> myByDisplayOrderList = new List<iLOR4Member>();   // The Main List
		private List<iLOR4Member> myByIDList = new List<iLOR4Member>();
		private List<iLOR4Member> myByAltIDList = new List<iLOR4Member>();
		//private List<LOR4Timings>					myBySaveIDList =				new List<LOR4Timings>();
		//private List<LOR4Timings>					myByAltSaveIDList =			new List<LOR4Timings>();
		//private List<LOR4Track>						myByTrackIndexList =		new List<LOR4Track>();
		//private List<LOR4Track>						myByAltTrackIndexList = new List<LOR4Track>();
		//private List<LOR4VizDrawObject>		myByObjectIDList =			new List<LOR4VizDrawObject>();
		//private List<LOR4VizDrawObject>		myByAltObjectIDList =		new List<LOR4VizDrawObject>();
		//private List<LOR4VizItemGroup>		myByItemIDList =				new List<LOR4VizItemGroup>();
		//private List<LOR4VizItemGroup>		myByAltItemIDList =			new List<LOR4VizItemGroup>();
		private SortedDictionary<string, iLOR4Member> myByNameDictionary = new SortedDictionary<string, iLOR4Member>();

		private int myHighestID = LOR4Admin.UNDEFINED;
		private int myHighestAltID = LOR4Admin.UNDEFINED;

		// B-cuz item numbers in Visualizations are 1-based (instead of normal 0-based)
		//private int myHighestItemID = 0;
		//public int AltHighestItemID = LOR4Admin.UNDEFINED;
		// B-cuz drawobjects in Visualizations do the same thing
		//private int myHighestObjectID = 0;
		//public int AltHighestObjectID = LOR4Admin.UNDEFINED;
		//iLOR4Member Parent = null;  // Parent SEQUENCE
		protected iLOR4Member myOwner = null;  // Parent GROUP or TRACK or Sequence or Visualization
		private iLOR4Member myParent = null;


		public const int SORTbyID = 1;
		//public const int SORTbySavedIndex = 1;
		public const int SORTbyAltID = 2;
		//public const int SORTbyAltSavedIndex = 2;
		public const int SORTbyName = 3;
		public const int SORTbyOutput = 4;
		public static int sortMode = SORTbyID;

		private int myChannelCount = 0;
		private int myRGBChannelCount = 0;
		private int myChannelGroupCount = 0;
		private int myCosmicDeviceCount = 0;
		private int myTrackCount = 0;
		private int myTimingGridCount = 0;
		private int myVizChannelCount = 0;
		private int myVizItemGroupCount = 0;
		private int myVizDrawObjectCount = 0;
		//private	int myEverythingCount = 0;
		//private int duplNameFix = 2;

		// For Enumeration
		private int position = 0;

		//iLOR4Member IEnumerator<iLOR4Member>.Current => throw new NotImplementedException();

		//object IEnumerator.Current => throw new NotImplementedException();
		#endregion

		//! CONSTRUCTOR
		public LOR4Membership(iLOR4Member theOwner)
		{
			if (theOwner == null)
			{
				System.Diagnostics.Debugger.Break();
			}
			else
			{
				// Reminder: Owner = Member (Track, Group, or Cosmic) that owns this Membership
				//           Parent = Base Sequence or Visualization
				myOwner = theOwner;
				if (myOwner.Parent != null)
				{
					myParent = myOwner.Parent;
				}
			}

			if (theOwner.MemberType == LOR4MemberType.Visualization)
			{
				// SavedIndices and SaveIDs in Sequences start at 0. Cool! Great! No Prob!
				// But Channels, Groups, and DrawObjects in Visualizations start at 1 (Grrrrr)
				// So add a dummy object at the [0] start of the lists
				/*
				LOR4VizChannel lvc = new LOR4VizChannel(this.Parent, "\0\0DUMMY VIZCHANNEL AT INDEX [0] - DO NOT USE!");
				lvc.SetIndex(0);
				lvc.SetSavedIndex(0);
				lvc.SetParent(myParent);
				myHighestSavedIndex = 0;
				myBySavedIndexList.Add(lvc);
				LOR4VizDrawObject lvdo = new LOR4VizDrawObject(this.Parent, "\0\0DUMMY VIZDRAWOBJECT AT INDEX [0] - DO NOT USE!");
				lvdo.SetIndex(0);
				lvdo.SetSavedIndex(0);
				lvdo.SetParent(myParent);
				if (myParent != null)
				{
					LOR4Visualization pv = (LOR4Visualization)myParent;
					if (pv.VizChannels.Count > 0)
					{
						lvdo.redChannel = pv.VizChannels[0];
					}
				}
				myHighestObjectID = 0;
				myByObjectIDList.Add(lvdo);
				LOR4VizItemGroup lvig = new LOR4VizItemGroup(this.Parent, "\0\0DUMMY VIZITEMGROUP AT INDEX [0] - DO NOT USE!");
				lvig.SetIndex(0);
				lvig.SetSavedIndex(0);
				lvig.SetParent(myParent);
				myHighestSavedIndex = 0;
				myBySavedIndexList.Add(lvig);
				*/
			}
			if (theOwner.MemberType == LOR4MemberType.VizItemGroup)
			{
				//LOR4VizDrawObject lvdo = new LOR4VizDrawObject(this.Parent, "\0\0DUMMY VIZDRAWOBJECT AT INDEX [0] - DO NOT USE!");
				//lvdo.SetIndex(0);
				//lvdo.SetSavedIndex(0);
				//lvdo.SetParent(myParent);
				//myHighestItemID = 0;
				//if (myByObjectIDList.Count == 0) myByObjectIDList.Add(null);
				//myByObjectIDList[0] = lvdo;
			}


		}
		public void Clear()
		{
			myByDisplayOrderList = new List<iLOR4Member>();   // The Main List
			myByIDList = new List<iLOR4Member>();
			myByAltIDList = new List<iLOR4Member>();
			//myBySaveIDList = new List<LOR4Timings>();
			//myByAltSaveIDList = new List<LOR4Timings>();
			//myByTrackIndexList = new List<LOR4Track>();
			//myByAltTrackIndexList = new List<LOR4Track>();
			//myByObjectIDList = new List<LOR4VizDrawObject>();
			//myByAltObjectIDList = new List<LOR4VizDrawObject>();
			//myByItemIDList = new List<LOR4VizItemGroup>();
			//myByAltItemIDList = new List<LOR4VizItemGroup>();

			myByNameDictionary = new SortedDictionary<string, iLOR4Member>();
			myHighestID = LOR4Admin.UNDEFINED;
			myHighestAltID = LOR4Admin.UNDEFINED;
			//myHighestSaveID = LOR4Admin.UNDEFINED;
			//AltHighestSaveID = LOR4Admin.UNDEFINED;
			//myHighestItemID = LOR4Admin.UNDEFINED;
			//AltHighestItemID = LOR4Admin.UNDEFINED;
			//myHighestObjectID = LOR4Admin.UNDEFINED;
			//AltHighestObjectID = LOR4Admin.UNDEFINED;
			//myOwner = null;  // Parent GROUP or TRACK
			//myParentSeq = null;
			sortMode = SORTbyID;
			myChannelCount = 0;
			myRGBChannelCount = 0;
			myChannelGroupCount = 0;
			myCosmicDeviceCount = 0;
			myTrackCount = 0;
			myTimingGridCount = 0;
			//myEverythingCount = 0;
			position = 0;
		}
		public List<iLOR4Member> Items
		{ get { return myByDisplayOrderList; } }
		public List<iLOR4Member> Members
		{ get { return myByDisplayOrderList; } }
		//private List<iLOR4Member> Members = null;
		public List<iLOR4Member> BySavedIndex
		{
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.Sequence)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access members by SavedIndex?";
						Fyle.BUG(msg);
					}
				}
				return myByIDList;
			}
		}
		public List<iLOR4Member> ByDisplayOrder
		{ get { return myByDisplayOrderList; } }

		public List<iLOR4Member> ByAltSavedIndex
		{
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.Sequence)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access members by AltSavedIndex?";
						Fyle.BUG(msg);
					}
				}
				return myByAltIDList;
			}
		}
		public List<iLOR4Member> ByItemID
		{
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.Visualization)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access members by ItemID?";
						Fyle.BUG(msg);
					}
				}
				return myByIDList;
			}
		}
		public List<iLOR4Member> ByAltItemID
		{
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.Visualization)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access members by AltItemID?";
						Fyle.BUG(msg);
					}
				}
				return myByAltIDList;
			}
		}
		public List<iLOR4Member> BySaveID
		{
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.Sequence)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access members by SaveID?";
						Fyle.BUG(msg);
					}
				}
				return myByIDList;
			}
		}
		public List<iLOR4Member> ByAltSaveID
		{
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.Sequence)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access members by AltSaveID?";
						Fyle.BUG(msg);
					}
				}
				return myByAltIDList;
			}
		}
		public List<iLOR4Member> ByObjectID
		{
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.VizItemGroup)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access members by ObjectID?";
						Fyle.BUG(msg);
					}
				}
				return myByIDList;
			}
		}
		public List<iLOR4Member> ByAltObjectID
		{
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.VizItemGroup)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access members by AltObjectID?";
						Fyle.BUG(msg);
					}
				}
				return myByAltIDList;
			}
		}
		public List<iLOR4Member> ByTrackIndex
		{
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.Sequence)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access members by TrackIndex?";
						Fyle.BUG(msg);
					}
				}
				return myByIDList;
			}
		}
		public List<iLOR4Member> ByAltTrackIndex
		{
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.Sequence)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access members by AltTrackIndex?";
						Fyle.BUG(msg);
					}
				}
				return myByAltIDList;
			}
		}
		public SortedDictionary<string, iLOR4Member> ByName
		{ get { return myByNameDictionary; } }
		public int HighestSavedIndex
		{
			// Used by Sequence Channels, RGB Channels, Channel Groups, and Cosmic Devices
			// Used by Visualization VizChannels
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.Sequence)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access HighestSavedIndex?";
						Fyle.BUG(msg);
					}
				}
				return myHighestID;
			}
		}
		public int HighestAltSavedIndex
		{
			// Used by Sequence Channels, RGB Channels, Channel Groups, and Cosmic Devices
			// Used by Visualization VizChannels
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.Sequence)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access HighestSavedIndex?";
						Fyle.BUG(msg);
					}
				}
				return myHighestAltID;
			}
			set
			{ myHighestAltID = value; }
		}
		public int HighestItemID
		{
			// Used by Sequence Channels, RGB Channels, Channel Groups, and Cosmic Devices
			// Used by Visualization VizChannels
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.Visualization)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access HighestItemID?";
						Fyle.BUG(msg);
					}
				}
				return myHighestAltID;
			}
		}
		public int HighestDrawObjectID
		{
			// Used by Sequence Channels, RGB Channels, Channel Groups, and Cosmic Devices
			// Used by Visualization VizChannels
			get
			{
				if (Fyle.isWiz)
				{
					if (myOwner.MemberType != LOR4MemberType.VizItemGroup)
					{
						string msg = "Why is a " + LOR4SeqEnums.MemberTypeName(myOwner.MemberType);
						msg += "Trying to access HighestDrawObjectID?";
						Fyle.BUG(msg);
					}
				}
				return myHighestAltID;
			}
		}
		public int ChannelCount
		{ get { return myChannelCount; } }
		public int RGBChannelCount
		{ get { return myRGBChannelCount; } }
		public int ChannelGroupCount
		{ get { return myChannelGroupCount; } }
		public int CosmicDeviceCount
		{ get { return myCosmicDeviceCount; } }
		public int TrackCount
		{ get { return myTrackCount; } }
		public int TimingGridCount
		{ get { return myTimingGridCount; } }
		public int VizChannelCount
		{ get { return myVizChannelCount; } }
		public int VizItemGroupCount
		{ get { return myVizItemGroupCount; } }
		public int VizDrawObjectCount
		{ get { return myVizDrawObjectCount; } }
		public int AllCount
		{ get { return myByDisplayOrderList.Count; } }
		public iLOR4Member Owner
		{
			get
			{
				return myOwner;
			}
		}
		public void ChangeOwner(iLOR4Member newOwner)
		{
			//! WHY?!?!
			if (Fyle.DebugMode) System.Diagnostics.Debugger.Break();
			myOwner = newOwner;
			myParent = myOwner.Parent;
		}
		// LOR4Membership.Add(Member)
		public int Add(iLOR4Member newMember)
		{
			int psi = LOR4Admin.UNDEFINED;
			int tc = LOR4Admin.UNDEFINED;
			int psv = LOR4Admin.UNDEFINED;
			LOR4MemberType parentType = myParent.MemberType;
			LOR4MemberType ownerType = myOwner.MemberType;
			LOR4MemberType newMemberType = newMember.MemberType;
			int newMemberID = newMember.ID;

			if (newMember.ID < 0)
			{
				Fyle.BUG("New Member has no ID yet!");
			}

			// Add to the mYMembers list, no matter who the owner of this membership is
			myByDisplayOrderList.Add(newMember);

			// By Name Dictionary
			string itemName = newMember.Name;
			// Check for blank name (common with Tracks and TimingGrids if not changed/set by the user)
			if (itemName == "")
			{
				// Make up a name based on type and index
				itemName = LOR4SeqEnums.MemberTypeName(newMember.MemberType) + " " + newMember.Index.ToString("0000");
			}
			// Check for duplicate names
			while (myByNameDictionary.ContainsKey(itemName))
			{
				// Append a number
				itemName = newMember.Name + " �" + myByNameDictionary.Count.ToString() + "�";
			}
			myByNameDictionary.Add(itemName, newMember);

			myParent.MakeDirty(true);
			// Is this the new Guiness World Record Highest ID?
			if (newMemberID > myHighestID) myHighestID = newMemberID;


			//! ** SEQUENCES **
			// Is the base parent a sequence, and will this contain sequence-type objects?
			if (parentType == LOR4MemberType.Sequence)
			{
				// Is new member a 'regular' type that has a SaveIndex?
				if ((newMemberType == LOR4MemberType.Channel) ||
						(newMemberType == LOR4MemberType.RGBChannel) ||
						(newMemberType == LOR4MemberType.ChannelGroup) ||
						(newMemberType == LOR4MemberType.Cosmic))
				{
					//myByIDList.Add(newMember);
					//myByAltIDList.Add(newMember);
					if (newMemberType == LOR4MemberType.Channel)
					{
						myChannelCount++;
					}
					if (newMemberType == LOR4MemberType.RGBChannel)
					{
						myRGBChannelCount++;
					}
					if (newMemberType == LOR4MemberType.ChannelGroup)
					{
						myChannelGroupCount++;
					}
					if (newMember.MemberType == LOR4MemberType.Cosmic)
					{
						myCosmicDeviceCount++;
					}
				} // End if 'regular' Sequence-Type Member: Channel, RGB, Group, or Cosmic

				// Not a 'regular' member...
				// Is it a track?
				if (newMemberType == LOR4MemberType.Track)
				{
					myTrackCount++;
				} // End if a track

				// OK, not a 'regular' member or a track...
				// Is it a timing grid?
				if (newMemberType == LOR4MemberType.Timings)
				{
					myTimingGridCount++;
				} // End timing Grid
			} // End base parent is a sequence and newmember is a sequenc-y type thing



			//! ** VISUALIZATIONS **
			// Is the base parent a Visualization?
			if (parentType == LOR4MemberType.Visualization)
			{
				// Is it a 'regular' visual thing; a channel, drawobject, or itemgroup?
				if ((newMemberType == LOR4MemberType.VizChannel) ||
						(newMemberType == LOR4MemberType.VizDrawObject) ||
						(newMemberType == LOR4MemberType.VizItemGroup))
				{
					if (newMemberType == LOR4MemberType.VizChannel)
					{
						myVizChannelCount++;
					}
					if (newMember.MemberType == LOR4MemberType.VizDrawObject)
					{
						myVizDrawObjectCount++;
					}
					if (newMember.MemberType == LOR4MemberType.VizItemGroup)
					{
						myVizItemGroupCount++;
					} // End if ItemGroup
				} // End if a 'regular' visualizer channel, drawobject, or itemgroup
			} // end base parent is a visualization

			// Owner may be a sequence, and Members will be just the tracks,
			//                          and AllMembers will be regular items with saved indices
			// For these, we want to be able to fetch the by SavedIndex, AltSavedIndex, or TrackIndex
			// NOTE: do NOT do this if the owner is a track, channelgroup, or vizitemgroup
			if ((ownerType == LOR4MemberType.Sequence) ||
					(ownerType == LOR4MemberType.Visualization))
			{
				while ((myByIDList.Count - 1) < newMember.ID)
				{
					myByIDList.Add(null);
					//myByAltIDList.Add(null);
				}
				myByIDList[newMemberID] = newMember;
				while ((myByAltIDList.Count - 1) < newMember.ID)
				{
					//myByIDList.Add(null);
					myByAltIDList.Add(null);
				}
				myByAltIDList[newMemberID] = newMember;
			}





			return newMemberID;
		}
		// For iEnumerable
		public iLOR4Member this[int index]
		{
			get
			{
				if (index < myByDisplayOrderList.Count)
				{
					return myByDisplayOrderList[index];
				}
				else
				{
					return null;
				}
			}
			set
			{
				while (myByDisplayOrderList.Count <= index)
				{
					myByDisplayOrderList.Add(null);
				}
				myByDisplayOrderList[index] = value;
			}
		}
		public IEnumerator GetEnumerator()
		{
			return (IEnumerator)this;
		}
		public bool Includes(string memberName)
		{
			//! NOTE: Does NOT check sub-groups!
			bool found = false;
			for (int m = 0; m < myByDisplayOrderList.Count; m++)
			{
				iLOR4Member member = myByDisplayOrderList[m];
				if (member.Name == memberName)
				{
					found = true;
					m = myByDisplayOrderList.Count; // Exit loop
				}
			}
			return found;
		}
		public bool Includes(int savedIndex)
		{
			//! NOTE: Does NOT check sub-groups!
			bool found = false;
			for (int m = 0; m < myByDisplayOrderList.Count; m++)
			{
				iLOR4Member member = myByDisplayOrderList[m];
				if (member.ID == savedIndex)
				{
					found = true;
					m = myByDisplayOrderList.Count; // Exit loop
				}
			}
			return found;
		}
		public bool MoveNext()
		{
			position++;
			return (position < myByDisplayOrderList.Count);
		}
		public void Reset()
		{ position = -1; }
		public object Current
		{
			get { return myByDisplayOrderList[position]; }
		}
		public int Count
		{
			get
			{
				return myByDisplayOrderList.Count;
			}
		}
		public CheckState CheckState
		{
			get
			{
				//! !! DEPRECIATED - Use Selected Property instead!!
				System.Diagnostics.Debugger.Break();
				return SelectedState;
			}
		}

		public int SelectedDescendantCount
		{
			// Besides getting the number of selected members and submembers
			// it also 'cleans up' the selection states

			get
			{
				//! !! DEPRECIATED - Use Selected property instead
				int count = 0;
				if (myOwner.SelectedState == CheckState.Checked)
				{
					foreach (iLOR4Member m in myByDisplayOrderList)
					{
						if (m.MemberType == LOR4MemberType.Channel)
						{
							if (m.SelectedState == CheckState.Checked) count++;
						}
						if (m.MemberType == LOR4MemberType.RGBChannel)
						{
							if (m.SelectedState == CheckState.Checked)
							{
								int subCount = 0;
								LOR4RGBChannel r = (LOR4RGBChannel)m;
								if (r.redChannel.SelectedState == CheckState.Checked) subCount++;
								if (r.grnChannel.SelectedState == CheckState.Checked) subCount++;
								if (r.bluChannel.SelectedState == CheckState.Checked) subCount++;
								if (subCount == 0)
								{
									m.SelectedState = CheckState.Unchecked;
								}
								else
								{
									//m.Selected = true;
									//subCount++;
									count += subCount;
								}
							}
						}
						if (m.MemberType == LOR4MemberType.ChannelGroup)
						{
							if (m.SelectedState == CheckState.Checked)
							{
								LOR4ChannelGroup g = (LOR4ChannelGroup)m;
								int subCount = g.Members.SelectedDescendantCount;  // Recurse!
								if (subCount == 0)
								{
									m.SelectedState = CheckState.Unchecked;
								}
								else
								{
									//m.Selected = true;
									count += subCount;
								}
							}
						}
						if (m.MemberType == LOR4MemberType.Cosmic)
						{
							if (m.SelectedState == CheckState.Checked)
							{
								LOR4Cosmic d = (LOR4Cosmic)m;
								int subCount = d.Members.SelectedDescendantCount;  // Recurse!
								if (subCount == 0)
								{
									m.SelectedState = CheckState.Unchecked;
								}
								else
								{
									//m.Selected = true;
									count += subCount;
								}
							}
						}
					}
					if (count == 0)
					{
						if (myOwner != null)
						{
							myOwner.SelectedState = CheckState.Unchecked;
						}
						else
						{
							//this.Owner.Selected = true;
						}
					}
				}
				return count;
			}
		}
		public int HighestObjectID
		{
			// For Visualization DrawObjects only
			get
			{
				return myHighestID;
			}
			set
			{
				myHighestID = value;
			}
		}
		public void ReIndex()
		{
			// Clear previous run

			myChannelCount = 0;
			myRGBChannelCount = 0;
			myChannelGroupCount = 0;
			myTrackCount = 0;
			myTimingGridCount = 0;
			myVizChannelCount = 0;
			myVizDrawObjectCount = 0;
			myVizItemGroupCount = 0;
			//myEverythingCount = 0;

			sortMode = SORTbyID;

			myByNameDictionary = new SortedDictionary<string, iLOR4Member>();
			myByIDList = new List<iLOR4Member>();
			myByAltIDList = new List<iLOR4Member>();

			try
			{
				for (int i = 0; i < myByDisplayOrderList.Count; i++)
				{
					iLOR4Member member = myByDisplayOrderList[i];

					string itemName = member.Name;
					// Check for blank name (common with Tracks and TimingGrids if not changed/set by the user)
					if (itemName == "")
					{
						// Make up a name based on type and index
						itemName = LOR4SeqEnums.MemberTypeName(member.MemberType) + " " + member.Index.ToString("0000");
					}
					// Check for duplicate names
					while (myByNameDictionary.ContainsKey(itemName))
					{
						// Append a number
						itemName = member.Name + " �" + myByNameDictionary.Count.ToString() + "�";
					}
					myByNameDictionary.Add(itemName, member);

					switch (member.MemberType)
					{
						case LOR4MemberType.Channel:
							myChannelCount++;
							break;
						case LOR4MemberType.RGBChannel:
							myRGBChannelCount++;
							break;
						case LOR4MemberType.ChannelGroup:
							myChannelGroupCount++;
							break;
						case LOR4MemberType.Cosmic:
							myCosmicDeviceCount++;
							break;
						case LOR4MemberType.Track:
							myTrackCount++;
							break;
						case LOR4MemberType.Timings:
							myTimingGridCount++;
							break;
						case LOR4MemberType.VizChannel:
							myVizChannelCount++;
							break;
						case LOR4MemberType.VizDrawObject:
							myVizDrawObjectCount++;
							break;
						case LOR4MemberType.VizItemGroup:
							myVizItemGroupCount++;
							break;
					}
					// NOTE: do NOT do this if the owner is a track, channelgroup, or vizitemgroup
					if ((myOwner.MemberType == LOR4MemberType.Sequence) ||
							(myOwner.MemberType == LOR4MemberType.Visualization))
					{
						while ((myByIDList.Count - 1) < member.ID)
						{
							myByIDList.Add(null);
						}
						myByIDList[member.ID] = member;
						if (member.AltID >= 0)
						{
							while ((myByAltIDList.Count - 1) < member.AltID)
							{
								myByAltIDList.Add(null);
							}
							myByAltIDList[member.AltID] = member;
						} // End member has valid AltID
					} // End if Owner is Sequence or Visualization
				} // End myMembers Loop
			} // End Try
			catch (Exception ex)
			{
				string msg = "Error Reindexing Membership\r\n\r\n";
				msg += ex.Message;
				Fyle.BUG(msg);
			}

			// Sort 'em all!
			sortMode = SORTbyID;
			//System.Diagnostics.Debugger.Break();
			// Sort is failing, supposedly because array elements are not set (null/empty)
			//  -- but a quick check of 'Locals' doesn't show any empties
			NULLITEMCHECK();
			myByIDList.Sort();
			myByAltIDList.Sort();

			//sortMode = SORTbyName;

			//LOR4Sequence mySeq = (LOR4Sequence)Parent;
			//if (myParentSeq.TimingGrids.Count > 0)
			//{
			//AlphaSortSavedIndexes(byTimingGridName, 0, Parent.TimingGrids.Count - 1);
			//byAlphaTimingGridNames.Sort();
			//}

		} // end ReIndex
		private void NULLITEMCHECK()
		{
			for (int i = 0; i < myByDisplayOrderList.Count; i++)
			{
				if (myByDisplayOrderList[i] == null)
				{
					string newName = "Member " + i.ToString("0000");
					string msg = "There is a null item in the membership of ";
					msg += this.Owner.Name + " at index " + i.ToString();
					Fyle.BUG(msg, Fyle.Noises.SamCurseF);
					//LOR4MemberBase mbr = new(myParent, newName);
					//mbr.SetID(i);
				}
				//System.Diagnostics.Debugger.Break();
				if (myByDisplayOrderList[i].Parent == null) System.Diagnostics.Debugger.Break();
				int v = myByDisplayOrderList[i].CompareTo(myByDisplayOrderList[0]);
			}

		}
		// LOR4Membership.find(name, type, create)
		public iLOR4Member FindByName(string theName, LOR4MemberType theType, bool createIfNotFound = false)
		{
			//iLOR4Member ret = null;
#if DEBUG
			string msg = "Membership.find(" + theName + ", ";
			msg += theType.ToString() + ", " + createIfNotFound.ToString() + ")";
			Debug.WriteLine(msg);
#endif


			if (myByNameDictionary.TryGetValue(theName, out iLOR4Member ret))
			{
				// Found the name, is the type correct?
				if (ret.MemberType != theType)
				{
					ret = null;
				}
			}
			if ((ret == null) && createIfNotFound)
			{
				if (myParent != null)
				{
					if (myParent.MemberType == LOR4MemberType.Sequence)
					{
						LOR4Sequence parentSeq = (LOR4Sequence)Parent;
						if (theType == LOR4MemberType.Channel)
						{
							ret = parentSeq.CreateNewChannel(theName);
							Add(ret);
						}
						if (theType == LOR4MemberType.RGBChannel)
						{
							LOR4RGBChannel grp = parentSeq.CreateNewRGBChannel(theName);

							LOR4Channel ch = parentSeq.CreateNewChannel(theName + " (R)");
							ch.color = LOR4Admin.LORCOLOR_RED;
							ch.rgbChild = LOR4RGBChild.Red;
							grp.redChannel = ch;
							ch = parentSeq.CreateNewChannel(theName + " (G)");
							ch.color = LOR4Admin.LORCOLOR_GRN;
							ch.rgbChild = LOR4RGBChild.Green;
							grp.grnChannel = ch;
							ch = parentSeq.CreateNewChannel(theName + " (B)");
							ch.color = LOR4Admin.LORCOLOR_BLU;
							ch.rgbChild = LOR4RGBChild.Blue;
							grp.bluChannel = ch;


							Add(grp);
							ret = grp;
						}
						if (theType == LOR4MemberType.ChannelGroup)
						{
							ret = parentSeq.CreateNewChannelGroup(theName);
							Add(ret);
						}
						if (theType == LOR4MemberType.Cosmic)
						{
							ret = parentSeq.CreateNewCosmicDevice(theName);
							Add(ret);
						}
						if (theType == LOR4MemberType.Timings)
						{
							ret = parentSeq.CreateNewTimingGrid(theName);
							Add(ret);
						}
						if (theType == LOR4MemberType.Track)
						{
							ret = parentSeq.CreateNewTrack(theName);
							Add(ret);
						}
					} // End if parent is sequence

					if (myParent.MemberType == LOR4MemberType.Visualization)
					{
						LOR4Visualization parentViz = (LOR4Visualization)Parent;
						if (theType == LOR4MemberType.VizChannel)
						{
							//ret = parentViz.CreateVizChannel(theName);
							Add(ret);
						}
						if (theType == LOR4MemberType.VizDrawObject)
						{
							//ret = parentViz.CreateDrawObject(theName);
							Add(ret);
						}
						if (theType == LOR4MemberType.VizItemGroup)
						{
							//ret = parentViz.CreateItemGroup(theName);
							Add(ret);
						} // End if VizItemGroup
					} // End if parent is Visualization
				} // End if parent isn't null
			} // End if find by name returned null and CreateIfNotFound is true
			return ret;
		}

		public CheckState SelectedState
		{
			get
			{
				CheckState finalState = CheckState.Unchecked;
				CheckState groupState = CheckState.Indeterminate;
				// Start with 'All' true, if ANY are NOT selected, set it false
				bool all = true;
				// Start with 'Some' false, if ANY are selected, set it true
				bool some = false;

				// Loop thru all members in the membership
				for (int i=0; i< myByDisplayOrderList.Count; i++)
				{
					iLOR4Member member = myByDisplayOrderList[i];
					LOR4MemberType type = member.MemberType;
					// What type is this member?
					switch(type)
					{
						case LOR4MemberType.Channel:	//! CHANNEL
							// If the channel is selected, then at least Some is true
							if (member.SelectedState == CheckState.Checked)
							{ some = true; }
							// If the channel is not selected, then All can't be true
							if (member.SelectedState == CheckState.Unchecked)
							{ all = false; }
							break;

						case LOR4MemberType.RGBChannel:		//! RGB CHANNEL
							// Cast member so we can get subchannels
							LOR4RGBChannel rgbChan = (LOR4RGBChannel)member;
							bool rgbAll = true;
							bool rgbSome = false;
							// Are ANY of the colored subchannels selected?
							if ((rgbChan.redChannel.SelectedState == CheckState.Checked) ||
								 (rgbChan.grnChannel.SelectedState == CheckState.Checked) ||
								 (rgbChan.bluChannel.SelectedState == CheckState.Checked))
							// Then Some is True
							{ rgbSome = true; some = true; }
							// Are ANY of the colored subchannels unselected?
							if ((rgbChan.redChannel.SelectedState == CheckState.Unchecked) ||
								 (rgbChan.grnChannel.SelectedState == CheckState.Unchecked) ||
								 (rgbChan.bluChannel.SelectedState == CheckState.Unchecked))
							// Then All must be false
							{ rgbAll=false; all = false; }
							// If all 3 colors are selected, state is Checked
							if (rgbAll) { rgbChan.SelectedState = CheckState.Checked; }
							// If none of the 3 colors are selected, state is Unchecked
							else if (!rgbSome) { rgbChan.SelectedState = CheckState.Unchecked; }
							// If some, but not all of the 3 colors are selected, state is Indeterminate
							else { rgbChan.SelectedState = CheckState.Indeterminate; }
							break;

						case LOR4MemberType.ChannelGroup:		//! CHANNEL GROUP
							// Cast member to a group so we can get its membership
							LOR4ChannelGroup group = (LOR4ChannelGroup)member;
							//# RECURSE- Check state of this groups members
							groupState = group.Members.SelectedState;
							// If all or any of the members are selected, then Some must be true
							if ((groupState == CheckState.Checked) || (groupState == CheckState.Indeterminate))
							{ some = true; }
							// If none, or some but not all, of the members are selected, then All can't be true
							if ((groupState == CheckState.Unchecked) || (groupState == CheckState.Indeterminate))
							{ all = false; }
							group.SelectedState = groupState;
							break;

						case LOR4MemberType.Cosmic:		//! COSMIC COLOR DEVICE
							LOR4Cosmic cosmic = (LOR4Cosmic)member;
							groupState = cosmic.Members.SelectedState;
							// If all or any of the members are selected, then Some must be true
							if ((groupState == CheckState.Checked) || (groupState == CheckState.Indeterminate))
							{ some = true; }
							// If none, or some but not all, of the members are selected, then All can't be true
							if ((groupState == CheckState.Unchecked) || (groupState == CheckState.Indeterminate))
							{ all = false; }
							cosmic.SelectedState = groupState;
							break;

						case LOR4MemberType.Track:		//! TRACK
							// (See comments for ChannelGroup)
							LOR4Track track = (LOR4Track)member;
							groupState = track.Members.SelectedState;
							if ((groupState == CheckState.Checked) || (groupState == CheckState.Indeterminate))
							{ some = true; }
							if ((groupState == CheckState.Unchecked) || (groupState == CheckState.Indeterminate))
							{ all = false; }
							track.SelectedState = groupState;
							break;

						case LOR4MemberType.VizChannel:		//! VISUALIZER CHANNEL
							// (See comments for Channel)
							if (member.SelectedState == CheckState.Checked)
							{ some = true; }
							if (member.SelectedState == CheckState.Unchecked)
							{ all = false; }
							break;

						case LOR4MemberType.VizDrawObject:		//! VISUALIZER DRAW OBJECT
							// (See comments for Channel)
							if (member.SelectedState == CheckState.Checked)
							{ some = true; }
							if (member.SelectedState == CheckState.Unchecked)
							{ all = false; }
							break;

						case LOR4MemberType.VizItemGroup:		//! VISUALIZER ITEM GROUP
							// (See comments for ChannelGroup)
							LOR4VizItemGroup items = (LOR4VizItemGroup)member;
							groupState = items.Members.SelectedState;
							if ((groupState == CheckState.Checked) || (groupState == CheckState.Indeterminate))
							{ some = true; }
							if ((groupState == CheckState.Unchecked) || (groupState == CheckState.Indeterminate))
							{ all = false; }
							items.SelectedState = groupState;
							break;
					}
				}

				// If ALL members are selected, state is Checked
				if (all) { finalState = CheckState.Checked; }
				// If no members (Not Some) are selected, state is Unchecked
				else if (!some) { finalState = CheckState.Unchecked; }
				// Otherwise (NOT ALL, but SOME) state is Indeterminate
				else { finalState = CheckState.Indeterminate; }

				return finalState;
			}
			set 
			{
				if (value != CheckState.Indeterminate)
				{
					// Loop thru all members in the membership
					for (int i = 0; i < myByDisplayOrderList.Count; i++)
					{
						iLOR4Member member = myByDisplayOrderList[i];
						member.SelectedState = value;
					}
				}
			}
		}
		public LOR4Channel FindChannel(string channelName, bool createIfNotFound = false, bool clearEffects = false)
		{
			LOR4Channel ret = null;
			iLOR4Member member = FindByName(channelName, LOR4MemberType.Channel, createIfNotFound);
			if (member != null)
			{
				ret = (LOR4Channel)member;
				if (clearEffects)
				{
					ret.effects.Clear();
				}
			}
			else
			{
				LOR4Sequence sq = (LOR4Sequence)myParent;
				ret = sq.CreateNewChannel(channelName);
				this.Add(ret);
			}
			return ret;
		}
		public LOR4RGBChannel FindRGBChannel(string rgbChannelName, bool createIfNotFound = false, bool clearEffects = false)
		{
			LOR4RGBChannel ret = null;
			iLOR4Member member = FindByName(rgbChannelName, LOR4MemberType.RGBChannel, createIfNotFound);
			if (member != null)
			{
				if (member.MemberType == LOR4MemberType.RGBChannel)
				{
					ret = (LOR4RGBChannel)member;
					if (clearEffects)
					{
						ret.redChannel.effects.Clear();
						ret.grnChannel.effects.Clear();
						ret.redChannel.effects.Clear();
					}
				}
			}
			else
			{
				LOR4Sequence psq = (LOR4Sequence)myParent;
				LOR4RGBChannel newrgb = psq.CreateNewRGBChannel(rgbChannelName, true);
				this.Add(newrgb);
			}

			return ret;
		}
		public LOR4ChannelGroup FindChannelGroup(string channelGroupName, bool createIfNotFound = false)
		{
			LOR4ChannelGroup ret = null;
			iLOR4Member member = FindByName(channelGroupName, LOR4MemberType.ChannelGroup, createIfNotFound);
			if (member != null)
			{
				ret = (LOR4ChannelGroup)member;
			}
			else
			{
				LOR4Sequence seq = (LOR4Sequence)myParent;
				ret = seq.CreateNewChannelGroup(channelGroupName);
			}
			return ret;
		}
		public LOR4Track FindTrack(string trackName, bool createIfNotFound = false)
		{
			LOR4Track ret = null;
			iLOR4Member member = FindByName(trackName, LOR4MemberType.Track, createIfNotFound);
			if (member != null)
			{
				ret = (LOR4Track)member;
			}
			return ret;
		}
		public LOR4VizChannel FindVizChannel(string channelName, bool createIfNotFound = false)
		{
			LOR4VizChannel ret = null;
			iLOR4Member member = FindByName(channelName, LOR4MemberType.Channel, createIfNotFound);
			if (member != null)
			{
				ret = (LOR4VizChannel)member;
			}
			return ret;
		}
		public LOR4VizDrawObject FindVizDrawObject(string drawObjectName, bool createIfNotFound = false)
		{
			LOR4VizDrawObject ret = null;
			iLOR4Member member = FindByName(drawObjectName, LOR4MemberType.RGBChannel, createIfNotFound);
			if (member != null)
			{
				ret = (LOR4VizDrawObject)member;
			}
			return ret;
		}
		public LOR4VizItemGroup FindVizItemGroup(string itemGroupName, bool createIfNotFound = false)
		{
			LOR4VizItemGroup ret = null;
			iLOR4Member member = FindByName(itemGroupName, LOR4MemberType.ChannelGroup, createIfNotFound);
			if (member != null)
			{
				ret = (LOR4VizItemGroup)member;
			}
			return ret;
		}
		public iLOR4Member FindBySavedIndex(int theSavedIndex)
		{
			return FindByID(theSavedIndex);
		}
		public iLOR4Member FindByID(int theID)
		{
			iLOR4Member ret = null;
			if ((theID >= 0) && (theID < myByIDList.Count))
			{
				ret = myByIDList[theID];
			}
			return ret;
		}
		/*
		private static iLOR4Member FindByName(string theName, List<iLOR4Member> Members)
		{
			iLOR4Member ret = null;
			int idx = BinarySearch(theName, Members);
			if (idx > LOR4Admin.UNDEFINED)
			{
				ret= Members[idx];
			}
			return ret;
		}

		private static int BinarySearch(string theName, List<iLOR4Member> Members)
		{
			return TreeSearch(theName, Members, 0, Members.Count - 1);
		}

		private static int TreeSearch(string theName, List<iLOR4Member> Members, int start, int end)
		{
			int index = -1;
			int mid = (start + end) / 2;
			string sname = Members[start].Name;
			string ename = Members[end].Name;
			string mname = Members[mid].Name;
			//if ((theName.CompareTo(Members[start].Name) > 0) && (theName.CompareTo(Members[end].Name) < 0))
			if ((theName.CompareTo(sname) >= 0) && (theName.CompareTo(ename) <= 0))
			{
				int cmp = theName.CompareTo(mname);
				if (cmp < 0)
					index = TreeSearch(theName, Members, start, mid);
				else if (cmp > 0)
					index = TreeSearch(theName, Members, mid + 1, end);
				else if (cmp == 0)
					index = mid;
				//if (index != -1)
				//	Console.WriteLine("got it at " + index);
			}
			return index;
		}
		*/
		public void ResetWritten()
		{
			foreach (iLOR4Member member in myByIDList)
			{
				member.AltID = LOR4Admin.UNDEFINED;
				//member.Written = false;
			}
			myHighestAltID = LOR4Admin.UNDEFINED;
		}
		public int DescendantCount(bool selectedOnly = false, bool countPlain = true, bool countRGBparents = false, bool countRGBchildren = true)
		{
			// Depending on situation/usaage, you will probably want to count
			// The RGBchannels, OR count their 3 children.
			//    Unlikely you will need to count neither or both, but you can if you want
			// ChannelGroups themselves are not counted, but all their descendants are (except descendant groups).
			// Tracks are not counted.

			//! !! DEPRECIATED (Or at least untested to see if it is still accurate after changing Selected to be tri-state)
			System.Diagnostics.Debugger.Break();
			int c = 0;
			for (int l = 0; l < myByDisplayOrderList.Count; l++)
			{
				LOR4MemberType t = myByDisplayOrderList[l].MemberType;
				if (t == LOR4MemberType.Channel)
				{
					if (countPlain)
					{
						if (myByDisplayOrderList[l].SelectedState == CheckState.Checked || !selectedOnly) c++;
					}
				}
				if (t == LOR4MemberType.RGBChannel)
				{
					if (countRGBparents)
					{
						if (myByDisplayOrderList[l].SelectedState == CheckState.Checked || !selectedOnly) c++;
					}
					if (countRGBchildren)
					{
						LOR4RGBChannel rgbCh = (LOR4RGBChannel)myByDisplayOrderList[l];
						if (rgbCh.redChannel.SelectedState == CheckState.Checked || !selectedOnly) c++;
						if (rgbCh.grnChannel.SelectedState == CheckState.Checked || !selectedOnly) c++;
						if (rgbCh.bluChannel.SelectedState == CheckState.Checked || !selectedOnly) c++;
					}
				}
				if (t == LOR4MemberType.ChannelGroup)
				{
					LOR4ChannelGroup grp = (LOR4ChannelGroup)myByDisplayOrderList[l];
					// Recurse!!
					c += grp.Members.DescendantCount(selectedOnly, countPlain, countRGBparents, countRGBchildren);
				}
				if (t == LOR4MemberType.Cosmic)
				{
					LOR4Cosmic dev = (LOR4Cosmic)myByDisplayOrderList[l];
					// Recurse!!
					c += dev.Members.DescendantCount(selectedOnly, countPlain, countRGBparents, countRGBchildren);
				}
			}


			return c;
		}
		public iLOR4Member Parent
		{
			get
			{
				if (myParent == null)
				{
					if (myOwner != null)
					{
						myParent = myOwner.Parent;
					}
				}
				return myParent;
			}
		}
		//bool IEnumerator.MoveNext()
		//{
		//	throw new NotImplementedException();
		//}

		//void IEnumerator.Reset()
		//{
		//	throw new NotImplementedException();
		//}
		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~LOR4Membership() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		//void IDisposable.Dispose()
		//{
		// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//Dispose(true);
		// TODO: uncomment the following line if the finalizer is overridden above.
		// GC.SuppressFinalize(this);
		//}
		#endregion


	} // end LOR4Membership Class (Collection)




}
