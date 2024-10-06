using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lr1
{
    class Program
    {
      static void Main(string[] args)
      {
        // Ввод двух строк от пользователя
        Console.WriteLine("Введите первую строку:");
        string str1 = Console.ReadLine();

        Console.WriteLine("Введите вторую строку:");
        string str2 = Console.ReadLine();

        // Проверка, являются ли строки анаграммами
        if (AreAnagrams(str1, str2))
        {
          Console.WriteLine("Строки являются анаграммами.");
        }
        else
        {
          Console.WriteLine("Строки не являются анаграммами.");
        }
      }

        // Метод для проверки анаграммы
      static bool AreAnagrams(string str1, string str2)
      {
        // Убираем пробелы и приводим к нижнему регистру
        str1 = new string(str1.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLower();
        str2 = new string(str2.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLower();

        // Если длина строк не совпадает, это не анаграммы
        if (str1.Length != str2.Length)
          return false;

        // Сортируем символы в строках и сравниваем их
        return string.Concat(str1.OrderBy(c => c)) == string.Concat(str2.OrderBy(c => c));
      }
    }
}
