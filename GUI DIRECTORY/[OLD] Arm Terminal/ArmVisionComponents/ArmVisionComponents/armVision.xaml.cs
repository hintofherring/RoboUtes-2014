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

//added to be compatible with forms, not jsut wpf
using System.Windows.Forms;
using AForge.Video;
using AForge.Controls;

namespace ArmVisionComponents
{
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("ArmVisionComponents", true)]
    public partial class ToolboxControl : System.Windows.Controls.UserControl
    {
        VideoSourcePlayer vPlayer = new VideoSourcePlayer();
        public ToolboxControl()
        {
            InitializeComponent();

            formsHost.Child = vPlayer;

            MJPEGStream mjpegSource = new MJPEGStream("http://localhost:8080");

            // open it
            OpenVideoSource(mjpegSource);
        }

        // Open video source
        private void OpenVideoSource(IVideoSource source)
        {

            // start new video source
            vPlayer.VideoSource = source;
            vPlayer.Start();
        }
    }
}
