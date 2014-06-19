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

using videoSocketTools;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Net;
using System.Net.Sockets;

namespace videoSocketSender_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        videoSocketSender VSSTest;
        bool connected = false;

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
            VSSTest = new videoSocketSender(tempSource);
            VSSTest.connectionLost += VSSTest_connectionLost;
            

        }

        void VSSTest_connectionLost()
        {
            VSSTest.stop();
            MessageBox.Show("Connection Loss Detected on Sending Side");
            VSSTest.beginConnect(IPAddress.Parse("127.0.0.1"), 4321, connectedCallbackTWO);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        public void connectedCallbackTWO(bool connectionStatus)
        {
            if (connectionStatus)
            {
                VSSTest.beginTransmitting();
            }
            else
            {
                VSSTest.beginConnect(IPAddress.Parse("127.0.0.1"), 4321, connectedCallbackTWO);
            }
        }

        public void connectedCallback(bool connectionState){
            if (connectionState)
            {
                connected = true;
            }
            else
            {
                connected = false;
                MessageBox.Show("Unable to connect");
            }
        }

        private void beginConnectButton_Click(object sender, RoutedEventArgs e)
        {
                VSSTest.beginConnect(IPAddress.Parse("127.0.0.1"), 4321, connectedCallbackTWO);
        }

        private void transmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (connected)
            {
                VSSTest.beginTransmitting();
            }
            else
            {
                MessageBox.Show("Not connected yet...");
            }
        }
    }
}
