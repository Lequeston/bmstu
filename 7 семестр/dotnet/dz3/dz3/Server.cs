using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace AjaxWebServer
{
  public class Server
  {
    private HttpListener listener;
    private const int PORT = 49314;
    private Calculator calculator;

    public Server()
    {
      listener = new HttpListener();
      listener.Prefixes.Add($"http://localhost:{PORT}/");
      calculator = new Calculator();
    }

    public void Start()
    {
      listener.Start();
      Console.WriteLine($"Сервер запущен на порту {PORT}. Ожидание запросов...");

      while (true)
      {
        var context = listener.GetContext();
        Task.Run(() => HandleRequest(context));
      }
    }

    private async Task HandleRequest(HttpListenerContext context)
    {
      string url = context.Request.Url.AbsolutePath;

      if (url == "/calculate" && context.Request.HttpMethod == "POST")
      {
        using (StreamReader reader = new StreamReader(context.Request.InputStream))
        {
          // Читаем выражение как строку JSON
          string jsonExpression = await reader.ReadToEndAsync();

          // Десериализуем JSON, чтобы извлечь строку выражения
          string expression = JsonSerializer.Deserialize<string>(jsonExpression);

          // Выполняем вычисление через Calculator
          int result = calculator.Calculate(expression);

          // Создаем объект для ответа
          var responseJson = new
          {
            Expression = expression,
            Result = result
          };

          // Сериализуем объект ответа в JSON
          string jsonResponse = JsonSerializer.Serialize(responseJson);
          context.Response.ContentType = "application/json";
          await context.Response.OutputStream.WriteAsync(System.Text.Encoding.UTF8.GetBytes(jsonResponse));
          context.Response.Close();
        }
      }
      else if (url == "/")
      {
        // Отправка HTML страницы
        string htmlPage = File.ReadAllText("index.html");
        byte[] htmlBytes = System.Text.Encoding.UTF8.GetBytes(htmlPage);
        context.Response.ContentType = "text/html";
        await context.Response.OutputStream.WriteAsync(htmlBytes, 0, htmlBytes.Length);
        context.Response.Close();
      }
    }
  }
}
