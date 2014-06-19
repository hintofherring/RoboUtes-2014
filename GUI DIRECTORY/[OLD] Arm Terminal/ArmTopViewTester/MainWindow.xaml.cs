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

namespace topTester {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        XboxController.XboxController xboxController;
        public MainWindow() {
            InitializeComponent();
            xboxController = new XboxController.XboxController();
            armTopTest.XboxController = xboxController;
            gAngleSlider.ValueChanged += updateGArmAngle;
            gLengthSlider.ValueChanged += updateGArmLength;

            aAngleSlider.ValueChanged += updateAArmAngle;
            aLengthSlider.ValueChanged += updateAArmLength;
        }

        private void updateAArmLength(object sender, RoutedPropertyChangedEventArgs<double> e) {
            armTopTest.updateActualArmLength(e.NewValue);
        }

        private void updateAArmAngle(object sender, RoutedPropertyChangedEventArgs<double> e) {
            armTopTest.updateActualArmAngle(e.NewValue);
        }

        private void updateGArmLength(object sender, RoutedPropertyChangedEventArgs<double> e) {
            armTopTest.updateGoalArmLength(e.NewValue);
        }

        private void updateGArmAngle(object sender, RoutedPropertyChangedEventArgs<double> e) {
            armTopTest.updateGoalArmAngle(e.NewValue);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
