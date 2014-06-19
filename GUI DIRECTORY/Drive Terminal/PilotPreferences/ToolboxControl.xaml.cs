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

namespace PilotPreferences {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("PilotPreferences", true)]
    public partial class ToolboxControl : UserControl {

        public delegate void sliderChangedEventHandler(object sender, int newValue);
        public event sliderChangedEventHandler deadZoneLeftSensitivityChanged;
        public event sliderChangedEventHandler deadZoneRightSensitivityChanged;
        public event sliderChangedEventHandler deadZoneLeftMinChanged;
        public event sliderChangedEventHandler deadZoneLeftMaxChanged;
        public event sliderChangedEventHandler deadZoneRightMinChanged;
        public event sliderChangedEventHandler deadZoneRightMaxChanged;
        public event sliderChangedEventHandler topSpeedChanged;

        public XboxController.XboxController xboxController
        {
            set
            {
                _xboxController = value;
                XBoxControllerConnected = true;
                liveXboxControllerMonitor.xboxController = _xboxController;
            }
        }
        private XboxController.XboxController _xboxController;

        /// <summary>
        /// used to represent/change the state of the Pilot Preferences component.
        /// </summary>
        private enum DrivePreferencesState { LIVE = 0, SETTINGS };

		/// <summary>
		/// Returns the top speed percentage of the robot controls. from 0-100
		/// </summary>
        public int TopSpeedPercentage {
            get {
                return Int32.Parse(speedLabel.Content.ToString().Substring(0, speedLabel.Content.ToString().Length-1));
            }
            private set {
                Dispatcher.Invoke(()=>speedLabel.Content = value + "%");
            }
        }

        private DrivePreferencesState currentState = DrivePreferencesState.SETTINGS;

        private bool _XBOXCONNECTED = false;
        public bool XBoxControllerConnected
        {
            set {
                _XBOXCONNECTED = value;
                if (_XBOXCONNECTED)
                {
                    xboxControllerConnectedIndicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                }
                else
                {
                    xboxControllerConnectedIndicator.setIndicatorState(toggleIndicator.indicatorState.Red);
                }
            }
            get
            {
                return _XBOXCONNECTED;
            }
        }

        public ToolboxControl() {
            InitializeComponent();
            Dispatcher.Invoke(()=>stateButton.Content = currentState);
            setState(currentState);
            Dispatcher.Invoke(()=>speedSlider.Value = 100);
            XBoxControllerConnected = false;

            #region eventRelayAssignment (passing events from internal components to the surface of the pilotPrefences component)

            deadzoneLeft.sensitivityValueChanged +=deadzoneLeft_sensitivityValueChanged;
            deadzoneRight.sensitivityValueChanged += deadzoneRight_sensitivityValueChanged;
            deadzoneLeft.minValueChanged += deadzoneLeft_minValueChanged;
            deadzoneLeft.maxValueChanged += deadzoneLeft_maxValueChanged;
            deadzoneRight.minValueChanged += deadzoneRight_minValueChanged;
            deadzoneRight.maxValueChanged += deadzoneRight_maxValueChanged;
            speedSlider.ValueChanged+= topSpeedSlider_ValueChanged;
            #endregion
        }


        #region eventRelayDefinition (passing events from internal components to the surface of the pilotPrefences component)
        void deadzoneRight_maxValueChanged(object sender, int newValue)
        {
            if (deadZoneRightMaxChanged != null)
            {
                deadZoneRightMaxChanged(sender, newValue);
            }
        }

        void deadzoneRight_minValueChanged(object sender, int newValue)
        {
            if (deadZoneRightMinChanged != null)
            {
                deadZoneRightMinChanged(sender, newValue);
            }
        }

        void deadzoneLeft_maxValueChanged(object sender, int newValue)
        {
            if (deadZoneLeftMaxChanged != null)
            {
                deadZoneLeftMaxChanged(sender, newValue);
            }
        }

        void deadzoneLeft_minValueChanged(object sender, int newValue)
        {
            if (deadZoneLeftMinChanged != null)
            {
                deadZoneLeftMinChanged(sender, newValue);
            }
        }

        void deadzoneRight_sensitivityValueChanged(object sender, int newValue)
        {
            if (deadZoneRightSensitivityChanged != null)
            {
                deadZoneRightSensitivityChanged(sender, newValue);
            }
        }

        private void deadzoneLeft_sensitivityValueChanged(object sender, int newValue)
        {
            if (deadZoneLeftSensitivityChanged != null)
            {
                deadZoneLeftSensitivityChanged(sender, newValue);
            }
        }
        #endregion


        private void topSpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            Dispatcher.Invoke(()=>TopSpeedPercentage = (int)e.NewValue);
            if (topSpeedChanged != null)
            {
                topSpeedChanged(sender, (int)e.NewValue);
            }
        }

        private void stateButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(()=>stateButton.MoveFocus(new TraversalRequest(new FocusNavigationDirection()))); //Move focus from the button so it doesnt keep blinking...
            currentState ^= (DrivePreferencesState)1; //Toggle the state value. Alternatively: _value = (SomeEnum)(((int)_value + 1) % 2);
            Dispatcher.Invoke(()=>stateButton.Content = currentState);
            setState(currentState);
        }

        private void setState(DrivePreferencesState desiredState)
        {
            if (desiredState == DrivePreferencesState.SETTINGS)
            {
                Dispatcher.Invoke(()=>settingsItems.Visibility = Visibility.Visible);
                Dispatcher.Invoke(()=>liveItems.Visibility = Visibility.Hidden);
            }
            else if (desiredState == DrivePreferencesState.LIVE)
            {
                Dispatcher.Invoke(()=>settingsItems.Visibility = Visibility.Hidden);
                Dispatcher.Invoke(()=>liveItems.Visibility = Visibility.Visible);
            }
        }

        public enum xBoxControllerButtons {leftTrigger = 1, leftBumper, DUp, DDown, DLeft, DRight, Back, Start, A, B, Y, X, rightTrigger, rightBumper };

        public void toggleXboxControllerButtonClick(xBoxControllerButtons button, bool onState)
        {
            switch (button){
                case xBoxControllerButtons.leftTrigger:
                    liveXboxControllerMonitor.leftTrigger = onState;
                break;

                case xBoxControllerButtons.leftBumper:
                    liveXboxControllerMonitor.leftBumper = onState;
                    break;

                case xBoxControllerButtons.DUp:
                    liveXboxControllerMonitor.dUp = onState;
                    break;

                case xBoxControllerButtons.DDown:
                    liveXboxControllerMonitor.dDown = onState;
                    break;

                case xBoxControllerButtons.DLeft:
                    liveXboxControllerMonitor.dLeft = onState;
                    break;

                case xBoxControllerButtons.DRight:
                    liveXboxControllerMonitor.dRight = onState;
                    break;

                case xBoxControllerButtons.Back:
                    liveXboxControllerMonitor.backButton = onState;
                    break;

                case xBoxControllerButtons.Start:
                    liveXboxControllerMonitor.startButton = onState;
                    break;

                case xBoxControllerButtons.A:
                    liveXboxControllerMonitor.aButton = onState;
                    break;

                case xBoxControllerButtons.B:
                    liveXboxControllerMonitor.bButton = onState;
                    break;

                case xBoxControllerButtons.Y:
                    liveXboxControllerMonitor.yButton = onState;
                    break;

                case xBoxControllerButtons.X:
                    liveXboxControllerMonitor.xButton = onState;
                    break;

                case xBoxControllerButtons.rightTrigger:
                    liveXboxControllerMonitor.rightTrigger = onState;
                    break;

                case xBoxControllerButtons.rightBumper:
                    liveXboxControllerMonitor.rightBumper = onState;
                    break;
            }
        }
    }
}
