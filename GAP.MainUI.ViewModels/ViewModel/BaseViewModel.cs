using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Ninject;
using AutoMapper;
using GAP.BL.CollectionEntities;
using GAP.MainUI.ViewModels.Helpers;
using System;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public abstract class ExtendedBaseViewModel<T> : BaseViewModel<T> where T : BaseEntity
    {
        public ExtendedBaseViewModel(string token)
            : base(token)
        {
            CurrentObject = Activator.CreateInstance<T>();
            CurrentObject.Name = GetValidName();
        }
        protected virtual string GetValidName()
        {
            return string.Empty;
        }
        protected abstract bool AddObjectValidation();

        protected abstract bool UpdateObjectValidation();

        protected abstract bool CommonValidation();
    }

    public class BaseViewModel<T> : INotifyPropertyChanged where T : BaseEntity
    {
        public BaseViewModel(string token)
        {
            Mapper.CreateMap<T, T>();
            Token = token;
        }

        ICommand _cancelCommand;
        ICommand _saveCommand;

        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel)); }
        }

        protected virtual void CurrentObject_ObjectChanged()
        {
            IsDirty = !CurrentObject.Equals(OriginalObject);
        }

        public T CurrentObject { get; set; }

        public T OriginalObject { get; set; }

        [ContractInvariantMethod]
        private void InvariantMethodsBase()
        {
            Contract.Invariant(!string.IsNullOrWhiteSpace(Token), "Token should not be empty");
            Contract.Invariant(GlobalCollection.Instance.Projects != null, "Global Project List should not be null");
            Contract.Invariant(GlobalCollection.Instance.Charts != null, "Global Chart List should not be null");
            Contract.Invariant(IoC.Kernel.Get<IGlobalDataModel>().MainViewModel != null, "Main View Model should not be null");
        }

        public virtual void Cancel()
        {
            CloseScreen();
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(SaveObject, CanSave));
            }
        }

        public void SaveObject()
        {
            using (new WaitCursor())
            {
                Save();
            }
        }

        public bool IsNew { get; set; }
        public virtual void Save() { }
                
        protected virtual bool CanSave() { return true; }
        public string Token { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        bool _isDirty;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                NotifyPropertyChanged("IsDirty");
            }
        }

        public bool AutoSave { get; set; }

        protected void CloseScreen()
        {
            if (IsDirty)
            {
                AutoSave = false;
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CancelScreen);
                if (AutoSave)
                {
                    if (CanSave())
                        Save();
                    else
                        IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "Unable to save as all mandatory values are not supplied");
                }
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseScreen);
            }
            else
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseScreen);
        }

        protected virtual void CancelScreen()
        {

        }

        string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }
    }//end class
}//end namespace
