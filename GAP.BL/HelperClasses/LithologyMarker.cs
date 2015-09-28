using Abt.Controls.SciChart.Rendering.Common;
using Abt.Controls.SciChart.Visuals.PointMarkers;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GAP.BL.HelperClasses
{
    public class LithologyMarker : BasePointMarker
    {
        public LithologyMarker(string imageName)
        {                
            imgBrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Lits\\" + imageName +".bmp", UriKind.Relative)),
                Stretch = Stretch.None,
                TileMode = TileMode.Tile
            };
            imgBrush.Viewport = new Rect(0, 0, 20, 20);
            imgBrush.ViewportUnits = BrushMappingMode.Absolute;
        }

        ImageBrush imgBrush;
        protected override void DrawInternal(IRenderContext2D context, IEnumerable<Point> centers, IPen2D pen, IBrush2D brush)
        {
            foreach (var center in centers)
            {
                DrawInternal(context, center.X, center.Y, pen, brush);
            }
        }

        protected override void DrawInternal(IRenderContext2D context, double x, double y, IPen2D pen, IBrush2D brush)
        {
            var halfWidth = (float)(Width * 0.5);
            var halfHeight = (float)(Height * 0.5);

            double left = x - halfWidth;
            double right = x + halfWidth;

            var s = this.DataContext as FastLineRenderableSeries;
            var shouldBeHeight = context.ViewportSize.Height / double.Parse(s.YAxis.VisibleRange.Diff.ToString());

            var points = new[]
                {
                    new Point(left,y),
                    new Point(left,y+shouldBeHeight),
                    new Point(right,y+shouldBeHeight),
                    new Point(right,y),
                    new Point(left,y)
                };
            var otherBrush = context.CreateBrush(imgBrush);
            context.FillPolygon(otherBrush, points);
        }
    }//end class
}//end namespace
