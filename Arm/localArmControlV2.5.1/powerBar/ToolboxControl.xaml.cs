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

namespace powerBar
{
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("powerBar", true)]
    public partial class ToolboxControl : UserControl
    {
        public ToolboxControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Give a percentage to fill the bar.
        /// </summary>
        /// <param name="value"></param>
        public void setFillValue(double value)
        {
            value = value.Constrain(0, 100);
            value = (100 - value)/100;
            Dispatcher.Invoke(()=>coverBar.Height = (fillBar.Height * value));
        }
    }

    public static class ExtensionMethods
    {
        public static double Constrain(this double value, double min, double max)
        {
            if (value > max)
            {
                return max;
            }
            else if (value < min)
            {
                return min;
            }
            else
            {
                return value;
            }
        }
    }
}
