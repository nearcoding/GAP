using GalaSoft.MvvmLight.Command;
using GAP.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class PorePressureViewModel : ExtendedBaseViewModel<BaseEntity>
    {
        public PorePressureViewModel(string token)
            :base(token)
        {

        }

        ICommand _drillingExponentCommand, _resistivityCommand, _sonicCommand, _artesianCommand;

        public ICommand DrillingExponentCommand
        {
            get { return _drillingExponentCommand ?? (_drillingExponentCommand = new RelayCommand(DrillingExponent, () => true)); }
        }

        private void DrillingExponent()
        {
            GlobalDataModel.Instance.SendMessage(Token, GAP.Helpers.NotificationMessageEnum.PorePresureDrillingExponent);
        }

        public ICommand ResistivityCommand
        {
            get { return _resistivityCommand ?? (_resistivityCommand = new RelayCommand(Resistivity, () => false)); }
        }

        private void Resistivity()
        {

        }

        public ICommand SonicCommand
        {
            get { return _sonicCommand ?? (_sonicCommand = new RelayCommand(Sonic, () => false)); }
        }

        private void Sonic()
        {

        }

        public ICommand ArtesianCommand
        {
            get { return _artesianCommand ?? (_artesianCommand = new RelayCommand(Artesian, () => false)); }
        }

        private void Artesian()
        {

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
            throw new NotImplementedException();
        }
    }//end class
}//end namespace
