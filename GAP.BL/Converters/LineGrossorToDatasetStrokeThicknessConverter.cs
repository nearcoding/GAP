using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class LineGrossorToDatasetStrokeThicknessConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int grossorValue = 0, lineStyle = 0;
            Int32.TryParse(values[0].ToString(), out grossorValue);
            Int32.TryParse(values[1].ToString(), out lineStyle);
            if (lineStyle == 0) return 1;
            return (int)grossorValue + 1;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null; 
        }
    }//end class
}