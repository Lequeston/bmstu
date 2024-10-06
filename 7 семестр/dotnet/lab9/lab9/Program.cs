using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace lab9
{
  public class Customer
  {
    public string fio { get; set; }
    public string address { get; set; }
    public double discount { get; set; }
    public Customer(string fio0, string address0, double discount0)
    {
      fio = fio0;
      address = address0; discount = discount0;
    }
  }

  public class Product
  {
    public string name { get; set; }
    public decimal price { get; set; }
    public void SetParams(string name0, decimal price0)
    {
      name = name0; price = price0;
    }

  }

  public class Base
  {
    public Dictionary<string, Product> products { get; set; }
    public Base()
    {
      products = new Dictionary<string, Product>();
    }
    public void AddProduct(string id, string name, decimal price)
    {
      Product product = new Product(); product.SetParams(name, price); products.Add(id, product);
    }
    public void Show()
    {
      foreach (KeyValuePair<string, Product> product in products)
      {
        Console.WriteLine("{0} {1} {2}", product.Key, product.Value.name, product.Value.price);
      }

    }

    public void SaveBase()
    {
      var options = new JsonSerializerOptions
      {
        WriteIndented = true
      };
      string json = JsonSerializer.Serialize(this, options);
      File.WriteAllText("base.json", json);
      Console.WriteLine(json);
    }

    public void LoadBase()
    {
      var options = new JsonSerializerOptions
      {
        WriteIndented = true
      };
      string json = File.ReadAllText("base.json");
      Base? restoredBase = JsonSerializer.Deserialize<Base>(json, options); products = restoredBase.products;
      Console.WriteLine(json);
    }

  }

  public class OrderLine
  {
    public string product_id { get; set; }
    public int count { get; set; }
    public OrderLine(string id, int count0)
    {
      product_id = id; count = count0;
    }
  }
  public class Order
  {
    public int number { get; set; }
    public Customer customer { get; set; }
    public Base shopBase;
    public double discount { get; set; }
    public decimal sum_price { get; set; }
    public List<OrderLine> orderLines { get; set; }

    public Order(int number0, Customer customer0, Base base0)
    {
      number = number0; customer = customer0; shopBase = base0; sum_price = 0;
      orderLines = new List<OrderLine>();
    }

    public void AddOrderLine(string id, int count)
    {
      orderLines.Add(new OrderLine(id, count));
      sum_price = sum_price + shopBase.products[id].price * count;
      //Console.WriteLine(shopBase.products[id].price);
      //Console.WriteLine(count);
      //Console.WriteLine(sum_price);
      discount = (double)sum_price * customer.discount;
    }

    public void SaveOrder()
    {
      string json = JsonSerializer.Serialize(this);
      File.WriteAllText("order.json", json);
      Console.WriteLine(json);
    }
  }
  class Program
  {
    static void Main(string[] args)
    {
      Base shopBase = new Base();
      string input = "";
      string id; string name; decimal price;
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("Заполнение БД");
      Console.ForegroundColor = ConsoleColor.White;
      while (input != "end")
      {
        Console.WriteLine("ID товара"); input = Console.ReadLine();
        if (input == "end") break;
        id = input;

        Console.WriteLine("Имя товара"); name = Console.ReadLine();
        if (name == "end") break;

        Console.WriteLine("Цена товара"); input = Console.ReadLine();
        if (input == "end") break;
        price = decimal.Parse(input);

        shopBase.AddProduct(id, name, price);
      }
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("Данные в БД");
      Console.ForegroundColor = ConsoleColor.White;
      shopBase.Show();

      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("Создание пользователя");
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine("ФИО пользователя");
      string fio = Console.ReadLine();
      Console.WriteLine("Адрес пользователя");
      string address = Console.ReadLine();
      Console.WriteLine("Скидка пользователя в %");
      double discount = double.Parse(Console.ReadLine()) / 100; Customer customer = new Customer(fio, address, discount);

      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("Сбор заказа");
      Console.ForegroundColor = ConsoleColor.White;

      Order order = new Order(0, customer, shopBase); input = "";
      int count;
      while (input != "end")
      {
        Console.WriteLine("ID товара"); input = Console.ReadLine();
        if (input == "end") break;
        id = input;

        Console.WriteLine("Количество единиц товара"); input = Console.ReadLine();
        if (input == "end")
          break;
        count = int.Parse(input);

        order.AddOrderLine(id, count);
      }

      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("Данные заказа");
      Console.ForegroundColor = ConsoleColor.White;
      order.SaveOrder();
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("Данные в базе");
      Console.ForegroundColor = ConsoleColor.White;
      shopBase.SaveBase();
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("Востановленные данные в базе");
      Console.ForegroundColor = ConsoleColor.White;
      shopBase.LoadBase();
    }
  }
}