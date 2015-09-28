using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GAP.Converters
{
    public class GridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            double val = double.Parse(value.ToString());
            GridLength gridLength;
            if (!Double.IsNaN(val))
            {
                gridLength = new GridLength(val);
            }
            else
            {
                gridLength = new GridLength(0);
            }


            return gridLength;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GridLength val = (GridLength)value;

            return val.Value;
        }
    }
    //public class GridLengthValueConverter : IValueConverter
    //{
    //    GridLengthConverter _converter = new GridLengthConverter();

    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return _converter.ConvertFrom(value);
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return new GridLength(double.Parse(value.ToString()));            
    //    }
    //}
}
