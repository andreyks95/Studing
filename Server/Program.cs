using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpListenerApp
{
    class Program
    {
        const int port = 8888; // порт для прослушивания подключений
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
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

                //Теперь вводим диапазоны
                Console.WriteLine(
               "Поиск наибольшего значения функции нескольких переменных методом сканирования с заданным шагом");
                int startX, endX, startY, endY;
                double stepX, stepY;
                List<double> arrayValues = new List<double>();
                //считываем значения
                Console.WriteLine("Введите: нач. значение, кон. значение и шаг для оси X: ");
                startX = Convert.ToInt32(Console.ReadLine().Trim());
                endX = Convert.ToInt32(Console.ReadLine().Trim());
                string valueDoubleX = Console.ReadLine().Trim();
                Console.WriteLine("Введите: нач. значение, кон. значение и шаг для оси Y: ");
                startY = Convert.ToInt32(Console.ReadLine().Trim());
                endY = Convert.ToInt32(Console.ReadLine().Trim());
                string valueDoubleY = Console.ReadLine().Trim();
                double.TryParse(valueDoubleX, NumberStyles.Any, CultureInfo.InvariantCulture, out stepX);
                double.TryParse(valueDoubleY, NumberStyles.Any, CultureInfo.InvariantCulture, out stepY);

                //посчитать процент одного клиента от всех доступных нам клиентов
                double percentage = TakePercentage(clients.Count());

                //получаем шаг для каждого клиента "для раздачи"
                //Например: -20 ... 20 Клиентов 4  - шаг для каждого клиента 10 
                var axis = TakeRangeAxis(startX, endX, startY, endY, percentage);
                double rangeX = axis.Item1,
                    rangeY = axis.Item2;

                //с откуда начинать
                double internalX = startX, 
                       internalY = startY;
               
                List<double> maxList = new List<double>();
                sw.Start();
                //работаем с каждым клиентом
                //создаём для каждого свой поток
                foreach (var currentClient in clients)
                {
                    //отсылаем сначала промежуток для текущего клиента
                    var task = Task.Factory.StartNew(() => Message(currentClient, internalX, internalX+rangeX, stepX, 
                                                                                   internalY, internalY+rangeY, stepY));
                    task.Wait();
                    //добавляем максимумы с каждого клиента
                    maxList.Add(task.Result);
                    //Увеличиваем интервалы для следующего клиента
                    internalX += rangeX;
                    internalY += rangeY;
                }
                double max = GetMax(maxList);
                sw.Stop();
                Console.WriteLine("Максимум функции: " + max);
                Console.WriteLine("Длительность выполнения расчётов (сек.): " + sw.Elapsed.TotalSeconds);
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

        private static double Message(TcpClient currentClient, double startX, double endX, double stepX, double startY, double endY, double stepY)
        {
            // получаем сетевой поток для чтения и записи
            NetworkStream stream = currentClient.GetStream();
            //Отправляем сообщение
            SendMessage(stream, startX, endX, stepX, startY, endY, stepY);
            
            //Принимаем сообщение
            double result =  GetMessage(stream);

            //закрываем подключение
            currentClient.Close();

            return result;
        }

        private static double GetMessage(NetworkStream stream)
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
            Console.WriteLine("Принято сообщение " + message + "\n");
            return Convert.ToDouble(message);
        }

        private static void SendMessage(NetworkStream stream, double startX, double endX, double stepX, double startY, double endY, double stepY)
        {
            //отправляем данные клиенту о промежутке для X и Y в виде строки
            string message = "X: " + startX + " " + endX + " " + stepX + "\nY: " + startY + " " + endY + " " + stepY;
            // сообщение для отправки клиенту
            // преобразуем сообщение в массив байтов
            byte[] data = Encoding.UTF8.GetBytes(message);
            // отправка сообщения
            stream.Write(data, 0, data.Length);
            Console.WriteLine("\nОтправлено сообщение: \n{0}", message);
        }


        //Пример: X на промежутке -20 ... 20 => [-20] + [20] = 40 * 0.25 = 10 возвращаем этот диапазон для одного клиента
        //аналогично для Y. Потом "засовываем" в кортеж и возвращаем
        private static Tuple<double,double> TakeRangeAxis(double startX, double endX, double startY, double endY,  double percentage)
        {
            double rangeX = (Math.Abs(startX) + Math.Abs(endX)) * percentage,
            rangeY = (Math.Abs(startY) + Math.Abs(endY)) * percentage;
            return new Tuple<double, double>(rangeX, rangeY);
        }

        //Пример: 4 клиента, значит доля один клиент занимает 25% 
        private static double TakePercentage(int countClients)
        {
           return (double) 1 / countClients;
        }

        //Поиск маскимума
        private static double GetMax(List<double> array)
        {
            return double.IsNaN(array[0]) ? 0 : array.Max();
        }
    }
}