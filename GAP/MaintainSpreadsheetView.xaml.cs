using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using GAP.BL;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;
using System.Windows;
using Ninject;
using GAP.MainUI.ViewModels.Helpers;
using GAP.ExtendedControls;
namespace GAP
{
    public partial class MaintainSpreadsheetView : BaseWindow
    {
        bool _isEditEndingEnded = true;

        private void ExportToExcel()
        {
            BrowseFiles();
            if (string.IsNullOrWhiteSpace(FileName))
            {
                MessageBox.Show(
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("SelectExcelFileToCopyData"), GlobalData.MESSAGEBOXTITLE);
                return;
            }
            var lst = _dataContext.CurrentObject.DepthAndCurves.Select(u => new DepthCurveClass
                {
                    Depth = u.Depth,
                    Curve = u.Curve
                }).ToList();
            CreateExcelFile.CreateExcelDocument<DepthCurveClass>(lst, FileName);
            MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("DataExportedToExcelSuccessfully")
                , GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public class DepthCurveClass
        {
            public decimal Depth { get; set; }
            public decimal Curve { get; set; }
        }

        #region Constructors

        public MaintainSpreadsheetView(Dataset dataSet, bool isEdit)
        {
            InitializeComponent();

            _dataContext = new MaintainSpreadsheetViewModel(Token, dataSet, isEdit);
            DataContext = _dataContext;
            AddKeyBindings<Dataset>();
            Closing += MaintainSpreadsheetView_Closing;
        }

        void MaintainSpreadsheetView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= MaintainSpreadsheetView_Closing;
            _dataContext = null;
            DataContext = null;//both lines required not only for memory leaks but also for a bug from CanSave method
            //#todo verify can save method by replacing closing method with closed
        }

        #endregion

        MaintainSpreadsheetViewModel _dataContext;
        private void BrowseFiles()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();

            dialog.Filter = "Excel Spreadsheet,Excel Files|*.xlsx";

            if (dialog.ShowDialog() == true)
            {
                FileName = dialog.FileName;
            }
        }

        public string FileName { get; set; }

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.OpenAutomaticDepthScreen:
                    OpenAutomaticDepthScreen();
                    break;
                case NotificationMessageEnum.SaveSpreadsheet:
                    IsSaved = true;
                    Close();
                    break;
                case NotificationMessageEnum.SpreadsheetExcel:
                    ExportToExcel();
                    break;
                case NotificationMessageEnum.CancelScreen:
                    var result = MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("SaveChangesBeforeClosingTheScreen"),
                      GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes) _dataContext.AutoSave = true;
                    break;
            }
        }

        private void OpenAutomaticDepthScreen()
        {
            var view = new SpreadsheetSettingsView(_dataContext.CurrentObject);
            view.ShowDialog();

            if (view.IsSaved)
            {
                _dataContext.CurrentObject.DepthAndCurves.Clear();
                DataGrid1.CanUserAddRows = true;
                AddItemsToGridBasedOnSettings();
            }
        }

        private void AddItemsToGridBasedOnSettings()
        {
            for (decimal i = _dataContext.CurrentObject.InitialDepth; i <= _dataContext.CurrentObject.FinalDepth; i = i + _dataContext.CurrentObject.Step)
            {
                _dataContext.CurrentObject.DepthAndCurves.Add(new DepthCurveInfo
                {
                    Depth = i
                });
            }
        }

        //private void dataGrid1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{            
        //    if ((sender as DataGrid).SelectedIndex == -1)
        //        return;
        //    //it will generate the new rows
        //  //  _dataContext.DataGridSelectionChanged();
        //    List<DepthCurveInfo> selectedObjects = (sender as DataGrid).SelectedItems.OfType<DepthCurveInfo>().ToList();
        //    _dataContext.SelectedItems = selectedObjects;
        //}

        private void DataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (_isEditEndingEnded)
            {
                if (e.Column.DisplayIndex == 0)
                    EditingDepthCell(sender, e);
                else if (e.Column.DisplayIndex == 1)
                    EditingValueCell(sender, e);
            }
        }

        private void EditingValueCell(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!ClearValueCellIfDepthIsZero(sender, e)) return;

            var value = e.EditingElement as TextBox;
            decimal decValue = 0;
            if (!decimal.TryParse(value.Text.ToString(), out decValue))
                CheckForNonDecimalValue(sender, e);
        }

        private bool ClearValueCellIfDepthIsZero(object sender, DataGridCellEditEndingEventArgs e)
        {
            if ((e.Row as DataGridRow).Item != null && (e.Row as DataGridRow).Item as DepthCurveInfo != null)
            {
                var curveInfo = (e.Row as DataGridRow).Item as DepthCurveInfo;

                if (curveInfo.Depth <= 0)
                {
                    _isEditEndingEnded = false;
                    e.Cancel = true;
                    (sender as DataGrid).CancelEdit(DataGridEditingUnit.Cell);
                    _isEditEndingEnded = true;
                    return false;
                }
            }
            return true;
        }

        private void CheckValueForMinMaxRange(object sender, DataGridCellEditEndingEventArgs e, decimal decValue)
        {
            if ((decValue < _dataContext.CurrentObject.MinUnitValue || decValue > _dataContext.CurrentObject.MaxUnitValue)
                && (decValue != -999 && decValue != decimal.Parse("-999.25")))
            {
                _isEditEndingEnded = false;
                e.Cancel = true;
                (sender as DataGrid).CancelEdit(DataGridEditingUnit.Cell);
                _isEditEndingEnded = true;
            }
        }

        private void EditingDepthCell(object sender, DataGridCellEditEndingEventArgs e)
        {
            var value = e.EditingElement as TextBox;
            decimal decValue = 0;
            if (decimal.TryParse(value.Text.ToString(), out decValue))
                CheckForDuplicateValue(sender, e, decValue);
            else
                CheckForNonDecimalValue(sender, e);
        }

        private void CheckForNonDecimalValue(object sender, DataGridCellEditEndingEventArgs e)
        {
            _isEditEndingEnded = false;
            e.Cancel = true;
            (sender as DataGrid).CancelEdit(DataGridEditingUnit.Cell);
            _isEditEndingEnded = true;
        }

        private void CheckForDuplicateValue(object sender, DataGridCellEditEndingEventArgs e, decimal decValue)
        {
            if (_dataContext.CurrentObject.DepthAndCurves.Any(u => u.Depth == decValue))
            {
                _isEditEndingEnded = false;
                e.Cancel = true;
                (sender as DataGrid).CancelEdit(DataGridEditingUnit.Cell);
                _isEditEndingEnded = true;
            }
        }
    }//end class
}//end namespace