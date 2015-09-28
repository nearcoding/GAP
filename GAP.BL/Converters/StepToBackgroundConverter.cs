using System;
using System.Windows.Data;
using System.Windows.Media;
namespace GAP.BL
{
    public class StepToBackgroundConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int currentStepValue, sourceStepValue;
            Int32.TryParse(values[0].ToString(), out currentStepValue);
            Int32.TryParse(values[1].ToString(), out sourceStepValue);
            if (currentStepValue == sourceStepValue)
                return new SolidColorBrush(Color.FromRgb(0, 255, 0));

            return new SolidColorBrush(Color.FromRgb(255, 0, 0));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }//end class
}//end namespace
