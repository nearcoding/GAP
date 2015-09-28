using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.Visuals.PointMarkers;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Ninject;
using Abt.Controls.SciChart.Visuals.Annotations;
using System.Windows.Data;
using System.Windows.Media.Imaging;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class TrackAnalysisViewModel : BaseViewModel<BaseEntity>
    {
        public TrackAnalysisViewModel(string token, Track track)
            : base(token)
        {
            TrackObject = track;
            GenerateObjectsFromTrackToShow();
            SelectedChartTheme = GlobalDataModel.MainViewModel.SelectedTrackTheme;
            LithologyX1 = 0.9;
        }

        bool _isTooltipVisible;

        public bool IsTooltipVisible
        {
            get { return _isTooltipVisible; }
            set
            {
                if (value == _isTooltipVisible) return;
                _isTooltipVisible = value;
                NotifyPropertyChanged("IsTooltipVisible");
            }
        }

        string _selectedChartTheme;
        public string SelectedChartTheme
        {
            get { return _selectedChartTheme; }
            set
            {
                _selectedChartTheme = value;
                NotifyPropertyChanged("SelectedChartTheme");
            }
        }
        Track _trackObject;
        public Track TrackObject
        {
            get { return _trackObject; }
            set
            {
                _trackObject = value;
                NotifyPropertyChanged("TrackObject");
            }
        }
        public TrackToShow TrackToShow { get; set; }

        private void GenerateObjectsFromTrackToShow()
        {
            TrackToShow = AddTrackToShowObject(TrackObject);
            AddCurveToExistingChart(TrackToShow);
            GenerateFormations();
            GenerateLithologies();
        }

        private void GenerateLithologies()
        {
            foreach (var lithology in TrackObject.Lithologies)
            {
                AddLithologyToTrack(lithology);
            }
        }
        private void AddLithologyToTrack(LithologyInfo lithology)
        {
            var trackToShowObject = TrackToShow;// Charts.Single(u => u.ChartObject.ChartName == lithology.RefChart).Tracks.Single(u => u.TrackObject.TrackName == lithology.RefTrack);
            //track to show object has renderable series  for lithologies
            //get the proper renderable series and add an annotation to it
            if (!trackToShowObject.CurveRenderableSeries.Any(u => (u as FastLineRenderableSeries).Name == "Lithology"))
                AddLithologyAxisInChart(trackToShowObject);

            var renderableSeries = trackToShowObject.CurveRenderableSeries.Single(u => (u as FastLineRenderableSeries).Name == "Lithology");
            ImageBrush brush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(GlobalDataModel.LithologyImageFolder + "\\" + lithology.ImageFile)),
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute
            };
            brush.Viewport = new Rect(0, 0, brush.ImageSource.Width, brush.ImageSource.Height);

            var startingPoint = double.Parse(lithology.InitialDepth.ToString());
            var endingPoint = double.Parse(lithology.FinalDepth.ToString());

            BoxAnnotation annotation = new BoxAnnotation
            {
                X2 = 1,
                Y1 = startingPoint,
                Y2 = endingPoint,
                CoordinateMode = AnnotationCoordinateMode.RelativeX,
                Background = brush
            };
            AddLithologyBinding(renderableSeries, annotation);

            var series = renderableSeries.DataSeries as XyDataSeries<double, double>;
            series.Append(-1, startingPoint);
            series.Append(-1, endingPoint);
            var targetXAxis = trackToShowObject.XAxisCollection.Single(u => u.Id == "Lithology");
            annotation.XAxisId = targetXAxis.Id;
            trackToShowObject.Annotations.Add(annotation);
        }
        private void AddLithologyAxisInChart(TrackToShow trackToShowObject)
        {
            AddNamedAxisInChart(trackToShowObject, "Lithology");

            if (IsLithologyVisible || IsFullLithology)
                trackToShowObject.HasCurves = trackToShowObject.CurveCollection.Any();
        }
        private void AddLithologyBinding(IRenderableSeries renderableSeries, BoxAnnotation annotation)
        {
            annotation.SetBinding(BoxAnnotation.X1Property, new Binding("LithologyX1")
            {
                Source = this,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay
            });
            var multiBinding = new MultiBinding();
            multiBinding.Converter = new LithologyVisibilityConverter();
            multiBinding.Bindings.Add(new Binding("IsVisible")
            {
                Source = renderableSeries,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            multiBinding.Bindings.Add(new Binding("IsLithologyVisible")
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            multiBinding.Bindings.Add(new Binding("IsFullLithology")
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            annotation.SetBinding(BoxAnnotation.IsHiddenProperty, multiBinding);
        }
        private void GenerateFormations()
        {
            var chartObject = HelperMethods.Instance.GetChartByID(TrackObject.RefChart);
            foreach (var formation in chartObject.Formations)
                AddFormationInfoInTrackToShowObject(formation, TrackToShow);
        }

        private void AddFormationAxisInChart(TrackToShow trackToShowObject)
        {
            AddNamedAxisInChart(trackToShowObject, "Formation");
        }
        private void AddNamedAxisInChart(TrackToShow trackToShowObject, string axisName)
        {
            var numericAxis = new NumericAxis
            {
                DrawLabels = false,
                DrawMajorBands = false,
                DrawMajorGridLines = false,
                DrawMajorTicks = false,
                Id = axisName
            };
            var fastLineRenderableSeries = new FastLineRenderableSeries
            {
                Name = axisName,
                XAxisId = numericAxis.Id
            };
            trackToShowObject.CurveRenderableSeries.Add(fastLineRenderableSeries);
            if (axisName == "Lithology")
                trackToShowObject.CurveCollection[0].FastLineRenderableSeries = fastLineRenderableSeries;
            var dataSeries = new XyDataSeries<double, double>();
            //dataSeries.Append(0, 10);

            fastLineRenderableSeries.DataSeries = dataSeries;
            trackToShowObject.XAxisCollection.Add(numericAxis);

            numericAxis.VisibleRange = new DoubleRange(0, 10);
            numericAxis.VisibleRangeLimit = new DoubleRange(0, 10);
            numericAxis.VisibleRangeChanged += xAxis_VisibleRangeChanged;
        }

        private void AddFormationBinding(IRenderableSeries renderableSeries, LineAnnotationExtended annotation)
        {
            annotation.SetBinding(LineAnnotationExtended.IsHiddenProperty, new Binding("IsFormationVisible")
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Source = this,
                Converter = new InverseBooleanConverter()
            });
        }
        private void AddFormationInfoInTrackToShowObject(FormationInfo formation, TrackToShow trackToShowObject)
        {
            if (!trackToShowObject.CurveRenderableSeries.Any(u => (u as FastLineRenderableSeries).Name == "Formation"))
                AddFormationAxisInChart(trackToShowObject);

            var renderableSeries = trackToShowObject.CurveRenderableSeries.Single(u => (u as FastLineRenderableSeries).Name == "Formation");

            var startingPoint = double.Parse(formation.Depth.ToString());
            var annotation = new LineAnnotationExtended
            {
                X1 = 0,
                X2 = 1,
                Y1 = startingPoint,
                Y2 = startingPoint,
                Visibility = Visibility.Visible,
                Stroke = new SolidColorBrush(formation.FormationColor),
                CoordinateMode = AnnotationCoordinateMode.RelativeX,
                Tag = formation.FormationName //we are using this tag property to display the tooltip
            };
            string toolTipString = IoC.Kernel.Get<IResourceHelper>().ReadResource("FormationTooltip");
            toolTipString = toolTipString.Replace(@"\n", Environment.NewLine);

            annotation.ToolTip = string.Format(toolTipString, annotation.Y1.ToString(), formation.FormationName);
            annotation.SetValue(ToolTipService.IsEnabledProperty, IsFTTooltipVisible);
            AddFormationBinding(renderableSeries, annotation);
            var series = renderableSeries.DataSeries as XyDataSeries<double, double>;
            series.Append(0, startingPoint);

            annotation.XAxisId = "Formation";
            trackToShowObject.Annotations.Add(annotation);
            AddNameFormation(formation, trackToShowObject);
        }
        bool _isFTVisible;
        public bool IsFTTooltipVisible
        {
            get { return _isFTVisible; }
            set
            {
                if (_isFTVisible == value) return;
                _isFTVisible = value;

                var formationAxis = TrackToShow.XAxisCollection.Single(u => u.Id == "Formation");
                foreach (var annotation in TrackToShow.Annotations)
                {
                    var lineAnnotation = annotation as LineAnnotationExtended;
                    if (lineAnnotation == null) continue;
                    lineAnnotation.SetValue(ToolTipService.IsEnabledProperty, value);
                }

                NotifyPropertyChanged("IsFTTooltipVisible");
            }
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
                Margin = new Thickness(-50, -20, 0, 0),
                CoordinateMode = AnnotationCoordinateMode.RelativeX,
                Tag = "Formation"
            };
            textAnnotation.XAxisId = "Formation";
            Binding binding = new Binding("ActualWidth")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ElementName = "SciChartControl1",
            };
            textAnnotation.SetBinding(CustomAnnotation.WidthProperty, binding);

            AddFTNameBinding(textAnnotation);
            trackToShow.Annotations.Add(textAnnotation);
        }
        bool _isFTNameVisible;
        public bool IsFTNameVisible
        {
            get { return _isFTNameVisible; }
            set
            {
                if (_isFTNameVisible == value) return;
                _isFTNameVisible = value;
                NotifyPropertyChanged("IsFTNameVisible");
            }
        }
        private void AddFTNameBinding(CustomAnnotation customAnnotation)
        {
            var binding = new MultiBinding();
            binding.Converter = new FTVisibilityConverter();
            binding.Bindings.Add(new Binding("IsFTNameVisible")
            {
                Source = this,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            binding.Bindings.Add(new Binding("IsFormationVisible")
            {
                Source = this,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            customAnnotation.SetBinding(CustomAnnotation.IsHiddenProperty, binding);
        }

        public AxisCollection XAxisCollection { get; set; }
        private TrackToShow AddTrackToShowObject(Track track)
        {
            var trackToShowObject = new TrackToShow
            {
                TrackObject = track,
                XAxisCollection = new AxisCollection(),
                CurveCollection = new ObservableCollection<CurveToShow>()
            };

            trackToShowObject.CurveCollection.Add(new CurveToShow(track.RefChart, track.Name,"")
            {
                MinValue = 0,
                MaxValue = 0,
                //LabelText = "Lithology",
                //SeriesColor = new SolidColorBrush(Colors.Black),
                Visibility = this.IsLithologyVisible ? Visibility.Visible : Visibility.Collapsed
            });
            return trackToShowObject;
        }

        private void AddCurveToExistingChart(TrackToShow track)
        {
            track.Curves = new ObservableCollection<CurveToShow>();
            foreach (var curve in track.TrackObject.Curves)
            {
                var curveToShow = new CurveToShow(track.TrackObject.RefChart, track.TrackObject.Name,curve.RefDataset)
                {
                    CurveObject = curve
                };
                AddCurveToTrack(curveToShow, track);
                track.Curves.Add(curveToShow);
                track.CurveCollection.Add(curveToShow);
            }
        }

        private void AddCurveToTrack(CurveToShow curveToShow, TrackToShow trackToShow)
        {
            var curve = curveToShow.CurveObject;
            var dataset = HelperMethods.Instance.GetDatasetByID(curve.RefDataset);
            var dataSeries = new XyDataSeries<double, double>();

            double minValueXAxis, maxValueXAxis;
            decimal minUnitValue;
            if (dataset.MinUnitValue >= (decimal)-999.999 && dataset.MinUnitValue <= -999)
                minUnitValue = dataset.DepthAndCurves.Except(dataset.DepthAndCurves.Where(u => u.Curve >= ((decimal)-999.99) && u.Curve <= -999)).Min(u => u.Curve);
            else
                minUnitValue = dataset.MinUnitValue;
            minValueXAxis = double.Parse(minUnitValue.ToString());
            maxValueXAxis = Convert.ToDouble(dataset.MaxUnitValue);
            
            FastLineRenderableSeries fastLineSeries = new FastLineRenderableSeries();

            curveToShow.MinValue = decimal.Parse(minValueXAxis.ToString());
            curveToShow.MaxValue = decimal.Parse(maxValueXAxis.ToString());
            //curveToShow.SeriesColor = new SolidColorBrush(dataset.LineColor);
            //curveToShow.LabelText = string.Format("{0}/{1} [{2}]", curve.RefWell, curve.RefDataset, dataset.Units);
            curveToShow.FastLineRenderableSeries = fastLineSeries;

            var lstToBeUsed = dataset.DepthAndCurves.Except(dataset.DepthAndCurves.Where(u => u.Curve <= -999 && u.Curve >= (decimal)-999.99)).ToList();

            lstToBeUsed.ForEach(u => dataSeries.Append((double)u.Curve, (double)u.Depth));
            var xAxis = AddXAxisToChart(dataset);
            var minDepth = lstToBeUsed.Min(u => u.Depth);
            trackToShow.XAxisCollection.Add(xAxis);
            curveToShow.CurveObject.ID = xAxis.Id;
         //   ApplyCurveStyle.ApplyLineGrosserToCurve(dataset, fastLineSeries);
            //fastLineSeries = ApplyCurveStyle.ApplyLineStyleToCurve(dataset, fastLineSeries);

            //fastLineSeries.XAxisId = xAxis.Id;
            //if (dataset.MarkerStyle > 0)
            //{
            //    var pointMarker = new SpritePointMarker();
            //    var markerStyleTemplate = ApplyCurveStyle.ApplyMarkerStyle(dataset);
            //    pointMarker.PointMarkerTemplate = (ControlTemplate)XamlReader.Parse(markerStyleTemplate);
            //    fastLineSeries.PointMarker = pointMarker;
            //}

            fastLineSeries.XAxisId = xAxis.Id;
            fastLineSeries.DataSeries = dataSeries;
            fastLineSeries.SeriesColor = Color.FromRgb(dataset.LineColor.R, dataset.LineColor.G, dataset.LineColor.B);
            fastLineSeries.Tag = dataset.Name;
            trackToShow.CurveRenderableSeries.Add(fastLineSeries);
            trackToShow.HasCurves = trackToShow.CurveRenderableSeries.Any();
            //CalculateMinMaxVisibleRangeLimitForYAxis();
        }

        double _lithologyX1;
        public double LithologyX1
        {
            get { return _lithologyX1; }
            set
            {
                _lithologyX1 = value;
                NotifyPropertyChanged("LithologyX1");
            }
        }

        bool _isFullLithology;
        public bool IsFullLithology
        {
            get { return _isFullLithology; }
            set
            {
                if (value == _isFullLithology) return;
                _isFullLithology = value;

                SetHasCurvesInCaseOfLithology(value);

                LithologyX1 = value ? 0 : 0.9;
                NotifyPropertyChanged("IsFullLithology");
                //GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.ShowFullLithology);
            }
        }

        bool _isLithologyVisible;
        public bool IsLithologyVisible
        {
            get { return _isLithologyVisible; }
            set
            {
                if (value == _isLithologyVisible) return;
                _isLithologyVisible = value;

                SetHasCurvesInCaseOfLithology(value);
                NotifyPropertyChanged("IsLithologyVisible");
            }
        }
        bool _isFormationVisible;

        public bool? IsFormationVisible
        {
            get { return _isFormationVisible; }
            set
            {
                if (value == _isFormationVisible) return;
                _isFormationVisible = value.Value;
                NotifyPropertyChanged("IsFormationVisible");
                //  GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.ShowHideFormation);
            }
        }
        private void SetHasCurvesInCaseOfLithology(bool value)
        {
            if (IsLithologyVisible == false && IsFullLithology == false)
            {
                TrackToShow.HasCurves = TrackToShow.CurveCollection.Count > 1;
            }
            else
            {
                TrackToShow.HasCurves = true;
            }
            TrackToShow.CurveCollection[0].Visibility = (IsLithologyVisible || IsFullLithology) ? Visibility.Visible : Visibility.Collapsed;
        }
        private NumericAxis AddXAxisToChart(Dataset dataset)
        {
            double minValueXAxis, maxValueXAxis;
            decimal minUnitValue;
            if (dataset.MinUnitValue >= (decimal)-999.999 && dataset.MinUnitValue <= -999)
                minUnitValue = dataset.DepthAndCurves.Except(dataset.DepthAndCurves.Where(u => u.Curve >= ((decimal)-999.99) && u.Curve <= -999)).Min(u => u.Curve);
            else
                minUnitValue = dataset.MinUnitValue;
            minValueXAxis = double.Parse(minUnitValue.ToString());
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
            xAxis.VisibleRangeChanged += xAxis_VisibleRangeChanged;

            xAxis.Id = Guid.NewGuid().ToString();

            return xAxis;
        }
        void xAxis_VisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
        {
            (sender as NumericAxis).VisibleRange = (sender as NumericAxis).VisibleRangeLimit;
        }
    }//end class
}//end namespace
