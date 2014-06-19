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
        private armInputManager armInput;
        Timer elbowTimer;
        Timer shoulderTimer;
        Timer turnTableTimer;
        bool elbowTimerExpired = false;
        bool shoulderTimerExpired = false;
        bool turnTableTimerExpired = false;

        int delay = 100;    //How often new commands will be sent, used to avoid overflowing the line (Serial or internet)

        int oldElbow = 0;
        int oldTurnTable = 0;
        int oldShoulder = 0;

        public localArmCommandTransmitter(Arduino _armArduino , armInputManager _armInput)
        {
            armArduino = _armArduino;
            armInput = _armInput;
            armInput.targetElbowChanged += armInput_targetElbowChanged;
            armInput.targetShoulderChanged += armInput_targetShoulderChanged;
            armInput.targetTurnTableChanged += armInput_targetTurnTableChanged;
            elbowTimer = new Timer(elbowTimerCallback, null, 0, delay);
            shoulderTimer = new Timer(shoulderTimerCallback, null, 0, delay);
            turnTableTimer = new Timer(turnTableTimerCallback, null, 0, delay);
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

        void armInput_targetTurnTableChanged(double newAngle)
        {
            if (( ((int)newAngle).Map(0, 90, 0, 1023) != oldTurnTable) && turnTableTimerExpired)
            {
                oldTurnTable = ((int)newAngle).Map(0, 90, 0, 1023);
                turnTableTimerExpired = false;
                armArduino.write("TTPOS:" + ((int)newAngle).Map(0, 90, 0, 1023)); //TODO: This is temporary! When finished we will be sending just an angle.
            }
        }

        void armInput_targetShoulderChanged(double newAngle)
        {
            if (( ((int)newAngle).Map(0, 90, 0, 1023) != oldShoulder) && shoulderTimerExpired)
            {
                oldShoulder = ((int)newAngle).Map(0, 90, 0, 1023);
                shoulderTimerExpired = false;
                armArduino.write("S1POS:" + ((int)newAngle).Map(0, 90, 0, 1023)); //TODO: This is temporary! When finished we will be sending just an angle.
            }
        }

        void armInput_targetElbowChanged(double newAngle)
        {
            if (( ((int)newAngle).Map(0, 120, 0, 1023) != oldElbow) && elbowTimerExpired)
            {
                oldElbow = ((int)newAngle).Map(0, 120, 0, 1023);
                elbowTimerExpired = false;
                armArduino.write("ELPOS:" + ((int)newAngle).Map(0, 120, 0, 1023)); //TODO: This is temporary! When finished we will be sending just an angle.
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

        double commandedTurntableAngle;
        double turnTableRate;
        object turnTableSync = 1;

        double commandedElbowAngle;
        double elbowRate;
        object elbowSync = 1;

        double commandedShoulderAngle;
        double shoulderRate;
        object shoulderSync = 1;

        Thread elbowUpdateThread;
        Thread shoulderUpdateThread;
        Thread turntableUpdateThread;

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
            turntableUpdateThread = new Thread(new ThreadStart(turnTableUpdate));
            turntableUpdateThread.Start();
            elbowUpdateThread = new Thread(new ThreadStart(elbowUpdate));
            elbowUpdateThread.Start();
            shoulderUpdateThread = new Thread(new ThreadStart(shoulderUpdate));
            shoulderUpdateThread.Start();
        }

        void shoulderUpdate()
        {
            while (true)
            {
                lock (shoulderSync)
                {
                    commandedShoulderAngle += shoulderRate;
                    commandedShoulderAngle = commandedShoulderAngle.Constrain(0, 90);
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
                    commandedElbowAngle -= elbowRate;   //TODO: This is currently inverted, make it += instead to un-invert it
                    commandedElbowAngle = commandedElbowAngle.Constrain(0, 120);
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
                    commandedTurntableAngle += turnTableRate;
                    commandedTurntableAngle = commandedTurntableAngle.Constrain(0, 90);
                    if (turnTableRate != 0)
                    {
                        if (targetTurnTableChanged != null)
                        {
                            targetTurnTableChanged(commandedTurntableAngle);
                        }
                    }
                    Thread.Sleep(20);
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
    }
}
