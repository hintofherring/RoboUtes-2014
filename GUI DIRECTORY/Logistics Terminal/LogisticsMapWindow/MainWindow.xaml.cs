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

using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Design;
using logisticsTools;
using System.Threading;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace LogisticsMapWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class mapWindow : Window
    {
        private logisticsConstants.paletteItems currentPinItem = logisticsConstants.paletteItems.redRock;
        Thread animationThread;
        SpeechSynthesizer speechsynth;

        public logisticsConstants.paletteItems selectedItem
        {
            set
            {
                currentPinItem = value;
            }
            get
            {
                return currentPinItem;
            }
        }

        public mapWindow()
        {
            InitializeComponent();
            map.Mode = new AerialMode(false);
            map.MouseDoubleClick += map_MouseDoubleClick;
            speechsynth = new SpeechSynthesizer();
            speechsynth.SetOutputToDefaultAudioDevice();
        }

        void map_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition(map);

            Location pinLocation = map.ViewportPointToLocation(mousePosition);

            Pushpin pin = new Pushpin();
            pin.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            pin.Location = pinLocation;
            pin.MouseRightButtonDown += pin_MouseRightButtonDown;

            switch (currentPinItem)
            {
                case logisticsConstants.paletteItems.redRock:
                    pin.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    break;
                case logisticsConstants.paletteItems.blueRock:
                    pin.Background = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                    break;
                case logisticsConstants.paletteItems.greenRock:
                    pin.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    break;
                case logisticsConstants.paletteItems.orangeRock:
                    pin.Background = new SolidColorBrush(Color.FromRgb(255, 205, 0));
                    break;
                case logisticsConstants.paletteItems.purpleRock:
                    pin.Background = new SolidColorBrush(Color.FromRgb(102, 0, 255));
                    break;
                case logisticsConstants.paletteItems.yellowRock:
                    pin.Background = new SolidColorBrush(Color.FromRgb(255, 255, 0));
                    break;
                case logisticsConstants.paletteItems.ALIEN:
                    pin.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    break;
            }

            map.Children.Add(pin);
        }

        void pin_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Dispatcher.Invoke(()=>map.Children.Remove((Pushpin)sender));
        }

        public void clearPins()
        {
            Dispatcher.Invoke(()=>map.Children.Clear());
        }

        /*You can use this event for all the Windows*/
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var senderWindow = sender as Window;
            senderWindow.WindowState = WindowState.Maximized;
            ShowOnMonitor(3, this);
            animationThread = new Thread(new ThreadStart(animate));
            animationThread.Start();
            this.Title = "MAP Window";
        }


        private void ShowOnMonitor(int monitor, Window window)
        {
            var screen = ScreenHandler.GetScreen(monitor);
            var currentScreen = ScreenHandler.GetCurrentScreen(this);
            window.WindowState = WindowState.Normal;
            window.Left = screen.WorkingArea.Left;
            window.Top = screen.WorkingArea.Top;
            window.Width = screen.WorkingArea.Width;
            window.Height = screen.WorkingArea.Height;
            window.Loaded += Window_Loaded;
        }

        public static class ScreenHandler
        {
            public static Screen GetCurrentScreen(Window window)
            {
                var parentArea = new System.Drawing.Rectangle((int)window.Left, (int)window.Top, (int)window.Width, (int)window.Height);
                return Screen.FromRectangle(parentArea);
            }

            public static Screen GetScreen(int requestedScreen)
            {
                var screens = Screen.AllScreens;
                var mainScreen = 0;
                if (screens.Length > 1 && mainScreen < screens.Length)
                {
                    return screens[requestedScreen];
                }
                return screens[0];
            }
        }

        public void placeRover(double lat, double lon) //TODO: add heading as a third parameter
        {
            MapPolygon polygon = new MapPolygon();
            polygon.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            polygon.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            polygon.StrokeThickness = 5;
            polygon.Opacity = 0.7;
            polygon.Locations = new LocationCollection() { 
            new Location(lat+.00002,lon), 
            new Location(lat,lon-.000012), 
            new Location(lat,lon+.000012)
            };

            map.Children.Add(polygon);
        }

        private void animate()
        {
            speechsynth.SpeakAsync("Logistics terminal coming online, welcome bridge team");
            for (double i = 4; i < 20.3; i+=.1)
            {
                Dispatcher.Invoke(()=>map.SetView(new Location(29.564753, -95.081363), i, 0));
                Thread.Sleep(20);
            }
            Dispatcher.Invoke(() => map.SetView(new Location(29.564753, -95.081363), 20.5, 0));
        }

        public void centerOnNasa()
        {
            Dispatcher.Invoke(()=>map.SetView(new Location(29.564753, -95.081363), 20.5, 0));  //location of the rock yard
        }

        private void MaximizeToSecondaryMonitor()
        {
            var secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

            if (secondaryScreen != null)
            {
                if (!this.IsLoaded)
                    this.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = secondaryScreen.WorkingArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (this.IsLoaded)
                    this.WindowState = WindowState.Normal;
            }
        }

    }
}
