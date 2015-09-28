using System;
using System.Windows;
using System.Windows.Data;

namespace GAP.BL
{
    public class InverseVisibilityConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isVisibility;
            Boolean.TryParse(value.ToString(), out isVisibility);
            if (!isVisibility)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }//end class
}//end namespace
