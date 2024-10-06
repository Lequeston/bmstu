using System;
using System.Reflection;

class Program
{
  // Класс с конструкторами, свойствами и методами
  public class SampleClass
  {
    public int Number { get; set; }

    [CustomAttribute]
    public string Text { get; set; }

    public SampleClass() { }

    public SampleClass(int number)
    {
      this.Number = number;
    }

    public void Display()
    {
      Console.WriteLine($"Number: {this.Number}, Text: {this.Text}");
    }

    public string GetFormattedText()
    {
      return $"Formatted: {Text}";
    }
  }

  // Пользовательский атрибут
  [AttributeUsage(AttributeTargets.Property)]
  public class CustomAttribute : Attribute
  {
  }

  static void Main(string[] args)
  {
    // Получение информации о типе
    Type type = typeof(SampleClass);

    // Вывод информации о конструкторах
    Console.WriteLine("Constructors:");
    foreach (ConstructorInfo constructor in type.GetConstructors())
    {
      Console.WriteLine(constructor.ToString());
    }

    // Вывод информации о свойствах
    Console.WriteLine("\nProperties:");
    foreach (PropertyInfo property in type.GetProperties())
    {
      Console.WriteLine(property.ToString());

      // Вывод свойств с пользовательским атрибутом
      if (Attribute.IsDefined(property, typeof(CustomAttribute)))
      {
        Console.WriteLine($"Property with CustomAttribute: {property.Name}");
      }
    }

    // Вывод информации о методах
    Console.WriteLine("\nMethods:");
    foreach (MethodInfo method in type.GetMethods())
    {
      Console.WriteLine(method.ToString());
    }

    // Вывод информации о свойствах с пользовательским атрибутом
    Console.WriteLine("\nProperties with CustomAttribute:");
    foreach (PropertyInfo property in type.GetProperties())
    {
      if (Attribute.IsDefined(property, typeof(CustomAttribute)))
      {
        Console.WriteLine(property.ToString());
      }
    }

    // Вызов метода с использованием рефлексии
    Console.WriteLine("\nInvoking Display method:");
    SampleClass instance = (SampleClass)Activator.CreateInstance(type);
    MethodInfo displayMethod = type.GetMethod("Display");
    displayMethod.Invoke(instance, null);

    // Вызов метода GetFormattedText
    Console.WriteLine("\nInvoking GetFormattedText method:");
    MethodInfo getFormattedTextMethod = type.GetMethod("GetFormattedText");
    string result = (string)getFormattedTextMethod.Invoke(instance, null);
    Console.WriteLine(result);
  }
}