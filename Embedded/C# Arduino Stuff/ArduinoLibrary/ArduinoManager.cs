using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace ArduinoLibrary
{
    public class ArduinoManager
    {

        private static Dictionary<string,Arduino> ArduinoMap;
        private static ArduinoManager instance;

        public static ArduinoManager Instance {
            get {
                if (instance == null) {
                    instance = new ArduinoManager();
                    ArduinoMap = new Dictionary<string, Arduino>();
                }
                return instance;
            }
        }

        private ArduinoManager() {

        }

        /// <summary>
        /// Must be called before any Arduinos can be connected to. Returns true if ANY Arduinos were found. If non-standard Arduinos need to be talked to, pass in a string array with their names
        /// </summary>
        /// <returns></returns>
        public bool findArduinos() {
            string[] COMPorts = SerialPort.GetPortNames();
            /*Console.WriteLine("FOUND COM PORTS: ");
            foreach (string serialPort in COMPorts)
            {
                Console.WriteLine(" "+serialPort);
            }*/
            bool foundAny = false;
            foreach (string potentialArduino in COMPorts) {
                if (identify(ref ArduinoMap, potentialArduino)) {
                    foundAny = true;
                }
            }
            return foundAny;
        }

        private bool identify(ref Dictionary<string, Arduino> _ArduinoMap, string _potentialArduino) {
            if (!_ArduinoMap.ContainsKey(_potentialArduino)) {
                SerialPort temp;
                try {
                    temp = new SerialPort(_potentialArduino);
                    temp.RtsEnable = !temp.RtsEnable;
                    Thread.Sleep(300);
                    temp.RtsEnable = !temp.RtsEnable;
                    temp.Open();
                    string toWrite = Arduino_Codes.IDENTITY_QUERY;
                    temp.WriteLine(toWrite);
                    Thread.Sleep(500);
                    string ID = temp.ReadExisting();
                    if (ID.Contains(Arduino_Codes.ARM_IDENTITY_RESPONSE))
                    {
                        _ArduinoMap.Add(Arduino_Codes.ARM_IDENTITY, new Arduino(temp, Arduino_Codes.ARM_IDENTITY));
                        return true;
                    }
                    else if (ID.Contains(Arduino_Codes.HAND_IDENTITY_RESPONSE))
                    {
                        _ArduinoMap.Add(Arduino_Codes.HAND_IDENTITY, new Arduino(temp, Arduino_Codes.HAND_IDENTITY));
                        return true;
                    }
                    else if (ID.Contains(Arduino_Codes.DRIVEFRONT_IDENTITY_RESPONSE))
                    {
                        _ArduinoMap.Add(Arduino_Codes.DRIVEFRONT_IDENTITY, new Arduino(temp, Arduino_Codes.DRIVEFRONT_IDENTITY));
                        return true;
                    }
                    else if (ID.Contains(Arduino_Codes.DRIVEBACK_IDENTITY_RESPONSE))
                    {
                        _ArduinoMap.Add(Arduino_Codes.DRIVEBACK_IDENTITY, new Arduino(temp, Arduino_Codes.DRIVEBACK_IDENTITY));
                        return true;
                    }
                    else if (ID.Contains(Arduino_Codes.PT_IDENTITY_RESPONSE))
                    {
                        _ArduinoMap.Add(Arduino_Codes.PT_IDENTITY, new Arduino(temp, Arduino_Codes.PT_IDENTITY));
                        return true;
                    }

                    temp.Dispose(); //Gets rid of safe handle issue! Or at least appears to!
                }
                catch {
                    return false;
                }
            }
            return false;
        }

        public Arduino getArmArduino() {
            if (ArduinoMap.ContainsKey(Arduino_Codes.ARM_IDENTITY))
            {
                return ArduinoMap[Arduino_Codes.ARM_IDENTITY];
            }
            else {
                throw new Exception("No Arm Arduino Exists");
            }
        }

        public Arduino getHandArduino()
        {
            if (ArduinoMap.ContainsKey(Arduino_Codes.HAND_IDENTITY))
            {
                return ArduinoMap[Arduino_Codes.HAND_IDENTITY];
            }
            else
            {
                throw new Exception("No Hand Arduino Exists");
            }
        }

        public Arduino getDriveFrontArduino()
        {
            if (ArduinoMap.ContainsKey(Arduino_Codes.DRIVEFRONT_IDENTITY))
            {
                return ArduinoMap[Arduino_Codes.DRIVEFRONT_IDENTITY];
            }
            else
            {
                throw new Exception("No Drive Front Arduino Exists");
            }
        }

        public Arduino getDriveBackArduino()
        {
            if (ArduinoMap.ContainsKey(Arduino_Codes.DRIVEBACK_IDENTITY))
            {
                return ArduinoMap[Arduino_Codes.DRIVEBACK_IDENTITY];
            }
            else
            {
                throw new Exception("No Drive Back Arduino Exists");
            }
        }

        public Arduino getPanTiltArduino()
        {
            if (ArduinoMap.ContainsKey(Arduino_Codes.PT_IDENTITY))
            {
                return ArduinoMap[Arduino_Codes.PT_IDENTITY];
            }
            else
            {
                throw new Exception("No Drive Back Arduino Exists");
            }
        }

    }

    public static class Arduino_Codes
    {
        /////////////////////////////////////////////////////////////
        ///////////////  UNIVERSAL SEND CODES - Apply to all arduinos
        /////////////////////////////////////////////////////////////
        public static readonly string IDENTITY_QUERY = (char)1 + "";
        //public static readonly string START_OF_TRANSMISSION = (char)2 + "\n";
        public static readonly string END_OF_TRANSMISSION = (char)3 + "\n";
        public static readonly string HEARTBEAT_QUERY = (char)4 + "";

        /////////////////////////////////////////////////////////////
        ///////////  UNIVERSAL RESPONSE CODES - Apply to all arduinos
        /////////////////////////////////////////////////////////////
        public static readonly string HEARTBEAT_RESPONSE = "|";
        public static readonly string ARDUINO_READY = "~";


        /////////////////////////////////////////////////////////////
        ///////////////          ARM CODES - Apply to the arm arduino
        /////////////////////////////////////////////////////////////
        public static readonly string ARM_IDENTITY_RESPONSE = "POLO->ARM";
        public static readonly string ARM_IDENTITY = "ARM";

        /////////////////////////////////////////////////////////////
        ///////////////      HAND CODES - Apply to the hand arduino
        /////////////////////////////////////////////////////////////
        public static readonly string HAND_IDENTITY_RESPONSE = "POLO->HAND";
        public static readonly string HAND_IDENTITY = "HAND";

        /////////////////////////////////////////////////////////////
        ///////////////      DRIVE FRONT CODES - Apply to the drive front arduino
        /////////////////////////////////////////////////////////////
        public static readonly string DRIVEFRONT_IDENTITY_RESPONSE = "POLO->DRIVEFRONT";
        public static readonly string DRIVEFRONT_IDENTITY = "DRIVEFRONT";

        /////////////////////////////////////////////////////////////
        ///////////////      DRIVE BACK CODES - Apply to the drive back arduino
        /////////////////////////////////////////////////////////////
        public static readonly string DRIVEBACK_IDENTITY_RESPONSE = "POLO->DRIVEBACK";
        public static readonly string DRIVEBACK_IDENTITY = "DRIVEBACK";

        /////////////////////////////////////////////////////////////
        ///////////////      PAN/TILT CODES - Apply to the pan tilt arduino
        /////////////////////////////////////////////////////////////
        public static readonly string PT_IDENTITY_RESPONSE = "POLO->PT";
        public static readonly string PT_IDENTITY = "PT";
    }
    
    
}
