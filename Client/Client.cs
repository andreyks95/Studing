using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class Program
    {
        const int port = 8888;
        //const string address = "127.0.0.1";
        //const string address = "87.76.238.192";
        const string address = "192.168.1.127";

        static void Main(string[] args)
        {
            //Console.Write("Введите свое имя:");
            //string userName = Console.ReadLine();
            TcpClient client = null;


            //IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());
            ////string adr = IPHost.AddressList[2].ToString();

            try
            {
                client = new TcpClient(address, port);
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    //Console.Write(userName + ": ");
                    // ввод сообщения
                    //string message = Console.ReadLine();
                    //message = String.Format("{0}: {1}", userName, message);
                    // преобразуем сообщение в массив байтов
                    byte[] data = new byte[] {0};//Encoding.Unicode.GetBytes(message*);
                    // отправка сообщения
                    stream.Write(data, 0, data.Length);

                    // получаем ответ
                    data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    //message = builder.ToString();
                    //Console.WriteLine("Сервер: {0}", message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}