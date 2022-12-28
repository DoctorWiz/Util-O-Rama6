namespace FuzzORama
{
	public interface ISubstitutionCost
	{
		double GetCost(string source, int sourceIndex, string target, int targetIndex);

		double MaxCost { get; }

		double MinCost { get; }
	}
}

