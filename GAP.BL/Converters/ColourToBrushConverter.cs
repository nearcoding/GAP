using System;
using System.Windows.Data;
using System.Windows.Media;

namespace GAP.BL
{
    public class ColourToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var color = (Colour)value;
            SolidColorBrush brush;
            brush = new SolidColorBrush(color);
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var brush = (System.Windows.Media.SolidColorBrush)value;
            var color=brush.Color;
            return new Colour(color.A, color.R, color.G, color.B);
        }
    }//end class
}//end namespace
