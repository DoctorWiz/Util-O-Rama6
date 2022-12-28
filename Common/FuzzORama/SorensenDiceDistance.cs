using System;
using System.Linq;
using System.Collections.Generic;

namespace FuzzORama
{
	public static partial class FuzzyFunctions
	{

		public static double SorensenDiceSimilarity(this string source, string target)
		{
			HashSet<string> setA = new HashSet<string>();
			HashSet<string> setB = new HashSet<string>();

			for (int i = 0; i < source.Length - 1; ++i)
				setA.Add(source.Substring(i, 2));

			for (int i = 0; i < target.Length - 1; ++i)
				setB.Add(target.Substring(i, 2));

			HashSet<string> intersection = new HashSet<string>(setA);
			intersection.IntersectWith(setB);

			return (2.0 * intersection.Count) / (setA.Count + setB.Count);
		}
	}

}