using System;

namespace FuzzORama
{
	public abstract class AbstractStringMetric : IStringMetric
	{

		public abstract double GetSimilarity(string source, string target);
		public abstract string GetSimilarityExplained(string source, string target);
		public long GetSimilarityTimingActual(string source, string target)
		{
			long num = (DateTime.Now.Ticks - 0x89f7ff5f7b58000L) / 0x2710L;
			GetSimilarity(source, target);
			long num2 = (DateTime.Now.Ticks - 0x89f7ff5f7b58000L) / 0x2710L;
			return num2 - num;
		}

		public abstract double GetSimilarityTimingEstimated(string source, string target);
		public abstract double GetUnnormalisedSimilarity(string source, string target);

	}
}

