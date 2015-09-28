using System;
using System.Windows.Data;

namespace GAP.BL
{ 
    public class ColourToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Colour currentColor = (Colour)value;
            return currentColor.GetMediaColor().ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Windows.Media.Color color = (System.Windows.Media.Color)value;
            return new Colour(color);
        }
    }//end class
}//end namespace
