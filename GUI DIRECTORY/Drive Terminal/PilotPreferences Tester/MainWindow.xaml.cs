using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace PilotPreferences_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        XboxController.XboxController xboxController;

        public MainWindow()
        {
            InitializeComponent();
            pilotPrefComp.deadZoneLeftSensitivityChanged +=pilotPrefComp_deadZoneLeftSensitivityChanged;
            pilotPrefComp.deadZoneRightSensitivityChanged += pilotPrefComp_deadZoneRightSensitivityChanged;
            pilotPrefComp.deadZoneLeftMinChanged += pilotPrefComp_deadZoneLeftMinChanged;
            pilotPrefComp.deadZoneLeftMaxChanged += pilotPrefComp_deadZoneLeftMaxChanged;
            pilotPrefComp.deadZoneRightMinChanged += pilotPrefComp_deadZoneRightMinChanged;
            pilotPrefComp.deadZoneRightMaxChanged += pilotPrefComp_deadZoneRightMaxChanged;
            pilotPrefComp.topSpeedChanged += pilotPrefComp_topSpeedChanged;

            topSpeedVal.Content = pilotPrefComp.TopSpeedPercentage; //just so the gui shows the same value as the slider at boot...

            xboxController = new XboxController.XboxController();
            pilotPrefComp.xboxController = xboxController;
        }

        void pilotPrefComp_topSpeedChanged(object sender, int newValue)
        {
            topSpeedVal.Content = newValue;
        }

        void pilotPrefComp_deadZoneRightMaxChanged(object sender, int newValue)
        {
            rightMaxVal.Content = newValue;
        }

        void pilotPrefComp_deadZoneRightMinChanged(object sender, int newValue)
        {
            rightMinVal.Content = newValue;
        }

        void pilotPrefComp_deadZoneLeftMaxChanged(object sender, int newValue)
        {
            leftMaxVal.Content = newValue;
        }

        void pilotPrefComp_deadZoneLeftMinChanged(object sender, int newValue)
        {
            leftMinVal.Content = newValue;
        }

        void pilotPrefComp_deadZoneRightSensitivityChanged(object sender, int newValue)
        {
            rightSensitivityValLabel.Content = newValue;
        }

        private void pilotPrefComp_deadZoneLeftSensitivityChanged(object sender, int newValue)
        {
            leftSensitivityValLabel.Content = newValue;
        }

        private void toggleXboxControllerButton_Click(object sender, RoutedEventArgs e)
        {
            pilotPrefComp.XBoxControllerConnected = !pilotPrefComp.XBoxControllerConnected;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() => ((Label)sender).Background = new SolidColorBrush(Color.FromRgb(255, 0, 0)));
            switch (((Label)sender).Name) {
                case "leftTrigLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.leftTrigger, true);
                    break;

                case "rightTrigLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.rightTrigger, true);
                    break;

                case "leftBumpLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.leftBumper, true);
                    break;

                case "rightBumpLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.rightBumper, true);
                    break;

                case "DUpLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.DUp, true);
                    break;

                case "DDownLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.DDown, true);
                    break;

                case "DLeftLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.DLeft, true);
                    break;

                case "DRightLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.DRight, true);
                    break;

                case "BackLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.Back, true);
                    break;

                case "startLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.Start, true);
                    break;

                case "YLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.Y, true);
                    break;

                case "XLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.X, true);
                    break;

                case "BLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.B, true);
                    break;

                case "ALab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.A, true);
                    break;

            }
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() => ((Label)sender).Background = new SolidColorBrush(Color.FromRgb(0, 0, 0)));
            switch (((Label)sender).Name)
            {
                case "leftTrigLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.leftTrigger, false);
                    break;

                case "rightTrigLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.rightTrigger, false);
                    break;

                case "leftBumpLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.leftBumper, false);
                    break;

                case "rightBumpLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.rightBumper, false);
                    break;

                case "DUpLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.DUp, false);
                    break;

                case "DDownLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.DDown, false);
                    break;

                case "DLeftLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.DLeft, false);
                    break;

                case "DRightLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.DRight, false);
                    break;

                case "BackLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.Back, false);
                    break;

                case "startLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.Start, false);
                    break;

                case "YLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.Y, false);
                    break;

                case "XLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.X, false);
                    break;

                case "BLab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.B, false);
                    break;

                case "ALab":
                    pilotPrefComp.toggleXboxControllerButtonClick(PilotPreferences.ToolboxControl.xBoxControllerButtons.A, false);
                    break;
            }
        }
    }
}
