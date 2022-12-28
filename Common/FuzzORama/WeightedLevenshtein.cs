/*
 * The MIT License
 *
 * Copyright 2016 feature[23]
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
//using F23.StringSimilarity.Interfaces;
// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable TooWideLocalVariableScope

namespace FuzzORama
{
	/// Implementation of Levenshtein that allows to define different weights for
	/// different character substitutions.
	//public class WeightedLevenshtein : IStringDistance
	//{

	/// <summary>
	/// Create a new instance with provided character substitution.
	/// </summary>
	/// <param name="characterSubstitution">The strategy to determine character substitution weights.</param>
	//public WeightedLevenshtein(ICharacterSubstitution characterSubstitution)
	//{
	//    _characterSubstitution = characterSubstitution;
	//}

	/// <summary>
	/// Compute Levenshtein distance using provided weights for substitution.
	/// </summary>
	/// <param name="source">The first string to compare.</param>
	/// <param name="target">The second string to compare.</param>
	/// <returns>The computed weighted Levenshtein distance.</returns>
	/// <exception cref="ArgumentNullException">If source or target is null.</exception>
	/// 


	public partial class FuzzyFunctions
	{

		public static double WeightedLevenshteinSimilarity(this string source, string target)
		{
			double score = WeightedLevenshteinDistance(source, target);
			return SimilarityFromDistance(score, source.Length, target.Length);
		}


		public static double WeightedLevenshteinDistance(string source, string target)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (target == null)
			{
				throw new ArgumentNullException(nameof(target));
			}

			if (source.Equals(target))
			{
				return 0;
			}

			if (source.Length == 0)
			{
				return target.Length;
			}

			if (target.Length == 0)
			{
				return source.Length;
			}

			// create two work vectors of integer distances
			double[] v0 = new double[target.Length + 1];
			double[] v1 = new double[target.Length + 1];
			double[] vtemp;

			// initialize v0 (the previous row of distances)
			// this row is A[0][i]: edit distance for an empty s
			// the distance is just the number of characters to delete from t
			for (int i = 0; i < v0.Length; i++)
			{
				v0[i] = i;
			}

			for (int i = 0; i < source.Length; i++)
			{
				// calculate v1 (current row distances) from the previous row v0
				// first element of v1 is A[i+1][0]
				//   edit distance is delete (i+1) chars from s to match empty t
				v1[0] = i + 1;

				// use formula to fill in the rest of the row
				for (int j = 0; j < target.Length; j++)
				{
					double cost = 0;
					if (source[i] != target[j])
					{
						cost = DetermineCost(source[i], target[j]);
					}
					v1[j + 1] = Math.Min(
						v1[j] + 1, // Cost of insertion
								Math.Min(
									v0[j + 1] + 1, // Cost of remove
										v0[j] + cost)); // Cost of substitution
				}

				// copy v1 (current row) to v0 (previous row) for next iteration
				//System.arraycopy(v1, 0, v0, 0, v0.length);
				// Flip references to current and previous row
				vtemp = v0;
				v0 = v1;
				v1 = vtemp;

			}

			return v0[target.Length];
		}

		public static double DetermineCost(char source, char target)
		{
			double d1 = DetermineOneCost(source, target);
			double d2 = DetermineOneCost(target, source);
			if (d2 < d1) d1 = d2;
			return d1;
		}


		private static double DetermineOneCost(char source, char target)
		{
			double theCost = 1.0D;  // Default if it meets none of the below condtions
															// Exact match?
			if (source == target) theCost = 0.0D;

			// Characters which are similar looking or have similar uses

			// Different Case
			else if ((source > 65) && (source < 95))
			{
				if ((source + 32) == target)
				{
					theCost = 0.5D;
				}
				// Vowels
				else if (source == 'a')
				{
					if (target == 'e') theCost = 0.7D;
					else if (target == 'i') theCost = 0.7D;
					else if (target == 'o') theCost = 0.7D;
					else if (target == 'u') theCost = 0.7D;
					else if (target == '@') theCost = 0.8D;
				}
				else if (source == 'e')
				{
					if (target == 'i') theCost = 0.7D;
					else if (target == 'o') theCost = 0.7D;
					else if (target == 'u') theCost = 0.7D;
				}
				else if (source == 'i')
				{
					if (target == 'o') theCost = 0.7D;
					else if (target == 'u') theCost = 0.7D;
				}
				else if (source == 'o')
				{
					if (target == 'u') theCost = 0.7D;
					else if (target == '0') theCost = 0.8D;
				}

				// Common spelling mistakes
				else if ((source == 'd') && (target == 't')) theCost = 0.8D;
				else if ((source == 'j') && (target == 'g')) theCost = 0.8D;
				else if (source == 'l')
				{
					if (target == 'r') theCost = 0.8D;
					else if (target == '1') theCost = 0.7D;
				}
				else if ((source == 'm') && (target == 'n')) theCost = 0.8D;
				else if (source == 'b')
				{
					if (target == 'p') theCost = 0.8D;
					else if (target == 'v') theCost = 0.8D;
				}
				else if (source == 'x')
				{
					if (target == 'y') theCost = 0.9D;
					else if (target == '*') theCost = 0.6D;
				}
				else if ((source == 'v') && (target == '^')) theCost = 0.9D;

			} // End source is a lower case alpha character

			// Different Numbers
			//! Note: In my case, I thought it was best to depreciate mismatched numbers in favor of alpha characters because I'm dealing with names
			//! your milage may vary depending on your data
			else if ((source >= 48) && (source <= 57))
			{
				if ((target >= 48) && (target <= 57))
				{
					theCost = 0.7D;
				}
			} // end source is a number

			// upper case letters
			else if (source == 'X')
			{
				if (target == '*') theCost = 0.6D;
				else if (target == 'Y') theCost = 0.9D;
			}
			else if ((source == 'O') && (target == '0')) theCost = 0.7D;

			// Puctuation & Symbols
			else if (source == '.')
			{
				if (target == '?') theCost = 0.8D;
				else if (target == '!') theCost = 0.8D;
				else if (target == ' ') theCost = 0.6D;
				else if (target == ',') theCost = 0.7D;
				else if (target == '_') theCost = 0.8D;
				else if (target == '-') theCost = 0.8D;
			}
			else if (source == '(')
			{
				if (target == '{') theCost = 0.6D;
				else if (target == '[') theCost = 0.6D;
				else if (target == '<') theCost = 0.8D;
			}
			else if (source == '[')
			{
				if (target == '{') theCost = 0.6D;
				else if (target == '<') theCost = 0.8D;
			}
			else if ((source == '{') && (target == '<')) theCost = 0.8D;
			else if (source == ')')
			{
				if (target == '}') theCost = 0.6D;
				else if (target == ']') theCost = 0.6D;
				else if (target == '>') theCost = 0.8D;
			}
			else if (source == ']')
			{
				if (target == '}') theCost = 0.6D;
				else if (target == '>') theCost = 0.8D;
			}
			else if ((source == '}') && (target == '>')) theCost = 0.8D;
			else if ((source == '/') && (target == 134)) theCost = 0.8D;
			else if (source == '-')
			{
				if (target == '_') theCost = 0.8D;
				else if (target == '+') theCost = 0.9D;
			}
			else if ((source == ':') && (target == ';')) theCost = 0.6D;
			else if (source == 27)
			{
				if (target == '"') theCost = 0.8D;
				else if (target == '`') theCost = 0.8D;
			}
			else if (source == '!')
			{
				if (target == '|') theCost = 0.7D;
				else if (target == '1') theCost = 0.8D;
			}

			// OCR errors
			//if ((source == '0') && (target == 'O')) theCost = 0.7D;
			//if ((source == '2') && (target == 'z')) theCost = 0.9D;
			//if ((source == '2') && (target == 'Z')) theCost = 0.8D;
			//if ((source == '8') && (target == 'B')) theCost = 0.8D;
			//if ((source == '1') && (target == 'l')) theCost = 0.6D;
			//if ((source == 'p') && (target == 'q')) theCost = 0.8D;
			//if ((source == 'd') && (target == 'b')) theCost = 0.8D;







			//double otherCost = DetermineCost(target, source);
			//if (otherCost < theCost) theCost = otherCost;
			return theCost;
		} // end DetermineCost



	} // end partial class FuzzyFunctions
} // end namespace FuzzORama
