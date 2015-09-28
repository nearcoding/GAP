using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GAP.HelperClasses
{
    public class GridHelper : Grid
    {
        public static int GetRowCount(DependencyObject obj)
        {
            return (int)obj.GetValue(RowCountProperty);
        }

        public static void SetRowCount(DependencyObject obj, int value)
        {
            obj.SetValue(RowCountProperty, value);
        }

        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.RegisterAttached("RowCount", typeof(int), typeof(GridHelper), new PropertyMetadata(0, RowCountChanged));

        private static void RowCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if (grid == null) return;
            int rowCount = 0;
            if (!Int32.TryParse(e.NewValue.ToString(), out rowCount)) return;
            string[] rowSize = GetRowHeight(grid).Split(',');
            for (int i = 0; i < rowCount; i++)
            {
                var rowDefinition = new RowDefinition { Height = GridLength.Auto };
                if (rowSize.Length <= i)
                {
                    continue;
                }
                if (rowSize[i].Contains("*"))
                {
                    int getLength;
                    if (rowSize[i].Length > 1)
                    {
                        getLength = Int32.Parse(rowSize[i].Substring(0, rowSize[i].Length - 1));
                    }
                    else
                    {
                        getLength = 1;
                    }
                    rowDefinition.Height = new GridLength(getLength, GridUnitType.Star);
                }
                else if (rowSize[i].ToLower() == "auto")
                {
                    rowDefinition.Height = GridLength.Auto;
                }
                else
                {
                    int pixelLength = 0;
                    if (Int32.TryParse(rowSize[i].ToString(), out pixelLength))
                        rowDefinition.Height = new GridLength(pixelLength, GridUnitType.Pixel);
                    else
                        rowDefinition.Height = GridLength.Auto;
                }
                grid.RowDefinitions.Add(rowDefinition);
            }
        }

        public static int GetColumnCount(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnCountProperty);
        }

        public static void SetColumnCount(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnCountProperty, value);
        }

        // Using a DependencyProperty as the backing store for ColumnCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.RegisterAttached("ColumnCount", typeof(int), typeof(GridHelper), new PropertyMetadata(0, ColumnCountChanged));

        private static void ColumnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WriteGridColumns(d, ref e);
        }

        private static void WriteGridColumns(DependencyObject d, ref DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if (grid == null) return;
            grid.ColumnDefinitions.Clear();
            int columnCount = 0;
            if (!Int32.TryParse(e.NewValue.ToString(), out columnCount)) return;

            var columnWidths = GetColumnWidth(grid).Split(',');
            for (int i = 0; i < columnCount; i++)
            {
                var columnDefinition = new ColumnDefinition { MinWidth =140 };
                grid.ColumnDefinitions.Add(columnDefinition);
                if (columnWidths.Length <= i) continue;
                     
                columnDefinition.SetBinding(ColumnDefinition.WidthProperty,new Binding("TrackObject.Width")
                {
                    Mode=BindingMode.TwoWay,
                    Converter=new GAP.Converters.GridLengthConverter()
                });
            }
        }

        public static string GetRowHeight(DependencyObject obj)
        {
            return (string)obj.GetValue(RowHeightProperty);
        }

        public static void SetRowHeight(DependencyObject obj, string value)
        {
            obj.SetValue(RowHeightProperty, value);
        }

        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.RegisterAttached("RowHeight", typeof(string), typeof(GridHelper), new PropertyMetadata(""));



        public static string GetColumnWidth(DependencyObject obj)
        {
            return (string)obj.GetValue(ColumnWidthProperty);
        }

        public static void SetColumnWidth(DependencyObject obj, string value)
        {
            obj.SetValue(ColumnWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for ColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnWidthProperty =
            DependencyProperty.RegisterAttached("ColumnWidth", typeof(string), typeof(GridHelper), new PropertyMetadata(""));



        public static double GetMinimumWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(MinimumWidthProperty);
        }

        public static void SetMinimumWidth(DependencyObject obj, double value)
        {
            obj.SetValue(MinimumWidthProperty, value);
        }

        public static readonly DependencyProperty MinimumWidthProperty =
            DependencyProperty.RegisterAttached("MinimumWidth", typeof(double), typeof(GridHelper)
            , new PropertyMetadata(double.Parse("0"), MinWidthChanged));
        private static void MinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //WriteGridColumns(d, ref e);
        }
    }//end grid helper class
}//end namespace