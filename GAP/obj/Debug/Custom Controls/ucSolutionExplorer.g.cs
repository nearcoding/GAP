﻿#pragma checksum "..\..\..\Custom Controls\ucSolutionExplorer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "83B4F4C51288D0FA38EE5A952089A6E9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GAP.BL;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace GAP.Custom_Controls {
    
    
    /// <summary>
    /// ucSolutionExplorer
    /// </summary>
    public partial class ucSolutionExplorer : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\Custom Controls\ucSolutionExplorer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView TreeView1;
        
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
            System.Uri resourceLocater = new System.Uri("/GAP;component/custom%20controls/ucsolutionexplorer.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Custom Controls\ucSolutionExplorer.xaml"
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
            
            #line 9 "..\..\..\Custom Controls\ucSolutionExplorer.xaml"
            ((GAP.Custom_Controls.ucSolutionExplorer)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ucSolutionExplorer_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TreeView1 = ((System.Windows.Controls.TreeView)(target));
            
            #line 21 "..\..\..\Custom Controls\ucSolutionExplorer.xaml"
            this.TreeView1.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.TreeViewOfProjects_PreviewMouseDown);
            
            #line default
            #line hidden
            
            #line 22 "..\..\..\Custom Controls\ucSolutionExplorer.xaml"
            this.TreeView1.PreviewMouseMove += new System.Windows.Input.MouseEventHandler(this.TreeViewOfProjects_PreviewMouseMove);
            
            #line default
            #line hidden
            
            #line 23 "..\..\..\Custom Controls\ucSolutionExplorer.xaml"
            this.TreeView1.MouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.TreeViewOfProjects_MouseRightButtonDown);
            
            #line default
            #line hidden
            
            #line 24 "..\..\..\Custom Controls\ucSolutionExplorer.xaml"
            this.TreeView1.SelectedItemChanged += new System.Windows.RoutedPropertyChangedEventHandler<object>(this.TreeViewOfProjects_OnSelectedItemChanged);
            
            #line default
            #line hidden
            
            #line 25 "..\..\..\Custom Controls\ucSolutionExplorer.xaml"
            this.TreeView1.SizeChanged += new System.Windows.SizeChangedEventHandler(this.TreeViewOfProjects_OnSizeChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
