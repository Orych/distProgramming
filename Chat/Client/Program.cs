using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

class Program
{
    public static void StartClient(string hostaddress, int port, string msg)
    {
        try
        {
            // Разрешение сетевых имён
            //"localhost" то используется локальный адрес 
            IPAddress ipAddress = (hostaddress == "localhost") ? IPAddress.Loopback : IPAddress.Parse(hostaddress);

            //объект представляющий точку к которой будет осуществлено подключение.
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            // CREATE
            Socket sender = new Socket(
                ipAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            //Устанавливается соединение с удаленной конечной точкой
            sender.Connect(remoteEP);

            //Отправляется сообщение на сервер в виде массива байтов, закодированного в UTF-8.
            int bytesSent = sender.Send(Encoding.UTF8.GetBytes(msg));

            //Происходит прием данных от сервера в буфер размером 1024 байта.
            byte[] buf = new byte[1024];
            int bytesRec = sender.Receive(buf);

            //данные декодируются из массива байтов в строку с использованием UTF-8 
            Console.WriteLine("{0}",
                Encoding.UTF8.GetString(buf, 0, bytesRec));

            //сокет закрывается для чтения и записи
            sender.Shutdown(SocketShutdown.Both);
            //закрывается и освобождаются связанные с ним ресурсы.
            sender.Close();
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Invalid usage.\nUsage: dotnet run <hostAddress> <port> <msg>");
            return;
        }
        string hostAddress = args[0];
        bool success = int.TryParse(args[1], out int port);
        string msg = args[2];

        if (String.IsNullOrEmpty(msg))
        {
            Console.WriteLine("Empty message.");
            return;
        }

        if (!success)
        {
            Console.WriteLine("Invalid port");
            return;
        }

        StartClient(hostAddress, port, msg);
    }
}