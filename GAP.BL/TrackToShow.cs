using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml;
using System.Linq;
using Abt.Controls.SciChart.Utility.Mouse;
using Abt.Controls.SciChart.Visuals.Axes;
using System.Collections.Generic;
namespace GAP.BL
{
    public class TrackToShow : SubBaseEntity
    {
        public TrackToShow()
        {
            XAxisCollection = new AxisCollection();
            YAxis = new NumericAxis
            {
                DrawMajorTicks = false,
                AxisAlignment = AxisAlignment.Left,
                FlipCoordinates = true,
                MinimalZoomConstrain = 1,
                Name = "yAxisDesign",
            };

            HasCurves = false;
            ChartModifier = new ModifierGroup();

            SeriesModifier = new SeriesSelectionModifier
            {
                ReceiveHandledEvents = true
            };
            ScaleModifier = new ScaleFactorModifier
            {
                ReceiveHandledEvents = true
            };
            ZoomPanModifier = new ZoomPanModifierExtension
            {
                ReceiveHandledEvents = true,
                ClipModeX = ClipMode.None,
                ExecuteOn = ExecuteOn.MouseLeftButton,
                IsEnabled = true,
                XyDirection = XyDirection.YDirection,
                ZoomExtentsY = true
            };

            LegendModifier = new LegendModifier
            {
                GetLegendDataFor = SourceMode.AllSeries
            };
            MouseWheelModifier = new MouseWheelZoomModifier
            {
                ReceiveHandledEvents = true,
                XyDirection = XyDirection.YDirection
            };
            ZoomModifier = new ZoomExtentsModifier
            {
                ReceiveHandledEvents = true,
                XyDirection = XyDirection.YDirection
            };
            TooltipModifierObject = new TooltipModifier
            {
                UseInterpolation = true
            };

            TooltipModifierObject.TooltipLabelTemplate = GetTooltipLabelTemplate();

            ChartModifier.ChildModifiers.Add(SeriesModifier);
            ChartModifier.ChildModifiers.Add(ScaleModifier);
            ChartModifier.ChildModifiers.Add(ZoomPanModifier);
            ChartModifier.ChildModifiers.Add(LegendModifier);
            ChartModifier.ChildModifiers.Add(MouseWheelModifier);
            ChartModifier.ChildModifiers.Add(ZoomModifier);
            ChartModifier.ChildModifiers.Add(TooltipModifierObject);
            ChartModifier.ChildModifiers.Add(ExtendedTooltipModifier);
        }

        List<FormationInfo> _formationsList;
        public List<FormationInfo> FormationsList
        {
            get
            {
                if (_formationsList == null)
                    FormationsList = new List<FormationInfo>();

                return _formationsList;
            }
            set
            {
                _formationsList = value;
                NotifyPropertyChanged("FormationsList");
            }
        }

        ExtendedTooltipModifier _extendedTooltipModifier;
        public ExtendedTooltipModifier ExtendedTooltipModifier
        {
            get
            {
                if (_extendedTooltipModifier == null)
                    _extendedTooltipModifier = new ExtendedTooltipModifier();

                return _extendedTooltipModifier;
            }
            set
            {
                _extendedTooltipModifier = value;
            }
        }

        public NumericAxis YAxis { get; set; }

        public ModifierGroup ChartModifier { get; set; }
        private ControlTemplate GetTooltipLabelTemplate()
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>");
            sBuilder.AppendLine("<Border Background='Black'");
            sBuilder.AppendLine("BorderThickness='1'");
            sBuilder.AppendLine("CornerRadius='5'");
            sBuilder.AppendLine("Padding='5'>");
            sBuilder.AppendLine("<StackPanel>");
            sBuilder.AppendLine("<TextBlock FontSize='12' Foreground='White' Text='{Binding YValue, StringFormat=Depth: \\{0:0.0\\}}' />");
            sBuilder.AppendLine("<TextBlock FontSize='12' Foreground='White' Text='{Binding XValue, StringFormat=Value: \\{0:0.0\\}}' />");
            sBuilder.AppendLine("</StackPanel>");
            sBuilder.AppendLine("</Border>");
            sBuilder.AppendLine("</ControlTemplate>");
            StringReader stringReader = new StringReader(sBuilder.ToString());
            XmlReader xmlReader = XmlReader.Create(stringReader);
            var controlTemplate = (ControlTemplate)XamlReader.Load(xmlReader);
            controlTemplate.TargetType = typeof(TemplatableControl);
            return controlTemplate;
        }
        public TooltipModifier TooltipModifierObject { get; set; }
        public ZoomExtentsModifier ZoomModifier { get; set; }
        public MouseWheelZoomModifier MouseWheelModifier { get; set; }
        public LegendModifier LegendModifier { get; set; }
        public ZoomPanModifierExtension ZoomPanModifier { get; set; }
        public ScaleFactorModifier ScaleModifier { get; set; }
        public SeriesSelectionModifier SeriesModifier { get; set; }

        AnnotationCollection _annotations;
        public AnnotationCollection Annotations
        {
            get
            {
                if (_annotations == null)
                    _annotations = new AnnotationCollection();
                return _annotations;
            }
            set
            {
                _annotations = value;
                NotifyPropertyChanged("Annotations");
            }
        }

        double _trackSliderValue;
        public double SliderValue
        {
            get { return _trackSliderValue; }
            set
            {
                _trackSliderValue = value;
                NotifyPropertyChanged("SliderValue");
            }
        }

        bool _hasCurves;
        public bool HasCurves
        {
            get { return _hasCurves; }
            set
            {
                _hasCurves = value;
                NotifyPropertyChanged("HasCurves");
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
       
        ObservableCollection<IRenderableSeries> _curveRenderableSeries;
        public ObservableCollection<IRenderableSeries> CurveRenderableSeries
        {
            get
            {
                if (_curveRenderableSeries == null)
                    _curveRenderableSeries = new ObservableCollection<IRenderableSeries>();
                return _curveRenderableSeries;
            }
            set
            {
                _curveRenderableSeries = value;
                NotifyPropertyChanged("CurveRenderableSeries");
            }
        }

        AxisCollection _xAxisCollection;
        public AxisCollection XAxisCollection
        {
            get { return _xAxisCollection; }
            set
            {
                _xAxisCollection = value;
                NotifyPropertyChanged("XAxisCollection");
            }
        }
        bool _isTrackSelected;
        public bool IsTrackSelected
        {
            get { return _isTrackSelected; }
            set
            {
                _isTrackSelected = value;
                NotifyPropertyChanged("IsTrackSelected");
            }
        }


        ObservableCollection<CurveToShow> _curves;
        public ObservableCollection<CurveToShow> Curves
        {
            get
            {
                if (_curves == null)
                    _curves = new ObservableCollection<CurveToShow>();
                return _curves;
            }
            set
            {
                _curves = value;
            }
        }

    }//end class
}//end namespace
