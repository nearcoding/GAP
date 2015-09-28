using GalaSoft.MvvmLight.Command;
using GAP.BL;
using System.Windows.Input;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class ChartTrackPropertiesBaseViewModel : BaseViewModel<BaseEntity>
    {
        ICommand _upCommand, _downCommand;
        public ChartTrackPropertiesBaseViewModel(string token)
            : base(token)
        {

        }

        public ICommand UpCommand
        {
            get { return _upCommand ?? (_upCommand = new RelayCommand(Up, () => CanUp())); }
        }

        public ICommand DownCommand
        {
            get { return _downCommand ?? (_downCommand = new RelayCommand(Down, () => CanDown())); }
        }

        protected virtual void Up()
        {

        }

        protected virtual void Down()
        {

        }

        protected virtual bool CanUp()
        {
            return false;
        }

        protected virtual bool CanDown()
        {
            return false;
        }
    }
}
