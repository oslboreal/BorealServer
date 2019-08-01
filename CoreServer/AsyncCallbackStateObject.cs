using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CoreServer
{
    /// <summary>
    /// 
    /// </summary>
    public class AsyncCallbackStateObject
    {
        public const int BufferSize = 1024;
        public byte[] Buffer { get; set; } = new byte[BufferSize];
        public StringBuilder sb { get; set; } = new StringBuilder();
        public Socket Socket { get; set; } = null;
    }
}
