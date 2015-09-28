using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class BooleanToVisibilityConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (bool.Parse(value.ToString()))
                return System.Windows.Visibility.Visible;
            else
                return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Windows.Visibility.Collapsed;
        }
    }//end class
}//end namespace
