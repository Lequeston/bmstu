using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lr3
{
  // Делегат для проверки элемента
  public delegate bool ElementCondition(int element, int target);

  class Program
  {
    static void Main(string[] args)
    {
      // Ввод данных от пользователя
      Console.WriteLine("Введите размер массива:");
      int arraySize = int.Parse(Console.ReadLine());

      Console.WriteLine("Введите контрольное число:");
      int targetNumber = int.Parse(Console.ReadLine());

      // Генерация случайного массива
      int[] randomArray = GenerateRandomArray(arraySize);
      Console.WriteLine("Сгенерированный массив: ");
      Console.WriteLine(string.Join(", ", randomArray));

      // Создаем делегат для проверки условия (разница не более чем на 4)
      ElementCondition condition = (element, target) => Math.Abs(element - target) <= 4;

      // Получаем подмножество элементов, соответствующих условию
      int[] subset = GetSubset(randomArray, targetNumber, condition);

      // Вывод результата
      Console.WriteLine("Элементы, которые отличаются от заданного числа не более чем на 4:");
      Console.WriteLine(subset.Length > 0 ? string.Join(", ", subset) : "Нет подходящих элементов.");
    }

    // Метод для генерации массива случайных чисел
    static int[] GenerateRandomArray(int size)
    {
      Random random = new Random();
      int[] array = new int[size];

      for (int i = 0; i < size; i++)
      {
        array[i] = random.Next(0, 100); // Генерируем случайные числа от 0 до 100
      }

      return array;
    }

    // Метод для получения подмножества элементов
    static int[] GetSubset(int[] array, int target, ElementCondition condition)
    {
      return array.Where(element => condition(element, target)).ToArray();
    }
  }
}

