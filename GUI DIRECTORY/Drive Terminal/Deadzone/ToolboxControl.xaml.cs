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

namespace Deadzone {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("Deadzone", true)]
    public partial class ToolboxControl : UserControl {

        public delegate void sliderChangedEventHandler(object sender,int newValue);
        public event sliderChangedEventHandler minValueChanged;
        public event sliderChangedEventHandler maxValueChanged;
        public event sliderChangedEventHandler sensitivityValueChanged;

		/// <summary>
		/// Returns the value of the minimum slider bar.
		/// </summary>
        public int Min {
            get {
                return Int32.Parse(minValue.Content.ToString());
            }
            private set {
                minValue.Content = value;
            }
        }

		/// <summary>
		/// Returns the value of the maximum slider bar.
		/// </summary>
        public int Max {
            get {
                return Int32.Parse(maxValue.Content.ToString());
            }
            private set {
                maxValue.Content = value;
            }
        }

		/// <summary>
		/// Returns the value of the sensitivity slider bar.
		/// </summary>
        public int Sensitivity {
            get {
                return Int32.Parse(sensitivityValue.Content.ToString());
            }
            private set {
                sensitivityValue.Content = value;
            }
        }

        public ToolboxControl() {
            InitializeComponent();

            minSlider.Value = 0;
            maxSlider.Value = 0;
            sensitivitySlider.Value = 0;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            Min = (int)e.NewValue;
            if (minValueChanged != null) {
                minValueChanged(sender, (int)e.NewValue);
            }
            if (Min > Max) //min cannot be greater than max.
            {
                maxSlider.Value = e.NewValue;
                if (maxValueChanged != null)
                {
                    maxValueChanged(maxSlider, (int)maxSlider.Value);
                }
            }
        }

        private void maxSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            Max = (int)e.NewValue;
            if (maxValueChanged != null) {
                maxValueChanged(sender, (int)e.NewValue);
            }
            if (Min > Max) //min cannot be greater than max.
            {
                minSlider.Value = e.NewValue;
                if (minValueChanged != null)
                {
                    minValueChanged(minSlider, (int)minSlider.Value);
                }
            }
        }

        private void sensitivitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            Sensitivity = (int)e.NewValue;
            if (sensitivityValueChanged != null) {
                sensitivityValueChanged(sender, (int)e.NewValue);
            }
        }
    }
}
