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

using ArduinoLibrary;
using XboxController;
using System.Threading;

namespace wristTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Arduino hanDuino;
        ArduinoManager ARMan;
        XboxController.XboxController XBController;
        volatile bool ArduinoReady = false;
        float oldX = 0;
        float oldY = 0;
        float deadzone = 15; // set lower for HIGHER resolution
        public MainWindow()
        {
            InitializeComponent();
            ARMan = ArduinoManager.Instance;
            ARMan.findArduinos();
            hanDuino = ARMan.getHandArduino();
            hanDuino.Data_Received += hanDuino_Data_Received;
            comInViz.setTitle("WRIST COM IN");
            comOutViz.setTitle("WRIST COM OUT");
            XBController = new XboxController.XboxController();
            XBController.ThumbStickLeft += XBController_ThumbStickLeft;
            XBController.ButtonAReleased += XBController_ButtonAReleased; //Press A to start/restart dance, exit live data mode.
            XBController.ButtonBReleased += XBController_ButtonBReleased; //Press B to go into live input mode, exit dance mode.
            XBController.ButtonStartReleased += XBController_ButtonStartReleased; //Press start button for emergency stop
            XBController.TriggerRight += XBController_TriggerRight;

        }

        void XBController_TriggerRight(object sender, EventArgs e)
        {
            if (ArduinoReady)
            {
                XboxEventArgs args = (XboxEventArgs)e;
                float val = RoundFloat(args.GetTriggerRight());
                val = val.Map(0, 1, 100, 255);//In theory could be zero to 255, but I dont want it to self destruct...
                hanDuino.write("G:" + (int)val);
                comOutViz.addText("G:" + (int)val);
            }
        }

        void XBController_ButtonStartReleased(object sender, EventArgs e)
        {
            if (ArduinoReady)
            {
                hanDuino.write("STOP");
            }
        }

        void XBController_ButtonBReleased(object sender, EventArgs e)
        {
            if (ArduinoReady)
            {
                hanDuino.write("LIVE");
            }
        }

        void XBController_ButtonAReleased(object sender, EventArgs e)
        {
            if (ArduinoReady)
            {
                hanDuino.write("DANCE");
            }
        }

        void XBController_ThumbStickLeft(object sender, EventArgs e)
        {
            if (ArduinoReady)
            {
                XboxEventArgs args = (XboxEventArgs)e;
                Tuple<float, float> vec = args.GetThumbStickLeft();
                float newX = vec.Item1.Map(-1, 1, -127, 127);
                float newY = vec.Item2.Map(-1, 1, -127, 127);
                float Xdif = Math.Abs(newX - oldX);
                float Ydif = Math.Abs(newY - oldY);
                if (Xdif >= deadzone || Ydif >= deadzone)
                {
                    hanDuino.write("Y:" + (int)newX);
                    comOutViz.addText("\nY:" + (int)newX);
                    Thread.Sleep(20);
                    hanDuino.write("P:" + (int)newY);
                    comOutViz.addText("P:" + (int)newY);
                    oldX = newX;
                    oldY = newY;
                }
            }
        }

        void hanDuino_Data_Received(string receivedDataString)
        {
            if (receivedDataString.Contains("READY"))
            {
                ArduinoReady = true;
            }
            else
            {
                comInViz.addText("\n" + receivedDataString + "\n");
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Used to truncate a float to one decimal place.
        /// </summary>
        private float RoundFloat(float f)
        {
            int accuracy = 1000;
            int i = (int)(f * accuracy);
            f = i;
            return f / accuracy;
        }
    }

    public static class ExtensionMethods
    {
        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
    }
}
