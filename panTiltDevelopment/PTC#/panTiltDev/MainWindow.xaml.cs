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

using OculusOrientationLibrary;
using AForge.Video;
using AForge.Video.DirectShow;
using ArduinoLibrary;
using dualCameraViewer;

namespace panTiltDev
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        OculusOrientation orientation;
        ArduinoManager ArduMan;
        Arduino PTDuino;
        dualCameraViewer.MainWindow cameraView;
        bool camViewMaximized = false;

        int YAW = 0;
        int PITCH = 0;

        public MainWindow()
        {
            InitializeComponent();

            cameraView = new dualCameraViewer.MainWindow();
            cameraView.Show();

            comVizIn.setTitle("PT COM IN");
            comVizOut.setTitle("PT COM OUT");

            ArduMan = ArduinoManager.Instance;
            ArduMan.findArduinos();
            PTDuino = ArduMan.getPanTiltArduino();
            PTDuino.Data_Received += PTDuino_Data_Received;

            orientation = OculusOrientation.getInstance();
            orientation.orientationChanged += orientation_orientationChanged;
            
        }

        void PTDuino_Data_Received(string receivedData)
        {
            comVizIn.addText(receivedData);
        }

        private void orientation_orientationChanged(double[] newOrientation)
        {
            if (((int)newOrientation[0]).Map(300, 50, 0, 180) != YAW)
            {
                if ((int)newOrientation[0] < 300 && (int)newOrientation[0] > 50) //only if in range...
                {
                    YAW = ((int)newOrientation[0]).Map(300, 50, 0, 180);
                    Dispatcher.Invoke(() => yawValLabel.Content = YAW);
                    string toSend = "Y:" + YAW;
                    comVizOut.addText(toSend);
                    PTDuino.write(toSend);
                }
            }
            if (((int)newOrientation[1]).Map(-120, 120, 0, 180) != PITCH)
            {
                if ((int)newOrientation[1] > -120 && (int)newOrientation[1] < 120)
                {
                    PITCH = ((int)newOrientation[1]).Map(-120, 120, 0, 180);
                    Dispatcher.Invoke(() => pitchValLabel.Content = PITCH);
                    string toSend = "P:" + PITCH;
                    comVizOut.addText(toSend);
                    PTDuino.write(toSend);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void fullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (camViewMaximized)
            {
                cameraView.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                cameraView.WindowState = System.Windows.WindowState.Normal;
                camViewMaximized = false;
            }
            else
            {
                cameraView.WindowStyle = System.Windows.WindowStyle.None;
                cameraView.WindowState = System.Windows.WindowState.Maximized;
                camViewMaximized = true;
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            orientation.resetOrientation();
        }
    }

    public static class ExtensionMethods
    {
        public static int Map(this int value, int fromSource, int toSource, int fromTarget, int toTarget)
        {
            return (value - fromSource) * (toTarget-fromTarget)/(toSource-fromSource)+fromTarget;
        }
    }
}
