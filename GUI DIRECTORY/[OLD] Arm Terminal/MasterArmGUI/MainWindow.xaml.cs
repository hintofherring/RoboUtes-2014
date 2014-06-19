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
using ArduinoLibrary;
using System.Threading;

namespace MasterArmGUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Dictionary<string, Arduino> GUIArduinoMap = new Dictionary<string, Arduino>();  //TODO: see the test classes in the ArduinoLibrary solution
        ArduinoManager ArduManage;
        Arduino ARM;
        bool usingGUIDrive = false;
        int currentPos; //[Theoretical Range is 0-1023] [Hardware Range is 0-255]
        int TTtargetPos = -1;
        int ELtargetPos = -1;
        int S1targetPos = -1;
        public MainWindow() {
            InitializeComponent();
            incomingCOMViz.title = "Incoming COM";
            incomingNETViz.title="Incoming NET";
            outgoingCOMViz.title="Outgoing COM";
            outgoingNETViz.title="Outgoing NET";
            leftVid.setTitle("Palm Camera");
            rightVid.setTitle("Angle Camera");
            macroButtons.MacroPressed += macroButtons_MacroPressed;
            GUIArmDrive.directionPressed += GUIArmDrive_directionPressed;
            GUIArmDrive.isGrayedOut = true;
            ControlStatusPanel.activateButtonClicked += ControlStatusPanel_activateButtonClicked;
            ArduManage = ArduinoManager.Instance;
            if (ArduManage.findArduinos()) { //An ARM Arduino is connected to the computer
                try {
                    ARM = ArduManage.getArmArduino();
                }
                catch {
                    MessageBox.Show("No ARM Arduino found, GUI cannot function correctly...");
                }
                ARM.Data_Received += ARM_Data_Received;
                ControlStatusPanel.toggleLights(Control_Status.ControlStatus.Indication_Lights.Arm_Connected, true);
            }
            else {
                ControlStatusPanel.toggleLights(Control_Status.ControlStatus.Indication_Lights.Arm_Connected, false);
            }

            leftVid.setMJPEGVideoFeedSource("HTTP://localhost:8080");
            leftVid.StartVideo();
            rightVid.setMJPEGVideoFeedSource("HTTP://localhost:8080");
            rightVid.StartVideo();
        }

        void macroButtons_MacroPressed(Button sender) {
            switch (sender.Uid) {
                case "Emergency Stop":
                    ARM.write("EMERSTOP");
                    break;
            }
        }

        void ControlStatusPanel_activateButtonClicked(Button sender) {
            switch (sender.Name) {
                case "guiDriveButton":
                    usingGUIDrive = !usingGUIDrive;
                    if (usingGUIDrive) {
                        ControlStatusPanel.toggleLights(Control_Status.ControlStatus.Indication_Lights.GUI_Drive, true);
                        GUIArmDrive.isGrayedOut = false;
                    }
                    else {
                        ControlStatusPanel.toggleLights(Control_Status.ControlStatus.Indication_Lights.GUI_Drive, false);
                        GUIArmDrive.isGrayedOut = true;
                    }
                    break;
                case "controllerDriveButton":
                    ControlStatusPanel.toggleLights(Control_Status.ControlStatus.Indication_Lights.Controller_Drive, true);
                    break;
                case "gripperInputButton":
                    ControlStatusPanel.toggleLights(Control_Status.ControlStatus.Indication_Lights.Gripper_Input, true);
                    break;
            }
        }

        void GUIArmDrive_directionPressed(GUIArmDrive.GUIDriveUIDParser t) {
            int MAG = t.Magnitude;
            string Axis = t.Axis;
            string toSend = "";
            switch (t.Axis) {
                case "X":
                    if ((TTtargetPos + MAG) <= 0) {
                        TTtargetPos = 1;
                    }
                    else if ((TTtargetPos + MAG) >= 1023) {
                        TTtargetPos = 1023;
                    }
                    else {
                        TTtargetPos += MAG;
                    }
                    toSend = "TTPOS:" + TTtargetPos;
                    break;
                case "Y":
                    if ((ELtargetPos + MAG) <= 0) {
                        ELtargetPos = 1;
                    }
                    else if ((ELtargetPos + MAG) >= 1023) {
                        ELtargetPos = 1023;
                    }
                    else {
                        ELtargetPos += MAG;
                    }
                    toSend = "ELPOS:" + ELtargetPos;
                    break;
                case "Z":
                    if ((S1targetPos + MAG) <= 0) {
                        S1targetPos = 1;
                    }
                    else if ((S1targetPos + MAG) >= 1023) {
                        S1targetPos = 1023;
                    }
                    else {
                        S1targetPos += MAG;
                    }
                    toSend = "S1POS:" + S1targetPos;
                    break;
            }
            if (toSend != "") { //If there is data to send, send it
                ARM.write(toSend);
                Dispatcher.Invoke(() => outgoingCOMViz.addText(toSend + "\n"));
            }

        }

        void ARM_Data_Received(string currentData) {
            //Print info to Incoming COM window and use data.
            Dispatcher.Invoke(()=>incomingCOMViz.addText("REC: "+currentData+"\n"));
            if (currentData.StartsWith("POS") && currentData.Length > 4) {
                string data = currentData.Remove(0,3);
                int tempPos = 0;
                if (int.TryParse(data, out tempPos)) {
                    if (tempPos >= 0 && tempPos <= 255) {
                        currentPos = tempPos;
                    }
                }
                else {
                    //Dispatcher.BeginInvoke(new ThreadStart(delegate { tempLABEL.Content = "BROKE: "+currentPos; }));
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Environment.Exit(0);
        }

        private void localCamButton_Click(object sender, RoutedEventArgs e) {
            rightVid.showLocalCam(1);
            leftVid.showLocalCam(0);
        }
    }
}
