using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GAP.CustomControls
{
    public class ExtendedDataGrid : DataGrid
    {
        public ExtendedDataGrid()
        {
            IsReadOnly = true;
            CanUserAddRows = false;
            CanUserDeleteRows = false;
            CanUserReorderColumns = false;
            CanUserResizeRows = false;
            AutoGenerateColumns = false;
            CanUserResizeRows = false;
            HeadersVisibility = DataGridHeadersVisibility.Column;
            this.SelectionChanged += ExtendedDataGrid_SelectionChanged;
        }

        void ExtendedDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList = this.SelectedItems;
        }

        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
            DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(ExtendedDataGrid), new PropertyMetadata(null));
        
    }//end class
}//end namespace
