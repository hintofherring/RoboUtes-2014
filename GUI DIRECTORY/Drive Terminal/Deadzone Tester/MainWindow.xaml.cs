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

namespace Deadzone_Tester {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            deadZoneComp.minValueChanged +=deadZoneComp_ValueChanged;
            deadZoneComp.maxValueChanged += deadZoneComp_ValueChanged;
            deadZoneComp.sensitivityValueChanged += deadZoneComp_ValueChanged;

        }

        private void deadZoneComp_ValueChanged(object sender, int newValue) {
            switch (((Slider)sender).Uid) {
                case "MIN":
                    minLabel.Content = newValue;
                    break;
                case "MAX":
                    maxLabel.Content = newValue;
                    break;
                case "SENSITIVITY":
                    sensitivityLabel.Content = newValue;
                    break;
            }
        }
    }
}
