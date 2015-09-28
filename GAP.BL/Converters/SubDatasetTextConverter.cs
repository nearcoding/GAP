using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class SubDatasetTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var subDatasetName = values[0].ToString();
            bool isNCT;
            if (bool.TryParse(values[1].ToString(), out isNCT))
            {
                return string.Format("{0}({1})", subDatasetName, isNCT ? "NCT" : "SHPT");
            }
            else
                return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw null;
        }
    }//end class
}//end namespace
