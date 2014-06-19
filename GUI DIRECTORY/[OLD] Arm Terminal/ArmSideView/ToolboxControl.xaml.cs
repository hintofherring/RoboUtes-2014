using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XboxController;
using System.Threading;

namespace ArmSideView {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("ArmSideView", true)]
    public partial class ArmSide : UserControl {

        Thread elbowUpdateThread;
        Thread shoulderUpdateThread;

        double commandedElbowAngle;
        double elbowRate;
        object elbowSync = 1;

        double commandedShoulderAngle;
        double shoulderRate;
        object shoulderSync = 1;

        private XboxController.XboxController _xboxController;
        public XboxController.XboxController XboxController
        {
            set
            {
                _xboxController = value;
                _xboxController.ThumbStickRight += _xboxController_ThumbStickRight;
                _xboxController.TriggerLeft += _xboxController_TriggerLeft;
                _xboxController.TriggerRight += _xboxController_TriggerRight;
            }
        }

        double gShoulderAngle = 0;
        double gElbowAngle = 0;
        double gElbowOffsetAngle = -180;

        double aShoulderAngle = 0;
        double aElbowAngle = 0;
        double aElbowOffsetAngle = -180;

        public ArmSide() {
            InitializeComponent();
            gRec2.RenderTransform = new RotateTransform(gElbowOffsetAngle);
            aRec2.RenderTransform = new RotateTransform(aElbowOffsetAngle);
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
                    Dispatcher.Invoke(() => updateGoalShoulder(commandedShoulderAngle));
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
                    commandedElbowAngle -= elbowRate;   //TODO: This is currently inverted, make it += instead
                    Dispatcher.Invoke(() => updateGoalElbow(commandedElbowAngle));
                    Thread.Sleep(20);
                }
            }
        }

        void _xboxController_TriggerLeft(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            float val = args.GetLeftTrigger();
            val = val / 2;  //keep it slow
            Console.WriteLine("val: " + val);
            lock (shoulderSync)
            {
                shoulderRate = -val;    //left trigger is down
            }
        }

        void _xboxController_TriggerRight(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            float val = args.GetRightTrigger();
            val = val / 2;  //keep it slow
            Console.WriteLine("val: " + val);
            lock (shoulderSync)
            {
                shoulderRate = val;    //right trigger is up
            }
        }

        void _xboxController_ThumbStickRight(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetRightThumbStick();
            double Y = vec.Item2.Map(-1, 1, -2, 2);
            lock (elbowSync)
            {
                elbowRate = Y;
            }
        }

        /// <summary>
        /// Takes in the angle between the shoulder-arm and the forearm and updates the rendering of the ACTUAL position accordingly
        /// Takes a value from 0-360
        /// </summary>
        /// <param name="angle"></param>
        public void updateActualElbow(double angle) {
            aElbowAngle = angle;
            aRec2.RenderTransform = new RotateTransform(180 - aElbowAngle + (aShoulderAngle));
        }

        /// <summary>
        /// Takes in the angle between the shoulder-arm and the forearm and updates the rendering of the GOAL position accordingly
        /// Takes a value from 0-360
        /// </summary>
        /// <param name="angle"></param>
        public void updateGoalElbow(double angle) {
            gElbowAngle = angle;
            gRec2.RenderTransform = new RotateTransform(180 - gElbowAngle + (gShoulderAngle));
        }

        /// <summary>
        /// Takes a value from 0-360 (usually from 0-90 though)
        /// </summary>
        /// <param name="angle"></param>
        public void updateActualShoulder(double angle) {
            aShoulderAngle = -angle;
            aRec1.RenderTransform = new RotateTransform(aShoulderAngle);
            Canvas.SetLeft(aRec2, Canvas.GetLeft(aRec1) + (aRec1.Width * Math.Cos(ConvertToRadians(aShoulderAngle)))); //set rec2 dist from left
            Canvas.SetBottom(aRec2, Canvas.GetBottom(aRec1) + (aRec1.Width * Math.Sin(ConvertToRadians(-aShoulderAngle)))); //set rec2 dist from top
            updateActualElbow(aElbowAngle);
        }

        /// <summary>
        /// Takes a value from 0-360 (usually from 0-90 though)
        /// </summary>
        /// <param name="angle"></param>
        public void updateGoalShoulder(double angle) {
            gShoulderAngle = -angle;
            gRec1.RenderTransform = new RotateTransform(gShoulderAngle);
            Canvas.SetLeft(gRec2, Canvas.GetLeft(gRec1) + (gRec1.Width * Math.Cos(ConvertToRadians(gShoulderAngle)))); //set rec2 dist from left
            Canvas.SetBottom(gRec2, Canvas.GetBottom(gRec1) + (gRec1.Width * Math.Sin(ConvertToRadians(-gShoulderAngle)))); //set rec2 dist from top
            updateGoalElbow(gElbowAngle);
        }

        public double ConvertToRadians(double angle) {
            return (Math.PI / 180) * angle;
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
