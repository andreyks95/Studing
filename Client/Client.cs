using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;

namespace TcpClientApp
{
    class Program
    {
        private const int port = 8888;
        private const string server = "192.168.1.127";//"127.0.0.1";

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(server, port);
                NetworkStream stream = client.GetStream();

                //получаем сообщение из сервера
                string messageGet = GetMessage(stream);
                //разбиваем для оси X и Y 
                string[] message = GetStringData(messageGet, new char[] { '\n' });
                //создаём массивы значений отдельно для оси X и Y
                string[] messageX = GetStringData(message[0], new char[] { 'X', 'Y', ':', ' ', '\n' }),
                    messageY = GetStringData(message[1], new char[] { 'X', 'Y', ':', ' ', '\n' });

                List<string> axisX = GetStringTrim(messageX),
                             axisY = GetStringTrim(messageY);
                //конвертируем значение строк в числа
                double startX = Convert.ToDouble(axisX[0]),
                    endX   = Convert.ToDouble(axisX[1]),
                    stepX  = Convert.ToDouble(axisX[2]),
                    startY = Convert.ToDouble(axisY[0]),
                    endY   = Convert.ToDouble(axisY[1]),
                    stepY  = Convert.ToDouble(axisY[2]);

                Console.WriteLine("Ось X: начало промежутка = {0}; конец = {1}; шаг = {2}", startX, endX, stepX);
                Console.WriteLine("Ось Y: начало промежутка = {0}; конец = {1}; шаг = {2}", startY, endY, stepY);

                sw.Start();
                List<double> maxResultsList = GetValuesXY(startX, endX, stepX, startY, endY, stepY); //получили все максимумы на оси XY
                double max = GetMax(maxResultsList); //Максимум с оси X
                sw.Stop();
                Console.WriteLine("Максимум на текущем отрезке: {0}", max);
                Console.WriteLine("Длительность выполнения расчётов (сек.): " + sw.Elapsed.TotalSeconds);

                //отправляем сообщение серверу
                SendMessage(stream, max);
                Console.WriteLine("Данные отправлены серверу на дальнейшую обработку");

                // Закрываем потоки
                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

            Console.WriteLine("Запрос завершен...");
            Console.Read();
        }


        private static string GetMessage(NetworkStream stream)
        {
            byte[] data = new byte[256];
            StringBuilder getMessage = new StringBuilder();
            //чтение записи
            do
            {
                int bytes = stream.Read(data, 0, data.Length);
                getMessage.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable); // пока данные есть в потоке
            return getMessage.ToString();
        }

        private static void SendMessage(NetworkStream stream, double max)
        {
            byte[] data = new byte[256];
            //отправка записи
            string message = max.ToString();
            data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);

        }

        private static string[] GetStringData(string message, char[] charsets)
        {
            string[] messageOne = message.Split(charsets);
            return messageOne;
        }

       private static List<string> GetStringTrim(string[] message)
        {
            List <string> strList = new List<string>();
            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] != "")
                    strList.Add(message[i]);
            }
            return strList;
        }

        //Вычисления
        private static double GetMax(List<double> array)
        {
            return double.IsNaN(array[0]) ? 0 : array.Max();
        }


        private static List<double> GetValuesXY(double startX, double endX, double stepX, double startY, double endY, double stepY)
        {
            //Вместо Parellel.For, потому что нельзя указать свой шаг
            //Задействует все ядра процессора распиливание задачи
            //SteppedIterator (Enumerable<double>) перечислитель для дипазона, 
            //возвращает текущее значение в диапазоне Х здесь как index

            var valuesYX = new List<double>();
            object localLockObject = new object();

            System.Threading.Tasks.Parallel.ForEach(
               SteppedIterator((double)startX, (double)endX, stepX),
               () => new List<double>(),
               (index, state, localList) =>
               {
                   //double max = GetMax(GetValuesY(startY, endY, stepY, index));
                   //localList.Add(max);
                   localList.Add(GetMax(GetValuesY(startY, endY, stepY, index)));
                   if (localList.Count > 1000)
                   {
                       //max = GetMax(localList);
                       double max = GetMax(localList);
                       localList.Clear();
                       localList.TrimExcess();
                       localList.Add(max);
                   }
                   return localList; //с каждой итерацией увеличиваем localList, закидываем текущий диапазон X и считаем его на промежутке
               },
               (finalResult) => { lock (localLockObject) valuesYX.AddRange(finalResult); }
               );
            return valuesYX;
        }

        private static IEnumerable<double> SteppedIterator(double startIndex, double endIndex, double stepSize)
        {
            for (double i = startIndex; i < endIndex; i = i + stepSize)
            {
                yield return i;
            }
        }

        private static List<double> GetValuesY(double startY, double endY, double stepY, double valueX)
        {
            List<double> valuesY = new List<double>();
            for (double y = startY; y <= endY; y += stepY)
                valuesY.Add(GetValueFunc(valueX, y));
            return valuesY;
        }

        private static double GetValueFunc(double x, double y)
        {
            return x * x + 2 * y;
        }
    }
}