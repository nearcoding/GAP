using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GAP.BL
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;
            SolidColorBrush brush;
            brush = new SolidColorBrush(color);
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }//end class
}//end namespace
