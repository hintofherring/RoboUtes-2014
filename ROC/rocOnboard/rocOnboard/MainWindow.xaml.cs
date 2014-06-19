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

using rocTools;
using ArduinoLibrary;
using videoSocketTools;
using AForge.Video;
using AForge.Video.DirectShow;
using ROC_infoTools;
using snapShotTools;

//TODO: find out how to (and if its worth it) make the program start with priority set to "realtime"

namespace rocOnboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ROCInfo hwMonitor;

        networkManager NetMan;
        ArduinoManager ArduMan;
        driveManager driveMan;
        ptManager ptMan;
        cameraManager camMan;
        armManager armMan;

        Arduino backDrive;
        Arduino frontDrive;
        Arduino ptDuino;
        Arduino ArmDuino;
        Arduino HandDuino;
        Arduino miscDuino;

        managedDualVideoTransmitter panTiltTransmitter;
        managedVideoTransmitter palmCamTransmitter;
        managedVideoTransmitter noseCamTransmitter;
        managedVideoTransmitter humerusCamTransmitter;

        snapShotSender logFrontSS;
        snapShotSender logRightSS;
        snapShotSender logBackSS;
        snapShotSender logLeftSS;

        public MainWindow()
        {
            InitializeComponent();

            driveIPLabel.Content = rocConstants.MCIP_DRIVE.ToString();
            armIPLabel.Content = rocConstants.MCIP_ARM.ToString();
            logisticsIPLabel.Content = rocConstants.MCIP_LOGISTICS;
            engineeringIPLabel.Content = rocConstants.MCIP_ENG.ToString();

            NetMan = networkManager.getInstance(incomingDriveLineManager, incomingEngLineManager, incomingArmLineManager, incomingLogisticsLineManager);
            NetMan.DriveConnectionStatusChanged += NetMan_DriveConnectionStatusChanged;
            NetMan.EngineeringConnectionStatusChanged += NetMan_EngineeringConnectionStatusChanged;
            NetMan.ArmConnectionStatusChanged += NetMan_ArmConnectionStatusChanged;
            NetMan.LogisticsConnectionStatusChanged += NetMan_LogisticsConnectionStatusChanged;

            ArduMan = ArduinoManager.Instance;
            ArduMan.findArduinos();

            backDrive = ArduMan.getDriveBackArduino(false);
            frontDrive = ArduMan.getDriveFrontArduino(false);
            ptDuino = ArduMan.getPanTiltArduino(true);
            ArmDuino = ArduMan.getArmArduino(false);
            HandDuino = ArduMan.getHandArduino(false);
            miscDuino = ArduMan.getMiscArduino(false);

            backDrive.Data_Received += backDrive_Data_Received;
            frontDrive.Data_Received += frontDrive_Data_Received;
            ptDuino.Data_Received += ptDuino_Data_Received;
            ArmDuino.Data_Received += ArmDuino_Data_Received;
            HandDuino.Data_Received += HandDuino_Data_Received;
            miscDuino.Data_Received += miscDuino_Data_Received;

            driveMan = driveManager.getInstance(backDrive, frontDrive, NetMan);
            ptMan = ptManager.getInstance(ptDuino, NetMan);
            armMan = armManager.getInstance(ArmDuino, HandDuino, NetMan);

            hwMonitor = ROCInfo.getInstance(1000);//updates once per second
            hwMonitor.updatedValue += hwMonitor_updatedValue;

            camMan = cameraManager.getInstance();
            camMan.assignCameras();

            VideoCaptureDevice panTiltLeft;
            VideoCaptureDevice panTiltRight;
            if(camMan.getCamera(rocConstants.CAMS.PT_left, out panTiltLeft)  &&  camMan.getCamera(rocConstants.CAMS.PT_right, out panTiltRight)){ //if both the left and right cameras are found...
                panTiltTransmitter = new managedDualVideoTransmitter(panTiltLeft, panTiltRight, rocConstants.MCIP_DRIVE, rocConstants.MCPORT_DRIVE_VIDEO_OCULUS);
                panTiltTransmitter.startTransmitting();
            }

            //////////////
            ///VIDEO CAMS SETUP
            //////////////

            VideoCaptureDevice palmCam;
            if (camMan.getCamera(rocConstants.CAMS.PALM, out palmCam))
            {
                palmCamTransmitter = new managedVideoTransmitter(palmCam, rocConstants.MCIP_ARM, rocConstants.MCPORT_ARM_VIDEO_PALM);
            }

            VideoCaptureDevice noseCam;
            if (camMan.getCamera(rocConstants.CAMS.NOSE, out noseCam))
            {
                noseCamTransmitter = new managedVideoTransmitter(noseCam, rocConstants.MCIP_DRIVE, rocConstants.MCPORT_DRIVE_VIDEO_NOSE);
            }

            VideoCaptureDevice humerusCam;
            if (camMan.getCamera(rocConstants.CAMS.Humerus, out humerusCam))
            {
                humerusCamTransmitter = new managedVideoTransmitter(humerusCam, rocConstants.MCIP_ARM, rocConstants.MCPORT_ARM_VIDEO_HUMERUS);
            }

            //////////////
            ///SNAPSHOT CAMS SETUP
            //////////////

            VideoCaptureDevice logFrontCam;
            if (camMan.getCamera(rocConstants.CAMS.LOG_FRONT, out logFrontCam))
            {
                logFrontSS = new snapShotSender(logFrontCam);
            }

            VideoCaptureDevice logRightCam;
            if (camMan.getCamera(rocConstants.CAMS.LOG_RIGHT, out logRightCam))
            {
                logRightSS = new snapShotSender(logRightCam);
            }

            VideoCaptureDevice logBackCam;
            if (camMan.getCamera(rocConstants.CAMS.LOG_BACK, out logBackCam))
            {
                logBackSS = new snapShotSender(logBackCam);
            }

            VideoCaptureDevice logLeftCam;
            if (camMan.getCamera(rocConstants.CAMS.LOG_LEFT, out logLeftCam))
            {
                logLeftSS = new snapShotSender(logLeftCam);
            }

        }

        void miscDuino_Data_Received(string receivedData)
        {
            Dispatcher.Invoke(() => miscCOMIN.addText(receivedData));
        }

        void NetMan_LogisticsConnectionStatusChanged(bool commSockIsConnected)
        {
            if (commSockIsConnected)
            {
                logisticsConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Green);
            }
            else
            {
                logisticsConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Red);
            }
        }

        private void incomingLogisticsLineManager(string incoming)
        {
            Dispatcher.Invoke(() => incomingInternet.addText(incoming));
            NetMan.write(rocConstants.COMID.LOGISTICSCOM, " logPING ");

            switch (incoming)
            {
                case "LOG_FRONT":
                    logFrontSS.transmitSnapshot(rocConstants.MCIP_LOGISTICS, rocConstants.MCPORT_LOGISTICS_FRONT_SS);
                    break;
                case "LOG_RIGHT":
                    logRightSS.transmitSnapshot(rocConstants.MCIP_LOGISTICS, rocConstants.MCPORT_LOGISTICS_RIGHT_SS);
                    break;
                case "LOG_REAR":
                    logBackSS.transmitSnapshot(rocConstants.MCIP_LOGISTICS, rocConstants.MCPORT_LOGISTICS_BACK_SS);
                    break;
                case "LOG_LEFT":
                    logLeftSS.transmitSnapshot(rocConstants.MCIP_LOGISTICS, rocConstants.MCPORT_LOGISTICS_LEFT_SS);
                    break;
            }
        }

        void hwMonitor_updatedValue(ROCinfoConstants.hardwareInfoID hardwareID, int val)
        {
            try
            {
                switch (hardwareID)
                {
                    case ROCinfoConstants.hardwareInfoID.CPULoad:
                        NetMan.write(rocConstants.COMID.ENGCOM, "CPU_LOAD_" + val);
                        break;
                    case ROCinfoConstants.hardwareInfoID.CPUTemp:
                        NetMan.write(rocConstants.COMID.ENGCOM, "CPU_TEMP_" + val);
                        break;
                    case ROCinfoConstants.hardwareInfoID.GPULoad:
                        NetMan.write(rocConstants.COMID.ENGCOM, "GPU_LOAD_" + val);
                        break;
                    case ROCinfoConstants.hardwareInfoID.GPUTemp:
                        NetMan.write(rocConstants.COMID.ENGCOM, "GPU_TEMP_" + val);
                        break;
                    case ROCinfoConstants.hardwareInfoID.RAMLoad:
                        NetMan.write(rocConstants.COMID.ENGCOM, "RAM_LOAD_" + val);
                        break;
                }
            }
            catch
            {
                //do nothing
                return;
            }
        }

        void HandDuino_Data_Received(string receivedData)
        {
            Dispatcher.Invoke(() => wristCOMIN.addText(receivedData));
        }

        void ArmDuino_Data_Received(string receivedData)
        {
            Dispatcher.Invoke(() => armCOMIN.addText(receivedData));
        }

        void NetMan_ArmConnectionStatusChanged(bool commSockIsConnected)
        {
            if (commSockIsConnected)
            {
                armConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Green);
            }
            else
            {
                armConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Red);
            }
        }

        private void incomingArmLineManager(string incoming)
        {
            Dispatcher.Invoke(() => incomingInternet.addText(incoming + "\n"));
            NetMan.write(rocConstants.COMID.ARMCOM, " armPING ");
        }

        void ptDuino_Data_Received(string receivedData)
        {
            Dispatcher.Invoke(() => panTiltCOMIN.addText(receivedData + "\r"));
        }

        void frontDrive_Data_Received(string receivedData)
        {
            Dispatcher.Invoke(() => driveFrontCOMIN.addText(receivedData+"\r"));
        }

        void backDrive_Data_Received(string receivedData)
        {
            Dispatcher.Invoke(() => driveBackCOMIN.addText(receivedData + "\r"));
        }

        void NetMan_DriveConnectionStatusChanged(bool commSockIsConnected)
        {
            if(commSockIsConnected){
                driveConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Green);
            }
            else{
                driveConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Red);
            }
        }

        void NetMan_EngineeringConnectionStatusChanged(bool commSockIsConnected)
        {
            if (commSockIsConnected)
            {
                engineeringConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Green);
            }
            else
            {
                engineeringConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Red);
            }
        }

        void incomingDriveLineManager(string incoming)
        {
            Dispatcher.Invoke(() => incomingInternet.addText(incoming+"\n"));
            NetMan.write(rocConstants.COMID.DRIVECOM, " drivePING ");
        }

        void incomingEngLineManager(string incoming) //TODO: Use a switch here, not a massive else if tree
        {
            try
            {
                if (incoming != null)
                {
                    if (incoming.Contains("TRANSMIT"))
                    {
                        Dispatcher.Invoke(() => incomingInternet.addText(incoming + "\n"));
                        NetMan.write(rocConstants.COMID.ENGCOM, " engPING ");
                        if (incoming == "PT_TRANSMIT")
                        {
                            if (panTiltTransmitter != null)
                            {
                                panTiltTransmitter.startTransmitting();
                            }
                        }
                        else if (incoming == "PT_STOP_TRANSMIT")
                        {
                            panTiltTransmitter.stop();
                        }
                        else if (incoming == "PALM_TRANSMIT")
                        {
                            palmCamTransmitter.startTransmitting();
                        }
                        else if (incoming == "PALM_STOP_TRANSMIT")
                        {
                            palmCamTransmitter.stop();
                        }
                        else if (incoming == "NOSE_TRANSMIT")
                        {
                            noseCamTransmitter.startTransmitting();
                        }
                        else if (incoming == "NOSE_STOP_TRANSMIT")
                        {
                            noseCamTransmitter.stop();
                        }
                        else if (incoming == "HUMERUS_TRANSMIT")
                        {
                            humerusCamTransmitter.startTransmitting();
                        }
                        else if (incoming == "HUMERUS_STOP_TRANSMIT")
                        {
                            humerusCamTransmitter.stop();
                        }
                    }
                    else if (incoming.Contains("QUALITY"))
                    {
                        int quality;
                        if (incoming.StartsWith("PT_QUALITY_"))
                        {
                            if (int.TryParse(incoming.Substring(incoming.LastIndexOf("_") + 1), out quality))
                            {
                                panTiltTransmitter.setQuality(quality);
                            }
                        }
                        else if (incoming.StartsWith("WORKSPACE_QUALITY_"))
                        {
                            if (int.TryParse(incoming.Substring(incoming.LastIndexOf("_") + 1), out quality))
                            {
                                noseCamTransmitter.setQuality(quality);
                            }
                        }
                        else if (incoming.StartsWith("PALM_QUALITY_"))
                        {
                            if (int.TryParse(incoming.Substring(incoming.LastIndexOf("_") + 1), out quality))
                            {
                                palmCamTransmitter.setQuality(quality);
                            }
                        }
                        else if (incoming.StartsWith("HUMERUS_QUALITY_"))
                        {
                            if (int.TryParse(incoming.Substring(incoming.LastIndexOf("_") + 1), out quality))
                            {
                                humerusCamTransmitter.setQuality(quality);
                            }
                        }
                    }
                    else if (incoming.Contains("FPS"))
                    {
                        int fps;
                        if (incoming.StartsWith("PT_FPS_"))
                        {
                            if (int.TryParse(incoming.Substring(incoming.LastIndexOf("_") + 1), out fps))
                            {
                                panTiltTransmitter.setFPS(fps);
                            }
                        }
                        else if (incoming.StartsWith("PALM_FPS_"))
                        {
                            if (int.TryParse(incoming.Substring(incoming.LastIndexOf("_") + 1), out fps))
                            {
                                palmCamTransmitter.setFPS(fps);
                            }
                        }
                        else if (incoming.StartsWith("HUMERUS_FPS_"))
                        {
                            if (int.TryParse(incoming.Substring(incoming.LastIndexOf("_") + 1), out fps))
                            {
                                humerusCamTransmitter.setFPS(fps);
                            }
                        }
                        else if (incoming.StartsWith("WORKSPACE_FPS_"))
                        {
                            if (int.TryParse(incoming.Substring(incoming.LastIndexOf("_") + 1), out fps))
                            {
                                noseCamTransmitter.setFPS(fps);
                            }
                        }
                    }
                }
            }
            catch
            {
                //do nothing...
                return;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            miscDuino.write("MAST_DOWN");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            miscDuino.write("MAST_UP");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            miscDuino.write("MAST_STOP");
        }
    }
}
