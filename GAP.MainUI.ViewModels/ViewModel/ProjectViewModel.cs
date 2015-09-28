using System;
using System.Linq;
using System.Collections.Generic;
using Ninject;
using AutoMapper;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using System.Text.RegularExpressions;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class ProjectViewModel : ExtendedBaseViewModel<Project>
    {
        public ProjectViewModel(string token)
            : base(token)
        {
            CurrentObject = new Project
            {
                Name = GetValidName()
            };
            Title = IoC.Kernel.Get<IResourceHelper>().ReadResource("NewProject");
            Initialization();
            CurrentObject.Units = ProjectUnits.First();
        }

        public ProjectViewModel(string token, string projectID)
            : base(token)
        {
            OriginalObject = HelperMethods.Instance.GetProjectByID(projectID);
            if (OriginalObject == null)
                throw new Exception("Project you are trying to open, is no longer exist in the system");
            else
                CurrentObject = HelperMethods.GetNewObject<Project>(OriginalObject);

            Title = IoC.Kernel.Get<IResourceHelper>().ReadResource("EditProject");
            Initialization();
            CurrentObject.ObjectChanged += CurrentObject_ObjectChanged;
        }

        protected override string GetValidName()
        {
            var lstProjects = GlobalCollection.Instance.Projects.Where
                   (u => u.Name.StartsWith("Project_")).Select(v => v.Name);
            return GlobalDataModel.GetIncrementalEntityName<Project>(lstProjects);
        }

        private void Initialization()
        {
            AddProjectUnits();
            Mapper.CreateMap<Project, Project>();
        }

        public IEnumerable<string> ProjectUnits { get; private set; }

        private void AddProjectUnits()
        {
            ProjectUnits = new[] { "API", "SI" };
        }

        protected override bool CanSave()
        {
            return IsObjectValid() && (OriginalObject == null || IsDirty);
        }

        public override void Save()
        {
            if (!CommonValidation()) return;
            if (OriginalObject != null)
            {
                if (UpdateObjectValidation())
                {
                    //copy the properties of current object to actual object
                    Mapper.Map(CurrentObject, OriginalObject);
                    UndoRedoObject.GlobalUndoStack.Push(new UndoRedoData
                    {
                        ActionType = ActionPerformed.ItemUpdated,
                        ActualObject = CurrentObject,
                        EffectedType = UndoRedoType.Project
                    });
                }
                else
                    return;
            }
            else
            {
                if (AddObjectValidation())
                    GlobalCollection.Instance.Projects.Add(CurrentObject);
                else
                    return;
            }

            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        protected override bool AddObjectValidation() { return true; }

        private bool IsObjectValid()
        {
            if (string.IsNullOrWhiteSpace(CurrentObject.Name)) return false;
            if (string.IsNullOrWhiteSpace(CurrentObject.Units)) return false;
            return true;
        }

        protected override bool UpdateObjectValidation() { return true; }

        protected override bool CommonValidation()
        {
            if (CurrentObject.Name.Trim().ToLower() == "lithology")
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "Keyword 'Lithology' is reserved for system use, you can't use 'Lithology' as Project name");
                return false;
            }
            if (!CurrentObject.IsValidInput(CurrentObject.Name))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,string.Format( "No special characters allowed. {0} is not a valid input", CurrentObject.Name));
                return false;
            }
            var projectObject = HelperMethods.Instance.GetProjectByID(CurrentObject.ID); ;
            if (projectObject != null)
            {
                if (OriginalObject != null && projectObject.Name == OriginalObject.Name) return true;
                ProjectNameAlreadyInUse();
                return false;
            }
           
            return true;
        }

        private void ProjectNameAlreadyInUse()
        {
            var msgText = IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_ProjectNameAlreadyInUse");
            IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, msgText);
        }
    }//end class
}//end namespace
