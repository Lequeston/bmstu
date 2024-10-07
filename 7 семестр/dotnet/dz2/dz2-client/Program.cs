using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace SimpleTcpClient
{
  class Program
  {
    static void Main(string[] args)
    {
      const string SERVER_IP = "127.0.0.1"; // Локальный адрес
      const int PORT = 49314;

      using (TcpClient client = new TcpClient(SERVER_IP, PORT))
      {
        NetworkStream stream = client.GetStream();

        // Получение HTML файла
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string htmlContent = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        // Сохранение HTML файла
        File.WriteAllText("received_report.html", htmlContent);
        Console.WriteLine("HTML файл получен и сохранён как 'received_report.html'");
      }
    }
  }
}
