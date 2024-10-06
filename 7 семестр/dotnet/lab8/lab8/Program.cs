using System;

class Program
{
  static void Main()
  {
    try
    {
      // Ввод первого числа
      Console.Write("Введите первое число: ");
      string num1Input = Console.ReadLine();

      // Проверка на не введенное число
      if (string.IsNullOrEmpty(num1Input))
      {
        throw new InvalidOperationException("Не введено число");
      }

      // Проверка на слишком длинное число
      if (num1Input.Length > 15)
      {
        throw new InvalidOperationException("Введено слишком длинное число");
      }

      // Преобразование ввода в вещественное число
      if (!double.TryParse(num1Input, out double num1))
      {
        throw new InvalidOperationException("Ошибка преобразования");
      }

      // Ввод второго числа
      Console.Write("Введите второе число: ");
      string num2Input = Console.ReadLine();

      // Проверка на не введенное число
      if (string.IsNullOrEmpty(num2Input))
      {
        throw new InvalidOperationException("Не введено число");
      }

      // Проверка на слишком длинное число
      if (num2Input.Length > 15)
      {
        throw new InvalidOperationException("Введено слишком длинное число");
      }

      // Преобразование ввода во вещественное число
      if (!double.TryParse(num2Input, out double num2))
      {
        throw new InvalidOperationException("Ошибка преобразования");
      }

      // Выполнение деления
      if (num2 == 0)
      {
        throw new DivideByZeroException("Деление на ноль");
      }

      double result = num1 / num2;
      Console.WriteLine($"Результат деления: {result}");
    }
    catch (InvalidOperationException ex)
    {
      Console.WriteLine($"Ошибка: {ex.Message}");
    }
    catch (DivideByZeroException ex)
    {
      Console.WriteLine($"Ошибка: {ex.Message}");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
    }
  }
}
