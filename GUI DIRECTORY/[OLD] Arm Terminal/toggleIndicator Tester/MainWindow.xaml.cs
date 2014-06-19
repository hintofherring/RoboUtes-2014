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

namespace toggleIndicator_Tester {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void redButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => indicatorTest.setIndicatorState(toggleIndicator.indicatorState.Red));
        }

        private void offButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => indicatorTest.setIndicatorState(toggleIndicator.indicatorState.Off));
        }

        private void greenButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => indicatorTest.setIndicatorState(toggleIndicator.indicatorState.Green));
        }

    }
}
