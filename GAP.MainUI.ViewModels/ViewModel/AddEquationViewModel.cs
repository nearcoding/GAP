using GAP.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using GAP.MainUI.ViewModels.Helpers;
using AutoMapper;
using System.Collections.ObjectModel;
using GAP.Helpers;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class AddEquationViewModel : ExtendedBaseViewModel<Dataset>
    {
        public AddEquationViewModel(string token)
            : base(token)
        {
            Mapper.CreateMap<Dataset, Dataset>();
            Equation = 0;
            Projects = HelperMethods.Instance.GetProjectsWithDatasets();
            if (Projects.Any())
                SelectedProject = Projects.First();
            ParamVisibility = false;
        }
        int _selectedEquation;
        public int Equation
        {
            get { return _selectedEquation; }
            set
            {
                _selectedEquation = value;
                if (_selectedEquation == 5)
                {
                    ParamVisibility = true;
                }
                else
                {
                    ParamVisibility = false;
                }
                SetDatasetName();
                NotifyPropertyChanged("Equation");
            }
        }
        bool _paramVisibility;
        public bool ParamVisibility
        {
            get { return _paramVisibility; }
            set
            {
                _paramVisibility = value;
                NotifyPropertyChanged("ParamVisibility");
            }
        }


        double _equationNumber;
        public double EquationNumber
        {
            get { return _equationNumber; }
            set
            {
                _equationNumber = value;

                NotifyPropertyChanged("EquationNumber");
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
                    SelectedWell = null;
                    Wells = null;
                    return;
                }
                Wells = HelperMethods.Instance.GetWellsWithDatasetsByProjectID(SelectedProject.ID);
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
                    SelectedDataset = null;
                    Datasets = null;
                    return;
                }
                Datasets = _selectedWell.Datasets;
                if (Datasets.Any())
                    SelectedDataset = Datasets.First();
                NotifyPropertyChanged("SelectedWell");
            }
        }

        Dataset _selectedDataset;
        public Dataset SelectedDataset
        {
            get { return _selectedDataset; }
            set
            {
                _selectedDataset = value;
                CurrentObject = (Dataset)Mapper.Map(SelectedDataset, typeof(Dataset), typeof(Dataset));
                if (SelectedDataset == null)
                {
                    NotifyPropertyChanged("CurrentObject");
                    return;
                }
                CurrentObject.LineStyle = SelectedDataset.LineStyle;
                CurrentObject.ID = Guid.NewGuid().ToString();
                SetDatasetName();
                NotifyPropertyChanged("SelectedDataset");
            }
        }

        Dataset _selectedDataset2;
        public Dataset SelectedDataset2
        {
            get { return _selectedDataset2; }
            set
            {
                _selectedDataset2 = value;
            }
        }

        private void SetDatasetName()
        {
            if (CurrentObject == null || SelectedDataset == null) return;
            CurrentObject.Name = string.Format("{0}_E{1}", SelectedDataset.Name, Equation.ToString());
            NotifyPropertyChanged("CurrentObject");
        }

        IEnumerable<Project> _projects;
        IEnumerable<Well> _wells;
        IEnumerable<Dataset> _datasets;

        public IEnumerable<Project> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                NotifyPropertyChanged("Projects");
            }

        }
        public IEnumerable<Well> Wells
        {
            get { return _wells; }
            set
            {
                _wells = value;
                NotifyPropertyChanged("Wells");
            }
        }
        public IEnumerable<Dataset> Datasets
        {
            get { return _datasets; }
            set
            {
                _datasets = value;
                NotifyPropertyChanged("Datasets");
            }
        }

        protected override bool CanSave()
        {
            if (SelectedDataset == null || string.IsNullOrWhiteSpace(CurrentObject.Name) || string.IsNullOrWhiteSpace(Equation.ToString()))
                return false;
            return true;
        }

        public override void Save()
        {
            if (!AddObjectValidation()) return;
            ApplyEquation();
            CurrentObject.SubDatasets.Clear();
            var maxDisplayIndex = SelectedWell.Datasets.Max(u => u.DisplayIndex);
            CurrentObject.DisplayIndex = maxDisplayIndex;
            SelectedWell.Datasets.Add(CurrentObject);

            var lst = GlobalDataModel.Instance.GetValidListOfDepthAndCurves(CurrentObject.DepthAndCurves);
            CurrentObject.SystemNotes = GlobalDataModel.Instance.GetNotesFirstAndLastValidData(lst);
            IoC.Kernel.Get<ISendMessage>().MessageBoxWithInformation(Token, "Dataset created successfully");
            IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        private void ApplyEquation()
        {
            var depthAndCurves = SelectedDataset.DepthAndCurves;
            CurrentObject.DepthAndCurves.Clear();
            AddDataSetEquation addEquation = new AddDataSetEquation(depthAndCurves);
            switch (Equation)
            {
                case 0:
                    CurrentObject.DepthAndCurves = addEquation.SumDataSet(SelectedDataset2.DepthAndCurves);
                    break;
                case 1:
                    CurrentObject.DepthAndCurves = addEquation.SubTDataSet(SelectedDataset2.DepthAndCurves);
                    break;
                case 2:
                    CurrentObject.DepthAndCurves = addEquation.DivDataSet(SelectedDataset2.DepthAndCurves);
                    break;
                case 3:
                    CurrentObject.DepthAndCurves = addEquation.MulDataSet(SelectedDataset2.DepthAndCurves);
                    break;
                case 4:
                    if (EquationNumber != 0)
                        CurrentObject.DepthAndCurves = addEquation.LogarithmDataSet(EquationNumber.ToString());
                    else
                        CurrentObject.DepthAndCurves = addEquation.LogarithmDataSet("5");
                    break;
            }
        }

        protected override bool AddObjectValidation()
        {
            if (!IsDatasetNameUnique())
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "Another dataset with this name already exists");
                return false;
            }

            if (CurrentObject.MarkerStyle == 0 && CurrentObject.LineStyle == 0)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("MarkerAndLineStyleShouldNotBeSame"));
                return false;
            }
            return true;
        }

        private bool IsDatasetNameUnique()
        {
            var parentWell = HelperMethods.Instance.GetWellByID(CurrentObject.RefWell);
            return !parentWell.Datasets.Any(u => u.Name == CurrentObject.Name);
        }

        protected override bool UpdateObjectValidation()
        {
            return true;
        }

        protected override bool CommonValidation()
        {
            return true;
        }
    }//end class
}//end namespace

