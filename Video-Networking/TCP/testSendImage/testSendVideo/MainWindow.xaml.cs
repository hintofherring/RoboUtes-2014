using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
using System.Drawing;
using System.Net.Sockets;
using System.Net;

namespace testSendVideo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VideoCaptureDevice localSource;
        private WPFBitmapConverter converter = new WPFBitmapConverter();
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public MainWindow()
        { 
            InitializeComponent();

            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0) {
                MessageBox.Show("found video device, connecting");
            }
            else {
                MessageBox.Show("Not enough local cameras found to satisfy request local cameras found");
                return;
            }
            localSource = new VideoCaptureDevice(videoDevices[0].MonikerString); //just use the first camera found

            localSource.NewFrame+=localSource_NewFrame;

            // 1. to create a socket
            Socket sListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 2. Fill IP
            IPAddress IP = IPAddress.Parse("127.0.0.1");
            IPEndPoint IPE = new IPEndPoint(IP, 4321);

            // 3. binding
            sListen.Bind(IPE);

            // 4. Monitor
            Console.WriteLine("Service is listening ...");
            sListen.Listen(2);
            
            // 5. loop to accept client connection requests
            clientSocket = sListen.Accept();
            MessageBox.Show("connected");

            localSource.Start();
        }

        void localSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            byte[] jpegImage = Bitmap2JpegArray(eventArgs.Frame);
            Dispatcher.Invoke(() => primaryImageBox.Source = ByteImageConverter.ByteToImage(jpegImage));

            string sizeString = "SIZE:" + jpegImage.Length + "Z";
            byte[] startMessage = Encoding.ASCII.GetBytes(sizeString);
            clientSocket.Send(startMessage, startMessage.Length, SocketFlags.None);
            clientSocket.Send(jpegImage, jpegImage.Length, SocketFlags.None);
        }

        public byte[] Bitmap2JpegArray(Bitmap Frame)
        {
            MemoryStream ms = new MemoryStream();
            Frame.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] toReturn = ms.ToArray();
            return toReturn;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        } 
    }

    public class ByteImageConverter
    {
        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }
    }
}
