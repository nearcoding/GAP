using GAP.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class PorePressureDrillingExponentViewModel : BaseViewModel<Dataset>
    {
        public PorePressureDrillingExponentViewModel(string token)
            : base(token)
        {
            CurrentObject = new Dataset();
            CurrentObject.Family = "Pore Pressure";
            CurrentObject.Units = "PPG";
            Projects = HelperMethods.Instance.GetProjectsWithDatasets();
            if (Projects.Any())
                SelectedProject = Projects.First();
            else
                throw new Exception("No project found in the system");


        }

        Project _selectedProject;
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                if (_selectedProject == null)
                    Wells = null;
                else
                {
                    Wells = _selectedProject.Wells;
                    CurrentObject.RefProject = _selectedProject.ID;
                }
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
                if (_selectedWell == null)
                    Datasets = null;
                else
                {
                    Datasets = _selectedWell.Datasets.Where(u => u.Family == "Overburden");
                    CurrentObject.RefWell = _selectedWell.ID;
                }
            }
        }

        protected override bool CanSave()
        {
            return (SelectedSubDataset != null) && CurrentObject.IsValid();
        }

        public override void Save()
        {
            CurrentObject.RefProject = SelectedProject.ID;
            CurrentObject.RefWell = SelectedWell.ID;
            CalculateDataset();
        }

        private void CalculateDataset()
        {

        }
        string _normalHG;
        public string NormalHG
        {
            get { return _normalHG; }
            set
            {
                _normalHG = value;
                NotifyPropertyChanged("NormalHG");
            }
        }
        private IEnumerable<DepthCurveInfo> OverburdenGradientMinusNormalHG()
        {
            Int32 normalHG = 0;
            Int32.TryParse(NormalHG, out normalHG);
            var curves = SelectedDataset.DepthAndCurves.ToList();
            var ValidCurves = GlobalDataModel.Instance.GetValidListOfDepthAndCurves(SelectedDataset.DepthAndCurves);
            foreach (var curve in ValidCurves)
            {
                curve.Curve = curve.Curve - normalHG;
            }
            return curves;x
        }

        string _eatonExponent;
        public string EatonExponent
        {
            get { return _eatonExponent; }
            set
            {
                _eatonExponent = value;
                NotifyPropertyChanged("EatonExponent");
            }
        }

        string _dExponent;
        public string DExponent
        {
            get { return _dExponent; }
            set
            {
                _dExponent = value;
                NotifyPropertyChanged("DExponent");
            }
        }

        IEnumerable<Dataset> _datasets;
        public IEnumerable<Dataset> Datasets
        {
            get { return _datasets; }
            set
            {
                _datasets = value;
                if (_datasets.Any())
                    SelectedDataset = Datasets.First();
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
                if (value != null)
                {
                    SubDatasets = _selectedDataset.SubDatasets.Where(u => u.IsNCT);
                }
                NotifyPropertyChanged("SelectedDataset");
            }
        }
        IEnumerable<SubDataset> _subDatasets;
        public IEnumerable<SubDataset> SubDatasets
        {
            get { return _subDatasets; }
            set
            {
                _subDatasets = value;
                if (_subDatasets.Any())
                    SelectedSubDataset = _subDatasets.First();
                NotifyPropertyChanged("SubDatasets");
            }
        }

        SubDataset _selectedSubDataset;
        public SubDataset SelectedSubDataset
        {
            get { return _selectedSubDataset; }
            set
            {
                _selectedSubDataset = value;
                NotifyPropertyChanged("SelectedSubDataset");
            }
        }

        IEnumerable<Well> _wells;
        public IEnumerable<Well> Wells
        {
            get { return _wells; }
            set
            {
                _wells = value;
                if (_wells == null)
                    _wells = value;
                else
                {
                    if (_wells.Any())
                        SelectedWell = _wells.First();
                }
                NotifyPropertyChanged("Wells");
            }
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
    }//end class
}//end namespace
