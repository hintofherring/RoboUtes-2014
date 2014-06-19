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
using System.Net.Sockets;
using System.IO;

namespace telnetReceiver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpListener listener;
        int serverPort = 20000;
        Socket client;
        byte[] receiveBytes;
        FileStream FS;

        public MainWindow()
        {
            InitializeComponent();
            FS = new FileStream("C:\\Users\\CORE\\Desktop\\vidTEST",FileMode.Create,FileAccess.Write);
            receiveBytes = new byte[1000000];
            listener = new TcpListener(IPAddress.Any, serverPort);
            listener.Start();
            listener.BeginAcceptSocket(acceptClient, null);
        }

        private void acceptClient(IAsyncResult ar)
        {
            client = listener.EndAcceptSocket(ar);
            string clientLocale = client.RemoteEndPoint.ToString();
            client.BeginReceive(receiveBytes, 0, receiveBytes.Length, SocketFlags.None, BytesReceived, null);
        }

        private void BytesReceived(IAsyncResult ar)
        {
            int count;
            count = client.EndReceive(ar);
            if (count == 0) //disconnect...
            {
                MessageBox.Show("zero bytes received, disconnected");
                Environment.Exit(0);
            }
            else
            {
                FS.Write(receiveBytes, 0, count);
               /* Dispatcher.Invoke(()=>primaryTextBox.Text+= Encoding.Default.GetString(receiveBytes,0,count));
                    Dispatcher.Invoke(() => {
                        if (primaryTextBox.Text.Length >= 100000)
                            primaryTextBox.Text = "\n\nCLEARED!!!!\n\n"; 
                                            });*/
            }
            try
            {
                client.BeginReceive(receiveBytes, 0, receiveBytes.Length, SocketFlags.None, BytesReceived, null);
            }
            catch
            {
                MessageBox.Show("begin receive exception caught, probably disconnect by remote host...");
                FS.Close();
            }
        }
    }
}
