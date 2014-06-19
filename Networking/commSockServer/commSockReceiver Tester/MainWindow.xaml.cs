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

using commSockServer;

namespace commSockReceiver_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        commSockReceiver CSR;
        public MainWindow()
        {
            InitializeComponent();
            CSR = new commSockReceiver(35000);
            CSR.beginAccept();
            CSR.IncomingLine += CSR_IncomingLine;
            CSR.newConnection += CSR_newConnection;
        }

        void CSR_newConnection(bool obj)
        {
            //MessageBox.Show("NEW CONNECTION INCOMING");
        }

        void CSR_IncomingLine(string obj)
        {
            Dispatcher.Invoke(() => receivedDataBox.Text += obj);
        }

        private void sendDataButton_Click(object sender, RoutedEventArgs e)
        {
            CSR.write(" BLARG ");
        }

        private void floodButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                CSR.write("MC FLOOD: " + i+"\r");
            }
        }
    }
}
