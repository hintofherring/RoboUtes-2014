using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
 
namespace testReceiveImage
{
    class Program
    {
        static void Main (String[] args)
        {
            // 1. to create a socket
            Socket S = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
 
            // 2. fill in the remote IP
            IPAddress IP = IPAddress.Parse ("127.0.0.1");
            IPEndPoint IPE = new IPEndPoint (IP, 4321);
 
            Console.WriteLine ("started connection service ....");
            // 3. connect to the server
            S.Connect(IPE);
 
            // 4. receive data
            byte[] buffer = new byte[1000000];
            S.Receive(buffer, buffer.Length, SocketFlags.None);
            //var Msg = Encoding.Unicode.GetString (buffer);
            //Console.WriteLine ("received message: (0)", msg);
            Console.WriteLine ("Receive success");
 
            FileStream fs =  File.Create ("received.jpg");
            fs.Write (buffer, 0, buffer.Length);
            fs.Close ();

            byte[] buffer2 = new byte[1000000];
            S.Receive(buffer2, buffer2.Length, SocketFlags.None);
            //var Msg = Encoding.Unicode.GetString (buffer);
            //Console.WriteLine ("received message: (0)", msg);
            Console.WriteLine("Receive success 2");

            FileStream fs2 = File.Create("received2.jpg");
            fs2.Write(buffer2, 0, buffer2.Length);
            fs2.Close();
 
            Console.ReadKey ();
        }
    }
}