using CoreServer.Components;
using System;
using System.Net;
using System.Net.Sockets;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server");

            // Received request handler.
            CoreServer.Server.receivedRequestEvent += ProcessReceivedRequest;

            CoreServer.Server.Instance.Start();

            Console.WriteLine("Server started..");
            Console.ReadKey();
        }

        /// <summary>
        /// Handles the event that is invoked when a request is received by the server.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="request"></param>
        public static void ProcessReceivedRequest(Socket handler, string request)
        {
            try
            {
                MycustomAction(handler, request);

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

                if (handler.Connected)
                    handler.Close();
            }
        }

        /// <summary>
        /// Processing action example.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="receivedMessage"></param>
        /// <returns></returns>
        public static bool MycustomAction(Socket handler, string receivedMessage)
        {
            try
            {
                IPEndPoint remoteIpEndPoint = handler.RemoteEndPoint as IPEndPoint;
                Console.WriteLine($"[{DateTime.Now} from {remoteIpEndPoint}]" + receivedMessage);
                CoreServer.Server.Send(handler, "Recibido");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
