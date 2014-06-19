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

using ArmControlTools;
using XboxController;

namespace handInfoViewer
{
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("handInfoViewer", true)]
    public partial class ToolboxControl : UserControl
    {


        private armInputManager armIn;

        public armInputManager armInput
        {
            set { 
                armIn = value;
                armIn.targetWristChanged += armIn_targetWristChanged;
                armIn.targetGripperChanged += armIn_targetGripperChanged;
                armIn.targetWristRotationAngleChanged += armIn_targetWristRotationAngleChanged;
                Dispatcher.Invoke(() => gripperStrengthBar.setFillValue(100-armIn.getCommandedGripperVal()));
            }
            get {
                return armIn;
            }
        }

        public ToolboxControl()
        {
            InitializeComponent();
        }

        void armIn_targetWristRotationAngleChanged(double rotationAngle)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                spinHand.Width = 80;
                RotateTransform RT = new RotateTransform(rotationAngle);
                spinHand.RenderTransform = RT;
                wristAngleLabel.Content = Math.Round(rotationAngle,3);
            }));
        }

        void armIn_targetGripperChanged(double newValue)
        {
            
            gripperStrengthBar.setFillValue(newValue);
            Dispatcher.Invoke(() => gripperPercentLabel.Content = newValue);
        }

        void armIn_targetWristChanged(wristPositionData newPosition)
        {
            if (newPosition.upVal > 0)
            {
                upInd.setIndicatorState(toggleIndicator.indicatorState.Green);
            }
            else
            {
                upInd.setIndicatorState(toggleIndicator.indicatorState.Off);
            }

            if (newPosition.leftVal > 0)
            {
                leftInd.setIndicatorState(toggleIndicator.indicatorState.Green);
            }
            else
            {
                leftInd.setIndicatorState(toggleIndicator.indicatorState.Off);
            }

            if (newPosition.rightVal > 0)
            {
                rightInd.setIndicatorState(toggleIndicator.indicatorState.Green);
            }
            else
            {
                rightInd.setIndicatorState(toggleIndicator.indicatorState.Off);
            }
        }

    }
}
