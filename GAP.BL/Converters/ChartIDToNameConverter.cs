using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class ChartIDToNameConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return HelperMethods.Instance.GetChartByID(value.ToString()).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }//end class
}//end namespae
