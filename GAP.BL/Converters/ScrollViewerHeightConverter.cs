using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class ScrollViewerHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (double.Parse(value.ToString()) > 40)
                return double.Parse(value.ToString()) - 40;
            
                return double.Parse(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
