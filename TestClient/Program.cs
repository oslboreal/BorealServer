using System;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var dateStart = DateTime.Now;
            Task[] tasks = new Task[1000];
            int finishedTasks = 0;

            Console.WriteLine("Hello World!");

            CoreClient.MessageSender.Test();
            Console.ReadKey();
        }
    }
}

