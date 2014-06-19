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

using ArmControlTools;
using XboxController;
using commSockServer;
using videoViewerWindow;
using macroInProgressWindow;

namespace Arm_Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XboxController.XboxController xboxController;

        commSockReceiver comSock;

        armInputManager armInputMan;
        armCommandTransmitter armTransmit;

        videoWindow palmVidWindow;
        videoWindow humerusVidWindow;

        macroInProgressWindow.MainWindow macroProgWindow;

        public MainWindow()
        {
            InitializeComponent();

            palmVidWindow = new videoWindow(35003, videoWindow.monitors.secondMonitor,"Palm Cam Window");
            palmVidWindow.Show();

            humerusVidWindow = new videoWindow(35005, videoWindow.monitors.thirdMonitor, "Humerus Cam Window");
            humerusVidWindow.Show();

            inputOnlineInd.setIndicatorState(toggleIndicator.indicatorState.Red);

            xboxController = new XboxController.XboxController();

            comSock = new commSockReceiver(35002);
            comSock.IncomingLine += comSock_IncomingLine;
            comSock.newConnection += comSock_newConnection;
            comSock.connectionLost += comSock_connectionLost;
            comSock.beginAccept();

            armInputMan = armInputManager.getInstance(xboxController,comSock);
            armInputMan.InputUnlockedEvent += inputUnlocked;

            macros.newMacroData += macros_newMacroData;
            

            armTransmit = new armCommandTransmitter(armInputMan, comSock);

            xboxControlMonitor.xboxController = xboxController;
            armSideView.armInputManager = armInputMan;
            armTopView.armInputManager = armInputMan;
            wristVisualizer.armInput = armInputMan;
        }

        void macros_newMacroData(newMacros.ToolboxControl.armCommand[] newTargets)
        {
            foreach (newMacros.ToolboxControl.armCommand command in newTargets)
            {
                switch (command.ID)
                {
                    case armConstants.armActuatorID.turnTable:
                        armInputMan.manuallySetTurnTable(command.target);
                        break;
                    case armConstants.armActuatorID.shoulder:
                        armInputMan.manuallySetShoulder(command.target);
                        break;
                    case armConstants.armActuatorID.grip:
                        armInputMan.manuallySetGripper(command.target);
                        break;
                    case armConstants.armActuatorID.elbow:
                        armInputMan.manuallySetElbow(command.target);
                        break;
                }
            }
        }

        void AME_macroEventStatusUpdate(bool currentStatus)
        {
            if (macroProgWindow != null)
            {
                macroProgWindow.stop();
            }
            if (currentStatus)
            {
                macroProgWindow = new macroInProgressWindow.MainWindow();
                macroProgWindow.Show();
            }
            else
            {
                if (macroProgWindow != null)
                {
                    macroProgWindow.stop();
                }
            }
        }

        private void inputUnlocked(bool unlocked)
        {
            if (unlocked)
            {
                Dispatcher.Invoke(() => inputOnlineInd.setIndicatorState(toggleIndicator.indicatorState.Green));
            }
            else
            {
                Dispatcher.Invoke(() => inputOnlineInd.setIndicatorState(toggleIndicator.indicatorState.Red));
            }
        }

        void comSock_connectionLost()
        {
            Dispatcher.Invoke(() =>netStatusInd.connected = false);
        }

        void comSock_newConnection(bool obj)
        {
            Dispatcher.Invoke(() =>netStatusInd.connected = true);
        }

        void comSock_IncomingLine(string obj)
        {
            Dispatcher.Invoke(() => internetInFeed.addText(obj));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
