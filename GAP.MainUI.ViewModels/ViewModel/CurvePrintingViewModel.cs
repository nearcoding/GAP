using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Ninject;
using AutoMapper;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using GAP.BL;
using Abt.Controls.SciChart.Visuals.Annotations;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class CurvePrintingViewModel : BaseViewModel<BaseEntity>
    {
        ICommand _printerSettingsCommand, _printCommand, _printPreviewCommand;

        public CurvePrintingViewModel(string token)
            : base(token)
        {
            GlobalDataModel.DeleteFilesInTempFolder();
            Charts = new List<Chart>();
            Title = IoC.Kernel.Get<IResourceHelper>().ReadResource("PrintCurve");
            var charts = GlobalCollection.Instance.Charts.ToList();
            Mapper.CreateMap(typeof(Curve), typeof(Curve));

            InitializeChartList(charts);

            var allTracks = Charts.Where(u => u.Tracks != null).SelectMany(v => v.Tracks);

            foreach (var track in allTracks)
            {
                track.Curves = new ExtendedBindingList<Curve>(track.Curves.ToList());
            }

            foreach (var chart in Charts)
            {
                chart.OnEntitySelectionChanged += chart_OnEntitySelectionChanged;
                foreach (var track in chart.Tracks)
                {
                    track.OnEntitySelectionChanged += track_OnEntitySelectionChanged;
                    foreach (var curve in track.Curves)
                    {
                        curve.OnEntitySelectionChanged += curve_OnEntitySelectionChanged;
                    }
                }
            }
        }

        private void InitializeChartList(List<Chart> charts)
        {
            foreach (var chart in charts)
            {
                var chartObj = new Chart
                {
                    Name = chart.Name,
                    ID = chart.ID,
                    DisplayIndex = chart.DisplayIndex
                };
                AddTracksToChart(chart, chartObj);
                Charts.Add(chartObj);
            }
        }

        private static void AddTracksToChart(Chart chart, Chart chartObj)
        {
            foreach (var track in chart.Tracks)
            {
                var trackObject = new Track
                {
                    Name = track.Name,
                    DisplayIndex = track.DisplayIndex,
                    RefChart = chart.ID,
                    ID = track.ID
                };
                AddLithologyToTrack(track, trackObject);
                AddCurvesToTrack(track, trackObject);
                chartObj.Tracks.Add(trackObject);
            }
        }

        private static void AddLithologyToTrack(Track track, Track trackObj)
        {
            foreach (var obj in track.Lithologies)
            {
                var lithology = new LithologyInfo
                {
                    Name = obj.Name,
                    InitialDepth = obj.InitialDepth,
                    FinalDepth = obj.FinalDepth,
                    LithologyName = obj.LithologyName,
                    RefChart = obj.RefChart,
                    RefTrack = obj.RefTrack,
                    ImageFile = obj.ImageFile
                };
                trackObj.Lithologies.Add(lithology);
            }
        }

        private static void AddCurvesToTrack(Track track, Track trackObject)
        {
            foreach (var curve in track.Curves)
            {
                var curveObject = (Curve)Mapper.Map(curve, typeof(Curve), typeof(Curve));
                trackObject.Curves.Add(curveObject);
            }
        }

        public ICommand PrinterSettingsCommand
        {
            get { return _printerSettingsCommand ?? (_printerSettingsCommand = new RelayCommand(PrinterSettings)); }
        }
        private void PrinterSettings()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.PrinterSettings);
        }
        void curve_OnEntitySelectionChanged(BaseEntity entity)
        {
            var curve = entity as Curve;
            var track = Charts.SingleOrDefault(u => u.ID == curve.RefChart).Tracks.SingleOrDefault(v => v.ID == curve.RefTrack);

            track.OnEntitySelectionChanged -= track_OnEntitySelectionChanged;
            var curves = track.Curves;
            if (curves.All(u => u.IsEntitySelected.HasValue))
            {
                if (curves.All(u => u.IsEntitySelected != null && u.IsEntitySelected.Value))
                    track.IsEntitySelected = true;
                else if (curves.All(u => u.IsEntitySelected != null && u.IsEntitySelected.Value == false))
                    track.IsEntitySelected = false;
                else
                    track.IsEntitySelected = null;
            }
            else
                track.IsEntitySelected = null;

            track.OnEntitySelectionChanged += track_OnEntitySelectionChanged;
            UpdateChartFromTrack(track);
        }

        private void UpdateChartFromTrack(Track track)
        {
            var chart = Charts.SingleOrDefault(u => u.ID == track.RefChart);
            chart.OnEntitySelectionChanged -= chart_OnEntitySelectionChanged;
            if (track.IsEntitySelected.HasValue)
            {
                if (chart.Tracks.All(u => u.IsEntitySelected != null && u.IsEntitySelected.Value))
                    chart.IsEntitySelected = true;
                else if (chart.Tracks.All(u => u.IsEntitySelected != null && u.IsEntitySelected.Value == false))
                    chart.IsEntitySelected = false;
                else
                    chart.IsEntitySelected = null;
            }
            else
                chart.IsEntitySelected = null;
            chart.OnEntitySelectionChanged += chart_OnEntitySelectionChanged;
        }

        void track_OnEntitySelectionChanged(BaseEntity entity)
        {
            var track = entity as Track;
            var chart = Charts.SingleOrDefault(u => u.ID == track.RefChart);// HelperMethods.Instance.GetChartByChartName(track.RefChart);
            var curves = track.Curves;//.Where(u => u.RefProject.ToLower() != "lithology");

            foreach (var curve in curves)
            {
                curve.OnEntitySelectionChanged -= curve_OnEntitySelectionChanged;
                curve.IsEntitySelected = track.IsEntitySelected;
                curve.OnEntitySelectionChanged += curve_OnEntitySelectionChanged;
            }

            chart.OnEntitySelectionChanged -= chart_OnEntitySelectionChanged;
            if (chart.Tracks.All(u => u.IsEntitySelected.HasValue))
            {
                if (chart.Tracks.All(u => u.IsEntitySelected != null && u.IsEntitySelected.Value))
                    chart.IsEntitySelected = true;
                else if (chart.Tracks.All(u => u.IsEntitySelected != null && u.IsEntitySelected.Value == false))
                    chart.IsEntitySelected = false;
                else
                    chart.IsEntitySelected = null;
            }
            else
                chart.IsEntitySelected = null;
            chart.OnEntitySelectionChanged += chart_OnEntitySelectionChanged;
        }

        void chart_OnEntitySelectionChanged(BaseEntity entity)
        {
            foreach (var track in (entity as Chart).Tracks)
            {
                track.OnEntitySelectionChanged -= track_OnEntitySelectionChanged;
                track.IsEntitySelected = entity.IsEntitySelected;
                UpdateCurveFromTrack(track);
                track.OnEntitySelectionChanged += track_OnEntitySelectionChanged;
            }
        }
        private void UpdateCurveFromTrack(Track track)
        {
            if (track.IsEntitySelected.HasValue)
            {
                var curves = track.Curves.Where(u => u.RefProject.ToLower() != "lithology");
                foreach (var curve in curves)
                {
                    curve.OnEntitySelectionChanged -= curve_OnEntitySelectionChanged;
                    curve.IsEntitySelected = track.IsEntitySelected.Value;
                    curve.OnEntitySelectionChanged += curve_OnEntitySelectionChanged;
                }
            }
        }
        public bool PrintLithology { get; set; }
        public bool PrintFormationTop { get; set; }
        public bool PrintDataset { get; set; }
        public List<Chart> Charts { get; set; }
        public ICommand PrintCommand
        {
            get { return _printCommand ?? (_printCommand = new RelayCommand(() => FillChart())); }
        }

        public ICommand PrintPreviewCommand
        {
            get { return _printPreviewCommand ?? (_printPreviewCommand = new RelayCommand(() => FillChart(true))); }
        }
        private void FillChart(bool printPreview = false)
        {
            _lstRemovedCurveObjects.Clear();
            GlobalDataModel.Instance.ChartToShowObjects.Clear();

            foreach (var chart in Charts.Where(u => u.IsEntitySelected == null || u.IsEntitySelected.Value))
                ProcessChartPrinting(chart, printPreview);

            foreach (var item in _lstRemovedCurveObjects)
                CurveManager.Instance.AddCurveObject(item, Charts);
        }

        private void ProcessChartPrinting(Chart chart, bool printPreview)
        {
            if (!GlobalDataModel.Instance.ChartToShowObjects.Any(u => u.ChartObject.ID == chart.ID))
                ChartManager.Instance.AddChartObject(chart, Charts);

            var chartToShow = GlobalDataModel.Instance.ChartToShowObjects.SingleOrDefault(u => u.ChartObject.ID == chart.ID);
            foreach (var track in chart.Tracks.Where(u => u.IsEntitySelected == null || u.IsEntitySelected.Value))
            {
                var trackToShow = ProcessTrackPrinting(chartToShow, track);

                IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token,
                    printPreview ? NotificationMessageEnum.PrintPreviewChart : NotificationMessageEnum.PrintChart, trackToShow);
            }
        }

        private TrackToShow ProcessTrackPrinting(ChartToShow chartToShow, Track track)
        {
            var actualTrackObject = HelperMethods.Instance.GetTrackByID(track.ID);
            GetTrackToShowObject(chartToShow, track);

            var trackToShow = RemoveUnselectedCurvesFromTrack(chartToShow, track);
            IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.CalculateMinMaxVisibleRangeLimitForYAxis(trackToShow.CurveRenderableSeries);
            RemoveDefaultAnnotationsFromReport(trackToShow);
            AddFormationAnnotation(trackToShow);
            return trackToShow;
        }

        private void AddFormationAnnotation(TrackToShow trackToShow)
        {
            if (!PrintFormationTop) return;
            var actualChartObject = HelperMethods.Instance.GetChartByID(trackToShow.TrackObject.RefChart);
            foreach (var formation in actualChartObject.Formations)
                FormationManager.Instance.AddFormationInfoInTrackToShowObject(formation, trackToShow);
        }



        private void GetTrackToShowObject(ChartToShow chartToShow, Track track)
        {
            if (!chartToShow.Tracks.Any(u => u.TrackObject.ID == track.ID))
                TrackManager.Instance.AddTrackObject(track, Charts);
        }

        private TrackToShow RemoveUnselectedCurvesFromTrack(ChartToShow chartToShow, Track track)
        {
            var curves = track.Curves.Where(u => u.IsEntitySelected == null || u.IsEntitySelected.Value);
            var trackToShow = chartToShow.Tracks.SingleOrDefault(u => u.TrackObject.ID == track.ID);

            foreach (var curve in trackToShow.Curves.ToList())
            {
                if (!curve.CurveObject.IsEntitySelected.Value)
                {
                    CurveManager.Instance.RemoveCurveObject(curve.CurveObject, Charts);
                    _lstRemovedCurveObjects.Add(curve.CurveObject);
                }
            }

            var lithologyCurve = track.Curves.SingleOrDefault(u => u.RefDataset == "Lithology");
            if (lithologyCurve != null)
            {
                if (lithologyCurve.IsEntitySelected == null || !lithologyCurve.IsEntitySelected.Value)
                {
                    track.Curves.Remove(lithologyCurve);
                    foreach (var annotation in trackToShow.Annotations.Where(u => u.XAxisId == "Lithology").ToList())
                    {
                        trackToShow.Annotations.Remove(annotation);
                    }
                }
            }
            return trackToShow;
        }

        List<Curve> _lstRemovedCurveObjects = new List<Curve>();
        private static void RemoveDefaultAnnotationsFromReport(TrackToShow trackToShow)
        {
            var sliderAnnotation = trackToShow.Annotations.Where(u => u.GetType() == typeof(CustomAnnotation))
                .SingleOrDefault(v => (v as CustomAnnotation).Name == "SliderAnnotation");
            if (sliderAnnotation != null) trackToShow.Annotations.Remove(sliderAnnotation);

            var saveAnnotation = trackToShow.Annotations.Where(u => u.GetType() == typeof(CustomAnnotation))
                .SingleOrDefault(v => (v as CustomAnnotation).Name == "SaveAnnotation");
            if (saveAnnotation != null) trackToShow.Annotations.Remove(saveAnnotation);
        }
    }//end class
}//end namespace
