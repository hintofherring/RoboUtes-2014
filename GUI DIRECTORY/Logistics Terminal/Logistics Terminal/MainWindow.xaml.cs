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

using LogisticsMapWindow;
using logisticsMagnificationWindow;
using logisticsTools;
using commSockServer;
using snapShotTools;
using System.IO;

namespace Logistics_Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private mapWindow mapWin;
        private magnificationWindow leftMagWin;
        private magnificationWindow rightMagWin;

        commSockReceiver comSock;

        snapShotReceiver frontSSR;
        snapShotReceiver rightSSR;
        snapShotReceiver backSSR;
        snapShotReceiver leftSSR;

        public MainWindow()
        {
            InitializeComponent();

            comSock = new commSockReceiver(35006);
            comSock.IncomingLine += comSock_IncomingLine;
            comSock.newConnection += comSock_newConnection;
            comSock.connectionLost += comSock_connectionLost;
            comSock.beginAccept();

            frontSSR = new snapShotReceiver(35007);
            frontSSR.newSnapShotReceived += frontSSR_newSnapShotReceived;

            rightSSR = new snapShotReceiver(35009);
            rightSSR.newSnapShotReceived += rightSSR_newSnapShotReceived;

            backSSR = new snapShotReceiver(35008);
            backSSR.newSnapShotReceived += backSSR_newSnapShotReceived;

            leftSSR = new snapShotReceiver(35011);
            leftSSR.newSnapShotReceived += leftSSR_newSnapShotReceived;

            this.WindowState = System.Windows.WindowState.Maximized;
            mapPalette.newPaletteItemSelected += mapPalette_newPaletteItemSelected;
            mapWin = new mapWindow();
            mapWin.Show();
            leftMagWin = new magnificationWindow(1, "Left Magnification");
            leftMagWin.Show();
            rightMagWin = new magnificationWindow(2, "Right Magnification");
            rightMagWin.Show();
        }

        void leftSSR_newSnapShotReceived(byte[] receivedImage)
        {
            Action work = delegate
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(receivedImage);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                ImageSource ImgSrc = biImg as ImageSource;

                leftImage.Source = ImgSrc;
            };
            Dispatcher.Invoke(work);
        }

        void backSSR_newSnapShotReceived(byte[] receivedImage)
        {
            Action work = delegate
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(receivedImage);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                ImageSource ImgSrc = biImg as ImageSource;

                rearImage.Source = ImgSrc;
            };
            Dispatcher.Invoke(work);
        }

        void rightSSR_newSnapShotReceived(byte[] receivedImage)
        {
            Action work = delegate
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(receivedImage);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                ImageSource ImgSrc = biImg as ImageSource;

                rightImage.Source = ImgSrc;
            };
            Dispatcher.Invoke(work);
        }

        void frontSSR_newSnapShotReceived(byte[] receivedImage)
        {
            Action work = delegate
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(receivedImage);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                ImageSource ImgSrc = biImg as ImageSource;

                frontImage.Source = ImgSrc;
            };
            Dispatcher.Invoke(work);
        }

        void comSock_connectionLost()
        {
            Dispatcher.Invoke(() => networkConnectionInd.connected = false);
        }

        void comSock_newConnection(bool obj)
        {
            Dispatcher.Invoke(() => networkConnectionInd.connected = obj);
        }

        void comSock_IncomingLine(string obj)
        {
            Dispatcher.Invoke(() => internetINViz.addText(obj));
        }

        /// <summary>
        /// Used to get palette information to the bing map window
        /// </summary>
        /// <param name="Item"></param>
        void mapPalette_newPaletteItemSelected(logisticsConstants.paletteItems Item)
        {
            mapWin.selectedItem = Item;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                (sender as Rectangle).ContextMenu.IsEnabled = true;
                (sender as Rectangle).ContextMenu.PlacementTarget = (sender as Rectangle);
                (sender as Rectangle).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
                (sender as Rectangle).ContextMenu.IsOpen = true;
            }
            catch
            {
                //do nothing
                return;
            }
        } 

        public string getCurrentRockSelection()
        {
            return mapPalette.currentItem.ToString();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            switch ((string)item.Uid)
            {
                case "newFront":
                    comSock.write("LOG_FRONT");
                    break;
                case "magFrontL":
                    if (frontImage.Source != null)
                    {
                        leftMagWin.displayImage(frontImage.Source);
                    }
                    break;
                case "magFrontR":
                    if (frontImage.Source != null)
                    {
                        rightMagWin.displayImage(frontImage.Source);
                    }
                    break;
                case "newRight":
                    comSock.write("LOG_RIGHT");
                    break;
                case "magRightL":
                    if (rightImage.Source != null)
                    {
                        leftMagWin.displayImage(rightImage.Source);
                    }
                    break;
                case "magRightR":
                    if (rightImage.Source != null)
                    {
                        rightMagWin.displayImage(rightImage.Source);
                    }
                    break;
                case "newRear":
                    comSock.write("LOG_REAR");
                    break;
                case "magRearL":
                    if (rearImage.Source != null)
                    {
                        leftMagWin.displayImage(rearImage.Source);
                    }
                    break;
                case "magRearR":
                    if (rearImage.Source != null)
                    {
                        rightMagWin.displayImage(rearImage.Source);
                    }
                    break;
                case "newLeft":
                    comSock.write("LOG_LEFT");
                    break;
                case "magLeftL":
                    if (leftImage.Source != null)
                    {
                        leftMagWin.displayImage(leftImage.Source);
                    }
                    break;
                case "magLeftR":
                    if (leftImage.Source != null)
                    {
                        rightMagWin.displayImage(leftImage.Source);
                    }
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void centerMapButton_Click(object sender, RoutedEventArgs e)
        {
            mapWin.centerOnNasa();
        }

        private void clearMapButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MR = MessageBox.Show("Are you sure you want to clear all of the pins off of the map???","TEST CAPTION",MessageBoxButton.YesNo);
            if (MR == MessageBoxResult.Yes)
            {
                mapWin.clearPins();
            }
        }


        public class ByteImageConverter
        {
            public static ImageSource ByteToImage(byte[] imageData)
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(imageData);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                ImageSource ImgSrc = biImg as ImageSource;

                return ImgSrc;
            }
        }

        private void tempButton_Click(object sender, RoutedEventArgs e)
        {
            mapWin.placeRover(29.564698, -95.081483);
        }

    }
}
