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
using ArduinoLibrary;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ArduinoManager ArduMan = ArduinoManager.Instance;
        Arduino testDuino;
        public MainWindow()
        {
            InitializeComponent();
            ArduMan.findArduinos();
            testDuino = ArduMan.getHandArduino();
            testDuino.Data_Received+=testDuino_Data_Received;
        }

        private void testDuino_Data_Received(string receivedData)
        {
                Dispatcher.Invoke(() => recBox.Text += "REC: " + receivedData + "\n");
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            string toSend = toSendBox.Text;
            Dispatcher.Invoke(() => toSendBox.Text = "");
            testDuino.write(toSend);
        }

        private void stressButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 200; i++)
            {
                testDuino.write("N:"+i);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void toSendBox_KeyDown(object sender, KeyEventArgs e)
        {
            Key read = e.Key;
            if (read == Key.Return)
            {
                string toSend = toSendBox.Text;
                Dispatcher.Invoke(() => toSendBox.Text = "");
                testDuino.write(toSend);
            }
        }
    }
}
