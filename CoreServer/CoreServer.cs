using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreServer
{
    /// <summary>
    /// Multithreading server.
    /// </summary>
    public class CoreServer
    {
        #region Singleton
        private static CoreServer proc;
        public static CoreServer Instance { get { if (proc == null) proc = new CoreServer(); return proc; } }
        #endregion

        public delegate void ProcessReceivedRequest(Socket handler, string request);
        public event ProcessReceivedRequest receivedRequestEvent;

        static ManualResetEvent ManualResetEvent { get; set; } = new ManualResetEvent(false);
        static SemaphoreSlim SemaphoreSlim { get; set; } = new SemaphoreSlim(10);

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Components.ConfigurationComponents.ConfigurationComponent.NetworkingConfiguration.Ip);
                IPAddress ipAddress = ipHostInfo.AddressList[0];

                if (ipAddress == null)
                    ipAddress = IPAddress.Parse(Components.ConfigurationComponents.ConfigurationComponent.NetworkingConfiguration.Ip);

                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Components.ConfigurationComponents.ConfigurationComponent.NetworkingConfiguration.Port);
                //LoggingComponent.Log("Server started on " + ipAddress.ToString() + " at port " + localEndPoint.Port, LogType.Succes);

                StartListening(localEndPoint);
            });
        }

        // Incoming data from the client.  
        public static string data = null;

        public static void StartListening(IPEndPoint localEndPoint)
        {
            // Buffer
            byte[] bytes = new Byte[1024];

            // Create a TCP/IP socket.  
            Socket listener = new Socket(localEndPoint.Address.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                // Start listening for connections.  
                while (true)
                {
                    //Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  

                    ManualResetEvent.Reset();

                    SemaphoreSlim.Wait();
                    Socket handler = listener.Accept();
                    SemaphoreSlim.Release();

                    Task.Factory.StartNew(() =>
                    {
                        Socket currentHandler = handler;

                        data = null;

                        // An incoming connection needs to be processed.  
                        while (true)
                        {
                            int bytesRec = currentHandler.Receive(bytes);

                            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                            if (data.IndexOf("<EOF>") > -1)
                                break;
                        }

                        if (string.IsNullOrEmpty(data))
                        {
                            Console.WriteLine("Mensaje vacío");
                            return;
                        }

                        // Show the data on the console
                        Console.WriteLine("Text received : {0}", data);

                        if (data == "TEST<EOF>TEST<EOF>")
                            Console.WriteLine("Mensaje duplicado");

                        // Echo the data back to the client.  
                        byte[] msg = Encoding.ASCII.GetBytes(data);

                        currentHandler.Send(msg);
                        currentHandler.Shutdown(SocketShutdown.Both);
                        currentHandler.Close();
                        ManualResetEvent.Set();
                    });

                    ManualResetEvent.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

    }
}

