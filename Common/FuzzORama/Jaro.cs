using System;
using System.Text;
//using SimMetrics.Net.API;

namespace FuzzORama
{
	public partial class FuzzyFunctions // Jaro : AbstractStringMetric
	{
		public static double DefaultMismatchScore = 0.0D;

		private static StringBuilder GetCommonCharacters(this string source, string target, int distanceSep)
		{
			if (source == null || target == null)
			{
				return null;
			}
			StringBuilder builder = new StringBuilder();
			StringBuilder builder2 = new StringBuilder(target);
			for (int i = 0; i < source.Length; i++)
			{
				char ch = source[i];
				bool flag = false;
				for (int j = Math.Max(0, i - distanceSep); !flag && j < Math.Min(i + distanceSep, target.Length); j++)
				{
					if (builder2[j] == ch)
					{
						flag = true;
						builder.Append(ch);
						builder2[j] = '#';
					}
				}
			}
			return builder;
		}

		public static double JaroSimilarity(this string source, string target)
		{
			if (source == null || target == null)
			{
				return 0.0;
			}
			int distanceSep = Math.Min(source.Length, target.Length) / 2 + 1;
			StringBuilder builder = source.GetCommonCharacters(target, distanceSep);
			int length = builder.Length;
			if (length == 0)
			{
				return DefaultMismatchScore;
			}
			StringBuilder builder2 = target.GetCommonCharacters(source, distanceSep);
			if (length != builder2.Length)
			{
				return DefaultMismatchScore;
			}
			int num3 = 0;
			for (int i = 0; i < length; i++)
			{
				if (builder[i] != builder2[i])
				{
					num3++;
				}
			}
			num3 /= 2;
			return length / (3.0 * source.Length) + length / (3.0 * target.Length) + (length - num3) / (3.0 * length);
		}


	} // end public partial class FuzzyFunctions
} // end namespace FuzzORama
