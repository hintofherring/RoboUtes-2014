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

namespace testGuiStuff {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            slider1.Maximum = 360;
            slider1.Minimum = 0;
            slider1.TickFrequency = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            int angleTemp;
            if (int.TryParse(angleTextBox.Text, out angleTemp)) {
                inclinometer1.rotate(angleTemp);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            inclinometer1.rotate((int)slider1.Value);
        }

    }
}
