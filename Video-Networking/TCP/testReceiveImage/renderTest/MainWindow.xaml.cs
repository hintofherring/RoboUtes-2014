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
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace renderTest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Socket S = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 2. fill in the remote IP
            IPAddress IP = IPAddress.Parse("127.0.0.1");
            IPEndPoint IPE = new IPEndPoint(IP, 4321);

            Console.WriteLine("started connection service ....");
            // 3. connect to the server
            S.Connect(IPE);

            // 4. receive data
            byte[] buffer = new byte[1000000];
            S.Receive(buffer, 0, buffer.Length, SocketFlags.None);
            if (buffer[0] == Encoding.ASCII.GetBytes("S")[0]) {
                Console.WriteLine("Received START MESSAGE (\"S\")");
                S.Receive(buffer, 0, buffer.Length, SocketFlags.None);
            }
            //var Msg = Encoding.Unicode.GetString (buffer);
            //Console.WriteLine ("received message: (0)", msg);
            Console.WriteLine("Receive success");

            FileStream fs = File.Create("received.jpg");
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();

            //MemoryStream ms = new MemoryStream(buffer);
            imageBox.Source = ByteImageConverter.ByteToImage(buffer);
        }
    }

    public class ByteImageConverter {
        public static ImageSource ByteToImage(byte[] imageData) {
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
