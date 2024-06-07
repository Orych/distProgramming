using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace Server;

class Program
{
    public static void StartListening(int port)
    {
        // Привязываем сокет ко всем сетевым интерфейсам на текущей машине
        IPAddress ipAddress = IPAddress.Any;

        //представляет конечную точку сети (IP-адрес и порт)
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

        // CREATE
        Socket listener = new Socket(
            ipAddress.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        // связывания сокета с локальной конечной точкой
        listener.Bind(localEndPoint);

        //указывает сокету начать прослушивание входящих подключений
        listener.Listen(10);

        List<string> history = new List<string>();

        while (true)
        {
            //блокирует выполнение программы до тех пор, пока не будет получено новое входящее соединение
            Socket handler = listener.Accept();

            byte[] buf = new byte[1024];
            string data = null;

            //считывает данные из сокета и сохраняет их в буфере
            int bytesRec = handler.Receive(buf);

            //данные из буфера декодируются из массива байтов в строку с использованием UTF-8 кодировки
            data = Encoding.UTF8.GetString(buf, 0, bytesRec);

            Console.WriteLine("Message received: {0}", data);
            
            //добавляется в список history
            history.Add(data);

            // Создаем объект для объединения всех строк
            StringBuilder stringBuilder = new StringBuilder();

            // Объединяем все строки в одну
            foreach (string str in history)
            {
                stringBuilder.AppendLine(str);
            }

            // Преобразуем объединенную строку в массив байт 
            // Отправляем текст обратно клиенту
            byte[] msg = Encoding.UTF8.GetBytes(stringBuilder.ToString());

            // отправляет массив байтов обратно клиенту через сокет
            handler.Send(msg);

            // закрывает соединение для чтения и записи
            handler.Shutdown(SocketShutdown.Both);
            //закрывает сокет полностью и освобождает все связанные с ним ресурсы.
            handler.Close();
        }
    }

    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Invalid usage\nUsage: dotnet run <port>");
            return;
        }
        bool success = int.TryParse(args[0], out int port);
        if (!success)
        {
            Console.WriteLine("Invalid port");
            return;
        }
        StartListening(port);
    }
}