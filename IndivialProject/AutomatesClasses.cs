using System;
using System.Collections.Generic;
using System.Linq;

public class AutomatesClasses
{
    private Dictionary<char, string> _firstAutomatePunctuation;
    private Dictionary<string, IEnumerable<string>> _keyWords;

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

        _keyWords = new Dictionary<string, IEnumerable<string>>
        {
            {"КЛСЛОВО", new SortedSet<string>{ "var","array", "of"} },
            {"Стандартный тип", new SortedSet<string>{ "boolean", "byte", "char",  "integer", "longint", "real", "string", "word" } }
        };
    }

    public CharIdentifier CheckTranslateBlock(char symbol)
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
        catch
        {
            Console.WriteLine("Символ не распознан");
            return null;
        }
    }

    public StringIdentifier CheckLexisBlock(string mString)
    {
        if (!mString.AsEnumerable().Any(ch => char.IsLetter(ch)))
        {
            if ((char.IsDigit(mString[0]) && !(mString.Contains('+') || mString.Contains('-'))) || (_firstAutomatePunctuation[mString[0]].Equals("знак") && !(mString.Substring(1).Contains('+') || mString.Substring(1).Contains('-'))))
            {
                return new StringIdentifier(mString, "Целое число");
            }
        }

        if (mString.Contains('+') || mString.Contains('-'))
        {
            Console.WriteLine("Идентификатор не может иметь знаков");
            return null;
        }

        if (char.IsDigit(mString[0]))
        {
            Console.WriteLine("Идентификатор не может начинаться с цифры");
            return null;
        }

        return new StringIdentifier(mString, "Идентификатор");
    }

    public StringIdentifier CheckIdentityBlock(string mString)
    {
		SortedSet<string> temp = (SortedSet<string>)_keyWords["КЛСЛОВО"];
		if (temp.Contains(mString.ToLower()))
			return new StringIdentifier(mString,String.Format("КЛСЛОВО_{0}", mString.ToLower()));

		temp = (SortedSet<string>)_keyWords["Стандартный тип"];
		if (temp.Contains(mString.ToLower()))
			return new StringIdentifier(mString, "Стандартный тип");

        return new StringIdentifier(mString, "Идентификатор");
    }
}
