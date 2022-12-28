using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LOR4
{
  ///////////////////////////////////////////////////////////////////////////////
  //
  //  Viz 4 Stub
  //
  //     Most of the Util-O-Rama programs do NOT use the Visualizer classes,
  //     but parts of the Sequence classes refer to it.
  //			This Stub is for programs that do not use the Visualizer classes
  //			and contains only the needed dummy objects referenced in the
  //			sequencer class.
  //				Include this stub file along with the Sequencer class files
  //				for all utilities which don't need the Visualizer.
  //					Or include the 4 Visualizer class files INSTEAD for those
  //					utilities which DO require it.
  /////////////////////////////////////////////////////////////////////////////



  public class LOR4Visualization : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>
  {

    public LOR4Visualization(iLOR4Member NullParentIgnoreThis, string fileName)
    { }
  } // end Visualization class
  public class LOR4VizChannel : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>
  {
    public LOR4Output output = null;
    public LOR4VizChannel(iLOR4Member theParent, string lineIn)
    {
      output = new LOR4Output(this);
    }
  }
  public class LOR4VizDrawObject : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>
  {
    public LOR4VizDrawObject(iLOR4Member theParent, string lineIn)
    { }
  }
  public class LOR4VizItemGroup : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>
  {
    public LOR4Membership Members = null;
    public LOR4VizItemGroup(iLOR4Member parent, string lineIn)
    {
      Members = new LOR4Membership(this);
    }
  }
} // end namespace LOR4