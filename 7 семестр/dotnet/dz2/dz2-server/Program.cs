using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace HttpServerWithThreadPool
{
  class HttpServer
  {
    private HttpListener _listener;
    private bool _isRunning = false;
    private const int PORT = 49314;
    private string htmlFilePath = "report.html"; // Путь к HTML файлу

    public HttpServer()
    {
      _listener = new HttpListener();
      _listener.Prefixes.Add($"http://localhost:{PORT}/");
    }

    public void Start()
    {
      _listener.Start();
      _isRunning = true;
      Console.WriteLine($"Сервер запущен на порту {PORT}. Ожидание запросов...");

      // Основной цикл ожидания подключений
      while (_isRunning)
      {
        var context = _listener.GetContext();
        ThreadPool.QueueUserWorkItem(o => HandleRequest(context));
      }
    }

    public void Stop()
    {
      _isRunning = false;
      _listener.Stop();
      Console.WriteLine("Сервер остановлен.");
    }

    // Обработка запроса клиента
    private void HandleRequest(HttpListenerContext context)
    {
      HttpListenerResponse response = context.Response;

      try
      {
        // Проверяем существование HTML файла
        if (File.Exists(htmlFilePath))
        {
          // Отправляем HTML файл клиенту
          response.ContentType = "text/html";
          byte[] htmlData = File.ReadAllBytes(htmlFilePath);
          response.ContentLength64 = htmlData.Length;

          using (Stream output = response.OutputStream)
          {
            output.Write(htmlData, 0, htmlData.Length);
          }
          Console.WriteLine("HTML файл успешно отправлен клиенту.");
        }
        else
        {
          // Если файл не найден, отправляем сообщение об ошибке
          string errorMessage = "Ошибка: HTML файл не найден.";
          byte[] errorData = Encoding.UTF8.GetBytes(errorMessage);
          response.ContentType = "text/plain";
          response.ContentLength64 = errorData.Length;

          using (Stream output = response.OutputStream)
          {
            output.Write(errorData, 0, errorData.Length);
          }
          Console.WriteLine("Ошибка: HTML файл не найден.");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Ошибка при обработке запроса: {ex.Message}");
      }
      finally
      {
        // Закрываем ответ
        response.Close();
      }
    }

    // Деструктор для остановки сервера
    ~HttpServer()
    {
      if (_isRunning)
      {
        Stop();
      }
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      HttpServer server = new HttpServer();

      // Обработка остановки сервера при завершении программы
      AppDomain.CurrentDomain.ProcessExit += (s, e) => server.Stop();

      server.Start();
    }
  }
}
