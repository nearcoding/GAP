using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GAP.BL;
using GAP.CustomControls;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using GAP.BL.CollectionEntities;
using GAP.MainUI.ViewModels.Helpers;
using System.Windows;
using Ninject;
namespace GAP.Custom_Controls
{
    /// <summary>
    /// Interaction logic for CustomChartLegend.xaml
    /// </summary>
    public partial class CustomChartLegend : UserControl, INotifyPropertyChanged
    {
        public CustomChartLegend()
        {
            InitializeComponent();
            //SeriesColor = new SolidColorBrush(Colors.Black);
            //we dont need checkbox visibility in case control added by lithology or formation. 
            if (MinValue == 0 && MaxValue == 0) CheckBoxVisibility.IsChecked = true;// System.Windows.Visibility.Collapsed;
            _dataContext = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel;
        }

        IMainScreenViewModel _dataContext;
        public SciChartExtended SciChart { get; set; }
        bool _isDoubleLine;

        public bool IsDoubleLine
        {
            get { return _isDoubleLine; }
            set
            {
                _isDoubleLine = value;
                NotifyPropertyChanged("IsDoubleLine");
            }
        }

        DoubleCollection _strokeDashArray;
        public DoubleCollection StrokeDashArray
        {
            get { return _strokeDashArray; }
            set
            {
                _strokeDashArray = value;
                NotifyPropertyChanged("StrokeDashArray");
            }
        }

        public decimal MinValue
        {
            get { return (decimal)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(decimal), typeof(CustomChartLegend), new PropertyMetadata(decimal.Parse("0")));

        public decimal MaxValue
        {
            get { return (decimal)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(decimal), typeof(CustomChartLegend), new PropertyMetadata(decimal.Parse("0")));

        public Brush SeriesColor
        {
            get { return (Brush)GetValue(SeriesColorProperty); }
            set { SetValue(SeriesColorProperty, value); }
        }

        public static readonly DependencyProperty SeriesColorProperty =
            DependencyProperty.Register("SeriesColor", typeof(Brush), typeof(CustomChartLegend), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(string), typeof(CustomChartLegend), new PropertyMetadata(string.Empty));

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public bool IsSeriesVisible
        {
            get { return (bool)GetValue(IsSeriesVisibleProperty); }
            set { SetValue(IsSeriesVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsSeriesVisibleProperty =
            DependencyProperty.Register("IsSeriesVisible", typeof(bool), typeof(CustomChartLegend), new PropertyMetadata(true));

        //public FastLineRenderableSeries RenderableSeriesAssociated
        //{
        //    get { return (FastLineRenderableSeries)GetValue(RenderableSeriesAssociatedProperty); }
        //    set { SetValue(RenderableSeriesAssociatedProperty, value); }
        //}

        //public static readonly DependencyProperty RenderableSeriesAssociatedProperty =
        //    DependencyProperty.Register("RenderableSeriesAssociated", typeof(FastLineRenderableSeries), typeof(CustomChartLegend), new PropertyMetadata(null));

        //public Curve CurveAssociated { get; set; }

        //private void CheckBoxVisibility_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    RenderableSeriesAssociated.IsVisible = (CheckBoxVisibility.IsChecked != null && CheckBoxVisibility.IsChecked.Value);
        //}
    }//end class
}//end namespace
