using Abt.Controls.SciChart.Visuals.RenderableSeries;
using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class LithologyVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isRenderSeriesVisible, isLithologyVisible;
            bool.TryParse(values[0].ToString(), out isRenderSeriesVisible);
            bool.TryParse(values[1].ToString(), out isLithologyVisible);
            return isRenderSeriesVisible && isLithologyVisible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class FullLithologyVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isRenderSeriesVisible, isLithologyVisible;
            bool.TryParse(values[0].ToString(), out isRenderSeriesVisible);
            bool.TryParse(values[1].ToString(), out isLithologyVisible);
            return (isRenderSeriesVisible && isLithologyVisible);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
