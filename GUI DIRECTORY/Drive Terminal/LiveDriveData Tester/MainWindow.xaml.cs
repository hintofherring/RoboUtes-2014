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

namespace LiveDriveData_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int newVal = (int)e.NewValue;
            switch (((Slider)sender).Name)
            {
                case "pidGoalSlider":
                    liveDriveDataComp.PIDGoal = newVal;
                    break;
                case "actualSlider":
                    liveDriveDataComp.actualSpeed = newVal;
                    break;
                case "speedSlider":
                    
                    liveDriveDataComp.UpLeftWheel.Speed = newVal;
                    liveDriveDataComp.UpRightWheel.Speed = newVal;
                    liveDriveDataComp.BackLeftWheel.Speed = newVal;
                    liveDriveDataComp.BackRightWheel.Speed = newVal;
                    break;
                case "currentSlider":
                    liveDriveDataComp.UpLeftWheel.Current = newVal;
                    liveDriveDataComp.UpRightWheel.Current = newVal;
                    liveDriveDataComp.BackLeftWheel.Current = newVal;
                    liveDriveDataComp.BackRightWheel.Current = newVal;
                    break;
            }
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (((Label)sender).Name)
            {
                case "stallLabel":
                    liveDriveDataComp.UpLeftWheel.Stall = true;
                    liveDriveDataComp.UpRightWheel.Stall = true;
                    liveDriveDataComp.BackLeftWheel.Stall = true;
                    liveDriveDataComp.BackRightWheel.Stall = true;
                    break;
                case "slipLabel":
                    liveDriveDataComp.UpLeftWheel.Slip = true;
                    liveDriveDataComp.UpRightWheel.Slip = true;
                    liveDriveDataComp.BackLeftWheel.Slip = true;
                    liveDriveDataComp.BackRightWheel.Slip = true;
                    break;
                case "spinningLabel":
                    liveDriveDataComp.UpLeftWheel.Spinning = true;
                    liveDriveDataComp.UpRightWheel.Spinning = true;
                    liveDriveDataComp.BackLeftWheel.Spinning = true;
                    liveDriveDataComp.BackRightWheel.Spinning = true;
                    break;
            }
        }

        private void MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (((Label)sender).Name)
            {
                case "stallLabel":
                    liveDriveDataComp.UpLeftWheel.Stall = false;
                    liveDriveDataComp.UpRightWheel.Stall = false;
                    liveDriveDataComp.BackLeftWheel.Stall = false;
                    liveDriveDataComp.BackRightWheel.Stall = false;
                    break;
                case "slipLabel":
                    liveDriveDataComp.UpLeftWheel.Slip = false;
                    liveDriveDataComp.UpRightWheel.Slip = false;
                    liveDriveDataComp.BackLeftWheel.Slip = false;
                    liveDriveDataComp.BackRightWheel.Slip = false;
                    break;
                case "spinningLabel":
                    liveDriveDataComp.UpLeftWheel.Spinning = false;
                    liveDriveDataComp.UpRightWheel.Spinning = false;
                    liveDriveDataComp.BackLeftWheel.Spinning = false;
                    liveDriveDataComp.BackRightWheel.Spinning = false;
                    break;
            }
        }
    }
}
