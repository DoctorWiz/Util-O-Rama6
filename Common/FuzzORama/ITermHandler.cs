using System.Text;

namespace FuzzORama
{
	public interface ITermHandler
	{
		void AddWord(string termToAdd);
		bool IsWord(string termToTest);
		void RemoveWord(string termToRemove);

		int NumberOfWords { get; }

		string ShortDescriptionString { get; }

		StringBuilder WordsAsBuffer { get; }
	}
}

