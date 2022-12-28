using FuzzORama;
using System.Text;

namespace FuzzyTest
{
	public partial class frmFuzzyTest : Form
	{
		private string[] List1 = {"Big Stars","Tomato Tree A1","Red Roof Outline","Mega Tree 1","Mega Tree 2","Tomato Tree #B5",
															"Super Spinner","Wheel of Dharma","Nativity Flood","Little Stars","Blue Roof Outline","Spiral Trees Left",
															"Spiral Tree Right","Sidewalk Flood","Laser Projector","Monster Spinner","Top Snowflakes","Middle Snowflakes",
															"Super Silly Santa","Plywood Cutouts","Flying Spaghetti Monster","Neon Peace Sign","Red Rope Light","Blue Rope Lights" };

		private string[] List2 = {"Big stars","Tomato Tree A-1","Red-Roof Outline","Mega Tree 01","Mega Tree 2.","Tomato Tree B5",
															"Super-Spinner","Wheel O' Dharma","Nativity Floods","Little Starz","Blue Roof Outlines","Spiral Trees on the Left",
															"Spiral Tree Far Right","Sidewalk Floodlight","Laser Projector!!","Monstor Spinner","Topp Snowflakes","Middle snwflak",
															"SuperSillySanta","Wood Cutouts","Spaghetti Flying Monster","Neon Peez Sign","Redish Rope Light","Blue-Rope light 2" };

		StringBuilder logText = new StringBuilder();


		public frmFuzzyTest()
		{
			InitializeComponent();
			RunTest();
		}

		public string LogText
		{
			get
			{
				return txtOutput.Text;
			}
			set
			{
				txtOutput.Text = value;
				txtOutput.SelectionStart = txtOutput.TextLength;
				txtOutput.ScrollToCaret();
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			RunTest();
		}

		private void RunTest()
		{
			for (int l1 = 0; l1 < List1.Length; l1++)
			{
				string check = List1[l1];
				string result = FuzzyMatch(check);
			}
			txtOutput.Text = logText.ToString();
		}

		private string FuzzyMatch(string source)
		{
			string target = "";
			double[] preScores = null;
			Array.Resize(ref preScores, List1.Length);

			int bestIndex = -1;
			double bestScore = 0D;
			int preMatches = 0;
			bestScore = 0D;
			bestIndex = -1;
			for (int l2 = 0; l2 < List2.Length; l2++)
			{
				preScores[l2] = source.FuzzyScoreFast(List2[l2]);
				if (preScores[l2] > FuzzyFunctions.SUGGESTED_MIN_PREMATCH_SCORE)
				{
					preMatches++;
					double postScore = source.FuzzyScoreAccurate(List2[l2]);
					string nfo = source + " vs. " + List2[l2] + " = " + postScore.ToString("0.000");
					//logText.AppendLine(nfo);

					if (postScore > bestScore)
					{
						bestScore = postScore;
						bestIndex = l2;
					}
				}
			}
			string info = source;
			if (bestIndex >= 0)
			{
				target = List2[bestIndex];
				info += " Best match is '" + target;
				info += "' with score of " + bestScore.ToString("0.000");
				info += " out of " + preMatches.ToString() + " prematches.";
			}
			else
			{
				info += " Failed to find a final match out of " + preMatches.ToString() + " prematches.";
			}
			logText.AppendLine(info);
			return target;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}