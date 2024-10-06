using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lr2
{
    class Program
    {
      static void Main(string[] args)
      {
        // Ввод размера матрицы
        Console.WriteLine("Введите размер матрицы (n):");
        int n = int.Parse(Console.ReadLine());

        // Ввод элементов матрицы
        int[,] matrix = new int[n, n];
        Console.WriteLine("Введите элементы матрицы:");

        for (int i = 0; i < n; i++)
        {
          for (int j = 0; j < n; j++)
          {
            matrix[i, j] = int.Parse(Console.ReadLine());
          }
        }

        // Нахождение максимального элемента ниже главной диагонали
        int maxBelowDiagonal = int.MinValue;
        for (int i = 1; i < n; i++)
        {
          for (int j = 0; j < i; j++)
          {
            if (matrix[i, j] > maxBelowDiagonal)
            {
              maxBelowDiagonal = matrix[i, j];
            }
          }
        }

        // Вычисление суммы элементов на главной диагонали и выше нее, которые больше maxBelowDiagonal
        int sum = 0;
        bool found = false;
        for (int i = 0; i < n; i++)
        {
          for (int j = i; j < n; j++) // Только элементы на главной диагонали и выше
          {
            if (matrix[i, j] > maxBelowDiagonal)
            {
              sum += matrix[i, j];
              found = true;
            }
          }
        }

        // Вывод результата
        if (found)
        {
          Console.WriteLine($"Сумма элементов на главной диагонали и выше, превышающих элементы ниже диагонали: {sum}");
        }
        else
        {
          Console.WriteLine("Таких элементов нет.");
        }
    }
  }
}
