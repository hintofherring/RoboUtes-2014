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

namespace networkStatusIndicator {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("networkStatusIndicator", true)]
    public partial class ToolboxControl : UserControl {
        public bool connected{
            set {
                if (value) {
                    Dispatcher.Invoke(()=>connectionIndicator.setIndicatorState(toggleIndicator.indicatorState.Green));
                    Dispatcher.Invoke(()=>connectedLabel.Content = "CONNECTED");
                }
                else {
                    Dispatcher.Invoke(()=>connectionIndicator.setIndicatorState(toggleIndicator.indicatorState.Red));
                    Dispatcher.Invoke(()=>connectedLabel.Content = "NOT CONNECTED");
                }
            }
        }

        public ToolboxControl() {
            InitializeComponent();
        }
    }
}
