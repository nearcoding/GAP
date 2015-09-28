using Abt.Controls.SciChart.Rendering.Common;
using Abt.Controls.SciChart.Visuals.PointMarkers;
using System.Collections.Generic;
using System.Windows;

namespace GAP.BL.HelperClasses
{
    public class RightTriangleMarker : BasePointMarker
    {
        protected override void DrawInternal(IRenderContext2D context, double x, double y, IPen2D pen, IBrush2D brush)
        {
            List<Point> lst = new List<Point>();
            lst.Add(new Point
            {
                X = x,
                Y = y
            });
            DrawInternal(context, lst, pen, brush);
        }

        protected override void DrawInternal(IRenderContext2D context, IEnumerable<Point> centers, IPen2D pen, IBrush2D brush)
        {
            double halfWidth = Width * 0.5;
            double halfHeight = Height * 0.5;

            foreach (var center in centers)
            {
                //x0
                //
                //x1       x2
                double top = center.Y - halfHeight;
                double bottom = center.Y + halfHeight;
                double left = center.X - halfWidth;
                double right = center.X + halfHeight;

                var points = new[]
                {
                    new Point(left,top),
                    new Point(left,bottom),
                    new Point(right, bottom),
                    new Point(left,top)
                };
                context.FillPolygon(brush, points);
                context.DrawLines(pen, points);
            }
        }
    }//end class
}//end namespace
