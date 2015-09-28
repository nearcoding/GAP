using Abt.Controls.SciChart.Model.DataSeries;
using GAP.BL;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

namespace GAP.MainUI.ViewModels.Converters
{
    public class DepthAndCurvesToDataSeriesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var depthAndCurves = value as ObservableCollection<DepthCurveInfo>;
            var lstToBeUsed = depthAndCurves.Except(depthAndCurves.Where(u => u.Curve <= -999 && u.Curve >= (decimal)-999.99)).ToList();
            var dataSeries = new XyDataSeries<double, double>();
            dataSeries.AcceptsUnsortedData = true;
            lstToBeUsed.ForEach(u => dataSeries.Append((double)u.Curve, (double)u.Depth));
            return lstToBeUsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }//end class
}//end namespace
