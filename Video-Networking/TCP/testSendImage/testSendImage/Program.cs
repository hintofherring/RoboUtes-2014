 using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
 
namespace testSendImage
{
    class Program
    {
        static void Main (String[] args)
        {
            // 1. to create a socket
            Socket sListen = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
 
            // 2. Fill IP
            IPAddress IP = IPAddress.Parse ("127.0.0.1");
            IPEndPoint IPE = new IPEndPoint (IP, 4321);
 
            // 3. binding
            sListen.Bind (IPE);
 
            // 4. Monitor
            Console.WriteLine ("Service is listening ...");
            sListen.Listen (2);
            Socket clientSocket;
            // 5. loop to accept client connection requests
            while (true)
            {
                Console.WriteLine("start listening");
                
                try
                {
                    clientSocket = sListen.Accept ();
                }
                catch
                {
                    throw;
                }
 
                // send data to the client
                //clientSocket.Send (Encoding.Unicode.GetBytes ("I am a server, you there?? !!!!"));
 
                // send the file
                byte[] buffer = ReadImageFile ("1.jpg");
                clientSocket.Send (buffer, buffer.Length, SocketFlags.None);
                Console.WriteLine ("Send success!");
            }
        }

        private static byte[] ReadImageFile(String img)
        {
            FileInfo fileinfo = new FileInfo(img);
            byte[] buf = new byte[fileinfo.Length];
            FileStream fs = new FileStream(img, FileMode.Open, FileAccess.Read);
            fs.Read(buf, 0, buf.Length);
            fs.Close();
            //fileInfo.Delete ();
            GC.ReRegisterForFinalize(fileinfo);
            GC.ReRegisterForFinalize(fs);
            return buf;
        }

    }
}