
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("               FILE CLIENT");
            Console.WriteLine("==========================================");

            while (true)
            {
                Console.WriteLine("\n1 - Fayl gonder");
                Console.WriteLine("0 - Cixis");
                Console.Write("Secim edin: ");

                string? choice = Console.ReadLine();

                if (choice == "0")
                    break;

                if (choice == "1")
                {
                    Console.Write("Faylin yolunu daxil edin: ");
                    string path = Console.ReadLine()!;

                    if (!File.Exists(path))
                    {
                        Console.WriteLine(
                            "XETA: Fayl tapilmadi! Yeniden cehd edin.");
                        continue;
                    }

                    Socket client = new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp
                    );

                    client.Connect(
                        new IPEndPoint(IPAddress.Loopback, 5000));

                    string fileName = Path.GetFileName(path);
                    byte[] name = Encoding.UTF8.GetBytes(fileName);

                    client.Send(name);

                    using (FileStream fs = new FileStream(
                        path, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[1024];
                        int length;

                        while ((length = fs.Read(
                            buffer, 0, buffer.Length)) > 0)
                        {
                            client.Send(buffer, 0, length, SocketFlags.None);
                        }
                    }

                    client.Shutdown(SocketShutdown.Send);

                    Console.WriteLine("Fayl gonderildi!");

                    client.Close();
                }
            }
        }
    }
}
