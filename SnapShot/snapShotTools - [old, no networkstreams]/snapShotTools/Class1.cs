using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace snapShotTools
{
    public class snapShotSender
    {
        private VideoCaptureDevice videoDevice;
        private VideoCapabilities[] videoCapabilities;
        private VideoCapabilities[] snapshotCapabilitites;
        private VideoSourcePlayer VSP;

        private IPAddress target;
        private int port;
        private Bitmap currentFrame;
        private object frameSync = 1;
        private volatile bool transmit = false;
        private volatile bool transmitInProgress = false;

        byte[] jpegToSend;

        private TcpClient tcpClient;

        private int _quality = 0;
        /// <summary>
        /// set from 0 - 100
        /// </summary>
        public double Quality
        {
            set
            {
                _quality = (int)((value / 100.00) * snapshotCapabilitites.Length);
                videoDevice.SnapshotResolution = snapshotCapabilitites[_quality];
            }
        }

        public snapShotSender(VideoCaptureDevice VCD)
        {
            videoDevice = VCD;
            VSP = new VideoSourcePlayer();
            videoCapabilities = videoDevice.VideoCapabilities;
            snapshotCapabilitites = videoDevice.SnapshotCapabilities;

            videoDevice.VideoResolution = videoCapabilities[0];
            videoDevice.ProvideSnapshots = true;
            videoDevice.SnapshotResolution = snapshotCapabilitites[_quality];
            videoDevice.SnapshotFrame += videoDevice_SnapshotFrame;
            VSP.VideoSource = videoDevice; //Idk why this has to happen to get snapshots, but it does...
            videoDevice.Start();
        }

        public void transmitSnapshot(IPAddress IP, int _port)
        {
            transmit = true;
            target = IP;
            port = _port;
            videoDevice.SimulateTrigger();
        }

        private void transmitCallback(IAsyncResult ar)
        {
            if (transmit)
            {
                transmit = false;
                try
                {
                    tcpClient.EndConnect(ar);
                    if (tcpClient.Connected)
                    {
                        lock (frameSync)
                        {
                            //byte[] toSend = Bitmap2JpegArray(currentFrame);
                            int t = tcpClient.Client.Send(jpegToSend, jpegToSend.Length, SocketFlags.None);
                            if (t != jpegToSend.Length)
                            {
                                Console.WriteLine("Partial send!!!");
                            }
                            else
                            {
                                Console.WriteLine("Succesful send");
                            }
                            tcpClient.Client.Close(0);
                            tcpClient.Client.Dispose();
                            tcpClient.Close();
                            transmitInProgress = false;
                        }
                    }
                }
                catch
                {
                    //do nothing
                    return;
                }
            }
        }

        void videoDevice_SnapshotFrame(object sender, NewFrameEventArgs eventArgs)
        {
            lock (frameSync)
            {
                MemoryStream ms = new MemoryStream();
                eventArgs.Frame.Save(ms, ImageFormat.Jpeg);
                jpegToSend = ms.ToArray();
            }
            if (transmit && !transmitInProgress)
            {
                transmitInProgress = true;
                tcpClient = new TcpClient();
                tcpClient.BeginConnect(target, port, transmitCallback, null);
            }
        }

        private byte[] Bitmap2JpegArray(Bitmap Frame)
        {
            Bitmap temp = Frame;
            MemoryStream ms = new MemoryStream();

            temp.Save(ms, ImageFormat.Jpeg);
            byte[] toReturn = ms.ToArray();
            return toReturn;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }



    public class snapShotReceiver
    {
        public delegate void snapShotReceivedEventHandler(byte[] receivedImage);
        public event snapShotReceivedEventHandler newSnapShotReceived;

        private TcpListener listener;
        private int port;

        public snapShotReceiver(int _port)
        {
            port = _port;
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            listener.BeginAcceptTcpClient(newConnectionCallback, null);
        }

        private void newConnectionCallback(IAsyncResult ar)
        {
            TcpClient snapShotSource = listener.EndAcceptTcpClient(ar);
            byte[] buffer = new byte[5000000];
            snapShotSource.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, snapShotReceivedCallback, buffer);
        }

        private void snapShotReceivedCallback(IAsyncResult ar)
        {
            byte[] receiveBuffer = (byte[])ar.AsyncState;
            if (receiveBuffer[0] != null && newSnapShotReceived != null)
            {
                newSnapShotReceived(receiveBuffer);
            }
        }
    }
}
