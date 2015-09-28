using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GAP.BL
{
    public class LithologyPrintingTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string wellText, datasetText;
            var well = HelperMethods.Instance.GetWellByID(values[0].ToString());
            wellText = well == null ? "Lithology" : well.Name;

            var dataset = HelperMethods.Instance.GetDatasetByID(values[1].ToString());
            datasetText = dataset == null ? "Lithology" : dataset.Name;
            
            if (wellText == "Lithology" && datasetText == "Lithology")
            {
                return "Include Lithology";
            }
            else
                return string.Format("{0}/{1}", wellText, datasetText);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] { string.Empty };
        }
    }
}
