using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using GAP.Helpers;
namespace GAP.MainUI.ViewModels.ViewModel
{ 
    public class OverburdenGradientViewModel : ExtendedBaseViewModel<Dataset>
    {
        IEnumerable<Well> _wells;
        public OverburdenGradientViewModel(string token)
            : base(token)
        {
            if (GlobalDataModel.Families == null || !GlobalDataModel.Families.Any())
                GlobalDataModel.LoadFamilies();
            Families = GlobalDataModel.Families;

            Projects = HelperMethods.Instance.ProjectsWithRHOBDatasets();
            if (Projects.Any()) SelectedProject = Projects.First();
            SelectedFamily = Families.SingleOrDefault(u => u.FamilyName == "Overburden");
        }

        private void CalculateDepthAndCurves()
        {
            for (int j = 0; j < SelectedSourceDataset.DepthAndCurves.Count; j++)
            {
                DepthCurveInfo depthCurveInfo = new DepthCurveInfo
                {
                    Depth = j + 1
                };
                int step = 0;
                decimal curve = 0;
                for (int i = 0; i < CurrentObject.Step; i++)
                {
                    j = j + i;
                    if (SelectedSourceDataset.DepthAndCurves.Count > j)
                    {
                        var obj = SelectedSourceDataset.DepthAndCurves[j];
                        if (obj.Curve <= -999 && obj.Curve >= (decimal)-999.99)
                        {
                            curve = 0;
                        }
                        else
                            curve = curve + (obj.Depth * obj.Curve * decimal.Parse("0.4339"));
                        step += 1;
                    }
                }
                depthCurveInfo.Curve = (curve / step).ToTwoDigits();
                CurrentObject.DepthAndCurves.Add(depthCurveInfo);
            }
        }

        public override void Save()
        {
            if (!CommonValidation()) return;
            CalculateDepthAndCurves();
            StringBuilder sbuilder = new StringBuilder();
            sbuilder.AppendLine("This dataset has been generated using overburden gradient");
            sbuilder.AppendLine("Source dataset is  " + SelectedSourceDataset.Name);
            sbuilder.Append("Step used to create this dataset  is " + CurrentObject.Step.ToString());
            CurrentObject.SystemNotes = sbuilder.ToString();

            SelectedWell.Datasets.Add(CurrentObject);
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        string _selectedUnit;
        public string SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                if (CurrentObject != null) CurrentObject.Units = _selectedUnit;
                NotifyPropertyChanged("SelectedUnit");
            }
        }

        Family _selectedFamily;
        public Family SelectedFamily
        {
            get { return _selectedFamily; }
            set
            {
                _selectedFamily = value;
                if (value == null)
                {
                    CurrentObject.Family = string.Empty;
                    return;
                }
                CurrentObject.Family = _selectedFamily.FamilyName;
                if (SelectedFamily.Units.Any()) SelectedUnit = SelectedFamily.Units.First();
                NotifyPropertyChanged("SelectedFamily");
            }
        }
        public List<Family> Families { get; set; }

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
                if (value == null) return;
                if (CurrentObject != null) CurrentObject.RefProject = value.ID;

                Wells = HelperMethods.Instance.WellsWithRHOBDatasets(SelectedProject);
                if (Wells.Any()) SelectedWell = Wells.First();

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
                if (value == null) return;
                if (CurrentObject != null) CurrentObject.RefWell = value.ID;
                RHOBDatasets = HelperMethods.Instance.GetRHOBDatasets(_selectedWell);
                if (RHOBDatasets.Any()) SelectedSourceDataset = RHOBDatasets.First();
                NotifyPropertyChanged("SelectedWell");
            }
        }

        Dataset _selectedSourceDataset;
        public Dataset SelectedSourceDataset
        {
            get { return _selectedSourceDataset; }
            set
            {
                _selectedSourceDataset = value;
                if (_selectedSourceDataset != null)
                    CurrentObject.Name = "OB" + _selectedSourceDataset.Name;
                NotifyPropertyChanged("SelectedSourceDataset");
            }
        }

        IEnumerable<Dataset> _rhobDatasets;

        public IEnumerable<Dataset> RHOBDatasets
        {
            get { return _rhobDatasets; }
            set
            {
                _rhobDatasets = value;
                NotifyPropertyChanged("RHOBDatasets");
            }
        }

        protected override bool AddObjectValidation()
        {
            throw new NotImplementedException();
        }

        protected override bool UpdateObjectValidation()
        {
            throw new NotImplementedException();
        }

        protected override bool CommonValidation()
        {
            if (CurrentObject == null)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    "No dataset object has been generated by this screen");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurrentObject.Name))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    "Dataset name must be specified");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurrentObject.Family))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                     "A Family must be selected");
                return false;
            }
            if (!Families.Contains(SelectedFamily))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                     "Selected Family is not valid");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurrentObject.Units))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                     "A Unit must be selected");
                return false;
            }
            if (!SelectedFamily.Units.Contains(CurrentObject.Units))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                                   "Units must belongs to correct Family");
                return false;
            }
            if (CurrentObject.RefProject != SelectedProject.ID)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                                    "Invalid project passed in dataset");
                return false;
            }
            if (CurrentObject.RefWell != SelectedWell.ID)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                                    "Invalid well passed in dataset");
                return false;
            }
            var datasetObject = HelperMethods.Instance.GetDatasetByID(CurrentObject.ID);
            if (!VerifyMinMaxValues()) return false;
            if (datasetObject != null)
            {
                //object is being edited
                if (OriginalObject != null && datasetObject.Name == OriginalObject.Name) return true;
                DatasetNameAlreadyInUse();
                return false;
            }
            if (!CurrentObject.IsValidInput(CurrentObject.Name))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, string.Format("No special characters allowed. {0} is not a valid input", CurrentObject.Name));
                return false;
            }
            return true;
        }
        protected void DatasetNameAlreadyInUse()
        {
            IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                IoC.Kernel.Get<IResourceHelper>().ReadResource("DatasetNameAlreadyInUse"));
        }

        private bool VerifyMinMaxValues()
        {
            double minValueXAxis, maxValueXAxis;
            decimal minUnitValue;

            if (CurrentObject.MinUnitValue >= (decimal)-999.999 && CurrentObject.MinUnitValue <= -999)
                minUnitValue = CurrentObject.DepthAndCurves.Except(CurrentObject.DepthAndCurves.Where(u => u.Curve >= ((decimal)-999.99) && u.Curve <= -999)).Min(u => u.Curve);
            else
                minUnitValue = CurrentObject.MinUnitValue;
            minValueXAxis = double.Parse(minUnitValue.ToString());
            maxValueXAxis = Convert.ToDouble(CurrentObject.MaxUnitValue);
            if (maxValueXAxis < minValueXAxis)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, "Max value should be greater than ACTUAL min value(non null)" +
                    Environment.NewLine + "Max Value is " + maxValueXAxis + " and Min Value is " + minValueXAxis +
                    Environment.NewLine + "Process is now being terminated");
                return false;
            }
            return true;
        }
    }//end class
}//end namespace
