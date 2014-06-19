using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XboxController;
using ArduinoLibrary;
using System.Threading;
using ArmControlTools;

namespace ArmControlTools
{
    public class localArmCommandTransmitter
    {
        private Arduino armArduino;
        private Arduino handArduino;
        private armInputManager armInput;

        Timer elbowTimer;
        Timer shoulderTimer;
        Timer turnTableTimer;
        Timer gripperTimer;
        Timer wristTimer;

        volatile bool elbowTimerExpired = false;
        volatile bool shoulderTimerExpired = false;
        volatile bool turnTableTimerExpired = false;
        volatile bool gripperTimerExpired = false;
        volatile bool wristTimerExpired = false;

        private int delay = 100;    //How often new commands will be sent, used to avoid overflowing the line (Serial or internet)

        int oldElbow = 0;
        int oldTurnTable = 0;
        int oldShoulder = 0;
        int oldGripper = 0;
        wristPositionData oldWrist = new wristPositionData(0, 0, 0);

        public localArmCommandTransmitter(Arduino _armArduino, Arduino _handArduino, armInputManager _armInput)
        {
            armArduino = _armArduino;
            armInput = _armInput;
            handArduino = _handArduino;

            armInput.targetElbowChanged += armInput_targetElbowChanged;
            armInput.targetShoulderChanged += armInput_targetShoulderChanged;
            armInput.targetTurnTableChanged += armInput_targetTurnTableChanged;
            armInput.targetWristChanged += armInput_targetWristChanged;
            armInput.targetGripperChanged += armInput_targetGripperChanged;

            elbowTimer = new Timer(elbowTimerCallback, null, 0, delay);
            shoulderTimer = new Timer(shoulderTimerCallback, null, 0, delay);
            turnTableTimer = new Timer(turnTableTimerCallback, null, 0, delay);
            wristTimer = new Timer(wristTimerCallback, null, 0, delay);
            gripperTimer = new Timer(gripperTimerCallback, null, 0, delay);

        }

        private void wristTimerCallback(object state)
        {
            wristTimerExpired = true;
        }

        private void gripperTimerCallback(object state)
        {
            gripperTimerExpired = true;
        }

        private void turnTableTimerCallback(object state)
        {
            turnTableTimerExpired = true;
        }

        private void shoulderTimerCallback(object state)
        {
            shoulderTimerExpired = true;
        }

        private void elbowTimerCallback(object state)
        {
            elbowTimerExpired = true;
        }

        void armInput_targetWristChanged(wristPositionData newPosition)
        {
            if (!newPosition.Equals(oldWrist) && wristTimerExpired)
            {
                oldWrist = newPosition;
                wristTimerExpired = false;
                handArduino.write("U:" + newPosition.upVal);
                handArduino.write("L:" + newPosition.leftVal);
                handArduino.write("R:" + newPosition.rightVal);
            }
        }

        void armInput_targetGripperChanged(double newGrip)
        {
            if (((int)newGrip != oldGripper) && gripperTimerExpired)
            {
                oldGripper = (int)newGrip;
                gripperTimerExpired = false;
                handArduino.write("G:"+ ((int)newGrip) );
            }
        }

        void armInput_targetTurnTableChanged(double newAngle)
        {
            if (( ((int)newAngle) != oldTurnTable) && turnTableTimerExpired)
            {
                oldTurnTable = ((int)newAngle);
                turnTableTimerExpired = false;
                newAngle = newAngle.Constrain(armConstants.MIN_TURNTABLE_ANGLE, armConstants.MAX_TURNTABLE_ANGLE);
                armArduino.write("TTPOS:"+ ((int)newAngle) );
            }
        }

        void armInput_targetShoulderChanged(double newAngle)
        {
            if (( ((int)newAngle) != oldShoulder) && shoulderTimerExpired)
            {
                oldShoulder = ((int)newAngle);
                shoulderTimerExpired = false;
                newAngle = newAngle.Constrain(armConstants.MIN_SHOULDER_ANGLE, armConstants.MAX_SHOULDER_ANGLE);
                armArduino.write("S1POS:" + ((int)newAngle) );
            }
        }

        void armInput_targetElbowChanged(double newAngle)
        {
            if (( ((int)newAngle) != oldElbow) && elbowTimerExpired)
            {
                oldElbow = ((int)newAngle);
                elbowTimerExpired = false;
                newAngle = newAngle.Constrain(armConstants.MIN_ELBOW_ANGLE, armConstants.MAX_ELBOW_ANGLE);
                armArduino.write("ELPOS:" + ((int)newAngle) ); //TODO: This is temporary! When finished we will be sending just an angle.
            }
        }
    }

