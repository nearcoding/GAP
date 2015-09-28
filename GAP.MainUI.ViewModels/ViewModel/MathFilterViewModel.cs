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
using System.Windows.Input;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class MathFilterViewModel : ExtendedBaseViewModel<Dataset>
    {
        public MathFilterViewModel(string token)
            : base(token)
        {
            Mapper.CreateMap<Dataset, Dataset>();
            Projects = HelperMethods.Instance.GetProjectsWithDatasets();
            if (Projects.Any())
                SelectedProject = Projects.First();
        }
        int _filter;
        public int Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                SetDatasetName();
                NotifyPropertyChanged("Filter");
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
                CurrentObject.UserNotes = string.Empty;
                CurrentObject.SystemNotes = string.Empty;
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

        private void SetDatasetName()
        {
            if (CurrentObject == null || SelectedDataset == null) return;
            CurrentObject.Name = string.Format("{0}_F{1}", SelectedDataset.Name, Filter.ToString());
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
            if (SelectedDataset == null || string.IsNullOrWhiteSpace(CurrentObject.Name) || string.IsNullOrWhiteSpace(Filter.ToString()))
                return false;

            return true;
        }

        public override void Save()
        {
            if (!AddObjectValidation()) return;
            ApplyFilter();
            CurrentObject.SubDatasets.Clear();
            var maxDisplayIndex = SelectedWell.Datasets.Max(u => u.DisplayIndex);
            CurrentObject.DisplayIndex = maxDisplayIndex;

            var lst = GlobalDataModel.Instance.GetValidListOfDepthAndCurves(CurrentObject.DepthAndCurves);
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.AppendLine("Math Filter Dataset");
            sBuilder.AppendLine(GlobalDataModel.Instance.GetNotesFirstAndLastValidData(lst));
            sBuilder.Append(string.Format("Filter Value Applied : {0}",Filter.ToString()));            
            CurrentObject.SystemNotes = sBuilder.ToString();
            SelectedWell.Datasets.Add(CurrentObject);
            IoC.Kernel.Get<ISendMessage>().MessageBoxWithInformation(Token, "Dataset created successfully");
            IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        private void ApplyFilter()
        {
            int filterValue = Filter - 1;
            filterValue = filterValue / 2;

            var depthAndCurves = SelectedDataset.DepthAndCurves;
            CurrentObject.DepthAndCurves.Clear();
            for (int i = 0; i < depthAndCurves.Count; i++)
            {
                decimal valueToProcess;

                DepthCurveInfo currentInfo = new DepthCurveInfo();
                decimal valueToGet = 0;
                int counter = 0;
                for (int j = i - filterValue; j < i; j++)
                {
                    if (j >= 0)
                    {
                        valueToProcess = depthAndCurves[j].Curve;
                        if (valueToProcess >= (decimal)-999.999 && valueToProcess <= -999)
                            continue;

                        valueToGet += valueToProcess;
                        counter += 1;
                    }
                }
                valueToProcess = depthAndCurves[i].Curve;
                if (valueToProcess >= (decimal)-999.999 && valueToProcess <= -999)
                {
                    //this is null value and should not be part of the calculation
                }
                else
                {
                    valueToGet += valueToProcess;
                    counter += 1;
                }
                for (int k = i + 1; k <= i + filterValue; k++)
                {
                    if (k < depthAndCurves.Count)
                    {
                        valueToProcess = depthAndCurves[k].Curve;
                        if (valueToProcess >= (decimal)-999.999 && valueToProcess <= -999)
                            continue;

                        valueToGet += valueToProcess;
                        counter += 1;
                    }
                }
                currentInfo.Depth = depthAndCurves[i].Depth;

                if (counter > 0)
                {
                    valueToGet = valueToGet / counter;
                    currentInfo.Curve = decimal.Parse(valueToGet.ToString("0.##"));
                }
                else
                {
                    currentInfo.Curve = 0;
                }
                CurrentObject.DepthAndCurves.Add(currentInfo);
            }
        }

        protected override bool AddObjectValidation()
        {
            if (Filter % 2 == 0)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "Filter should be an odd number");
                return false;
            }

            if (Filter < 1)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "Filter value not valid");
                return false;
            }

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

