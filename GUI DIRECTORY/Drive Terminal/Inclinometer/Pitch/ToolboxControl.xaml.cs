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

namespace Pitch {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("Pitch", true)]
    public partial class ToolboxControl : UserControl {

        /// <summary>
        /// Sets the angle of pitch rotation in degrees.
        /// </summary>
        public double rotateAngle {
            set {
                rotate.Angle = value;
                rollImage.RenderTransform = rotate;
            }
        }

        private RotateTransform rotate;

        public ToolboxControl() {
            InitializeComponent();

            rotate = new RotateTransform();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e) {
            this.Width = this.ActualHeight;
            primaryViewBox.Width = this.Width;
            primaryViewBox.Height = this.Height;
        }
    }
}
