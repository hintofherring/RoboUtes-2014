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

namespace Inclinometer_Window_Tester {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void rollSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            inclinometerWindow.rollAngle = (int)rollSlider.Value;
        }

        private void pitchSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            inclinometerWindow.pitchAngle = (int)pitchSlider.Value;
        }

        private void rollWarningButton_Click(object sender, RoutedEventArgs e)
        {
            inclinometerWindow.RollWarningLight = !inclinometerWindow.RollWarningLight;
        }

        private void pitchWarningButton_Click(object sender, RoutedEventArgs e)
        {
            inclinometerWindow.PitchWarningLight = !inclinometerWindow.PitchWarningLight;
        }
    }
}
