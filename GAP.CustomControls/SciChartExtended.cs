using Abt.Controls.SciChart.Visuals;
using Abt.Controls.SciChart.Visuals.Annotations;
using GAP.BL;
using System;
using System.Collections.Generic;
using System.Windows;

namespace GAP.CustomControls
{
    public class SciChartExtended : SciChartSurface, IDisposable
    {
        
        public SciChartExtended()
        {
            LithologyAnnotations = new List<AnnotationBase>();
            FormationAnnotations = new List<AnnotationBase>();
        }

        public string Chart
        {
            get { return (string)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        public static readonly DependencyProperty ChartProperty =
            DependencyProperty.Register("Chart", typeof(string), typeof(SciChartExtended), new PropertyMetadata(string.Empty));

        public string Track
        {
            get { return (string)GetValue(TrackProperty); }
            set { SetValue(TrackProperty, value); }
        }

        public static readonly DependencyProperty TrackProperty =
            DependencyProperty.Register("Track", typeof(string), typeof(SciChartExtended), new PropertyMetadata(string.Empty));

        public List<AnnotationBase> LithologyAnnotations { get; set; }

        public List<AnnotationBase> FormationAnnotations { get; set; }

        void IDisposable.Dispose()
        {
            LithologyAnnotations.Clear();
            FormationAnnotations.Clear();
        }
    }//end class
}//end namespace
