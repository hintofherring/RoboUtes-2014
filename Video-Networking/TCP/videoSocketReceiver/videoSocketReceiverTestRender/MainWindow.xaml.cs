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
using videoSocketTools;
using System.IO;
using System.Threading;

namespace videoSocketReceiverTestRender {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        videoSocketReceiver vidSocRec;
        TcpClient client;
        ThreadStaticAttribute testThread;
        int frameCount = 0;

        public MainWindow() {
            InitializeComponent();
            //client = new TcpClient();
            //client.Connect("127.0.0.1", 4321);
            vidSocRec = new videoSocketReceiver(4321);
            vidSocRec.frameReceived += vidSocRec_frameReceived;
            
        }

        void vidSocRec_frameReceived(byte[] image) {
            frameCount++;
            try {
                Dispatcher.Invoke(() => imageBox.Source = ByteImageConverter.ByteToImage(image));
            }
            catch {
                MessageBox.Show("Unable to render image");
            }
            Dispatcher.Invoke(() => frameNum.Content = frameCount);
            /*MessageBox.Show("Got Image: "+frameCount);
            FileStream FS = File.Create("RECEIVED.txt");
            FS.Write(image, 0, image.Length); //TODO: we are parsing wrong, 4 bytes are left at the beginning... Other than that it works!!!!
            FS.Close();*/
        }

        private void nextFrameButton_Click(object sender, RoutedEventArgs e)
        {
            vidSocRec.start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
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
