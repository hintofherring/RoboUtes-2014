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

using snapShotTools;
using System.IO;

namespace snapShotReceiver_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        snapShotReceiver SSReceiver;
        public MainWindow()
        {
            InitializeComponent();
            SSReceiver = new snapShotReceiver(35005);
            SSReceiver.newSnapShotReceived += SSReceiver_newSnapShotReceived;
        }

        private void SSReceiver_newSnapShotReceived(byte[] receivedImage)
        {
            receivedImageViewer.Source = ByteImageConverter.ByteToImage(receivedImage);
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
    }
}
