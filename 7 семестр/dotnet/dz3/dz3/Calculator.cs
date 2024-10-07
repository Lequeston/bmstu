using System;
using System.Collections.Generic;

namespace AjaxWebServer
{
  public class Calculator
  {
    // Метод для вычисления выражения по методу Бауэра-Земельзона
    public int Calculate(string expression)
    {
      // Шаг 1: Преобразование выражения в обратную польскую запись (ОПЗ)
      string rpn = ConvertToRPN(expression);

      // Шаг 2: Вычисление значения выражения, представленного в ОПЗ
      return EvaluateRPN(rpn);
    }

    // Преобразование инфиксного выражения в обратную польскую запись (ОПЗ)
    private string ConvertToRPN(string expression)
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
    private int EvaluateRPN(string rpn)
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
    private bool IsOperator(char c)
    {
      return c == '+' || c == '-' || c == '*' || c == '/';
    }

    // Определение приоритета операций (чем выше значение, тем выше приоритет)
    private int GetPrecedence(char op)
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
    private int PerformOperation(int operand1, int operand2, char op)
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
