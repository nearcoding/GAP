using GAP.HelperClasses;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace GAP.Converters
{
    public class TrackColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            bool isSelected = false;
            bool.TryParse(value.ToString(),out isSelected);
            if (isSelected)
            {
                return GlobalData.MainWindow.FindResource("AccentColorBrush") as SolidColorBrush;
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }//end class
}//end namespace
