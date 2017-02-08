using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace lr_3_3_TRSPO
{
    public class lr_3_3_TRSPO
    {
        private static void Main(string[] args)
        {

            Stopwatch sw = new Stopwatch();

            Console.WriteLine(
                "Поиск наибольшего значения функции нескольких переменных методом сканирования с заданным шагом");
            BigInteger startX, endX, startY, endY;
            double stepX, stepY, max;
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
                               // arrayValues = new double[GetSizeArray(startX, endX, stepX, startY, endY, stepY)];
                    arrayValues = GetValuesXY(startX, endX, stepX, startY, endY, stepY);
                    max = GetMax(arrayValues);
                    Console.WriteLine("Наибольшее значение функции: " + max);
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
            double max = double.IsNaN(array[0]) ? 0 : array[0];
            for (int i = 0; i < array.Count(); i++)
            {
                if (max < array[i])
                    max = array[i];
            }
            return max;
        }

        /*private static List<double> GetArrayValues(BigInteger startX, BigInteger endX, double stepX, BigInteger startY, BigInteger endY, double stepY)
        {
            List<double> values = new List<double>();
            //for (double x = (double)startX; x <= (double)endX; x += stepX)
            //   values.AddRange(GetValuesY(startY, endY, stepY, x)); //этот выполниться за 3,1531478  
            values.AddRange(GetValuesXY(startX, endX, stepX, startY, endY, stepY)); // этот выполниться  за 3,2830899
           
            return values;
        }*/

        private static List<double> GetValuesXY(BigInteger startX, BigInteger endX, double stepX, BigInteger startY, BigInteger endY, double stepY)
        {
            List<double> valuesYX = new List<double>();
            for (double x = (double)startX; x <= (double)endX; x += stepX)
                valuesYX.AddRange(GetValuesY(startY, endY, stepY, x));
            return valuesYX;
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

        /* private static ulong GetSizeArray(BigInteger startX, BigInteger endX, double stepX, BigInteger startY, BigInteger endY, double stepY)
         {
             return (ulong)(((double)endX - (double)startX) / stepX + 1.0) * (ulong)(((double)endY - (double)startY) / stepY + 1.0);
         }*/
    }
}