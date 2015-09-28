using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class DatasetIDToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var datasetObject = HelperMethods.Instance.GetDatasetByID(value.ToString());
            return datasetObject == null ? "Lithology" : datasetObject.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }//end class
}//end namespace
