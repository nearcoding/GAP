using GAP.BL;
using GAP.ExtendedControls;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;
using System.Collections.Generic;

namespace GAP
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class ExportDataView 
    {

        #region Internal Variables

        public bool isSaved = false;

        #endregion

        #region Constructurs

        public ExportDataView()
        {
            InitializeComponent();
            _dataContext = new ExportDataViewModel(Token);
            DataContext = _dataContext;
            SetDialogFilters();
            AddKeyBindings<Dataset>();
        }

        ExportDataViewModel _dataContext;
        #endregion

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.BrowseFiles:
                    BrowseFiles();
                    break;
            }
        }


        Dictionary<string, string> _dicDialogFilters;
        private void SetDialogFilters()
        {
            _dicDialogFilters = new Dictionary<string, string>();
            _dicDialogFilters.Add("TXT", "TXT Files (*.txt)|*.txt|All Files (*.*)|*.*");
            _dicDialogFilters.Add("Excel Spreadsheet", "Excel Files (*.xls,*.xlsx)|*.xls*|All Files (*.*)|*.*");
            //_dicDialogFilters.Add("WITSML", "WITSML Files (*.witsml)|*.witsml|All Files (*.*)|*.*");
            //_dicDialogFilters.Add("ASCII", "ASCII Files (*.ascii)|*.ascii|All Files (*.*)|*.*");
            //_dicDialogFilters.Add("LAS", "LAS Files (*.las)|*.las|All Files (*.*)|*.*");
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

    }
}
