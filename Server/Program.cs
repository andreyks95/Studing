using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpListenerApp
{
    class Program
    {
        const int port = 8888; // порт для прослушивания подключений
        static void Main(string[] args)
        {
            //Список клиентов клиентов
            List<TcpClient> clients = new List<TcpClient>();
            List<NetworkStream> streams = new List<NetworkStream>();
            TcpListener server = null;
            try
            {
                //IPAddress localAddr = IPAddress.Parse("127.0.0.1");//IPAddress.Any
                server = new TcpListener(IPAddress.Any, port);

                // запуск слушателя
                server.Start();

                Console.WriteLine("Ожидание подключений... ");
                Console.WriteLine("Нажмите stop для начала выполнения задания.");
                //Посчитать количество, потом распилить массив и ждать от каждого максимум.

                while (true)
                {
                    // получаем входящее подключение
                    TcpClient client = server.AcceptTcpClient();
                    //добавляем в список клиента
                    clients.Add(client);
                    Console.WriteLine("Подключено клиент. Количество клиентов " + clients.Count());
                    //Если нажато "stop" - выходим с цикла подключений, и дальше работаем с существующими клиентами
                    if (Console.ReadLine().ToLower() != "stop") continue;
                    else
                        break;
                }
                int response = 0;
                //работаем с каждым клиентом
                //сделать для каждого свой поток
                foreach (var currentClient in clients)
                {
                    response += 1;
                    Task task = Task.Factory.StartNew(() => Message(currentClient, response));
                    task.Wait();
                }

                /*int response = 0;
                //работаем с каждым клиентом
                //сделать для каждого свой поток
                foreach (var currentClient in clients)
                {
                    response += 1;
                    Message(currentClient, response);
                }*/

                #region Черновик

                #region старый цикл
                /*
                while (true)
                {

                    // получаем входящее подключение
                    TcpClient client = server.AcceptTcpClient();

                    clients.Add(client);
                    Console.WriteLine("Подключено клиент. Количество клиентов " + clients.Count());
                    // streams.Add(client.GetStream());
                    //Console.WriteLine("Подключен клиент. Выполнение запроса... stop для остановки и отдачи задача");

                    //Если клиент подключён, продолжать дальше подкючения 
                    //всё тормозиться из-за чтения записи. 
                    //проверить успешно ли подключён клиент
                    //если да 
                    //то продолжить подключение
                    //или отключить подключение 
                    if (Console.ReadLine().ToLower() != "stop") continue;
                    else
                    {
                        break;
                    }
                    
                    // получаем сетевой поток для чтения и записи
                    //NetworkStream stream = client.GetStream();
                    //
                    // сообщение для отправки клиенту
                    //string response = "Привет мир";
                    // преобразуем сообщение в массив байтов
                    //byte[] data = Encoding.UTF8.GetBytes(response);
                    //
                    // отправка сообщения
                    //stream.Write(data, 0, data.Length);
                    //Console.WriteLine("Отправлено сообщение: {0}", response);
                    // закрываем подключение
                    //client.Close();
                }
            */
                #endregion

                #region более менее
                /*int response = 0;
                //работаем с каждым клиентом
                //сделать для каждого свой поток
                foreach (var currentClient in clients)
                {
                    // получаем сетевой поток для чтения и записи
                    NetworkStream stream = currentClient.GetStream();

                    // сообщение для отправки клиенту
                    response += 1;
                    // преобразуем сообщение в массив байтов
                    byte[] data = Encoding.UTF8.GetBytes(response.ToString());
                    // отправка сообщения
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Отправлено сообщение: {0}", response);

                    //для принятия сообщений
                    data = new byte[256];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    string message = builder.ToString();
                    Console.WriteLine("Принято сообщения " + message);

                    // закрываем подключение
                    currentClient.Close();
                }*/
                #endregion

                #region Ещё одна запись
                /*
                int response = 0;
                //работаем с каждым клиентом
                //сделать для каждого свой поток
                foreach (var currentClient in clients)
                {
                    // получаем сетевой поток для чтения и записи
                    NetworkStream stream = currentClient.GetStream();
                    response += 1;
                    //Отправляем сообщение
                    SendMessage(stream, response);
                    //Принимаем сообщение
                    GetMessage(stream);
                    // закрываем подключение
                    currentClient.Close();
                }*/
                #endregion

                #region Оригинальная запись

                /*while (true)
                {
                    Console.WriteLine("Ожидание подключений... ");

                    // получаем входящее подключение
                    TcpClient client = server.AcceptTcpClient();

                    Console.WriteLine("Подключен клиент. Выполнение запроса... stop для остановки и отдачи задача");
                    if (Console.ReadLine().ToLower() == "stop")
                    {

                    }
                    // получаем сетевой поток для чтения и записи
                    NetworkStream stream = client.GetStream();

                    // сообщение для отправки клиенту
                    string response = "Привет мир";
                    // преобразуем сообщение в массив байтов
                    byte[] data = Encoding.UTF8.GetBytes(response);

                    // отправка сообщения
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Отправлено сообщение: {0}", response);
                    // закрываем подключение
                    client.Close();
                }*/

                #endregion

                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
                Console.Read();
            }
        }

        private static void Message(TcpClient currentClient, int response)
        {
            // получаем сетевой поток для чтения и записи
            NetworkStream stream = currentClient.GetStream();
            //Отправляем сообщение
            SendMessage(stream, response);
            //Принимаем сообщение
            GetMessage(stream);
            // закрываем подключение
            currentClient.Close();
        }

        private static void GetMessage(NetworkStream stream)
        {

            //для принятия сообщений
            byte[] data = new byte[256];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);
            string message = builder.ToString();
            Console.WriteLine("Принято сообщения " + message);
        }

        private static void SendMessage(NetworkStream stream, int response)
        {
            // сообщение для отправки клиенту
            // преобразуем сообщение в массив байтов
            byte[] data = Encoding.UTF8.GetBytes(response.ToString());
            // отправка сообщения
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Отправлено сообщение: {0}", response);
        }
    }
}