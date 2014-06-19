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

namespace Macros_Tester {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            testMacroPanel.MacroPressed+=testMacroPanel_MacroPressed;
        }

        private void testMacroPanel_MacroPressed(object sender) {
            Button testButton = (Button)sender;
            MessageBox.Show((string)testButton.Content);
        }
    }
}
