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

namespace ConsoleOutput {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("ConsoleOutput", true)]
    public partial class ToolboxControl : UserControl {


		/// <summary>
		/// Adds another line of output.  Usage: Output = outputStringText;
		/// </summary>
        public string Output {
            private get {
                return outputText.Text;
            }
            set {
                outputText.Text += value + "\n_> ";
            }
        }
        public ToolboxControl() {
            InitializeComponent();
        }
    }
}
