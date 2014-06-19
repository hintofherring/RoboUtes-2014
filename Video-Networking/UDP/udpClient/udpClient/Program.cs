using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace udpClient
{
    class Program
    {
        static UdpClient uclient;
        static void Main(string[] args)
        {
            byte[] toSend = Encoding.UTF8.GetBytes("CAT");
            uclient = new UdpClient();
            Console.WriteLine("trying to connect to server");
            uclient.Connect("50.168.197.42", 45000);
           // while (true){
                uclient.Send(toSend, toSend.Length);
           // }
            Console.WriteLine("Connected to the server! :)");
            Console.Read();
        }
    }
}
