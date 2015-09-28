using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace GAP.BL
{
    public class CurveToShow : SubBaseEntity
    {
        public CurveToShow(string chart, string track, string dataset)
        {
            SubDatasets = new List<SubDataset>();
            RefChart = chart;
            RefTrack = track;
            RefDataset = dataset;
            DatasetObject = HelperMethods.Instance.GetDatasetByID(dataset);
        }
        //we need this property because we need to bind lots of properties from dataset like curve and marker properties and legend control
        public Dataset DatasetObject { get; set; }

        bool _isSeriesVisible;
        public bool IsSeriesVisible
        {
            get { return _isSeriesVisible; }
            set
            {
                _isSeriesVisible = value;
                CurveObject.IsSeriesVisible = value;
                if (FastLineRenderableSeries != null) FastLineRenderableSeries.IsVisible = value;
                NotifyPropertyChanged("IsSeriesVisible");
            }
        }

        public string RefDataset { get; set; }
        public void AddAnnotationByAnnotationAndSubDatasetObject(AnnotationInfo annotation, SubDataset subDataset, CurveToShow curveToShow, string uniqueAnnotationName)
        {
            if (string.IsNullOrWhiteSpace(uniqueAnnotationName))
                throw new Exception("Annotation does not have unique name");
            var lineAnnotation = HelperMethods.Instance.GetLineAnnotationByAnnotationInfoAndSubDataset(annotation, subDataset, curveToShow);
            lineAnnotation.Id = uniqueAnnotationName;
            LineAnnotationExtendedBinding(lineAnnotation);
            AddAnnotation(lineAnnotation);
        }

        /// <summary>
        /// this method binds the subdataset annotation visibility with the curve to show IsSeries visible property,
        /// so upon checking visibility for curve, it should hide the annotations as well
        /// </summary>
        /// <param name="lineAnnotation"></param>
        public void LineAnnotationExtendedBinding(LineAnnotationExtended lineAnnotation)
        {
            lineAnnotation.SetBinding(LineAnnotationExtended.IsHiddenProperty, new Binding("IsSeriesVisible")
            {
                Source = this,
                Converter = new InverseBooleanConverter()
            });

            lineAnnotation.SetBinding(LineAnnotationExtended.StrokeProperty, new Binding("LineColor")
            {
                Source = lineAnnotation.SubDataset,
                Converter = new ColourToBrushConverter()
            });

            lineAnnotation.SetBinding(LineAnnotationExtended.StrokeThicknessProperty, new Binding("LineGrossor")
            {
                Source = lineAnnotation.SubDataset,
                Converter = new LineGrossorToStrokeThicknessConverter()
            });

            lineAnnotation.SetBinding(LineAnnotationExtended.StrokeDashArrayProperty, new Binding("LineStyle")
                {
                    Source = lineAnnotation.SubDataset,
                    Converter = new SubDatasetLineStyleToStrokeDashArrayConverter()
                });
        }

        private LineAnnotationExtended ReplaceLineAnnotationWithLineAnnotationExtended()
        {
            var lineAnnotation = TrackToShowObject.Annotations[TrackToShowObject.Annotations.Count - 1];
            var lineAnnotationExtended = new LineAnnotationExtended
            {
                SubDataset = CurrentSubDataset,
                CurveToShow = this,
                X1 = lineAnnotation.X1,
                X2 = lineAnnotation.X2,
                Y1 = lineAnnotation.Y1,
                Y2 = lineAnnotation.Y2,
                XAxisId = this.CurveObject.ID,
                Id = Guid.NewGuid().ToString()
            };
            TrackToShowObject.Annotations.Remove(lineAnnotation);
            TrackToShowObject.Annotations.Add(lineAnnotationExtended);
            return lineAnnotationExtended;
        }
        
        SubDataset _currentSubDataset;
        public SubDataset CurrentSubDataset
        {
            get { return _currentSubDataset; }
            set
            {
                _currentSubDataset = value;
            }
        }

        public AnnotationCreationModifier AnnotationModifier { get; set; }
        void AnnotationModifier_AnnotationCreated(object sender, EventArgs e)
        {
            if (CurrentSubDataset == null) throw new Exception("Current sub dataset is null while adding annotation");
            var currentAnnotation = AnnotationModifier.Annotation as LineAnnotationExtended;

            if (currentAnnotation == null) return;
            var lineAnnotation = ReplaceLineAnnotationWithLineAnnotationExtended();

            LineAnnotationExtendedBinding(lineAnnotation);

            lineAnnotation.CoordinateMode = AnnotationCoordinateMode.Absolute;

            lineAnnotation.IsEditable = false;
            lineAnnotation.IsEnabled = false;
            AnnotationModifier.IsEnabled = false;
            AnnotationModifier.IsEnabled = true;

            AnnotationInfo info = new AnnotationInfo
            {
                ID = lineAnnotation.Id,
                X1 = lineAnnotation.X1.ToString(),
                X2 = lineAnnotation.X2.ToString(),
                Y1 = lineAnnotation.Y1.ToString(),
                Y2 = lineAnnotation.Y2.ToString(),
                SubDatasetName = CurrentSubDataset.Dataset
            };

            var annotationRecorder = new AnnotationRecorder
                {
                    AnnotationInfo = info,
                    LineAnnotation = lineAnnotation,
                    ActionPerformed = BL.ActionPerformed.ItemAdded,
                    TrackToShow = TrackToShowObject
                };

            AnnotationAdded(annotationRecorder, this, lineAnnotation);
        }

        public string RefChart { get; set; }
        public string RefTrack { get; set; }

        public delegate void AnnotationAddedDelegate(AnnotationRecorder annotation, CurveToShow curveToShow, LineAnnotationExtended lineAnnotation);//, Curve curve);

        public event AnnotationAddedDelegate AnnotationAdded;

        public List<SubDataset> SubDatasets { get; set; }

        public Curve CurveObject { get; set; }

        decimal _minValue, _maxValue;
        public decimal MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                NotifyPropertyChanged("MinValue");
            }
        }
        public decimal MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                NotifyPropertyChanged("MaxValue");
            }
        }

        //this property is used by checkbox  in legend control to show hide the curve
        public FastLineRenderableSeries FastLineRenderableSeries { get; set; }

        TrackToShow _trackToShowObject;
        public TrackToShow TrackToShowObject
        {
            get { return _trackToShowObject; }
            set
            {
                _trackToShowObject = value;
                AnnotationModifier = new AnnotationCreationModifier
                {
                    ReceiveHandledEvents = true,
                    IsEnabled = false,
                    AnnotationType = typeof(LineAnnotationExtended)
                };

                _trackToShowObject.ChartModifier.ChildModifiers.Add(AnnotationModifier);
                AnnotationModifier.AnnotationCreated -= AnnotationModifier_AnnotationCreated;
                AnnotationModifier.AnnotationCreated += AnnotationModifier_AnnotationCreated;
            }
        }
        Visibility _visibility;
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                NotifyPropertyChanged("Visibility");
            }
        }

        public void AddAnnotation(LineAnnotationExtended lineAnnotation)
        {
            var lineAnnotations = TrackToShowObject.Annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended)).Select(v => v as LineAnnotationExtended);
            if (lineAnnotations.Any(u => u.Id == lineAnnotation.Id))
            {
                throw new Exception("Duplication line annotation is about th enter in the system");
            }
            TrackToShowObject.Annotations.Add(lineAnnotation);
        }
    }//end class
}//end namespace
