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
	public partial interface iLOR4Member : IComparable<iLOR4Member>
	{
		string Name
		{ get; }
		void ChangeName(string newName);
		int Centiseconds
		{ get; set; }
		int Index
		{ get; }
		void SetIndex(int theIndex);
		int ID { get; }
		void SetID(int newID);
		int AltID
		{ get; set; }
		int SavedIndex
		{ get; }
		void SetSavedIndex(int theSavedIndex);

		// For LOR members, color (lower case) is a LOR color in format int32 0x00BBGGRR (Blue, Green, Red)(No Alpha)
		// for xLights members, Color (upper case) is a standard .Net/HTML color in format 0xAARRGGBB (Alpha, Red, Green, Blue)
		// LOR4 has functions for converting back and forth
		int color
		{ get; set; }
		Color Color
		{ get; set; }

		// For Channels, RGBChannels, ChannelGroups, Tracks, Timings, etc.  This will be the parent Sequence
		// For VizChannels and VizDrawObjects this will be the parent Visualization
		iLOR4Member Parent
		{ get; }
		void SetParent(iLOR4Member newParent);

		CheckState SelectedState
		{ get; set; }
		bool Dirty
		{ get; }
		void MakeDirty(bool dirtyState = true);
		LOR4MemberType MemberType
		{ get; }
		int CompareTo(iLOR4Member otherObj);
		string LineOut();
		string ToString();
		void Parse(string lineIn);
		bool Written
		{ get; }
		iLOR4Member Clone();
		iLOR4Member Clone(string newName);
		object Tag
		{ get; set; }
		iLOR4Member MapTo
		{ get; set; }
		bool ExactMatch
		{ get; set; }
		int UniverseNumber
		{ get; }
		int DMXAddress
		{ get; }
		void SetAddress(int universe, int dmxAddress);
		string Comment
		{ get; set; }
		int ZCount
		{ get; set; }

	}
}