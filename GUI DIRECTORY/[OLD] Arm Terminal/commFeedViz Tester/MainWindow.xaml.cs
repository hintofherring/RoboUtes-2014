using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace commFeedViz_Tester {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            testFeed.title = "catsrKewl";
            Console.WriteLine("NORMAL LOCATION");
            Console.SetOut(testFeed.getStreamLink());
            Console.WriteLine("This text was printed from a call to Console.WriteLine and should contain all NORMAL (error has not been, but can be, redirected in this example) console output.");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Environment.Exit(0);
        }
    }
}
