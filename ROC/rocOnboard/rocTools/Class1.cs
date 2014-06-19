using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using commSockClient;
using ArduinoLibrary;
using AForge.Video;
using AForge.Video.DirectShow;
using videoSocketTools;

namespace rocTools
{
    public class networkManager
    {
        public delegate void ConnectionChangedEventHandler(bool commSockIsConnected);
        public event ConnectionChangedEventHandler DriveConnectionStatusChanged;
        public event ConnectionChangedEventHandler EngineeringConnectionStatusChanged;
        public event ConnectionChangedEventHandler ArmConnectionStatusChanged;
        public event ConnectionChangedEventHandler LogisticsConnectionStatusChanged;

        public delegate void incomingLineEventHandler(string incomingString);
        public event incomingLineEventHandler incomingDrive;
        public event incomingLineEventHandler incomingEngineering;
        public event incomingLineEventHandler incomingArm;
        public event incomingLineEventHandler incomingLogistics;

        private static networkManager NM;

        public static networkManager getInstance(incomingLineEventHandler incomingDriveLineHandler, incomingLineEventHandler incomingEngineeringLineHandler, incomingLineEventHandler incomingArmLineHandler, incomingLineEventHandler incomingLogisticsLineHandler)
        {
            if (NM != null)
            {
                return NM;
            }
            else
            {
                NM = new networkManager(incomingDriveLineHandler, incomingEngineeringLineHandler, incomingArmLineHandler, incomingLogisticsLineHandler);
                return NM;
            }
        }

        private commSockSender DRIVECOM;
        private commSockSender ENGCOM;
        private commSockSender ARMCOM;
        private commSockSender LOGCOM;

        private networkManager(incomingLineEventHandler incomingDriveLineHandler, incomingLineEventHandler incomingEngineeringLineHandler, incomingLineEventHandler incomingArmLineHandler, incomingLineEventHandler incomingLogisticsLineHandler)
        {
            //Drive networking setup
            incomingDrive = incomingDriveLineHandler;
            DRIVECOM = new commSockSender("DRIVECOM");
            DRIVECOM.incomingLineEvent += DRIVECOM_incomingLineEvent;
            DRIVECOM.connectionStatusChanged += DRIVECOM_connectionStatusChanged;
            DRIVECOM.beginConnect(rocConstants.MCIP_DRIVE, rocConstants.MCPORT_DRIVE);

            //Engineering networking setup
            incomingEngineering = incomingEngineeringLineHandler;
            ENGCOM = new commSockSender("ENGCOM");
            ENGCOM.incomingLineEvent += ENGCOM_incomingLineEvent;
            ENGCOM.connectionStatusChanged += ENGCOM_connectionStatusChanged;
            ENGCOM.beginConnect(rocConstants.MCIP_ENG, rocConstants.MCPORT_ENGINEERING);

            //Arm networking setup
            incomingArm = incomingArmLineHandler;
            ARMCOM = new commSockSender("ARMCOM");
            ARMCOM.incomingLineEvent +=ARMCOM_incomingLineEvent;
            ARMCOM.connectionStatusChanged +=ARMCOM_connectionStatusChanged;
            ARMCOM.beginConnect(rocConstants.MCIP_ARM, rocConstants.MCPORT_ARM);

            //Logistics networking setup
            incomingLogistics = incomingLogisticsLineHandler;
            LOGCOM = new commSockSender("LOGCOM");
            LOGCOM.incomingLineEvent += LOGCOM_incomingLineEvent;
            LOGCOM.connectionStatusChanged += LOGCOM_connectionStatusChanged;
            LOGCOM.beginConnect(rocConstants.MCIP_LOGISTICS, rocConstants.MCPORT_LOGISTICS);

        }

        void LOGCOM_connectionStatusChanged(bool commSockIsConnected)
        {
            if (LogisticsConnectionStatusChanged != null)
            {
                LogisticsConnectionStatusChanged(commSockIsConnected);
            }
        }

