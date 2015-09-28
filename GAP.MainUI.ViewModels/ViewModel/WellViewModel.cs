using AutoMapper;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using Ninject;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class WellViewModel : ExtendedBaseViewModel<Well>
    {
        string _engineerName;
        ICommand _addEngineerCommand, _deleteEngineerCommand;
        public WellViewModel(string token)
            : base(token)
        {
            Initialization();
            Projects = GlobalCollection.Instance.Projects;
            Title = IoC.Kernel.Get<IResourceHelper>().ReadResource("NewWell");
            if (Projects.Any())
                SelectedProject = Projects.First();
        }

        public WellViewModel(string token, Well well)
            : base(token)
        {
            Initialization();
            OriginalObject = well;
            Title = IoC.Kernel.Get<IResourceHelper>().ReadResource("EditWell");
            Projects = new[] { HelperMethods.Instance.GetProjectByID(well.RefProject) };
            SelectedProject = Projects.First();
            CurrentObject = HelperMethods.GetNewObject<Well>(OriginalObject);
            CurrentObject.ObjectChanged += CurrentObject_ObjectChanged;
        }

        private void Initialization()
        {
            Mapper.CreateMap<Well, Well>();
        }

        Project _selectedProject;
        public Project SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            set
            {
                _selectedProject = value;
                if (value == null) return; //otherwise while undo redo project and well, well name should set to ""
                CurrentObject.Name = GetValidName();
            }
        }

        public IEnumerable<Project> Projects { get; set; }

        protected override string GetValidName()
        {
            if (OriginalObject != null || SelectedProject == null) return string.Empty;
            var projectObject = HelperMethods.Instance.GetProjectByID(SelectedProject.ID);
            var lstWells = projectObject.Wells.Where
               (u => u.Name.StartsWith("Well_")).Select(v => v.Name);
            CurrentObject.RefProject = SelectedProject.ID;
            return GlobalDataModel.GetIncrementalEntityName<Well>(lstWells);
        }

        public ICommand AddEngineerCommand
        {
            get
            {
                return _addEngineerCommand ??
                    (_addEngineerCommand = new RelayCommand(AddEngineers, () => !string.IsNullOrWhiteSpace(EngineerName)));
            }
        }

        public string EngineerName
        {
            get { return _engineerName; }
            set
            {
                _engineerName = value;
                NotifyPropertyChanged("EngineerName");
            }
        }

        public ICommand DeleteEngineerCommand
        {
            get
            {
                return _deleteEngineerCommand ??
                    (_deleteEngineerCommand = new RelayCommand(DeleteEngineer, () => !string.IsNullOrWhiteSpace(SelectedEngineer)));
            }
        }

        private void AddEngineers()
        {
            if (!string.IsNullOrWhiteSpace(EngineerName)) CurrentObject.EngineerNames.Add(EngineerName);
            EngineerName = string.Empty;
        }

        private void DeleteEngineer()
        {
            if (CurrentObject.EngineerNames.Contains(SelectedEngineer)) CurrentObject.EngineerNames.Remove(SelectedEngineer);
        }
        public string SelectedEngineer { get; set; }

        protected override bool CanSave()
        {
            return IsObjectValid() && (IsDirty || OriginalObject == null);
        }

        private bool IsObjectValid()
        {
            if (string.IsNullOrWhiteSpace(CurrentObject.RefProject)) return false;
            if (string.IsNullOrWhiteSpace(CurrentObject.Name)) return false;
            return true;
        }

        protected override bool CommonValidation()
        {
            var wellObject = HelperMethods.Instance.GetWellByID(CurrentObject.ID);
            if (wellObject != null)
            {
                if (OriginalObject != null && wellObject.Name == OriginalObject.Name) return true;
                WellNameAlreadyInUse();
                return false;
            } 
            if (!CurrentObject.IsValidInput(CurrentObject.Name))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, string.Format("No special characters allowed. {0} is not a valid input", CurrentObject.Name));
                return false;
            }
            return true;
        }
        private void WellNameAlreadyInUse()
        {
            var msgText = IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_WellNameIsAlreadyUsed");
            IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, msgText);
        }
        protected override bool AddObjectValidation()
        {
            if (HelperMethods.Instance.GetWellsByProjectID(CurrentObject.RefProject).Any
                (u => u.RefProject == CurrentObject.RefProject && u.Name == CurrentObject.Name.Trim()))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation
                    (Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_WellNameIsAlreadyUsed"));

                return false;
            }
            return true;
        }

        protected override bool UpdateObjectValidation()
        {
            if (HelperMethods.Instance.GetWellsByProjectID(CurrentObject.RefProject).Any
               (u => u.RefProject == CurrentObject.RefProject && u.Name == CurrentObject.Name.Trim()
                   && u.Name != OriginalObject.Name))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation
                        (Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_WellNameIsAlreadyUsed"));
                return false;
            }
            return true;
        }
        public override void Save()
        {
            if (!CommonValidation()) return;
            if (OriginalObject != null)
            {
                if (UpdateObjectValidation())
                {
                    Mapper.Map(CurrentObject, OriginalObject);
                    UndoRedoObject.GlobalUndoStack.Push(new UndoRedoData
                    {
                        ActionType = ActionPerformed.ItemUpdated,
                        ActualObject = CurrentObject,
                        EffectedType = UndoRedoType.Well
                    });
                }
                else
                    return;
            }
            else
            {
                if (AddObjectValidation())
                    SelectedProject.Wells.Add(CurrentObject);
                else
                    return;
            }

            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }
    }//end class
}//end namespace
