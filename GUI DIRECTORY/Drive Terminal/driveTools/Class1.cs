using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XboxController;
using System.Threading;
using commSockServer;

namespace driveTools
{
    public class driveTransmitter {
        Timer sendTimer;
        private driveInputManager driveInput;
        private commSockReceiver CSR;

        int leftVal = 0;
        int rightVal = 0;
        int oldLeftVal = 0;
        int oldRightVal = 0;
        object sync = 1;

        public driveTransmitter(driveInputManager _driveInput, commSockReceiver _CSR) {
            CSR = _CSR;
            driveInput = _driveInput;
            driveInput.newLeftValue += driveInput_newLeftValue;
            driveInput.newRightValue += driveInput_newRightValue;
            sendTimer = new Timer(sendTimerCallback, null, 100, 100);
        }

        private void sendTimerCallback(object state) {
            lock (sync) {
                if (leftVal != oldLeftVal) {
                    oldLeftVal = leftVal;
                    CSR.write("L:" + leftVal);
                }

                if (rightVal != oldRightVal) {
                    oldRightVal = rightVal;
                    CSR.write("R:" + rightVal);
                }
            }
        }

        void driveInput_newRightValue(int obj) {
            lock (sync) {
                rightVal = obj;
            }
        }

        void driveInput_newLeftValue(int obj) {
            lock (sync) {
                leftVal = obj;
            }
        }
    }
    public class driveInputManager
    {
        XboxController.XboxController xboxController;

        public event Action<int> newLeftValue;
        public event Action<int> newRightValue;
        object sync = 1;
        private readonly string valFormat = "000";

        private volatile int throttle = 50; //stop
        private volatile int direction = 0; //forward

        private volatile float _throttleSensitivity = 1;

        /// <summary>
        /// Magnifies the strength of throttle input. Can be passed any int between 0 and 5.
        /// </summary>
        public float throttleSensitivity {
            set {
                float temp = value;
                _throttleSensitivity = temp.Constrain(0, 5);
            }
            get {
                return (int)(_throttleSensitivity);
            }
        }

        private volatile float _turnMagnification = 1;

        /// <summary>
        /// Magnifies the strength of turn input. Can be passed any value between 0 and 5.
        /// </summary>
        public float turnMagnification {
            set {
                double temp = value;
                _turnMagnification = (float)temp.Constrain(0, 5);
            }
            get {
                return _turnMagnification;
            }
        }

        public driveInputManager(XboxController.XboxController _xboxController) {
            xboxController = _xboxController;
            xboxController.ThumbStickLeft += xboxController_ThumbStickLeft;
            xboxController.ThumbStickRight += xboxController_ThumbStickRight;
        }

        private void xboxController_ThumbStickRight(object sender, EventArgs e) { //DIRECTION
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetRightThumbStick();
            float newRightX = vec.Item1.Map(-1, 1, -30, 30);
            direction = (int)newRightX;
            Console.WriteLine("DIRECTION: " + direction);
            updateMotors();
        }

        private void xboxController_ThumbStickLeft(object sender, EventArgs e) { //THROTTLE
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetLeftThumbStick();
            float newLeftY = (vec.Item2*_throttleSensitivity).Map(-1, 1, 0, 100);
            throttle = (int)newLeftY;
            Console.WriteLine("THROTTLE: " + throttle);
            updateMotors();
        }

        private void updateMotors() {
            int leftCommand = 50; //default is stopped
            int rightCommand = 50; //default is stopped

            if (throttle > 50) { //FORWARD
                if (direction < 0) {      //Left turn
                    rightCommand = (int)((throttle) - ((-direction) * _turnMagnification)).Constrain(50, 100); //direction is negated to make it positive...
                    leftCommand = (int)(throttle).Constrain(0, 100);
                }
                else if (direction > 0) {       //right turn
                    rightCommand = (int)(throttle).Constrain(0, 100);
                    leftCommand = (int)((throttle) - (direction * _turnMagnification)).Constrain(50, 100);
                }
                else { //going straight ---- if(direction == 0)
                    leftCommand = (int)(throttle).Constrain(0, 100);
                    rightCommand = (int)(throttle).Constrain(0, 100);
                }
            }

            else if(throttle < 50){ //REVERSE
                if (direction < 0) {      //Left turn
                    rightCommand = (int)((throttle) - (direction * _turnMagnification)).Constrain(0,50); //direction is negated to make it positive...
                    leftCommand = (int)(throttle).Constrain(0, 100);
                }
                else if (direction > 0) {       //right turn
                    rightCommand = (int)(throttle).Constrain(0, 100);
                    leftCommand = (int)((throttle) - ((-direction) * _turnMagnification)).Constrain(0, 50);
                }
                else { //going straight ---- if(direction == 0)
                    leftCommand = (int)(throttle).Constrain(0, 100);
                    rightCommand = (int)(throttle).Constrain(0, 100);
                }
            }

            else if (throttle == 50 && direction != 0) { //turn in place, tank drive style...
                if (direction < 0) { //left turn in place
                    direction = -direction; //direction is negative here, so make it positive
                    leftCommand = 50 + direction;
                    rightCommand = 50 - direction;
                }
                else if (direction > 0) { //right turn in place
                    leftCommand = 50 - direction;
                    rightCommand = 50 + direction;
                }
            }

            else { //stop if throttle and direction are both neutral.
                leftCommand = 50;
                rightCommand = 50;
            }


            if (newLeftValue != null) {
                newLeftValue(leftCommand);
            }
            if (newRightValue != null) {
                newRightValue(rightCommand);
            }
        }


        /*      // old, tank drive code
                void xboxController_ThumbStickRight(object sender, EventArgs e) {
                    XboxEventArgs args = (XboxEventArgs)e;
                    Tuple<float, float> vec = args.GetRightThumbStick();
                    float newRightY = vec.Item2.Map(-1, 1, 0, 100);
                    if (newRightValue != null) {
                        newRightValue((int)(newRightY *_sensitivity));
                    }
                }

                void xboxController_ThumbStickLeft(object sender, EventArgs e) {
                    XboxEventArgs args = (XboxEventArgs)e;
                    Tuple<float, float> vec = args.GetLeftThumbStick();
                    float newLeftY = vec.Item2.Map(-1, 1, 0, 100);
                    if (newLeftValue != null) {
                        newLeftValue((int)(newLeftY*_sensitivity));
                    }
                }*/
    }

    public static class ExtensionMethods {
        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget) {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public static double Constrain(this double value, double min, double max){
            if (value > max) {
                return max;
            }
            else if (value < min) {
                return min;
            }
            else {
                return value;
            }
        }
        public static int Constrain(this int value, int min, int max) {
            if (value > max) {
                return max;
            }
            else if (value < min) {
                return min;
            }
            else {
                return value;
            }
        }
        public static float Constrain(this float value, float min, float max) {
            if (value > max) {
                return max;
            }
            else if (value < min) {
                return min;
            }
            else {
                return value;
            }
        }
    }
}