        void LOGCOM_incomingLineEvent(string obj)
        {
            if (incomingLogistics != null)
            {
                incomingLogistics(obj);
            }
        }

        private void ARMCOM_connectionStatusChanged(bool commSockIsConnected)
        {
            if (ArmConnectionStatusChanged != null)
            {
                ArmConnectionStatusChanged(commSockIsConnected);
            }
        }

        private void ARMCOM_incomingLineEvent(string obj)
        {
            if (incomingArm != null)
            {
                incomingArm(obj);
            }
        }

        void ENGCOM_connectionStatusChanged(bool commSockIsConnected)
        {
            if (EngineeringConnectionStatusChanged != null)
            {
                EngineeringConnectionStatusChanged(commSockIsConnected);
            }
        }

        void ENGCOM_incomingLineEvent(string obj)
        {
            if (incomingEngineering != null)
            {
                incomingEngineering(obj);
            }
        }

        void DRIVECOM_connectionStatusChanged(bool commSockIsConnected)
        {
            if (DriveConnectionStatusChanged != null)
            {
                DriveConnectionStatusChanged(commSockIsConnected);
            }
        }

        void DRIVECOM_incomingLineEvent(string obj)
        {
            if (incomingDrive != null)
            {
                incomingDrive(obj);
            }
        }

        public void disconnect(rocConstants.COMID ID)
        {
            switch (ID)
            {
                case rocConstants.COMID.DRIVECOM:
                    DRIVECOM.disconnect();
                    break;
                case rocConstants.COMID.ENGCOM:
                    ENGCOM.disconnect();
                    break;
                case rocConstants.COMID.ARMCOM:
                    ARMCOM.disconnect();
                    break;
                case rocConstants.COMID.LOGISTICSCOM:
                    LOGCOM.disconnect();
                    break;
            }
        }

        /// <summary>
        /// Only needs to be called after calling disconnect. Sockets try to connect as soon as the networkManager is online.
        /// </summary>
        /// <param name="ID"></param>
        public void connect(rocConstants.COMID ID)
        {
            switch (ID)
            {
                case rocConstants.COMID.DRIVECOM:
                    DRIVECOM.beginConnect(rocConstants.MCIP_DRIVE, rocConstants.MCPORT_DRIVE);
                    break;
                case rocConstants.COMID.ENGCOM:
                    ENGCOM.beginConnect(rocConstants.MCIP_ENG, rocConstants.MCPORT_ENGINEERING);
                    break;
                case rocConstants.COMID.ARMCOM:
                    ARMCOM.beginConnect(rocConstants.MCIP_ARM, rocConstants.MCPORT_ARM);
                    break;
                case rocConstants.COMID.LOGISTICSCOM:
                    LOGCOM.beginConnect(rocConstants.MCIP_LOGISTICS, rocConstants.MCPORT_LOGISTICS);
                    break;
            }
        }

        public void write(rocConstants.COMID ID, string Message)
        {
            switch (ID)
            {
                case rocConstants.COMID.DRIVECOM:
                    Message = Message.Replace("\n", ""); //Do not allow \n to be transmitted... I think this is dealt with elsewhere, but this is safe...
                    DRIVECOM.sendMessage(Message);
                    break;

                case rocConstants.COMID.ENGCOM:
                    Message = Message.Replace("\n", ""); //Do not allow \n to be transmitted... I think this is dealt with elsewhere, but this is safe...
                    ENGCOM.sendMessage(Message);
                    break;

                case rocConstants.COMID.ARMCOM:
                    Message = Message.Replace("\n", ""); //Do not allow \n to be transmitted... I think this is dealt with elsewhere, but this is safe...
                    ARMCOM.sendMessage(Message);
                    break;

                case rocConstants.COMID.LOGISTICSCOM:
                    Message = Message.Replace("\n", ""); //Do not allow \n to be transmitted... I think this is dealt with elsewhere, but this is safe...
                    LOGCOM.sendMessage(Message);
                    break;
            }
        }

    }

