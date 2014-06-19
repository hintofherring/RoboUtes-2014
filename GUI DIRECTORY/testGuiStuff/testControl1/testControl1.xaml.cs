using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication1;

namespace testControl1 {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("testControl1", true)]
    public partial class ToolboxControl : UserControl {

        WPFBitmapConverter converter;
        public Ellipse front;
        public Ellipse back;
        //private int rotateAngle = 0;

        public ToolboxControl() {
            InitializeComponent();
            front = frontEllipse;
            back = backEllipse;
            this.SizeChanged += ToolboxControl_SizeChanged;

            converter = new WPFBitmapConverter();
            BitmapImage frontImg = (BitmapImage)converter.Convert(Properties.Resources.inclinometer_pitch, null, null, null);
            var frontBrush = new ImageBrush();
            frontBrush.ImageSource = frontImg;
            frontEllipse.Fill = frontBrush;
            frontEllipse.SnapsToDevicePixels = true;
            frontEllipse.Stretch = Stretch.Fill;

            BitmapImage backImg = (BitmapImage)converter.Convert(Properties.Resources.inclinometer_outsidedegrees, null, null, null);
            var backBrush = new ImageBrush();
            backBrush.ImageSource = backImg;
            backEllipse.Stretch = Stretch.Fill;

            backEllipse.Fill = backBrush;
            backEllipse.Width = this.Width;
            backEllipse.Height = this.Height;
        }

        void ToolboxControl_SizeChanged(object sender, SizeChangedEventArgs e) {
            this.Width = this.ActualWidth;
            this.Height = this.ActualWidth; //yes, width... trying to keep it square...
            frontEllipse.Width = backEllipse.Width * .7;
            frontEllipse.Height = backEllipse.Height * .7;
        }

        public void rotate(int degreesOffZero) {
            if (degreesOffZero == 360) {
                degreesOffZero = 0;
            }

            RotateTransform rotTransTemp = new RotateTransform(degreesOffZero, frontEllipse.Width / 2, frontEllipse.Height / 2);
            frontEllipse.RenderTransform = rotTransTemp;

        }

        
    }
}
