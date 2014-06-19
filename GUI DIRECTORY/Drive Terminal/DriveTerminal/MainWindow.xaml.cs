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

using XboxController;
using commSockServer;
using driveTools;
using OculusOrientationLibrary;
using videoViewerWindow;

namespace DriveTerminal {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        XboxController.XboxController xboxController;

        commSockReceiver comSock;

        driveInputManager driveInputMan;
        driveTransmitter driveTransmit;

        OculusOrientation orientation;
        OculusTransmitter ptTransmitter;

        videoWindow oculusVidWindow;
        videoWindow noseCamVidWindow;

        public MainWindow() {
            InitializeComponent();

            xboxController = new XboxController.XboxController();
            pilotPreferences.xboxController = xboxController;
            comSock = new commSockReceiver(35000);
            comSock.IncomingLine += comSock_IncomingLine;
            comSock.newConnection += comSock_newConnection;
            comSock.connectionLost += comSock_connectionLost;
            comSock.beginAccept();

            driveInputMan = new driveInputManager(xboxController);
            driveTransmit = new driveTransmitter(driveInputMan, comSock);

            try
            {
                orientation = OculusOrientation.getInstance();
                ptTransmitter = new OculusTransmitter(comSock, orientation);
            }
            catch (Exception e)
            {
                MessageBox.Show("WARNING! Oculus offline (probably not plugged in). ERROR: " + e.Message);
            }

            pilotPreferences.topSpeedChanged += pilotPreferences_topSpeedChanged;
        }

        void pilotPreferences_topSpeedChanged(object sender, int newValue) {
            driveInputMan.throttleSensitivity = newValue;
        }

        void orientation_orientationChanged(double[] newOrientation) {
            throw new NotImplementedException();
        }

        void comSock_connectionLost() {
            Dispatcher.Invoke(() => connectionIndicator.connected = false);
        }

        void comSock_newConnection(bool obj) {
            Dispatcher.Invoke(() => connectionIndicator.connected = obj);
        }

        void comSock_IncomingLine(string obj) {
            Dispatcher.Invoke(() => commViz.addText("IN: " + obj));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {


            oculusVidWindow = new videoWindow(35001, videoWindow.monitors.thirdMonitor,"Oculus");
            // oculusVidWindow.setIDLabel("oculusVidWindow");
            oculusVidWindow.Show();

            noseCamVidWindow = new videoWindow(35004, videoWindow.monitors.secondMonitor,"Nose");
            // noseCamVidWindow.setIDLabel("noseCamVidWindow");
            noseCamVidWindow.Show();
        }
    }
}
