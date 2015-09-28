using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using Ninject;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using Abt.Controls.SciChart.Model.DataSeries;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class FormationManager
    {
        private FormationManager() { }

        static FormationManager _instance = new FormationManager();
        public static FormationManager Instance { get { return _instance; } }

        public void AddFormationObject(FormationInfo formation, ChartToShow chartToShow = null)
        {
            if (chartToShow == null)
                chartToShow = ChartManager.Instance.GetChartToShowObjectByID(formation.RefChart);
            if (chartToShow == null) throw new Exception("Chart to show object is null while adding formations");
            if (chartToShow.ChartObject == null) throw new Exception("Chart object is null while adding formations");

            if (!chartToShow.ChartObject.Formations.Any(u => u.ID == formation.ID))
                chartToShow.ChartObject.Formations.Add(formation);
            AddFormationToTrack(formation, chartToShow);
        }

        private void AddFormationToTrack(FormationInfo formation, ChartToShow chartToShowObject)
        {
            foreach (var trackToShowObject in chartToShowObject.Tracks)            
                AddFormationInfoInTrackToShowObject(formation, trackToShowObject);            
        }

        public void AddFormationInfoInTrackToShowObject(FormationInfo formation, TrackToShow trackToShowObject)
        {
            if (!trackToShowObject.CurveRenderableSeries.Any(u => (u as FastLineRenderableSeries).Name == "Formation"))
                AddFormationAxisInChart(trackToShowObject);

            if (trackToShowObject.FormationsList.Any(u => u.ID == formation.ID))
                return;

            var renderableSeries = trackToShowObject.CurveRenderableSeries.Single(u => (u as FastLineRenderableSeries).Name == "Formation");

            var startingPoint = double.Parse(formation.Depth.ToString());

            var lineAnnotations = trackToShowObject.Annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended)).Select(v => v as LineAnnotationExtended);

            var annotation = new LineAnnotationExtended
            {
                X1 = 0,
                X2 = 1,
                Y1 = startingPoint,
                Y2 = startingPoint,
                Visibility = Visibility.Visible,
                Stroke = new SolidColorBrush(formation.FormationColor),
                CoordinateMode = AnnotationCoordinateMode.RelativeX,
                Tag = formation.FormationName
            };

            trackToShowObject.FormationsList.Add(formation);

            string toolTipString = IoC.Kernel.Get<IResourceHelper>().ReadResource("FormationTooltip");
            toolTipString = toolTipString.Replace(@"\n", Environment.NewLine);

            GlobalDataModel.ApplyStyleToLine(annotation, formation.LineStyle);
            GlobalDataModel.ApplyGrossStyleToLine(annotation, formation.LineGrossor);

            annotation.ToolTip = string.Format(toolTipString, annotation.Y1.ToString(), formation.FormationName);
            annotation.SetValue(ToolTipService.IsEnabledProperty, IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.IsFTTooltipVisible.Value);

            AddFormationBinding(annotation);

            var series = renderableSeries.DataSeries as XyDataSeries<double, double>;

            series.Append(0, startingPoint);

            annotation.XAxisId = "Formation";

            trackToShowObject.Annotations.Add(annotation);
            AddNameFormation(formation, trackToShowObject);
        }

        private void AddNameFormation(FormationInfo formation, TrackToShow trackToShow)
        {
            var annotationText = formation.FormationName.Length > 20 ? formation.FormationName.Substring(0, 20) : formation.FormationName;

            var textAnnotation = new CustomAnnotation
            {
                Content = annotationText,
                X1 = 0,
                X2 = 1,
                Y1 = double.Parse(formation.Depth.ToString()),
                Y2 = double.Parse(formation.Depth.ToString()),
                FontFamily = new FontFamily("Arial"),
                FontSize = 12,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Right,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                Padding = new Thickness(0, -20, 25, 0),
                CoordinateMode = AnnotationCoordinateMode.RelativeX,
                Tag = "Formation"
            };
            textAnnotation.XAxisId = "Formation";
            Binding binding = new Binding("Width")
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Source = trackToShow.TrackObject
            };
            textAnnotation.SetBinding(CustomAnnotation.WidthProperty, binding);

            AddFTNameBinding(textAnnotation);
            trackToShow.Annotations.Add(textAnnotation);
        }

        private void AddFTNameBinding(CustomAnnotation customAnnotation)
        {
            var binding = new MultiBinding();
            binding.Converter = new FTVisibilityConverter();
            binding.Bindings.Add(new Binding("IsFTNameVisible")
            {
                Source = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            binding.Bindings.Add(new Binding("IsFormationVisible")
            {
                Source = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            customAnnotation.SetBinding(CustomAnnotation.IsHiddenProperty, binding);
        }

        private void AddFormationAxisInChart(TrackToShow trackToShowObject)
        {
            GlobalDataModel.Instance.AddNamedAxisInChart(trackToShowObject, "Formation");
        }

        private void AddFormationBinding(LineAnnotationExtended annotation)
        {
            annotation.SetBinding(LineAnnotationExtended.IsHiddenProperty, new Binding("IsFormationVisible")
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Source = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu,
                Converter = new InverseBooleanConverter()
            });
        }
        public void RemoveFormationObject(FormationInfo formation)
        {
            var chartToShow = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Single(u => u.ChartObject.ID == formation.RefChart);
            if (chartToShow == null) throw new Exception("Chart to show object is null while adding formations");
            if (chartToShow.ChartObject == null) throw new Exception("Chart object is null while adding formations");
            chartToShow.ChartObject.Formations.Remove(formation);
            var annotations = chartToShow.Tracks.SelectMany(u => u.Annotations);
            foreach (var track in chartToShow.Tracks)
            {
                var customAnnotations = track.Annotations.Where(u => u.GetType() == typeof(CustomAnnotation))
                    .Select(v => v as CustomAnnotation).Where(w => w.Content.ToString() == formation.FormationName &&
                        w.XAxisId == "Formation" && w.Tag != null && w.Tag.ToString() == "Formation");

                customAnnotations.ToList().ForEach(u => track.Annotations.Remove(u));

                var lineAnnotations = track.Annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended))
                    .Select(v => v as LineAnnotationExtended).Where(w => w.XAxisId == "Formation" && w.Tag != null && w.Tag.ToString() == formation.FormationName);

                lineAnnotations.ToList().ForEach(u => track.Annotations.Remove(u));

                var renderableSeries = track.CurveRenderableSeries.SingleOrDefault(u => (u as FastLineRenderableSeries).Name == "Formation");
                XyDataSeries<double, double> dataSeries = renderableSeries.DataSeries as XyDataSeries<double, double>;
                dataSeries.RemoveAt(track.FormationsList.IndexOf(formation));
                track.FormationsList.Remove(formation);
            }
        }
    }//end class
}//end namespace
