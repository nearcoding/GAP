using System;
using System.Windows;
using System.Windows.Data;

namespace GAP.BL
{
    public class Operand1VisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int srNo = 0, displayIndex = 0;
            bool isEntitySelected = false;
            Int32.TryParse(values[0].ToString(), out srNo);
            Int32.TryParse(values[1].ToString(), out displayIndex);
            bool.TryParse(values[2].ToString(), out isEntitySelected);
            if (!isEntitySelected) return true;
            if (srNo < displayIndex) return false;
            return true ;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }//end class
}//end namespae
