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

namespace LiveDriveData {
    /// <summary>
    /// Live drive data component.  Keeps track of wheel monitoring.
    /// </summary>
    [ProvideToolboxControl("LiveDriveData", true)]
    public partial class ToolboxControl : UserControl {

        private bool _stuck = false;
        private int _PIDGoal = 0;
        private int _actualSpeed = 0;
		/// <summary>
		/// Returns whether the stuck indicator light is on or off.  Setting true
        /// will turn the light on, false will turn it off.
		/// </summary>
		public bool Stuck {
			get {
                return _stuck;
			}
			set {
                _stuck = value;
                if (_stuck)
                {
                    Dispatcher.Invoke(()=>stuckIndicator.setIndicatorState(toggleIndicator.indicatorState.Red));
                }
                else {
                    Dispatcher.Invoke(()=>stuckIndicator.setIndicatorState(toggleIndicator.indicatorState.Off));
                }
			}
		}


		/// <summary>
		/// Returns the PID Goal in m/s.  Set with an integer to change the value to the desired m/s.
		/// </summary>
        public int PIDGoal {
            get {
                return _PIDGoal;
            }
            set {
                _PIDGoal = value;
                Dispatcher.Invoke(()=>pidGoalSpeed.Content = _PIDGoal + " m/s");
            }
        }

		/// <summary>
		/// Returns the PID Actual in m/s.  Set with an integer to change the value to the desired m/s.
		/// </summary>
        public int actualSpeed {
            get {
                return _actualSpeed;
            }
            set {
                _actualSpeed = value;
                Dispatcher.Invoke(() => actualSpeedLabel.Content = _actualSpeed + " m/s");
            }
        }

        public WheelMonitor.ToolboxControl UpLeftWheel;
        public WheelMonitor.ToolboxControl UpRightWheel;
        public WheelMonitor.ToolboxControl BackLeftWheel;
        public WheelMonitor.ToolboxControl BackRightWheel;

        public ToolboxControl() {
            InitializeComponent();

            UpLeftWheel = upLeftWheelMon;
            UpRightWheel = upRightWheelMon;
            BackLeftWheel = backLeftWheelMon;
            BackRightWheel = backRightWheelMon;

            UpLeftWheel.errorStateChanged += Wheel_errorStateChanged;
            UpRightWheel.errorStateChanged += Wheel_errorStateChanged;
            BackLeftWheel.errorStateChanged += Wheel_errorStateChanged;
            BackRightWheel.errorStateChanged += Wheel_errorStateChanged;

            Stuck = false;
        }

        void Wheel_errorStateChanged(bool errorState)
        {
            if (errorState) //If a wheel is stuck, check if the others are. If they ALL are then show the whole robot as stuck.
            {
                if (Convert.ToInt32(UpLeftWheel.WheelError) + Convert.ToInt32(UpRightWheel.WheelError) + Convert.ToInt32(BackLeftWheel.WheelError) + Convert.ToInt32(BackRightWheel.WheelError) >= 3)
                {
                    Stuck = true;
                }
            }
            else //If a wheel is now NOT stuck, then show the robot as not stuck.
            {
                Stuck = false;
            }
        }
    }
}
