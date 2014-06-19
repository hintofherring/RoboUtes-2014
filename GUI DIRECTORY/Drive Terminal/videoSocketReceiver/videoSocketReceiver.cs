using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Threading;

namespace videoSocketToolsV2
{
    public class videoSocketReceiver
    {
        private NetworkStream NetStream;
        //private int currentFrameSize = -1;
        private TcpClient TCPClient;
        private int port;
        TcpListener listener;
        private bool connected = false;
        connectionEstablished connectionEstablishedHandler;

        public delegate void frameReceivedHandler(byte[] newFrame);
        public event frameReceivedHandler frameReceived;

        public delegate void connectionLostEventHandler();
        public event connectionLostEventHandler connectionLost;

        public delegate void connectionEstablished(bool connectionStatus);

        private int debugCount = 0;

        Thread worker;

        public videoSocketReceiver(int _Port, connectionEstablished _connectionEstablishedHandler)
        {
            connectionEstablishedHandler = _connectionEstablishedHandler;
            port = _Port;
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            listener.BeginAcceptTcpClient(connectionCallback, connectionEstablishedHandler);
        }

        public void close()
        {
            if (TCPClient != null)
            {
                TCPClient.Close();
            }
            if (listener != null)
            {
                listener.Stop();
            }
        }

        private void connectionCallback(IAsyncResult ar)
        {
            if (!connected)//only one connection at a time...
            {
                TCPClient = listener.EndAcceptTcpClient(ar);
                NetStream = TCPClient.GetStream();
                connected = true;
                startReceiving();
            }
            else
            {
                connected = false;
            }
            connectionEstablished state = (connectionEstablished)ar.AsyncState;
            if (state != null)
            {
                state(connected);
            }
        }

        private bool startReceiving()
        {
            if (TCPClient.Connected)
            {
                if (worker != null)
                {
                    worker.Abort();
                }
                worker = new Thread(new ThreadStart(receive));
                worker.Start();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void receive(){
            while (true)
            {
                beginReceive();
            }
        }

        private void beginReceive()
        {
            bool parsing = false;
            try
            {
                int currentFrameSize = -1;
                while (!parsing)
                {
                    byte[] tempBuffer = new byte[5] { 0, 0, 0, 0, 0 }; //start with zero value.

                    NetStream.Read(tempBuffer, 0, tempBuffer.Length);
                    string sizeTagcandidate = Encoding.UTF8.GetString(tempBuffer);
                    if (sizeTagcandidate == "SIZE:") //look for the "SIZE:" tag
                    {
                        string size = "";
                        while (true)
                        {
                            byte[] tempSizeBuffer = new byte[1];
                            NetStream.Read(tempSizeBuffer, 0, 1);
                            string received = Encoding.UTF8.GetString(tempSizeBuffer);
                            if (received != "Z") //"Z" is the delimiter that specifies the end of the size parameter
                            {
                                size += received;
                            }
                            else
                            {
                                break;
                            }
                        }
                        int parsedSize;
                        if (!int.TryParse(size, out parsedSize))
                        {
                            Console.WriteLine("Unable to parse supposed size parameter, this might break stuff, but HOPEFULLY another beginReceive will be called to fix stuff...");
                            break;
                        }
                        NetStream.Flush();
                        parsing = true;
                        currentFrameSize = parsedSize;
                        break;
                    }
                }

                if (parsing)
                {
                    debugCount++;
                    byte[] currentFrameBuffer = new byte[currentFrameSize];
                    List<byte> tempBuffer = new List<byte>();
                    while (tempBuffer.Count < currentFrameSize)
                    {
                        int readSize = NetStream.Read(currentFrameBuffer, 0, currentFrameSize - tempBuffer.Count); //by reading currentFrameSize-tempBuffer.Count you only read up to the possible number of bytes remaining in the image. This keeps you from reading into the next image
                        tempBuffer.AddRange(currentFrameBuffer.Take(readSize).ToArray());
                    }

                    if (frameReceived != null)
                    {
                        frameReceived(tempBuffer.ToArray());
                    }
                    currentFrameBuffer = null;
                    tempBuffer = null;

                    parsing = false;
                    currentFrameSize = -1;
                }
                //beginReceive(); //TODO: Commented out to just reaceive a frame at a time
            }
            catch
            {
                if (connectionLost != null)
                {
                    connectionLost();
                }
            }
        }
    }
}
