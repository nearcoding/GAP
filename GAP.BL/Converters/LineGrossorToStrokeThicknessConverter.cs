using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class LineGrossorToStrokeThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int grossorValue = 0;
            Int32.TryParse(value.ToString(), out grossorValue);
            return grossorValue + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0;
        }
    }//end class
}//end namespace
