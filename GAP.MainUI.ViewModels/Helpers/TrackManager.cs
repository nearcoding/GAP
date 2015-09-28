using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals;
using GalaSoft.MvvmLight.Command;
using Ninject;
using GAP.BL;
using GAP.CustomControls;
using GAP.Helpers;
using System.Collections.Generic;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class TrackManager : SubBaseEntity
    {
        private TrackManager() { }

        static TrackManager _instance = new TrackManager();
        public static TrackManager Instance { get { return _instance; } }

        public void AddTrackObject(Track track, IEnumerable<Chart> charts = null)
        {
            var chart = HelperMethods.Instance.GetChartByID(track.RefChart, charts);
            if (chart == null) throw new Exception("Chart not found where track is supposed to insert");

            var trackObject = HelperMethods.Instance.GetTrackByID(track.ID, charts);
            if (trackObject == null)//in case user tries to add same object twice
            {
                track.DisplayIndex = chart.Tracks.Any() ? chart.Tracks.Max(u => u.DisplayIndex) + 1 : 0;
                chart.Tracks.Add(track);
            }
            //only when the track object is being added in selected chart then we have to do this hard work
            //if we pass the custom list then we might need the  object
            if (charts != null || IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject.ID == track.RefChart)
                AddTrackToShowObject(track, charts);
        }

        private void AddTrackToShowObject(Track track, IEnumerable<Chart> charts = null)
        {
            var chartToShow = ChartManager.Instance.GetChartToShowObjectByID(track.RefChart, charts);
            if (chartToShow == null) throw new Exception("Chart to show object not found in the collection");

            var trackToShow = GetTrackToShowById(track.ID, charts);
            if (trackToShow != null) return; //in case user tries to add  track to show object twice

            trackToShow = GetNewTrackToShowObject(track, chartToShow, charts);

            foreach (var curve in track.Curves.ToList())
                CurveManager.Instance.AddCurveObject(curve, charts);

            foreach (var formation in chartToShow.ChartObject.Formations)
                FormationManager.Instance.AddFormationInfoInTrackToShowObject(formation, trackToShow);
        }

        public void RemoveTrackObject(Track track)
        {
            var chart = HelperMethods.Instance.GetChartByID(track.RefChart);
            if (chart == null) throw new Exception("Chart not found against the removed track");
            RemoveTrackToShowObject(track);
            chart.Tracks.Remove(track);
        }

        private void RemoveTrackToShowObject(Track track)
        {
            var chartToShow = ChartManager.Instance.GetChartToShowObjectByID(track.RefChart);
            if (chartToShow == null) throw new Exception("Chart to show object not found");
            var trackToShow = chartToShow.Tracks.SingleOrDefault(u => u.TrackObject.ID == track.ID);
            //we are returning from this point as track to show object remains null for non selected charts
            if (trackToShow == null) return;// throw new Exception("Track to show object not found");
            chartToShow.Tracks.Remove(trackToShow); ;
        }

        public TrackToShow GetTrackToShowById(string trackId, IEnumerable<Chart> charts = null)
        {
            var track = HelperMethods.Instance.GetTrackByID(trackId);
            if (track == null) return null;
            var chartToShow = ChartManager.Instance.GetChartToShowObjectByID(track.RefChart, charts);
            if (chartToShow == null) return null;
            return chartToShow.Tracks.SingleOrDefault(u => u.TrackObject.ID == trackId);
        }

        private TrackToShow GetNewTrackToShowObject(Track track, ChartToShow chartToShow, IEnumerable<Chart> charts = null)
        {
            var trackToShow = new TrackToShow
            {
                TrackObject = track,
            };

            //this is important for lithology processing. they need this track to show and they are going to get it from chart to show object
            chartToShow.Tracks.Add(trackToShow);

            TooltipBindingToTrackObject(trackToShow);
            AddDefaultAnnotationsAndAxisToTrackToShow(trackToShow);

            if (track.Lithologies.Any())
                LithologyManager.Instance.AddLithologiesToTrack(trackToShow);

            //foreach (var lithology in track.Lithologies)
            //    LithologyManager.Instance.AddLithologyObject(lithology, chartToShow, charts);

            GlobalDataModel.Instance.CheckForHasCurves(trackToShow);

            if (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.IsSyncZoom)
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.BindVisibilityRangeOfTrackToShow(trackToShow);

            return trackToShow;
        }

        public void AddLithologyCurveToTrack(TrackToShow trackToShow, IEnumerable<Chart> charts = null)
        {
            Curve curve;
            Track track = trackToShow.TrackObject;
            curve = track.Curves.SingleOrDefault(u => u.RefProject == "Lithology" && u.RefWell == "Lithology" && u.RefDataset == "Lithology");
            if (curve == null)
            {
                curve = new Curve
                {
                    RefTrack = track.ID,
                    RefChart = track.RefChart,
                    RefDataset = "Lithology",
                    RefWell = "Lithology",
                    RefProject = "Lithology"
                };
                track.Curves.Add(curve);
            }

            CurveToShow curveToShow;
            curveToShow = CurveManager.Instance.GetCurveToShowById(curve.ID, charts, true);
            if (curveToShow != null) return;

            curveToShow = new CurveToShow(track.RefChart, track.ID, "Lithology")
            {
                MinValue = 0,
                MaxValue = 0,
                Visibility = (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.IsLithologyVisible ||
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.IsFullLithology) && track.Lithologies.Any() ?
                Visibility.Visible : Visibility.Collapsed,
                TrackToShowObject = trackToShow,
                CurveObject = curve,
                IsSeriesVisible = curve.IsSeriesVisible
            };
            trackToShow.Curves.Insert(0, curveToShow);
        }

        private static void TooltipBindingToTrackObject(TrackToShow trackToShow)
        {
            trackToShow.TooltipModifierObject.SetBinding(TooltipModifier.IsEnabledProperty, new Binding("IsTooltipVisible")
            {
                Source = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }

        private void AddDefaultAnnotationsAndAxisToTrackToShow(TrackToShow trackToShow)
        {
            trackToShow.XAxisCollection.Add(new NumericAxis
            {
                DrawLabels = false,
                DrawMajorBands = false,
                DrawMajorTicks = false,
                VisibleRange = new DoubleRange(0, 10),
                VisibleRangeLimit = new DoubleRange(0, 10)
            });

            AddSaveScreenshotAnnotationToTrack(trackToShow);
            AddSliderAnnotationToTrack(trackToShow);
        }

        public void RemoveAllAnnotationsBySubDataset(SubDataset subDataset)
        {
            var curvesToShow = GlobalDataModel.GetCurveToShowBySubdataset(subDataset);
            foreach (var curveToShow in curvesToShow)
            {
                var annotations = curveToShow.TrackToShowObject.Annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended)).Select(v => v as LineAnnotationExtended);
                annotations = annotations.Where(u => u.SubDataset != null && u.SubDataset.Name == subDataset.Name && u.SubDataset.IsNCT == subDataset.IsNCT);

                while (annotations.Any())
                    curveToShow.TrackToShowObject.Annotations.Remove(annotations.First());

                if (curveToShow.SubDatasets.Contains(subDataset))
                    curveToShow.SubDatasets.Remove(subDataset);
            }
        }

        ICommand _saveScreenShotCommand;
        public ICommand SaveScreenShotCommand
        {
            get { return _saveScreenShotCommand ?? (_saveScreenShotCommand = new RelayCommand<CustomAnnotation>(SaveScreenshot)); }
        }

        private void SaveScreenshot(CustomAnnotation customAnnotation)
        {
            var surface = customAnnotation.ParentSurface as SciChartSurface;
            GlobalDataModel.Instance.SendMessage(IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Token, NotificationMessageEnum.TakeScreenshot, surface);
        }

        private void AddSliderAnnotationToTrack(TrackToShow trackToShow)
        {
            var slider = new Slider
            {
                Width = 20,
                IsDirectionReversed = true,
                Height = 65,
                Maximum = 0.1,
                Minimum = -4,
                Orientation = Orientation.Vertical,
                Name = "Slider1"
            };
            var sliderAnnotation = new CustomAnnotation
            {
                X1 = 1,
                Y1 = 0.4,
                XAxisId = "DefaultAxisId",
                Visibility = Visibility.Visible,
                CoordinateMode = AnnotationCoordinateMode.Relative,
                IsEditable = true,
                Name = "SliderAnnotation",
                Margin = new Thickness(-35, 0, 0, 0)
            };

            Panel.SetZIndex(sliderAnnotation, 99);
            sliderAnnotation.Content = slider;

            slider.SetBinding(Slider.ValueProperty, new Binding("ZoomScaleFactor")
            {
                Source = trackToShow.ScaleModifier,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            sliderAnnotation.SetBinding(CustomAnnotation.IsHiddenProperty, new Binding("IsTrackControlsVisible")
            {
                Source = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Converter = new InverseBooleanConverter()
            });
            trackToShow.Annotations.Add(sliderAnnotation);
        }

        private void AddSaveScreenshotAnnotationToTrack(TrackToShow trackToShow)
        {
            var saveAnnotation = new CustomAnnotation
            {
                X1 = 1,
                Y1 = 0.9,
                XAxisId = "DefaultAxisId",
                Visibility = Visibility.Visible,
                CoordinateMode = AnnotationCoordinateMode.Relative,
                IsEditable = true,
                Name = "SaveAnnotation",
                Margin = new Thickness(-45, 0, 0, 0)
            };
            Panel.SetZIndex(saveAnnotation, 99);
            var bindingVisibility = new Binding("IsTrackControlsVisible")
            {
                Source = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Converter = new InverseBooleanConverter()
            };
            var button = new LinkButton
            {
                Width = 40,
                Height = 40
            };
            BindingOperations.SetBinding(button, LinkButton.CommandProperty, new Binding("SaveScreenShotCommand")
            {
                Source = this
            });
            button.CommandParameter = saveAnnotation;
            button.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Resources/Save.png", UriKind.Absolute)));

            saveAnnotation.Content = button;
            saveAnnotation.SetBinding(CustomAnnotation.IsHiddenProperty, bindingVisibility);
            trackToShow.Annotations.Add(saveAnnotation);
        }
    }//end class
}//end namespace
