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

namespace liveXboxMonitor
{
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("liveXboxMonitor", true)]
    public partial class ToolboxControl : UserControl
    {

        #region buttonVariables

        public bool rightBumper
        {
            set
            {
                if (value)
                {
                    rightBumpInd.Visibility = Visibility.Visible;
                }
                else
                {
                    rightBumpInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool leftBumper
        {
            set
            {
                if (value)
                {
                    leftBumpInd.Visibility = Visibility.Visible;
                }
                else
                {
                    leftBumpInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool rightTrigger
        {
            set
            {
                if (value)
                {
                    rightTrigInd.Visibility = Visibility.Visible;
                }
                else
                {
                    rightTrigInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool leftTrigger
        {
            set
            {
                if (value)
                {
                    leftTrigInd.Visibility = Visibility.Visible;
                }
                else
                {
                    leftTrigInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool yButton
        {
            set
            {
                if (value)
                {
                    yButtonInd.Visibility = Visibility.Visible;
                }
                else
                {
                    yButtonInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool xButton
        {
            set
            {
                if (value)
                {
                    xButtonInd.Visibility = Visibility.Visible;
                }
                else
                {
                    xButtonInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool bButton
        {
            set
            {
                if (value)
                {
                    bButtonInd.Visibility = Visibility.Visible;
                }
                else
                {
                    bButtonInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool aButton
        {
            set
            {
                if (value)
                {
                    aButtonInd.Visibility = Visibility.Visible;
                }
                else
                {
                    aButtonInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool startButton
        {
            set
            {
                if (value)
                {
                    startButtonInd.Visibility = Visibility.Visible;
                }
                else
                {
                    startButtonInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool backButton
        {
            set
            {
                if (value)
                {
                    backButtonInd.Visibility = Visibility.Visible;
                }
                else
                {
                    backButtonInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool dUp
        {
            set
            {
                if (value)
                {
                    dUpInd.Visibility = Visibility.Visible;
                }
                else
                {
                    dUpInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool dDown
        {
            set
            {
                if (value)
                {
                    dDownInd.Visibility = Visibility.Visible;
                }
                else
                {
                    dDownInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool dLeft
        {
            set
            {
                if (value)
                {
                    dLeftInd.Visibility = Visibility.Visible;
                }
                else
                {
                    dLeftInd.Visibility = Visibility.Hidden;
                }
            }
        }
        public bool dRight
        {
            set
            {
                if (value)
                {
                    dRightInd.Visibility = Visibility.Visible;
                }
                else
                {
                    dRightInd.Visibility = Visibility.Hidden;
                }
            }
        }

        #endregion

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
    }
}
