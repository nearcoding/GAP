
using GAP.MainUI.ViewModels.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GAP.Converters
{
    public class LithologyImageBinding : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return string.Empty;
            return GlobalDataModel.LithologyImageFolder + value.ToString();
        }


        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
