using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using System.Collections.Generic;

namespace GAP
{
    /// <summary>
    /// Interaction logic for LithologyExportDataView.xaml
    /// </summary>
    public partial class LithologyExportDataView
    {
        LithologyExportDataViewModel _dataContext;
        public LithologyExportDataView()
        {
            InitializeComponent();
            _dataContext = new LithologyExportDataViewModel(Token);
            DataContext = _dataContext;
            AddKeyBindings<BaseEntity>();
            SetDialogFilters();
        }

        Dictionary<string, string> _dicDialogFilters;
        private void SetDialogFilters()
        {
            _dicDialogFilters = new Dictionary<string, string> {{"Excel Spreadsheet", "Excel Files|*.xlsx"}};
            //_dicDialogFilters.Add("TXT", "TXT Files (*.txt)|*.txt|All Files (*.*)|*.*");
            //_dicDialogFilters.Add("WITSML", "WITSML Files (*.witsml)|*.witsml|All Files (*.*)|*.*");
            //_dicDialogFilters.Add("ASCII", "ASCII Files (*.ascii)|*.ascii|All Files (*.*)|*.*");
            //_dicDialogFilters.Add("LAS", "LAS Files (*.las)|*.las|All Files (*.*)|*.*");
        }
        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch(messageType.MessageType)
            {
                case NotificationMessageEnum.BrowseFiles:
                    BrowseFiles();
                    break;
            }
        }

        private void BrowseFiles()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();

            dialog.Filter = _dicDialogFilters[_dataContext.SelectedDataSource];

            if (dialog.ShowDialog() == true)
            {
                _dataContext.FileName = dialog.FileName;
            }
        }
    }//end class
}//end namespace
