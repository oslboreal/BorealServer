using CoreServer.Helpers;
using MessengerService.Requests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreServer
{
    public class MainProc
    {
        #region Singleton
        private static MainProc proc;
        public static MainProc Instance { get { if (proc == null) proc = new MainProc(); return proc; } }
        #endregion

        // Thread signal.  
        private static ManualResetEvent allDone = new ManualResetEvent(false);

        // TODO : Get this out of here, use delegates to allow custom processes.
        public static void ProcessReceivedRequest(Socket handler, string request)
        {
            // Expected processes.

            var testRequest = TestRequest.Deserialize(request);
            if (testRequest != null)
                ProcessTestRequest(testRequest);


            Send(handler, "Received");
        }

        public static void ProcessTestRequest(TestRequest testRequest)
        {
            var output = $"Number: {testRequest.RequestNumber} - Message: {testRequest.Message}";
            LoggingService.Log(output);
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

                // TODO : Validate case, message have not eof tag.
                StartListening(localEndPoint);
            });
        }

        private static void StartListening(IPEndPoint iPEndPoint)
        {
            // Create a TCP/IP socket.  
            Socket listener = new Socket(iPEndPoint.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(iPEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    //Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            AsyncCallbackStateObject state = new AsyncCallbackStateObject();
            state.Socket = handler;
            handler.BeginReceive(state.Buffer, 0, AsyncCallbackStateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        private static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            AsyncCallbackStateObject state = (AsyncCallbackStateObject)ar.AsyncState;
            Socket handler = state.Socket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.Buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the   
                    // client. Display it on the console.  
                    //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                    //content.Length, content);
                    // Echo the data back to the client. 
                    ProcessReceivedRequest(handler, content);
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.Buffer, 0, AsyncCallbackStateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
