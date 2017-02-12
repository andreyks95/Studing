using System;
using MPI;
namespace lr_7_3_TRSPO
{
    public class Fail
    {
        static void Main(string[] args)
        {
            double msg3to5 = 7.875;
            int msg5to2first = 87, msg5to3 = 5;
            string msg5to2second = "Каждой принцессе положен палач.";


            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;

                /*
                if (!(comm.Rank == 2) || !(comm.Rank == 3) || !(comm.Rank == 5))
                {
                    //принимаем в 5 процессе сообщение с 3 процесса 5
                    double[] receiveFor3 = new double[3];
                    comm.ImmediateReceive(3, 42, receiveFor3);
                    foreach (var i in receiveFor3)
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + i);

                    //принимаем в 3 процессе сообщение с 5 процесса 5
                    int[] receive = new int[3];
                    comm.ImmediateReceive(5, 42, receive);
                    foreach (var i in receive)
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + i);

                    //принимаем в 2 процессе сообщение с 5 процесса 85
                    int[] receiveFor2first = new int[3];
                    comm.ImmediateReceive(5, 42, receiveFor2first);
                    foreach (var i in receiveFor2first)
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + i);

                    //принимаем в 2 процессе сообщение с 5 процесса "Каждой принцессе положен палач."
                    string[] receiveFor2second = new string[3];
                    comm.ImmediateReceive(5, 42, receiveFor2second);
                    foreach (var i in receiveFor2second)
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + i);

                }

                else
                    Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: false");

                //Сообщения для 2 процесса

                //отправляем сообщение с 5 процесса к 2 процессу 87
                comm.ImmediateSend(msg5to2first, 2, 42);
                Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 2 сообщение: " +
                                  msg5to2first);
                //отправляем сообщение с 5 процесса к 2 процессу "Каждой принцессе положен палач."
                comm.ImmediateSend(msg5to2second, 2, 42);
                Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 2 сообщение: " +
                                  msg5to2second);

                //сообщения для 3 процесса

                //отправляем сообщение с 5 процесса к 3 процессу 5
                comm.ImmediateSend(msg5to3, 3, 42);
                Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 3 сообщение: " +
                                  msg5to3);


                //отправляем сообщение с 3 процесса к 5 процессу 7.875
                comm.ImmediateSend(msg3to5, 5, 42);

                Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 5, сообщение: " +
                                  msg3to5);


                if ((comm.Rank == 2) || (comm.Rank == 3) || (comm.Rank == 5))
                {
                    //принимаем в 5 процессе сообщение с 3 процесса 5
                    double[] receiveFor3 = new double[3];
                    comm.ImmediateReceive(3, 42, receiveFor3);
                    foreach (var i in receiveFor3)
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + i);

                    //принимаем в 3 процессе сообщение с 5 процесса 5
                    int[] receive = new int[3];
                    comm.ImmediateReceive(5, 42, receive);
                    foreach (var i in receive)
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + i);

                    //принимаем в 2 процессе сообщение с 5 процесса 85
                    int[] receiveFor2first = new int[3];
                    comm.ImmediateReceive(5, 42, receiveFor2first);
                    foreach (var i in receiveFor2first)
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + i);

                    //принимаем в 2 процессе сообщение с 5 процесса "Каждой принцессе положен палач."
                    string[] receiveFor2second = new string[3];
                    comm.ImmediateReceive(5, 42, receiveFor2second);
                    foreach (var i in receiveFor2second)
                        Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + i);
                }

                else
                    Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: false");
                */

                
                if ((comm.Rank == 2) || (comm.Rank == 3) || (comm.Rank == 5))
                {
                   
                        //если попали на 5-й процесс
                        if (comm.Rank == 5)
                        {

                        //принимаем в 5 процессе сообщение с 3 процесса 5
                        double[] receiveFor3 = new double[3];
                        comm.ImmediateReceive(3, 42, receiveFor3);
                        foreach (var i in receiveFor3)
                            Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + i);

                        //Сообщения для 2 процесса

                                //отправляем сообщение с 5 процесса к 2 процессу 87
                                comm.Send(msg5to2first, 2, 42);
                                Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 2 сообщение: " +
                                                  msg5to2first);
                                //отправляем сообщение с 5 процесса к 2 процессу "Каждой принцессе положен палач."
                                comm.Send(msg5to2second, 2, 42);
                                Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 2 сообщение: " +
                                                  msg5to2second);

                                //сообщения для 3 процесса

                                //отправляем сообщение с 5 процесса к 3 процессу 5
                                comm.Send(msg5to3, 3, 42);
                                Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 3 сообщение: " +
                                                  msg5to3);
                        }
                        //если попали на 3-й процесс
                        if (comm.Rank == 3)
                        {
                                //принимаем в 3 процессе сообщение с 5 процесса 5
                                int[] receive = new int[3];
                                comm.ImmediateReceive(5, 42, receive);
                                foreach (var i in receive)
                                    Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + i);

                                //отправляем сообщение с 3 процесса к 5 процессу 7.875
                                comm.Send(msg3to5, 5, 42);
                                
                                Console.WriteLine("<-- Процесс " + comm.Rank + " отправляет процессу 5, сообщение: " +
                                                  msg3to5);
                        }
                        //если попали на 2-й процесс
                        if (comm.Rank == 2)
                        {
                                //принимаем в 2 процессе сообщение с 5 процесса 85
                                int[] receiveFor2first = new int[3];
                                comm.ImmediateReceive(5, 42, receiveFor2first);
                                foreach (var i in receiveFor2first)
                                    Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + i);

                                //принимаем в 2 процессе сообщение с 5 процесса "Каждой принцессе положен палач."
                                string[] receiveFor2second = new string[3];
                                comm.ImmediateReceive(5, 42, receiveFor2second);
                                foreach(var i in receiveFor2second)
                                    Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: " + i);
                               
                        }
                }
                else
                    Console.WriteLine("--> Процесс " + comm.Rank + " получает сообщение: false");
            }
        }
    }
}