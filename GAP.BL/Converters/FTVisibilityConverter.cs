using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class FTVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isRenderSeriesVisible = bool.Parse(values[0].ToString());
            bool isLithologyVisible = bool.Parse(values[1].ToString());

            if (isLithologyVisible && isRenderSeriesVisible)
                return false;
            else
                return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }//end class
}//end namespace
