using Abt.Controls.SciChart.Visuals.RenderableSeries;
using System.Linq;
using System.Windows;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class DoubleLineRenderableSeries : FastLineRenderableSeries
    {
        protected override void InternalDraw(Abt.Controls.SciChart.Rendering.Common.IRenderContext2D renderContext, IRenderPassData renderPassData)
        {
            var pointSeries = CurrentRenderPassData.PointSeries;
            var shiftedLine = new LinesEnumerable(pointSeries, renderPassData.XCoordinateCalculator,
                    renderPassData.YCoordinateCalculator, false).Select(u => new Point(u.X, u.Y - 5));

            //base.InternalDraw(renderContext, renderPassData);
            using (var segmentPen = renderContext.CreatePen(SeriesColor, AntiAliasing, StrokeThickness, Opacity, null))
            {
                renderContext.DrawLines(segmentPen, shiftedLine);
            }
            base.InternalDraw(renderContext, renderPassData);
        }
    }//end class
}//end namespace