    public static class rocConstants
    {
        public static readonly IPAddress MCIP_DRIVE = IPAddress.Parse("155.98.4.10");
        public static readonly IPAddress MCIP_ENG = IPAddress.Parse("155.98.4.14");
        public static readonly IPAddress MCIP_ARM = IPAddress.Parse("155.99.230.60");
        public static readonly IPAddress MCIP_LOGISTICS = IPAddress.Parse("155.98.5.147");

        public static readonly int MCPORT_DRIVE = 35000;
        public static readonly int MCPORT_DRIVE_VIDEO_OCULUS = 35001;
        public static readonly int MCPORT_ENGINEERING = 35010;
        public static readonly int MCPORT_ARM = 35002;
        public static readonly int MCPORT_ARM_VIDEO_PALM = 35003;
        public static readonly int MCPORT_DRIVE_VIDEO_NOSE = 35004;
        public static readonly int MCPORT_ARM_VIDEO_HUMERUS = 35005;
        public static readonly int MCPORT_LOGISTICS = 35006;

        public static readonly int MCPORT_LOGISTICS_FRONT_SS = 35007;
        public static readonly int MCPORT_LOGISTICS_BACK_SS = 35008;
        public static readonly int MCPORT_LOGISTICS_RIGHT_SS = 35009;
        public static readonly int MCPORT_LOGISTICS_LEFT_SS = 35011;

        public enum COMID
        {
            DRIVECOM = 0,
            ARMCOM = 1,
            LOGISTICSCOM = 2,
            ENGCOM = 3
        };

        public static int[] defaultCameraAssignments = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        public enum CAMS
        {
            PT_left = 1,
            PT_right = 0,
            PALM = 7,
            NOSE = 8,
            Humerus = 6,
            LOG_FRONT = 5,
            LOG_BACK = 3,
            LOG_RIGHT = 0,
            LOG_LEFT = 1,
        };
    }

    public class driveManager
    {
        private Arduino frontDriveDuino;
        private Arduino backDriveDuino;

        private volatile int leftSpeed = 0;
        private volatile int rightSpeed = 0;

        private networkManager netMan;

        private static driveManager instance;
        public static driveManager getInstance(Arduino backArduino, Arduino frontArduino, networkManager _netMan)
        {
            if (instance == null)
            {
                instance = new driveManager(backArduino, frontArduino, _netMan);
            }
            return instance;
        }

        private driveManager(Arduino backArduino, Arduino frontArduino, networkManager _netMan)
        {
            frontDriveDuino = frontArduino;
            backDriveDuino = backArduino;

            frontDriveDuino.Data_Received += frontDriveDuino_Data_Received;
            backDriveDuino.Data_Received += backDriveDuino_Data_Received;

            netMan = _netMan;
            netMan.incomingDrive += netMan_incomingDrive;
        }

        void netMan_incomingDrive(string incomingString)
        {
            if (incomingString.StartsWith("R:"))
            {
                incomingString = incomingString.Replace("R:", "");
                int newRightSpeed;
                if (int.TryParse(incomingString, out newRightSpeed))
                {
                    updateRightSpeed(newRightSpeed);
                }
            }
            else if (incomingString.StartsWith("L:"))
            {
                incomingString = incomingString.Replace("L:", "");
                int newLeftSpeed;
                if (int.TryParse(incomingString, out newLeftSpeed))
                {
                    updateLeftSpeed(newLeftSpeed);
                }
            }
        }

        public void updateLeftSpeed(int speed)
        {
            frontDriveDuino.write("L:" + speed);
            backDriveDuino.write("L:" + speed);
        }

        public void updateRightSpeed(int speed)
        {
            frontDriveDuino.write("R:" + speed);
            backDriveDuino.write("R:" + speed);
        }


        private void frontDriveDuino_Data_Received(string receivedData)
        {

        }

