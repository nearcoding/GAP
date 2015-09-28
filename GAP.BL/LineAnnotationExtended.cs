using Abt.Controls.SciChart.Visuals.Annotations;
using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace GAP.BL
{
    public class LineAnnotationExtended : LineAnnotation
    {
        public LineAnnotationExtended()
        {
           // Id = Guid.NewGuid().ToString();
        }
        //this property will be used to remove the dataseries when associated annotation has been removed
        //currently being used in formation addition and removal, if dataseries has been removed then 
        //visibility range will be updated as well
        public int Index { get; set; }
        public CurveToShow CurveToShow { get; set; }

        public SubDataset SubDataset { get; set; }

        public string Id { get; set; }
        public bool IsValidSlope { get; set; }

        public bool EditLineSegment { get; set; }

        public decimal ActualSlope { get; set; }
        public decimal CurrentSlope { get; set; }
        protected override void AddAdorners(Canvas adornerLayer)
        {
            base.AddAdorners(adornerLayer);

            if (adornerLayer == null) return;
            var adorners = GetUsedAdorners<IAnnotationResizeAdorner>(adornerLayer);
            foreach (var adorner in adorners)
            {
                foreach (var thumb in adorner.AdornerMarkers)
                {
                    thumb.DragCompleted += ThumbOnDragCompleted;
                }
            }
        }

        private void ThumbOnDragCompleted(object sender, DragCompletedEventArgs dragCompletedEventArgs)
        {
            LineDraggingEnded(this);
            // code with calculations
            if (EditLineSegment && !IsValidSlope)
            {
                EditLineSegment = false;
                if (SlopeNotValid != null) SlopeNotValid(this);
            }
        }

        public event InvalidSlopeDelegate LineDraggingEnded;

        public delegate void InvalidSlopeDelegate(LineAnnotationExtended sender);
        public event InvalidSlopeDelegate SlopeNotValid;
        protected override void RemoveAdorners(Canvas adornerLayer)
        {
            var adorners = GetUsedAdorners<IAnnotationResizeAdorner>(adornerLayer);
            foreach (var adorner in adorners)
            {
                foreach (var thumb in adorner.AdornerMarkers)
                {
                    thumb.DragCompleted -= ThumbOnDragCompleted;
                }
            }

            base.RemoveAdorners(adornerLayer);
        }
    }//end class
}//end namespace
