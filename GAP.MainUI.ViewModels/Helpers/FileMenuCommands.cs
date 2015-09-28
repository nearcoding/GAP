using System.Linq;
using System.Windows.Input;
using Ninject;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class FileMenuCommands
    {
        string _token;
        public FileMenuCommands(string token)
        {
            _token = token;
        }

        ICommand _fileNewProjectCommand;
        ICommand _fileLoadFileCommand;
        ICommand _fileNewWellCommand;
        ICommand _fileSaveProjectCommand;
        ICommand _fileSaveProjectAsCommand;
        ICommand _filePrintPreviewCommand;
        ICommand _filePrintCommand;
        ICommand _fileProjectPropertiesCommand;
        ICommand _fileWellPropertiesCommand;
        ICommand _fileExitCommand;
        ICommand _fileEditProjectCommand;
        ICommand _fileDeleteProjectCommand;
        ICommand _fileEditWellCommand;
        ICommand _fileDeleteWellCommand;
        ICommand _fileEditDatasetCommand;
        ICommand _fileDeleteDatasetCommand;
        ICommand _fileDeleteSubDatasetCommand;
        ICommand _fileEditSpreadsheetCommand;
        public ICommand FileEditDatasetCommand
        {
            get
            {
                return _fileEditDatasetCommand ?? (_fileEditDatasetCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.EditDataset)));
            }
        }

        ICommand _fileEditSubDatasetSpreadsheetCommand;
        public ICommand FileEditSubDatasetSpreadsheetCommand
        {
            get { return _fileEditSubDatasetSpreadsheetCommand ?? (_fileEditSubDatasetSpreadsheetCommand = new RelayCommand(FileEditSubDatasetSpreadsheet)); }
        }

        private void FileEditSubDatasetSpreadsheet()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.EditSubdatasetSpreadsheet);
        }

        public ICommand FileEditSpreadsheetCommand
        {
            get
            {
                return _fileEditSpreadsheetCommand ?? (_fileEditSpreadsheetCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.EditSpreadsheet)));
            }
        }

        ICommand _fileEditSubDatasetCommand;
        public ICommand FileEditSubDatasetCommand
        {
            get { return _fileEditSubDatasetCommand ?? (_fileEditSubDatasetCommand = new RelayCommand(FileEditSubDataset)); }
        }

        private void FileEditSubDataset()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.EditSubdataset);
        }

        public ICommand FileDeleteSubDatasetCommand
        {
            get
            {
                return _fileDeleteSubDatasetCommand ?? (_fileDeleteSubDatasetCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.DeleteSubDataset)));
            }
        }
        public ICommand FileDeleteDatasetCommand
        {
            get
            {
                return _fileDeleteDatasetCommand ?? (_fileDeleteDatasetCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.DeleteDataset)));
            }
        }

        public ICommand FileDeleteWellCommand
        {
            //can delete well has same requirements as of can delete well
            get
            {
                return _fileDeleteWellCommand ?? (_fileDeleteWellCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.DeleteWell), () => CanEditWell()));
            }
        }

        private void SendMessage(NotificationMessageEnum message)
        {
            GlobalDataModel.Instance.SendMessage(_token, message);
        }

        public ICommand FileDeleteProjectCommand
        {
            get
            {
                return _fileDeleteProjectCommand ?? (_fileDeleteProjectCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.DeleteProject)));
            }
        }

        public ICommand FileNewProjectCommand
        {
            get
            {
                return _fileNewProjectCommand ?? (_fileNewProjectCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.AddNewProject)));
            }
        }

        public ICommand FileEditWellCommand
        {
            get
            {
                return _fileEditWellCommand ?? (_fileEditWellCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.EditWell), () => CanEditWell()));
            }
        }

        private bool CanEditWell()
        {
            return SelectedWell != null && !string.IsNullOrWhiteSpace(SelectedWell.Name);
        }

        public Well SelectedWell { get; set; }

        public ICommand FileEditProjectCommand
        {
            get
            {
                return _fileEditProjectCommand ?? (_fileEditProjectCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.EditProject)));
            }
        }

        public ICommand FileNewWellCommand
        {
            get
            {
                return _fileNewWellCommand ?? (_fileNewWellCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.AddNewWell), () => GlobalCollection.Instance.Projects.Any()));
            }
        }

        public ICommand FileLoadFileCommand
        {
            get
            {
                return _fileLoadFileCommand ?? (_fileLoadFileCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.LoadProject)));
            }
        }

        public ICommand FileSaveProjectCommand
        {
            get
            {
                return _fileSaveProjectCommand ?? (_fileSaveProjectCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.SaveProject), () => GlobalDataModel.ShouldSave ));
            }
        }

        public ICommand FileSaveProjectAsCommand
        {
            get
            {
                return _fileSaveProjectAsCommand ?? (_fileSaveProjectAsCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.SaveProjectAs), () => GlobalCollection.Instance.Projects.Any()));
            }
        }

        public ICommand FilePrintPreviewCommand
        {
            get { return _filePrintPreviewCommand ?? (_filePrintPreviewCommand = new RelayCommand(() => { }, () => false)); }
        }

        public ICommand FilePrintCommand
        {
            get { return _filePrintCommand ?? (_filePrintCommand = new RelayCommand(() => { }, () => false)); }
        }

        public ICommand FileProjectPropertiesCommand
        {
            get
            {
                return _fileProjectPropertiesCommand ?? (_fileProjectPropertiesCommand
                    = new RelayCommand(() => { }, () => false));
            }
        }

        public ICommand FileWellPropertiesCommand
        {
            get
            {
                return _fileWellPropertiesCommand ?? (_fileWellPropertiesCommand
                    = new RelayCommand(() => { }, () => false));
            }
        }

        public ICommand FileExitCommand
        {
            get
            {
                return _fileExitCommand ?? (_fileExitCommand
                    = new RelayCommand(() => SendMessage(NotificationMessageEnum.ExitApplication)));
            }
        }
    }//end class
}//end namespace
