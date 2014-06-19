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

namespace ArmTopView {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("ArmTopView", true)]
    public partial class ArmTop : UserControl {
        private armInputManager _armInputManager;
        public armInputManager armInputManager
        {
            set
            {
                _armInputManager = value;
                _armInputManager.targetTurnTableChanged += _armInputManager_targetTurnTableChanged;
            }
        }

        void _armInputManager_targetTurnTableChanged(double newAngle)
        {
            Dispatcher.Invoke(() => updateGoalArmAngle(newAngle));
        }

        public double maxLength = 260; //starting standard value
        public double maxRotation = 90; //starting standard value
        public ArmTop() {
            InitializeComponent();
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
