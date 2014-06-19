 using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
 
namespace testSendImageV2
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
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 5. loop to accept client connection requests
            byte[] buffer1 = ReadImageFile("1.jpg");
            byte[] buffer2 = ReadImageFile("2.jpg");
            int frameCount = 0;
            while (true)
            {
                
                Console.WriteLine("start listening");
                
                try
                {
                    //clientSocket.Connect("155.98.5.180", 20000);
                    clientSocket = sListen.Accept();
                }
                catch
                {
                    throw;
                }

                // send the file
                int counter = 0;
                while (true) {
                    counter++;
                    //byte[] buffer = ReadImageFile(counter+".jpg");
                    //string sizeString = "SIZE:" + buffer.Length+"Z";
                    //Console.WriteLine("Size String: " + sizeString);
                    if (counter == 1)
                    {
                        string sizeString = "SIZE:" + buffer1.Length + "Z";
                        byte[] startMessage = Encoding.ASCII.GetBytes(sizeString);
                        clientSocket.Send(startMessage, startMessage.Length, SocketFlags.None);
                        clientSocket.Send(buffer1, buffer1.Length, SocketFlags.None);
                    }
                    else
                    {
                        string sizeString = "SIZE:" + buffer2.Length + "Z";
                        byte[] startMessage = Encoding.ASCII.GetBytes(sizeString);
                        clientSocket.Send(startMessage, startMessage.Length, SocketFlags.None);
                        clientSocket.Send(buffer2, buffer2.Length, SocketFlags.None);
                    }
                    frameCount++;
                    Console.WriteLine("counter: " + counter);
                    Console.WriteLine("frameCount: " + frameCount);
                    
                    //clientSocket.Send(buffer, buffer.Length, SocketFlags.None);
                    
                    Console.WriteLine("Send success!");
                    if (counter == 2)
                    {
                        counter = 0;
                    }
                }
                clientSocket.Close();
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