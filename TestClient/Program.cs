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
            int total = 1000;

            for (int i = 0; i < total; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    CoreClient.MessageSender.Test();
                });

                Task.Factory.ContinueWhenAll(tasks, t =>
                {
                    var dateEnd = DateTime.Now;
                    var timeElapsed = (dateEnd - dateStart).TotalMilliseconds;
                    Console.WriteLine("[" + DateTime.Now + "] - Time elapsed: " + timeElapsed);
                });

                Console.ReadKey();
            }
        }
    }
}
