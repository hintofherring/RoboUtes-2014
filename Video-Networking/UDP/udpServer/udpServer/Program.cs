using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace udpServer
{
    class Program
    {
        static Socket serverSocket;
        static byte[] buffer;
        static void Main(string[] args)
        {
            buffer = new byte[22000];
            serverSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 3500);
            serverSocket.Bind(endpoint);
            Console.WriteLine("Listening on port 3500");
            serverSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, gotData, null);
            Console.Read();
        }

        private static void gotData(IAsyncResult ar)
        {
            int size = serverSocket.EndReceive(ar);
            Console.WriteLine("GOT: "+size+" bytes of data!");
            serverSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, gotData, null);
        }
    }
}
