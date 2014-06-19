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

namespace WheelMonitor {
    /// <summary>
    /// A single Wheel Monitor.  Has a title, speed, current, stall indicator, slip indicator,
    /// and an image that changes if there are any problems.
    /// </summary>
    [ProvideToolboxControl("WheelMonitor", true)]
    public partial class ToolboxControl : UserControl {


        public delegate void errorStateChangedEventHandler(bool errorState);
        public event errorStateChangedEventHandler errorStateChanged;

        private bool wheelSlip = false;
        private bool wheelStall = false;
        private bool _Spinning = false;

        Brush redBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        Brush greenBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        Brush blankBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));

		/// <summary>
		/// Title.
		/// </summary>
        public string Title {
            get {
                return title.Content.ToString();
            }
            set {
                title.Content = value;
            }
        }


        public bool Spinning {
            set
            {
                _Spinning = value;
                if (_Spinning)
                {
                    if (!WheelError)
                    {
                        spinningOverlay.Fill = greenBrush;
                    }
                }
                else
                {
                    if (!WheelError)
                    {
                        spinningOverlay.Fill = blankBrush;
                    }
                }
            }
            get
            {
                return _Spinning;
            }
        }

		/// <summary>
		/// Returns whether there are any problems with the wheel at the time.
		/// </summary>
        public bool WheelError {
            get {
                return wheelSlip || wheelStall;
            }
            set {
                if (value)
                {
                    spinningOverlay.Fill = redBrush;
                }
                else
                {
                    spinningOverlay.Fill = blankBrush;
                }
            }
        }

		/// <summary>
		/// Returns the speed of the wheel.  Set with an integer value to the desired m/s.
		/// </summary>
        public int Speed {
            get {
                return Int32.Parse(speed.Content.ToString().Split(' ')[0]);	// Make sure to remove m/s
            }
            set {
                speed.Content = value + " m/s";
            }
        }

		/// <summary>
		/// Returns the current draw of the wheel.  Set with an integer value to the desired mA.
		/// </summary>
        public int Current {
            get {
                return Int32.Parse(current.Content.ToString().Split(' ')[0]);	// Make sure to remove mA
            }
            set {
                current.Content = value + " mA";
            }
        }

		/// <summary>
		/// Returns whether the wheel is stalling.  Set to true if wheel is stalling, false otherwise.
		/// </summary>
        public bool Stall {
            get {
                return wheelStall;
            }
            set {
				// Turn light on
                if (value) {
                    if (!WheelError)
                    {
                        if (errorStateChanged != null)
                        {
                            errorStateChanged(true);
                        }
                    }
                    wheelStall = true;
                    stallIndicator.setIndicatorState(toggleIndicator.indicatorState.Red);
                    spinningOverlay.Fill = redBrush;
                }
				// Turn light off
                else {
                    wheelStall = false;
                    stallIndicator.setIndicatorState(toggleIndicator.indicatorState.Off);
                    if (!WheelError)
                    {
                        if (errorStateChanged != null)
                        {
                            errorStateChanged(false);
                        }
                        if (Spinning)
                        {
                            spinningOverlay.Fill = greenBrush;
                        }
                        else
                        {
                            spinningOverlay.Fill = blankBrush;
                        }
                    }
                }
            }
        }
		/// <summary>
		/// Returns whether the wheel is slipping.  Set to true if wheel is slipping, false otherwise.
		/// </summary>
        public bool Slip {
            get
            {
                return wheelSlip;
            }
            set
            {
                // Turn light on
                if (value)
                {
                    if (!WheelError)
                    {
                        if (errorStateChanged != null)
                        {
                            errorStateChanged(true);
                        }
                    }
                    wheelSlip = true;
                    slipIndicator.setIndicatorState(toggleIndicator.indicatorState.Red);
                    spinningOverlay.Fill = redBrush;
                }
                // Turn light off
                else
                {
                    wheelSlip = false;
                    slipIndicator.setIndicatorState(toggleIndicator.indicatorState.Off);
                    if (!WheelError)
                    {
                        if (errorStateChanged != null)
                        {
                            errorStateChanged(false);
                        }
                        if (Spinning)
                        {
                            spinningOverlay.Fill = greenBrush;
                        }
                        else
                        {
                            spinningOverlay.Fill = blankBrush;
                        }
                    }
                }
            }
        }

        public ToolboxControl() {
            InitializeComponent();
            //Spinning = false;

            redBrush.Opacity = .5;
            greenBrush.Opacity = .5;
            blankBrush.Opacity = 0;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = this.ActualHeight;
            primaryViewBox.Width = this.Width;
            primaryViewBox.Height = this.Height;
        }
    }
}
