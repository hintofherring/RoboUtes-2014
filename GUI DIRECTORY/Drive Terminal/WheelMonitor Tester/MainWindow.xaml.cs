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

namespace WheelMonitor_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            wheelMonitor.errorStateChanged += wheelMonitor_errorStateChanged;
        }

        void wheelMonitor_errorStateChanged(bool errorState)
        {
            if (errorState)
            {
                errorMonitor.setIndicatorState(toggleIndicator.indicatorState.Red);
            }
            else
            {
                errorMonitor.setIndicatorState(toggleIndicator.indicatorState.Off);
            }
        }

        private void stallLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (((Label)sender).Name)
            {
                case "stallLabel":
                    wheelMonitor.Stall = true;
                    break;
                case "slipLabel":
                    wheelMonitor.Slip = true;
                    break;
                case "spinLabel":
                    wheelMonitor.Spinning = true;
                    break;
            }
        }

        private void stallLabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (((Label)sender).Name)
            {
                case "stallLabel":
                    wheelMonitor.Stall = false;
                    break;
                case "slipLabel":
                    wheelMonitor.Slip = false;
                    break;
                case "spinLabel":
                    wheelMonitor.Spinning = false;
                    break;
            }
        }

        private void speedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            wheelMonitor.Speed = (int)e.NewValue;
        }

        private void currentSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            wheelMonitor.Current = (int)e.NewValue;
        }
    }
}
