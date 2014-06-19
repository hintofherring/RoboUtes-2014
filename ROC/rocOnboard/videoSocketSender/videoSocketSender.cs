using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Video;
using AForge.Video.DirectShow;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xaml;

namespace videoSocketTools
{
    public class videoSocketSender
    {
        private VideoCaptureDevice cameraSource;
        private IPAddress IPAddress;
        private int port;
        private TcpClient tcpClient;
        private Socket sendSocket;
        private volatile bool Transmitting = false;
        private long quality = 15; //15% is default quality
        private object qualitySync = 1;
        private volatile bool timeExpired = true; //when expired the frame can be sent (used to control FPS)
        private int FPS = 15;
        private object fpsSync = 1;
        private int framesSent = 0;
        Timer fpsTimer;
        Timer transmittedFPSTimer;

        public int transmissionFPS
        {
            get { return FPS; }
            set {
                if (value < 0)
                {
                    lock (fpsSync)
                    {
                        FPS = 1; //only go down to 1. Otherwise your dividing by zero...
                    }
                }
                else if (value > 1000)
                {
                    lock (fpsSync)
                    {
                        FPS = 1000; //only 1000 ms in a second...
                    }
                }
                else
                {
                    lock (fpsSync)
                    {
                        FPS = value;
                    }
                }
                lock (fpsSync)
                {
                    fpsTimer.Change(0, 1000 / FPS);
                }
            }
        }

        /// <summary>
        /// set the video quality to transmit, only accepts values from 0 to 100.
        /// </summary>
        public long transmissionQuality
        {
            get { return quality; } 
            set {
                    if (value>100)
                    {
                        lock (qualitySync)
                        {
                            quality = 100;
                        }
                    }
                    else if (value < 0)
                    {
                        lock (qualitySync)
                        {
                            quality = 0;
                        }
                    }
                    else
                    {
                        lock (qualitySync)
                        {
                            quality = value;
                        }
                    }
                }
        }

        public delegate void connectionLostEventHandler();
        public event connectionLostEventHandler connectionLost;

        public delegate void actualFPSChangedEventHandler(int newFPS);
        public event actualFPSChangedEventHandler actualFPSRecalculated;


        public delegate void connectCallback(bool connectionStatus);

        public videoSocketSender(VideoCaptureDevice _cameraSource){
            lock (fpsSync)
            {
                fpsTimer = new Timer(timerCallback, null, 0, 1000 / FPS);
            }
            transmittedFPSTimer = new Timer(transmittedFPSTimerCallback, null, 0, 1000);
            cameraSource = _cameraSource;
            cameraSource.NewFrame +=cameraSource_NewFrame;
        }

        private void transmittedFPSTimerCallback(object state)
        {
            int temp;
            lock (fpsSync)
            {
                temp = framesSent;
                framesSent = 0;
            }
            if (actualFPSRecalculated != null)
            {
                actualFPSRecalculated(temp);
            }
        }

        private void timerCallback(object state)
        {
            timeExpired = true;
        }

        public void beginConnect(IPAddress IP, int port, connectCallback callback){
            tcpClient = new TcpClient();
            tcpClient.BeginConnect(IP, port, connectedCallback, callback);
        }

        private void connectedCallback(IAsyncResult ar)
        {
            try
            {
                tcpClient.EndConnect(ar);
                if (tcpClient.Connected)
                {
                    connectCallback callback = (connectCallback)ar.AsyncState;
                    sendSocket = tcpClient.Client;
                    callback(true);
                }
                else
                {
                    connectCallback callback = (connectCallback)ar.AsyncState;
                    callback(false);
                }
            }
            catch
            {
                connectCallback callback = (connectCallback)ar.AsyncState;
                callback(false);
            }
        }

        public bool connect(IPAddress IP, int port)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(IP, port);
            sendSocket = tcpClient.Client;
            return true;
        }

