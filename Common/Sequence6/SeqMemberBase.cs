using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using FileHelper;

//! FOR PROJECTS THAT INCLUDE THE [SYNCFUSION] TREEVIEW---
// Include the file 'SeqMemberBaseExtenderForNodes.cs' in the project
// Be sure to Add 'TREENODES' to Project Properties -> Build -> Conditional Compile Symbols

namespace LOR4
{
  public abstract partial class LOR4MemberBase : iLOR4Member, IComparable<iLOR4Member>
  {
    protected string myName = "";
    protected int myCentiseconds = LOR4Admin.UNDEFINED;
    protected int myIndex = LOR4Admin.UNDEFINED;
    protected int myID = LOR4Admin.UNDEFINED;
    protected int myAltID = LOR4Admin.UNDEFINED;
    protected int mycolor = 0; // Note: LOR color, 12-bit int in BGR order
                               // Do not confuse with .Net or HTML color, 16 bits in ARGB order
    protected iLOR4Member myParent = null;
    //protected bool imSelected = false;
    protected CheckState myCheckState = CheckState.Unchecked;
    protected bool isDirty = false;
    protected bool isExactMatch = false;
    protected object myTag = null; // General Purpose Object
                                   // Note: Mapped to is used by Map-O-Rama and possibly by other utils in the future
                                   // Only holds a single member so only works for destination to source mapping
                                   // source-to-destination mapping may include multiple members, and is stored in a List<iLOR4Member> stored in the Tag property
    protected iLOR4Member mappedTo = null;

    // Holds a List<TreeNodeAdv> but is not defined that way, so that this base member is NOT dependant on
    // SyncFusion's TreeViewAdv in projects that don't use it.
    // protected object myNodes = null;
    protected int myUniverseNumber = LOR4Admin.UNDEFINED;
    protected int myDMXAddress = LOR4Admin.UNDEFINED;
    protected string myComment = ""; // Not really a comment somuch as a general purpose temporary string.
    protected int miscNumber = 0; // General purpose temporary integer.  Use varies according to utility and function.
    protected string _testInfo = "";
    protected object _testObj = null;

    internal LOR4MemberBase()
    // Necessary to be the base member of other members
    // Should never be called directly!
    { }

    public virtual string Name
    { get { return myName; } }
    // Note: Name property does not have a 'set'.  Uses ChangeName() instead-- because this property is
    // usually only set once and not usually changed thereafter.
    public void ChangeName(string newName)
    {
      myName = newName;
      MakeDirty(true);
    }

    public virtual int Centiseconds
    {
      get
      {
        return myCentiseconds;
      }
      set
      {
        if (value != myCentiseconds)
        {
          if (value > 360000)
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
            if (value < 950)
            {
              string m = "WARNING!! Setting Centiseconds to less than 1 second!  Are you sure?";
              //Fyle.WriteLogEntry(m, "Warning",true);
              if (Fyle.DebugMode)
              {
                //System.Diagnostics.Debugger.Break();
              }
            }
            else
            {
              myCentiseconds = value;
              if (myParent != null)
              {
                if (myParent.Centiseconds < value)
                {
                  myParent.Centiseconds = value;
                }
              }
              MakeDirty(true);
            }
          }
        }
      }
    }

    public int Index
    { get { return myIndex; } }
    // Note: Index property does not have a 'set'.  Uses SetIndex() instead-- because this property is
    // usually only set once and not usually changed thereafter.
    public void SetIndex(int theIndex)
    {
      myIndex = theIndex;
      //MakeDirty(true);
    }

    public int ID
    { get { return myID; } }
    // Note: SavedIndex property does not have a 'set'.  Uses SetSavedIndex() instead-- because this property is
    // usually only set once and not usually changed thereafter.
    public void SetID(int newID)
    {
      myID = newID;
      //MakeDirty(true);
    }

    public int AltID
    { get { return myAltID; } set { myAltID = value; } }

    public int SavedIndex
    { get { return myID; } }
    public void SetSavedIndex(int newSavedIndex)
    { myID = newSavedIndex; }

    public int AltSavedIndex
    { get { return myAltID; } set { myAltID = value; } }

    // Important difference-- color with lower case c is LOR color, 12-bit int in BGR order
    public virtual int color
    { get { return mycolor; } set { mycolor = value; } }
    // Whereas Color property with capital C returns the .Net or HTML color in ARGB order
    public virtual Color Color
    { get { return LOR4Admin.Color_LORtoNet(mycolor); } set { mycolor = LOR4Admin.Color_NettoLOR(value); } }


