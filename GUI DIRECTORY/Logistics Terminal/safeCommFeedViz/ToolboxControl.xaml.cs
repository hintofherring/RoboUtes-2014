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

namespace safeCommFeedViz
{
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("safeCommFeedViz", true)]
    public partial class ToolboxControl : UserControl
    {
        private string _title;
        public String title
        {
            set
            {
                _title = value;
                Dispatcher.Invoke(() => titleLabel.Content = _title);
            }
            get
            {
                return _title;
            }
        }

        public ToolboxControl()
        {
            InitializeComponent();
        }

        public void addText(string text)
        {
            Dispatcher.Invoke(new Action<string>(appendText),new object[] {text});
        }

        private void appendText(string text){
            if (scrollBox.Text.Length >= 2000)
                {
                    scrollBox.Text = "****WIPED TO AVOID MASSIVE STRING**** \r";
                }
                scrollBox.Text += text;

                scrollBox.ScrollToEnd();
        }
    }
}