    public class armInputManager
    {
        XboxController.XboxController xboxController;
        private static armInputManager actualInstance;

        public delegate void ChangedJointPositionEventHandler(double newAngle);
        public event ChangedJointPositionEventHandler targetElbowChanged;
        public event ChangedJointPositionEventHandler targetShoulderChanged;
        public event ChangedJointPositionEventHandler targetTurnTableChanged;
        public event ChangedJointPositionEventHandler targetGripperChanged;
        public event ChangedJointPositionEventHandler targetWristRotationAngleChanged;

        public delegate void ChangedWristPositionEventHandler(wristPositionData newPosition);
        public event ChangedWristPositionEventHandler targetWristChanged;

        double commandedTurnTableAngle;
        double turnTableRate;
        object turnTableSync = 1;

        double commandedElbowAngle;
        double elbowRate;
        object elbowSync = 1;

        double commandedShoulderAngle;
        double shoulderRate;
        object shoulderSync = 1;

        int commandedGripper = 100;
        int gripperRate;
        object gripperSync = 1;

        Thread elbowUpdateThread;
        Thread shoulderUpdateThread;
        Thread turntableUpdateThread;
        Thread wristUpdateThread;
        Thread gripperUpdateThread;

        ////////// Special variable for wrist stuff
        volatile bool newWristPositions = false;
        double MAX_MAGNITUDE = 100; //can basically be used as a sensitivity adjustment

        double upPerc = 0;
        double leftPerc = 0;
        double rightPerc = 0;
        double upMag = 0;
        double leftMag = 0;
        double rightMag = 0;

        object wristSync = 1;
        //////////

        private volatile bool shoulderInitialized = false;
        private volatile bool elbowInitialized = false;
        private volatile bool turnTableInitialized = false;

        public static armInputManager getInstance(XboxController.XboxController _xboxController)
        {
            if (actualInstance != null)
            {
                return actualInstance;
            }
            else
            {
                actualInstance = new armInputManager(_xboxController);
                return actualInstance;
            }
        }

        private armInputManager(XboxController.XboxController _xboxController)
        {
            xboxController = _xboxController;

            xboxController.ThumbStickRight += xboxController_ThumbStickRight;
            xboxController.TriggerLeft += xboxController_TriggerLeft;
            xboxController.TriggerRight += xboxController_TriggerRight;
            xboxController.ThumbStickLeft += xboxController_ThumbStickLeft;
            xboxController.ButtonRightShoulderPressed += xboxController_ButtonRightShoulderPressed;
            xboxController.ButtonLeftShoulderPressed += xboxController_ButtonLeftShoulderPressed;
            xboxController.ButtonRightShoulderReleased += bumperReleased;
            xboxController.ButtonLeftShoulderReleased += bumperReleased;

            turntableUpdateThread = new Thread(new ThreadStart(turnTableUpdate));
            turntableUpdateThread.Start();
            elbowUpdateThread = new Thread(new ThreadStart(elbowUpdate));
            elbowUpdateThread.Start();
            shoulderUpdateThread = new Thread(new ThreadStart(shoulderUpdate));
            shoulderUpdateThread.Start();
            wristUpdateThread = new Thread(new ThreadStart(wristUpdate));
            wristUpdateThread.Start();
            gripperUpdateThread = new Thread(new ThreadStart(gripperUpdate));
            gripperUpdateThread.Start();
        }

        private void gripperUpdate()
        {
            while (true)
            {
                lock (gripperSync)
                {
                    commandedGripper += gripperRate;
                    commandedGripper = commandedGripper.Constrain(armConstants.MIN_GRIPPER, armConstants.MAX_GRIPPER);
                    if (gripperRate != 0)
                    {
                        if (targetGripperChanged != null)
                        {
                            targetGripperChanged(100-commandedGripper); //this way 100 is fully gripped and 0 is relaxed, seems more intuituve...
                        }
                    }
                }
                Thread.Sleep(20);
            }
        }

