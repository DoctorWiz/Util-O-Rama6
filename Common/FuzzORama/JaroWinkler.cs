using System;
//using SimMetrics.Net.API;
//using SimMetrics.Net.Utilities;

namespace FuzzORama
{
	public partial class FuzzyFunctions // JaroWinkler : AbstractStringMetric
	{
		//private readonly AbstractStringMetric _jaroStringMetric = new Jaro();
		private const int MinPrefixTestLength = 4;
		private const double PrefixAdustmentScale = 0.10000000149011612;

		private static int GetPrefixLength(string source, string target)
		{
			if (source == null || target == null)
			{
				return MinPrefixTestLength;
			}

			int num = MathFunctions.MinOf3(MinPrefixTestLength, source.Length, target.Length);
			for (int i = 0; i < num; i++)
			{
				if (source[i] != target[i])
				{
					return i;
				}
			}
			return num;
		}

		public static double JaroWinklerSimilarity(this string source, string target)
		{
			if (source != null && target != null)
			{
				//double similarity = _jaroStringMetric.GetSimilarity(source, target);
				double similarity = source.JaroSimilarity(target);
				int prefixLength = GetPrefixLength(source, target);
				return similarity + prefixLength * PrefixAdustmentScale * (1.0 - similarity);
			}
			return DefaultMismatchScore;
		}


	} // end public partial class FuzzyFunctions
} // end namespace FuzzORama
