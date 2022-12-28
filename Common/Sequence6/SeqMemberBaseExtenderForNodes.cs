using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;

//! IMPORTANT ! !
//* Be sure to Add 'TREENODES' to Project Properties -> Build -> Conditional Compile Symbols

namespace LOR4
{
  public partial interface iLOR4Member : IComparable<iLOR4Member>
  {
    List<TreeNodeAdv> Nodes
    { get; set; }

    ListViewItem ListViewItem
    { get; set; }
  }

  public abstract partial class LOR4MemberBase : iLOR4Member, IComparable<iLOR4Member>
  {
    // Holds a List<TreeNodeAdv> for SyncFusion's TreeViewAdv in projects that use it.
    protected List<TreeNodeAdv> myNodes = new List<TreeNodeAdv>();
    // Holds a ListViewItem for Timing Grids in projects that use it.
    protected ListViewItem myListViewItem = new ListViewItem("");

    public virtual List<TreeNodeAdv> Nodes
    { get { return myNodes; } set { myNodes = value; } }

    public virtual ListViewItem ListViewItem
    { get { return myListViewItem; } set { myListViewItem = value; } }

    public virtual CheckState SelectedState
    {
      get { return myCheckState; }
      set
      {
        if (MemberType == LOR4MemberType.Timings)
        {
          if (myListViewItem != null)
          {
            if (value == CheckState.Checked)
            {
              myListViewItem.Checked = true;
            }
            else
            {
              myListViewItem.Checked = false;
            }
          } // end not null
        } // end if timing grid
        else
        {
          //TODO Should probably check to see if it is Track, Group, Channel, RGB, Cosmic...
          //TODO and not Sequence, Visualization...
          for (int n = 0; n < Nodes.Count; n++)
          {
            Nodes[n].CheckState = value;
          }
        } // end not timing grid
        myCheckState = value;
      } // end set
    } // end Selected property
  } // end class LOR4MemberBase
} // end namespace