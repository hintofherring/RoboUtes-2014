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

using commSockServer;
using engineeringTerminalTools;
using System.Threading;

namespace Engineering_Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        commSockReceiver comSock;
        engineeringNetworkManager engNetMan;
        Timer rareHeartbeatTimer;

        public MainWindow()
        {
            InitializeComponent();

            comSock = new commSockReceiver(35010);
            comSock.IncomingLine += comSock_IncomingLine;
            comSock.newConnection += comSock_newConnection;
            comSock.connectionLost += comSock_connectionLost;
            comSock.beginAccept();

            videoManager.intendedCameraStatusChanged+=videoManager_intendedCameraStatusChanged;
            videoQualityControl.userUpdatedVideoTraits += videoQualityControl_userUpdatedVideoTraits;

            rareHeartbeatTimer = new Timer(rareHeartbeatTimerCallback, null, 2000, 25000);
        }

        void videoQualityControl_userUpdatedVideoTraits(videoTraits.ToolboxControl.FeedID ID, int quality, int fps)
        {
            switch (ID)
            {
                case videoTraits.ToolboxControl.FeedID.humerus:
                    comSock.write("HUMERUS_QUALITY_" + quality);
                    comSock.write("HUMERUS_FPS_" + fps);
                    break;
                case videoTraits.ToolboxControl.FeedID.palm:
                    comSock.write("PALM_QUALITY_" + quality);
                    comSock.write("PALM_FPS_" + fps);
                    break;
                case videoTraits.ToolboxControl.FeedID.pantilt:
                    comSock.write("PT_QUALITY_" + quality);
                    comSock.write("PT_FPS_" + fps);
                    break;
                case videoTraits.ToolboxControl.FeedID.workspace:
                    comSock.write("WORKSPACE_QUALITY_" + quality);
                    comSock.write("WORKSPACE_FPS_" + fps);
                    break;
            }
        }

        private void rareHeartbeatTimerCallback(object state) {
            try {
                comSock.write("engHeartbeat");
            }
            catch {
                //do nothing, the terminal is probably not connected
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            engNetMan = new engineeringNetworkManager(comSock, videoManager);   //Must happen after the form is loaded or else the videoManager will be a null pointer exception...
        }

        void videoManager_intendedCameraStatusChanged(videoManager.ToolboxControl.FeedID videoFeedID, bool feedState)
        {
            switch (videoFeedID)
            {
                case global::videoManager.ToolboxControl.FeedID.OculusPT:
                    if (feedState)
                    {
                        comSock.write("PT_TRANSMIT");
                    }
                    else
                    {
                        comSock.write("PT_STOP_TRANSMIT");
                    }
                    break;

                case global::videoManager.ToolboxControl.FeedID.Palm:
                    if (feedState) {
                        comSock.write("PALM_TRANSMIT");
                    }
                    else {
                        comSock.write("PALM_STOP_TRANSMIT");
                    }
                    break;

                case global::videoManager.ToolboxControl.FeedID.Nose:
                    if (feedState) {
                        comSock.write("NOSE_TRANSMIT");
                    }
                    else {
                        comSock.write("NOSE_STOP_TRANSMIT");
                    }
                    break;

                case global::videoManager.ToolboxControl.FeedID.Humerus:
                    if (feedState) {
                        comSock.write("HUMERUS_TRANSMIT");
                    }
                    else {
                        comSock.write("HUMERUS_STOP_TRANSMIT");
                    }
                    break;
            }
        }

        void videoManager_resetRequest(videoManager.ToolboxControl.FeedID videoFeedID)
        {
            //throw new NotImplementedException();
        }

        void comSock_connectionLost()
        {
            Dispatcher.Invoke(() => connectionIndicator.connected = false);
            comSock.beginAccept();
        }

        void comSock_newConnection(bool obj)
        {
            Dispatcher.Invoke(() => connectionIndicator.connected = true);
        }

        void comSock_IncomingLine(string obj)
        {
            if (obj != null) {
                Dispatcher.Invoke(() => internetInComViz.addText(obj + "\r"));
                int val;
                if (obj.StartsWith("CPU_LOAD_")) {
                    if (int.TryParse(obj.Substring(obj.LastIndexOf("_")+1), out val)) {
                        hardwareMonitor.setCPULoad(val);
                    }
                }
                else if (obj.StartsWith("CPU_TEMP_")) {
                    if (int.TryParse(obj.Substring(obj.LastIndexOf("_") + 1), out val)) {
                        hardwareMonitor.setCPUTemp(val);
                    }
                }
                else if (obj.StartsWith("GPU_LOAD_")) {
                    if (int.TryParse(obj.Substring(obj.LastIndexOf("_") + 1), out val)) {
                        hardwareMonitor.setGPULoad(val);
                    }
                }
                else if (obj.StartsWith("GPU_TEMP_")) {
                    if (int.TryParse(obj.Substring(obj.LastIndexOf("_") + 1), out val)) {
                        hardwareMonitor.setGPUTemp(val);
                    }
                }
                else if (obj.StartsWith("RAM_LOAD_")) {
                    if (int.TryParse(obj.Substring(obj.LastIndexOf("_") + 1), out val)) {
                        hardwareMonitor.setRAMLoad(val);
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
