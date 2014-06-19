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
using AForge.Controls;

namespace dualCameraViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VideoSourcePlayer leftPlayer = new VideoSourcePlayer();
        VideoSourcePlayer rightPlayer = new VideoSourcePlayer();

        public MainWindow()
        {
            InitializeComponent();

            ////////////////////Video Config - Start//////////////////////
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            string toPrint = "";
            foreach (FilterInfo vidDevice in videoDevices)
            {
                toPrint += vidDevice.MonikerString + "  \n  ";
            }
            MessageBox.Show("Found:\n\n" + toPrint);
            if (videoDevices.Count < 2)
            {
                MessageBox.Show("Not enough webcams detected, closing");
                Environment.Exit(0);
            }
            VideoCaptureDevice leftSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            VideoCaptureDevice rightSource = new VideoCaptureDevice(videoDevices[1].MonikerString);

            leftSource.Start();
            rightSource.Start();

            leftPlayer.VideoSource = leftSource;
            rightPlayer.VideoSource = rightSource;

            leftHost.Child = leftPlayer;
            rightHost.Child = rightPlayer;

            leftPlayer.Start();
            rightPlayer.Start();
        }
    }
}
