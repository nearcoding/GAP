using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GAP.BL;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ninject;
using System.Collections.Generic;
using System.Threading;
using GAP.BL.HelperClasses;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Abt.Controls.SciChart.Numerics;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class LithologyManager
    {
        private LithologyManager()
        {
        }
        public void AddLithologiesToTrack(TrackToShow track)
        {
            var renderableSeries = track.CurveRenderableSeries.Where(u => u.GetType() == typeof(FastLineRenderableSeries)).Select(v => v as FastLineRenderableSeries);

            FastLineRenderableSeries actualLithologySeries = null;
            actualLithologySeries = renderableSeries.SingleOrDefault(u => u.Name == "Lithology" && u.XAxisId == "Lithology");

            if (actualLithologySeries == null)
            {
                LithologyManager.Instance.AddLithologyAxisInChart(track);
                actualLithologySeries = renderableSeries.SingleOrDefault(u => u.Name == "Lithology" && u.XAxisId == "Lithology");
            }
            var lst = track.TrackObject.Lithologies;

            actualLithologySeries.IsVisible = true;
            var grouped = lst.GroupBy(u => u.LithologyName).ToList();
            var seriesCollection = new ObservableCollection<FastLineRenderableSeries>();

            var trackObject = track.TrackObject;
            var curveToShowObject = track.Curves.SingleOrDefault(u => u.RefChart == trackObject.RefChart && u.RefTrack == trackObject.ID && u.RefDataset == "Lithology");

            foreach (var obj in grouped)
            {
                var dataSeries = new XyDataSeries<double, double>();
                var fullDataSeries = new XyDataSeries<double, double>();

                string imageName = obj.Key;

                FastLineRenderableSeries normalLithologySeries = GetNormalLithologySeries(dataSeries, imageName);
                FastLineRenderableSeries fullLithologySeries = GetFullLithologySeries(fullDataSeries, imageName, track);
                ApplyBindingToNormalLithology(actualLithologySeries, normalLithologySeries);
                ApplyBindingToFullLithology(actualLithologySeries, fullLithologySeries);

                seriesCollection.Add(normalLithologySeries);
                seriesCollection.Add(fullLithologySeries);

                foreach (var item in obj)
                {
                    decimal initialDepth = item.InitialDepth;
                    decimal finalDepth = item.FinalDepth;
                    for (decimal i = initialDepth; i <= finalDepth; i++)
                    {
                        dataSeries.Append(9.5, double.Parse(i.ToString()));
                        fullDataSeries.Append(5, double.Parse(i.ToString()));
                    }
                }
                if (curveToShowObject != null)
                    actualLithologySeries.IsVisible = curveToShowObject.IsSeriesVisible;
            }
            foreach (var series in seriesCollection)
                track.CurveRenderableSeries.Add(series);
        }

        private static FastLineRenderableSeries GetNormalLithologySeries(XyDataSeries<double, double> dataSeries, string imageName)
        {
            return new FastLineRenderableSeries
            {
                Tag = imageName,
                DataSeries = dataSeries,
                XAxisId = "Lithology",
                ResamplingMode = ResamplingMode.None,
                PointMarker = new LithologyMarker(imageName)
                {
                    Width = 50
                },
            };
        }
        private static FastLineRenderableSeries GetFullLithologySeries(XyDataSeries<double, double> dataSeries, string imageName, TrackToShow track)
        {
            var marker = new LithologyMarker(imageName)
            {
                Fill = Colors.Red,
                Stroke = Colors.Green
            };
            FastLineRenderableSeries series = new FastLineRenderableSeries
            {
                Tag = imageName,
                DataSeries = dataSeries,
                XAxisId = "Lithology",
                ResamplingMode = ResamplingMode.None,
                PointMarker = marker
            };
            BindingOperations.SetBinding(marker, LithologyMarker.WidthProperty, new Binding("Width")
            {
                Source = track.TrackObject
            });
            return series;
        }
        private static void ApplyBindingToFullLithology(FastLineRenderableSeries actualLithologySeries, FastLineRenderableSeries series)
        {
            var multiBinding = new MultiBinding();
            multiBinding.Converter = new FullLithologyVisibilityConverter();
            multiBinding.Bindings.Add(new Binding("IsVisible")
            {
                Source = actualLithologySeries,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            multiBinding.Bindings.Add(new Binding("IsFullLithology")
            {
                Source = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            series.SetBinding(FastLineRenderableSeries.IsVisibleProperty, multiBinding);
        }

        private static void ApplyBindingToNormalLithology(FastLineRenderableSeries actualLithologySeries, FastLineRenderableSeries series)
        {
            var multiBinding = new MultiBinding();
            multiBinding.Converter = new LithologyVisibilityConverter();
            multiBinding.Bindings.Add(new Binding("IsVisible")
            {
                Source = actualLithologySeries,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            multiBinding.Bindings.Add(new Binding("IsLithologyVisible")
            {
                Source = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            series.SetBinding(FastLineRenderableSeries.IsVisibleProperty, multiBinding);
        }


        static LithologyManager _instance = new LithologyManager();

        public static LithologyManager Instance
        {
            get { return _instance; }
        }

        public void AddLithologyAxisInChart(TrackToShow trackToShowObject, IEnumerable<Chart> charts = null)
        {
            TrackManager.Instance.AddLithologyCurveToTrack(trackToShowObject, charts);
            GlobalDataModel.Instance.AddNamedAxisInChart(trackToShowObject, "Lithology");

            if (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.IsLithologyVisible || IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.IsFullLithology)
                if (trackToShowObject.TrackObject.Lithologies.Any()) trackToShowObject.HasCurves = trackToShowObject.Curves.Any();
        }

        public void RemoveLithologyObject(LithologyInfo lithology)
        {
            var trackToShow = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Single(u => u.ChartObject.ID == lithology.RefChart).Tracks.Single(u => u.TrackObject.ID == lithology.RefTrack);
            if (trackToShow == null) return;
            if (trackToShow.TrackObject == null) throw new Exception("Track object is null against the track to show object while removing lithologies");

            trackToShow.Curves[0].Visibility =
                trackToShow.Annotations.Any(u => u.XAxisId == "Lithology") ? Visibility.Visible : Visibility.Collapsed;

            RemoveDataSeriesForLithology(trackToShow, lithology);
            trackToShow.TrackObject.Lithologies.Remove(lithology);
            GlobalDataModel.Instance.CheckForHasCurves(trackToShow);

            IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SyncZoom();
        }

        private void RemoveDataSeriesForLithology(TrackToShow trackToShow, LithologyInfo lithology)
        {
            //all lithology series on the track
            var renderableSeries = trackToShow.CurveRenderableSeries.Where(u => u.XAxisId == "Lithology" && u.GetType() == typeof(FastLineRenderableSeries)).Select(v => v as FastLineRenderableSeries);
            renderableSeries = renderableSeries.Where(u => u.Tag != null && u.Tag.ToString() == lithology.LithologyName);
            //var renderableSeries = trackToShow.CurveRenderableSeries.SingleOrDefault(u => (u as FastLineRenderableSeries).Name == "Lithology");
            if (renderableSeries != null)
            {
                //there should be two series with this information one is normal and other is full lithology
                foreach (var series in renderableSeries)
                {
                    //get all lithologies object from the track which are lower  than current object's initial depth
                    var lithologies = trackToShow.TrackObject.Lithologies.Where(u => u.LithologyName == lithology.LithologyName && u.InitialDepth < lithology.InitialDepth);
                    int indexToSkip = 0;
                    foreach (var obj in lithologies)
                    {
                        indexToSkip = indexToSkip + 1 + Int32.Parse((obj.FinalDepth - obj.InitialDepth).ToString());
                    }
                    var dataSeries = series.DataSeries as XyDataSeries<double, double>;
                    int count = Int32.Parse(((lithology.FinalDepth + 1) - lithology.InitialDepth).ToString());
                    dataSeries.RemoveRange(indexToSkip, count);
                }
            }
        }

        public void AddLithologyObject(LithologyInfo lithology, ChartToShow chartToShow = null, IEnumerable<Chart> charts = null)
        {
            if (chartToShow == null)
                chartToShow = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.SingleOrDefault(u => u.ChartObject.ID == lithology.RefChart);

            if (chartToShow == null) return;

            var trackToShowObject = chartToShow.Tracks.SingleOrDefault(u => u.TrackObject.ID == lithology.RefTrack);
            if (trackToShowObject == null) return;

            if (trackToShowObject.TrackObject == null)
                throw new Exception("Track not found against track to show object while adding lithology");
            //while loading project track object would already have lithologies in it, so we dont have to re-add it in the object
            if (!trackToShowObject.TrackObject.Lithologies.Any(u => u.ID == lithology.ID))
                trackToShowObject.TrackObject.Lithologies.Add(lithology);
            FastLineRenderableSeries actualLithologySeries = null;

            //track to show object has renderable series  for lithologies
            //get the proper renderable series and add an annotation to it
            if (!trackToShowObject.CurveRenderableSeries.Any(u => (u as FastLineRenderableSeries).Name == "Lithology"))
            {
                AddLithologyAxisInChart(trackToShowObject, charts);
            }
            var normalDataSeries = new XyDataSeries<double, double>();
            var fullDataSeries = new XyDataSeries<double, double>();

            var renderableSeries = trackToShowObject.CurveRenderableSeries.Where(u => u.GetType() == typeof(FastLineRenderableSeries)).Select(v => v as FastLineRenderableSeries);
            actualLithologySeries = renderableSeries.SingleOrDefault(u => u.Name == "Lithology" && u.XAxisId == "Lithology");

            FastLineRenderableSeries normalLithologySeries = null;
            FastLineRenderableSeries fullLithologySeries = null;
            var subSeries = renderableSeries.Where(u => u.Tag != null && u.Tag.ToString() == lithology.LithologyName);
            if (!subSeries.Any())
            {
                normalLithologySeries = GetNormalLithologySeries(normalDataSeries, lithology.LithologyName);
                fullLithologySeries = GetFullLithologySeries(fullDataSeries, lithology.LithologyName, trackToShowObject);
                trackToShowObject.CurveRenderableSeries.Add(normalLithologySeries);
                trackToShowObject.CurveRenderableSeries.Add(fullLithologySeries);
                ApplyBindingToNormalLithology(actualLithologySeries, normalLithologySeries);
                ApplyBindingToFullLithology(actualLithologySeries, fullLithologySeries);

            }
            else
            {
                foreach (var series in subSeries)
                {
                    if (series.PointMarker.Width == 50)
                    {
                        normalLithologySeries = series;
                        normalDataSeries = normalLithologySeries.DataSeries as XyDataSeries<double, double>;
                    }
                    else
                    {
                        fullLithologySeries = series;
                        fullDataSeries = fullLithologySeries.DataSeries as XyDataSeries<double, double>;
                    }
                }
            }

            var startingPoint = int.Parse(lithology.InitialDepth.ToString());
            var endingPoint = int.Parse(lithology.FinalDepth.ToString());

            for (decimal i = startingPoint; i <= endingPoint; i++)
            {
                normalDataSeries.Append(9.5, double.Parse(i.ToString()));
                fullDataSeries.Append(5, double.Parse(i.ToString()));
            }
        }
    }//end class
}//end namespace
