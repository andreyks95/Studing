using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace lr_3_3_TRSPO
{
    public class Parallel
    {
        private static void Main(string[] args)
        {

            Stopwatch sw = new Stopwatch();

            Console.WriteLine(
                "Поиск наибольшего значения функции нескольких переменных методом сканирования с заданным шагом");
            BigInteger startX, endX, startY, endY;
            double stepX, stepY;
            List<double> arrayValues = new List<double>();
            try
            {
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
                    arrayValues = GetValuesXY(startX, endX, stepX, startY, endY, stepY);
                    Console.WriteLine("Наибольшее значение функции: " + GetMax(arrayValues));
                    sw.Stop();
                    Console.WriteLine("Длительность выполнения расчётов (сек.): " + sw.Elapsed.TotalSeconds);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.ToString());
            }
            finally
            {
                Console.WriteLine("Завершение работы приложения!");
                Console.ReadKey();
            }

        }

        private static double GetMax(List<double> array)
        {
            return double.IsNaN(array[0]) ? 0 : array.Max();
        }

        //выполняеться на 15% быстрее за 2,8635439 сек
        //Parallel - Предоставляет поддержку параллельных циклов и областей.
        //Пространство имен:   System.Threading.Tasks
        private static List<double> GetValuesXY(BigInteger startX, BigInteger endX, double stepX, BigInteger startY, BigInteger endY, double stepY)
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
                   return  localList; //с каждой итерацией увеличиваем localList, закидываем текущий диапазон X и считаем его на промежутке
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

        private static List<double> GetValuesY(BigInteger startY, BigInteger endY, double stepY, double valueX)
        {
            List<double> valuesY = new List<double>();
            for (double y = (double)startY; y <= (double)endY; y += stepY)
                 valuesY.Add(GetValueFunc(valueX, y));
            return valuesY;
        }

        private static double GetValueFunc(double x, double y)
        {
            return x * x + 2 * y;
        }
    }
}