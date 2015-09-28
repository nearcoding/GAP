using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GAP.CustomControls
{
    /// <summary>
    /// Interaction logic for ScaleControl.xaml
    /// </summary>
    public partial class ScaleControl : UserControl
    {
        public ScaleControl()
        {
            InitializeComponent();
        }
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(ScaleControl), new PropertyMetadata(0.0, PropertyIsBeingChanged));

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(ScaleControl), new PropertyMetadata(0.0, PropertyIsBeingChanged));

        public int NumberOfUnits
        {
            get { return (int)GetValue(NumberOfUnitsProperty); }
            set { SetValue(NumberOfUnitsProperty, value); }
        }

        public static readonly DependencyProperty NumberOfUnitsProperty =
            DependencyProperty.Register("NumberOfUnits", typeof(int), typeof(ScaleControl), new PropertyMetadata(0, PropertyIsBeingChanged));

        public Brush SeriesColor
        {
            get { return (Brush)GetValue(SeriesColorProperty); }
            set { SetValue(SeriesColorProperty, value); }
        }

        public static readonly DependencyProperty SeriesColorProperty =
            DependencyProperty.Register("SeriesColor", typeof(Brush), typeof(ScaleControl), new PropertyMetadata(Brushes.Black, PropertyIsBeingChanged));

        private static void PropertyIsBeingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ScaleControl;
            RedrawControl(control);
        }

        private static void RedrawControl(ScaleControl control)
        {
            control.Grid1.Children.Clear();
            Rectangle rectangle = new Rectangle
            {
                VerticalAlignment = VerticalAlignment.Center,
                Height = 2,
                Stroke = control.SeriesColor
            };
            BindingOperations.SetBinding(rectangle, Rectangle.FillProperty, new Binding("DatasetObject.LineColor")
                {
                    Converter = new ColourToBrushConverter()
                });

            control.Grid1.Children.Add(rectangle);
            Grid.SetColumnSpan(rectangle, 4);
            double differenceInBetween = (control.MaxValue - control.MinValue) / (control.NumberOfUnits + 1);
            double currentValue = control.MinValue;

            for (int i = 0; i <= control.NumberOfUnits + 1; i++)
            {
                if (control.MinValue == 0 && control.MaxValue == 0)
                {
                    if (i != 0 && i != control.NumberOfUnits + 1) continue;
                }
                Line line = new Line
                {
                    Stroke = control.SeriesColor,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    StrokeThickness = 2,
                    VerticalAlignment = VerticalAlignment.Center,
                    Y1 = 0,
                    Y2 = 10
                };
                control.Grid1.Children.Add(line);
                Grid.SetRow(line, 0);
                Grid.SetColumn(line, i);

                TextBlock txt = new TextBlock
                {
                    Text = Math.Round(currentValue).ToString()
                };
                Grid.SetRow(txt, 1);
                Grid.SetColumn(txt, i);
                control.Grid1.Children.Add(txt);
                if (i == control.NumberOfUnits + 1)
                {
                    txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    line.HorizontalAlignment = HorizontalAlignment.Right;
                }

                currentValue += differenceInBetween;
            }
        }
    }//end class
}//end namespace
