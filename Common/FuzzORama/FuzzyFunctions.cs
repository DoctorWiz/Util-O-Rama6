using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace FuzzORama
{

	public static partial class FuzzyFunctions
	{
		//private const long USE_CASEINSENSITIVE = 0x8000000;

		//! Very Important: First algorith should start with 1
		//   each subsequent algoritm should shift 1 bit to the left
		//    no gaps
		private const long USE_JAROWINKLER = 0x0001; //     1
		private const long USE_DAMERAULEVENSHTEIN = 0x0002; //    2
		private const long USE_YETILEVENSHTEIN = 0x0004; //    4
		private const long USE_LONGESTCOMMONSUBSTRING = 0x0008; //   8
		private const long USE_SIFT = 0X0010; // 16
		private const long USE_SORENSENDICE = 0x0020; // 32
		private const int ALGORITHM_COUNT = 6;
		private const long USE_ALLSIMILARITIES = USE_JAROWINKLER | // middle speed, good accuracy
																						USE_DAMERAULEVENSHTEIN | // little slower than normalized but even more accurate
																						USE_YETILEVENSHTEIN | // The King!!  Fastest and very accurate!
																						USE_LONGESTCOMMONSUBSTRING |  // Accurate but one of the slower ones
																						USE_SIFT | // Fast and Accurate
																						USE_SORENSENDICE; // One of the fastest, good accuracy

		// Not only the fastest, but also one of the most accurate
		private const long USE_SUGGESTED_PREMATCH = USE_YETILEVENSHTEIN;

		// The best and most accurate 5 algorithms, each a little different to give a good rounded score
		private const long USE_SUGGESTED_FINALMATCH = USE_JAROWINKLER |
																									USE_DAMERAULEVENSHTEIN |
																									USE_LONGESTCOMMONSUBSTRING |
																									USE_SIFT |
																									USE_SORENSENDICE;

		public const double SUGGESTED_MIN_PREMATCH_SCORE = 0.40D;
		public const double SUGGESTED_MIN_FINAL_SCORE = 0.55D;


		public static double FuzzyScoreFast(this string source, string target, bool caseInsenstiive = false)
		{
			// This uses just Yeti-Levenshtein.  Reasonably accurate and fast.
			// Suggested use is to run this score first as a pre-match to eliminate strings which aren't even close.
			// Then use the Accurate comparison below on the remaining list.
			if (caseInsenstiive)
			{
				source = source.ToLower();
				target = target.ToLower();
			}
			return source.YetiLevenshteinSimilarity(target);
		}

		public static double FuzzyScoreAccurate(this string source, string target, bool caseInsenstiive = false)
		{
			// This uses an average of Jaro-Winkler, Damerau-Levenshtein, Longest Common Substring, Sift, and Sorensen-Dice
			// These take longer but are more accurate, and by using the average from 5 of them that makes it take even
			// longer but be even more accurate.
			int methodCount = 0;
			double runningTotal = 0D;
			double ret = 0D;
			List<double> comparisonResults = new List<double>();
			double minScore = 0.4D;
			double maxScore = 0.99D;
			//double thisScore = 0.5D;
			//double WLscore = 0.8D;
			bool valid = false;

			if (caseInsenstiive)
			{
				source = source.ToLower();
				target = target.ToLower();
			}

			double YLScore = source.YetiLevenshteinSimilarity(target);
			valid = AddIfValid(YLScore, 0.41D, 1.0D, ref runningTotal, ref methodCount);
			double JWScore = source.JaroWinklerSimilarity(target);
			valid = AddIfValid(JWScore, 0.40D, 1.0D, ref runningTotal, ref methodCount);
			double DLScore = source.DamerauLevenshteinSimilarity(target);
			valid = AddIfValid(DLScore, 0.41D, 1.0D, ref runningTotal, ref methodCount);
			double SSScore = source.SiftSimilarity(target);
			valid = AddIfValid(SSScore, 0.58D, 1.0D, ref runningTotal, ref methodCount);
			double SDScore = source.SorensenDiceSimilarity(target);
			valid = AddIfValid(SDScore, 0.66D, 1.0D, ref runningTotal, ref methodCount);

			// Use Yeti-Levenshtein to set a baseline
			//  for what is an acceptable score from other algorithms
			//WLscore = source.YetiLevenshteinSimilarity(target);
			maxScore = YLScore + (1.0D - YLScore) / 2.0D;
			minScore = YLScore / 3.0D;
			minScore = Math.Max(minScore, 0.4D);

			double LCScore = source.LongestCommonSubstringSimilarity(target);
			valid = AddIfValid(LCScore, minScore, maxScore, ref runningTotal, ref methodCount);


			//string nfo = source + " <=> " + target;
			//nfo += "\r\nYeti:" + YLScore.ToString("0.000");
			//nfo += "\r\nJaro:" + JWScore.ToString("0.000");
			//nfo += "\r\nDame:" + DLScore.ToString("0.000");
			//nfo += "\r\nSift:" + SSScore.ToString("0.000");
			//nfo += "\r\nDice:" + SDScore.ToString("0.000");
			//nfo += "\r\nLong:" + LCScore.ToString("0.000");


			if (methodCount > 0)
			{
				ret = runningTotal / methodCount;
			}

			//nfo += "\r\n*AVG:" + ret.ToString("0.000");
			//Debug.WriteLine(nfo);
			//Console.WriteLine(nfo);


			return ret;
		}

		//TODO: Have to figure out how to pass in a list of an unknown type...
		/*
		public static int BestMatchInList(this List<> list, string target, bool caseInsenstiive = false)
		{
			int idx = -1;
			double highScore = 0D;
			for (int i = 1; i < list.Count; i++)
			{
				//string itemName = list[i].Name;
				string itemName = list[i].ToString();
				double preScore = itemName.FuzzyScoreFast(target);
				// if the score is above the minimum PreMatch
				if (preScore > FuzzyFunctions.SUGGESTED_MIN_PREMATCH_SCORE)
				{
					double finalScore = itemName.FuzzyScoreAccurate(target);
					if (finalScore > FuzzyFunctions.SUGGESTED_MIN_FINAL_SCORE)
					{
						if (finalScore > highScore)
						{
							highScore = finalScore;
						idx = i;
						} // > high score so far
					} // >= min final match
				} // >= min prematch
			} // end loop thru VizItemGroups
			return idx;
		}
		*/

		/*

		private static string AlgorithmNames(long algorithms)
		{
			string ret = "";

			if ((algorithms & USE_JAROWINKLER) > 0)
			{
				ret += "Jaro-Winkler, ";
			}
			if ((algorithms & USE_DAMERAULEVENSHTEIN) > 0)
			{
				ret += "Damerau-Levenshtein, ";
			}
			if ((algorithms & USE_YETILEVENSHTEIN) > 0)
			{
				ret += "Yeti-Levenshtein, ";
			}
			if ((algorithms & USE_LONGESTCOMMONSUBSTRING) > 0)
			{
				ret += "Longest Common Substring, ";
			}
			if ((algorithms & USE_SIFT) > 0)
			{
				ret += "Sift Similarity, ";
			}
			if ((algorithms & USE_SORENSENDICE) > 0)
			{
				ret += "Sorensen-Dice, ";
			}
			if ((algorithms & USE_CASEINSENSITIVE) > 0)
			{
				ret += "and Case Insensitive, ";
			}

			if (ret.Length > 5)
			{
				ret = ret.Substring(0, ret.Length - 2);
			}




			return ret;
		}


		private static double RankEquality(this string source, string target)
		{
			return source.RankEquality(target, USE_ALLSIMILARITIES);
		}


		private static double RankEquality(this string source, string target, long FuzzORamaComparisonOptions)
		{
			int methodCount = 0;
			double runningTotal = 0D;
			double ret = 0D;
			List<double> comparisonResults = new List<double>();
			double minScore = 0.4D;
			double maxScore = 0.99D;
			double thisScore = 0.5D;
			double WLscore = 0.8D;
			bool valid = false;

			if ((FuzzORamaComparisonOptions & USE_CASEINSENSITIVE) > 0)
			{
				source = source.ToUpper();
				target = target.ToUpper();
			}

			//string foo = "Fuzzy " + source + " vs. " + target;
			//Console.WriteLine(foo);
			//if (target.IndexOf("llow Snow") > 0)
			//{
			//	foo += "STOP!";
			//}


			// Now perform all other requested algorithms
			if ((FuzzORamaComparisonOptions & USE_JAROWINKLER) > 0)
			{
				thisScore = source.JaroWinklerSimilarity(target);
				//valid = AddIfValid(thisScore, minScore, maxScore, ref runningTotal, ref methodCount);
				valid = AddIfValid(thisScore, 0.40D, 1.0D, ref runningTotal, ref methodCount);
			}
			if ((FuzzORamaComparisonOptions & USE_DAMERAULEVENSHTEIN) > 0)
			{
				thisScore = source.DamerauLevenshteinSimilarity(target);
				//valid = AddIfValid(thisScore, minScore, maxScore, ref runningTotal, ref methodCount);
				valid = AddIfValid(thisScore, 0.41D, 1.0D, ref runningTotal, ref methodCount);
			}
			if ((FuzzORamaComparisonOptions & USE_YETILEVENSHTEIN) > 0)
			{
				thisScore = source.YetiLevenshteinSimilarity(target);
				//valid = AddIfValid(thisScore, minScore, maxScore, ref runningTotal, ref methodCount);
				valid = AddIfValid(thisScore, 0.41D, 1.0D, ref runningTotal, ref methodCount);
			}
			if ((FuzzORamaComparisonOptions & USE_SIFT) > 0)
			{
				thisScore = source.SiftSimilarity(target);
				//valid = AddIfValid(thisScore, minScore, maxScore, ref runningTotal, ref methodCount);
				valid = AddIfValid(thisScore, 0.58D, 1.0D, ref runningTotal, ref methodCount);
			}
			if ((FuzzORamaComparisonOptions & USE_SORENSENDICE) > 0)
			{
				thisScore = source.SorensenDiceSimilarity(target);
				//valid = AddIfValid(thisScore, minScore, maxScore, ref runningTotal, ref methodCount);
				valid = AddIfValid(thisScore, 0.66D, 1.0D, ref runningTotal, ref methodCount);
			}

			if ((FuzzORamaComparisonOptions & USE_LONGESTCOMMONSUBSTRING) > 0)
			{
				// Use Yeti-Levenshtein to set a baseline
				//  for what is an acceptable score from other algorithms
				WLscore = source.YetiLevenshteinSimilarity(target);
				maxScore = WLscore + (1.0D - thisScore) / 2.0D;
				minScore = WLscore / 3.0D;
				minScore = Math.Max(minScore, 0.4D);

				if ((FuzzORamaComparisonOptions & USE_LONGESTCOMMONSUBSTRING) > 0)
				{
					thisScore = source.LongestCommonSubstringSimilarity(target);
					valid = AddIfValid(WLscore, minScore, maxScore, ref runningTotal, ref methodCount);
					//valid = AddIfValid(thisScore, 0.80D, 1.0D, ref runningTotal, ref methodCount);
				}
			}







			if (methodCount > 0)
			{
				ret = runningTotal / methodCount;
			}

			return ret;
		}
		*/
		private static bool AddIfValid(double thisScore, double minScore, double maxScore, ref double runningTotal, ref int methodCount)
		{
			bool valid = false;
			if ((thisScore >= minScore) && (thisScore <= maxScore)) valid = true;
			if (valid)
			{
				runningTotal += thisScore;
				methodCount++;
			}
			return valid;
		}




		private static double SimilarityFromDistance(this double distanceScore, int sourceLength, int targetLength)
		{
			double ret = 0;
			int mn = Math.Min(sourceLength, targetLength);
			if (distanceScore <= 0)
			{
				ret = 1;
			}
			else
			{
				if (mn > 0)
				{
					if (distanceScore < mn)
					{
						ret = 1 - (distanceScore / mn);
					}
				}
			}
			return ret;
		} // end Fuzzy Index Distance



	} // end partial class FuzzyFunctions
} //end namespace FuzzORama
