using System;
using MPI;
namespace TRSPO_5_11
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "Андрей",
                   surname = "Когут";
            //M and K - это номера процессов 
            int M = 2,  //for name
                K = 0;  //for surname
            using (new MPI.Environment(ref args))
            {
                if (MPI.Communicator.world.Rank == M || MPI.Communicator.world.Rank == K)
                {
                    if (MPI.Communicator.world.Rank == K) 
                    {
                        Console.WriteLine("[Процесс " + MPI.Communicator.world.Rank + "] Сообщение: " + surname); //display surname
                    }
                    if (MPI.Communicator.world.Rank == M)
                    {
                        Console.WriteLine("[Процесс " + MPI.Communicator.world.Rank + "] Сообщение: " + name); //display name
                    }
                }
                else
                {
                    Console.WriteLine("[Процесс " + MPI.Communicator.world.Rank + "] Сообщение: false"); //display other
                }
                Console.ReadKey();
            }
            
        }
    }
}