        private void backDriveDuino_Data_Received(string receivedData)
        {

        }
    }

    public class ptManager
    {
        private Arduino ptDuino;
        private networkManager netMan;

        private static ptManager instance;
        public static ptManager getInstance(Arduino panTiltArduino, networkManager _netMan)
        {
            if (instance == null)
            {
                instance = new ptManager(panTiltArduino, _netMan);
            }
            return instance;
        }

        private ptManager(Arduino panTiltArduino, networkManager _netMan)
        {
            ptDuino = panTiltArduino;
            netMan = _netMan;
            ptDuino.Data_Received += ptDuino_Data_Received;
            netMan.incomingDrive += netMan_incomingDrive;
        }

        void netMan_incomingDrive(string incomingString)
        {
            if (incomingString.StartsWith("Y:"))
            {
                incomingString = incomingString.Replace("Y:", "");
                int newYaw;
                if (int.TryParse(incomingString, out newYaw))
                {
                    updateYaw(newYaw);
                }
            }
            else if (incomingString.StartsWith("P:"))
            {
                incomingString = incomingString.Replace("P:", "");
                int newPitch;
                if (int.TryParse(incomingString, out newPitch))
                {
                    updatePitch(newPitch);
                }
            }
        }

        private void updatePitch(int newPitch)
        {
            ptDuino.write("P:" + newPitch);
        }

        private void updateYaw(int newYaw)
        {
            ptDuino.write("Y:" + newYaw);
        }

        void ptDuino_Data_Received(string receivedData)
        {
            //throw new NotImplementedException();
        }
    }

    public class armManager
    {
        private Arduino armDuino;
        private Arduino handDuino;
        private networkManager netMan;
        
        private int shoulderPos;
        private int elbowPos;
        private int turnTablePos;
        private object positionSync = 1;

        private static armManager instance;

        public static armManager getInstance(Arduino armArduino, Arduino handArduino, networkManager _netman)
        {
            if (instance == null)
            {
                instance = new armManager(armArduino,handArduino,_netman);
            }
            return instance;
        }

        private armManager(Arduino armArduino, Arduino handArduino, networkManager _netman)
        {
            armDuino = armArduino;
            handDuino = handArduino;
            netMan = _netman;
            netMan.incomingArm += netMan_incomingArm;
            armDuino.Data_Received += armDuino_Data_Received;
            handDuino.Data_Received += handDuino_Data_Received;
        }

        void handDuino_Data_Received(string receivedData)
        {
            //throw new NotImplementedException();
        }

        void armDuino_Data_Received(string receivedData)
        {
            string toParse = receivedData.Substring(receivedData.LastIndexOf(":") + 1);
            int parsedVal;

            if (receivedData.Contains("Shoulder Position:"))
            {
                
                if (int.TryParse(toParse, out parsedVal))
                {
                    lock (positionSync)
                    {
                        int val = (int)((parsedVal / 100.0) * armConstants.SHOULDER_RANGE);
                        if (val != shoulderPos)
                        {
                            shoulderPos = val;
                            string updateMC = "SH_REAL_UPDATE_" + shoulderPos;
                            netMan.write(rocConstants.COMID.ARMCOM, updateMC);
                        }
                    }
                }
            }
            else if (receivedData.Contains("Elbow Position:"))
            {
                if (int.TryParse(toParse, out parsedVal))
                {
                    lock (positionSync)
                    {
                        int val = (int)((parsedVal / 100.0) * armConstants.ELBOW_RANGE);
                        if (val != elbowPos)
                        {
                            elbowPos = val;
                            string updateMC = "EL_REAL_UPDATE_" + elbowPos;
                            netMan.write(rocConstants.COMID.ARMCOM, updateMC);
                        }
                    }
                }
            }
            else if (receivedData.Contains("Turn Table Position:"))
            {
                if (int.TryParse(toParse, out parsedVal))
                {
                    lock (positionSync)
                    {
                        int val = (int)((parsedVal / 100.0) * armConstants.TURNTABLE_RANGE);
                        if (val != turnTablePos)
                        {
                            turnTablePos = val;
                            string updateMC = "TT_REAL_UPDATE_" + turnTablePos;
                            netMan.write(rocConstants.COMID.ARMCOM, updateMC);
                        }
                    }
                }
            }
        }

