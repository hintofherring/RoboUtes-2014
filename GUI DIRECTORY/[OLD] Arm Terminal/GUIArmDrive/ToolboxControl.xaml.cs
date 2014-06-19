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

namespace GUIArmDrive {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("GUIArmDrive", true)]
    public partial class GUIArmDrive : UserControl {

        /// <summary>
        /// Returns a GUIDriveUIDParser object containing variables set to values that specify what axis the button was on (Axis) and what magnitude
        /// the button represents.
        /// </summary>
        /// <param name="directionUID"></param>
        public delegate void directionPressedEventHandler(GUIDriveUIDParser t);


        public event directionPressedEventHandler directionPressed;

        private int[] magnifications = { 1, 1, 1 };
        private GUIDriveUIDParser currentPathInfo;

        private bool grayedOut = true;

        public bool isGrayedOut {
            get {
                return grayedOut;
            }
            set {
                if (value) { //if isGrayedOut is set equal to false, make it visible
                    grayedOut = true;
                    grayedOutRectangle.IsEnabled = true;
                    grayedOutRectangle.Visibility = System.Windows.Visibility.Visible;
                }
                else {
                    grayedOut = false;
                    grayedOutRectangle.IsEnabled = false;
                    grayedOutRectangle.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        public GUIArmDrive() {
            InitializeComponent();
            xMagSlider.ValueChanged += MagSlider_ValueChanged;
            yMagSlider.ValueChanged += MagSlider_ValueChanged;
            zMagSlider.ValueChanged += MagSlider_ValueChanged;
        }

        void MagSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            Slider currentSlider = (Slider)sender;
            switch (currentSlider.Uid) {
                case "X":
                    xMagValLabel.Content = currentSlider.Value + " X";
                    magnifications[0] = (int)currentSlider.Value;
                    break;
                case "Y":
                    yMagValLabel.Content = currentSlider.Value + " X";
                    magnifications[1] = (int)currentSlider.Value;
                    break;
                case "Z":
                    zMagValLabel.Content = currentSlider.Value + " X";
                    magnifications[2] = (int)currentSlider.Value;
                    break;
            }
        }

        void PathMouseLeave(object sender, MouseEventArgs e) {
            if (sender.GetType() == typeof(Path)) {
                Path thing = (Path)sender;
                if (currentPathInfo != null) {
                    thing.Fill = currentPathInfo.baseColor;
                    currentPathInfo = null;
                }
            }
            else if (sender.GetType() == typeof(Rectangle)) {
                Rectangle thing = (Rectangle)sender;
                if (currentPathInfo != null) {
                    thing.Fill = currentPathInfo.baseColor;
                    currentPathInfo = null;
                }
            }
        }

        void PathMouseEnter(object sender, MouseEventArgs e) {
            //int[] magnifications = { (int)xMagSlider.Value, (int)yMagSlider.Value, (int)zMagSlider.Value };
            if(sender.GetType() == typeof(Path)){
                Path thing = (Path)sender;
                if (currentPathInfo == null) {
                    currentPathInfo = new GUIDriveUIDParser(thing.Uid,magnifications);
                }
                thing.Fill = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            }
            else if (sender.GetType() == typeof(Rectangle)) {
                Rectangle thing = (Rectangle)sender;
                if (currentPathInfo == null) {
                    currentPathInfo = new GUIDriveUIDParser(thing.Uid, magnifications);
                }
                thing.Fill = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            }
            
            
        }

        void PathMouseDown(object sender, MouseButtonEventArgs e) {
            if (sender.GetType() == typeof(Path)) {
                Path thing = (Path)sender;
                if (directionPressed != null)
                    directionPressed(new GUIDriveUIDParser(thing.Uid, magnifications));
                thing.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else if (sender.GetType() == typeof(Rectangle)) {
                Rectangle thing = (Rectangle)sender;
                if (directionPressed != null)
                    directionPressed(new GUIDriveUIDParser(thing.Uid, magnifications));
                thing.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        void PathMouseUp(object sender, MouseButtonEventArgs e) {
            if (sender.GetType() == typeof(Path)) {
                Path thing = (Path)sender;
                thing.Fill = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            }
            else if (sender.GetType() == typeof(Rectangle)) {
                Rectangle thing = (Rectangle)sender;
                thing.Fill = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            }
        }

    }

    public class GUIDriveUIDParser {
        public Brush baseColor;
        public String Axis;
        public int Magnitude;

        public GUIDriveUIDParser(string uid , int[] Magnifications) {
            String[] uidComps = uid.Split(' ');
            Magnitude = int.Parse(uidComps[2]);
            Axis = uidComps[1];
            switch (uidComps[0]){
                case "Red":
                    baseColor = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    break;
                case "Green":
                    baseColor = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    break;
                case "Blue":
                    baseColor = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                    break;
            }
            switch (Axis) {
                case "X":
                    Magnitude *= Magnifications[0];
                    break;
                case "Y":
                    Magnitude *= Magnifications[1];
                    break;
                case "Z":
                    Magnitude *= Magnifications[2];
                    break;
            }
        }
    }
}
