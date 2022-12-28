namespace FuzzORama
{
	public interface IStringMetric
	{
		double GetSimilarity(string source, string target);
		string GetSimilarityExplained(string source, string target);
		long GetSimilarityTimingActual(string source, string target);
		double GetSimilarityTimingEstimated(string source, string target);
		double GetUnnormalisedSimilarity(string source, string target);
	}
}

