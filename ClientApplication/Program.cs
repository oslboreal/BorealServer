using CoreClient;
using System;

namespace ClientApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter to send.");
            Console.ReadLine();
            MessageSender.Test();
            Console.ReadLine();
        }
    }
}
