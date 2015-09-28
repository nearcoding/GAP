using System;
using System.Windows.Data;
using System.Windows.Media;

namespace GAP.BL
{
    public class BoolToForeColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isAccepted = Boolean.Parse(value.ToString());
            if (isAccepted)
                return new SolidColorBrush(Colors.Green);
            else
                return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
