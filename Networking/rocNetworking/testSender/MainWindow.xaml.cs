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
using System.Net;
using commSockClient;

namespace testSender {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        commSockSender CSock;

        public MainWindow() {
            InitializeComponent();
            connectButton.Click += connectButton_Click;
            disconnectButton.Click += disconnectButton_Click;

            CSock = new commSockSender("testSock");
            CSock.connectionStatusChanged += CSock_connectionStatusChanged;
            CSock.incomingLineEvent += CSock_incomingLineEvent;
        }

        void disconnectButton_Click(object sender, RoutedEventArgs e) {
            CSock.disconnect();
            
        }

        void CSock_incomingLineEvent(string obj) {
            Dispatcher.Invoke(() => receivedBox.Text = obj);
        }

        void CSock_connectionStatusChanged(bool commSockIsConnected) {
            if (commSockIsConnected) {
                Dispatcher.Invoke(()=>connectedRectangle.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0)));
               // MessageBox.Show("green from connectionStatusChanged Event");
            }
            else {
                Dispatcher.Invoke(()=>connectedRectangle.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0)));
              //  MessageBox.Show("red from connectionStatusChanged Event");
            }
        }

        void sendButton_Click(object sender, RoutedEventArgs e) {
            if (CSock.isConnected) {
                CSock.sendMessage(messageToSendBox.Text);
                Dispatcher.Invoke(()=>connectedRectangle.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0)));
             //   MessageBox.Show("Green from CSock.isConnected returning true when trying to send a message");
            }
        }

        void connectButton_Click(object sender, RoutedEventArgs e) {
          //  if (CSock == null) {
                
           // }
            CSock.beginConnect(IPAddress.Parse(IP_TextBox.Text), 2000);
        }

        private void floodButton_Click(object sender, RoutedEventArgs e)
        {
            if (CSock.isConnected)
            {
                for (int i = 0; i < 100; i++)
                {
                    CSock.sendMessage("ROC FLOOD: " + i);
                }
            }
        }
    }
}
