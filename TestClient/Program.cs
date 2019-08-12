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

            IPHostEntry ipHostInfo = Dns.GetHostEntry("IT-MARCOS");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            for (int i = 0; i < 5000; i++)
            {
                Console.WriteLine(CoreClient.MessageSender.Test($"{i}<EOF>", localEndPoint));
            }

            Console.WriteLine("Comando enviado correctamente");
            Console.ReadKey();
        }
    }
}

