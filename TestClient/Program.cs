using System;
using System.Net;
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

            //IPHostEntry ipHostInfo = Dns.GetHostEntry(pc);
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("192.168.14.23"), 11000);

            Console.WriteLine(CoreClient.MessageSender.Test("TEST<EOF>", localEndPoint));
           
            Console.WriteLine("Comando enviado correctamente");
            Console.ReadKey();
        }
    }
}

