using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CoreClient
{
    public class MessageReceiver
    {
        public static string Receive(Socket socket)
        {
            byte[] bytes = new byte[1024];

            // Receive the response from the remote device.  
            int bytesRec = socket.Receive(bytes);

            // Get content using ASCII encoding.
            string contentRec = Encoding.ASCII.GetString(bytes, 0, bytesRec);

            // Release the socket.  
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            // Return the received content.
            return contentRec;
        }
    }
}
