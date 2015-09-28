using System.Collections.Generic;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;
using GAP.BL;
using GAP.HelperClasses;
using System.Windows;
using System;
using Ninject;
using GAP.MainUI.ViewModels.Helpers;
using System.Windows.Controls;
using System.Windows.Input;

namespace GAP
{
    /// <summary>
    /// Interaction logic for LithologyImportData.xaml
    /// </summary>
    public partial class ImportLithologyView
    {
        public ImportLithologyView()
        {
            InitializeComponent();
            _dataContext = new ImportLithologyViewModel(Token);
            SetDialogFilters();
            DataContext = _dataContext;
            AddKeyBindings<BaseEntity>();
            Closed += LithologyImportDataView_Closed;
        }

        void LithologyImportDataView_Closed(object sender, EventArgs e)
        {
            _dataContext = null;
            Closed -= LithologyImportDataView_Closed;
        }

        ImportLithologyViewModel _dataContext;

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.BrowseFiles:
                    BrowseFiles();
                    break;
                case NotificationMessageEnum.LithologiesImportedSuccessfully:
                    int lithologiesCount = Int32.Parse(messageType.MessageObject.ToString());
                    MessageBox.Show(string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("LithologiesImportedInSystem"), lithologiesCount), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                    GlobalData.ShouldSave = true;
                    Close();
                    break;
            }
        }

        Dictionary<string, string> _dicDialogFilters;
        private void SetDialogFilters()
        {
            _dicDialogFilters = new Dictionary<string, string>();
            _dicDialogFilters.Add("Excel Spreadsheet", "Excel Files (*.xls,*.xlsx)|*.xls*|All Files (*.*)|*.*");
            _dicDialogFilters.Add("LAS", "LAS Files (*.las)|*.las|All Files (*.*)|*.*");
        }

        private void BrowseFiles()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Filter = _dicDialogFilters[_dataContext.SelectedDataSource];

            if (dialog.ShowDialog() == true)
            {
                _dataContext.FileName = dialog.FileName;
            }
        }
    }//end class
}//end namespace