        public bool beginTransmitting()
        {
            try
            {
                if (!tcpClient.Connected)
                {
                    return false;
                }
                cameraSource.Start();
                Transmitting = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void stop()
        {
            cameraSource.Stop();
            Transmitting = false;
        }

        private void cameraSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (Transmitting && timeExpired)
            {
                timeExpired = false;
                try
                {
                    byte[] jpegImage = Bitmap2JpegArray(eventArgs.Frame);
                    string headerBuilder = "SIZE:" + jpegImage.Length + "Z";
                    byte[] header = Encoding.ASCII.GetBytes(headerBuilder);
                    int headerSent = sendSocket.Send(header, header.Length, SocketFlags.None);
                    int imageSent = sendSocket.Send(jpegImage, jpegImage.Length, SocketFlags.None);
                    //Console.WriteLine("test B");
                    if (headerSent != header.Length)
                    {
                        Console.WriteLine("HEADER SIZE CONFLICT: \n SENT: " + headerSent + "\n  REQUIRED: " + header.Length + "\n");
                    }
                    if (imageSent != jpegImage.Length)
                    {
                        Console.WriteLine("IMAGE SIZE CONFLICT: \n SENT: " + imageSent + "\n  REQUIRED: " + jpegImage.Length + "\n");
                    }
                    lock (fpsSync)
                    {
                        framesSent++;
                    }
                }
                catch
                {
                    cameraSource.Stop();
                    Transmitting = false;
                    if (connectionLost != null)
                    {
                        connectionLost();
                    }
                }
            }
        }

        private byte[] Bitmap2JpegArray(Bitmap Frame)
        {
            MemoryStream ms = new MemoryStream();
            long sendQuality;
            lock (qualitySync)
            {
                sendQuality = quality;
            }
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter qualityParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, sendQuality);
            myEncoderParameters.Param[0] = qualityParameter;
            Frame.Save(ms, GetEncoder(ImageFormat.Jpeg), myEncoderParameters);
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

    public class dualVideoSocketSender
    {
        private VideoCaptureDevice leftCameraSource;
        private VideoCaptureDevice rightCameraSource;
        private IPAddress IPAddress;
        private int port;
        private TcpClient tcpClient;
        private Socket sendSocket;
        private volatile bool Transmitting = false;
        private long quality = 15; //15% is default quality
        private object qualitySync = 1;
        private volatile bool timeExpired = true; //when expired the frame can be sent (used to control FPS)
        private int FPS = 15;
        private object fpsSync = 1;
        private int framesSent = 0;
        Timer fpsTimer;
        Timer transmittedFPSTimer;

        private volatile bool rightSourceReady = false;
        private volatile bool leftSourceReady = false;
        private object camSync = 1;
        private dualBitmap DB;

        public int transmissionFPS
        {
            get { return FPS; }
            set
            {
                if (value < 0)
                {
                    lock (fpsSync)
                    {
                        FPS = 1; //only go down to 1. Otherwise your dividing by zero...
                    }
                }
                else if (value > 1000)
                {
                    lock (fpsSync)
                    {
                        FPS = 1000; //only 1000 ms in a second...
                    }
                }
                else
                {
                    lock (fpsSync)
                    {
                        FPS = value;
                    }
                }
                lock (fpsSync)
                {
                    fpsTimer.Change(0, 1000 / FPS);
                }
            }
        }

        /// <summary>
        /// set the video quality to transmit, only accepts values from 0 to 100.
        /// </summary>
        public long transmissionQuality
        {
            get { return quality; }
            set
            {
                if (value > 100)
                {
                    lock (qualitySync)
                    {
                        quality = 100;
                    }
                }
                else if (value < 0)
                {
                    lock (qualitySync)
                    {
                        quality = 0;
                    }
                }
                else
                {
                    lock (qualitySync)
                    {
                        quality = value;
                    }
                }
            }
        }

        public delegate void connectionLostEventHandler();
        public event connectionLostEventHandler connectionLost;

        public delegate void actualFPSChangedEventHandler(int newFPS);
        public event actualFPSChangedEventHandler actualFPSRecalculated;


        public delegate void connectCallback(bool connectionStatus);


        public dualVideoSocketSender(VideoCaptureDevice _leftCameraSource, VideoCaptureDevice _rightCameraSource)
        {
            lock (fpsSync)
            {
                fpsTimer = new Timer(timerCallback, null, 0, 1000 / FPS);
            }
            leftCameraSource = _leftCameraSource;
            leftCameraSource.NewFrame += leftCameraSource_NewFrame;
            rightCameraSource = _rightCameraSource;
            rightCameraSource.NewFrame += rightCameraSource_NewFrame;

            DB = new dualBitmap();
        }

        private void transmitWideImage()
        {
            if (Transmitting && timeExpired)
            {
                rightSourceReady = false;
                leftSourceReady = false;

                timeExpired = false;

                Bitmap wideImage = DB.getCombined();    //TODO: maybe lock camSync in transmitWideImage, sometimes one camera beats the other. Also maybe make sure getCombined has an "isReady" member.

                try
                {
                    byte[] jpegImage = Bitmap2JpegArray(wideImage);
                    string headerBuilder = "SIZE:" + jpegImage.Length + "Z";
                    byte[] header = Encoding.ASCII.GetBytes(headerBuilder);
                    int headerSent = sendSocket.Send(header, header.Length, SocketFlags.None);
                    int imageSent = sendSocket.Send(jpegImage, jpegImage.Length, SocketFlags.None);
                    //Console.WriteLine("test B");
                    if (headerSent != header.Length)
                    {
                        Console.WriteLine("HEADER SIZE CONFLICT: \n SENT: " + headerSent + "\n  REQUIRED: " + header.Length + "\n");
                    }
                    if (imageSent != jpegImage.Length)
                    {
                        Console.WriteLine("IMAGE SIZE CONFLICT: \n SENT: " + imageSent + "\n  REQUIRED: " + jpegImage.Length + "\n");
                    }
                    lock (fpsSync)
                    {
                        framesSent++;
                    }
                }
                catch
                {
                    leftCameraSource.Stop();
                    rightCameraSource.Stop();
                    Transmitting = false;
                    if (connectionLost != null)
                    {
                        connectionLost();
                    }
                }
            }
        }

        private void rightCameraSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            lock (camSync)
            {
                DB.setRightBitmap(eventArgs.Frame);
               // RW.setRight(ImageToByte(eventArgs.Frame));
                rightSourceReady = true;
                if (leftSourceReady)
                {
                    transmitWideImage();
                }
            }
        }

        private void leftCameraSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            lock (camSync)
            {
                DB.setLeftBitmap(eventArgs.Frame);
              //  RW.setLeft(ImageToByte(eventArgs.Frame));
                leftSourceReady = true;
                if (rightSourceReady)
                {
                    transmitWideImage();
                }
            }
        }

