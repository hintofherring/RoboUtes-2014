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

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace renderer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class renderWindow : Window
    {
        object sync = 1;
        public renderWindow()
        {
            InitializeComponent();
        }

        public void setLeft(byte[] image){
            Dispatcher.Invoke(()=>leftImage.Source = (ImageSource)ByteImageConverter.ByteToImage(image));
        }

        public void setRight(byte[] image)
        {
            Dispatcher.Invoke(()=>rightImage.Source = (ImageSource)ByteImageConverter.ByteToImage(image));
        }

        public void setMerged(byte[] image)
        {
            Dispatcher.Invoke(() => mergedImage.Source = (ImageSource)ByteImageConverter.ByteToImage(image));
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
}
