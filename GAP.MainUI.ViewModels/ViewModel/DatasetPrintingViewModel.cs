using GalaSoft.MvvmLight.Command;
using GAP.BL;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Ninject;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using AutoMapper;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class DatasetPrintingViewModel : BaseViewModel<BaseEntity>
    {
        public DatasetPrintingViewModel(string token)
            : base(token)
        {
            Title = IoC.Kernel.Get<IResourceHelper>().ReadResource("PrintDataset");
            var projects = GlobalCollection.Instance.Projects.ToList();
            Projects = new List<Project>();
            projects = projects.Where(u => u.Wells.Any(v => v.Datasets.Any())).ToList();
            Mapper.CreateMap<Dataset, Dataset>();
            foreach (var project in projects)
            {
                var projectObject = new Project
                {
                    Name = project.Name,
                    ID=project.ID,
                    Wells = new ExtendedBindingList<Well>()
                };
                projectObject.OnEntitySelectionChanged += project_OnEntitySelectionChanged;
                foreach (var well in project.Wells)
                {
                     var wellObject = new Well
                    {
                        RefProject = well.RefProject,
                        Name = well.Name,
                        ID=well.ID,
                        Datasets = new ExtendedBindingList<Dataset>()
                    };
                    wellObject.OnEntitySelectionChanged += well_OnEntitySelectionChanged;
                    foreach (var dataset in well.Datasets)
                    {
                        var datasetObject = (Dataset)Mapper.Map(dataset, typeof(Dataset), typeof(Dataset));
                        datasetObject.OnEntitySelectionChanged += dataset_OnEntitySelectionChanged;
                        wellObject.Datasets.Add(datasetObject);
                    }
                    projectObject.Wells.Add(wellObject);
                }
                Projects.Add(projectObject);
            }
        }
        ICommand _printerSettingsCommand;
        public ICommand PrinterSettingsCommand
        {
            get { return _printerSettingsCommand ?? (_printerSettingsCommand = new RelayCommand(PrinterSettings)); }
        }
        private void PrinterSettings()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.PrinterSettings);
        }
        private void UpdateProjectFromWell(Well well)
        {
            var project = Projects.SingleOrDefault(u => u.ID == well.RefProject);
            project.OnEntitySelectionChanged -= project_OnEntitySelectionChanged;
            if (project.Wells.All(u => u.IsEntitySelected.HasValue))
            {
                if (project.Wells.All(u => u.IsEntitySelected.Value))
                    project.IsEntitySelected = true;
                else if (project.Wells.All(u => u.IsEntitySelected.Value == false))
                    project.IsEntitySelected = false;
                else
                    project.IsEntitySelected = null;
            }
            else
            {
                project.IsEntitySelected = null;
            }
            project.OnEntitySelectionChanged += project_OnEntitySelectionChanged;
        }

        void dataset_OnEntitySelectionChanged(BaseEntity entity)
        {
            var dataset = entity as Dataset;
            var well = Projects.SingleOrDefault(u => u.ID == dataset.RefProject).Wells.SingleOrDefault(v => v.ID == dataset.RefWell);
            // HelperMethods.Instance.GetWellByProjectNameAndWellName(dataset.RefProject, dataset.RefWell);
            well.OnEntitySelectionChanged -= well_OnEntitySelectionChanged;
            if (well.Datasets.All(u => u.IsEntitySelected.HasValue))
            {
                if (well.Datasets.All(u => u.IsEntitySelected.Value))
                    well.IsEntitySelected = true;
                else if (well.Datasets.All(u => u.IsEntitySelected.Value == false))
                    well.IsEntitySelected = false;
                else
                    well.IsEntitySelected = null;
            }
            else
                well.IsEntitySelected = null;

            UpdateProjectFromWell(well);
            well.OnEntitySelectionChanged += well_OnEntitySelectionChanged;
        }

        private void UpdateWellFromDataset(Dataset dataset)
        {
            var well = Projects.SingleOrDefault(u => u.ID == dataset.RefProject).Wells.SingleOrDefault(v => v.ID == dataset.RefWell);
            well.OnEntitySelectionChanged -= well_OnEntitySelectionChanged;
            if (well.Datasets.All(u => u.IsEntitySelected.HasValue))
            {
                if (well.Datasets.All(u => u.IsEntitySelected.Value))
                    well.IsEntitySelected = true;
                else if (well.Datasets.All(u => u.IsEntitySelected.Value == false))
                    well.IsEntitySelected = false;
                else
                    well.IsEntitySelected = null;
            }
            else
                well.IsEntitySelected = null;
            well.OnEntitySelectionChanged += well_OnEntitySelectionChanged;
        }

        void well_OnEntitySelectionChanged(BaseEntity entity)
        {
            var well = entity as Well;
            var project = Projects.SingleOrDefault(u => u.ID == well.RefProject);

            foreach (var dataset in well.Datasets)
            {
                dataset.OnEntitySelectionChanged -= dataset_OnEntitySelectionChanged;
                dataset.IsEntitySelected = well.IsEntitySelected;
                dataset.OnEntitySelectionChanged += dataset_OnEntitySelectionChanged;
            }
            project.OnEntitySelectionChanged -= project_OnEntitySelectionChanged;
            if (project.Wells.All(u => u.IsEntitySelected.HasValue))
            {
                if (project.Wells.All(u => u.IsEntitySelected.Value))
                    project.IsEntitySelected = true;
                else if (project.Wells.All(u => u.IsEntitySelected.Value == false))
                    project.IsEntitySelected = false;
                else
                    project.IsEntitySelected = null;
            }
            else
                project.IsEntitySelected = null;
            project.OnEntitySelectionChanged += project_OnEntitySelectionChanged;
        }

        void project_OnEntitySelectionChanged(BaseEntity entity)
        {
            foreach (var well in (entity as Project).Wells)
            {
                well.OnEntitySelectionChanged -= well_OnEntitySelectionChanged;
                well.IsEntitySelected = entity.IsEntitySelected;
                foreach(var dataset in well.Datasets)
                {
                    dataset.OnEntitySelectionChanged -= dataset_OnEntitySelectionChanged;
                    dataset.IsEntitySelected = entity.IsEntitySelected;
                    dataset.OnEntitySelectionChanged += dataset_OnEntitySelectionChanged;
                }
                well.OnEntitySelectionChanged += well_OnEntitySelectionChanged;
            }
        }

        public List<Project> Projects { get; set; }

        ICommand _printDatasetCommand, _printPreviewDatasetCommand;
        public ICommand PrintDatasetCommand
        {
            get
            {
                return _printDatasetCommand ?? (_printDatasetCommand = new RelayCommand(PrintDataset, () => GetSelectedDatasets().Any()));
            }
        }

        public bool IncludeSpreadsheetData { get; set; }

        private IEnumerable<Dataset> GetSelectedDatasets()
        {
            return Projects.SelectMany(u => u.Wells.SelectMany(v => v.Datasets.Where(w => w.IsEntitySelected.Value)));
        }

        private void PrintDataset()
        {
            using (new WaitCursor())
            {
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.PrintDataset, GetSelectedDatasets());
            }
        }

        public ICommand PrintPreviewDatasetCommand
        {
            get
            {
                return _printPreviewDatasetCommand ?? (_printPreviewDatasetCommand = new RelayCommand(PrintPreviewDataset,()=>GetSelectedDatasets().Any()));
            }
        }

        private void PrintPreviewDataset()
        {
            using (new WaitCursor())
            {
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.PrintPreviewDataset, GetSelectedDatasets());
            }
        }
    }//end class
}//end namespace
