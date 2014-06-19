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
using System.Threading;

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
        private volatile bool camOn = false;

        byte[] jpegToSend;

        private TcpClient tcpClient;

        private volatile bool transmitRequested = false;
        Timer fullfilTransmitTimer;

        private int _quality = 7;   //TODO: This value is temporary. It should be adjsutable and probably start at 0. 7 just happens to be the best the rocketfish can give.
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

            _quality = 5;  //TODO: This sets it to lowest quality, i doubt we want that every time... to the right is code for the second highest, but it crashes on bad cameras with few snapshot choices... can be fixed easily... But it usually gets rid of the blob       videoCapabilities.Length - 2;

            videoDevice.VideoResolution = videoCapabilities[0];
            videoDevice.ProvideSnapshots = true;
            videoDevice.SnapshotResolution = snapshotCapabilitites[_quality]; //
            videoDevice.SnapshotFrame += videoDevice_SnapshotFrame;
            VSP.VideoSource = videoDevice; //Idk why this has to happen to get snapshots, but it does...
            fullfilTransmitTimer = new Timer(timerCallback, null, Timeout.Infinite, Timeout.Infinite);
        }

        private void timerCallback(object state)
        {
            if (transmitRequested)
            {
                videoDevice.SimulateTrigger();
            }
            else
            {
                fullfilTransmitTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        public void transmitSnapshot(IPAddress IP, int _port)
        {
            transmitRequested = true;
            if (!camOn)
            {
                camOn = true;
                videoDevice.Start();

                transmit = true;
                target = IP;
                port = _port;
            }
            videoDevice.SimulateTrigger();
            fullfilTransmitTimer.Change(1000, 1500);
        }

        private void transmitCallback(IAsyncResult ar)
        {
            videoDevice.Stop();
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
                            NetworkStream NS = tcpClient.GetStream();
                            NS.Write(jpegToSend, 0, jpegToSend.Length);
                            NS.Flush();
                            NS.Dispose();
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
            transmitRequested = false;
            fullfilTransmitTimer.Change(Timeout.Infinite, Timeout.Infinite);
            camOn = false;
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



    public class snapShotReceiver {
        public delegate void snapShotReceivedEventHandler(byte[] receivedImage);
        public event snapShotReceivedEventHandler newSnapShotReceived;

        private TcpListener listener;
        private int port;

        public snapShotReceiver(int _port) {
            port = _port;
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            listener.BeginAcceptTcpClient(newConnectionCallback, null);
        }

        private void newConnectionCallback(IAsyncResult ar) {
            TcpClient snapShotSource = listener.EndAcceptTcpClient(ar);
            byte[] buffer = new byte[200000];
            object[] holder = new object[] { buffer, snapShotSource };
            NetworkStream NS = snapShotSource.GetStream();
            NS.BeginRead(buffer, 0, buffer.Length, snapShotReceivedCallback, holder);
        }

        private void snapShotReceivedCallback(IAsyncResult ar) {
            object[] holder = (object[])ar.AsyncState;
            byte[] receiveBuffer = (byte[])holder[0];
            if (newSnapShotReceived != null) {
                newSnapShotReceived(receiveBuffer);
            }
            TcpClient oldClient = (TcpClient)holder[1];
            oldClient.Client.Close(0);
            oldClient.Client.Dispose();
            oldClient.Close();
            listener.BeginAcceptTcpClient(newConnectionCallback, null);
        }
    }
}
