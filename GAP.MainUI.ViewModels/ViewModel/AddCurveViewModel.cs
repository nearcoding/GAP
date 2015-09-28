using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using System.Collections.Generic;
using System.Linq;
using Ninject;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class  AddCurveViewModel : BaseViewModel<Curve>
    {
        public AddCurveViewModel(string token)
            : base(token)
        {
            CurrentObject = new Curve();
            Charts = HelperMethods.Instance.GetChartsWithTracks();
            SelectedChart = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject != null
                ? IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject : Charts.First();
            Projects = HelperMethods.Instance.GetProjectsWithDatasets();
            SelectedProject = Projects.First();
        }

        public void ReclaimMemory()
        {
            SelectedChart = null;
            SelectedTrack = null;
        }

        public AddCurveViewModel(string token, string projectID, string wellID, string datasetID, string chartID, string trackID)
            : base(token)
        {
            CurrentObject = new Curve();
            Charts = new[] { HelperMethods.Instance.GetChartByID(chartID) };
            if (Charts.Any()) SelectedChart = Charts.First();
            CurrentObject.RefTrack = trackID;

            Projects = new[] { HelperMethods.Instance.GetProjectByID(projectID) };
            if (Projects.Any())
            {
                SelectedProject = Projects.First();
                Wells = new[] { SelectedProject.Wells.Single(u => u.ID == wellID) };
                if (Wells.Any())
                {
                    SelectedWell = Wells.First();
                    Datasets = new[] { SelectedWell.Datasets.Single(u => u.ID == datasetID) };
                    if (Datasets.Any()) CurrentObject.RefDataset = Datasets.First().ID;
                }
            }
            Save();
        }

        public IEnumerable<Chart> Charts { get; set; }

        Chart _selectedChart;

        public Chart SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                if (value == null || value == _selectedChart) return;
                _selectedChart = value;
                CurrentObject.RefChart = _selectedChart.ID;
                Tracks = SelectedChart.Tracks;
                SelectedTrack = Tracks.First();
                NotifyPropertyChanged("SelectedChart");
            }
        }
        Project _selectedProject;
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                if (value == null || _selectedProject == value) return;
                _selectedProject = value;
                Wells = HelperMethods.Instance.GetWellsWithDatasetsByProjectID(SelectedProject.ID);
                NotifyPropertyChanged("Wells");
                SelectedWell = Wells.First();
                CurrentObject.RefProject = _selectedProject.ID;
                NotifyPropertyChanged("SelectedProject");
            }
        }

        Well _selectedWell;
        public Well SelectedWell
        {
            get { return _selectedWell; }
            set
            {
                if (value == null || _selectedWell == value) return;
                _selectedWell = value;
                CurrentObject.RefWell = _selectedWell.ID;
                Datasets = HelperMethods.Instance.GetDatasetsByWellID(SelectedWell.ID);
                NotifyPropertyChanged("Datasets");
                SelectedDataset = Datasets.First();
                NotifyPropertyChanged("SelectedWell");
                NotifyPropertyChanged("SelectedDataset");
            }
        }
        Dataset _selectedDataset;
        public Dataset SelectedDataset
        {
            get { return _selectedDataset; }
            set
            {
                if (_selectedDataset == value || value == null) return;
                _selectedDataset = value;
                CurrentObject.RefDataset = _selectedDataset.ID;
                NotifyPropertyChanged("SelectedDataset");
            }
        }
        Track _selectedTrack;
        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                if (value == null || _selectedTrack == value) return;
                _selectedTrack = value;
                CurrentObject.RefTrack = SelectedTrack.ID;
                NotifyPropertyChanged("SelectedTrack");
            }
        }

        public IEnumerable<Track> Tracks { get; set; }

        public IEnumerable<Project> Projects { get; set; }

        public IEnumerable<Well> Wells { get; set; }

        public IEnumerable<Dataset> Datasets { get; set; }

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(CurrentObject.RefChart) &&
                !string.IsNullOrWhiteSpace(CurrentObject.RefDataset) &&
                !string.IsNullOrWhiteSpace(CurrentObject.RefProject) &&
                !string.IsNullOrWhiteSpace(CurrentObject.RefTrack) &&
                !string.IsNullOrWhiteSpace(CurrentObject.RefWell);
        }

        private bool ValidateCurveData()
        {
            var curves = HelperMethods.Instance.GetCurvesByTrackID(CurrentObject.RefTrack);
            if (curves.Any(u => u.RefProject == CurrentObject.RefProject && u.RefWell == CurrentObject.RefWell && u.RefDataset == CurrentObject.RefDataset))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("AnotherCurveAlreadyExists"));
                return false;
            }

            return true;
        }

        public override void Save()
        {
            if (!ValidateCurveData()) return;
            CurveManager.Instance.AddCurveObject(CurrentObject);
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }
    }//end class
}//end namespace
