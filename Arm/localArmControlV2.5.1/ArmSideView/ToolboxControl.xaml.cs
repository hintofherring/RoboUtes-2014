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
using ArmControlTools;

namespace ArmSideView {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("ArmSideView", true)]
    public partial class ArmSide : UserControl {

        private Line elbowGoalLine;
        private Line shoulderGoalLine;
        private Line elbowActualLine;
        private Line shoulderActualLine;

        private armInputManager _armInputManager;
        public armInputManager armInputManager
        {
            set
            {
                _armInputManager = value;
                _armInputManager.targetElbowChanged += _armInputManager_targetElbowChanged;
                _armInputManager.targetShoulderChanged += _armInputManager_targetShoulderChanged;
                _armInputManager.EmergencyStop += emergencyStop;
            }
        }

        private void emergencyStop() {
            Dispatcher.Invoke(() => updateGoalElbow(aElbowAngle));
            Dispatcher.Invoke(() => updateGoalShoulder(-aShoulderAngle));
            _armInputManager.manuallySetElbow((int)aElbowAngle);
            _armInputManager.manuallySetShoulder(-(int)aShoulderAngle);
        }

        void _armInputManager_targetShoulderChanged(double newAngle)
        {
            Dispatcher.Invoke(() => updateGoalShoulder(newAngle));
        }

        void _armInputManager_targetElbowChanged(double newAngle)
        {
            Dispatcher.Invoke(() => updateGoalElbow(newAngle));
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
            elbowGoalLine = new Line();
            shoulderGoalLine = new Line();
            elbowActualLine = new Line();
            shoulderActualLine = new Line();
        }

        /// <summary>
        /// Takes in the angle between the shoulder-arm and the forearm and updates the rendering of the ACTUAL position accordingly
        /// Takes a value from 0-360
        /// </summary>
        /// <param name="angle"></param>
        public void updateActualElbow(double angle) {
            aElbowAngle = angle;
            Dispatcher.Invoke(()=>aRec2.RenderTransform = new RotateTransform(180 - aElbowAngle + (aShoulderAngle)));


            Action update = delegate()
            {
                realElbowAngleLabel.Content = angle;
                Point realElbowLabelPos = new Point(Canvas.GetLeft(realElbowLabel), Canvas.GetBottom(realElbowLabel));
                Point realElbowBasePos = new Point(Canvas.GetLeft(aRec2), Canvas.GetBottom(aRec2));

                canv.Children.Remove(elbowActualLine);

                elbowActualLine.X1 = realElbowLabelPos.X; ;
                elbowActualLine.Y1 = canv.Height - realElbowLabelPos.Y - realElbowLabel.Height*.5;

                elbowActualLine.X2 = realElbowBasePos.X;
                elbowActualLine.Y2 = canv.Height - realElbowBasePos.Y;

                elbowActualLine.StrokeThickness = 3;
                elbowActualLine.Stroke = new LinearGradientBrush(Colors.Red, Colors.Black, 90);
                canv.Children.Add(elbowActualLine);
            };
            Dispatcher.Invoke(update);
        }

        /// <summary>
        /// Takes in the angle between the shoulder-arm and the forearm and updates the rendering of the GOAL position accordingly
        /// Takes a value from 0-360
        /// </summary>
        /// <param name="angle"></param>
        public void updateGoalElbow(double angle) {
            gElbowAngle = angle;
            gRec2.RenderTransform = new RotateTransform(180 - gElbowAngle + (gShoulderAngle));

            Action update = delegate()
            {
                elbowAngleLabel.Content = angle;
                Point elbowLabelPos = new Point(Canvas.GetLeft(elbowAngleLabel), Canvas.GetBottom(elbowAngleLabel));
                Point elbowBasePos = new Point(Canvas.GetLeft(gRec2), Canvas.GetBottom(gRec2));

                canv.Children.Remove(elbowGoalLine);

                elbowGoalLine.X1 = elbowLabelPos.X + (elbowAngleLabel.Width * .5);
                elbowGoalLine.Y1 = canv.Height - (elbowAngleLabel.Height);

                elbowGoalLine.X2 = elbowBasePos.X;
                elbowGoalLine.Y2 = canv.Height - elbowBasePos.Y;

                elbowGoalLine.StrokeThickness = 3;
                elbowGoalLine.Stroke = new LinearGradientBrush(Colors.Green, Colors.Black, 90);
                canv.Children.Add(elbowGoalLine);
            };
            Dispatcher.Invoke(update);
        }

        /// <summary>
        /// Takes a value from 0-360 (usually from 0-90 though)
        /// </summary>
        /// <param name="angle"></param>
        public void updateActualShoulder(double angle) {
            aShoulderAngle = -angle;
            //aShoulderAngle = (270 - aShoulderAngle);
            Dispatcher.Invoke(()=>aRec1.RenderTransform = new RotateTransform(aShoulderAngle));
            Dispatcher.Invoke(()=>Canvas.SetLeft(aRec2, Canvas.GetLeft(aRec1) + (aRec1.Width * Math.Cos(ConvertToRadians(aShoulderAngle))))); //set rec2 dist from left
            Dispatcher.Invoke(()=>Canvas.SetBottom(aRec2, Canvas.GetBottom(aRec1) + (aRec1.Width * Math.Sin(ConvertToRadians(-aShoulderAngle))))); //set rec2 dist from top
            Dispatcher.Invoke(()=>updateActualElbow(aElbowAngle));

            Action update = delegate()
            {
                realShoulderAngleLabel.Content = angle;
                Point realShoulderLabelPos = new Point(Canvas.GetLeft(realShoulderLabel), Canvas.GetBottom(realShoulderLabel));
                Point realShoulderBasePos = new Point(Canvas.GetLeft(aRec1), Canvas.GetBottom(aRec1));

                canv.Children.Remove(shoulderActualLine);

                shoulderActualLine.X1 = realShoulderLabelPos.X; ;
                shoulderActualLine.Y1 = canv.Height - realShoulderLabelPos.Y - realShoulderLabel.Height * .5;

                shoulderActualLine.X2 = realShoulderBasePos.X;
                shoulderActualLine.Y2 = canv.Height - realShoulderBasePos.Y;

                shoulderActualLine.StrokeThickness = 3;
                shoulderActualLine.Stroke = new LinearGradientBrush(Colors.Red, Colors.Black, 90);
                canv.Children.Add(shoulderActualLine);
            };
            Dispatcher.Invoke(update);
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

            Action update = delegate()
            {
                shoulderAngleLabel.Content = angle;
                Point shoulderLabelPos = new Point(Canvas.GetLeft(shoulderAngleLabel), Canvas.GetBottom(shoulderAngleLabel));
                Point shoulderBasePos = new Point(Canvas.GetLeft(gRec1), Canvas.GetBottom(gRec1));

                canv.Children.Remove(shoulderGoalLine);

                shoulderGoalLine.X1 = shoulderLabelPos.X + (shoulderAngleLabel.Width * .5);
                shoulderGoalLine.Y1 = canv.Height - (shoulderAngleLabel.Height);

                shoulderGoalLine.X2 = shoulderBasePos.X;
                shoulderGoalLine.Y2 = canv.Height - shoulderBasePos.Y;

                shoulderGoalLine.StrokeThickness = 3;
                shoulderGoalLine.Stroke = new LinearGradientBrush(Colors.Green, Colors.Black, 90);
                canv.Children.Add(shoulderGoalLine);
            };
            Dispatcher.Invoke(update);
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
    }
}
