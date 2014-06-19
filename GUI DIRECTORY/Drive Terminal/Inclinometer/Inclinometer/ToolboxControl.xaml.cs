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

namespace Inclinometer {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("Inclinometer_Window", true)]
    public partial class ToolboxControl : UserControl {

        private bool rollLight = false;
		/// <summary>
		/// Returns true if the roll warning light is on, false otherwise.  Sets
        /// the light to on or off.
		/// </summary>
        public bool RollWarningLight {
            get {
                return rollLight;
            }
            set {
                if (value) {
                    rollLight = true;
                    rollWarningLight.setIndicatorState(toggleIndicator.indicatorState.Red);
                }
                else {
                    rollLight = false;
                    rollWarningLight.setIndicatorState(toggleIndicator.indicatorState.Off);
                }
            }
        }

        private bool pitchLight = false;
		/// <summary>
		/// Returns true if the pitch warning light is on, false otherwise.  Sets
        /// the light to on or off.
		/// </summary>
        public bool PitchWarningLight {
            get {
                return pitchLight;
            }
            set {
                if (value) {
                    pitchLight = true;
                    pitchWarningLight.setIndicatorState(toggleIndicator.indicatorState.Red);
                }
                else {
                    pitchLight = false;
                    pitchWarningLight.setIndicatorState(toggleIndicator.indicatorState.Off);
                }
            }
        }

        private int _pitchAngle = 0;
        /// <summary>
        /// returns the angle the pitchInclinometer is currently set to. Can also
        /// set the angle to render the pitchInclinometer.
        /// </summary>
        public int pitchAngle {
            get {
                return _pitchAngle;
            }
            set {
                _pitchAngle = value;
                pitchInclinometer.rotateAngle = _pitchAngle;
            }

        }

        private int _rollAngle = 0;
        /// <summary>
        /// returns the angle the pitchInclinometer is currently set to. Can also
        /// set the angle to render the pitchInclinometer.
        /// </summary>
        public int rollAngle {
            get {
                return _rollAngle;
            }
            set {
                _rollAngle = value;
                rollInclinometer.rotateAngle = _rollAngle;
            }

        }

        public ToolboxControl() {
            InitializeComponent();
        }
    }
}
