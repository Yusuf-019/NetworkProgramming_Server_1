
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkProgramming_Server_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Socket server = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            server.Bind(new IPEndPoint(IPAddress.Any, 5000));
            server.Listen(10);

            Console.WriteLine("==========================================");
            Console.WriteLine("               FILE SERVER");
            Console.WriteLine("==========================================");
            Console.WriteLine("Server ishe salindi...");
            Console.WriteLine("Port: 5000");
            Console.WriteLine("Gelen fayllar qebul edilir.\n");

            while (true)
            {
                Socket client = server.Accept();

                byte[] nameBuffer = new byte[1024];
                int nameLength = client.Receive(nameBuffer);

                string fileName = Encoding.UTF8.GetString(
                    nameBuffer, 0, nameLength);

                fileName = Path.GetFileName(fileName);

                long fileSize = 0;

                using (FileStream fs = new FileStream(
                    "received_" + fileName, FileMode.Create))
                {
                    byte[] buffer = new byte[1024];
                    int length;

                    while ((length = client.Receive(buffer)) > 0)
                    {
                        fs.Write(buffer, 0, length);
                        fileSize += length;
                    }
                }

                double sizeKB = fileSize / 1024.0;
                string time = DateTime.Now.ToString("HH:mm:ss");

                Console.WriteLine(
                    $"[QEBUL EDILDI] {fileName} " +
                    $"({sizeKB:F1} KB)  {time}"
                );

                client.Close();
            }
        }
    }
}
