﻿#pragma checksum "..\..\..\Custom Controls\ucTrackItemsControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E5345B771B9F145BBC3CE6760D340D9A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Common;
using Abt.Controls.SciChart.Common.Extensions;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Rendering.Common;
using Abt.Controls.SciChart.Rendering.HighQualityRasterizer;
using Abt.Controls.SciChart.Rendering.HighSpeedRasterizer;
using Abt.Controls.SciChart.Utility.Mouse;
using Abt.Controls.SciChart.Visuals;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.Visuals.PointMarkers;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GAP.BL;
using GAP.Converters;
using GAP.CustomControls;
using GAP.Custom_Controls;
using GAP.ExtendedControls;
using GAP.HelperClasses;
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
    /// ucTrackItemsControl
    /// </summary>
    public partial class ucTrackItemsControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 14 "..\..\..\Custom Controls\ucTrackItemsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal GAP.Custom_Controls.ucTrackItemsControl ucTrackItemsControl1;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Custom Controls\ucTrackItemsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer ScrollViewer1;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\Custom Controls\ucTrackItemsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl Control1;
        
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
            System.Uri resourceLocater = new System.Uri("/GAP;component/custom%20controls/uctrackitemscontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Custom Controls\ucTrackItemsControl.xaml"
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
            this.ucTrackItemsControl1 = ((GAP.Custom_Controls.ucTrackItemsControl)(target));
            return;
            case 2:
            this.ScrollViewer1 = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 3:
            this.Control1 = ((System.Windows.Controls.ItemsControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 4:
            
            #line 92 "..\..\..\Custom Controls\ucTrackItemsControl.xaml"
            ((GAP.CustomControls.SciChartExtended)(target)).Drop += new System.Windows.DragEventHandler(this.CustomChart1_Drop);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
