using System;
using System.Collections.Generic;

namespace BauerZamelsonCalculator
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length > 0 && args[0] == "test")
      {
        RunTests();
      }
      else
      {
        RunConsoleInput();
      }
    }

    static void RunTests()
    {
      // Тестовые данные (входные выражения и ожидаемые результаты)
      string[] inputs = { "1+2", "(234-11)*34", "6*6/6", "(2+3)-4", "4-(2*3)", "10+((3*5)+2)", "100/(2+3)", "5+(6*7)-(8/4)", "15-(3*2)", "((3+5)*2)" };
      string[] outputs = { "3", "7582", "6", "1", "-2", "27", "20", "45", "9", "16" };

      int countTestsSuccess = 0;
      int countTestsFailed = 0;
      // Цикл по тестовым выражениям
      for (int i = 0; i < inputs.Length; i++)
      {
        Console.WriteLine($"Expression: {inputs[i]}");
        try
        {
          // Вычисляем результат выражения
          int result = Calculate(inputs[i]);
          Console.WriteLine($"Result: {result}, Expected: {outputs[i]}");

          // Сравнение результата с ожидаемым
          if (result.ToString() == outputs[i])
          {
            Console.WriteLine("Тест пройден успешно.\n");
            countTestsSuccess++;
          }
          else
          {
            Console.WriteLine("Тест не пройден.\n");
            countTestsFailed++;
          }
        }
        catch (Exception ex)
        {
          // Обработка ошибок (например, некорректное выражение)
          Console.WriteLine($"Тест не пройден: {ex.Message}\n");
        }
      }

      if (countTestsSuccess == inputs.Length)
      {
        Console.WriteLine("Все тесты пройдены успешно.\n");
      }
      else
      {
        Console.WriteLine($"Тесты пройдены с ошибками: {countTestsFailed} из {inputs.Length}.\n");
      }
    }

    static void RunConsoleInput()
    {
      Console.WriteLine("Введите выражение для вычисления (или 'exit' для выхода):");
      while (true)
      {
        Console.Write(">> ");
        string input = Console.ReadLine();
        if (input.ToLower() == "exit")
        {
          break;
        }

        try
        {
          int result = Calculate(input);
          Console.WriteLine($"Результат: {result}");
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Ошибка: {ex.Message}");
        }
      }
    }

    // Метод для вычисления выражения по методу Бауэра-Земельзона
    static int Calculate(string expression)
    {
      // Шаг 1: Преобразование выражения в обратную польскую запись (ОПЗ)
      string rpn = ConvertToRPN(expression);

      // Шаг 2: Вычисление значения выражения, представленного в ОПЗ
      return EvaluateRPN(rpn);
    }

    // Преобразование инфиксного выражения в обратную польскую запись (ОПЗ)
    static string ConvertToRPN(string expression)
    {
      // Стек для хранения операторов
      Stack<char> stack = new Stack<char>();
      // Строка для результата в ОПЗ
      string output = "";
      // Переменная для накопления многоразрядных чисел
      string number = "";

      // Проход по каждому символу выражения
      foreach (char token in expression)
      {
        // Если символ — цифра (накапливаем число)
        if (char.IsDigit(token))
        {
          number += token;
        }
        else
        {
          // Если число накоплено, добавляем его в результат и сбрасываем накопление
          if (number != "")
          {
            output += number + " ";
            number = "";
          }

          // Обработка скобок и операторов
          if (token == '(')
          {
            stack.Push(token);  // Открывающая скобка помещается в стек
          }
          else if (token == ')')
          {
            // Закрывающая скобка: извлекаем операторы до открывающей скобки
            while (stack.Count > 0 && stack.Peek() != '(')
            {
              output += stack.Pop() + " ";
            }
            stack.Pop();  // Убираем открывающую скобку
          }
          else if (IsOperator(token))
          {
            // Обработка операторов: поддержка приоритетов операций
            while (stack.Count > 0 && GetPrecedence(token) <= GetPrecedence(stack.Peek()))
            {
              output += stack.Pop() + " ";
            }
            stack.Push(token);  // Добавляем текущий оператор в стек
          }
        }
      }

      // Если осталось накопленное число, добавляем его в результат
      if (number != "")
      {
        output += number + " ";
      }

      // Извлекаем все оставшиеся операторы из стека
      while (stack.Count > 0)
      {
        output += stack.Pop() + " ";
      }

      // Возвращаем окончательный результат в виде строки ОПЗ
      return output.Trim();
    }

    // Вычисление выражения, представленного в ОПЗ
    static int EvaluateRPN(string rpn)
    {
      // Стек для операндов
      Stack<int> stack = new Stack<int>();
      // Разделяем строку ОПЗ на токены (операнды и операторы)
      string[] tokens = rpn.Split(' ');

      // Обрабатываем каждый токен
      foreach (string token in tokens)
      {
        // Если токен — число, помещаем его в стек
        if (int.TryParse(token, out int number))
        {
          stack.Push(number);
        }
        // Если токен — оператор, выполняем операцию
        else if (IsOperator(token[0]))
        {
          // Извлекаем два операнда
          int operand2 = stack.Pop();
          int operand1 = stack.Pop();

          // Выполняем операцию и результат кладём обратно в стек
          int result = PerformOperation(operand1, operand2, token[0]);
          stack.Push(result);
        }
      }

      // В стеке остаётся окончательный результат
      return stack.Pop();
    }

    // Проверка, является ли символ оператором
    static bool IsOperator(char c)
    {
      return c == '+' || c == '-' || c == '*' || c == '/';
    }

    // Определение приоритета операций (чем выше значение, тем выше приоритет)
    static int GetPrecedence(char op)
    {
      switch (op)
      {
        case '+':
        case '-':
          return 1;  // Низкий приоритет
        case '*':
        case '/':
          return 2;  // Высокий приоритет
        default:
          return 0;  // Неизвестный оператор
      }
    }

    // Выполнение арифметической операции
    static int PerformOperation(int operand1, int operand2, char op)
    {
      switch (op)
      {
        case '+': return operand1 + operand2;  // Сложение
        case '-': return operand1 - operand2;  // Вычитание
        case '*': return operand1 * operand2;  // Умножение
        case '/': return operand1 / operand2;  // Деление
        default: throw new ArgumentException("Неверный оператор");
      }
    }
  }
}