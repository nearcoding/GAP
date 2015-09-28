using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals;
using Abt.Controls.SciChart.Visuals.Axes;
using System;
using System.Windows;

namespace GAP.BL
{
    public class ScaleFactorModifier : ChartModifierBase
    {
        /// <summary>
        /// Prevents recursive calls of <see cref="SetZoomScaleFactor"/>.
        /// </summary>
        private bool _isUpdating;

        /// <summary>
        /// Defines the ZoomScaleFactor dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomScaleFactorProperty =
            DependencyProperty.Register("ZoomScaleFactor", typeof(double), typeof(ScaleFactorModifier), new PropertyMetadata(default(double), OnZoomScaleFactorChanged));

        /// <summary>
        /// Gets or sets zoom depth of the chart. It is calculated as Log10 of ratio between VisibleRange length and MaximumRange length
        /// (so, value of 0 means 100% zoom, value of -1 means 10% zoom etc.).
        /// </summary>
        public double ZoomScaleFactor
        {
            get { return (double)GetValue(ZoomScaleFactorProperty); }
            set { SetValue(ZoomScaleFactorProperty, value); }
        }

        public SciChartSurface ChartSurface{get;set;}
      
        /// <summary>
        /// Axis on which the zoom scale factor should be applied.
        /// </summary>
        public IAxis Axis
        {
            get
            {
                if (ParentSurface == null)
                    ParentSurface = this.ChartSurface;
                return ParentSurface.YAxis;
            }
        }

        private static void OnZoomScaleFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (ScaleFactorModifier)d;
            if (@this._isUpdating)
                return;
            @this.SetZoomScaleFactor(Math.Pow(10, (double)e.NewValue));
        }

        public override void OnAttached()
        {
            base.OnAttached();
            if (Axis == null) return;
            Axis.VisibleRangeChanged += OnVisibleRangeChanged;
            // First call to initialize the ZoomScaleFactor property.
             OnVisibleRangeChanged(this, null);
        }

        public override void OnDetached()
        {
            base.OnDetached();
            if (Axis == null) return;
            Axis.VisibleRangeChanged -= OnVisibleRangeChanged;
        }

        private void OnVisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
        {
            _isUpdating = true;
            ZoomScaleFactor = Math.Log10(GetZoomScaleFactor());
            _isUpdating = false;
        }

        /// <summary>
        /// Gets linear zoom scale factor (i. e. without logarithm).
        /// </summary>
        /// <returns>The linear zoom scale factor.</returns>
        private double GetZoomScaleFactor()
        {
            if (Axis.VisibleRange == null) return 0;
           return Axis.VisibleRange.AsDoubleRange().Diff / Axis.GetMaximumRange().AsDoubleRange().Diff;
        }

        /// <summary>
        /// Sets linear zoom scale factor (i. e. without logarithm).
        /// </summary>
        /// <param name="value">The new value.</param>
        private void SetZoomScaleFactor(double value)
        {
            var oldValue = GetZoomScaleFactor();
            var diff = value / (2 * oldValue) - 0.5;
            Axis.ZoomBy(diff, diff);
        }
    }//end class
}//end namespace