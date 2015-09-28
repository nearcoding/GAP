using Abt.Controls.SciChart.ChartModifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GAP.ExtendedControls
{
    public class ZoomPanModifierExtension : ZoomPanModifier
    {
        public override void Pan(Point currentPoint, Point lastPoint, Point startPoint)
        {
            var visibleRange = base.YAxis.VisibleRange;
            var visibleRangeLimit = base.YAxis.VisibleRangeLimit;
            if (visibleRangeLimit == null || visibleRange == null)
                return;
            if (visibleRange.Min.CompareTo(visibleRangeLimit.Min) == 0 && visibleRange.Max.CompareTo(visibleRangeLimit.Max) == 0)
                return;

            if (currentPoint.Y < startPoint.Y && !visibleRange.IsValueWithinRange(visibleRangeLimit.Max))
                base.Pan(currentPoint, lastPoint, startPoint);

            if (currentPoint.Y > startPoint.Y && !visibleRange.IsValueWithinRange(visibleRangeLimit.Min))
                base.Pan(currentPoint, lastPoint, startPoint);

        }
    }//end class
}//end namespace
