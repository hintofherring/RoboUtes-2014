using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ROC_infoTools;

namespace ROCInfo_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ROCInfo rocInfoTest;

        public MainWindow()
        {
            InitializeComponent();
            rocInfoTest = ROCInfo.getInstance(500);
            rocInfoTest.updatedValue += rocInfoTest_updatedValue;
        }

        void rocInfoTest_updatedValue(ROCinfoConstants.hardwareInfoID ID, int val)
        {
            switch (ID){
                case ROCinfoConstants.hardwareInfoID.CPULoad:
                    Dispatcher.Invoke(() => cpuLoadLabel.Content = "" + val);
                    break;
                case ROCinfoConstants.hardwareInfoID.CPUTemp:
                    Dispatcher.Invoke(() => cpuTempLabel.Content = "" + val);
                    break;
                case ROCinfoConstants.hardwareInfoID.GPULoad:
                    Dispatcher.Invoke(() => gpuLoadLabel.Content = "" + val);
                    break;
                case ROCinfoConstants.hardwareInfoID.GPUTemp:
                    Dispatcher.Invoke(() => gpuTempLabel.Content = "" + val);
                    break;
                case ROCinfoConstants.hardwareInfoID.RAMLoad:
                    Dispatcher.Invoke(() => ramLoadLabel.Content = "" + val);
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
