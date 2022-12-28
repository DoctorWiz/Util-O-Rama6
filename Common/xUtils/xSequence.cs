using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace xLights22
{

	public class xSequence
	{
		public xRGBEffects RGBEffects = null;
		public List<xTimings> TimingTracks = new List<xTimings>();
		public int Length = 0;
		public string Filename = "";
		public string MediaFile = "";
		public int FPS = 40;

		public xSequence(string fileName)
		{
			Load(fileName);
		}

		public int Load(string theFilename)
		{
			int err = 0;
			if (theFilename.Length > 2)
			{
				Filename = theFilename;
			}

			return err;
		}

		public int Save(string newFilename = "")
		{
			int err = 0;
			if (newFilename.Length > 2)
			{
				Filename = newFilename;
			}

			return err;
		}

		public int ExportTimings(string fileName)
		{
			// Note: This exports ALL timing tracks
			// (To export an individual timing track, use it's Export function)
			int count = 0; // Returns number of tracks exported


			for (int t = 0; t < TimingTracks.Count; t++)
			{


			}


			return count;
		}

		public int ImportTimings(string fileName)
		{
			// Note: If Timing(s) file has more than one timing track, they will ALL be imported
			// timing tracks with duplicate names will be overwritten!
			// (To import just one timing track from a multi-track file, create a new timing track
			//   and use it's Import function and specify which one you want by name or number)
			int count = 0; // returns number of tracks imported


			return count;
		}




	}
}