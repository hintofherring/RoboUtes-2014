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

namespace HardwareMonitor
{
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("HardwareMonitor", true)]
    public partial class ToolboxControl : UserControl
    {
        public ToolboxControl()
        {
            InitializeComponent();
        }

        public void setCPUTemp(int temp)
        {
            Action work = delegate
            {
                cpuTempLabel.Text = temp+" °C";
                if (temp >= HWMonitorTools.CPU_MAXSAFETEMP)
                {
                    cpuWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Red);
                    cpuTempLabel.Background = new SolidColorBrush(HWMonitorTools.red);
                }
                else
                {
                    cpuWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                    cpuTempLabel.Background = new SolidColorBrush(HWMonitorTools.green);
                }
            };
            Dispatcher.Invoke(work);
        }

        public void setGPUTemp(int temp)
        {
            Action work = delegate
            {
                gpuTempLabel.Text = temp + " °C";
                if (temp >= HWMonitorTools.GPU_MAXSAFETEMP)
                {
                    gpuWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Red);
                    gpuTempLabel.Background = new SolidColorBrush(HWMonitorTools.red);
                }
                else
                {
                    gpuWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                    gpuTempLabel.Background = new SolidColorBrush(HWMonitorTools.green);
                }
            };
            Dispatcher.Invoke(work);
        }

        public void setCPULoad(int load)
        {
            Action work = delegate {
                load = load.Constrain(0, 100);
                cpuLoadLabel.Text = load + " %";
                if (load < 50) {
                    cpuLoadLabel.Background = new SolidColorBrush(HWMonitorTools.green);
                }
                else if (load >= 50 && load < 70) {
                    cpuLoadLabel.Background = new SolidColorBrush(HWMonitorTools.orange);
                }
                else if (load >= 70) {
                    cpuLoadLabel.Background = new SolidColorBrush(HWMonitorTools.red);
                }
            };
            Dispatcher.Invoke(work);
        }

        public void setGPULoad(int load)
        {
            Action work = delegate {
                load = load.Constrain(0, 100);
                gpuLoadLabel.Text = load + " %";
                if (load < 50) {
                    gpuLoadLabel.Background = new SolidColorBrush(HWMonitorTools.green);
                }
                else if (load >= 50 && load < 70) {
                    gpuLoadLabel.Background = new SolidColorBrush(HWMonitorTools.orange);
                }
                else if (load >= 70) {
                    gpuLoadLabel.Background = new SolidColorBrush(HWMonitorTools.red);
                }
            };
            Dispatcher.Invoke(work);
        }

        public void setRAMLoad(int load)
        {
            Action work = delegate {
                load = load.Constrain(0, 100);
                ramLoadLabel.Text = load + " %";
                if (load < 50) {
                    ramWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                    ramLoadLabel.Background = new SolidColorBrush(HWMonitorTools.green);
                }
                else if (load >= 50 && load < 70) {
                    ramWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Green);
                    ramLoadLabel.Background = new SolidColorBrush(HWMonitorTools.orange);
                }
                else if (load >= 70) {
                    ramWarningIndicator.setIndicatorState(toggleIndicator.indicatorState.Red);
                    ramLoadLabel.Background = new SolidColorBrush(HWMonitorTools.red);
                }
            };

            Dispatcher.Invoke(work);
        }

    }

    public static class HWMonitorTools
    {
        public static readonly int CPU_MAXSAFETEMP = 63;
        public static readonly int GPU_MAXSAFETEMP = 100;

        public static readonly Color red = Color.FromRgb(255, 0, 0);
        public static readonly Color orange = Color.FromRgb(255, 104, 0);
        public static readonly Color green = Color.FromRgb(0, 255, 0);

        public static int Constrain(this int value, int min, int max)
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
