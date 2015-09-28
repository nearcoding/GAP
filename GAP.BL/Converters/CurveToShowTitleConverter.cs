using System;
using System.Windows.Data;
using System.Windows;
namespace GAP.BL
{
    public class CurveToShowTitleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 3 && values[0] == DependencyProperty.UnsetValue && values[1] == DependencyProperty.UnsetValue && values[2] == DependencyProperty.UnsetValue)
                return "Lithology";
            //title for dataset                
            string wellName, datasetName, units;
            
            wellName = values[0].ToString();
            wellName = HelperMethods.Instance.GetWellByID(wellName).Name;
            datasetName = values[1].ToString();
            units = values[2].ToString();
            return string.Format("{0}/{1} [{2}]", wellName, datasetName, units);

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }//end class
}//end namespace
