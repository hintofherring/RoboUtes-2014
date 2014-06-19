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
using ArduinoLibrary;
using ArmControlTools;

namespace localArmControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XboxController.XboxController xboxController;

        Arduino armDuino;
        Arduino handDuino;
        ArduinoManager ArduMan;
        armInputManager armInput;
        localArmCommandTransmitter armTransmitter;

        public MainWindow()
        {
            InitializeComponent();

            ArduMan = ArduinoManager.Instance;
            ArduMan.findArduinos();

            handDuino = ArduMan.getHandArduino();
            handDuino.Data_Received += handDuino_Data_Received;

            armDuino = ArduMan.getArmArduino();
            armDuino.Data_Received += armDuino_Data_Received;

            Console.SetOut(consoleViz.getStreamLink()); //Show console output in gui
            Console.WriteLine("***Arm Control Booted***");
            xboxController = new XboxController.XboxController();

            armInput = armInputManager.getInstance(xboxController);
            armTransmitter = new localArmCommandTransmitter(armDuino, handDuino, armInput);

            xboxControllerMonitor.xboxController = xboxController;
            armSideView.armInputManager = armInput;
            armTopView.armInputManager = armInput;
            Console.WriteLine("***XBOX CONTROLLER CONNECTED***");

            wristComponent._xboxController = xboxController;
        }

        void handDuino_Data_Received(string receivedData)
        {
            handComIn.addText(receivedData); //TODO: see how theres an armComHandler below, if the wrist involves the GUI one must be added here...
        }

        void armDuino_Data_Received(string receivedData)
        {
            armComIn.addText(receivedData);
            armComInHandler(receivedData);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void armComInHandler(string receivedData)
        {
            if (receivedData.Contains("Shoulder Position:"))
            {
                string toParse = receivedData.Substring(receivedData.LastIndexOf(":")+1);
                int parsedVal;
                if(int.TryParse(toParse,out parsedVal)){
                    parsedVal = (int)((parsedVal / 100.0) * armConstants.SHOULDER_RANGE);
                    armSideView.updateActualShoulder(parsedVal);
                    armInput.initShoulderPosition(parsedVal);
                }
            }
            else if (receivedData.Contains("Elbow Position:"))
            {
                string toParse = receivedData.Substring(receivedData.LastIndexOf(":") + 1);
                int parsedVal;
                if (int.TryParse(toParse, out parsedVal))
                {
                    parsedVal = (int)((parsedVal / 100.0) * armConstants.ELBOW_RANGE); ;
                    armSideView.updateActualElbow(parsedVal);
                    armInput.initElbowPosition(parsedVal);
                }
            }
            else if (receivedData.Contains("Turn Table Position:"))
            {
                string toParse = receivedData.Substring(receivedData.LastIndexOf(":") + 1);
                int parsedVal;
                if (int.TryParse(toParse, out parsedVal))
                {
                    parsedVal = (int)((parsedVal / 100.0) * armConstants.TURNTABLE_RANGE); ;
                    armTopView.updateActualArmAngle(parsedVal);
                    armInput.initTurnTablePosition(parsedVal);
                }
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
    }
}
