using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;

namespace lr_1_3_TRSPO
{
    public class lr_1_3_TRSPO
    {
        private static void Main(string[] args)
        {
            
           Stopwatch sw = new Stopwatch();
            
            Console.WriteLine(
                "Поиск наибольшего значения функции нескольких переменных методом сканирования с заданным шагом");
            BigInteger startX, endX, startY, endY;
            double stepX, stepY, max;
            double[] arrayValues;
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
                    !double.TryParse(valueDoubleY, NumberStyles.Any, CultureInfo.InvariantCulture, out stepY)   )                    
                    Console.WriteLine("Ошибка ввода дробного числа!");
               else {
                    sw.Start();//Проверяем длительность выполнения расчётов
                    arrayValues = new double[(ulong)GetSizeArray(startX, endX, stepX, startY, endY, stepY)];
                    arrayValues = GetArrayValues(startX, endX, stepX, startY, endY, stepY);
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

        private static double GetMax(double[] array)
        {
            double max = array[0];
            for (BigInteger i = 0; i < array.Length; i++)
            {
                if (max < array[(ulong)i])
                    max = array[(ulong)i];
            }
            return max;
        }

        private static double[] GetArrayValues(BigInteger startX, BigInteger endX, double stepX, BigInteger startY, BigInteger endY, double stepY)
        {
            BigInteger countValues = GetSizeArray(startX, endX, stepX, startY, endY, stepY);
            double[] values = new double[(ulong)countValues];
            BigInteger k = 0;
            //пример: начальная точка
            //start = -2;
            //i = -2; i <= 10; i += 0.3;
            for (double i = (double) startX; i <= (double) endX; i += stepX)
                for (double j = (double)startY; j <= (double)endY; j += stepY)
                    values[(ulong)k++] = GetValueFunc(i,j);
            return values;
        }

        private static double GetValueFunc(double i, double j)
        {
            return i*i + 2*j;
        }

        private static BigInteger GetSizeArray(BigInteger startX, BigInteger endX, double stepX, BigInteger startY, BigInteger endY, double stepY)
        {
            return (BigInteger)(((double)endX - (double)startX)/ stepX + 1.0) * (BigInteger)(((double)endY - (double)startY) / stepY + 1.0);
        }

    }
}