        void netMan_incomingArm(string incomingString)
        {
            if(incomingString.StartsWith("ARM_EL_")){
                incomingString = incomingString.Replace("ARM_EL_", "");
                int elbowPos;
                if(int.TryParse(incomingString,out elbowPos)){
                    updateElbow(elbowPos);
                }
            }
            else if(incomingString.StartsWith("ARM_SH_")){
                incomingString = incomingString.Replace("ARM_SH_","");
                int shoulderPos;
                if(int.TryParse(incomingString,out shoulderPos)){
                    updateShoulder(shoulderPos);
                }
            }
            else if(incomingString.StartsWith("ARM_TT_")){
                incomingString = incomingString.Replace("ARM_TT_","");
                int turnTablePos;
                if(int.TryParse(incomingString, out turnTablePos)){
                    updateTurnTable(turnTablePos);
                }
            }
            else if(incomingString.Contains("ARM_EMERGENCY_STOP")){
                armDuino.write("EMERSTOP");
                lock (positionSync)
                {
                    string updateMC = "EM_SH_REAL_UPDATE_" + shoulderPos;  //update mission control with shoulderPos after emergency stop WITH special "EM_" tag
                    netMan.write(rocConstants.COMID.ARMCOM, updateMC);

                    updateMC = "EM_EL_REAL_UPDATE_" + elbowPos;  //update mission control with elbowPos after emergency stop WITH special "EM_" tag
                    netMan.write(rocConstants.COMID.ARMCOM, updateMC);

                    updateMC = "EM_TT_REAL_UPDATE_" + turnTablePos;  //update mission control with elbowPos after emergency stop WITH special "EM_" tag
                    netMan.write(rocConstants.COMID.ARMCOM, updateMC);
                }
            }
            else if (incomingString.StartsWith("ARM_WR_"))
            {
                int up;
                int left;
                int right;
                if (int.TryParse(incomingString.Substring(9, 3), out up))//get up command
                {
                    handDuino.write("U:" + up);
                    if (int.TryParse(incomingString.Substring(15, 3), out right)) //get right command
                    {
                        handDuino.write("R:" + right);
                        if (int.TryParse(incomingString.Substring(21, 3), out left))
                        {
                            handDuino.write("L:" + left);
                        }
                    }
                }
            }
            else if (incomingString.StartsWith("ARM_GR_"))
            {
                int gripperPos;
                incomingString = incomingString.Replace("ARM_GR_","");
                if (int.TryParse(incomingString, out gripperPos))
                {
                    handDuino.write("G:" + gripperPos);
                }
            }
        }

        private void updateTurnTable(int target)
        {
            target = target.Constrain(armConstants.MIN_TURNTABLE_ANGLE,armConstants.MAX_TURNTABLE_ANGLE);
            armDuino.write("TTPOS:" + target);
        }

        private void updateShoulder(int target)
        {
            target = target.Constrain(armConstants.MIN_SHOULDER_ANGLE, armConstants.MAX_SHOULDER_ANGLE);
            armDuino.write("S1POS:" + target);
        }

        private void updateElbow(int target)
        {
            target = target.Constrain(armConstants.MIN_ELBOW_ANGLE, armConstants.MAX_ELBOW_ANGLE);
            armDuino.write("ELPOS:" + target);
        }
    }

    public class cameraManager
    {
        private FilterInfoCollection videoDevices;
        private Dictionary<String, VideoCaptureDevice> cameraMap;


        private static cameraManager instance;
        public static cameraManager getInstance()
        {
            if (instance == null)
            {
                instance = new cameraManager();
            }
            return instance;
        }

