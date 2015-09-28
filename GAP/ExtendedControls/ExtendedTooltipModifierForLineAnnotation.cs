using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals;
using Abt.Controls.SciChart.Visuals.Annotations;
using GAP.BL;
using GAP.Custom_Controls;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GAP.ExtendedControls
{
    public class ExtendedTooltipModifier : TooltipModifier
    {
      //  public CustomCharts CustomChart { get; set; }
        public ExtendedTooltipModifier()
        {
            Loaded += ExtendedTooltipModifier_Loaded;
        }

        void ExtendedTooltipModifier_Loaded(object sender, RoutedEventArgs e)
        {
          //  CustomChart = ((this.ParentSurface as SciChartSurface).Parent as Grid).Parent as CustomCharts;
        }
        protected override IEnumerable<SeriesInfo> GetSeriesInfoAt(Point point)
        {
            if (!ParentSurface.Annotations.Any(u => u.GetType() == typeof(LineAnnotationExtended))) return base.GetSeriesInfoAt(point);
            if (ParentSurface.RenderableSeries.Count < 1) return base.GetSeriesInfoAt(point);
            var xData = (IComparable)XAxis.GetCurrentCoordinateCalculator().GetDataValue(point.X);
            var yData = (IComparable)YAxis.GetCurrentCoordinateCalculator().GetDataValue(point.Y);

            var nonNullAnnotations = ParentSurface.Annotations.Where
               (u => u.X1 != null && u.X2 != null && u.Y1 != null && u.Y2 != null &&
                   Math.Round(decimal.Parse(u.X1.ToString())) != 0 && Math.Round(decimal.Parse(u.X2.ToString())) != 1);

            double radiusToGetElement = 0.1;
            var annotations = nonNullAnnotations.Where(u =>
                ((double.Parse(u.X1.ToString()) + radiusToGetElement >= double.Parse(xData.ToString())
                && double.Parse(u.X1.ToString()) - radiusToGetElement <= double.Parse(xData.ToString()))
                ||
                double.Parse(u.Y1.ToString()) + radiusToGetElement >= double.Parse(yData.ToString())
                && double.Parse(u.Y1.ToString()) - radiusToGetElement <= double.Parse(yData.ToString()))
                ||
                (double.Parse(u.X2.ToString()) + radiusToGetElement >= double.Parse(xData.ToString()) &&
                double.Parse(u.X2.ToString()) - radiusToGetElement <= double.Parse(xData.ToString())
                ||
                double.Parse(u.Y2.ToString()) + radiusToGetElement >= double.Parse(yData.ToString()) &&
                double.Parse(u.Y2.ToString()) - radiusToGetElement <= double.Parse(yData.ToString())));

            List<SeriesInfo> lst = new List<SeriesInfo>();
            foreach (var annotation in annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended)))
            {
                SeriesInfo seriesInfo = new SeriesInfo(ParentSurface.RenderableSeries[0]);
                seriesInfo.YValue = yData;
                seriesInfo.XValue = xData;
                lst.Add(seriesInfo);
            }

            if (lst != null && lst.Any()) lst[0].IsHit = true;

            return lst;
        }
    }//end class
}//end namespace
