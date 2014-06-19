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

        private armInputManager _armInputManager;
        public armInputManager armInputManager
        {
            set
            {
                _armInputManager = value;
                _armInputManager.targetElbowChanged += _armInputManager_targetElbowChanged;
                _armInputManager.targetShoulderChanged += _armInputManager_targetShoulderChanged;
            }
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
        }

        /// <summary>
        /// Takes in the angle between the shoulder-arm and the forearm and updates the rendering of the ACTUAL position accordingly
        /// Takes a value from 0-360
        /// </summary>
        /// <param name="angle"></param>
        public void updateActualElbow(double angle) {
            aElbowAngle = angle;
            Dispatcher.Invoke(()=>aRec2.RenderTransform = new RotateTransform(180 - aElbowAngle + (aShoulderAngle)));
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
            Dispatcher.Invoke(()=>aRec1.RenderTransform = new RotateTransform(aShoulderAngle));
            Dispatcher.Invoke(()=>Canvas.SetLeft(aRec2, Canvas.GetLeft(aRec1) + (aRec1.Width * Math.Cos(ConvertToRadians(aShoulderAngle))))); //set rec2 dist from left
            Dispatcher.Invoke(()=>Canvas.SetBottom(aRec2, Canvas.GetBottom(aRec1) + (aRec1.Width * Math.Sin(ConvertToRadians(-aShoulderAngle))))); //set rec2 dist from top
            Dispatcher.Invoke(()=>updateActualElbow(aElbowAngle));
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
    }
}
