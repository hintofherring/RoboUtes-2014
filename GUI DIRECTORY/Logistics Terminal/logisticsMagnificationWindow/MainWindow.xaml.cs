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

using System.Windows.Forms;

namespace logisticsMagnificationWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class magnificationWindow : Window
    {
        private int monitor;
        private string windowName;

        public magnificationWindow(int _monitor, string _WindowName)
        {
            windowName = _WindowName;
            monitor = _monitor;
            InitializeComponent();
        }

        private void MaximizeToThirdMonitor()
        {
            System.Windows.Forms.Screen secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();
            System.Windows.Forms.Screen tertiaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => s != secondaryScreen).FirstOrDefault();

            if (tertiaryScreen != null)
            {
                if (!this.IsLoaded)
                    this.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = tertiaryScreen.WorkingArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (this.IsLoaded)
                    this.WindowState = WindowState.Normal;
            }

            else if (secondaryScreen != null)
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

        private void MaximizeToFourthMonitor()
        {
            System.Windows.Forms.Screen secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();
            System.Windows.Forms.Screen tertiaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => s != secondaryScreen).FirstOrDefault();
            System.Windows.Forms.Screen fourthScreen = System.Windows.Forms.Screen.AllScreens.Where(s => s != tertiaryScreen).FirstOrDefault();

            if (fourthScreen != null)
            {
                if (!this.IsLoaded)
                    this.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = fourthScreen.WorkingArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (this.IsLoaded)
                    this.WindowState = WindowState.Normal;
            }

            else if (tertiaryScreen != null)
            {
                if (!this.IsLoaded)
                    this.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = tertiaryScreen.WorkingArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (this.IsLoaded)
                    this.WindowState = WindowState.Normal;
            }

            else if (secondaryScreen != null)
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

        public void displayImage(ImageSource IS){
            Dispatcher.Invoke(()=>magImage.Source = IS);
        }

        /*You can use this event for all the Windows*/
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var senderWindow = sender as Window;
            senderWindow.WindowState = WindowState.Maximized;
            ShowOnMonitor(monitor, this);
            this.Title = windowName;
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
    }
}