        private void transmittedFPSTimerCallback(object state)
        {
            int temp;
            lock (fpsSync)
            {
                temp = framesSent;
                framesSent = 0;
            }
            if (actualFPSRecalculated != null)
            {
                actualFPSRecalculated(temp);
            }
        }

        private void timerCallback(object state)
        {
            timeExpired = true;
        }

        public void beginConnect(IPAddress IP, int port, connectCallback callback)
        {
            tcpClient = new TcpClient();
            tcpClient.BeginConnect(IP, port, connectedCallback, callback);
        }

        private void connectedCallback(IAsyncResult ar)
        {
            try
            {
                tcpClient.EndConnect(ar);
                if (tcpClient.Connected)
                {
                    connectCallback callback = (connectCallback)ar.AsyncState;
                    sendSocket = tcpClient.Client;
                    callback(true);
                }
                else
                {
                    connectCallback callback = (connectCallback)ar.AsyncState;
                    callback(false);
                }
            }
            catch
            {
                connectCallback callback = (connectCallback)ar.AsyncState;
                callback(false);
            }
        }

        public bool connect(IPAddress IP, int port)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(IP, port);
            sendSocket = tcpClient.Client;
            return true;
        }

        public bool beginTransmitting()
        {
            try
            {
                if (!tcpClient.Connected)
                {
                    return false;
                }
                leftCameraSource.Start();
                rightCameraSource.Start();
                Transmitting = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void stop()
        {
            leftCameraSource.Stop();
            rightCameraSource.Stop();
            Transmitting = false;
        }

        private byte[] Bitmap2JpegArray(Bitmap Frame)
        {
            MemoryStream ms = new MemoryStream();
            long sendQuality;
            lock (qualitySync)
            {
                sendQuality = quality;
            }
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter qualityParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, sendQuality);
            myEncoderParameters.Param[0] = qualityParameter;
            Frame.Save(ms, GetEncoder(ImageFormat.Jpeg), myEncoderParameters);
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

        private static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

    }

    public class dualBitmap
    {
        private Bitmap left;
        private Bitmap right;
        private bool leftReady = false;
        private bool rightReady = false;
        private static Object sync = 0;

        public dualBitmap()
        {

        }

        public void setLeftBitmap(Bitmap leftBitmap)
        {
            lock (sync)
            {
                left = (Bitmap)leftBitmap.Clone();
                leftReady = true;
            }
        }

        public void setRightBitmap(Bitmap rightBitmap)
        {
            lock (sync)
            {
                right = (Bitmap)rightBitmap.Clone();
                rightReady = true;
            }
        }

        public Bitmap getCombined()
        {
            lock (sync)
            {
                if (leftReady && rightReady)
                {
                    try
                    {
                        leftReady = false;
                        rightReady = false;
                        Bitmap wideImage;
                        wideImage = new Bitmap((left.Width + right.Width), left.Height);
                        using (Graphics g = Graphics.FromImage(wideImage))
                        {
                            g.DrawImage(left, 0, 0);
                            g.DrawImage(right, left.Width, 0);
                        }
                        left.Dispose();
                        right.Dispose();
                        return wideImage;
                    }
                    catch
                    {
                        //do nothing...
                        Console.WriteLine("ERROR IN getCombined() in dualVideoSocketSender");
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
