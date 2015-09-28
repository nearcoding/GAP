using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninject;
using AutoMapper;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using System;
using System.Text;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class DatasetImportMappingViewModel : DatasetBaseViewModel
    {
        ICommand _importCommand;
        ObservableCollection<DatasetImportInfo> _importedDatasets;
        DatasetImportInfo _selectedDataset;
        public DatasetImportMappingViewModel(string token, List<Dataset> importedDataList)
            : base(token)
        {
            Mapper.CreateMap<Dataset, Dataset>();
            if (importedDataList == null || !importedDataList.Any())
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "No dataset found to import");
                return;
            }
            AcceptedDatasetList = new List<Dataset>();
            ImportedDatasetList = new ObservableCollection<DatasetImportInfo>();
            importedDataList.ForEach(dataset =>
                    {
                        ImportedDatasetList.Add(new DatasetImportInfo
                        {
                            Dataset = dataset
                        });
                    });

            InitializeViewModel();
            SelectedDataset = ImportedDatasetList.First();
        }


        ICommand _saveAllPropertiesCommand;
        public ICommand SaveAllPropertiesCommand
        {
            get
            {
                return _saveAllPropertiesCommand ?? (_saveAllPropertiesCommand = new RelayCommand(SaveAllProperties));
            }
        }

        private void SaveAllProperties()
        {
            using (new WaitCursor())
            {
                foreach (var dataset in ImportedDatasetList)
                {
                    if (!AcceptedDatasetList.Any(u => u.ID == dataset.Dataset.ID))
                    {
                        SelectedDataset = dataset;
                        SaveDataset();
                    }
                }
            }
        }

        protected override bool CommonValidation()
        {
            foreach (var dataset in AcceptedDatasetList)
            {
                var wellObject = HelperMethods.Instance.GetWellByID(dataset.RefWell);
                if (wellObject.Datasets.Any(u => u.Name == dataset.Name))
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, string.Format("Dataset {0} already exists in the system", dataset.Name));
                    return false;
                }
                if (!CurrentObject.IsValidInput(CurrentObject.Name))
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, string.Format("No special characters allowed. {0} is not a valid input", CurrentObject.Name));
                    return false;
                }
            }
            return true;
        }

        protected override bool AddObjectValidation()
        {
            throw new System.NotImplementedException();
        }

        protected override bool UpdateObjectValidation()
        {
            throw new System.NotImplementedException();
        }

        private void InitializeViewModel()
        {
            Projects = HelperMethods.Instance.ProjectsWithWells();
            if (!Projects.Any()) return;
            SelectedProject = Projects.First();

            SaveButtonText = "Save Properties[Ctrl + S]";
            IsNew = true;
            CancelButtonVisible = false;
            TVDSelected = true;
        }

        ICommand _saveDatasetCommand;
        public ICommand SaveDatasetCommand
        {
            get { return _saveDatasetCommand ?? (_saveDatasetCommand = new RelayCommand(SaveDataset)); }
        }

        //this method will be executed when user click on Add Dataset
        private void SaveDataset()
        {
            //check if the name in the selected well and project is unique
            if (!IsDatasetNameUniqueForAdding()) return;
         
            var newDatasetObjectByAutoMapper = Mapper.Map<Dataset>(SelectedDataset.Dataset);
            SelectedDataset.IsAccepted = true;
            if (AcceptedDatasetList.Any(u => u.ID == SelectedDataset.Dataset.ID))
            {
                var datasetObject = AcceptedDatasetList.Single(u => u.ID == SelectedDataset.Dataset.ID);
                AcceptedDatasetList.Remove(datasetObject);
            }
            MinMaxValidation(SelectedDataset.Dataset);
            AcceptedDatasetList.Add(newDatasetObjectByAutoMapper);
        }

        public List<Dataset> AcceptedDatasetList { get; set; }

        private void MinMaxValidation(Dataset dataset)
        {
            var lstItems = dataset.DepthAndCurves;
            if (lstItems.Any(u => u.Curve < dataset.MinUnitValue || u.Curve > dataset.MaxUnitValue))
            {
                var remainingList = lstItems.Where(u => u.Curve < dataset.MinUnitValue || u.Curve > dataset.MaxUnitValue).Select(v => v.Depth);
                if (remainingList.Any())
                    GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.UpdateDatasetWithMinMaxWarning);
            }
            else
                UpdateListBoxWithMarkDatasetsFromView();
        }

        public void UpdateListBoxWithMarkDatasetsFromView()
        {
            var selectedItem = ImportedDatasetList.SingleOrDefault(u => u.Dataset.ID == SelectedDataset.Dataset.ID);
            if (selectedItem == null) return;

            selectedItem.IsAccepted = true;
            NotifyPropertyChanged("ImportedDatasetList");
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.RefreshListbox);
        }

        protected override bool CanSave()
        {
            return SelectedDataset != null && SelectedDataset.Dataset != null &&
                !string.IsNullOrWhiteSpace(SelectedDataset.Dataset.Name) &&
                  !string.IsNullOrWhiteSpace(CurrentObject.RefWell) &&
                 !string.IsNullOrWhiteSpace(CurrentObject.RefProject);
        }

        private void DatasetSelectionChanged()
        {
            if (SelectedDataset == null) return;
            CurrentObject = SelectedDataset.Dataset;
            if (AcceptedDatasetList.Any(u => u.ID == CurrentObject.ID))
            {
                //because if user go to the already accepted dataset, its properties like color and backcolor should be updated
                var dataset = AcceptedDatasetList.Single(u => u.ID == CurrentObject.ID);
                Mapper.Map(dataset, CurrentObject);
            }
            else
            {
                if (CurrentObject == null) CurrentObject = new Dataset();

                CurrentObject.Name = SelectedDataset.Dataset.Name;
                CurrentObject.DepthAndCurves = SelectedDataset.Dataset.DepthAndCurves;
                CurrentObject.InitialDepth = decimal.Parse(SelectedDataset.Dataset.DepthAndCurves.Min(u => u.Depth).ToString());
                CurrentObject.FinalDepth = decimal.Parse(SelectedDataset.Dataset.DepthAndCurves.Max(u => u.Depth).ToString());
                SelectedDataset.Dataset.RefProject = SelectedProject.ID;
                SelectedDataset.Dataset.RefWell = SelectedWell.ID;
                SelectedDataset.Dataset.Units = SelectedUnit;

                SelectedDataset.Dataset.LineStyle = 1;
                SelectedDataset.Dataset.FinalDepth = 20;
                SelectedDataset.Dataset.Step = 1;
                SelectedDataset.Dataset.InitialDepth = 1;

                SelectedDataset.Dataset.LineColor = Color.FromArgb(255, 0, 0, 0);
                SelectedDataset.Dataset.MarkerColor = Color.FromArgb(255, 0, 0, 0);
                SelectedDataset.Dataset.ShouldApplyBorderColor = false;

                SelectedDataset.Dataset.MinUnitValue = SelectedDataset.Dataset.DepthAndCurves.Min(u => u.Curve);
                SelectedDataset.Dataset.MaxUnitValue = SelectedDataset.Dataset.DepthAndCurves.Max(u => u.Curve);

                if (SelectedFamily == null && Families.Any())
                    SelectedFamily = Families.First();

                SelectedDataset.Dataset.Family = SelectedFamily.FamilyName;
            }

            NotifyPropertyChanged("CurrentObject");
        }

        bool _tvdSelected;
        public new bool TVDSelected
        {
            get { return _tvdSelected; }
            set
            {
                _tvdSelected = value;
                if (SelectedDataset == null) return;
                SelectedDataset.Dataset.IsTVD = _tvdSelected;
                NotifyPropertyChanged("TVDSelected");
            }
        }

        private void ImportDatasets()
        {
            using (new WaitCursor())
            {
                if (!CommonValidation()) return;
                foreach (var dataset in AcceptedDatasetList)
                {
                    var wellObject = HelperMethods.Instance.GetWellByID(dataset.RefWell);

                    var lst = GlobalDataModel.Instance.GetValidListOfDepthAndCurves(dataset.DepthAndCurves);
                    var sBuilder = new StringBuilder();
                    sBuilder.AppendLine(GlobalDataModel.Instance.GetNotesFirstAndLastValidData(lst));
                    switch(GlobalDataModel.Instance.ImportDataType)
                    {
                        case ImportDataType.AverageChecked:
                            sBuilder.Append("Dataset imported : Arithmetic Average Value");
                            break;
                        case ImportDataType.ExactOrCloserValue:
                            sBuilder.Append("Dataset imported : Exact or Closer Value");
                            break;
                        case ImportDataType.ExactValue:
                            sBuilder.Append("Dataset imported : Exact value");
                            break;
                    }
                    
                    dataset.SystemNotes = sBuilder.ToString();
                    dataset.DisplayIndex = wellObject.Datasets.Count;
                    wellObject.Datasets.Add(dataset);
                }
            }
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.ImportDataset);
        }

        private bool CanImport()
        {
            return ImportedDatasetList.All(u => u.IsAccepted);
        }

        public DatasetImportInfo SelectedDataset
        {
            get { return _selectedDataset; }
            set
            {
                if (_selectedDataset == value) return;
                _selectedDataset = value;
                DatasetSelectionChanged();
                NotifyPropertyChanged("SelectedDataset");
            }
        }

        public string SaveButtonText { get; set; }
        public ObservableCollection<DatasetImportInfo> ImportedDatasetList
        {
            get { return _importedDatasets; }
            set
            {
                _importedDatasets = value;
                NotifyPropertyChanged("ImportedDatasetList");
            }
        }

        public ICommand ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new RelayCommand(ImportDatasets, CanImport)); }
        }
    }//end class
}//end namespace
