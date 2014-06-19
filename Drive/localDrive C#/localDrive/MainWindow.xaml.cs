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
using System.Threading;

namespace localDrive
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XboxController.XboxController xboxCont;
        string valFormat = "000";
        Arduino frontDriveDuino;
        Arduino backDriveDuino;
        ArduinoManager ArduMan;

        string toSendLeft = "";
        string toSendRight = "";
        Timer sendTimer;

        public MainWindow()
        {
            InitializeComponent();
            ArduMan = ArduinoManager.Instance;
            ArduMan.findArduinos();
            frontDriveDuino = ArduMan.getDriveFrontArduino();
            backDriveDuino = ArduMan.getDriveBackArduino();
            frontDriveDuino.Data_Received += frontDriveDuino_Data_Received;
            backDriveDuino.Data_Received += backDriveDuino_Data_Received;
            xboxCont = new XboxController.XboxController();
            xboxCont.ThumbStickLeft += xboxCont_ThumbStickLeft;
            xboxCont.ThumbStickRight += xboxCont_ThumbStickRight;

            
            backDuinoInViz.setTitle("BACK COM IN");
            backDuinoOutViz.setTitle("BACK COM OUT");
            frontDuinoInViz.setTitle("FRONT COM IN");
            frontDuinoOutViz.setTitle("FRONT COM OUT");
            sendTimer = new Timer(sendTimerCallback, null, 0, 200);
        }

        private void sendTimerCallback(object state)
        {
            lock (toSendRight)
            {
                lock (toSendLeft)
                {
                    frontDuinoOutViz.addText("\nSENDING: " + toSendRight + "\n");
                    backDuinoOutViz.addText("\nSENDING: " + toSendRight + "\n");
                    frontDriveDuino.write(toSendRight);
                    backDriveDuino.write(toSendRight);

                    frontDuinoOutViz.addText("\nSENDING: " + toSendLeft + "\n");
                    backDuinoOutViz.addText("\nSENDING: " + toSendLeft + "\n");
                    frontDriveDuino.write(toSendLeft);
                    backDriveDuino.write(toSendLeft);
                }
            }
        }

        void backDriveDuino_Data_Received(string receivedData)
        {
            backDuinoInViz.addText("REC: " + receivedData);
        }

        void frontDriveDuino_Data_Received(string receivedData)
        {
            frontDuinoInViz.addText("REC: " + receivedData);
        }

        void xboxCont_ThumbStickRight(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetThumbStickRight();
            Dispatcher.Invoke(() => rightStickVal.Content = vec.Item2);
            float newRightY = vec.Item2.Map(-1, 1, 0, 100);
            lock (toSendRight)
            {
                toSendRight = " R" + newRightY.ToString(valFormat);
            }
        }
        
        private void xboxCont_ThumbStickLeft(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetThumbStickLeft();
            Dispatcher.Invoke(() => leftStickVal.Content = vec.Item2);
            float newLeftY = vec.Item2.Map(-1, 1, 0, 100);
            lock (toSendLeft)
            {
                toSendLeft = " L" + newLeftY.ToString(valFormat);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
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
