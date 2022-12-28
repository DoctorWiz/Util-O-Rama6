using System;

namespace FuzzORama
{
	public static partial class FuzzyFunctions
	{
		/// <summary>
		/// Computes the Damerau-Levenshtein Distance between two strings, represented as arrays of
		/// integers, where each integer represents the code point of a character in the source string.
		/// Includes an optional threshhold which can be used to indicate the maximum allowable distance.
		/// </summary>
		/// <param name="source">An array of the code points of the first string</param>
		/// <param name="target">An array of the code points of the second string</param>
		/// <param name="threshold">Maximum allowable distance</param>
		/// <returns>Int.MaxValue if threshhold exceeded; otherwise the Damerau-Leveshteim distance between the strings</returns>
		/// 

		// Written by Joshua Honig (?) Copied from StackOverflow https://stackoverflow.com/questions/9453731/how-to-calculate-distance-similarity-measure-of-given-2-strings

		public static int DamerauLevenshteinDistance(string source, string target)
		{
			//int thr = (source.Length + target.Length) / 6;
			int thr = Math.Max(source.Length, target.Length);
			return DamerauLevenshteinDistance(source, target, thr);
		}

		public static double DamerauLevenshteinSimilarity(this string source, string target)
		{
			int distance = DamerauLevenshteinDistance(source, target);
			return SimilarityFromDistance(distance, source.Length, target.Length);
		}

		public static int DamerauLevenshteinDistance(string source, string target, int threshold)
		{

			int length1 = source.Length;
			int length2 = target.Length;

			// Return trivial case - difference in string lengths exceeds threshhold
			if (Math.Abs(length1 - length2) > threshold) { return int.MaxValue; }

			// Ensure arrays [i] / length1 use shorter length 
			if (length1 > length2)
			{
				Swap(ref target, ref source);
				Swap(ref length1, ref length2);
			}

			int maxi = length1;
			int maxj = length2;

			int[] dCurrent = new int[maxi + 1];
			int[] dMinus1 = new int[maxi + 1];
			int[] dMinus2 = new int[maxi + 1];
			int[] dSwap;

			for (int i = 0; i <= maxi; i++) { dCurrent[i] = i; }

			int jm1 = 0, im1 = 0, im2 = -1;

			for (int j = 1; j <= maxj; j++)
			{

				// Rotate
				dSwap = dMinus2;
				dMinus2 = dMinus1;
				dMinus1 = dCurrent;
				dCurrent = dSwap;

				// Initialize
				int minDistance = int.MaxValue;
				dCurrent[0] = j;
				im1 = 0;
				im2 = -1;

				for (int i = 1; i <= maxi; i++)
				{

					int cost = source[im1] == target[jm1] ? 0 : 1;

					int del = dCurrent[im1] + 1;
					int ins = dMinus1[i] + 1;
					int sub = dMinus1[im1] + cost;

					//Fastest execution for min value of 3 integers
					int min = (del > ins) ? (ins > sub ? sub : ins) : (del > sub ? sub : del);

					if (i > 1 && j > 1 && source[im2] == target[jm1] && source[im1] == target[j - 2])
						min = Math.Min(min, dMinus2[im2] + cost);

					dCurrent[i] = min;
					if (min < minDistance) { minDistance = min; }
					im1++;
					im2++;
				}
				jm1++;
				if (minDistance > threshold) { return int.MaxValue; }
			}

			int result = dCurrent[maxi];
			return (result > threshold) ? int.MaxValue : result;
		}

		static void Swap<T>(ref T arg1, ref T arg2)
		{
			T temp = arg1;
			arg1 = arg2;
			arg2 = temp;
		}

		public static double YetiLevenshteinSimilarity(this string source, string target)
		{
			double score = YetiLevenshteinDistance(source, target);
			return SimilarityFromDistance(score, source.Length, target.Length);
		}

