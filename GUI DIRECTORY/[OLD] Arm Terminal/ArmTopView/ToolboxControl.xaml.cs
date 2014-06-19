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

namespace ArmTopView {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("ArmTopView", true)]
    public partial class ArmTop : UserControl {
        private XboxController.XboxController _xboxController;
        public XboxController.XboxController XboxController
        {
            set
            {
                _xboxController = value;
                _xboxController.ThumbStickRight +=_xboxController_ThumbStickRight;
            }
        }

        Thread turntableUpdateThread;

        double commandedTurntableAngle;
        double turnTableRate;
        object turnTableSync = 1;

        public double maxLength = 260; //starting standard value
        public double maxRotation = 90; //starting standard value
        public ArmTop() {
            InitializeComponent();
            turntableUpdateThread = new Thread(new ThreadStart(turnTableUpdate));
            turntableUpdateThread.Start();
        }

        private void _xboxController_ThumbStickRight(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetRightThumbStick();
            double X = vec.Item1.Map(-1, 1, -2, 2);
            Console.WriteLine("val: " + X);
            lock (turnTableSync)
            {
                turnTableRate = X;
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
                    Dispatcher.Invoke(() => updateGoalArmAngle(commandedTurntableAngle));
                    Thread.Sleep(20);
                }
            }
        }

        public void updateActualArmAngle(double angle) {
            if (angle >= 0 && angle <= maxRotation) { //changes goal arm shoulder rotation angle
                Dispatcher.Invoke(() =>aRec.RenderTransform = new RotateTransform(180 + angle));
            }
        }

        public void updateGoalArmAngle(double angle){
            if (angle >= 0 && angle <= maxRotation) { //changes goal arm shoulder rotation angle
                gRec.RenderTransform = new RotateTransform(180+angle);
            }
        }

        public void updateActualArmLength(double lengthPercentage) {
            if (lengthPercentage >= 0 && lengthPercentage <= 100) { //changes goal arm length
                aRec.Width = maxLength * (lengthPercentage / 100);
            }
        }

        public void updateGoalArmLength(double lengthPercentage) {
            if (lengthPercentage >= 0 && lengthPercentage <= 100) { //changes goal arm length
                gRec.Width = maxLength * (lengthPercentage / 100);
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
