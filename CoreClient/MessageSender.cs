using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CoreClient
{
    public static class MessageSender
    {
        // TODO : Resolve this.

        public static string Test(string comando, IPEndPoint ep)
        {
            return Send(comando, ep);
        }

        public static string Send(string Message, IPEndPoint ipAndPort)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAndPort.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(ipAndPort);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes($"{Message}<EOF>");

                    // Send the data through the socket.  
                    int bytesSent = sender.Send(msg);

                    // Receive and return response.
                    return MessageReceiver.Receive(sender);
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    throw;
                    // TODO : LOGGEAR TODAS ESTAS EXCEPTION.
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    throw;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }


    }
}
