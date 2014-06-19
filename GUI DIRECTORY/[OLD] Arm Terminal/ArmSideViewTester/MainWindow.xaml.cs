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

namespace tester1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        XboxController.XboxController xboxController;
        public MainWindow() {
            InitializeComponent();
            gShoulderSlider.ValueChanged += updateGShoulder;
            gElbowSlider.ValueChanged += updateGElbow;
            aShoulderSlider.ValueChanged += updateAShoulder;
            aElbowSlider.ValueChanged += updateAElbow;
            xboxController = new XboxController.XboxController();
            testSideView.XboxController = xboxController;
        }

        private void updateGElbow(object sender, RoutedPropertyChangedEventArgs<double> e) {
            testSideView.updateGoalElbow(e.NewValue);
        }

        private void updateGShoulder(object sender, RoutedPropertyChangedEventArgs<double> e) {
            testSideView.updateGoalShoulder(e.NewValue);
        }

        private void updateAElbow(object sender, RoutedPropertyChangedEventArgs<double> e) {
            testSideView.updateActualElbow(e.NewValue);
        }

        private void updateAShoulder(object sender, RoutedPropertyChangedEventArgs<double> e) {
            testSideView.updateActualShoulder(e.NewValue);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