    public virtual iLOR4Member Parent
    { get { return myParent; } }
    // Note: Parent property does not have a 'set'.  Uses SetParent() instead-- because this property is
    // usually only set once and not usually changed thereafter
    public void SetParent(iLOR4Member newParent)
    {
      if (newParent != null)
      {
        Type t = newParent.GetType();
        if (t.Equals(typeof(LOR4Sequence)))
        {
          myParent = (LOR4Sequence)newParent;
        }
        else
        {
          if (t.Equals(typeof(LOR4Visualization)))
          {
            myParent = (LOR4Visualization)newParent;
          }
          else
          {
            if (Fyle.DebugMode)
            {
              // Why are we trying to assign something other than a sequence?!?!
              System.Diagnostics.Debugger.Break();
            }
          }
        }
      }
    }


    //! FOR PROJECTS THAT INCLUDE THE [SYNCFUSION] TREEVIEW---
    // Include the file 'SeqMemberBaseExtenderForNodes.cs' in the project
    // Add 'TREENODES' to Project Properties -> Build -> Conditional Compile Symbols


#if TREENODES
    // Use the Selected property in SeqMemberBaseExtenderForNodes
#else
			// Use the default/normal SelectedState property built into MemberBase and MemberInterface
			public virtual CheckState SelectedState 
			{ get { return myCheckState; } set { myCheckState = value; } }
#endif

    public bool Dirty
    { get { return isDirty; } }
    // Note: Dirty flag is read-only.  Uses MakeDirty() instead-- to set it
    // and optionally to clear it.
    public virtual void MakeDirty(bool dirtyState = true)
    {
      isDirty = dirtyState;
      if (dirtyState)
      {
        if (myParent != null)
        {
          if (!myParent.Dirty)
          {
            myParent.MakeDirty(true);
          }
        }
      }
    }

    // This property is included here to be part of the base interface
    // But every subclass should override it and return their own value
    public virtual LOR4MemberType MemberType
    { get { return LOR4MemberType.None; } }

    public virtual int CompareTo(iLOR4Member other)
    {
      int result = 0;
      //if (parentSequence.Members.sortMode == LOR4Membership.SORTbySavedIndex)
      if (other == null)
      {
        result = 1;
        //string msg = "Why are we comparing " + this.Name + " to null?";
        //msg+= "\r\nClick Cancel, step thru code, check call stack!";
        //Fyle.BUG(msg);

        //! TODO: Find out why we are getting null members in visualizations
        //! This is an ugly kludgy fix in the meantime
        //LOR4MemberBase bass = new LOR4MemberBase(this, "WTF");
        //other = bass;
      }
      else
      {
        if (LOR4Membership.sortMode == LOR4Membership.SORTbyID)
        {
          result = myID.CompareTo(other.ID);
        }
        else
        {
          if (LOR4Membership.sortMode == LOR4Membership.SORTbyName)
          {
            result = myName.CompareTo(other.Name);
          }
          else
          {
            if (LOR4Membership.sortMode == LOR4Membership.SORTbyAltID)
            {
              result = myID.CompareTo(other.AltID);
            }
            else
            {
              if (LOR4Membership.sortMode == LOR4Membership.SORTbyOutput)
              {
                result = UniverseNumber.CompareTo(other.UniverseNumber);
                if (result == 0)
                {
                  result = DMXAddress.CompareTo(other.DMXAddress);
                }
              }
            }
          }
        }
      }
      return result;
    }

    // This function is included here to be part of the base interface
    // But every subclass should override it and return their own value
    public virtual string LineOut()
    {
      return "";
    }

    // The 'Name' property is the default return value for ToString()
    // But subclasses may override it, for sorting, or other reasons
    public override string ToString()
    {
      return myName;
    }

