using System;
using System.Windows.Data;

namespace GAP.BL
{
    public class LineStyleToStrokeDashArrayConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int lineStyle;
            Int32.TryParse(value.ToString(), out lineStyle);
            switch (lineStyle)
            {
                case 0:
                    return 0;
                case 2:
                    return new Double[2] { 8, 5 };                    
                case 3:
                    return new Double[2] { 3, 5 };                                
                default:
                    return  1;
            }            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0;
        }
    }//end class
}//end namespace
