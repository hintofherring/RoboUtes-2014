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
using System.IO;
using System.Threading;

namespace commFeedViz {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("commFeedViz", true)]
    public partial class commFeedViz : UserControl {
        TextBoxStreamWriter _writer;
        bool autoScroll = true;

        public commFeedViz() {
            InitializeComponent();
            _writer = new TextBoxStreamWriter(terminalTextBox,this);
            autoScrollRadio.IsChecked = true;
        }

        public void setTitle(string title) {
            Dispatcher.Invoke(() => titleLabel.Content = title);
        }

        public void addText(string toAdd) {
            Action action = delegate()
            {
                if(terminalTextBox.Text.Length >= 7000){    //keeps you from printing stupidly huge amounts of data to the console. If it gets too long it will slow down the GUI
                    terminalTextBox.Text = ">>>Feed Visualizer RESET to avoid massive string>>>\n\n\n" + toAdd;
                }
                else{
                    terminalTextBox.Text += toAdd;
                }
            };
            Dispatcher.Invoke(action);
        }

        /// <summary>
        /// Used to link things like console output to the commFeedViz. I.E.:
        /// Console.SetOut(commFeedViz.getStreamLink());
        /// </summary>
        /// <returns></returns>
        public TextWriter getStreamLink() {
            return _writer;
        }

        public void clearText() {

        }

        private void terminalTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (autoScroll) {
                terminalTextBox.Focus();
                terminalTextBox.CaretIndex = terminalTextBox.Text.Length;
                terminalTextBox.ScrollToEnd();
            }
        }

        private void autoScrollRadio_Click(object sender, RoutedEventArgs e) {
            RadioButton button = (RadioButton)sender;
            autoScroll = !autoScroll;
            button.IsChecked = autoScroll;
        }
    }
}
