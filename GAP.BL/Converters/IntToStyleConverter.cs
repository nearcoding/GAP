using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class IntToStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int intValue = 0;
            if (Int32.TryParse(value.ToString(), out intValue))
            {
                switch (intValue)
                {
                    case 0:
                        return @"Images\Dataset_Line_Style_Preview1.png";                        
                    case 1:
                       return @"Images\Dataset_Line_Style_Preview2.png";
                    case 2:
                        return @"Images\Dataset_Line_Style_Preview3.png";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
