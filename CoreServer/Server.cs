using CoreServer.Components;
using CoreServer.Components.ConfigurationComponents;
using System;
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
    public class Server
    {
        #region Singleton
        private static Server proc;
        public static Server Instance { get { if (proc == null) proc = new Server(); return proc; } }
        #endregion

        // # Events
        public delegate void ProcessReceivedRequest(Socket handler, string request);
        public static event ProcessReceivedRequest receivedRequestEvent;

        // # Sync
        static ManualResetEvent ManualResetEvent { get; set; } = new ManualResetEvent(false);
        static SemaphoreSlim SemaphoreSlim { get; set; } = new SemaphoreSlim(Configuration.NetworkingConfiguration.ListeningSockets);

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Configuration.NetworkingConfiguration.Ip);
                IPAddress ipAddress = ipHostInfo.AddressList[0];

                if (ipAddress == null)
                    ipAddress = IPAddress.Parse(Configuration.NetworkingConfiguration.Ip);

                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Configuration.NetworkingConfiguration.Port);
                LoggingComponent.Log($"Server started on {localEndPoint}.", LogType.Succes);

                StartListening(localEndPoint);
            });
        }

        public static void StartListening(IPEndPoint localEndPoint)
        {
            // Create a TCP/IP socket.  
            Socket listener = new Socket(localEndPoint.Address.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(Configuration.NetworkingConfiguration.ListeningSockets);

                // Start listening for connections.
                while (true)
                {
                    ManualResetEvent.Reset();
                    SemaphoreSlim.Wait();
                    Socket handler = listener.Accept();
                    SemaphoreSlim.Release();
                    ProcessRequest(handler);
                    ManualResetEvent.WaitOne();
                }
            }
            catch (Exception e)
            {
                LoggingComponent.LogException(e);
                Console.WriteLine(e.ToString());
            }
        }

        private static void ProcessRequest(Socket handler)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    // Not shared variables.
                    string data = null;
                    byte[] bytes = new Byte[1024];
                    Socket currentHandler = handler;

                    // Fetch content..  
                    while (true)
                    {
                        int bytesRec = currentHandler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }

                    if (string.IsNullOrEmpty(data))
                        throw new ApplicationException("Received data is empty.");

                    // Removes EOF.
                    data = data.Replace("<EOF>", string.Empty);

                    // If the server does'nt have receivedRequestEvent handler, close the communication.
                    if (receivedRequestEvent != null)
                        receivedRequestEvent(handler, data);
                    else
                        Close(handler);

                    ManualResetEvent.Set();
                }
                catch (Exception ex)
                {
                    LoggingComponent.LogException(ex);
                }
            });
        }

        /// <summary>
        /// Sends a message to client.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="data"></param>
        public static void Send(Socket handler, String data)
        {
            // Echo the data back to the client.  
            byte[] msg = Encoding.ASCII.GetBytes(data);
            handler.Send(msg);
        }

        /// <summary>
        /// Closes the socket and the current communication.
        /// </summary>
        /// <param name="handler"></param>
        public static void Close(Socket handler)
        {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        /// <summary>
        /// Sends a response to the client and close the communication.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="data"></param>
        public static void SendAndClose(Socket handler, String data)
        {
            Send(handler, data);
            Close(handler);
        }
    }
}

