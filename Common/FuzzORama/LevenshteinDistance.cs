using System;

namespace FuzzORama
{
	public static partial class FuzzyFunctions
	{

		public static int LevenshteinDistance(this string source, string target)
		{
			// From WikiBooks https://en.wikibooks.org/wiki/Algorithm_Implementation/Strings/Levenshtein_distance

			if (String.IsNullOrEmpty(source))
			{
				if (String.IsNullOrEmpty(target)) return 0;
				return target.Length;
			}
			if (String.IsNullOrEmpty(target)) return source.Length;

			if (source.Length > target.Length)
			{
				var temp = target;
				target = source;
				source = temp;
			}

			var m = target.Length;
			var n = source.Length;
			var distance = new int[2, m + 1];
			// Initialize the distance 'matrix'
			for (var j = 1; j <= m; j++) distance[0, j] = j;

			var currentRow = 0;
			for (var i = 1; i <= n; ++i)
			{
				currentRow = i & 1;
				distance[currentRow, 0] = i;
				var previousRow = currentRow ^ 1;
				for (var j = 1; j <= m; j++)
				{
					var cost = (target[j - 1] == source[i - 1] ? 0 : 1);
					distance[currentRow, j] = Math.Min(Math.Min(
								distance[previousRow, j] + 1,
								distance[currentRow, j - 1] + 1),
								distance[previousRow, j - 1] + cost);
				}
			}
			return distance[currentRow, m];
		}


		public static int LevenshteinDistanceLowerBounds(string source, string target)
		{
			// If the two strings are the same length then the lower bound is zero.
			if (source.Length == target.Length) { return 0; }

			// If the two strings are different lengths then the lower bounds is the difference in length.
			else { return Math.Abs(source.Length - target.Length); }
		}



	} // end public partial class FuzzyFunctions
} // end namespace FuzzORama
