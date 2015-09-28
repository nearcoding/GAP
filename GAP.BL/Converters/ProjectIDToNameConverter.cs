using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class ProjectIDToNameConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return HelperMethods.Instance.GetProjectByID(value.ToString()).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }///end class
}//end namespace
