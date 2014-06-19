using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using System.Management;


namespace ArduinoLibrary {
    public class Arduino : IDisposable {

        private SerialPort ArduinoSerial;
        /// <summary>
        /// Reports data as a string once a newline ("\n") character is seen.
        /// </summary>
        /// <param name="receivedData"></param>
        public delegate void ArduinoDataReceivedEventHandler(string receivedData);
        public event ArduinoDataReceivedEventHandler Data_Received;
        public string name;
        public string COMPORT;
        string currentString;
        private volatile bool recentData = false;
        private volatile bool arduinoReady = true;
        private volatile bool waitingForHeartbeat = false;
        private static object sync = 1;
        Thread checkConnect;
        Thread sendThread;
        private Queue<string> thingsToSend = new Queue<string>();

        public Arduino(SerialPort Arduino_Location, string _name) {
            currentString = "";
            
            if (!Arduino_Location.IsOpen) {
                throw new ArduinoImproperlyPassed("Arduino port must already be open (identified as an Arduino and whatnot) by the time it is passed into the Arduino Constructor");
            }
            COMPORT = Arduino_Location.PortName;
            ArduinoSerial = Arduino_Location;
            ArduinoSerial.DataReceived += ArduinoSerial_DataReceived;
            ArduinoSerial.ErrorReceived += ArduinoSerial_ErrorReceived;
            name = _name;
            checkConnect = new Thread(checkConnection);
            checkConnect.Start();
            sendThread = new Thread(sendData);
            sendThread.Start();
        }

        private void sendData()
        {
            while (true)
            {
                if (arduinoReady)
                {
                    lock (thingsToSend)
                    {
                        if (thingsToSend.Count > 0)
                        {
                            lock (ArduinoSerial)
                            {
                                ArduinoSerial.Write(thingsToSend.Dequeue());
                                arduinoReady = false;
                            }
                        }
                    }
                }
                Thread.Sleep(20);
            }
        }

         void ArduinoSerial_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            lock (sync) {
                recentData = true;
                waitingForHeartbeat = false;
            }
            List<string> receivedStringList = new List<string>();
            SerialPort temp = (SerialPort)sender;
            if (currentString.Length >= 10000) {
                currentString = null;
            }
            else {
                string toParse = currentString + temp.ReadLine();
                string[] tempString = (toParse).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string t in tempString) {
                    if (t.Contains(Arduino_Codes.HEARTBEAT_RESPONSE))
                    {
                        //do nothing... recentData has already been reset...
                    }
                    else if (t.Contains(Arduino_Codes.ARDUINO_READY))
                    {
                        arduinoReady = true;
                    }
                    else if (t == tempString.First())
                    {
                        receivedStringList.Add(t);
                        currentString = "";
                    }
                    else if (t != tempString.Last())
                    {
                        receivedStringList.Add(t);
                    }
                    else
                    {
                        if (t.EndsWith("\r\n") || t.EndsWith("\n"))
                        {
                            receivedStringList.Add(t);
                        }
                        else
                        {
                            currentString += t;
                        }
                    }
                }
            }
            if (Data_Received != null) {
                foreach (string received in receivedStringList) //chances are youll only ever receive one string at a time b/c the Arduino is slow, but if you get many you jsut repeatedly fire the event.
                {
                    Data_Received(received);
                }
            }
        }

        /// <summary>
        /// The reccomended way to communicate with the arduino. Is slower than writeNOW, but also much safer.
        /// </summary>
        /// <param name="data"></param>
        public void write(String data) {
            lock (thingsToSend)
            {
                thingsToSend.Enqueue(data + Arduino_Codes.END_OF_TRANSMISSION);
            }
        }

        private void checkConnection() {
            while (true) {
                Thread.Sleep(3000);
                lock (sync) {
                    if (!recentData && waitingForHeartbeat)
                    {
                        throw new ArduinoCOMCloseError("check failed! ARDUINO DISCONNECTED!"); //TODO: Maybe make this a special type of exception...
                    }
                    else if (!recentData) {
                        lock (ArduinoSerial)
                        {
                            ArduinoSerial.WriteLine(Arduino_Codes.HEARTBEAT_QUERY);
                        }
                        waitingForHeartbeat = true;
                    }
                    recentData = false;
                }
            }
        }


        void ArduinoSerial_ErrorReceived(object sender, SerialErrorReceivedEventArgs e) {
            throw new ArduinoGeneralError((SerialPort)sender, e);
        }



        public void Dispose()
        {
            ArduinoSerial.Dispose();
        }
    }

    public class ArduinoGeneralError : Exception {
        public SerialPort ArduinoSerialPortObject;
        public SerialErrorReceivedEventArgs eventArgs;
        public ArduinoGeneralError(SerialPort SP,SerialErrorReceivedEventArgs e )
        {
            ArduinoSerialPortObject = SP;
            eventArgs = e;
        }
    }

    public class ArduinoCOMCloseError : Exception {
        public ArduinoCOMCloseError(string ErrorMessage)
            : base(ErrorMessage) {
        }
    }

    public class ArduinoImproperlyPassed : Exception {
        public ArduinoImproperlyPassed(string ErrorMessage)
        :base(ErrorMessage){
        }
    }
}
