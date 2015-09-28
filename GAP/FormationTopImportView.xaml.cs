using Ninject;
using GAP.HelperClasses;
using GAP.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using GAP.MainUI.ViewModels.Helpers;
using GAP.BL;

namespace GAP
{
    /// <summary>
    /// Interaction logic for FormationTopImportView.xaml
    /// </summary>
    public partial class FormationTopImportView 
    {
        public FormationTopImportView()
        {
            InitializeComponent();
            _dataContext = new FormationTopImportViewModel(Token);
            DataContext = _dataContext;
            AddKeyBindings<FormationInfo>();
            SetDialogFilters();
        }
        FormationTopImportViewModel _dataContext;

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.BrowseFiles:
                    BrowseFiles();
                    break;
                case NotificationMessageEnum.FormationsImportedSuccesfully:
                    int formationsCount=Int32.Parse(messageType.MessageObject.ToString());
                    MessageBox.Show(string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("FormationsImportedInSystem"), formationsCount), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                    GlobalData.ShouldSave = true;
                    //FormationTopHelper.ShowHideFormation();
                    Close();
                    break;
            }
        }

        Dictionary<string, string> _dicDialogFilters;
        private void SetDialogFilters()
        {
            _dicDialogFilters = new Dictionary<string, string>();
            //_dicDialogFilters.Add("TXT", "TXT Files (*.txt)|*.txt|All Files (*.*)|*.*");
            _dicDialogFilters.Add("Excel Spreadsheet", "Excel Files (*.xls,*.xlsx)|*.xls*|All Files (*.*)|*.*");
            //_dicDialogFilters.Add("WITSML", "WITSML Files (*.witsml)|*.witsml|All Files (*.*)|*.*");
            //_dicDialogFilters.Add("ASCII", "ASCII Files (*.ascii)|*.ascii|All Files (*.*)|*.*");
            //_dicDialogFilters.Add("LAS", "LAS Files (*.las)|*.las|All Files (*.*)|*.*");
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
