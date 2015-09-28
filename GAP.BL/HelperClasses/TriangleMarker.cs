using Abt.Controls.SciChart.Rendering.Common;
using Abt.Controls.SciChart.Visuals.PointMarkers;
using System.Collections.Generic;
using System.Windows;

namespace GAP.BL.HelperClasses
{
    public class TriangleMarker : BasePointMarker
    {
        protected override void DrawInternal(IRenderContext2D context, double x, double y, IPen2D pen, IBrush2D brush)
        {
            List<Point> points = new List<Point>();
            points.Add(new Point
            {
                X = x,
                Y = y
            });
            DrawInternal(context, points, pen, brush);
        }

        protected override void DrawInternal(IRenderContext2D context, IEnumerable<Point> centers, IPen2D pen, IBrush2D brush)
        {
            float widthHalf = (float)Width / 2;
            float heightHalf = (float)Height / 2;
            foreach (var point in centers)
            {
                double left = point.X - widthHalf;
                double right = point.X + widthHalf;
                double top = point.Y - heightHalf;
                double bottom = point.Y + heightHalf;
                var trianglePoints = new[]
                {
                    //draw points like this
                    //      x0
                    //
                    //
                    //x1            x2
                new Point(point.X,top),   //x0
                new Point(left,bottom),  //x1
                new Point(right,bottom) ,   //x2
                new Point(point.X,top)
                };
                context.FillPolygon(brush, trianglePoints);
                context.DrawLines(pen, trianglePoints);
            }
        }
    }//end class
}//end namespace
