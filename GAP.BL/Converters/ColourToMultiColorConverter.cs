using System;
using System.Windows.Data;

namespace GAP.BL.Converters
{
    public class ColourToMultiColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Colour currentColor = (Colour)values[0];
            int lineStyle = int.Parse(values[1].ToString());
            if (lineStyle == 0) return System.Drawing.Color.Transparent;// currentColor.GetMediaColor();
            return currentColor.GetMediaColor();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Windows.Media.Color color = (System.Windows.Media.Color)value;
            return new object[] { new Colour(color) };
        }
    }//end class
}//end namespace
