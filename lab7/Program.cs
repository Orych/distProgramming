﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chain;

class Program
{
    public static void StartInitChain(int listeningPort, string nextHost, int nextPort)
    {
        if (nextHost == "localhost") nextHost = "127.0.0.1";
        IPAddress nextIpAddress = IPAddress.Parse(nextHost);
        IPAddress currIpAddress = IPAddress.Parse("127.0.0.1");

        IPEndPoint senderEP = new IPEndPoint(nextIpAddress, nextPort);
        IPEndPoint listenerEP = new IPEndPoint(currIpAddress, listeningPort);

        // инициирует сокет для отправки сообщений;
        Socket sender = new Socket(
            nextIpAddress.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        // инициирует сокет для входящих сообщений;
        Socket listener = new Socket(
            currIpAddress.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        listener.Bind(listenerEP);
        listener.Listen(10);

        // инициирует переменную X значением из стандартного ввода;
        int x = int.Parse(Console.ReadLine());

        // отправляет следующему соседу значение X;
        byte[] msg = Encoding.UTF8.GetBytes(x.ToString());
        sender.Connect(senderEP);
        int bytesSent = sender.Send(msg);

        // получает от предыдущего соседа число Y и записывает в переменную X;
        Socket listenerHandler = listener.Accept();
        byte[] buf = new byte[1024];
        int bytesRec = listenerHandler.Receive(buf);
        x = int.Parse(Encoding.UTF8.GetString(buf, 0, bytesRec));

        // отправляет следующему соседу значение X;
        msg = Encoding.UTF8.GetBytes(x.ToString());
        bytesSent = sender.Send(msg);
        
        // получает от предыдущего соседа конечное значение X и выводит его в консоль.
        Console.WriteLine(x);
        
        // Закрываем соединения и освобождаем ресурсы               
        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
        listenerHandler.Shutdown(SocketShutdown.Both);
        listenerHandler.Close();
    }

    public static void StartChain(int listeningPort, string nextHost, int nextPort)
    {
        if (nextHost == "localhost") nextHost = "127.0.0.1";
        IPAddress nextIpAddress = IPAddress.Parse(nextHost);
        IPAddress currIpAddress = IPAddress.Parse("127.0.0.1");

        IPEndPoint senderEP = new IPEndPoint(nextIpAddress, nextPort);
        IPEndPoint listenerEP = new IPEndPoint(currIpAddress, listeningPort);

        // CREATE
        Socket sender = new Socket(
            nextIpAddress.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        
        // инициирует сокет для входящих сообщений;
        Socket listener = new Socket(
            currIpAddress.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        // Привязываем сокет прослушивания к его конечной точке и уст. режим прослушивания
        listener.Bind(listenerEP);
        listener.Listen(10);

        // инициирует переменную X значением из стандартного ввода;
        int x = int.Parse(Console.ReadLine());

        // получает от предыдущего соседа число Y;
        Socket listenerHandler = listener.Accept();
        byte[] buf = new byte[1024];
        int bytesRec = listenerHandler.Receive(buf);
        int y = int.Parse(Encoding.UTF8.GetString(buf, 0, bytesRec));

        // Вычисляем максимум
        x = int.Max(x, y);

        // Отправка сообщения следущему макса
        byte[] msg = Encoding.UTF8.GetBytes(x.ToString());
        sender.Connect(senderEP);
        int bytesSent = sender.Send(msg);

        // получает от предыдущего соседа конечное значение X и выводит его в консоль.
        bytesRec = listenerHandler.Receive(buf);
        x = int.Parse(Encoding.UTF8.GetString(buf, 0, bytesRec));
        msg = Encoding.UTF8.GetBytes(x.ToString());
        bytesSent = sender.Send(msg);
        Console.WriteLine(x);
        
        // Закрываем соединения и освобождаем ресурсы
        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
        listenerHandler.Shutdown(SocketShutdown.Both);
        listenerHandler.Close();
    }

    static void Main(string[] args)
    {
        if (args.Length > 3 && bool.Parse(args[3]))
        {
            StartInitChain(int.Parse(args[0]), args[1], int.Parse(args[2]));
            return;
        }
        StartChain(int.Parse(args[0]), args[1], int.Parse(args[2]));
    }
}