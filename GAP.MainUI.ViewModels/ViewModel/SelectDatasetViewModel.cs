using GalaSoft.MvvmLight.Command;
using GAP.BL;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Ninject;
using GAP.Helpers;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class SelectDatasetViewModel : BaseViewModel<BaseEntity>
    {
        public SelectDatasetViewModel(string token)
            : base(token)
        {
            Projects = HelperMethods.Instance.GetProjectsWithDatasets(u=>u.Units=="API");

            if (Projects.Any())
                SelectedProject = Projects.First();
        }

        IEnumerable<Project> _projects;
        public IEnumerable<Project> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                NotifyPropertyChanged("Projects");
            }
        }

        IEnumerable<Well> _wells;
        public IEnumerable<Well> Wells
        {
            get { return _wells; }
            set
            {
                _wells = value;
                NotifyPropertyChanged("Wells");
            }
        }

        Project _selectedProject;
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                if (value == null)
                {
                    Wells = null;
                    return;
                }
                Wells = SelectedProject.Wells;
                if (Wells.Any())
                    SelectedWell = Wells.First();
                NotifyPropertyChanged("SelectedProject");
            }
        }

        Well _selectedWell;
        public Well SelectedWell
        {
            get { return _selectedWell; }
            set
            {
                _selectedWell = value;
                if (value == null)
                {
                    Datasets = null;
                    return;
                }
                Datasets = SelectedWell.Datasets;
                if (Datasets.Any())
                    SelectedDataset = Datasets.First();
                NotifyPropertyChanged("SelectedWell");
            }
        }

        IEnumerable<Dataset> _datasets;
        public IEnumerable<Dataset> Datasets
        {
            get { return _datasets; }
            set
            {
                _datasets = value;
                NotifyPropertyChanged("Datasets");
            }
        }

        Dataset _selectedDataset;
        public Dataset SelectedDataset
        {
            get { return _selectedDataset; }
            set
            {
                _selectedDataset = value;
                NotifyPropertyChanged("SelectedDataset");
            }
        }
        ICommand _selectDatasetCommand;
        public ICommand SelectDatasetCommand
        {
            get { return _selectDatasetCommand ?? (_selectDatasetCommand = new RelayCommand(SelectDataset, () => SelectedDataset != null)); }
        }
        private void SelectDataset()
        {
            IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.SelectDataset, SelectedDataset);
        }
    }//end class
}//end namespace
