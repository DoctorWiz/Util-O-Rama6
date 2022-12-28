using System.Text;
//using SimMetrics.Net.API;

namespace FuzzORama
{
	public sealed class DummyStopTermHandler : ITermHandler
	{
		public void AddWord(string termToAdd)
		{
		}

		public bool IsWord(string termToTest)
		{
			return false;
		}

		public void RemoveWord(string termToRemove)
		{
		}

		public int NumberOfWords => 0;

		public string ShortDescriptionString => "DummyStopTermHandler";

		public StringBuilder WordsAsBuffer => new StringBuilder();
	} // end public partial class FuzzyFunctions
} // end namespace FuzzORama
