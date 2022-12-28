using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace xLights22
{
	public enum xMemberType { Unknown, Base, Model, RGBmodel, Pixels, ModelGroup }


	public interface ixMember : IComparable<ixMember>
	{
		string Name
		{
			get;
		}
		void ChangeName(string newName);

		int CompareTo(ixMember otherObj);

		string ToString();

		xMemberType MemberBaseType
		{ get; }

		xModelSubType MemberSubType
		{ get; set; }

		CheckState SelectedState
		{ get; set; }

		bool ExactMatch
		{ get; set; }

		object Tag
		{ get; set; }

		int UniverseNumber
		{ get; set; }

		int DMXAddress
		{ get; set; }

		int xLightsAddress
		{ get; set; }

		string StartChannel
		{ get; set; }

		Color Color
		{ get; set; }

	} // End Interface xMember

	public abstract class xMemberBase : ixMember, IComparable<ixMember>
	{
		protected string myName = "";
		private Color myColor = Color.Black;
		private CheckState isSelected = CheckState.Unchecked;
		private bool matchExact = false;
		private object tagItem = null;
		private int xaddress = -1;
		private string startChannel = "";
		private int dmxAddress = 1;
		private int universeNumber = 1;
		private xMemberType memberBaseType = xMemberType.Base;
		protected xModelSubType modelSubType = xModelSubType.Unknown;
		public static List<ixMember> AllMembers = new List<ixMember>();

		// Dummy Constructor, required for derived classes
		// DO NOT USE IT, use the real one above
		internal xMemberBase() { }

		// Properties, Methods, Functions...
		public string Name
		{
			get { return myName; }
		}

		public void ChangeName(string newName)
		{
			myName = newName;
		}

		public int CompareTo(ixMember otherObj)
		{
			return Name.CompareTo(otherObj.Name);
		}

		public Color Color
		{
			get { return myColor; }
			set { myColor = value; }
		}

		public string ToString()
		{
			return myName;
		}

		public virtual xMemberType MemberBaseType
		{
			get { return xMemberType.Base; }
		}

		public virtual xModelSubType MemberSubType
		{
			get { return modelSubType; }
			set { modelSubType = value; }
		}

		public CheckState SelectedState
		{
			get { return isSelected; }
			set { isSelected = value; }
		}

		public bool ExactMatch
		{
			get { return matchExact; }
			set { matchExact = value; }
		}

		public object Tag
		{
			get { return tagItem; }
			set { tagItem = value; }
		}

		public int UniverseNumber
		{
			get { return universeNumber; }
			set { universeNumber = value; }
		}

		public int DMXAddress
		{
			get { return dmxAddress; }
			set { dmxAddress = value; }
		}

		public int xLightsAddress
		{
			get { return xaddress; }
			set { xaddress = value; }
		}

		public string StartChannel
		{
			get { return startChannel; }
			set { startChannel = value; }
		}
	}

	public class xModel : xMemberBase, ixMember, IComparable<ixMember>
	// AC Channels
	{
		//public static List<xModel> AllModels = new List<xModel>();

		// Constructor, name is required
		public xModel(string theName)
		{
			myName = theName;
		}
		public xModel(string modelName, xModelSubType subType, int xAddress)
		{
			myName = modelName;
			modelSubType = subType;
		}

		// Properties, Methods, Functions...
		public override xMemberType MemberBaseType
		{
			get { return xMemberType.Model; }
		}
	}


	/// <summary>
	/// Model: StringType=3 Channel RGB
	/// </summary>
	public class xRGBModel : xMemberBase, ixMember, IComparable<ixMember>
	{
		public static List<xRGBModel> AllRGBModels = new List<xRGBModel>();
		public xRGBModel(string theName)
		{
			myName = theName;
		}
		public xRGBModel(string modelName, xModelSubType subType, int xAddress)
		{
			myName = modelName;
			modelSubType = subType;
		}

		public override xMemberType MemberBaseType
		{
			get { return xMemberType.RGBmodel; }
		}
	}

	/// <summary>
	/// Model: StringType = RGB Nodes
	/// </summary>
	public class xPixelModel : xMemberBase, ixMember, IComparable<ixMember>
	{
		public static List<xPixelModel> AllPixels = new List<xPixelModel>();
		public xPixelModel(string theName)
		{
			myName = theName;
		}
		public xPixelModel(string modelName, xModelSubType subType, int xAddress)
		{
			myName = modelName;
			modelSubType = subType;
		}


		public override xMemberType MemberBaseType
		{
			get { return xMemberType.Pixels; }
		}
	}


	public class xModelGroup : xMemberBase, ixMember, IComparable<ixMember>
	{
		public static List<xModelGroup> AllModelGroups = new List<xModelGroup>();

		public List<ixMember> Members = new List<ixMember>();

		public xModelGroup(string theName)
		{
			myName = theName;
		}
		public xModelGroup(string modelName, xModelSubType subType, int xAddress)
		{
			myName = modelName;
			modelSubType = subType;
		}

		public override xMemberType MemberBaseType
		{
			get { return xMemberType.ModelGroup; }
		}
	}

} // End namespace xAdmin
