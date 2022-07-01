using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IndivialProject
{
    class StringChecker
    {
        private static string _string;
        private static CharIdentifier[] _firstAutomate;
        private static AutomatesClasses _automatesClasses;

        private static List<StringIdentifier> _secondAutomate;
        private static List<StringIdentifier> _thirdAutomate;

        private static string path;

        static void Main(string[] args)
        {
            path = @"C:\Users\Acer\source\repos\IndivialProject\output.txt";

            Console.Write("Введите строку: ");
            _string = Console.ReadLine();

            _automatesClasses = new AutomatesClasses();
            _firstAutomate = new CharIdentifier[_string.Length];
            _secondAutomate = new List<StringIdentifier>();
            _thirdAutomate = new List<StringIdentifier>();
            #region FirstAutomate
            Console.WriteLine("Первый автомат:");

            for (int i = 0; i < _string.Length; i++)
            {
                _firstAutomate[i] = _automatesClasses.CheckTranslateBlock(_string[i]);
            }

            for(int i = 0; i< _string.Length; i++)
            {
                if (_firstAutomate[i] == null)
                {
                    File.WriteAllText(path, "Reject");
                    Console.WriteLine("Ошибка в транслитерации");
                    return;
                }

                Console.WriteLine(_firstAutomate[i].ToString());
            }

            #endregion
            #region SecondAutomate
            Console.WriteLine("Второй автомат:");

            for (int i = 0; i < _string.Length; i++)
            {
                if (_firstAutomate[i].indentifier.Equals("цифра") || _firstAutomate[i].indentifier.Equals("буква") || _firstAutomate[i].indentifier.Equals("знак"))
                {
                    string temp = "";
                    while (i < _string.Length && (_firstAutomate[i].indentifier.Equals("цифра") || _firstAutomate[i].indentifier.Equals("буква") || _firstAutomate[i].indentifier.Equals("знак")))
                    {
                        temp += _firstAutomate[i].symbol;
                        i++;
                    }

                    var id = _automatesClasses.CheckLexisBlock(temp);
                    if (id == null) //При ошибке выходим из кода
                    {
                        File.WriteAllText(path, "Reject");
                        Console.WriteLine("Ошибка в Лексическом блоке");
                        return;
                    }

                   _secondAutomate.Add(id);
                }

                if (i >= _string.Length)
                    break;

                if (_firstAutomate[i].indentifier.Equals("пробел"))
                    continue;

                _secondAutomate.Add(new StringIdentifier(_firstAutomate[i].symbol.ToString(), _firstAutomate[i].indentifier));
            }

            for (int i = 0; i < _secondAutomate.Count; i++)
            {
                Console.WriteLine(_secondAutomate[i].ToString());
            }
            #endregion
            #region ThirdAutomate
            Console.WriteLine("Третий автомат");

            foreach (var id in _secondAutomate)
            {
                StringIdentifier tempID;
                if (id.indentifier == "Идентификатор")
                {
                    tempID = _automatesClasses.CheckIdentityBlock(id.String);
                    if (tempID == null) //При ошибке выходим из кода
                    {
                        File.WriteAllText(path, "Reject");
                        Console.WriteLine("Ошибка в идентификации");
                        return;
                    }
                }
                else
                {
                    tempID = new StringIdentifier(id);
                }

                _thirdAutomate.Add(tempID);
            }

                for (int i = 0; i < _thirdAutomate.Count; i++)
            {
                Console.WriteLine(_thirdAutomate[i].ToString());
            }
            #endregion
            #region FourthAutomate
            List<string> idealString = new List<string>{
                "КЛСЛОВО_var",
                "Идентификатор",
                "двоеточие",
                "КЛСЛОВО_array",
                "обратная квадратная скобочка",
                "Целое число/Идентификатор",
                "точка",
                "точка",
                "Целое число/Идентификатор",
                "внутренняя квадратная скобочка",
                "КЛСЛОВО_of",
                "Стандартный тип",
                "тчкзпт"
            };

            while (_thirdAutomate.Count > 0 && idealString.Count > 0)
            {
                if(_thirdAutomate[0].indentifier == idealString[0])
                {
                    _thirdAutomate.RemoveAt(0);
                    if (idealString[0] == "Идентификатор" && _thirdAutomate.Count > 0 && _thirdAutomate[0].indentifier == "запятая")
                        _thirdAutomate.RemoveAt(0);
                    else
                        idealString.RemoveAt(0);
                }
                else
                {
                    if (idealString[0].Contains('/') && !_thirdAutomate[0].indentifier.Contains('/'))
                    {
                        if(idealString[0].Contains(_thirdAutomate[0].indentifier))
                        {
                            _thirdAutomate.RemoveAt(0);
                            idealString.RemoveAt(0);
                            continue;
                        }
                    }

                    break;
                }
            }

            if (_thirdAutomate.Count == 0 && idealString.Count == 0)
                File.WriteAllText(path, "Accept");
            else
                File.WriteAllText(path, "Reject");
            #endregion
        }
    }
}
