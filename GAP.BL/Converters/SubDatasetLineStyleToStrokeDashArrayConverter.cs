using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Media;

namespace GAP.BL
{
    public class SubDatasetLineStyleToStrokeDashArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int lineStyle;
            List<double> dash;
            Int32.TryParse(value.ToString(), out lineStyle);
            switch (lineStyle)
            {
                case 1:
                    dash = new List<double> { 8, 5 };
                    return new DoubleCollection(dash);
                case 2:
                    dash = new List<double> { 3, 5 };
                    return new DoubleCollection(dash);
                default:
                    return 1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0;
        }
    }//end class
}
