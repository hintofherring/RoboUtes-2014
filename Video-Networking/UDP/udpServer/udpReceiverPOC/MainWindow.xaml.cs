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

using System.Net;
using System.Net.Sockets;
using System.IO;

namespace udpReceiverPOC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Socket serverSocket;
        byte[] buffer;

        public MainWindow()
        {
            InitializeComponent();
            buffer = new byte[45000];
            serverSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 45000);
            serverSocket.Bind(endpoint);
            Console.WriteLine("Listening on port 45000");
            serverSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, gotData, null);
        }

        private void gotData(IAsyncResult ar)
        {
            int size = serverSocket.EndReceive(ar);
            Console.WriteLine("Received: " + size + " bytes of data :)");
            Dispatcher.Invoke(() => frameNum.Content = Encoding.UTF8.GetString(buffer.Take(15).ToArray()));
            Dispatcher.Invoke(() => imageBox.Source = ByteImageConverter.ByteToImage(buffer.Skip(15).ToArray()));
            serverSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, gotData, null);
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
