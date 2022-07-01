using System;
using System.Collections.Generic;
using System.Linq;

public class AutomatesClasses
{
	private Dictionary<char, string> _firstAutomatePunctuation;
	private Dictionary<string, IEnumerable<string>> _secondAutomate;


	public AutomatesClasses()
	{
		_firstAutomatePunctuation = new Dictionary<char, string>//Так удобней
		{
			{ ':', "двоеточие" },
			{';', "тчкзпт" },
			{'[', "обратная квадратная скобочка" },
			{']', "внутренняя квадратная скобочка" },
			{',', "запятая" },
			{'.', "точка" },
			{'+', "знак" },
			{'-', "знак" }
		};

		_secondAutomate = new Dictionary<string, IEnumerable<string>>
		{
			{"КЛСЛОВО", new SortedSet<string>{ "var","array", "of"} },
			{"Стандартный тип", new SortedSet<string>{ "boolean", "byte", "char",  "integer", "longint", "real", "string", "word" } }
		};
	}

	public CharIdentifier CheckFirstAutomate(char symbol)
    {
		if (char.IsDigit(symbol))
			return new CharIdentifier(symbol, "цифра");

		if (char.IsLetter(symbol))
			return new CharIdentifier(symbol, "буква");

		if (char.IsWhiteSpace(symbol))
			return new CharIdentifier(symbol, "пробел");

        try
        {
			return new CharIdentifier(symbol, _firstAutomatePunctuation[symbol]);
        }
        catch(Exception e)
        {
			Console.WriteLine("Символ не распознан");
			return null;
        }
	}

	public StringIdentifier CheckSecondAutomate(string mString)
    {
		SortedSet<string> temp = (SortedSet<string>)_secondAutomate["КЛСЛОВО"];
		if (temp.Contains(mString.ToLower()))
			return new StringIdentifier(mString, "КЛСЛОВО");

		temp = (SortedSet<string>)_secondAutomate["Стандартный тип"];
		if (temp.Contains(mString.ToLower()))
			return new StringIdentifier(mString, "Стандартный тип");

		if (char.IsDigit(mString[0]) && mString.AsEnumerable().Any(ch => char.IsLetter(ch)))
		{
			Console.WriteLine("Идентификатор не может начинаться с цифры");
			return null;
		}
        else
        {
			if (char.IsDigit(mString[0]))
				return new StringIdentifier(mString, "Число");

			return new StringIdentifier(mString, "Идентификатор");
		}
	}
}
