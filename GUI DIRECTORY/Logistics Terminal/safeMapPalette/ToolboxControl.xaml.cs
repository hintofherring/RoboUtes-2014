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

using logisticsTools;

namespace safeMapPalette
{
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("safeMapPalette", true)]
    public partial class ToolboxControl : UserControl
    {
        public delegate void paletteSelectionChangedEventHandler(logisticsConstants.paletteItems Item);
        public event paletteSelectionChangedEventHandler newPaletteItemSelected;

        private logisticsConstants.paletteItems currentlySelectedItem = logisticsConstants.paletteItems.redRock; //red rock is the starting default..

        public logisticsConstants.paletteItems currentItem
        {
            set
            {
                currentlySelectedItem = value;
                if (newPaletteItemSelected != null)
                {
                    newPaletteItemSelected(currentlySelectedItem);
                }
            }
            get
            {
                return currentlySelectedItem;
            }
        }

        public ToolboxControl()
        {
            InitializeComponent();
        }

        void radioButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton button = (RadioButton)sender;
                switch (button.Uid)
                {
                    case "RED":
                        currentItem = logisticsConstants.paletteItems.redRock;
                        break;
                    case "BLUE":
                        currentItem = logisticsConstants.paletteItems.blueRock;
                        break;
                    case "YELLOW":
                        currentItem = logisticsConstants.paletteItems.yellowRock;
                        break;
                    case "ORANGE":
                        currentItem = logisticsConstants.paletteItems.orangeRock;
                        break;
                    case "GREEN":
                        currentItem = logisticsConstants.paletteItems.greenRock;
                        break;
                    case "PURPLE":
                        currentItem = logisticsConstants.paletteItems.purpleRock;
                        break;
                    case "ALIEN":
                        currentItem = logisticsConstants.paletteItems.ALIEN;
                        break;
                }
            }
            catch
            {
                //do nothing
                return;
            }
        }   

    }
}
