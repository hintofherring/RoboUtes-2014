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
using System.Drawing;

namespace toggleIndicator {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("toggleIndicator", true)]
    public partial class ToolboxControl : UserControl {

        WPFBitmapConverter converter = new WPFBitmapConverter();

        public ToolboxControl() {
            InitializeComponent();
            //onState = false;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e) {
            this.Width = this.ActualHeight;
            primaryViewBox.Width = this.Width;
            primaryViewBox.Height = this.Height;
        }

        public void setIndicatorState(indicatorState state){
            switch (state){
                case indicatorState.Off:
                    Dispatcher.Invoke(() => indicatorREDImage.Visibility = Visibility.Hidden);
                    Dispatcher.Invoke(() => indicatorGREENImage.Visibility = Visibility.Hidden);
                    Dispatcher.Invoke(() => indicatorOFFImage.Visibility = Visibility.Visible);
                    break;
                case indicatorState.Red:
                    Dispatcher.Invoke(() => indicatorREDImage.Visibility = Visibility.Visible);
                    Dispatcher.Invoke(() => indicatorGREENImage.Visibility = Visibility.Hidden);
                    Dispatcher.Invoke(() => indicatorOFFImage.Visibility = Visibility.Hidden);
                    break;
                case indicatorState.Green:
                    Dispatcher.Invoke(() => indicatorREDImage.Visibility = Visibility.Hidden);
                    Dispatcher.Invoke(() => indicatorGREENImage.Visibility = Visibility.Visible);
                    Dispatcher.Invoke(() => indicatorOFFImage.Visibility = Visibility.Hidden);
                    break;
            }
        }
    }

    public enum indicatorState { Green = 1, Red, Off };
}