    // This function is included here to be a skeleton for the base interface
    // But every subclass should override it and return their own value
    public virtual void Parse(string lineIn)
    {
      int nu = lineIn.IndexOf(" Name=\"");
      int nl = lineIn.IndexOf(" name=\"");
      if ((nu + nl) > 0)
      {
        //LOR4Sequence Parent = ID.Parent;
        myName = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, LOR4Admin.FIELDname));
        if (myName.Length == 0) myName = LOR4Admin.HumanizeName(LOR4Admin.getKeyWord(lineIn, " Name"));
        myID = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDsavedIndex);
        myCentiseconds = LOR4Admin.getKeyValue(lineIn, LOR4Admin.FIELDcentiseconds);
      }
      else
      {
        myName = lineIn;
      }
      //if (myParent != null) myParent.MakeDirty(true);
    }

    public bool Written
    {
      get
      {
        // Sneaky trick: Uses AltSavedIndex to tell if it has been renumbered and thus written
        bool r = false;
        if (myAltID > LOR4Admin.UNDEFINED) r = true;
        return r;
      }
    }

    public virtual iLOR4Member Clone()
    {
      return Clone(myName);
    }

    public virtual iLOR4Member Clone(string newName)
    {
      iLOR4Member mbr = null;

      switch (this.MemberType)
      {
        case LOR4MemberType.Channel:
          mbr = new LOR4Channel(myParent, newName);
          break;
        case LOR4MemberType.RGBChannel:
          mbr = new LOR4RGBChannel(myParent, newName);
          break;
        case LOR4MemberType.ChannelGroup:
          mbr = new LOR4ChannelGroup(myParent, newName);
          break;
        case LOR4MemberType.Cosmic:
          mbr = new LOR4Cosmic(myParent, newName);
          break;
        case LOR4MemberType.Track:
          mbr = new LOR4Track(myParent, newName);
          break;
        case LOR4MemberType.Timings:
          mbr = new LOR4Timings(myParent, newName);
          break;
        case LOR4MemberType.Sequence:
          mbr = new LOR4Sequence(newName);
          break;
        case LOR4MemberType.Visualization:
          mbr = new LOR4Visualization(myParent, newName);
          break;
        case LOR4MemberType.VizChannel:
          mbr = new LOR4VizChannel(myParent, newName);
          break;
        case LOR4MemberType.VizDrawObject:
          mbr = new LOR4VizDrawObject(myParent, newName);
          break;
        case LOR4MemberType.VizItemGroup:
          mbr = new LOR4VizItemGroup(myParent, newName);
          break;
      }

      mbr.Centiseconds = myCentiseconds;
      mbr.SetIndex(myIndex);
      mbr.SetID(myID);
      mbr.AltID = myAltID;
      mbr.SetSavedIndex(myID);
      mbr.SelectedState = myCheckState;
      mbr.color = mycolor;
      mbr.Tag = myTag;
      //mbr.Nodes = myNodes;
      mbr.MapTo = mappedTo;
      mbr.MakeDirty(isDirty);
      mbr.SetParent(myParent);
      mbr.ExactMatch = isExactMatch;
      mbr.SetAddress(myUniverseNumber, myDMXAddress);
      mbr.Comment = myComment;
      mbr.ZCount = miscNumber;

      //LOR4MemberBase mbr = new LOR4MemberBase(myParent, newName);
      return mbr;
    }

    public object Tag
    { get { return myTag; } set { myTag = value; } }


    public iLOR4Member MapTo
    {
      get
      {
        return mappedTo;
      }
      set
      {
        if (value != null)
        {
          // Hmmmmmmm, do I really want to enforce this??
          if (value.MemberType == this.MemberType)
          {
            mappedTo = value;
          }
          else
          {
            string msg = "Why are you trying to map a " + LOR4SeqEnums.MemberTypeName(value.MemberType);
            msg += " a " + LOR4SeqEnums.MemberTypeName(this.MemberType) + " ?!?";
            //Fyle.BUG(msg);
            Debug.WriteLine(msg);
            Fyle.MakeNoise(Fyle.Noises.Pop);
            // Now that I've been warned, go ahead and do it anyway.
            // (Unless I tell the debugger to step over this next line...)
            //mappedTo = value;
          } // Type Match
        } // Not Null
      }
    }

    public bool ExactMatch
    { get { return isExactMatch; } set { isExactMatch = value; } }

    // Properties are read-only and overridden by subclasses to pull the correct values from the appropriate
    // locations.  Included in base class only as a placeholder.  (No way to set them in base class)
    public virtual int UniverseNumber
    { get { return myUniverseNumber; } }
    public virtual int DMXAddress
    { get { return myDMXAddress; } }
    public virtual void SetAddress(int universe, int dmxAddress)
    {
      myUniverseNumber = universe;
      myDMXAddress = dmxAddress;
    }

    // Not supported by ShowTime, and not saved along with the sequence file.  Included only for temporary use in Util-O-Rama
    public string Comment
    { get { return myComment; } set { myComment = value; } }
    public int ZCount
    { get { return miscNumber; } set { miscNumber = value; } }

    // These are just for testing and debugging, and thus invisible to IntelliSense
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public string TestInfo
    { get { return _testInfo; } set { _testInfo = value; } }
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public object TestObj
    { get { return _testObj; } set { _testObj = value; } }

  }// End class LOR4MemberBase
}
