using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GAP.BL;
using System; 
using System.Linq;
using System.Windows.Data;
using Ninject;
using GAP.BL.Converters;
using System.Collections.Generic;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class CurveManager
    {
        private CurveManager() { }

        static CurveManager _instance = new CurveManager();
        public static CurveManager Instance { get { return _instance; } }

        public void AddCurveObject(Curve curve, IEnumerable<Chart> charts = null)
        {
            var track = HelperMethods.Instance.GetTrackByID(curve.RefTrack, charts);
            if (track == null) throw new Exception("Track not found where curve is supposed to insert");

            var curveObject = HelperMethods.Instance.GetCurveById(curve.ID, charts);
            if (curveObject == null)
            {
                curveObject = HelperMethods.Instance.GetAllCurves(charts, true).SingleOrDefault(u => u.RefChart == curve.RefChart && u.RefTrack == curve.RefTrack && u.RefDataset == curve.RefDataset);
                if (curveObject == null)
                    track.Curves.Add(curve);
            }
            if (charts != null || IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject.ID == curve.RefChart)
                AddCurveToShowObject(curve, charts);
        }

        private void AddCurveToShowObject(Curve curve, IEnumerable<Chart> charts = null)
        {
            var trackToShow = TrackManager.Instance.GetTrackToShowById(curve.RefTrack, charts);
            //we are returning at this point as there is a strong possibility that the removed curve is from another 
            //chart which does not track to show objects with it as they only belongs to selected  chart
            if (trackToShow == null) return;// throw new Exception("Track to show object not found");

            var curveToShowByID = GetCurveToShowById(curve.ID, charts);
            if (curveToShowByID != null) return;
            AddCurveToShowToTrackToShow(trackToShow, curve, charts);
        }

        public void RemoveCurveObject(Curve curve, IEnumerable<Chart> charts=null)
        {
            var trackToShow = TrackManager.Instance.GetTrackToShowById(curve.RefTrack, charts);
            //we are returning at this point as there is a strong possibility that the removed curve is from another 
            //chart which does not track to show objects with it as they only belongs to selected  chart
            if (trackToShow == null)
            {
                var track = HelperMethods.Instance.GetTrackByID(curve.RefTrack, charts);
                if (track == null) throw new Exception("Track object is null");
                track.Curves.Remove(curve);
                return;
            }
            if (trackToShow.TrackObject == null) throw new Exception("Track object is null");

            var curveToShow = trackToShow.Curves.Where(u => u.CurveObject != null).Single(u => u.CurveObject.ID == curve.ID);
            trackToShow.Curves.Remove(curveToShow);

            var seriesToRemove = trackToShow.CurveRenderableSeries.SingleOrDefault(u => u.XAxisId == curve.ID);
            if (seriesToRemove != null)
            {
                var fastLineSeries = seriesToRemove as FastLineRenderableSeries;

                trackToShow.CurveRenderableSeries.Remove(seriesToRemove);
                trackToShow.XAxisCollection.Remove(trackToShow.XAxisCollection.Single(u => u.Id == curve.ID));
                trackToShow.TrackObject.Curves.Remove(curve);
                RemoveSubAnnotationsByCurve(trackToShow, curveToShow.CurveObject);
                GlobalDataModel.Instance.CheckForHasCurves(trackToShow);
            }
            if (curve.RefChart == IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject.ID)
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.CalculateMinMaxVisibleRangeLimitForYAxis();
        }
         
        public CurveToShow GetCurveToShowById(string curveId, IEnumerable<Chart> charts = null, bool includeLithologyCurves = false)
        {
            var curve = HelperMethods.Instance.GetCurveById(curveId, charts, includeLithologyCurves);
            if (curve == null) return null;
            var trackToShow = TrackManager.Instance.GetTrackToShowById(curve.RefTrack, charts);
            //this will be very helpful as when the project is being loaded and it has some lithologies in it
            //then track to show object is not being added in chart collection until we added the lithology curve
            if (trackToShow == null) return null;
            return trackToShow.Curves.SingleOrDefault(u => u.CurveObject.ID == curveId);
        }

        private void RemoveSubAnnotationsByCurve(TrackToShow trackToShow, Curve curve)
        {
            var lineAnnotations = trackToShow.Annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended)).Select(v => v as LineAnnotationExtended);
            lineAnnotations = lineAnnotations.Where(u => u.CurveToShow != null && u.CurveToShow.CurveObject.ID == curve.ID);

            foreach (var annotation in lineAnnotations.ToList())
            {
                trackToShow.Annotations.Remove(annotation);
            }
        }

        public void AddCurveToShowToTrackToShow(TrackToShow trackToShow, Curve curve, IEnumerable<Chart> charts = null)
        {
            var dataset = HelperMethods.Instance.GetDatasetByID(curve.RefDataset);
            if (dataset == null) return;

            if (charts == null)
                AddCurveInformationInDataset(curve, dataset);

            var curveToShow = new CurveToShow(trackToShow.TrackObject.RefChart, trackToShow.TrackObject.ID, dataset.ID)
                {
                    CurveObject = curve,
                    TrackToShowObject = trackToShow,
                    IsSeriesVisible = curve.IsSeriesVisible
                };
            if (charts == null)
                AddExistingSubDatasetToTrack(trackToShow, dataset, curveToShow);

            GlobalDataModel.Instance.StylingOfScaleControl(dataset, curveToShow); 
            
            var xAxis = GetNewXAxisAndSetVisibleRangeLimit(dataset);

            xAxis.Id = curveToShow.CurveObject.ID;
            trackToShow.XAxisCollection.Add(xAxis);

            curveToShow.AnnotationModifier.XAxisId = xAxis.Id;

             var fastLineSeries = GlobalDataModel.Instance.AddDataseriesInformationToCurve(dataset, curveToShow);

            BindCurveProperties(fastLineSeries, curveToShow);

            trackToShow.HasCurves = trackToShow.CurveRenderableSeries.Any();

            IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.CalculateMinMaxVisibleRangeLimitForYAxis();

            trackToShow.Curves.Add(curveToShow);
            curveToShow.IsSeriesVisible = curve.IsSeriesVisible;
        }

        /// <summary>
        /// this information is helpful while getting information about curves while restoring a dataset in undo redo process
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="dataset"></param>
        private static void AddCurveInformationInDataset(Curve curve, Dataset dataset)
        {
            if (!dataset.Curves.Any(u => u.RefChart == curve.RefChart && u.RefTrack == curve.RefTrack))
            {
                dataset.Curves.Add(new DatasetCurveInfo
                {
                    RefChart = curve.RefChart,
                    RefTrack = curve.RefTrack
                });
            }
        }

        private void BindCurveProperties(FastLineRenderableSeries fastLineSeries, CurveToShow curveToShow)
        {
            var multiBindingOne = new MultiBinding();
            multiBindingOne.Bindings.Add(new Binding("DatasetObject.LineColor")
                {
                    Source = curveToShow,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
            multiBindingOne.Bindings.Add(new Binding("DatasetObject.LineStyle")
                 {
                     Source = curveToShow,
                     UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                 });
            multiBindingOne.Converter = new ColourToMultiColorConverter();
            fastLineSeries.SetBinding(FastLineRenderableSeries.SeriesColorProperty, multiBindingOne);
            fastLineSeries.SetBinding(FastLineRenderableSeries.XAxisIdProperty, new Binding("CurveObject.ID")
            {
                Source = curveToShow
            });

            var multiBinding = new MultiBinding();
            multiBinding.Bindings.Add(new Binding("DatasetObject.LineGrossor")
            {
                Source = curveToShow,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            multiBinding.Bindings.Add(new Binding("DatasetObject.LineStyle")
            {
                Source = curveToShow,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            multiBinding.Converter = new LineGrossorToDatasetStrokeThicknessConverter();
            fastLineSeries.SetBinding(FastLineRenderableSeries.StrokeThicknessProperty, multiBinding);

            fastLineSeries.SetBinding(FastLineRenderableSeries.StrokeDashArrayProperty, new Binding("DatasetObject.LineStyle")
            {
                Source = curveToShow,
                Converter = new LineStyleToStrokeDashArrayConverter()
            });

            BindMarkerProperties(fastLineSeries, curveToShow);
        }

        private static void BindMarkerProperties(FastLineRenderableSeries fastLineSeries, CurveToShow curveToShow)
        {
            var markerStyleBinding = new MultiBinding
            {
                Converter = new MarkerStyleToPointMarkerConverter()
            };

            markerStyleBinding.Bindings.Add(new Binding("DatasetObject.MarkerStyle")
            {
                Source = curveToShow
            });
            markerStyleBinding.Bindings.Add(new Binding("DatasetObject.MarkerSize")
            {
                Source = curveToShow
            });
            markerStyleBinding.Bindings.Add(new Binding("DatasetObject.ShouldApplyBorderColor")
            {
                Source = curveToShow
            });
            markerStyleBinding.Bindings.Add(new Binding("DatasetObject.MarkerColor")
            {
                Source = curveToShow
            });
            fastLineSeries.SetBinding(FastLineRenderableSeries.PointMarkerProperty, markerStyleBinding);
        }

        public void AddExistingSubDatasetToTrack(TrackToShow trackToShow, Dataset dataset, CurveToShow curveToShow)
        {
            if (dataset.SubDatasets.Any())
            {
                foreach (var subdataset in dataset.SubDatasets)
                {
                    curveToShow.SubDatasets.Add(subdataset);
                    foreach (var annotation in subdataset.Annotations)
                    {
                        var lineAnnotation = HelperMethods.Instance.GetLineAnnotationByAnnotationInfoAndSubDataset(annotation, subdataset, curveToShow);
                        curveToShow.LineAnnotationExtendedBinding(lineAnnotation);
                        lineAnnotation.Id = annotation.ID;
                        trackToShow.Annotations.Add(lineAnnotation);
                    }
                }
            }
        }

        private NumericAxis GetNewXAxisAndSetVisibleRangeLimit(Dataset dataset)
        {
            double minValueXAxis, maxValueXAxis;
            minValueXAxis = GlobalDataModel.GetValidMinUnitValue(dataset);
            maxValueXAxis = Convert.ToDouble(dataset.MaxUnitValue);

            var xAxis = new NumericAxis
            {
                DrawLabels = false,
                DrawMajorBands = false,
                DrawMajorGridLines = false,
                DrawMajorTicks = false
            };
            xAxis.VisibleRange = new DoubleRange(minValueXAxis, maxValueXAxis);
            xAxis.VisibleRangeLimit = new DoubleRange(minValueXAxis, maxValueXAxis);

            return xAxis;
        }
    }//end class
}//end namespace
