using System.Linq;
using System.Collections.Generic;
using Ninject;
using AutoMapper;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using System.Windows.Media;
using System.Text;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class LinePropertiesViewModel : ExtendedBaseViewModel<SubDataset>
    {
        public LinePropertiesViewModel(string token)
            : base(token)
        {
            CurrentObject.IsNCT = true;
            InitializeCurrentObject();
            Projects = HelperMethods.Instance.GetProjectsWithCurves();
            if (Projects.Any())
                SelectedProject = Projects.First();
            CurrentObject.ObjectChanged += CurrentObject_ObjectChanged;

            IsNew = true;
            NotifyPropertyChanged("IsNew");
        }

        public LinePropertiesViewModel(SubDataset subDataset, string token)
            : base(token)
        {
            Mapper.CreateMap<SubDataset, SubDataset>();
            CurrentObject = (SubDataset)Mapper.Map(subDataset, typeof(SubDataset), typeof(SubDataset));

            OriginalObject = subDataset;
            Projects = GlobalCollection.Instance.Projects.Where(u => u.ID == CurrentObject.Project);
            SelectedProject = Projects.First();

            IsNew = false;
            NotifyPropertyChanged("IsNew");
        }

        private void InitializeCurrentObject()
        {
            CurrentObject.LineStyle = 0;
            CurrentObject.LineGrossor = 0;
            CurrentObject.LineColor = new Colour(Color.FromRgb(0, 0, 0));
            CurrentObject.Name = GetValidName();
        }
        bool _alreadyNotified;
        protected override void CurrentObject_ObjectChanged()
        {
            if (!_alreadyNotified)
            {
                _alreadyNotified = true;
                FillCurrentObjectIfExists();
                _alreadyNotified = false;
            }
        }

        public IEnumerable<Project> Projects { get; set; }

        public IEnumerable<Well> Wells { get; set; }

        public IEnumerable<Dataset> Datasets { get; set; }

        Project _selectedProject;
        Well _selectedWell;
        Dataset _selectedDataset;

        public Project SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            set
            {
                _selectedProject = value;
                if (value == null) return;
                CurrentObject.Project = _selectedProject.ID;

                if (OriginalObject != null)
                    Wells = _selectedProject.Wells.Where(u => u.ID == CurrentObject.Well);
                else
                    Wells = _selectedProject.Wells;
                NotifyPropertyChanged("Wells");
                if (Wells.Any()) SelectedWell = Wells.First();
                NotifyPropertyChanged("SelectedProject");
            }
        }

        public Well SelectedWell
        {
            get { return _selectedWell; }
            set
            {
                _selectedWell = value;
                if (_selectedWell != null)
                {
                    var datasets = HelperMethods.Instance.GetDatasetsByCurves();
                    datasets = datasets.Where(u => u != null);
                    if (OriginalObject != null)
                        Datasets = datasets.Where(u => u.RefProject == _selectedWell.RefProject && u.RefWell == _selectedWell.ID && u.ID == CurrentObject.Dataset);
                    else
                        Datasets = datasets.Where(u => u.RefProject == _selectedWell.RefProject && u.RefWell == _selectedWell.ID);

                    CurrentObject.Well = _selectedWell.ID;

                    if (Datasets.Any())
                        SelectedDataset = Datasets.First();
                    else
                        SelectedDataset = null;
                    NotifyPropertyChanged("SelectedWell");
                    NotifyPropertyChanged("Datasets");
                }
            }
        }

        public Dataset SelectedDataset
        {
            get { return _selectedDataset; }
            set
            {
                _selectedDataset = value;
                NotifyPropertyChanged("SelectedDataset");
                if (value == null) return;
                CurrentObject.Dataset = SelectedDataset.ID;
                if (OriginalObject == null)
                    FillCurrentObjectIfExists();

            }
        }
        protected override string GetValidName()
        {
            if (OriginalObject != null || SelectedDataset == null) return string.Empty;
            var datasetObject = HelperMethods.Instance.GetDatasetByID(SelectedDataset.ID);
            var lstSubDatasets =
                datasetObject.SubDatasets.Where(u => u.Name.StartsWith("SubDataset_")).Select(v => v.Name);
            return GlobalDataModel.GetIncrementalEntityName<SubDataset>(lstSubDatasets);
        }

        private void FillCurrentObjectIfExists()
        {
            var subDataset = HelperMethods.Instance.GetSubDatasetObjectBySubdatasetDetails(CurrentObject);
            if (subDataset == null)
            {
                InitializeCurrentObject();
                CurrentObject.Annotations.Clear();
            }
        }

        protected override bool CanSave()
        {
            return SelectedDataset != null && !string.IsNullOrWhiteSpace(CurrentObject.Name);
        }

        protected override bool AddObjectValidation()
        {
            var subDatasets = HelperMethods.Instance.GetSubDatasetsByDataset(CurrentObject.Dataset);

            if (subDatasets.Any(u => string.Compare(u.Name.Trim(), CurrentObject.Name.Trim(), true) == 0 && u.IsNCT == CurrentObject.IsNCT))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("SubDatasetWithTheseDetailsAlreadyExists"));
                return false;
            }
            return true;
        }

        protected override bool CommonValidation() { return true; }
        protected override bool UpdateObjectValidation() { return true; }

        public override void Save()
        {
            CurrentObject.Dataset = SelectedDataset.ID;
            CurrentObject.Project = SelectedDataset.RefProject;
            CurrentObject.Well = SelectedWell.ID;

            if (OriginalObject == null)
            {
                if (string.IsNullOrWhiteSpace(CurrentObject.UserNotes)) CurrentObject.UserNotes = string.Empty;

                var sSbuilder = new StringBuilder();
                sSbuilder.AppendLine(string.Format("Source Dataset : {0}", SelectedDataset.Name));
                sSbuilder.AppendLine(string.Format("Source Dataset Step : {0}", SelectedDataset.Step));
                sSbuilder.Append(string.Format("Source Dataset type is : {0}", SelectedDataset.IsTVD ? "TVD" : "MD"));
              
                CurrentObject.SystemNotes = sSbuilder.ToString();

                if (!AddObjectValidation()) return;

                var dataset = HelperMethods.Instance.GetDatasetByID(CurrentObject.Dataset);
                
                dataset.SubDatasets.Add(CurrentObject);
            }
            else
            {
                var subDatasets = HelperMethods.Instance.GetSubDatasetsByDataset(CurrentObject.Dataset);
                Mapper.Map(CurrentObject, OriginalObject);
            }

            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.OpenFlyoutWindow);
        }
    }//end class
}//end namespace
