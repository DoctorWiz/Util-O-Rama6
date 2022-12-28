using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR4;
using FileHelper;
using ReadWriteCsv;
using FuzzORama;
//using RecentlyUsed;

namespace UtilORama4
{
  /// <summary>
  /// Static <c>Selections</c> class used to select Channels, Groups, Tracks, etc. in a Sequence by name, type, and savedIndex
  /// </summary>
  internal static class Selections
  {

    //public static MRU recentLists = new MRU("ChannelLists");
    public static List<iLOR4Member> selectedMembers = new List<iLOR4Member>();
    // I expect that most, if not all, of the time I will also want to select what I have [successfully] matched
    //    But if I decide/need otherwise, I can turn it off
    public static bool autoSelect = true;
    public static bool fuzzySelect = true;

    /// <summary>
    /// Function <c>LoadSelections</c> returns a Sequence loaded from the specified filename and
    /// pre-selects any/all members listed in the specified .chlist file.
    /// <param><c>sequenceFile</c> specifies a Light-O-Rama Showtime S4 Sequence file (*.lms, *.las)</param>
    /// <param><c>selectionFile</c> specifies a Util-O-Rama Channel List file (*.chlist)</param>
    /// <returns><c>LOR4Sequence</c></returns>
    /// </summary>
    public static LOR4Sequence LoadSelections(string sequenceFile, string selectionFile)
    {
      LOR4Sequence seq = new LOR4Sequence(sequenceFile);
      int count = LoadSelections(seq, selectionFile);
      return seq;
    }

    /// <summary>
    /// Function <c>LoadSelections</c> selects any/all members listed in the specified .chlist file.
    /// <param><c>seq</c> specifies a Util-O-Rama LOR4Sequence object</param>
    /// <param><c>selectionFile</c> specifies a Util-O-Rama Channel List file (*.chlist)</param>
    /// <returns><c>int</c> How many members were selected.</returns>
    /// </summary>
    public static int LoadSelections(LOR4Sequence seq, string selectionFile)
    {
      int count = SelectExactMatches(seq, selectionFile);
      if (fuzzySelect)
      { count += SelectFuzzyMatches(seq, selectionFile); }
      //recentLists.UseItem(selectionFile);	
      return count;
    }

