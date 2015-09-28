using GAP.BL;
using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class WellIDToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var wellObject = HelperMethods.Instance.GetWellByID(value.ToString());

            return wellObject == null ? "Lithology" : wellObject.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }///end class
}//end namespace
