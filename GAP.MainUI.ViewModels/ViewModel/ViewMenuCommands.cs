using GalaSoft.MvvmLight.Command;
using GAP.Helpers;
using System.Windows.Input;
using Ninject;
using GAP.MainUI.ViewModels.Helpers;
using GAP.BL;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class ViewMenuCommands : SubBaseEntity
    {
        string _token;
        public ViewMenuCommands(string token)
        {
            _token = token;
        }

        ICommand _viewZoomInCommand;

        private void ViewZoomIn()
        {

        }

        private bool CanViewZoomIn()
        {
            return false;
        }

        bool _isToolbarVisible;
        public bool IsToolBarVisible
        {
            get { return _isToolbarVisible; }
            set
            {
                _isToolbarVisible = value;
                ShowHideToolbarText = value ? IoC.Kernel.Get<IResourceHelper>().ReadResource("HideToolbar") :
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("ShowToolbar");
                NotifyPropertyChanged("IsToolBarVisible");
            }
        }

        public ICommand ViewZoomInCommand
        {
            get { return _viewZoomInCommand ?? (_viewZoomInCommand = new RelayCommand(ViewZoomIn, CanViewZoomIn)); }
        }


        ICommand _viewZoomOutCommand;

        private void ViewZoomOut()
        {

        }

        private bool CanViewZoomOut()
        {
            return false;
        }

        public ICommand ViewZoomOutCommand
        {
            get { return _viewZoomOutCommand ?? (_viewZoomOutCommand = new RelayCommand(ViewZoomOut, CanViewZoomOut)); }
        }


        ICommand _viewZoomToScaleCommand;

        private void ViewZoomToScale()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.ZoomTracks);
        }

        public ICommand ViewZoomToScaleCommand
        {
            get { return _viewZoomToScaleCommand ?? (_viewZoomToScaleCommand = new RelayCommand(ViewZoomToScale, () => HelperMethods.Instance.AnyTrackExists())); }
        }

        ICommand _viewGoToMarkCommand;

        private void ViewGoToMark()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.GoToMarkScreen);
        }

        public ICommand ViewGoToMarkCommand
        {
            get { return _viewGoToMarkCommand ?? (_viewGoToMarkCommand = new RelayCommand(ViewGoToMark, () => HelperMethods.Instance.AnyTrackExists())); }
        }


        ICommand _viewMakeTransparentCommand;
        string _transparentText;
        public string TransparentText
        {
            get { return _transparentText; }
            set
            {
                _transparentText = value;
                NotifyPropertyChanged("TransparentText");
            }
        }

        string _showHideObjectNavigatorText;
        public string ShowHideObjectNavigatorText
        {
            get { return _showHideObjectNavigatorText; }
            set
            {
                _showHideObjectNavigatorText = value;
                NotifyPropertyChanged("ShowHideObjectNavigatorText");
            }
        }

        string _showHideToolbarText;
        public string ShowHideToolbarText
        {
            get { return _showHideToolbarText; }
            set
            {
                _showHideToolbarText = value;
                NotifyPropertyChanged("ShowHideToolbarText");
            }
        }

        double _opacity;
        public double Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                NotifyPropertyChanged("Opacity");
            }
        }


        private void ViewMakeTransparent()
        {
            if (Opacity == 0.8)
            {
                TransparentText = IoC.Kernel.Get<IResourceHelper>().ReadResource("MakeTransparent");
                Opacity = 1;
            }
            else
            {
                TransparentText = IoC.Kernel.Get<IResourceHelper>().ReadResource("MakeOpaque");
                Opacity = 0.8;
            }
        }

        private bool CanViewMakeTransparent()
        {
            return true;
        }

        public ICommand ViewMakeTransparentCommand
        {
            get { return _viewMakeTransparentCommand ?? (_viewMakeTransparentCommand = new RelayCommand(ViewMakeTransparent, CanViewMakeTransparent)); }
        }

        ICommand _viewShowHideToolbarCommand;

        private void ViewShowHideToolbar()
        {
            IsToolBarVisible = !IsToolBarVisible;
        }

        private bool CanViewShowHideToolbar()
        {
            return true;
        }

        public ICommand ViewShowHideToolbarCommand
        {
            get { return _viewShowHideToolbarCommand ?? (_viewShowHideToolbarCommand = new RelayCommand(ViewShowHideToolbar, CanViewShowHideToolbar)); }
        }

        ICommand _viewCustomizeToolbarCommand;

        private void ViewCustomizeToolbar()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.CustomizeToolbar);
        }

        private bool CanViewCustomizeToolbar()
        {
            return true;
        }

        public ICommand ViewCustomizeToolbarCommand
        {
            get { return _viewCustomizeToolbarCommand ?? (_viewCustomizeToolbarCommand = new RelayCommand(ViewCustomizeToolbar, CanViewCustomizeToolbar)); }
        }


        ICommand _viewShowHideObjectNavigatorCommand;

        private void ViewShowHideObjectNavigator()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.ShowHideObjectNavigator);
        }

        private bool CanViewShowHideObjectNavigator()
        {
            return true;
        }

        public ICommand ViewShowHideObjectNavigatorCommand
        {
            get { return _viewShowHideObjectNavigatorCommand ?? (_viewShowHideObjectNavigatorCommand = new RelayCommand(ViewShowHideObjectNavigator, CanViewShowHideObjectNavigator)); }
        }


        //ICommand _viewShowHideSpreadsheetCommand;

        //private void ViewShowHideSpreadsheet()
        //{

        //}

        //private bool CanViewShowHideSpreadsheet()
        //{
        //    return false;
        //}

        //public ICommand ViewShowHideSpreadsheetCommand
        //{
        //    get { return _viewShowHideSpreadsheetCommand ?? (_viewShowHideSpreadsheetCommand = new RelayCommand(ViewShowHideSpreadsheet, CanViewShowHideSpreadsheet)); }
        //}
    }
}
