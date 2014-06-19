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
using XboxController;

namespace liveXboxMonitorV2
{
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("liveXboxMonitorV2", true)]
    public partial class ToolboxControl : UserControl
    {
        public ToolboxControl()
        {
            InitializeComponent();
            rightBumper = false;
            leftBumper = false;
            rightTrigger = false;
            leftTrigger = false;
            yButton = false;
            xButton = false;
            bButton = false;
            aButton = false;
            startButton = false;
            backButton = false;
            dUp = false;
            dDown = false;
            dLeft = false;
            dRight = false;
        }

        #region xboxControllerAssignment
        public XboxController.XboxController xboxController
        {
            set
            {
                _xboxController = value;
                _xboxController.ButtonAPressed += _xboxController_ButtonAPressed;
                _xboxController.ButtonAReleased += _xboxController_ButtonAReleased;
                _xboxController.ButtonBackPressed += _xboxController_ButtonBackPressed;
                _xboxController.ButtonBackReleased += _xboxController_ButtonBackReleased;
                _xboxController.ButtonBPressed += _xboxController_ButtonBPressed;
                _xboxController.ButtonBReleased += _xboxController_ButtonBReleased;
                _xboxController.ButtonLeftShoulderPressed += _xboxController_ButtonLeftShoulderPressed;
                _xboxController.ButtonLeftShoulderReleased += _xboxController_ButtonLeftShoulderReleased;
                _xboxController.ButtonRightShoulderPressed += _xboxController_ButtonRightShoulderPressed;
                _xboxController.ButtonRightShoulderReleased += _xboxController_ButtonRightShoulderReleased;
                _xboxController.ButtonStartPressed += _xboxController_ButtonStartPressed;
                _xboxController.ButtonStartReleased += _xboxController_ButtonStartReleased;
                _xboxController.ButtonXPressed += _xboxController_ButtonXPressed;
                _xboxController.ButtonXReleased += _xboxController_ButtonXReleased;
                _xboxController.ButtonYPressed += _xboxController_ButtonYPressed;
                _xboxController.ButtonYReleased += _xboxController_ButtonYReleased;
                _xboxController.DPadDownPressed += _xboxController_DPadDownPressed;
                _xboxController.DPadDownReleased += _xboxController_DPadDownReleased;
                _xboxController.DPadLeftPressed += _xboxController_DPadLeftPressed;
                _xboxController.DPadLeftReleased += _xboxController_DPadLeftReleased;
                _xboxController.DPadRightPressed += _xboxController_DPadRightPressed;
                _xboxController.DPadRightReleased += _xboxController_DPadRightReleased;
                _xboxController.DPadUpPressed += _xboxController_DPadUpPressed;
                _xboxController.DPadUpReleased += _xboxController_DPadUpReleased;

                _xboxController.ThumbStickLeft += _xboxController_ThumbStickLeft;
                _xboxController.ThumbStickRight += _xboxController_ThumbStickRight;

                _xboxController.TriggerRight += _xboxController_TriggerRight;
                _xboxController.TriggerLeft += _xboxController_TriggerLeft;
            } 
        }

        void _xboxController_TriggerLeft(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            Dispatcher.Invoke(() => leftTrigInd.Height = 44 * args.GetLeftTrigger());
            leftTrigger = true;
        }

        void _xboxController_TriggerRight(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            Dispatcher.Invoke(()=>rightTrigInd.Height = 44*args.GetRightTrigger());
            rightTrigger = true;
        }

        void _xboxController_ThumbStickRight(object sender, EventArgs e)
        {
            int MAX_MAGNITUDE = 25; //TODO: TEMPORARY
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetRightThumbStick();
            float Y = vec.Item2.Map(-1, 1, -100, 100);
            float X = vec.Item1.Map(-1, 1, -100, 100);
            double rotationAngle = -((Math.Atan2(Y, X) * 180) / Math.PI);
            double MAGpercent = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            if (MAGpercent > 100) //gets rid of some slight noise
            {
                MAGpercent = 100;
            }
            double MAG = (MAGpercent / 100) * MAX_MAGNITUDE;

            this.Dispatcher.Invoke((Action)(() =>
            {
                rightStickSpinHand.Width = MAG;
                RotateTransform RT = new RotateTransform(rotationAngle);
                rightStickSpinHand.RenderTransform = RT;
            }));
        }

        #endregion
        void _xboxController_ThumbStickLeft(object sender, EventArgs e)
        {
            int MAX_MAGNITUDE = 25; //TEMPORARY
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetLeftThumbStick();
            float Y = vec.Item2.Map(-1, 1, -100, 100);
            float X = vec.Item1.Map(-1, 1, -100, 100);
            double rotationAngle = -((Math.Atan2(Y, X) * 180) / Math.PI);
            double MAGpercent = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            if (MAGpercent > 100) //gets rid of some slight noise
            {
                MAGpercent = 100;
            }
            double MAG = (MAGpercent / 100) * MAX_MAGNITUDE;

            this.Dispatcher.Invoke((Action)(() =>
            {
                leftStickSpinHand.Width = MAG;
                RotateTransform RT = new RotateTransform(rotationAngle);
                leftStickSpinHand.RenderTransform = RT;
            }));
        }

        void _xboxController_DPadUpReleased(object sender, EventArgs e)
        {
            dUp = false;
        }

        void _xboxController_DPadUpPressed(object sender, EventArgs e)
        {
            dUp = true;
        }

        void _xboxController_DPadRightReleased(object sender, EventArgs e)
        {
            dRight = false;
        }

        void _xboxController_DPadRightPressed(object sender, EventArgs e)
        {
            dRight = true;
        }

        void _xboxController_DPadLeftReleased(object sender, EventArgs e)
        {
            dLeft = false;
        }

        void _xboxController_DPadLeftPressed(object sender, EventArgs e)
        {
            dLeft = true;
        }

        void _xboxController_DPadDownReleased(object sender, EventArgs e)
        {
            dDown = false;
        }

        void _xboxController_DPadDownPressed(object sender, EventArgs e)
        {
            dDown = true;
        }

        void _xboxController_ButtonYReleased(object sender, EventArgs e)
        {
            yButton = false;
        }

        void _xboxController_ButtonYPressed(object sender, EventArgs e)
        {
            yButton = true;
        }

        void _xboxController_ButtonXReleased(object sender, EventArgs e)
        {
            xButton = false;
        }

        void _xboxController_ButtonXPressed(object sender, EventArgs e)
        {
            xButton = true;
        }

        void _xboxController_ButtonStartReleased(object sender, EventArgs e)
        {
            startButton = false;
        }

        void _xboxController_ButtonStartPressed(object sender, EventArgs e)
        {
            startButton = true;
        }

        void _xboxController_ButtonRightShoulderReleased(object sender, EventArgs e)
        {
            rightBumper = false;
        }

        void _xboxController_ButtonRightShoulderPressed(object sender, EventArgs e)
        {
            rightBumper = true;
        }

        void _xboxController_ButtonLeftShoulderReleased(object sender, EventArgs e)
        {
            leftBumper = false;
        }

        void _xboxController_ButtonLeftShoulderPressed(object sender, EventArgs e)
        {
            leftBumper = true;
        }

        void _xboxController_ButtonBReleased(object sender, EventArgs e)
        {
            bButton = false;
        }

        void _xboxController_ButtonBPressed(object sender, EventArgs e)
        {
            bButton = true;
        }

        void _xboxController_ButtonBackReleased(object sender, EventArgs e)
        {
            backButton = false;
        }

        void _xboxController_ButtonBackPressed(object sender, EventArgs e)
        {
            backButton = true;
        }

        void _xboxController_ButtonAReleased(object sender, EventArgs e)
        {
            aButton = false;
        }

        void _xboxController_ButtonAPressed(object sender, EventArgs e)
        {
            aButton = true;
        }

        private XboxController.XboxController _xboxController;

        #region buttonVariables

        public bool rightBumper
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(() => rightBumpInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(()=>rightBumpInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool leftBumper
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(()=>leftBumpInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(() => leftBumpInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool rightTrigger
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(()=>rightTrigInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(() => rightTrigInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool leftTrigger
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(()=>leftTrigInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(()=>leftTrigInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool yButton
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(() => yButtonInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(()=>yButtonInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool xButton
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(() => xButtonInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(()=>xButtonInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool bButton
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(() => bButtonInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(()=>bButtonInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool aButton
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(()=>aButtonInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(()=>aButtonInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool startButton
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(()=>startButtonInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(() => startButtonInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool backButton
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(()=>backButtonInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(() => backButtonInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool dUp
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(()=>dUpInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(() => dUpInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool dDown
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(()=>dDownInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(() => dDownInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool dLeft
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(()=>dLeftInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(() => dLeftInd.Visibility = Visibility.Hidden);
                }
            }
        }
        public bool dRight
        {
            set
            {
                if (value)
                {
                    Dispatcher.Invoke(()=>dRightInd.Visibility = Visibility.Visible);
                }
                else
                {
                    Dispatcher.Invoke(()=>dRightInd.Visibility = Visibility.Hidden);
                }
            }
        }

        #endregion
    }

    public static class ExtensionMethods
    {
        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public static double Map(this double value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
    }
}
