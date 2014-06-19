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

using System.Threading;
using System.Windows.Media.Animation;

namespace macroInProgressWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread fadeThread;
        private volatile bool active = true;

        public MainWindow()
        {
            InitializeComponent();
            fadeThread = new Thread(new ThreadStart(fade));
            fadeThread.Start();
           // Dispatcher.Invoke(() => mainRectangle.Fill.Opacity = 100);
        }

        private void fade()
        {
                while (true)
                {
                    if (active)
                    {
                        Action work = delegate
                        {
                            DoubleAnimation test = new DoubleAnimation(0, TimeSpan.FromSeconds(2));
                            mainRectangle.BeginAnimation(Rectangle.OpacityProperty, test);

                            DoubleAnimation test2 = new DoubleAnimation(1, TimeSpan.FromSeconds(1));
                            warningLabel.BeginAnimation(Rectangle.OpacityProperty, test);
                        };
                        Dispatcher.Invoke(work);

                        Thread.Sleep(1000);
                    }
                    else
                    {
                        break;
                    }
                    if (active)
                    {
                        Action work = delegate
                        {
                            DoubleAnimation test = new DoubleAnimation(1, TimeSpan.FromSeconds(2));
                            mainRectangle.BeginAnimation(Rectangle.OpacityProperty, test);

                            DoubleAnimation test2 = new DoubleAnimation(0, TimeSpan.FromSeconds(1));
                            warningLabel.BeginAnimation(Rectangle.OpacityProperty, test);
                        };
                        Dispatcher.Invoke(work);

                        Thread.Sleep(1000);
                    }
                    else
                    {
                        break;
                    }
                }
                Dispatcher.Invoke(()=>this.Close());
        }

        public void stop()
        {
            active = false;
        }
    }
}
