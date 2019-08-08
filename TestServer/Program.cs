using CoreServer;
using CoreServer.Components;
using System;
using System.Net.Sockets;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("BorealCoreServer");
            CoreServer.MainProc.Instance.receivedRequestEvent += ProcessReceivedRequest;
            CoreServer.MainProc.Instance.Start();
            Console.ReadKey();
        }

        public static void ProcessReceivedRequest(Socket handler, string request)
        {
            try
            {
                var msg = request.Replace("<EOF>", string.Empty);

                MainProc.Send(handler, "Recibido");

                //if (request.Contains("suspender"))
                    //SuspenderPc().ToString();

                //if (request.Contains("apagar"))
                    //ApagarPc().ToString();

            }
            catch (Exception ex)
            {
                LoggingComponent.Log($"{ex.Message} - Stack: {ex.StackTrace}", LogType.Error);
            }
            finally
            {
                LoggingComponent.Log($"Request received from: {handler}", LogType.Succes);
            }
        }

        public static bool ApagarPc()
        {
            try
            {
                System.Diagnostics.Process.Start("Shutdown", "-s -t 10");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool SuspenderPc()
        {
            try
            {
                System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll, LockWorkStation");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