        private cameraManager()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            cameraMap = new Dictionary<string, VideoCaptureDevice>();
        }

        /// <summary>
        /// must be called before any cameras are retrieved!!! The assignmentSheet tells which cameras are which and corespond with the monikerString. Uses the default assignmentSheet if none is passed in.
        /// returns false if there were less cameras plugged in than are supposed to be assigned to or if something went wrong.
        /// assignmentSheet KEY: firstVal = PT_left , secondVal = PT_right
        /// </summary>
        public bool assignCameras(int[] assignmentSheet)
        {
            bool toReturn = true;
            try
            {
                if (assignmentSheet.Count() < videoDevices.Count)
                {
                    toReturn = false;
                }

                for (int i = 0; i < videoDevices.Count; i++)
                {
                    cameraMap.Add((((rocConstants.CAMS)i).ToString()), new VideoCaptureDevice(videoDevices[assignmentSheet[i]].MonikerString)); //really complicated way to just add the cameras in the order specified by the assignmentSheet 
                }
            }
            catch
            {
                toReturn = false;
            }
            return toReturn;
        }

        /// <summary>
        /// must be called before any cameras are retrieved!!! The assignmentSheet tells which cameras are which and corespond with the monikerString. Uses the default assignmentSheet if none is passed in.
        /// returns false if there were less cameras plugged in than are supposed to be assigned to or if something went wrong.
        /// assignmentSheet KEY: firstVal = PT_left , secondVal = PT_right
        /// </summary>
        public bool assignCameras()
        {
            bool toReturn = true;
            try
            {
                int[] assignmentSheet = rocConstants.defaultCameraAssignments;
                if (assignmentSheet.Count() < videoDevices.Count)
                {
                    toReturn = false;
                }

                for (int i = 0; i < videoDevices.Count; i++)
                {
                    cameraMap.Add((((rocConstants.CAMS)i).ToString()), new VideoCaptureDevice(videoDevices[assignmentSheet[i]].MonikerString)); //really complicated way to just add the cameras in the order specified by the assignmentSheet 
                }
            }
            catch
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool getCamera(rocConstants.CAMS cameraID, out VideoCaptureDevice camera)
        {
            try
            {
                camera = cameraMap[cameraID.ToString()];
            }
            catch
            {
                camera = null;
                return false;
            }
            return true;
        }
    }

    public static class armConstants
    {
        public const int MAX_SHOULDER_ANGLE = 570;   /////NOTE: These r NOT angles, they are angles multiplied by whatever SHOULDER_RESOLUTION_MULTIPLIER determines is neccessary
        public const int MIN_SHOULDER_ANGLE = 0;
        public const int SHOULDER_RANGE = 570;

        public const int MAX_ELBOW_ANGLE = 1200;   /////NOTE: These r NOT angles, they are angles multiplied by whatever ELBOW_RESOLUTION_MULTIPLIER determines is neccessary
        public const int MIN_ELBOW_ANGLE = 0;
        public const int ELBOW_RANGE = 1200;

        public const int MAX_TURNTABLE_ANGLE = 1430;   /////NOTE: These r NOT angles, they are angles multiplied by whatever TURNTABLE_RESOLUTION_MULTIPLIER determines is neccessary
        public const int MIN_TURNTABLE_ANGLE = 0;
        public const int TURNTABLE_RANGE = 1430;

        public const int MAX_GRIPPER = 100;
        public const int MIN_GRIPPER = 0;
    }

    public static class ExtensionMethods
    {
        public static double Constrain(this double value, double min, double max)
        {
            if(value > max){
                return max;
            }
            else if (value < min)
            {
                return min;
            }
            else
            {
                return value;
            }
        }

        public static int Constrain(this int value, int min, int max)
        {
            if (value > max)
            {
                return max;
            }
            else if (value < min)
            {
                return min;
            }
            else
            {
                return value;
            }
        }
    }
}
