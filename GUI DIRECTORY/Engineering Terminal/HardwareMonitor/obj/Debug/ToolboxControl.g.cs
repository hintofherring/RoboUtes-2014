﻿#pragma checksum "..\..\ToolboxControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CF3B92E572033D94FDAF70BE2F49F989"
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
using toggleIndicator;


namespace HardwareMonitor {
    
    
    /// <summary>
    /// ToolboxControl
    /// </summary>
    public partial class ToolboxControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 66 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal toggleIndicator.ToolboxControl cpuWarningIndicator;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox cpuTempLabel;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox cpuLoadLabel;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal toggleIndicator.ToolboxControl gpuWarningIndicator;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox gpuTempLabel;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox gpuLoadLabel;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal toggleIndicator.ToolboxControl ramWarningIndicator;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ramLoadLabel;
        
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
            System.Uri resourceLocater = new System.Uri("/HardwareMonitor;component/toolboxcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ToolboxControl.xaml"
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
            this.cpuWarningIndicator = ((toggleIndicator.ToolboxControl)(target));
            return;
            case 2:
            this.cpuTempLabel = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.cpuLoadLabel = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.gpuWarningIndicator = ((toggleIndicator.ToolboxControl)(target));
            return;
            case 5:
            this.gpuTempLabel = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.gpuLoadLabel = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.ramWarningIndicator = ((toggleIndicator.ToolboxControl)(target));
            return;
            case 8:
            this.ramLoadLabel = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

