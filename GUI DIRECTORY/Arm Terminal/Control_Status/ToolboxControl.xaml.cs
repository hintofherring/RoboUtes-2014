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

namespace Control_Status {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("Control_Status", true)]
    public partial class ControlStatus : UserControl {

        public delegate void activateButtonClickedEventHandler(Button sender);
        public event activateButtonClickedEventHandler activateButtonClicked;

        /// <summary>
        /// Specifies an indicator light
        /// </summary>
        public enum Indication_Lights { Main_Controller_Connected, Mini_Controller_Connected, Arm_Connected, GUI_Drive, Controller_Drive, Gripper_Input };

        public ControlStatus() {
            InitializeComponent();
        }

        private void ButtonClicked(object sender, RoutedEventArgs e) {
            if (activateButtonClicked != null)
                activateButtonClicked((Button)sender);
        }

        /// <summary>
        /// changes the state on an indicator light. specify the light and pass true (for on) or false (for off)
        /// </summary>
        /// <param name="light"></param>
        /// <param name="ON"></param>
        public void toggleLights(Indication_Lights light, bool onState) {
            switch (light) {
                case Indication_Lights.Main_Controller_Connected:
                    if (onState)
                    {
                        Main_Controller_Connected_Indicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                    }
                    else {
                        Main_Controller_Connected_Indicator.setIndicatorState(toggleIndicator.indicatorState.Off);
                    }
                    break;

                case Indication_Lights.Mini_Controller_Connected:
                    if (onState)
                    {
                        Mini_Controller_Connected_Indicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                    }
                    else {
                        Mini_Controller_Connected_Indicator.setIndicatorState(toggleIndicator.indicatorState.Off);
                    }
                    break;

                case Indication_Lights.Arm_Connected:
                    if (onState)
                    {
                        Arm_Connected_Indicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                    }
                    else {
                        Arm_Connected_Indicator.setIndicatorState(toggleIndicator.indicatorState.Off);
                    }
                    break;

                case Indication_Lights.GUI_Drive:
                    if (onState)
                    {
                        GUI_Drive_Indicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                    }
                    else {
                        GUI_Drive_Indicator.setIndicatorState(toggleIndicator.indicatorState.Off);
                    }
                    break;

                case Indication_Lights.Controller_Drive:
                    if (onState)
                    {
                        Controller_Drive_Indicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                    }
                    else {
                        Controller_Drive_Indicator.setIndicatorState(toggleIndicator.indicatorState.Off);
                    }
                    break;

                case Indication_Lights.Gripper_Input:
                    if (onState)
                    {
                        Gripper_Input_Indicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                    }
                    else {
                        Gripper_Input_Indicator.setIndicatorState(toggleIndicator.indicatorState.Off);
                    }
                    break;
            }
        }


    }
}
