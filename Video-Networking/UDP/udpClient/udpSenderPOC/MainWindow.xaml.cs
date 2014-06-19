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

using AForge.Video;
using AForge.Video.DirectShow;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

namespace udpSenderPOC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UdpClient uclient;
        //byte[] toSend;
        Thread sendThread;
        volatile bool connected = false;
        long frameNum = 0;

        public MainWindow()
        {
            InitializeComponent();
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                MessageBox.Show("found video device, connecting");
            }
            else
            {
                MessageBox.Show("Not enough local cameras found to satisfy request local cameras found");
                return;
            }
            VideoCaptureDevice tempSource = new VideoCaptureDevice(videoDevices[0].MonikerString); //just use the first camera found
            tempSource.NewFrame += tempSource_NewFrame;
            tempSource.Start();
            uclient = new UdpClient();
            //uclient.AllowNatTraversal(true);
            Console.WriteLine("trying to connect to server");
            uclient.Connect("50.168.197.42", 45000);
            connected = true;
        }

        void tempSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (connected)
            {
                byte[] image = Bitmap2JpegArray(eventArgs.Frame);

                frameNum++;

                byte[] header = Encoding.UTF8.GetBytes(paddedNum(frameNum));
                byte[] toSend = new byte[header.Length + image.Length];

                header.CopyTo(toSend, 0);
                image.CopyTo(toSend, header.Length);

                Console.WriteLine("LENGTH: " + toSend.Length);
                uclient.Send(toSend,toSend.Length);
            }
        }

        private byte[] Bitmap2JpegArray(Bitmap Frame) //TODO:!!!! THIS IS WHERE QUALITY CAN BE DETERMINED!!!!
        {
            MemoryStream ms = new MemoryStream();
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter qualityParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 10L);
            myEncoderParameters.Param[0] = qualityParameter;
            Frame.Save(ms, GetEncoder(ImageFormat.Jpeg), myEncoderParameters);
            byte[] toReturn = ms.ToArray();
            return toReturn;
        }

        private string paddedNum(long num)
        {
            string fmt = "000000000000000"; //15 digits long
            string toReturn = num.ToString(fmt);
            Console.WriteLine("Formattted: " + toReturn);
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

        /*private void sendMethod()
        {
            while (true)
            {
                uclient.Send(toSend, toSend.Length);
            }
        }*/

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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
