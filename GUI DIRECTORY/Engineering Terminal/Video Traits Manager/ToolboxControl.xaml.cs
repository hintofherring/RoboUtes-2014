﻿using System;
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

namespace Video_Traits_Manager
{
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("Video_Traits_Manager", true)]
    public partial class ToolboxControl : UserControl
    {
        public delegate void videoTraitsUpdate(FeedID ID, int quality, int fps);
        public event videoTraitsUpdate userUpdatedVideoTraits;

        private int panTiltQuality = 15;
        private int panTiltFPS = 10;
        private object panTiltLock = 1;

        private int workspaceQuality = 15;
        private int workspaceFPS = 10;
        private object workspaceLock = 1;

        private int palmQuality = 15;
        private int palmFPS = 10;
        private object palmLock = 1;

        private int humerusQuality = 15;
        private int humerusFPS = 10;
        private object humerusLock = 1;

        public ToolboxControl()
        {
            InitializeComponent();

            Action work = delegate
            {
                panTiltQualitySlider.Value = panTiltQuality;
                panTiltFPSSlider.Value = panTiltFPS;

                workspaceQualitySlider.Value = workspaceQuality;
                workspaceFPSSlider.Value = workspaceFPS;

                palmQualitySlider.Value = palmQuality;
                palmFPSSlider.Value = palmFPS;

                humerusQualitySlider.Value = humerusQuality;
                humerusFPSSlider.Value = humerusFPS;
            };
            Dispatcher.Invoke(work);
        }

        public enum FeedID
        {
            pantilt,
            workspace,
            humerus,
            palm
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                Slider target = (Slider)sender;
                switch (target.Uid)
                {
                    case "ptQuality":
                        lock (panTiltLock)
                        {
                            panTiltQuality = (int)e.NewValue;
                        }
                        break;

                    case "ptFPS":
                        lock (panTiltLock)
                        {
                            panTiltFPS = (int)e.NewValue;
                        }
                        break;

                    case "workspaceQuality":
                        lock (workspaceLock)
                        {
                            workspaceQuality = (int)e.NewValue;
                        }
                        break;

                    case "workspaceFPS":
                        lock (workspaceLock)
                        {
                            workspaceFPS = (int)e.NewValue;
                        }
                        break;

                    case "palmQuality":
                        lock (palmLock)
                        {
                            palmQuality = (int)e.NewValue;
                        }
                        break;

                    case "palmFPS":
                        lock (palmLock)
                        {
                            palmFPS = (int)e.NewValue;
                        }
                        break;

                    case "humerusQuality":
                        lock (humerusLock)
                        {
                            humerusQuality = (int)e.NewValue;
                        }
                        break;

                    case "humerusFPS":
                        lock (humerusLock)
                        {
                            humerusFPS = (int)e.NewValue;
                        }
                        break;
                }
            }
            catch
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button target = (Button)sender;
                switch (target.Uid)
                {
                    case "panTiltUpdate":
                        if (userUpdatedVideoTraits != null)
                        {
                            lock (panTiltLock)
                            {
                                userUpdatedVideoTraits(FeedID.pantilt, panTiltQuality, panTiltFPS);
                            }
                        }
                        break;

                    case "workspaceUpdate":
                        if (userUpdatedVideoTraits != null)
                        {
                            lock (workspaceLock)
                            {
                                userUpdatedVideoTraits(FeedID.workspace, workspaceQuality, workspaceFPS);
                            }
                        }
                        break;

                    case "palmUpdate":
                        if (userUpdatedVideoTraits != null)
                        {
                            lock (palmLock)
                            {
                                userUpdatedVideoTraits(FeedID.palm, palmQuality, palmFPS);
                            }
                        }
                        break;

                    case "humerusUpdate":
                        if (userUpdatedVideoTraits != null)
                        {
                            lock (humerusLock)
                            {
                                userUpdatedVideoTraits(FeedID.humerus, humerusQuality, humerusFPS);
                            }
                        }
                        break;
                }
            }
            catch
            {

            }
        }
    }
}
