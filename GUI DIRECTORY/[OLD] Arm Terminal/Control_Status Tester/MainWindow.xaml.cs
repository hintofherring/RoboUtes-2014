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

namespace Control_Status_Tester {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            testControl.activateButtonClicked += testControl_activateButtonClicked;
        }

        void testControl_activateButtonClicked(Button sender) {
            MessageBox.Show(sender.Uid);
            if (sender.Uid == "GUI_Drive") {
                testControl.toggleLights(Control_Status.ControlStatus.Indication_Lights.GUI_Drive, true);
            }
            else if (sender.Uid == "Controller_Drive") {
                testControl.toggleLights(Control_Status.ControlStatus.Indication_Lights.Controller_Drive, true);
                testControl.toggleLights(Control_Status.ControlStatus.Indication_Lights.Main_Controller_Connected, true);
                testControl.toggleLights(Control_Status.ControlStatus.Indication_Lights.Arm_Connected, true);
            }
            else if (sender.Uid == "Gripper_Input") {
                testControl.toggleLights(Control_Status.ControlStatus.Indication_Lights.Gripper_Input, true);
                testControl.toggleLights(Control_Status.ControlStatus.Indication_Lights.Main_Controller_Connected, false);
                testControl.toggleLights(Control_Status.ControlStatus.Indication_Lights.Arm_Connected, false);
            }
        }
    }
}
