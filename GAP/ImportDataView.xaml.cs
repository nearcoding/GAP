using System.Collections.Generic;
using System.Windows.Documents;
using System.Linq;
using System.Windows;
using Ninject;
using Microsoft.Win32;
using GAP.BL;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;

namespace GAP
{
    public partial class ImportDataView
    {

        public bool isSaved = false;

        public ImportDataView()
        {
           InitializeComponent();

            var projects=HelperMethods.Instance.ProjectsWithWells();
          
            if (!projects.Any())
            {
                _shouldClose = true;
               Loaded += ImportDataView_Loaded;
            }

            _dataContext = new ImportDataViewModel(Token);
            DataContext = _dataContext;
            SetDialogFilters();
            AddKeyBindings<BaseEntity>();
        }

        bool _shouldClose;
        void ImportDataView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_shouldClose)
            {
                MessageBox.Show("No valid project found in the system, Unable to continue the operation", GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Close();
                return;
            }        
        }

        ImportDataViewModel _dataContext;

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.BrowseFiles:
                    BrowseFiles();
                    break;
                case NotificationMessageEnum.OpenDepthImportView:
                    OpenDepthImportScreen();
                    break;
                case NotificationMessageEnum.SaveDatasetImport:
                    SaveDatasetInformation(messageType);
                    break;
            }
        }

        private void SaveDatasetInformation(NotificationMessageType messageType)
        {
            List<Dataset> datasets = messageType.MessageObject as List<Dataset>;
            if (!datasets.Any())
            {
                MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("NoDatasetFoundToImport"),
                    GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
                return;
            }
            //this test ensures we dont have any two datasets with same name from the file where we import data
            if (datasets.GroupBy(u => u.Name).Count() != datasets.Count)
            {
                var grp = datasets.GroupBy(u => u.Name).Where(v => v.Count() > 1).Select(w => w.Key).ToList();
                foreach (var item in grp)
                {
                    int i = 0;
                    //get all the datasets by this name and then add suffix to them
                    foreach (var dataset in datasets.Where(u => u.Name == item))
                    {
                        if (i > 0) dataset.Name = dataset.Name + "_" + i;
                        i += 1;
                    }
                }
            }

            var datasetsWithCurves = datasets.Where(u => u.DepthAndCurves.Any()).ToList();
          
            var datasetImport = new DatasetImportMappingView(datasetsWithCurves);
            datasetImport.ShowDialog();

            if (datasetImport.IsSaved) Close();
        }

        private void OpenDepthImportScreen()
        {
            var automaticDepthImportView = new AverageAndExactOrCloserValueView(_dataContext.InitialDepth, _dataContext.FinalDepth);
            automaticDepthImportView.ShowDialog();

            if (automaticDepthImportView.IsSaved) UpdateViewModelObject(automaticDepthImportView);

            _dataContext.IsDepthImportViewSaved = automaticDepthImportView.IsSaved;
        }

        private void UpdateViewModelObject(AverageAndExactOrCloserValueView automaticDepthImportView)
        {
            var viewModel = automaticDepthImportView.DataContext as AverageAndExactOrCloserValueViewModel;
            _dataContext.InitialDepth = viewModel.InitialDepth;
            _dataContext.FinalDepth = viewModel.FinalDepth;
            _dataContext.Step = viewModel.Step;
            _dataContext.IsArithmeticValueChecked = viewModel.IsArithmeticAverageChecked;
            _dataContext.IsExactOrCloserValueChecked = viewModel.IsArithmeticExactOrCloserValueChecked;
        }

        Dictionary<string, string> _dicDialogFilters;
        private void SetDialogFilters()
        {
            _dicDialogFilters = new Dictionary<string, string>();
            _dicDialogFilters.Add("TXT", "TXT Files (*.txt)|*.txt|All Files (*.*)|*.*");
            _dicDialogFilters.Add("Excel Spreadsheet", "Excel Files (*.xls,*.xlsx)|*.xls*|All Files (*.*)|*.*");
            _dicDialogFilters.Add("WITSML", "XML Files (*.xml)|*.xml");
            _dicDialogFilters.Add("ASCII", "ASCII Files (*.ascii)|*.ascii|All Files (*.*)|*.*");
            _dicDialogFilters.Add("LAS", "LAS Files (*.las)|*.las|All Files (*.*)|*.*");
        }

        private void BrowseFiles()
        {
            var dialog = new OpenFileDialog
            {
                Filter = _dicDialogFilters[_dataContext.SelectedDataSource]
            };

            if (dialog.ShowDialog() == true) _dataContext.FileName = dialog.FileName;
        }
    }//end class
}//end namespace
