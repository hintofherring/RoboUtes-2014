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

using System.Drawing;
using videoSocketTools;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace videoSocketReceiver_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        videoSocketReceiver VSRTest;
        TcpListener server;
        TcpClient videoClient;


        //////////////////////////
        ///Just used for timing FPS, NOT REQUIRED
        int frameCount = 0;
        object sync = 1;
        Timer timer;
        int FPS = 0;
        ///////////////////////////////

        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer(timeCallback, null, 0, 1000);
            VSRTest = new videoSocketReceiver(61000,connectionCallback);
        }

        private void timeCallback(object state)
        {
            lock (sync)
            {
                FPS = frameCount;
                frameCount = 0;
            }
            Dispatcher.Invoke(() => fpsLabel.Content = FPS);
        }

        private void connectionCallback(bool connectionStatus)
        {
            if (connectionStatus)
            {
                VSRTest.frameReceived += VSRTest_frameReceived;
                VSRTest.connectionLost += VSRTest_connectionLost;
            }
        }

        void VSRTest_connectionLost()
        {
            MessageBox.Show("Connection Loss Detected on Server Side");
            VSRTest.close();
            VSRTest = new videoSocketReceiver(4322, connectionCallback);
        }

        private void VSRTest_frameReceived(byte[] newFrame)
        {
            if (newFrame.Length != null && newFrame.Length > 0)
            {
                lock (sync)
                {
                    frameCount++;
                }
                Dispatcher.Invoke(() => frameSizeLabel.Content = (newFrame.Length / 1000) + "kB");
                Dispatcher.Invoke(() => IBLabel.Content = (newFrame.Length / 1000) * FPS + "kB/S");
                Dispatcher.Invoke(() => imageBox.Source = ByteImageConverter.ByteToImage(newFrame));
            }
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
