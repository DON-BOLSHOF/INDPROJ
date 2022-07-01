using System;
using System.Collections.Generic;

namespace IndivialProject
{
    class StringChecker
    {
        private static string _string;
        private static CharIdentifier[] _firstAutomate;
        private static AutomatesClasses _automatesClasses;

        private static List<StringIdentifier> _secondAutomate;

        static void Main(string[] args)
        {
            Console.Write("Введите строку: ");
            _string = Console.ReadLine();

            _automatesClasses = new AutomatesClasses();
            _firstAutomate = new CharIdentifier[_string.Length];
            _secondAutomate = new List<StringIdentifier>();

            Console.WriteLine("Первый автомат:");

            for (int i = 0; i < _string.Length; i++)
            {
                _firstAutomate[i] = _automatesClasses.CheckFirstAutomate(_string[i]);
            }

            for(int i = 0; i< _string.Length; i++)
            {
                if (_firstAutomate[i] == null)
                    return;

                Console.WriteLine(_firstAutomate[i].ToString());
            }

            Console.WriteLine("Второй автомат:");

            for (int i = 0; i < _string.Length; i++)
            {
                if (_firstAutomate[i].indentifier.Equals("цифра") || _firstAutomate[i].indentifier.Equals("буква"))
                {
                    string temp = "";
                    while (_firstAutomate[i].indentifier.Equals("цифра") || _firstAutomate[i].indentifier.Equals("буква"))
                    {
                        temp += _firstAutomate[i].symbol;
                        i++;
                    }

                    var id = _automatesClasses.CheckSecondAutomate(temp);
                    if (id == null) //При ошибке выходим из кода
                        return;

                   _secondAutomate.Add(id);
                }

                if (_firstAutomate[i].indentifier.Equals("пробел"))
                    continue;

                _secondAutomate.Add(new StringIdentifier(_firstAutomate[i].symbol.ToString(), _firstAutomate[i].indentifier));
            }

            for (int i = 0; i < _secondAutomate.Count; i++)
            {
                if (_secondAutomate[i] == null)
                    return;

                Console.WriteLine(_secondAutomate[i].ToString());
            }
        }
    }
}
