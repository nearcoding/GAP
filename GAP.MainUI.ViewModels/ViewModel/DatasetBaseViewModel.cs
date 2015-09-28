using System.Linq;
using System.Collections.Generic;
using Ninject;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public abstract class DatasetBaseViewModel : BaseViewModel<Dataset>
    {
        bool _tvdSelected, _cancelButtonVisible;

        IEnumerable<Well> _wells;

        public DatasetBaseViewModel(string token)
            : base(token)
        {
            CancelButtonVisible = true;
            if (GlobalDataModel.Families == null || !GlobalDataModel.Families.Any())
                GlobalDataModel.LoadFamilies();
            Families = GlobalDataModel.Families;
        }
        public IEnumerable<Project> Projects { get; set; }

        public IEnumerable<Well> Wells
        {
            get { return _wells; }
            set
            {
                _wells = value;
                NotifyPropertyChanged("Wells");
            }
        }

        protected abstract bool AddObjectValidation();

        protected abstract bool CommonValidation();

        protected abstract bool UpdateObjectValidation();

        public override void Save()
        {
            if (!ValidateCurrentObject()) return;
            SaveDatasetObject();
        }

        public bool CancelButtonVisible
        {
            get { return _cancelButtonVisible; }
            set
            {
                _cancelButtonVisible = value;
                NotifyPropertyChanged("CancelButtonVisible");
            }
        }

        public bool ValidateCurrentObject()
        {
            if (CurrentObject.MinUnitValue > CurrentObject.MaxUnitValue)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("MinUnitValueMustBeSmaller"));
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

        protected virtual void SaveDatasetObject()
        {

        }

        Project _selectedProject;
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                if (value==null) return;
                if (CurrentObject != null) CurrentObject.RefProject = value.ID;

                Wells = HelperMethods.Instance.GetWellsByProjectID(SelectedProject.ID);
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
                GetValidName();
                NotifyPropertyChanged("SelectedWell");
            }
        }

        protected virtual void GetValidName()
        {

        }

        public List<Family> Families { get; set; }

        public int InteralCurrentUnits { get; set; }

        public bool TVDSelected
        {
            get { return _tvdSelected; }
            set
            {
                _tvdSelected = value;
                CurrentObject.IsTVD = _tvdSelected;
                NotifyPropertyChanged("TVDSelected");
            }
        }

        protected bool IsDatasetNameUniqueForAdding()
        {
            if (CurrentObject == null) return false;
            if (!HelperMethods.Instance.GetDatasetsByWellID
                (CurrentObject.RefWell).Any(v => v.ID == CurrentObject.ID.Trim()))
                return true;

            DatasetNameAlreadyInUse();
            return false;
        }

        protected void DatasetNameAlreadyInUse()
        {
            IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                IoC.Kernel.Get<IResourceHelper>().ReadResource("DatasetNameAlreadyInUse"));
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
    }//end class
}//end namespace
