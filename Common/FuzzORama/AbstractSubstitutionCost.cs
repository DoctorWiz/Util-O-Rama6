namespace FuzzORama
{
	public abstract class AbstractSubstitutionCost : ISubstitutionCost
	{
		public abstract double GetCost(string source, int sourceIndex, string target, int targetIndex);

		public abstract double MaxCost { get; }

		public abstract double MinCost { get; }

	}
}

