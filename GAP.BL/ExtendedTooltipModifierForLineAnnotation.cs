using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GAP.BL
{
    public class ExtendedTooltipModifier : TooltipModifier
    {
        public ExtendedTooltipModifier()
        {
            RadiusToGetElement = 0.5;
            ReceiveHandledEvents = true;
        }

        public double RadiusToGetElement { get; set; }

        protected override IEnumerable<SeriesInfo> GetSeriesInfoAt(Point point)
        {
            if (XAxis.GetCurrentCoordinateCalculator() == null) return base.GetSeriesInfoAt(point);
            var xData = (IComparable)XAxis.GetCurrentCoordinateCalculator().GetDataValue(point.X);
            var yData = (IComparable)YAxis.GetCurrentCoordinateCalculator().GetDataValue(point.Y);

            var nonNullAnnotations = ParentSurface.Annotations.Where
               (u => u.X1 != null && u.X2 != null && u.Y1 != null && u.Y2 != null &&
                   Math.Round(decimal.Parse(u.X1.ToString())) != 0 && Math.Round(decimal.Parse(u.X2.ToString())) != 1);

            var annotations = nonNullAnnotations.Where(u =>
                ((double.Parse(u.X1.ToString()) + RadiusToGetElement >= double.Parse(xData.ToString())
                && double.Parse(u.X1.ToString()) - RadiusToGetElement <= double.Parse(xData.ToString()))
                ||
                double.Parse(u.Y1.ToString()) + RadiusToGetElement >= double.Parse(yData.ToString())
                && double.Parse(u.Y1.ToString()) - RadiusToGetElement <= double.Parse(yData.ToString()))
                ||
                (double.Parse(u.X2.ToString()) + RadiusToGetElement >= double.Parse(xData.ToString()) &&
                double.Parse(u.X2.ToString()) - RadiusToGetElement <= double.Parse(xData.ToString())
                ||
                double.Parse(u.Y2.ToString()) + RadiusToGetElement >= double.Parse(yData.ToString()) &&
                double.Parse(u.Y2.ToString()) - RadiusToGetElement <= double.Parse(yData.ToString())));

            var lst = new List<SeriesInfo>();
            foreach (var annotation in annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended)))
            {
                if (ParentSurface.RenderableSeries.Any())
                {
                    var seriesInfo = new SeriesInfo(ParentSurface.RenderableSeries[0]);
                    seriesInfo.YValue = yData;
                    seriesInfo.XValue = xData;
                    lst.Add(seriesInfo);
                }
            }

            if (lst != null && lst.Any()) lst[0].IsHit = true;

            return lst;
        }
    }//end class
}//end namespace
