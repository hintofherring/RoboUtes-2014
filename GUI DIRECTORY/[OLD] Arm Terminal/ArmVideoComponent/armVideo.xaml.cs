using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ArmVideoComponent {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("ArmVideoComponent", true)]
    public partial class armVideoComp : UserControl {

        MJPEGStream mjpegSource;
        WPFBitmapConverter converter;
        private VideoCaptureDevice localSource;

        public armVideoComp() {
            InitializeComponent();
            converter = new WPFBitmapConverter();
        }

        /// <summary>
        /// Attempts to connect to an MJPEG feed from a given URL (ex. http://localhost:8080)
        /// and returns true/false depending on whether or not it succesfully connected. User must
        /// call StartVideo() to begin feed.
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public bool setMJPEGVideoFeedSource(string URL){
            if (mjpegSource != null && mjpegSource.IsRunning) { //stop other streams from running if they are
                mjpegSource.Stop();
            }
            try {
                mjpegSource = new MJPEGStream(URL);
                mjpegSource.NewFrame += mjpegSource_NewFrame;
                return true;
            }
            catch {
                return false;
            }

        }

        public void setTitle(string title) {
            titleLabel.Content = title;
        }

        public bool StartVideo() {
            try {
                mjpegSource.Start();
                return true;
            }
            catch {
                return false;
            }
        }

        public bool StopVideo() {
            try {
                mjpegSource.Stop();
                return true;
            }
            catch {
                return false;
            }
        }

        private void mjpegSource_NewFrame(object sender, NewFrameEventArgs eventArgs) {
            ImageSource IS = (ImageSource)converter.Convert(eventArgs.Frame, null, null, null);
            IS.Freeze();
            Dispatcher.BeginInvoke(new ThreadStart(delegate {
                videoImage.Source = IS;
                IS = null;
            }));
        }

        /// <summary>
        /// displays a feed from the first locally connected camera it can.
        /// set cameraIndex to 0 to see the first camera detected.
        /// </summary>
        public void showLocalCam(int cameraIndex) {
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count >= cameraIndex+1) {
                MessageBox.Show("found video device, connecting");
            }
            else {
                MessageBox.Show("Not enough local cameras found to satisfy request local cameras found");
                return;
            }
            localSource = new VideoCaptureDevice(videoDevices[cameraIndex].MonikerString);

            localSource.NewFrame += mjpegSource_NewFrame;
            localSource.Start();
        }

    }
}
