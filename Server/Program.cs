using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Threading;
using Server;

namespace ConsoleServer
{
    class Program
    {
        const int port = 8888;
        static TcpListener listener;
        static void Main(string[] args)
        {
            List<TcpClient> listConnectedClients = new List<TcpClient>();
            try
            {
                /*
                //string address = "127.0.0.1";
                //string address = "192.168.1.133";
                //listener = new TcpListener(IPAddress.Parse(address), port);
                Stopwatch sw = new Stopwatch();
                BigInteger startX, endX, startY, endY;
                double stepX, stepY, max;
                List<double> arrayValues = new List<double>();

                Console.WriteLine("Поиск наибольшего значения функции нескольких переменных методом сканирования с заданным шагом");
                //считываем значения
                Console.WriteLine("Введите: нач. значение, кон. значение и шаг для оси X: ");
                startX = Convert.ToInt32(Console.ReadLine().Trim());
                endX = Convert.ToInt32(Console.ReadLine().Trim());
                string valueDoubleX = Console.ReadLine().Trim();
                Console.WriteLine("Введите: нач. значение, кон. значение и шаг для оси Y: ");
                startY = Convert.ToInt32(Console.ReadLine().Trim());
                endY = Convert.ToInt32(Console.ReadLine().Trim());
                string valueDoubleY = Console.ReadLine().Trim();
                if (!double.TryParse(valueDoubleX, NumberStyles.Any, CultureInfo.InvariantCulture, out stepX) ||
                    !double.TryParse(valueDoubleY, NumberStyles.Any, CultureInfo.InvariantCulture, out stepY))
                    Console.WriteLine("Ошибка ввода дробного числа!");
                else
                {
                    sw.Start();//Проверяем длительность выполнения расчётов
                               // arrayValues = new double[GetSizeArray(startX, endX, stepX, startY, endY, stepY)];
                    //arrayValues = GetValuesXY(startX, endX, stepX, startY, endY, stepY);
                    //max = GetMax(arrayValues);
                    Console.WriteLine("Наибольшее значение функции: " + max);
                    sw.Stop();
                    Console.WriteLine("Длительность выполнения расчётов (сек.): " + sw.Elapsed.TotalSeconds);
                }
                */
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                Console.WriteLine("Ожидание подключений...");


                
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    listConnectedClients.Add(client);
                    Console.WriteLine(listConnectedClients.Count);
                }
                /*while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);

                    // создаем новый поток для обслуживания нового клиента
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
                
            }
        }
    }
}