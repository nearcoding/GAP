using System;
using System.Windows.Data;

namespace GAP.BL
{
  public  class TrackIDToNameConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return  HelperMethods.Instance.GetTrackByID(value.ToString()).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }///end class
}///end namespace
