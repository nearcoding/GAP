using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GAP.Helpers;
using System.Linq;
using System.Windows.Input;
using Ninject;
using GAP.MainUI.ViewModels.Helpers;
using GAP.BL;
using GAP.BL.CollectionEntities;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class DataMenuCommands
    {
        public DataMenuCommands(string token)
        {
            _token = token;
        }
        string _token;
        ICommand _dataImportDataCommand;

        private void DataImportData()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.ImportDataset);
        }

        private bool CanDataImportData()
        {
            return GlobalCollection.Instance.Projects.Any(u => u.Wells.Any());
        }

        public ICommand DataImportDataCommand
        {
            get { return _dataImportDataCommand ?? (_dataImportDataCommand = new RelayCommand(DataImportData, CanDataImportData)); }
        }


        ICommand _dataExportDataCommand;

        private void DataExportData()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.ExportDataset);
        }

        private bool CanDataExportData()
        {
            return GlobalCollection.Instance.Projects.Any(u => u.Wells.Any(v => v.Datasets.Any()));
        }

        public ICommand DataExportDataCommand
        {
            get { return _dataExportDataCommand ?? (_dataExportDataCommand = new RelayCommand(DataExportData, CanDataExportData)); }
        }

        ICommand _dataCreateDatasetCommand;

        private void DataCreateDataset()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.AddNewDataset);
        }

        private bool CanDataCreateDataset()
        {
            return GlobalCollection.Instance.Projects.Any(u => u.Wells.Any());
        }

        public ICommand DataCreateDatasetCommand
        {
            get { return _dataCreateDatasetCommand ?? (_dataCreateDatasetCommand = new RelayCommand(DataCreateDataset, CanDataCreateDataset)); }
        }

        ICommand _dataEditDatasetCommand;

        private void DataEditDataset()
        {
            if (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.IsDatasetSelected &&  IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedDataset != null)
                GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.EditDataset);
            else
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(_token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("NoDatasetHasBeenSelected"));
        }

        public ICommand DataEditDatasetCommand
        {
            get { return _dataEditDatasetCommand ?? (_dataEditDatasetCommand = new RelayCommand(DataEditDataset)); }
        }

        ICommand _dataPrintDatasetCommand;
        public ICommand DataPrintDatasetCommand
        {
            get
            {
                return _dataPrintDatasetCommand ?? (_dataPrintDatasetCommand = new RelayCommand(PrintDataset,
                    () =>HelperMethods.Instance.AnyDatasetExist()));
            }
        }
        private void PrintDataset()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.PrintDataset);
        }
    }//end class
}//end namespace
