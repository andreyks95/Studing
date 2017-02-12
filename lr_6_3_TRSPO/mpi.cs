using System;
using MPI;

namespace lr_6_3_TRSPO
{
    class mpi
    {

        static void Main(string[] args)
        {
            double msg3to5 = 7.875;
            int    msg5to2first = 87, msg5to3 = 5;
            string msg5to2second = "Каждой принцессе положен палач.";


            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                if ((comm.Rank == 2) || (comm.Rank == 3) || (comm.Rank == 5))
                {
                    //если попали на 5-й процесс
                    if (comm.Rank == 5)
                    {
                        //Сообщения для 2 процесса

                        //отправляем сообщение с 5 процесса к 2 процессу 87
                        comm.Send<int>(msg5to2first, 2, 42);
                        Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 2 сообщение: " +
                                          msg5to2first);

                        //отправляем сообщение с 5 процесса к 2 процессу "Каждой принцессе положен палач."
                        comm.Send<string>(msg5to2second, 2, 42);
                        Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 2 сообщение: " +
                                          msg5to2second);

                        //сообщения для 3 процесса

                        //отправляем сообщение с 5 процесса к 3 процессу 5
                        comm.Send<int>(msg5to3, 3, 42);
                        Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 3 сообщение: " + msg5to3);

                        //принимаем в 5 процессе сообщение с 3 процесса 5
                        double receiveFor3;
                        comm.Receive<double>(3, 42, out receiveFor3);
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + receiveFor3);

                    }

                    //если попали на 3-й процесс
                    if (comm.Rank == 3)
                    {

                        //отправляем сообщение с 3 процесса к 5 процессу 7.875
                        comm.Send<double>(msg3to5, 5, 42);
                        Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 5, сообщение: " + msg3to5);

                        //принимаем в 3 процессе сообщение с 5 процесса 5
                        int receive;
                        comm.Receive<int>(5, 42, out receive);
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + receive);

                    }

                    //если попали на 2-й процесс
                    if (comm.Rank == 2)
                    {

                        //принимаем в 2 процессе сообщение с 5 процесса 85
                        int receiveFor2first;
                        comm.Receive<int>(5, 42, out receiveFor2first);
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + receiveFor2first);

                        //принимаем в 2 процессе сообщение с 5 процесса "Каждой принцессе положен палач."
                        string receiveFor2second;
                        comm.Receive<string>(5, 42, out receiveFor2second);
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + receiveFor2second);

                    }
                }
                else
                    Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: false");
            }
        }
    }
}
