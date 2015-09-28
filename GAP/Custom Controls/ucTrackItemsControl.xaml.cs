using Abt.Controls.SciChart.Visuals;
using Abt.Controls.SciChart.Visuals.Axes;
using GAP.BL;
using System.Linq;
using GAP.CustomControls;
using GAP.HelperClasses;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ninject;
using GAP.MainUI.ViewModels.Helpers;
namespace GAP.Custom_Controls
{
    /// <summary>
    /// Interaction logic for ucTrackItemsControl.xaml
    /// </summary>
    public partial class ucTrackItemsControl : UserControl
    {
        public ucTrackItemsControl()
        {
            InitializeComponent();
            GlobalData.TrackItemsControl = this;
            LayoutUpdated += ucTrackItemsControl_LayoutUpdated;
        }

        void ucTrackItemsControl_LayoutUpdated(object sender, EventArgs e)
        {
            VisualTreeLooper(this.ScrollViewer1);
        }
      
        private static void VisualTreeLooper(Visual elementToRunLoopOn)
        {            
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(elementToRunLoopOn); i++)
            {
                var childVisual = (Visual)VisualTreeHelper.GetChild(elementToRunLoopOn, i);

                if (childVisual.GetType() == typeof(MainGrid))
                {
                    if ((childVisual as MainGrid) != null && (childVisual as MainGrid).Name == "PART_MainGrid")
                        VisualTreeLooper(childVisual);
                }
                else if (childVisual.GetType() == typeof(AxisArea))
                {
                    var ctrl = childVisual as AxisArea;
                    var grid = elementToRunLoopOn as Grid;
                    if (ctrl != null && grid != null)
                    {
                        switch (ctrl.Name)
                        {
                            case "PART_LeftAxisArea":
                                Grid.SetColumn(ctrl, 2);
                                break;
                            case "PART_BottomAxisArea":
                                Grid.SetColumn(ctrl, 2);
                                ctrl.MaxHeight = 1;
                                break;
                        }
                    }
                }
                VisualTreeLooper(childVisual);
            }
        }
        
        public void ScrollToCenter(string trackName)
        {
            double width = 0;
            foreach (var item in this.Control1.Items)
            {
                ContentPresenter cp = this.Control1.ItemContainerGenerator.ContainerFromItem(item) as ContentPresenter;
                GroupBox groupBox = FindVisualChild<GroupBox>(cp);
                if (groupBox == null) continue;
               
                if (groupBox != null && groupBox.Header.ToString() == trackName)
                {
                    //var point = this.ScrollViewer1.TranslatePoint(new Point(), groupBox);
                    // point.X = point.X + GlobalData.MainWindow._solutionExplorer.Width;
                    //double val = (point.X - 400) * -1;
                    this.ScrollViewer1.ScrollToHorizontalOffset(width);
                    return;
                } width += groupBox.ActualWidth;
            }
        }

        public static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }

        private void CustomChart1_Drop(object sender, DragEventArgs e)
        {
            var datasetItem = e.Data.GetData(typeof(TreeViewItem)) as TreeViewItem;
            if (datasetItem == null) return;
            var extended = sender as SciChartExtended;
            
            var dataset = datasetItem.Header as Dataset;
            var curves = HelperMethods.Instance.GetCurvesByTrackID(extended.Track);
            if (curves.Any(u => u.RefProject == dataset.RefProject && u.RefWell == dataset.RefWell && u.RefDataset == dataset.Name))
            {
                string msg = IoC.Kernel.Get<IResourceHelper>().ReadResource("OverlappingCurves");
                msg = msg.Replace(@"\n", Environment.NewLine);
                MessageBox.Show(msg, GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //draw a new curve to the chart
            new AddCurveView(dataset.RefProject, dataset.RefWell, dataset.ID, extended.Chart, extended.Track);
        }

    }//end class
}//end namespace
