using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GAP.BL;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using System.Collections.Generic;

namespace GAP.Custom_Controls
{
    /// <summary>
    /// Interaction logic for ucSolutionExplorer.xaml
    /// </summary>
    public partial class ucSolutionExplorer : UserControl
    {
        public ucSolutionExplorer()
        {
            InitializeComponent();
            TreeViewWidth = TreeViewOfProjects.ActualWidth;
            IsShowing = true;
            _lstMouseMoveItems = new List<TreeViewItem>();
        }

        void ucSolutionExplorer_Loaded(object sender, RoutedEventArgs e)
        {
            _dataContext = DataContext as IMainScreenViewModel;
        }
        public TreeViewItem CurrentTreeViewItem { get; set; }

        public bool IsShowing { get; set; }

        IMainScreenViewModel _dataContext;
        private void TreeViewOfProjects_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (_dataContext == null) return;
            _dataContext.IsDatasetSelected = false;
            _dataContext.IsSubDatasetSelected = false;
            var treeViewControl = sender as TreeView;
            if (treeViewControl == null || treeViewControl.SelectedItem == null) return;

            if (treeViewControl.SelectedItem.GetType() == typeof(Project))
                _dataContext.SelectedProject = treeViewControl.SelectedItem as Project;
            else if (treeViewControl.SelectedItem.GetType() == typeof(Well))
                _dataContext.FileMenu.SelectedWell = treeViewControl.SelectedItem as Well;
            else if (treeViewControl.SelectedItem.GetType() == typeof(Dataset))
            {
                _dataContext.IsDatasetSelected = true;
                _dataContext.SelectedDataset = treeViewControl.SelectedItem as Dataset;
                AddDragDropToTreeViewItem(CurrentTreeViewItem);
            }
            else if (treeViewControl.SelectedItem.GetType() == typeof(SubDataset))
            {
                _dataContext.IsSubDatasetSelected = true;
                _dataContext.SelectedSubDataset = treeViewControl.SelectedItem as SubDataset;
                // GlobalData.AddDragDropToTreeViewItem(CurrentTreeViewItem);
            }
        }

        Point startPoint;

        TreeView _treeViewOfProjects;
        public TreeView TreeViewOfProjects
        {
            get
            {
                if (_treeViewOfProjects == null) _treeViewOfProjects = TreeView1;
                return _treeViewOfProjects;
            }
            set
            {
                _treeViewOfProjects = value;
            }
        }
        private Double TreeViewWidth { get; set; }
        void TreeViewOfProjects_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        void TreeViewOfProjects_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var mousePos = e.GetPosition(null);
            var diff = startPoint - mousePos;
            if (e.OriginalSource.GetType() == typeof(System.Windows.Documents.Run))
                return;
            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                // Get the dragged ListViewItem
                var listView = sender as TreeView;
                var listViewItem =
                   GlobalData.FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
                CurrentTreeViewItem = listViewItem;
            }
        }

        void TreeViewOfProjects_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(System.Windows.Documents.Run))
                return;
            var item = GlobalData.FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);

            if (item == null) return;

            if (item.Header.GetType() == typeof(Project))
                _dataContext.SelectedProject = item.Header as Project;
            else if (item.Header.GetType() == typeof(Well))
                _dataContext.FileMenu.SelectedWell = item.Header as Well;
            else if (item.Header.GetType() == typeof(Dataset))
            {
                _dataContext.SelectedDataset = item.Header as Dataset;
                AddDragDropToTreeViewItem(CurrentTreeViewItem);
            }
            else if (item.Header.GetType()==typeof(SubDataset))
                _dataContext.SelectedSubDataset=item.Header as SubDataset;
        }

        List<TreeViewItem> _lstMouseMoveItems = new List<TreeViewItem>();
        void AddDragDropToTreeViewItem(TreeViewItem item)
        {
            if (item != null)
            {
                item.PreviewMouseMove -= item_PreviewMouseMove;
                item.PreviewMouseMove += item_PreviewMouseMove;
                if (!_lstMouseMoveItems.Contains(item)) _lstMouseMoveItems.Add(item);
            }
        }
        public void DetachEvents()
        {
            foreach(var item in _lstMouseMoveItems.ToList())
            {
                item.PreviewMouseMove -= item_PreviewMouseMove;
            }

            _lstMouseMoveItems.Clear();
        }
        void item_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var treeViewItem = GlobalData.FindAncestor<TreeViewItem>((DependencyObject)e.Source);

            if (treeViewItem == null) return;

            if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(treeViewItem, treeViewItem, DragDropEffects.Link);
            }
        }
        private void TreeViewOfProjects_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TreeViewWidth = TreeViewOfProjects.ActualWidth;
        }
    }//end class
}//end namespace
