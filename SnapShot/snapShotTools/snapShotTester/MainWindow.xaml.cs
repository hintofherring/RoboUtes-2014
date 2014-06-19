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

using snapShotTools;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Net;

namespace snapShotTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        snapShotSender SSSender;
        FilterInfoCollection videoDevices;
        VideoCaptureDevice VCD;

        public MainWindow()
        {
            InitializeComponent();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            VCD = new VideoCaptureDevice(videoDevices[0].MonikerString);
            SSSender = new snapShotSender(VCD);
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            SSSender.transmitSnapshot(IPAddress.Parse("155.99.167.9"), 35005);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
