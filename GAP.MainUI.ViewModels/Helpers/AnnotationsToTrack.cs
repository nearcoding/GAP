using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GAP.BL;
using GAP.MainUI.ViewModels.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class AnnotationsToTrack
    {
        //public void AddLithologyAnnotationsToTrack(TracksToShow track)
        //{
            //LithologyAnnotations = new AnnotationCollection();
            //foreach (var lithology in track.TrackObject.Lithologies)
            //{

            //    ImageBrush brush = new ImageBrush
            //    {
            //        ImageSource = new BitmapImage(new Uri(GlobalDataModel.LithologyImageFolder + "\\" + lithology.ImageFile)),
            //        TileMode = TileMode.Tile,
            //        ViewportUnits = BrushMappingMode.Absolute
            //    };
            //    brush.Viewport = new Rect(0, 0, brush.ImageSource.Width, brush.ImageSource.Height);

            //    LithologyAnnotations.Add(new BoxAnnotation
            //    {
            //        X1 = 0,
            //        X2 = 1,
            //        Y1 = lithology.InitialDepth,
            //        Y2 = lithology.FinalDepth,
            //        CoordinateMode = AnnotationCoordinateMode.RelativeX,
            //        Background = brush,
            //        Visibility = Visibility.Visible
            //    });
            //}
            //AddDefaultAxisForLithologyAnnotation(track);
        //}
        //private void AddDefaultAxisForLithologyAnnotation(TracksToShow trackToShow)
        //{
        //    var track = trackToShow.TrackObject;
        //    if (!track.Lithologies.Any()) return;
        //    //GlobalData.AddCustomChartLegendToSciChart(sciChart);
        //    var dataSeries = new XyDataSeries<double, double>();
        //    var initialDepths = track.Lithologies.Min(u => u.InitialDepth);
        //    var finalDepths = track.Lithologies.Max(u => u.FinalDepth);

        //    double initialDepthDouble = 0, finalDepthDouble = 0;

        //    Double.TryParse(initialDepths.ToString(), out initialDepthDouble);
        //    Double.TryParse(finalDepths.ToString(), out finalDepthDouble);

        //    foreach (LithologyInfo info in track.Lithologies)
        //    {
        //        dataSeries.Append(0, double.Parse(info.InitialDepth.ToString()));
        //        dataSeries.Append(0, double.Parse(info.FinalDepth.ToString()));
        //    }
        //    var series = new FastLineRenderableSeries
        //    {
        //        DataSeries = dataSeries,
        //        Tag = "Default Lithology Series"
        //    };

        //    var xAxis = new NumericAxis
        //    {
        //        DrawLabels = false,
        //        DrawMajorBands = false,
        //        DrawMajorGridLines = false,
        //        DrawMajorTicks = false
        //    };

        //    xAxis.Id = (trackToShow + "_" + "Lithology").ToString();
        //    series.XAxisId = xAxis.Id;

        //    trackToShow.XAxisCollection.Add(xAxis);
        //}
        //public AnnotationCollection LithologyAnnotations { get; set; }
    }//end class
}//end namespace
