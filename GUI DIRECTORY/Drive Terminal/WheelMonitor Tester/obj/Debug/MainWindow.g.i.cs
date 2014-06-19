﻿#pragma checksum "..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9F70730B37BF4DFB61BDC40751A86334"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WheelMonitor;
using toggleIndicator;


namespace WheelMonitor_Tester {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 7 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WheelMonitor.ToolboxControl wheelMonitor;
        
        #line default
        #line hidden
        
        
        #line 8 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label stallLabel;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label slipLabel;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider speedSlider;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider currentSlider;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label spinLabel;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal toggleIndicator.ToolboxControl errorMonitor;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WheelMonitor Tester;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.wheelMonitor = ((WheelMonitor.ToolboxControl)(target));
            return;
            case 2:
            this.stallLabel = ((System.Windows.Controls.Label)(target));
            
            #line 8 "..\..\MainWindow.xaml"
            this.stallLabel.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.stallLabel_MouseDown);
            
            #line default
            #line hidden
            
            #line 8 "..\..\MainWindow.xaml"
            this.stallLabel.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.stallLabel_MouseUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.slipLabel = ((System.Windows.Controls.Label)(target));
            
            #line 9 "..\..\MainWindow.xaml"
            this.slipLabel.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.stallLabel_MouseDown);
            
            #line default
            #line hidden
            
            #line 9 "..\..\MainWindow.xaml"
            this.slipLabel.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.stallLabel_MouseUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.speedSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 10 "..\..\MainWindow.xaml"
            this.speedSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.speedSlider_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.currentSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 11 "..\..\MainWindow.xaml"
            this.currentSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.currentSlider_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.spinLabel = ((System.Windows.Controls.Label)(target));
            
            #line 14 "..\..\MainWindow.xaml"
            this.spinLabel.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.stallLabel_MouseDown);
            
            #line default
            #line hidden
            
            #line 14 "..\..\MainWindow.xaml"
            this.spinLabel.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.stallLabel_MouseUp);
            
            #line default
            #line hidden
            return;
            case 7:
            this.errorMonitor = ((toggleIndicator.ToolboxControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
