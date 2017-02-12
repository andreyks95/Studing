using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;
using System.Threading;

namespace lr_7_3_TRSPO
{
    class Example
    {

        static void Main(string[] args)
        {
            double msgFrom5To1 = 4.4;
            string msgFrom1To2 = "Где, укажите мне, отечества отцы?";
            double msgFrom2To5first = -0.3;
            int msgFrom2To5second = 7;


            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                if (comm.Size == 6)
                {
                    if (comm.Rank == 5)
                    {
                        Console.WriteLine("\n-------------------------Начало Процесс 5-------------------------");
                        comm.Send<double>(msgFrom5To1, 1, 42);
                        Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 1 сообщение " + msgFrom5To1);
                        Console.WriteLine("KOLVO: " + comm.Size);
                        double receiveFirst;
                        comm.Receive<double>(2, 42, out receiveFirst);
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение " + receiveFirst);
                        int receiveSecond;
                        comm.Receive<int>(2, 42, out receiveSecond);
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение " + receiveSecond);

                        Console.WriteLine("-------------------------Конец Процесс 5--------------------------\n");
                    }
                    if (comm.Rank == 2)
                    {
                        Console.WriteLine("\n-------------------------Начало Процесс 2-------------------------");
                        comm.Send<double>(msgFrom2To5first, 5, 42);
                        Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 5 сообщение " + msgFrom2To5first);
                        comm.Send<int>(msgFrom2To5second, 5, 42);
                        Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 5 сообщение " + msgFrom2To5second);

                        string receive;
                        comm.Receive<string>(1, 42, out receive);
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение " + receive);
                        Console.WriteLine("-------------------------Конец Процесс 2--------------------------\n");
                    }

                    if (comm.Rank == 1)
                    {
                        Console.WriteLine("\n-------------------------Начало Процесс 1-------------------------");
                        comm.Send<string>(msgFrom1To2, 2, 42);
                        Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 2 сообщение " + msgFrom1To2);

                        double receive;
                        comm.Receive<double>(5, 42, out receive);
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение " + receive);
                        Console.WriteLine("-------------------------Конец Процесс 1--------------------------\n");
                    }
                }
                else
                {
                    Console.WriteLine("Количество процессов != 6. Тупиковая ситуация");
                }
            }
        }
    }
}
