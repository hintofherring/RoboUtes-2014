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
using videoSocketTools;
using System.Net;



namespace managedVideoTransmitter_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        managedVideoTransmitter MVT;

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
            MVT = new managedVideoTransmitter(tempSource, IPAddress.Parse("155.99.166.173"), 45000);
            MVT.actualFPSRecalculated += MVT_actualFPSRecalculated;
        }

        void MVT_actualFPSRecalculated(int newFPS)
        {
            Dispatcher.Invoke(() => actualFPSLabel.Content = newFPS);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            MVT.startTransmitting();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            MVT.stop();
        }

        private void qualitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MVT.setQuality((int)e.NewValue);
            Dispatcher.Invoke(() => qualityLabel.Content = (int)e.NewValue);
        }

        private void fpsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MVT.setFPS((int)e.NewValue);
            Dispatcher.Invoke(() => fpsLabel.Content = (int)e.NewValue);
        }
    }
}