		public static unsafe int YetiLevenshteinDistance(string source, string target)
		{
			fixed (char* p1 = source)
			fixed (char* p2 = target)
			{
				return YetiLevenshteinDistance(p1, source.Length, p2, target.Length, 0); // substitutionCost = 1
			}
		}
		/// <summary>
		/// Cetin Sert, David Necas
		/// http://webcleaner.svn.sourceforge.net/viewvc/webcleaner/trunk/webcleaner2/wc/levenshtein.c?revision=6015&view=markup
		/// </summary>
		/// <param name="source"></param>
		/// <param name="l1"></param>
		/// <param name="target"></param>
		/// <param name="l2"></param>
		/// <param name="xcost"></param>
		/// <returns></returns>
		public static unsafe int YetiLevenshteinDistance(char* source, int l1, char* target, int l2, int xcost)
		{
			int i;
			//int *row;  /* we only need to keep one row of costs */
			int* end;
			int half;

			/* strip common prefix */
			while (l1 > 0 && l2 > 0 && *source == *target)
			{
				l1--;
				l2--;
				source++;
				target++;
			}

			/* strip common suffix */
			while (l1 > 0 && l2 > 0 && source[l1 - 1] == target[l2 - 1])
			{
				l1--;
				l2--;
			}

			/* catch trivial cases */
			if (l1 == 0)
				return l2;
			if (l2 == 0)
				return l1;

			/* make the inner cycle (i.e. string2) the longer one */
			if (l1 > l2)
			{
				int nx = l1;
				char* sx = source;
				l1 = l2;
				l2 = nx;
				source = target;
				target = sx;
			}

			//check len1 == 1 separately
			if (l1 == 1)
			{
				//throw new NotImplementedException();
				if (xcost > 0)
					//return l2 + 1 - 2*(memchr(target, *source, l2) != NULL);
					return l2 + 1 - 2 * memchrRPLC(target, *source, l2);
				else
					//return l2 - (memchr(target, *source, l2) != NULL);
					return l2 - memchrRPLC(target, *source, l2);
			}

			l1++;
			l2++;
			half = l1 >> 1;

			/* initalize first row */
			//row = (int*)malloc(l2*sizeof(int));
			int* row = stackalloc int[l2];
			if (l2 < 0)
				//if (!row)
				return (int)(-1);
			end = row + l2 - 1;
			for (i = 0; i < l2 - (xcost > 0 ? 0 : half); i++)
				row[i] = i;

			/* go through the matrix and compute the costs.  yes, this is an extremely
			 * obfuscated version, but also extremely memory-conservative and
			 * relatively fast.
			 */
			if (xcost > 0)
			{
				for (i = 1; i < l1; i++)
				{
					int* p = row + 1;
					char char1 = source[i - 1];
					char* char2p = target;
					int D = i;
					int x = i;
					while (p <= end)
					{
						if (char1 == *(char2p++))
							x = --D;
						else
							x++;
						D = *p;
						D++;
						if (x > D)
							x = D;
						*(p++) = x;
					}
				}
			}
			else
			{
				/* in this case we don't have to scan two corner triangles (of size len1/2)
				 * in the matrix because no best path can go throught them. note this
				 * breaks when len1 == len2 == 2 so the memchr() special case above is
				 * necessary */
				row[0] = l1 - half - 1;
				for (i = 1; i < l1; i++)
				{
					int* p;
					char char1 = source[i - 1];
					char* char2p;
					int D, x;
					/* skip the upper triangle */
					if (i >= l1 - half)
					{
						int offset = i - (l1 - half);
						int c3;

						char2p = target + offset;
						p = row + offset;
						c3 = *(p++) + ((char1 != *(char2p++)) ? 1 : 0);
						x = *p;
						x++;
						D = x;
						if (x > c3)
							x = c3;
						*(p++) = x;
					}
					else
					{
						p = row + 1;
						char2p = target;
						D = x = i;
					}
					/* skip the lower triangle */
					if (i <= half + 1)
						end = row + l2 + i - half - 2;
					/* main */
					while (p <= end)
					{
						int c3 = --D + ((char1 != *(char2p++)) ? 1 : 0);
						x++;
						if (x > c3)
							x = c3;
						D = *p;
						D++;
						if (x > D)
							x = D;
						*(p++) = x;
					}
					/* lower triangle sentinel */
					if (i <= half)
					{
						int c3 = --D + ((char1 != *char2p) ? 1 : 0);
						x++;
						if (x > c3)
							x = c3;
						*p = x;
					}
				}
			}

			i = *end;
			return i;
		}

		private static unsafe int memchrRPLC(char* buffer, char c, int count)
		{
			char* p = buffer;
			char* e = buffer + count;
			while (p++ < e)
			{
				if (*p == c)
					return 1;
			}
			return 0;
		}
	} // end public partial class FuzzyFunctions
} // end namespace FuzzORama
