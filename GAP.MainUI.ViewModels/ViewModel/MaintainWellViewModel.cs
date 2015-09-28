using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using Ninject;
using AutoMapper;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using GAP.BL.CollectionEntities;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class MaintainWellViewModel : BaseViewModel<Well>
    {
        string _originalWellName, _originalProjectName, _engineerName;
        ICommand _addEngineerCommand, _deleteEngineerCommand, _projectSelectionCommand;
        bool _onShoreSelected, _offShoreSelected;

        public MaintainWellViewModel(string token, Well well)
            : base(token)
        {
            Projects = new List<string>();
            if (well != null &&
                (!string.IsNullOrWhiteSpace(well.WellName) && !string.IsNullOrWhiteSpace(well.RefProject)))
                InitializeExistingWell(well);
            else
                InitializeNewWell();
        }

        private void InitializeExistingWell(Well well)
        {
            Title = IoC.Kernel.Get<IResourceHelper>().ReadResource("EditWell");

            _originalProjectName = well.RefProject;
            _originalWellName = well.WellName;

            var result = ProjectCollection.Instance.CurrentList.Single(u => u.ProjectName == _originalProjectName)
                .Wells.Where(v => v.WellName == _originalWellName);
            //WellCollection.Instance.CurrentList.Where
               // (u => u.WellName == _originalWellName && u.RefProject == _originalProjectName).ToList();

            if (result.Count() > 1)
                throw new InvalidOperationException(IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_MultipleWellsWithSameName"));
            else if (result.Count == 1)
                OriginalObject = result[0];

            if (OriginalObject == null)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError
                    (Token, string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_WellNotFoundUnderProject"), well.WellName, well.RefProject));
                return;
            }

            OriginalObject = Mapper.Map<Well>(OriginalObject);
            CurrentObject = Mapper.Map<Well>(OriginalObject);

            Projects = new List<string>() { _originalProjectName };
            CurrentObject.ObjectChanged += CurrentObject_ObjectChanged;

            IsDirty = false;
        }

        private void InitializeNewWell()
        {
            IsNew = true;
            Title = IoC.Kernel.Get<IResourceHelper>().ReadResource("NewWell");

            ProjectCollection.Instance.CurrentList.ToList().ForEach(u => Projects.Add(u.ProjectName));

            CurrentObject = new Well
            {
                RefProject = Projects[0],
                EngineerNames = new ObservableCollection<string>()
            };
           // OnShoreSelected = true;

            IncrementalWellName();
        }

        [ContractInvariantMethod]
        private void CodeInvariants()
        {
            Contract.Invariant(CurrentObject != null, "Well Object should not be null");
            Contract.Invariant(Projects != null, "Project list should not be null");
            Contract.Invariant(Projects.Any(), "Project list must have some projects in it");
            Contract.Invariant(!string.IsNullOrWhiteSpace(Title), "Title should not be empty");
        }

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(CurrentObject.WellName) &&
                !string.IsNullOrWhiteSpace(CurrentObject.RefProject) && (IsDirty || IsNew);
        }

        public override void Save()
        {
            if (IsNew)
                AddWell();
            else
                UpdateWell();
        }

        private void AddWell()
        {
            if (WellCollection.Instance.CurrentList.Any(u => u.RefProject == CurrentObject.RefProject
                && u.WellName == CurrentObject.WellName.Trim()))
                WellNameAlreadyInUse();
            else
                SaveWell();
        }

        private void UpdateWell()
        {
            if (WellCollection.Instance.CurrentList.Any(u => u.RefProject == CurrentObject.RefProject
                && u.WellName == CurrentObject.WellName.Trim() && u.WellName != _originalWellName))
                WellNameAlreadyInUse();
            else
                UpdateWellObject();
        }
        private void WellNameAlreadyInUse()
        {
            IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_WellNameIsAlreadyUsed"));
        }

        private void UpdateWellObject()
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(_originalWellName), "Original Well name not found");
            Contract.Requires(!string.IsNullOrWhiteSpace(_originalProjectName), "Original Project name not found");

            var originalWell = WellCollection.Instance.CurrentList.SingleOrDefault
                (u => u.RefProject == _originalProjectName && u.WellName == _originalWellName);

            if (originalWell != null)
            {
                UpdateTreeViewForWellName();

                Mapper.Map<Well, Well>(CurrentObject.TrimStringProperties(), originalWell);

                if (!CurrentObject.Equals(originalWell))
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation
                        (Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_WellNameMustNotBeEmpty"));
                    return;
                }

                IoC.Kernel.Get<IUndoRedoObject>().AddEntityToUndoRedoOperation<Well>(OriginalObject, CurrentObject);

                //if (ProjectCollection.Instance.AnyCurveUnderThisWellPartOfSelectedChart(CurrentObject))
                //    IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.RefreshTab);
            }
            IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        private void UpdateTreeViewForWellName()
        {
            //if (GlobalDataModel.MainViewModel == null || GlobalDataModel.MainViewModel.Projects == null) return;

            //var projectToShow = GlobalDataModel.MainViewModel.Projects.SingleOrDefault(u => u.ProjectName == CurrentObject.RefProject);

            //if (projectToShow == null || projectToShow.Wells == null || !projectToShow.Wells.Any()) return;

            //var wellObject = projectToShow.Wells.SingleOrDefault(u => u.WellName == _originalWellName);
            //if (wellObject != null) wellObject.WellName = CurrentObject.WellName;
        }

        private void SaveWell()
        {
            var displayIndex = 0;
            if (WellCollection.Instance.CurrentList.Any(u => u.RefProject == CurrentObject.RefProject))
                displayIndex = WellCollection.Instance.CurrentList.Where(u => u.RefProject == CurrentObject.RefProject).Select(v => v.DisplayIndex).Max() + 1;

            CurrentObject.DisplayIndex = displayIndex;
            WellCollection.Instance.CurrentList.Add(CurrentObject);
            // if (GlobalDataModel.WellList.Contains(CurrentObject))
            //todo this is messy code, why previously we have add the object to list and in next line check if it is in the list
            //verify this piece of code
            if (!string.IsNullOrWhiteSpace(CurrentObject.WellName))
            {
                IoC.Kernel.Get<IUndoRedoObject>().UndoRedoOperationDone();
                IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
            }
            else
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_WellNameMustNotBeEmpty"));
        }

        public string SelectedEngineer { get; set; }

        public string EngineerName
        {
            get { return _engineerName; }
            set
            {
                _engineerName = value;
                NotifyPropertyChanged("EngineerName");
            }
        }

        public List<string> Projects { get; set; }

        public ICommand ProjectSelectionCommand
        {
            get
            {
                return _projectSelectionCommand ??
                    (_projectSelectionCommand = new RelayCommand(IncrementalWellName));
            }
        }

        public ICommand AddEngineerCommand
        {
            get
            {
                return _addEngineerCommand ??
                    (_addEngineerCommand = new RelayCommand(AddEngineers, () => !string.IsNullOrWhiteSpace(EngineerName)));
            }
        }

        public ICommand DeleteEngineerCommand
        {
            get
            {
                return _deleteEngineerCommand ??
                    (_deleteEngineerCommand = new RelayCommand(DeleteEngineer,
                  () => !string.IsNullOrWhiteSpace(SelectedEngineer)));
            }
        }

        //public bool OnShoreSelected
        //{
        //    get { return _onShoreSelected; }
        //    set
        //    {
        //        _onShoreSelected = value;
        //        CurrentObject.OnOffShore = _onShoreSelected ? 0 : 1;
        //        NotifyPropertyChanged("OnShoreSelected");
        //    }
        //}

        //public bool OffShoreSelected
        //{
        //    get { return _offShoreSelected; }
        //    set
        //    {
        //        _offShoreSelected = value;
        //        if (_offShoreSelected)
        //        {
        //            CurrentObject.OnOffShore = 1;
        //        }
        //        else
        //        {
        //            CurrentObject.OnOffShore = 0;
        //            CurrentObject.AirGap = string.Empty;
        //            CurrentObject.MftAirGap = -1;
        //            CurrentObject.WaterDepth = string.Empty;
        //            CurrentObject.MftWaterDepth = -1;
        //            CurrentObject.WaterDensity = string.Empty;
        //            NotifyPropertyChanged("CurrentObject");
        //        }
        //        NotifyPropertyChanged("OffShoreSelected");
        //    }
        //}

        private void IncrementalWellName()
        {
            Contract.Ensures(!string.IsNullOrWhiteSpace(CurrentObject.WellName), "Incremental Well name must have some value");
            var lstWells = WellCollection.Instance.CurrentList.Where(u => u.RefProject == CurrentObject.RefProject && u.WellName.StartsWith("Well_")).Select(v => v.WellName);
            CurrentObject.WellName = GlobalDataModel.GetIncrementalEntityName(lstWells, "Well_");
        }

        private void AddEngineers()
        {
            Contract.Requires(CurrentObject.EngineerNames != null, "Current object's engineers list should not be null");
            if (!string.IsNullOrWhiteSpace(EngineerName))
                CurrentObject.EngineerNames.Add(EngineerName);
            EngineerName = string.Empty;
        }

        private void DeleteEngineer()
        {
            Contract.Requires(CurrentObject.EngineerNames != null, "Engineers list should not be null");
            Contract.Requires(SelectedEngineer != null, "Some engineer must be selected");

            if (CurrentObject.EngineerNames.Contains(SelectedEngineer))
                CurrentObject.EngineerNames.Remove(SelectedEngineer);
        }
    }//end class
}//end namespace