    /// <summary>
    /// Function <c>SelectExactMatches</c> selects any/all members listed in the specified .chlist file whos
    /// name and type match exactly.
    /// <param><c>seq</c> specifies a Util-O-Rama LOR4Sequence object</param>
    /// <param><c>selectionFile</c> specifies a Util-O-Rama Channel List file (*.chlist)</param>
    /// <returns><c>int</c> How many members were selected.</returns>
    /// </summary>
    public static int SelectExactMatches(LOR4Sequence seq, string selectionFile)
    {
      int count = 0;
      int errs = 0;
      int lineCount = 0;
      string typeName = "";
      string theName = "";
      // Turn the list of all Channels to a generic list (which only contains channels)
      List<iLOR4Member> channelList = ConvertList(seq.Channels);
      try
      {
        CsvFileReader reader = new CsvFileReader(selectionFile);
        CsvRow row = new CsvRow();
        // Read and throw away first line which is headers;
        reader.ReadRow(row);
        lineCount++;
        while (reader.ReadRow(row))
        {
          lineCount++;
          try
          {
            // Line starts with the Member Type
            typeName = row[0];
            LOR4MemberType type = LOR4SeqEnums.EnumMemberType(typeName);
            switch (type)
            {
              case LOR4MemberType.None:
                // Any not recognized as MemberType shall considered to be
                // some sort of comment, and ignored.
                break;
              case LOR4MemberType.Channel:
                // Channels get processed!
                theName = row[1];  // Field 1 Name
                int si = -1;
                int.TryParse(row[2], out si);   // Field 2 SavedIndex
                                                // Lets try to find it!
                FindExactMatch(channelList, theName, LOR4MemberType.Channel);
                break;
              case LOR4MemberType.ChannelGroup:
                // Anything which is not a plain channel will get ignored, since
                // everything other than a plain channel is basically a group or
                // group-like.  Therefore the selection state will be reflective
                // of its children's selection state
                break;
              case LOR4MemberType.RGBChannel:
                // See comment above for ChannelGroup
                break;
              case LOR4MemberType.Track:
                // See comment above for ChannelGroup
                break;
              case LOR4MemberType.Cosmic:
                break;
              case LOR4MemberType.Timings:
                break;
              case LOR4MemberType.VizChannel:
              // Visualizer-based objects are not supported (yet)
              case LOR4MemberType.VizDrawObject:
              // Visualizer-based objects are not supported (yet)
              case LOR4MemberType.VizItemGroup:
                // Visualizer-based objects are not supported (yet)
                // See also, comments about groups, above
                break;

            }
          }
          catch (Exception ex)
          {
            string msg = "Error " + ex.ToString() + " while reading line number " + lineCount.ToString();
            msg += " type " + typeName + " " + theName;
            if (Fyle.isWiz)
            {
              DialogResult dr = MessageBox.Show(msg, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            errs++;
          }
        }
        reader.Close();
      }
      catch (Exception ex)
      {
        string msg = "Error " + ex.ToString() + " while reading Selection file " + selectionFile;
        if (Fyle.isWiz)
        {
          DialogResult dr = MessageBox.Show(msg, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        errs++;
      }
      return count;
    }

    /// <summary>
    /// Function <c>SelectFuzzyMatches</c> selects any/all members listed in the specified .chlist file whos
    /// name and type match the highest minimum score using a fuzzy name match.
    /// <param><c>seq</c> specifies a Util-O-Rama LOR4Sequence object</param>
    /// <param><c>selectionFile</c> specifies a Util-O-Rama Channel List file (*.chlist)</param>
    /// <returns><c>int</c> How many members were selected.</returns>
    /// <remark><c>SelectExactMatches</c> should have been run first with <c>autoSelect</c> turned on so
    /// that this doesn't change the selection of a channel with an exact match and to save unnessary fuzzy processing
    /// of exactly matched items.</remark>
    /// </summary>
    public static int SelectFuzzyMatches(LOR4Sequence seq, string selectionFile)
    {
      // FindExactMatches(...) should have been run first, and exactly matched channels should have Selected state
      // set to CheckState.Checked
      int count = 0;
      int errs = 0;
      int lineCount = 0;
      string typeName = "";
      string theName = "";
      // Turn the list of all Channels to a generic list (which only contains channels)
      List<iLOR4Member> channelList = ConvertList(seq.Channels);
      try
      {
        CsvFileReader reader = new CsvFileReader(selectionFile);
        CsvRow row = new CsvRow();
        // Read and throw away first line which is headers;
        reader.ReadRow(row);
        lineCount++;
        while (reader.ReadRow(row))
        {
          lineCount++;
          try
          {
            // Line starts with the Member Type
            typeName = row[0];
            LOR4MemberType type = LOR4SeqEnums.EnumMemberType(typeName);
            switch (type)
            {
              case LOR4MemberType.None:
                // Any not recognized as MemberType shall considered to be
                // some sort of comment, and ignored.
                break;
              case LOR4MemberType.Channel:
                // Channels get processed!
                theName = row[1];  // Field 1 Name
                                   // SavedIndex is not needed or used for a fuzzy match but collect it anyway
                                   // for diagnostic purposes.
                int si = -1;
                int.TryParse(row[2], out si);   // Field 2 SavedIndex
                                                // Lets try to find it!
                FindFuzzyMatch(channelList, theName, LOR4MemberType.Channel);
                break;
              case LOR4MemberType.ChannelGroup:
                // Anything which is not a plain channel will get ignored, since
                // everything other than a plain channel is basically a group or
                // group-like.  Therefore the selection state will be reflective
                // of its children's selection state
                break;
              case LOR4MemberType.RGBChannel:
                // See comment above for ChannelGroup
                break;
              case LOR4MemberType.Track:
                // See comment above for ChannelGroup
                break;
              case LOR4MemberType.Cosmic:
                break;
              case LOR4MemberType.Timings:
                break;
              case LOR4MemberType.VizChannel:
              // Visualizer-based objects are not supported (yet)
              case LOR4MemberType.VizDrawObject:
              // Visualizer-based objects are not supported (yet)
              case LOR4MemberType.VizItemGroup:
                // Visualizer-based objects are not supported (yet)
                // See also, comments about groups, above
                break;

            }
          }
          catch (Exception ex)
          {
            string msg = "Error " + ex.ToString() + " while reading line number " + lineCount.ToString();
            msg += " type " + typeName + " " + theName;
            if (Fyle.isWiz)
            {
              DialogResult dr = MessageBox.Show(msg, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            errs++;
          }
        }
        reader.Close();
      }
      catch (Exception ex)
      {
        string msg = "Error " + ex.ToString() + " while reading Selection file " + selectionFile;
        if (Fyle.isWiz)
        {
          DialogResult dr = MessageBox.Show(msg, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        errs++;
      }
      return count;
    }

    /// <summary>
    /// Function <c>FindExactMatch(List<iLOR4Member> fromMembers, ...)</c> returns the Member with the exact same name and type, if found.
    /// <param><c>fromMembers</c> specifies a List of generic members (iLOR4Member).</param>
    /// <param><c>theName</c> and <c>type</c> are the name and MemberType to match exactly.</param>
    /// <param><c>savedIndex</c> may optionally be specified as a hint to help speed up the search.</param>/>
    /// <returns><c>iLOR4Member</c></returns>
    /// </summary>
    public static iLOR4Member FindExactMatch(List<iLOR4Member> fromMembers, string theName, LOR4MemberType type, int savedIndex = -1)
    {
      iLOR4Member match = null;
      // If a SavedIndex was specified, try to find it that way first (Faster!!)
      if (savedIndex >= 0)
      {
        for (int i = 0; i < fromMembers.Count; i++)
        {
          iLOR4Member mbr = fromMembers[i];
          // Compare SavedIndex
          if (mbr.SavedIndex == savedIndex)
          {
            // Compare types
            if (mbr.MemberType == type)
            {
              // SavedIndex matched!  Does the Name?!?!
              if (mbr.Name == theName)
              {
                // Got it!
                if (autoSelect)
                {
                  mbr.Selected = CheckState.Checked;
                  selectedMembers.Add(mbr);
                }
                match = mbr;
                i = fromMembers.Count; // Force exit of loop
              } // End Name Matches!
            } // End Type matches
          } // End SavedIndex matches what was specified
        } // End loop thru members
      } // End SavedIndex specified

      if (match == null)
      {
        // OK, so we didn't find it by SavedIndex--
        // Don't panic, SavedIndex may have changed but the Name stayed the same
        for (int i = 0; i < fromMembers.Count; i++)
        {
          iLOR4Member mbr = fromMembers[i];
          // Compare types
          if (mbr.MemberType == type)
          {
            // Do Not Compare SavedIndex this time
            // Does the Name match?!?!
            if (mbr.Name == theName)
            {
              // Got it!
              if (autoSelect)
              {
                mbr.Selected = CheckState.Checked;
                selectedMembers.Add(mbr);
              }
              match = mbr;
              i = fromMembers.Count; // Force exit of loop
            } // End Name Matches!!
          } // End Type matches
        } // End loop thru members
      } // End either no SavedIndex was specified, or it wasn't found

      // Return what we found, if anything
      return match;
    }

    /// <summary>
    /// Function <c>FindExactMatch(List<LOR4Membership> fromMembership, ...)</c> returns the Member with the exact same name and type, if
    /// found.  This function is a wrapper to <c>FindExactMatch(List<iLOR4Member> fromMember, ...)</c>
    /// <param><c>fromMembership</c> specifies a group of members (LOR4Membership).</param>
    /// <param><c>theName</c> and <c>type</c> are the name and MemberType to match exactly.</param>
    /// <param><c>savedIndex</c> may optionally be specified as a hint to help speed up the search.</param>/>
    /// <returns><c>iLOR4Member</c></returns>
    /// <remarks>
    /// Regenerates a generic List of members from the membership with each and every call.  So for performance reasons, it is better to
    /// create and store the generic list once for all MemberType using the <c>ConvertList</c> function, then pass that list to the
    /// <c>FindExactMatch((List, name, type, savedIndex)</c> function.
    /// </remarks>
    /// <seealso cref="FindExactMatch(List{iLOR4Member}, string, LOR4MemberType, int)"/>
    /// </summary>
    public static iLOR4Member FindExactMatch(LOR4Membership fromMembership, string theName, LOR4MemberType type, int savedIndex = -1)
    {
      iLOR4Member match = null;
      List<iLOR4Member> list = fromMembership.Members;
      iLOR4Member mbr = FindExactMatch(list, theName, type, savedIndex);
      if (mbr != null)
      {
        // Probably redundant, but check anyway
        if (mbr.MemberType == type)
        {
          match = mbr;
        }
      }
      return match;
    }

    /// <summary>
    /// Function <c>FindExactMatch</c> returns the Channel with the exact same name, if found.  <c>SavedIndex</c> may optionally be specified as a hint to speed up the search.
    /// <returns><c>Channel</c></returns>
    /// <remarks>
    /// Regenerates a generic list with each call.  So for performance reasons, it is better to create and store the generic list once for
    /// all ChannelGroups using the <c>ConvertList</c> function, then pass that list to the <c>FindExactMatch((List, name, type, savedIndex)</c>
    /// </remarks>
    /// <seealso cref="FindExactMatch(List{iLOR4Member}, string, LOR4MemberType, int)"/>
    /// </summary>
    public static LOR4Channel FindExactMatch(List<LOR4Channel> fromMembers, string theName, int savedIndex = -1)
    {
      LOR4Channel match = null;
      List<iLOR4Member> list = ConvertList(fromMembers);
      iLOR4Member mbr = FindExactMatch(list, theName, LOR4MemberType.Channel, savedIndex);
      if (mbr != null)
      {
        // Probably redundant, but check anyway
        if (mbr.MemberType == LOR4MemberType.Channel)
        {
          match = (LOR4Channel)mbr;
        }
      }
      return match;
    }

    /// <summary>
    /// Function <c>FindExactMatch</c> returns the ChannelGroup with the exact same name, if found.  <c>SavedIndex</c> may optionally be specified as a hint to speed up the search.
    /// <returns><c>ChannelGroup</c></returns>
    /// <remarks>
    /// Regenerates a generic list with each call.  So for performance reasons, it is better to create and store the generic list once for
    /// all ChannelGroups using the <c>ConvertList</c> function, then pass that list to the <c>FindExactMatch((List, name, type, savedIndex)</c>
    /// </remarks>
    /// <seealso cref="FindExactMatch(List{iLOR4Member}, string, LOR4MemberType, int)"/>
    /// </summary>
    public static LOR4ChannelGroup FindExactMatch(List<LOR4ChannelGroup> fromMembers, string theName, int savedIndex = -1)
    {
      LOR4ChannelGroup match = null;
      List<iLOR4Member> list = ConvertList(fromMembers);
      iLOR4Member mbr = FindExactMatch(list, theName, LOR4MemberType.ChannelGroup, savedIndex);
      if (mbr != null)
      {
        // Probably redundant, but check anyway
        if (mbr.MemberType == LOR4MemberType.ChannelGroup)
        {
          match = (LOR4ChannelGroup)mbr;
        }
      }
      return match;
    }

    /// <summary>
    /// Function <c>FindExactMatch(List<iLOR4Member> fromMembers, ...)</c> returns the Member with the exact same name and type, if found.
    /// <param><c>fromMembers</c> specifies a List of generic members (iLOR4Member).</param>
    /// <param><c>theName</c> and <c>type</c> are the name and MemberType to match exactly.</param>
    /// <param><c>savedIndex</c> may optionally be specified as a hint to help speed up the search.</param>/>
    /// <returns><c>iLOR4Member</c></returns>
    /// </summary>
    public static iLOR4Member FindFuzzyMatch(List<iLOR4Member> fromMembers, string theName, LOR4MemberType type)
    {
      iLOR4Member match = null;
      double score = 0D;
      double bestScore = 0D;
      int bestIndex = -1;

      for (int i = 0; i < fromMembers.Count; i++)
      {
        iLOR4Member member = fromMembers[i];
        // Check selected state, has it already been matched up to something?
        if (member.Selected == CheckState.Unchecked)
        {
          // Compare types
          if (member.MemberType == type)
          {
            // So far so good.  How well does the name match?
            score = member.Name.FuzzyScoreFast(theName);
            // Above the minimum?
            if (score > FuzzyFunctions.SUGGESTED_MIN_PREMATCH_SCORE)
            {
              // Refine the match score further
              score = member.Name.FuzzyScoreAccurate(theName);
              // Still past the minimum
              if (score > FuzzyFunctions.SUGGESTED_MIN_FINAL_SCORE)
              {
                // Is it the best (so far)?
                if (score > bestScore)
                {
                  // Remember this one
                  bestScore = score;
                  bestIndex = i;
                } // End best, so far
              } // End above minimum final-match score
            } // End above minimum pre-match
          } // End type matches what was specified
        } // End member is not already selected/matched
      } // Loop thru list

      // Did we find one?
      if (bestIndex >= 0)
      {
        // Set our return value
        match = fromMembers[bestIndex];
        // Set selection to checked, remember it matched
        match.Selected = CheckState.Checked;
        // Add it to our list
        selectedMembers.Add(match);
      }

      // Return what we found, if anything
      return match;
    }



    /// <summary>
    /// Function <c>ConvertList(LOR4Membership)</c> is basically just a wrapper.
    /// <param><c>fromMembership</c> specifies the <c>LOR4Membership</c> of a ChannelGroup or other group-like member.
    /// <returns>[generic] <c>List<iLOR4Member</c></returns>
    /// </summary>
    public static List<iLOR4Member> ConvertList(LOR4Membership fromMembership)
    {
      return fromMembership.ByDisplayOrder;
    }

    /// <summary>
    /// Function <c>ConvertList</c> converts a list of a specific MemberType (such as Channel) to a list of generic Members (iLOR4Member)
    /// <param><c>fromMembers</c> specifies a <c>List</c> of the type <c>LOR4Channel</c>.</param>
    /// <returns><c>List<iLOR4Member</c></returns>
    /// </summary>
    public static List<iLOR4Member> ConvertList(List<LOR4Channel> fromMembers)
    {
      List<iLOR4Member> newList = new List<iLOR4Member>();
      for (int i = 0; i < fromMembers.Count; i++)
      {
        newList.Add(fromMembers[i]);
      }
      return newList;
    }

    /// <summary>
    /// Function <c>ConvertList</c> converts a list of a specific MemberType (such as ChannelGroup) to a list of generic Members (iLOR4Member)
    /// <param><c>fromMembers</c> specifies a <c>List</c> of the type <c>LOR4ChannelGroup</c>.</param>
    /// <returns><c>List<iLOR4Member</c></returns>
    /// </summary>
    public static List<iLOR4Member> ConvertList(List<LOR4ChannelGroup> fromMembers)
    {
      List<iLOR4Member> newList = new List<iLOR4Member>();
      for (int i = 0; i < fromMembers.Count; i++)
      {
        newList.Add(fromMembers[i]);
      }
      return newList;
    }

    /// <summary>
    /// Function <c>ConvertList</c> converts a list of a specific MemberType (such as RGBChannel) to a list of generic Members (iLOR4Member)
    /// <param><c>fromMembers</c> specifies a <c>List</c> of the type <c>LOR4RGBChannel</c>.</param>
    /// <returns><c>List<iLOR4Member</c></returns>
    /// </summary>
    public static List<iLOR4Member> ConvertList(List<LOR4RGBChannel> fromMembers)
    {
      List<iLOR4Member> newList = new List<iLOR4Member>();
      for (int i = 0; i < fromMembers.Count; i++)
      {
        newList.Add(fromMembers[i]);
      }
      return newList;
    }

    /// <summary>
    /// Function <c>GroupSelection</c> enumerates through the membership of a ChannelGroup or group-like member to find if All, Some, or
    /// None of them are selected.  Does so recursively including any subgroups.
    /// <returns><c>CheckState</c> .Checked, .Indeterminate, or .UnChecked, accordingly.</returns>
    /// <param><c>fromMembership</c> specifies the <c>LOR4Membership</c> of a ChannelGroup or group-like member.</param>
    /// </summary>
    public static CheckState GroupSelection(LOR4Membership fromMembership)
    {
      CheckState finalState = CheckState.Unchecked;
      CheckState groupState = CheckState.Indeterminate;
      // Start with 'All' true, if ANY are NOT selected, set it false
      bool all = true;
      // Start with 'Some' false, if ANY are selected, set it true
      bool some = false;

      // Loop thru all members in the membership
      for (int i = 0; i < fromMembership.Members.Count; i++)
      {
        iLOR4Member member = fromMembership.Members[i];
        LOR4MemberType type = member.MemberType;
        // What type is this member?
        switch (type)
        {
          case LOR4MemberType.Channel:  //! CHANNEL
                                        // If the channel is selected, then at least Some is true
            if (member.Selected == CheckState.Checked)
            { some = true; }
            // If the channel is not selected, then All can't be true
            if (member.Selected == CheckState.Unchecked)
            { all = false; }
            break;

          case LOR4MemberType.RGBChannel:   //! RGB CHANNEL
                                            // Cast member so we can get subchannels
            LOR4RGBChannel rgbChan = (LOR4RGBChannel)member;
            bool rgbAll = true;
            bool rgbSome = false;
            // Are ANY of the colored subchannels selected?
            if ((rgbChan.redChannel.Selected == CheckState.Checked) ||
               (rgbChan.grnChannel.Selected == CheckState.Checked) ||
               (rgbChan.bluChannel.Selected == CheckState.Checked))
            // Then Some is True
            { rgbSome = true; some = true; }
            // Are ANY of the colored subchannels unselected?
            if ((rgbChan.redChannel.Selected == CheckState.Unchecked) ||
               (rgbChan.grnChannel.Selected == CheckState.Unchecked) ||
               (rgbChan.bluChannel.Selected == CheckState.Unchecked))
            // Then All must be false
            { rgbAll = false; all = false; }
            // If all 3 colors are selected, state is Checked
            if (rgbAll) { rgbChan.Selected = CheckState.Checked; }
            // If none of the 3 colors are selected, state is Unchecked
            else if (!rgbSome) { rgbChan.Selected = CheckState.Unchecked; }
            // If some, but not all of the 3 colors are selected, state is Indeterminate
            else { rgbChan.Selected = CheckState.Indeterminate; }
            break;

          case LOR4MemberType.ChannelGroup:   //! CHANNEL GROUP
                                              // Cast member to a group so we can get its membership
            LOR4ChannelGroup group = (LOR4ChannelGroup)member;
            //# RECURSE- Check state of this groups members
            groupState = GroupSelection(group.Members);
            // If all or any of the members are selected, then Some must be true
            if ((groupState == CheckState.Checked) || (groupState == CheckState.Indeterminate))
            { some = true; }
            // If none, or some but not all, of the members are selected, then All can't be true
            if ((groupState == CheckState.Unchecked) || (groupState == CheckState.Indeterminate))
            { all = false; }
            group.Selected = groupState;
            break;

          case LOR4MemberType.Cosmic:   //! COSMIC COLOR DEVICE
            LOR4Cosmic cosmic = (LOR4Cosmic)member;
            groupState = GroupSelection(cosmic.Members);
            // If all or any of the members are selected, then Some must be true
            if ((groupState == CheckState.Checked) || (groupState == CheckState.Indeterminate))
            { some = true; }
            // If none, or some but not all, of the members are selected, then All can't be true
            if ((groupState == CheckState.Unchecked) || (groupState == CheckState.Indeterminate))
            { all = false; }
            cosmic.Selected = groupState;
            break;

          case LOR4MemberType.Track:    //! TRACK
                                        // (See comments for ChannelGroup)
            LOR4Track track = (LOR4Track)member;
            groupState = GroupSelection(track.Members);
            if ((groupState == CheckState.Checked) || (groupState == CheckState.Indeterminate))
            { some = true; }
            if ((groupState == CheckState.Unchecked) || (groupState == CheckState.Indeterminate))
            { all = false; }
            track.Selected = groupState;
            break;

          case LOR4MemberType.VizChannel:   //! VISUALIZER CHANNEL
                                            // (See comments for Channel)
            if (member.Selected == CheckState.Checked)
            { some = true; }
            if (member.Selected == CheckState.Unchecked)
            { all = false; }
            break;

          case LOR4MemberType.VizDrawObject:    //! VISUALIZER DRAW OBJECT
                                                // (See comments for Channel)
            if (member.Selected == CheckState.Checked)
            { some = true; }
            if (member.Selected == CheckState.Unchecked)
            { all = false; }
            break;

          case LOR4MemberType.VizItemGroup:   //! VISUALIZER ITEM GROUP
                                              // (See comments for ChannelGroup)
            LOR4VizItemGroup items = (LOR4VizItemGroup)member;
            groupState = GroupSelection(items.Members);
            if ((groupState == CheckState.Checked) || (groupState == CheckState.Indeterminate))
            { some = true; }
            if ((groupState == CheckState.Unchecked) || (groupState == CheckState.Indeterminate))
            { all = false; }
            items.Selected = groupState;
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


    /// <summary>
    /// Function <c>SaveSelections</c> creates and saves a Channel List file (*.chlist) containing the type, the name, and the savedIndex
    /// of all <b><i>selected</i></b> Channels, RGB Channels, Groups, Tracks, etc. in the sequence.
    /// <param><c>seq</c> specifies a populated Util-O-Rama LOR4Sequence object.</param>
    /// <param><c>selectionFile</c> specifies a Util-O-Rama Channel List file (*.chlist)</param>
    /// <returns><c>int</c> count of number of items written.</returns>
    /// </summary>
    public static int SaveSelections(LOR4Sequence seq, string selectionFile)
    {
      int count = 0;
      int errs = 0;
      int lineCount = 0;
      string typeName = "";
      string itemName = "";
      try
      {
        CsvFileWriter writer = new CsvFileWriter(selectionFile);
        lineCount++;
        CsvRow row = new CsvRow();
        // Create first line which is headers;
        writer.WriteLine(LOR4Admin.CSVHEAD_CHLIST);

        // Go ahead a look this up, once, ahead of time, and cache it
        typeName = LOR4SeqEnums.MemberTypeName(LOR4MemberType.Channel);
        // First pass - Channels
        for (int chi = 0; chi < seq.Channels.Count; chi++)
        {
          LOR4Channel channel = seq.Channels[chi];
          try
          {
            if (channel.Selected == CheckState.Checked)
            {
              lineCount++;
              row = new CsvRow();
              row.Add(typeName);  // Field 0
                                  // Copy name to string var first only so we will know what it is in case of an error
              itemName = channel.Name;
              row.Add(itemName);  // Field 1
              row.Add(channel.SavedIndex.ToString());  // Field 2
              row.Add(channel.UniverseNumber.ToString()); // Field 3, strictly for informational purposes
              row.Add(channel.DMXAddress.ToString());  // Field 4, for info
              writer.WriteRow(row);
              count++;
            } // End selected
          } // End try
          catch (Exception ex)
          {
            string msg = "Error " + ex.ToString() + " while saving Selections " + selectionFile;
            msg += " on line " + lineCount.ToString() + " for " + typeName + " " + itemName;
            if (Fyle.isWiz)
            {
              DialogResult dr = MessageBox.Show(msg, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            errs++;
          }
        } // End loop thru list of [selected] channels

        // Next passes - RGBChannels, ChannelGroups, Tracks, etc.
        // even though they will NOT be used when reading the chlist file back in
        // writing them to the file strictly for the sake of us lowly humans or
        // viewing or analzying in a spreadsheet or with external script.
        try
        {
          for (int rgbi = 0; rgbi < seq.RGBchannels.Count; rgbi++)
          {
            LOR4RGBChannel rgbChannel = seq.RGBchannels[rgbi];
            if (rgbChannel.Selected == CheckState.Checked)
            {
              lineCount++;
              row = new CsvRow();
              row.Add(typeName);  // Field 0
              itemName = rgbChannel.Name;
              row.Add(itemName);  // Field 1
              row.Add(rgbChannel.SavedIndex.ToString());  // Field 2
              row.Add(rgbChannel.UniverseNumber.ToString()); // Field 3, strictly for informational purposes
              row.Add(rgbChannel.DMXAddress.ToString());  // Field 4, for info
              writer.WriteRow(row);
              count++;
            } // End selected
          } // End loop thru RGB Channels

          for (int cdi = 0; cdi < seq.CosmicDevices.Count; cdi++)
          {
            LOR4Cosmic cosmic = seq.CosmicDevices[cdi];
            if (cosmic.Selected == CheckState.Checked)
            {
              lineCount++;
              row = new CsvRow();
              row.Add(typeName);  // Field 0
              itemName = cosmic.Name;
              row.Add(itemName);  // Field 1
              row.Add(cosmic.SavedIndex.ToString());  // Field 2
              row.Add(cosmic.UniverseNumber.ToString()); // Field 3, strictly for informational purposes
              row.Add(cosmic.DMXAddress.ToString());  // Field 4, for info
              writer.WriteRow(row);
              count++;
            } // End selected
          } // End loop thru Cosmic Devices

          for (int cgi = 0; cgi < seq.ChannelGroups.Count; cgi++)
          {
            LOR4ChannelGroup group = seq.ChannelGroups[cgi];
            if (group.Selected == CheckState.Checked)
            {
              lineCount++;
              row = new CsvRow();
              row.Add(typeName);  // Field 0
              itemName = group.Name;
              row.Add(itemName);  // Field 1
              row.Add(group.SavedIndex.ToString());  // Field 2
              row.Add(group.UniverseNumber.ToString()); // Field 3, strictly for informational purposes
              row.Add(group.DMXAddress.ToString());  // Field 4, for info
              writer.WriteRow(row);
              count++;
            } // End selected
          } // End loop thru Channel Groups

          for (int tri = 0; tri < seq.Tracks.Count; tri++)
          {
            LOR4Track track = seq.Tracks[tri];
            if (track.Selected == CheckState.Checked)
            {
              lineCount++;
              row = new CsvRow();
              row.Add(typeName);  // Field 0
              itemName = track.Name;
              row.Add(itemName);  // Field 1
              row.Add(track.TrackNumber.ToString());  // Field 2
              row.Add(track.UniverseNumber.ToString()); // Field 3, strictly for informational purposes
              row.Add(track.DMXAddress.ToString());  // Field 4, for info
              writer.WriteRow(row);
              count++;
            } // End selected
          } // End loop thru Tracks

          for (int tgi = 0; tgi < seq.TimingGrids.Count; tgi++)
          {
            LOR4Timings times = seq.TimingGrids[tgi];
            if (times.Selected == CheckState.Checked)
            {
              lineCount++;
              row = new CsvRow();
              row.Add(typeName);  // Field 0
              itemName = times.Name;
              row.Add(itemName);  // Field 1
              row.Add(times.SaveID.ToString());  // Field 2
              writer.WriteRow(row);
              count++;
            } // End selected
          } // End loop thru Timing Grids

          //TODO (eventually) add support for selected visualizer-related members

        } // End try
        catch (Exception ex)
        {
          string msg = "Error " + ex.ToString() + " while saving Selections " + selectionFile;
          msg += " on line " + lineCount.ToString() + " for " + typeName + " " + itemName;
          if (Fyle.isWiz)
          {
            DialogResult dr = MessageBox.Show(msg, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          errs++;
        } // End try/catch containing loops thru list of [selected] RGB Channels, Groups, Tracks, etc.

        writer.Close();
      }
      catch (Exception ex)
      {
        string msg = "Error " + ex.ToString() + " while saving Selections file " + selectionFile;
        if (Fyle.isWiz)
        {
          DialogResult dr = MessageBox.Show(msg, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        errs++;
      }
      return count;
    }

    public static CheckState InvertCheckState(CheckState state)
    {
      CheckState ret = state; // Default For Indeterminate
                              // Checked to Unchecked
      if (state == CheckState.Checked) { ret = CheckState.Unchecked; }
      // Unchecked to Checked (duh)
      else { if (state == CheckState.Unchecked) { ret = CheckState.Checked; } }
      return ret;
    }
  }
}
