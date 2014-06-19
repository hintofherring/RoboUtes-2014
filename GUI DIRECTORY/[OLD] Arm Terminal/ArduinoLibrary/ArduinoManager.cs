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
        /// Must be called before any Arduinos can be connected to. Returns true if ANY Arduinos were found.
        /// </summary>
        /// <returns></returns>
        public bool findArduinos() {
            string[] COMPorts = SerialPort.GetPortNames();
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
                    temp.Open();
                    string toWrite = Arduino_Codes.IDENTITY_QUERY;
                    temp.WriteLine(toWrite);
                    Thread.Sleep(300);
                    string ID = temp.ReadExisting();
                    if (ID.Contains(Arduino_Codes.ARM_IDENTITY_RESPONSE))
                    {
                        _ArduinoMap.Add(Arduino_Codes.ARM_IDENTITY, new Arduino(temp, Arduino_Codes.ARM_IDENTITY));
                        return true;
                    }
                    else if (ID.Contains(Arduino_Codes.WRIST_IDENTITY_RESPONSE))
                    {
                        _ArduinoMap.Add(Arduino_Codes.WRIST_IDENTITY, new Arduino(temp, Arduino_Codes.WRIST_IDENTITY));
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

        public Arduino getWristArduino()
        {
            if (ArduinoMap.ContainsKey(Arduino_Codes.WRIST_IDENTITY))
            {
                return ArduinoMap[Arduino_Codes.WRIST_IDENTITY];
            }
            else
            {
                throw new Exception("No Wrist Arduino Exists");
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
        ///////////////      WRIST CODES - Apply to the wrist arduino
        /////////////////////////////////////////////////////////////
        public static readonly string WRIST_IDENTITY_RESPONSE = "POLO->WRIST";
        public static readonly string WRIST_IDENTITY = "WRIST";
    }
    
    
}
