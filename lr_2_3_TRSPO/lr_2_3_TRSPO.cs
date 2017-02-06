using System;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

namespace lr_2_3_TRSPO
{
    public class lr_1_3_TRSPO
    {
        private static void Main(string[] args)
        {
            //таймер 
            Stopwatch sw = new Stopwatch();
            
            Console.WriteLine(
                "Поиск наибольшего значения функции нескольких переменных методом сканирования с заданным шагом");
            Console.WriteLine("Введите: нач. значение, кон. значение и шаг: ");
            int start, end;
            double step, max;
            double[] arrayValues;
            try
            {
                start = Convert.ToInt32(Console.ReadLine().Trim());
                end = Convert.ToInt32(Console.ReadLine().Trim());
                string valueDouble = Console.ReadLine().Trim();
                if (!double.TryParse(valueDouble, NumberStyles.Any, CultureInfo.InvariantCulture, out step))
                    Console.WriteLine("Ошибка ввода дробного числа: ");
                else
                {
                    //первый поток
                    Thread threadGetSizeArray = new Thread(delegate() {
                        arrayValues = new double[GetSizeArray(start, end, step)];
                    });
                    //второй поток
                    Thread threadGetArrayValues = new Thread(delegate()
                    {
                       arrayValues = GetArrayValues(start, end, step);
                    });
                    Thread threadGetMax= new Thread(delegate()
                    {
                        
                        max = GetMax(arrayValues);
                    });
                    
                    threadGetSizeArray.Priority = ThreadPriority.Highest;
                    threadGetArrayValues.Priority = ThreadPriority.AboveNormal;
                    threadGetMax.Priority = ThreadPriority.Normal;

                    sw.Start();
                    threadGetSizeArray.Start();
                    threadGetSizeArray.Join();

                    threadGetArrayValues.Start();
                    threadGetArrayValues.Join();
                   
                    threadGetMax.Start();
                    threadGetMax.Join();
                    
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
            for (int i = 0; i < array.Length; i++)
            {
                if (max < array[i])
                    max = array[i];
            }
            return max;
        }

        private static double[] GetArrayValues(int start, int end, double step)
        {
            int countValues = GetSizeArray(start, end, step);
            double[] values = new double[countValues];
            int j = 0;
            //пример: начальная точка
            //start = -2;
            //i = -2; i <= 10; i += 0.3;
            for (double i = (double)start; i <= (double)end; i += step)
                values[j++] = GetValueFunc(i);
            return values;
        }

        private static double GetValueFunc(double i)
        {
            return i * i + 2 * i;
        }

        private static int GetSizeArray(int start, int end, double step)
        {
            return (int)((((double)end - (double)start) / step) + 1.0);
        }

    }
}