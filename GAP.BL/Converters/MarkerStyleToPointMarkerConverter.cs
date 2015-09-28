using Abt.Controls.SciChart.Visuals.PointMarkers;
using GAP.BL.HelperClasses;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace GAP.BL
{
    public class MarkerStyleToPointMarkerConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int markerStyle, markerSize;
            bool shouldApplyBorderColor;
            Color markerColor;

            Int32.TryParse(values[0].ToString(), out markerStyle);
            Int32.TryParse(values[1].ToString(), out markerSize);
            bool.TryParse(values[2].ToString(), out shouldApplyBorderColor);
            var color = (Colour)values[3];
            markerColor = Color.FromRgb(color.R, color.G, color.B);

            if (markerStyle == 0) return null;
            IPointMarker marker = null;
            switch (markerStyle)
            {
                case 1:
                    marker = new SquarePointMarker();
                    break;
                case 2:
                    marker = new DiamondMarker();
                    break;
                case 3:
                    marker = new TriangleMarker();
                    break;
                case 4:
                    marker = new EllipsePointMarker();
                    break;
                case 5:
                    marker = new RightTriangleMarker();
                    break;
                case 6:
                    marker = new LeftTriangleMarker();
                    break;
            }
            if (marker != null)
            {
                marker.Height = (markerSize + 3) * 2;
                marker.Width = (markerSize + 3) * 2;
                marker.Fill = markerColor;
                marker.Stroke = shouldApplyBorderColor ? Colors.Black : Colors.Transparent;
                marker.StrokeThickness = shouldApplyBorderColor ? 1 : 0;
                return marker;
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }//end class
}//end namespace
