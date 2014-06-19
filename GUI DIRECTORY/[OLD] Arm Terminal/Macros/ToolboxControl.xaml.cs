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

namespace Macros {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("Macros", true)]
    public partial class Macros : UserControl {
        
        public delegate void MacroPressedEventHandler(Button Source);
        public event MacroPressedEventHandler MacroPressed;

        public Macros() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if(MacroPressed != null)
                MacroPressed((Button)sender);
        }
    }
}
