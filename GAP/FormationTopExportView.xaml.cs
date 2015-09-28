using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using System.Collections.Generic;

namespace GAP
{  
    public partial class FormationTopExportView 
    {
        public FormationTopExportView()
        {
            InitializeComponent();
            _dataContext = new FormationTopExportViewModel(Token);
            DataContext = _dataContext;
            SetDialogFilters();
            AddKeyBindings<FormationInfo>();
        }
        FormationTopExportViewModel _dataContext;

        Dictionary<string, string> _dicDialogFilters;
        private void SetDialogFilters()
        {
            _dicDialogFilters = new Dictionary<string, string>();
            _dicDialogFilters.Add("Excel Spreadsheet", "Excel Files|*.xlsx");
        }
        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
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