        private void wristUpdate()
        {
            while (true)
            {
                lock (wristSync)
                {
                    if(newWristPositions){
                        wristPositionData currentData = new wristPositionData((int)upMag, (int)leftMag, (int)rightMag);
                        if (targetWristChanged != null)
                        {
                            targetWristChanged(currentData);
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }

        void shoulderUpdate()
        {
            while (true)
            {
                lock (shoulderSync)
                {
                    commandedShoulderAngle += shoulderRate;
                    commandedShoulderAngle = commandedShoulderAngle.Constrain(armConstants.MIN_SHOULDER_ANGLE, armConstants.MAX_SHOULDER_ANGLE);
                    if (shoulderRate != 0)
                    {
                        if (targetShoulderChanged != null)
                        {
                            targetShoulderChanged(commandedShoulderAngle);
                        }
                    }
                    Thread.Sleep(20);
                }
            }
        }

        void elbowUpdate()
        {
            while (true)
            {
                lock (elbowSync)
                {
                    commandedElbowAngle -= elbowRate; //this is where inverting can happen
                    commandedElbowAngle = commandedElbowAngle.Constrain(armConstants.MIN_ELBOW_ANGLE, armConstants.MAX_ELBOW_ANGLE);
                    if (elbowRate != 0)
                    {
                        if (targetElbowChanged != null)
                        {
                            targetElbowChanged(commandedElbowAngle);
                        }
                    }
                    Thread.Sleep(20);
                }
            }
        }

        void turnTableUpdate()
        {
            while (true)
            {
                lock (turnTableSync)
                {
                    commandedTurnTableAngle += turnTableRate;
                    commandedTurnTableAngle = commandedTurnTableAngle.Constrain(armConstants.MIN_TURNTABLE_ANGLE, armConstants.MAX_TURNTABLE_ANGLE);
                    if (turnTableRate != 0)
                    {
                        if (targetTurnTableChanged != null)
                        {
                            targetTurnTableChanged(commandedTurnTableAngle);
                        }
                    }
                    Thread.Sleep(20);
                }
            }
        }

        void xboxController_ThumbStickLeft(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetLeftThumbStick();
            double Y = vec.Item2.Map(-1, 1, -100, 100);
            double X = vec.Item1.Map(-1, 1, -100, 100);
            double rotationAngle = -((Math.Atan2(Y, X) * 180) / Math.PI);
            //rotationAngle += 180;
            double MAGpercent = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            if (MAGpercent > 100) //gets rid of some slight noise
            {
                MAGpercent = 100;
            }
            double MAG = ((MAGpercent / 100) * MAX_MAGNITUDE) / 100;

            if (targetWristRotationAngleChanged != null)
            {
                targetWristRotationAngleChanged(rotationAngle);
            }

            updateModel(rotationAngle, MAG);
        }

        private void updateModel(double rotationAngle, double MAG)
        {
            if ((rotationAngle > 150 && rotationAngle <= 180) || (rotationAngle <= -90 && rotationAngle >= -180))  //top left region
            {
                //calculate actuator pull
                double swingPercent = 0;
                if (rotationAngle > 150 && rotationAngle <= 180)
                {
                    swingPercent = rotationAngle.Map(150, 270, 0, 120) / 120;
                }
                else if (rotationAngle <= -90 && rotationAngle >= -180)
                {
                    swingPercent = rotationAngle.Map(-210, -90, 0, 120) / 120;
                }
                leftPerc = (1 - swingPercent) * 100;
                upPerc = swingPercent * 100;
                rightPerc = 0;

                lock (wristSync)
                {
                    upMag = upPerc * MAG;
                    rightMag = rightPerc * MAG;
                    leftMag = leftPerc * MAG;
                    newWristPositions = true;
                }
            }

            else if (rotationAngle > 30 && rotationAngle <= 150) //bottom region
            {
                //calculate actuator pull
                double swingPercent = rotationAngle.Map(30, 150, 0, 120) / 120;
                rightPerc = (1 - swingPercent) * 100;
                leftPerc = swingPercent * 100;
                upPerc = 0;

                lock (wristSync)
                {
                    upMag = upPerc * MAG;
                    rightMag = rightPerc * MAG;
                    leftMag = leftPerc * MAG;
                    newWristPositions = true;
                }
            }
            else if (rotationAngle > -90 && rotationAngle <= 30) //top right region
            {
                //calculate actuator pull
                double swingPercent = rotationAngle.Map(-90, 30, 0, 120) / 120;
                upPerc = (1 - swingPercent) * 100;
                rightPerc = swingPercent * 100;
                leftPerc = 0;

                lock (wristSync)
                {
                    upMag = upPerc * MAG;
                    rightMag = rightPerc * MAG;
                    leftMag = leftPerc * MAG;
                    newWristPositions = true;
                }
            }
        }

        void xboxController_ThumbStickRight(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetRightThumbStick();
            double X = Math.Round(vec.Item1.Map(-1, 1, -2, 2), 1); //only 2 decimals of precision
            lock (turnTableSync)
            {
                turnTableRate = X;
            }

            double Y = Math.Round(vec.Item2.Map(-1, 1, -2, 2), 1); //only 2 decimals of precision
            lock (elbowSync)
            {
                elbowRate = Y;
            }
        }

        void xboxController_TriggerLeft(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            double val = args.GetLeftTrigger();
            val = Math.Round((val / 2), 1);  //keep it slow , only 2 decimals of precision
            lock (shoulderSync)
            {
                shoulderRate = -val;    //left trigger is down
            }
        }

        void xboxController_ButtonRightShoulderPressed(object sender, EventArgs e)
        {
            lock (gripperSync)
            {
                gripperRate = 1;
            }
        }
        void xboxController_ButtonLeftShoulderPressed(object sender, EventArgs e)
        {
            lock (gripperSync)
            {
                gripperRate = -1;
            }
        }

        void bumperReleased(object sender, EventArgs e)
        {
            lock (gripperSync)
            {
                gripperRate = 0;
            }
        }


        void xboxController_TriggerRight(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            double val = args.GetRightTrigger();
            val = Math.Round((val / 2), 1);  //keep it slow , only 2 decimals of precision
            lock (shoulderSync)
            {
                shoulderRate = val;    //right trigger is up
            }
        }

        public void initShoulderPosition(int target)
        {
            if (!shoulderInitialized)
            {
                shoulderInitialized = true;
                target = target.Constrain(armConstants.MIN_SHOULDER_ANGLE, armConstants.MAX_SHOULDER_ANGLE);
                lock (shoulderSync)
                {
                    commandedShoulderAngle = target;
                    if (targetShoulderChanged != null)
                    {
                        targetShoulderChanged(commandedShoulderAngle);
                    }
                }
            }
        }

        public void initElbowPosition(int target)
        {
            if (!elbowInitialized)
            {
                elbowInitialized = true;
                target = target.Constrain(armConstants.MIN_ELBOW_ANGLE, armConstants.MAX_ELBOW_ANGLE);
                lock (elbowSync)
                {
                    commandedElbowAngle = target;
                    if (targetElbowChanged != null)
                    {
                        targetElbowChanged(commandedElbowAngle);
                    }
                }
            }
        }

        public void initTurnTablePosition(int target)
        {
            if (!turnTableInitialized)
            {
                turnTableInitialized = true;
                target = target.Constrain(armConstants.MIN_TURNTABLE_ANGLE, armConstants.MAX_TURNTABLE_ANGLE);
                lock (turnTableSync)
                {
                    commandedTurnTableAngle = target;
                    if (targetTurnTableChanged != null)
                    {
                        targetTurnTableChanged(commandedTurnTableAngle);
                    }
                }
            }
        }
    }

    public class wristPositionData
    {
        public readonly int upVal;
        public readonly int leftVal;
        public readonly int rightVal;

        public wristPositionData(int upActuator, int leftActuator, int rightActuator)
        {
            upVal = upActuator;
            leftVal = leftActuator;
            rightVal = rightActuator;
        }

        /// <summary>
        /// Overridden version of the equals method. Determines if the values within the wristPosition are the same.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            try
            {
                if (obj.GetType() != base.GetType())
                {
                    return false;
                }
                wristPositionData tempOther = (wristPositionData)obj;
                if (tempOther.leftVal == leftVal && tempOther.rightVal == rightVal && tempOther.upVal == upVal)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }

    public static class ExtensionMethods
    {
        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public static double Map(this double value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public static int Map(this int value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (int)((value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget);
        }

        public static double Constrain(this double value, double min, double max)
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

    public static class armConstants
    {
        public const int MAX_SHOULDER_ANGLE = 50;
        public const int MIN_SHOULDER_ANGLE = 0;
        public const int SHOULDER_RANGE = 50;

        public const int MAX_ELBOW_ANGLE = 120;
        public const int MIN_ELBOW_ANGLE = 0;
        public const int ELBOW_RANGE = 120;

        public const int MAX_TURNTABLE_ANGLE = 330;
        public const int MIN_TURNTABLE_ANGLE = 0;
        public const int TURNTABLE_RANGE = 330;

        public const int MAX_GRIPPER = 100;
        public const int MIN_GRIPPER = 0;
    }
}
