using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Ninject;
using GAP.Helpers;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class HelpMenuCommands
    {
        string _token;
        public HelpMenuCommands(string token)
        {
            _token = token;
        }
        
        ICommand _helpContentsCommand;

        private void HelpContents()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.WindowScreen);
        }

        private bool CanHelpContents()
        {
            return true;
        }

        public ICommand HelpContentsCommand
        {
            get { return _helpContentsCommand ?? (_helpContentsCommand = new RelayCommand(HelpContents, CanHelpContents)); }
        }
              

        ICommand _helpIndexCommand;

        private void HelpIndex()
        {

        }

        private bool CanHelpIndex()
        {
            return false;
        }

        public ICommand HelpIndexCommand
        {
            get { return _helpIndexCommand ?? (_helpIndexCommand = new RelayCommand(HelpIndex, CanHelpIndex)); }
        }

        
        ICommand _helpAboutCommand;

        private void HelpAbout()
        {

        }

        private bool CanHelpAbout()
        {
            return false;
        }

        public ICommand HelpAboutCommand
        {
            get { return _helpAboutCommand ?? (_helpAboutCommand = new RelayCommand(HelpAbout, CanHelpAbout)); }
        }

        
    }//end class
}//end namespace
