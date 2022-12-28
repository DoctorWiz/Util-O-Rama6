using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using FileHelper;

namespace LOR4Utils
{
	public class LOR4Placeholder : LOR4MemberBase, iLOR4Member, IComparable<iLOR4Member>
	{
		public LOR4Placeholder()
		{ }
		public override LOR4MemberType MemberType
		{ get { return LOR4MemberType.None; } }
	}// End class LOR4MemberBase
}
