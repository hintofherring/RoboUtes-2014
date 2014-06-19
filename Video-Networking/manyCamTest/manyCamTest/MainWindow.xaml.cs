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

using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Controls;
using System.Threading;
using System.IO;
using System.Drawing;

namespace manyCamTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WPFBitmapConverter converter;
        string[] camIds;

        public MainWindow()
        {
            InitializeComponent();
            converter = new WPFBitmapConverter();

            ////////////////////Video Config - Start//////////////////////
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            camIds = new string[videoDevices.Count];
            int count = -1;
            foreach (FilterInfo vidDevice in videoDevices)
            {
                count++;
                    camIds[count] = vidDevice.MonikerString;
                    VideoCaptureDevice temp = new VideoCaptureDevice(vidDevice.MonikerString);
                    temp.Start();
                    temp.NewFrame += NewFrame;
                
            }
            MessageBox.Show("Found: " + videoDevices.Count + " cameras");
        }

        void NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bmp = eventArgs.Frame;
            int index = Array.IndexOf(camIds, ((VideoCaptureDevice)sender).Source);
            switch ( index )
            {
                case 0:
                    Dispatcher.Invoke(() => cam1Image.Source = (ImageSource)converter.Convert(bmp, null, null, null));
                    break;
                case 1:
                    Dispatcher.Invoke(() => cam2Image.Source = (ImageSource)converter.Convert(bmp, null, null, null));
                    break;
                case 2:
                    Dispatcher.Invoke(() => cam3Image.Source = (ImageSource)converter.Convert(bmp, null, null, null));
                    break;
                case 3:
                    Dispatcher.Invoke(() => cam4Image.Source = (ImageSource)converter.Convert(bmp, null, null, null));
                    break;
                case 4:
                    Dispatcher.Invoke(() => cam5Image.Source = (ImageSource)converter.Convert(bmp, null, null, null));
                    break;
                case 5:
                    Dispatcher.Invoke(() => cam6Image.Source = (ImageSource)converter.Convert(bmp, null, null, null));
                    break;
                case 6:
                    Dispatcher.Invoke(() => cam7Image.Source = (ImageSource)converter.Convert(bmp, null, null, null));
                    break;
                case 7:
                    Dispatcher.Invoke(() => cam8Image.Source = (ImageSource)converter.Convert(bmp, null, null, null));
                    break;
                case 8:
                    Dispatcher.Invoke(() => cam9Image.Source = (ImageSource)converter.Convert(bmp, null, null, null));
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }

    public class WPFBitmapConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)value).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
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
