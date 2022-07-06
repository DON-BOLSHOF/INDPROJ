using System;
using System.Collections.Generic;
using System.Linq;

public class AutomatesClasses
{
    private Dictionary<char, string> _firstAutomatePunctuation;
    private Dictionary<string, IEnumerable<string>> _keyWords;
    private Dictionary<string, Dictionary<int,int>> _syntaxAutomate;

    private bool IsEnglishLetter(char c)
    {
        return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
    }

    public AutomatesClasses()
    {
        _firstAutomatePunctuation = new Dictionary<char, string>//Так удобней
		{
            { ':', "Двоеточие" },
            {';', "тчкзпт" },
            {'[', "Обратная квадратная скобочка" },
            {']', "Внутренняя квадратная скобочка" },
            {',', "Запятая" },
            {'.', "Точка" },
            {'+', "знак" },
            {'-', "знак" }
        };

        _keyWords = new Dictionary<string, IEnumerable<string>>
        {
            {"КЛСЛОВО", new SortedSet<string>{ "var","array", "of", "and", "begin", "case", "const", "case", "div", "do", "downto", 
                "else", "if", "end", "file", "for", "function", "goto", "in", "label","mod", "nil", "not", "of", "or", "packed", "procedure",
                "program", "record", "repeat", "set", "then", "to", "type", "until", "while", "with"} },
            {"Стандартный тип", new SortedSet<string>{ "boolean", "byte", "char",  "integer", "longint", "real", "string", "word" } }
        };

        _syntaxAutomate = new Dictionary<string, Dictionary<int, int>>
        {
            { "КЛСЛОВО_var", new Dictionary<int, int> {[0] = 1}  },
            {"Идентификатор", new Dictionary<int, int>{[1] = 2 , [5] = 6, [8] = 9 } },
            { "Запятая", new Dictionary<int, int>{[2] = 1 } },
            { "Двоеточие", new Dictionary<int, int>{[2] = 3 } },
            { "КЛСЛОВО_array", new Dictionary<int, int>{[3] = 4 } },
            { "Обратная квадратная скобочка", new Dictionary<int, int>{[4] = 5 } },
            { "Целое число", new Dictionary<int, int>{[5] = 6, [8] = 9 } },
            { "Точка", new Dictionary<int, int>{[6] = 7, [7] = 8 } },
            { "Внутренняя квадратная скобочка", new Dictionary<int, int>{[9] = 10 } },
            { "КЛСЛОВО_of", new Dictionary<int, int>{[10] = 11 } },
            { "Стандартный тип", new Dictionary<int, int>{[11] = 12 } }, 
            { "тчкзпт", new Dictionary<int, int>{[12] = 13 } }
        };
    }

    public CharIdentifier CheckTranslateBlock(char symbol)
    {
        if (char.IsDigit(symbol))
            return new CharIdentifier(symbol, "цифра");

        if (IsEnglishLetter(symbol))
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
        if (!mString.AsEnumerable().Any(ch => IsEnglishLetter(ch)))
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

    public string CheckSyntaxBlock(List<StringIdentifier> finalString)
    {
        int stateIndex = 0;
        foreach(var mStringId in finalString)
        {
            try
            {
                stateIndex = _syntaxAutomate[mStringId.indentifier][stateIndex];
            }
            catch{
                Console.WriteLine("Ошибка в синтаксическом блоке.");
                return "Reject";
            }
        }

        return "Accept";
    }
}
