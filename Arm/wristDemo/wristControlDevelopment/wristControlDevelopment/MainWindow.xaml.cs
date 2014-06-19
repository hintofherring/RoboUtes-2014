﻿using System;
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

namespace wristControlDevelopment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ArduinoManager ArduMan;
        Arduino wristDuino;
        Timer serialTimer;
        volatile bool serialReady = false;

        XboxController.XboxController XBoxCon;
        float Y = 0;
        float X = 0;
        double MAG = 0;
        double MAGpercent = 0;
        private double MAX_MAGNITUDE = 100;

        double upPerc = 0;
        double leftPerc = 0;
        double rightPerc = 0;
        double upMag = 0;
        double leftMag = 0;
        double rightMag = 0;

        int oldUpMag = 0;
        int oldLeftMag = 0;
        int oldRightMag = 0;

        int deadzone = 1; //Lower this value for increased resolution (might cause slower response)

        public MainWindow()
        {
            InitializeComponent();

            serialTimer = new Timer(serialTimerCallback, null, 0, 50);

            wristInViz.setTitle("WRIST COM IN");

            ArduMan = ArduinoManager.Instance;
            ArduMan.findArduinos();
            wristDuino = ArduMan.getHandArduino();
            wristDuino.Data_Received += wristDuino_Data_Received;

            XBoxCon = new XboxController.XboxController();
            XBoxCon.ThumbStickLeft+= XBoxCon_ThumbStickLeft;
            Dispatcher.Invoke(() => maxMagSlider.Value = MAX_MAGNITUDE);
        }

        private void serialTimerCallback(object state)
        {
            serialReady = true;
        }

        void wristDuino_Data_Received(string receivedData)
        {
            wristInViz.addText(receivedData);
        }

        private void XBoxCon_ThumbStickLeft(object sender, EventArgs e)
        {
                XboxEventArgs args = (XboxEventArgs)e;
                Tuple<float, float> vec = args.GetLeftThumbStick();
                Y = vec.Item2.Map(-1, 1, -100, 100);
                X = vec.Item1.Map(-1, 1, -100, 100);
                double rotationAngle = -((Math.Atan2(Y, X) * 180) / Math.PI);
                MAGpercent = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
                if (MAGpercent > 100) //gets rid of some slight noise
                {
                    MAGpercent = 100;
                }
                MAG = (MAGpercent / 100) * MAX_MAGNITUDE;
                Dispatcher.Invoke(() => rotAngleLabel.Content = rotationAngle);
                Dispatcher.Invoke(() => magLabel.Content = MAG);

                this.Dispatcher.Invoke((Action)(() =>
                {
                    spinHand.Width = MAG;
                    RotateTransform RT = new RotateTransform(rotationAngle);
                    spinHand.RenderTransform = RT;
                }));

                updateModel(rotationAngle, MAG);
        }

        private void updateModel(double rotationAngle, double MAG)
        {
            if((rotationAngle > 150 && rotationAngle<=180) || (rotationAngle <= -90 && rotationAngle >= -180) )  //top left region
            {
                //calculate actuator pull
                double swingPercent = 0;
                if (rotationAngle > 150 && rotationAngle <= 180)
                {
                    swingPercent = rotationAngle.Map(150, 270, 0, 120)/120;
                }
                else if (rotationAngle <= -90 && rotationAngle >= -180)
                {
                    swingPercent = rotationAngle.Map(-210, -90,0 , 120) / 120;
                }
                leftPerc = (1 - swingPercent) * 100;
                upPerc = swingPercent * 100;

                rightMag = -MAG;
                leftMag = (leftPerc / 100) * MAG;
                upMag = (upPerc / 100) * MAG;

                //updateGUI
                rightInd.setIndicatorState(toggleIndicator.indicatorState.Red);
                upInd.setIndicatorState(toggleIndicator.indicatorState.Green);
                leftInd.setIndicatorState(toggleIndicator.indicatorState.Green);
                updatePercentagesGUI();
                updateArduino();
            }
            
            else if (rotationAngle > 30 && rotationAngle <= 150) //bottom region
            {
                //calculate actuator pull
                double swingPercent = rotationAngle.Map(30, 150, 0, 120) / 120;
                rightPerc = (1 - swingPercent) * 100;
                leftPerc = swingPercent * 100;

                upMag = -MAG;
                rightMag = (rightPerc / 100) * MAG;
                leftMag = (leftPerc / 100) * MAG;

                //updateGUI
                rightInd.setIndicatorState(toggleIndicator.indicatorState.Green);
                upInd.setIndicatorState(toggleIndicator.indicatorState.Red);
                leftInd.setIndicatorState(toggleIndicator.indicatorState.Green);
                updatePercentagesGUI();
                updateArduino();
            }
            else if (rotationAngle > -90 && rotationAngle <= 30) //top right region
            {
                //calculate actuator pull
                double swingPercent = rotationAngle.Map(-90, 30, 0, 120)/120;
                upPerc = (1 - swingPercent) * 100;
                rightPerc = swingPercent*100;

                leftMag = -MAG;
                upMag = (upPerc / 100) * MAG;
                rightMag = (rightPerc / 100) * MAG;

                //updateGUI
                rightInd.setIndicatorState(toggleIndicator.indicatorState.Green);
                upInd.setIndicatorState(toggleIndicator.indicatorState.Green);
                leftInd.setIndicatorState(toggleIndicator.indicatorState.Red);
                updatePercentagesGUI();
                updateArduino();
            }
        }

        private void updateArduino()
        {
            if (serialReady)
            {
                bool transmitted = false;
                if (Math.Abs(oldUpMag - (int)upMag) > deadzone)
                {
                    oldUpMag = (int)upMag;
                    wristDuino.write("U:" + (int)upMag);
                    transmitted = true;
                }
                if (Math.Abs(oldLeftMag - (int)leftMag) > deadzone)
                {
                    oldLeftMag = (int)leftMag;
                    wristDuino.write("L:" + (int)leftMag);
                    transmitted = true;
                }
                if (Math.Abs(oldRightMag - (int)rightMag) > deadzone)
                {
                    oldRightMag = (int)rightMag;
                    wristDuino.write("R:" + (int)rightMag);
                    transmitted = true;
                }

                if (transmitted)
                {
                    serialReady = false;
                }
            }
        }

        private void updatePercentagesGUI()
        {
            Dispatcher.Invoke((Action)(() =>
            {
                upPercentageLabel.Content = (int)upPerc;
                leftPercentageLabel.Content = (int)leftPerc;
                rightPercentageLabel.Content = (int)rightPerc;
                upMagLabel.Content = (int)upMag;
                leftMagLabel.Content = (int)leftMag;
                rightMagLabel.Content = (int)rightMag;
            }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MAX_MAGNITUDE = e.NewValue;
            maxMagLabel.Content = MAX_MAGNITUDE;
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