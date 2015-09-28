using Abt.Controls.SciChart.Rendering.Common;
using Abt.Controls.SciChart.Visuals.PointMarkers;
using System.Collections.Generic;
using System.Windows;

namespace GAP.BL.HelperClasses
{
    public class DiamondMarker: BasePointMarker
    {
        protected override void DrawInternal(IRenderContext2D context, double x, double y, IPen2D pen, IBrush2D brush)
        {
            List<Point> center = new List<Point>();
            center.Add(new Point(x, y));
            DrawInternal(context, center, pen, brush);
        }

        protected override void DrawInternal(IRenderContext2D context, IEnumerable<Point> centers, IPen2D pen, IBrush2D brush)
        {

            float width2 = (float)(Width * 0.5);
            float height2 = (float)(Height * 0.5);

            foreach (var center in centers)
            {
                double top = center.Y - height2;
                double bottom = center.Y + height2;
                double left = center.X - width2;
                double right = center.X + width2;

                var diamondPoints = new[]
                    {
                        // Points drawn like this:
                        // 
                        //      x0      (x4 in same location as x0)
                        // 
                        // x3        x1
                        //
                        //      x2

                        new Point(center.X, top),       // x0
                        new Point(right, center.Y),     // x1
                        new Point(center.X, bottom),    // x2
                        new Point(left, center.Y),      // x3
                        new Point(center.X, top),       // x4 == x0
                    };
                context.FillPolygon(brush, diamondPoints);
                context.DrawLines(pen, diamondPoints);
            }
        }
    }//end class
}//end namespace
