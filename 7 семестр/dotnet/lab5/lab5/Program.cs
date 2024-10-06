using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lr3
{
  class Program
  {
    static void Main(string[] args)
    {
        // Путь к файлу
        Console.WriteLine("Введите путь к текстовому файлу:");
        string filePath = Console.ReadLine();

        // Проверка существования файла
        if (File.Exists(filePath))
        {
            // Считываем файл построчно
            string[] lines = File.ReadAllLines(filePath);

            Console.WriteLine("Результаты подсчета символов в каждой строке:");

            // Подсчет количества символов в каждой строке
            for (int i = 0; i < lines.Length; i++)
            {
                int charCount = lines[i].Length;
                Console.WriteLine($"Строка {i + 1}: {charCount} символов");
            }
        }
        else
        {
            Console.WriteLine("Файл не найден.");
        }
    }
  }
}
