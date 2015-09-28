using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Ninject;
using GAP.Helpers;
using GAP.BL.CollectionEntities;
using GAP.BL;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class WindowMenuCommands
    {
        string _token;
        public WindowMenuCommands(string token)
        {
            _token = token;
        }

        ICommand _windowsCloseAllProjectsCommand;
        ICommand _windowsResetToolbarCommand;

        public ICommand WindowsResetToolbarCommand
        {
            get { return _windowsResetToolbarCommand ?? (_windowsResetToolbarCommand = new RelayCommand(WindowsResetToolbar)); }
        }
        ICommand _windowFullScreenSettingsViewCommand;
        public ICommand WindowFullScreenSettingsViewCommand
        {
            get { return _windowFullScreenSettingsViewCommand ?? (_windowFullScreenSettingsViewCommand = new RelayCommand(FullScreenSettings)); }
        }

        private void FullScreenSettings()
        {
            GlobalDataModel.Instance.SendMessage(_token, GAP.Helpers.NotificationMessageEnum.FullScreenSettings);
        }

        public void WindowsResetToolbar()
        {
            var obj = GlobalDataModel.AvailableToolbarItems.Where(u => u.Text == "CalculateOverburden"
                   || u.Text == @"PorePressure" || u.Text == "CalculateFracture");
            var lst = GlobalDataModel.AvailableToolbarItems.Except(obj).ToList();
            string folderName = IoC.Kernel.Get<IGlobalDataModel>().GetAppDataFolder();
            GlobalSerializer.SerializeObject(lst, folderName + "\\ToolbarInfo.xml");
            IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.BindToolBox();
        }

        private void WindowsCloseAllProjects()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.CloseAllProjectsAndWindows);
            if (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.IsProjectClosed)
            {
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Title = "GAP";
                IoC.Kernel.Get<IGlobalDataModel>().ClearAll();
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.IsProjectClosed = false;
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.ShowDefaultCharts();
            }
        }

        private bool CanWindowsCloseAllProjects()
        {
            return GlobalDataModel.ShouldSave || GlobalCollection.Instance.Projects.Any();
        }

        public ICommand WindowsCloseAllProjectsCommand
        {
            get { return _windowsCloseAllProjectsCommand ?? (_windowsCloseAllProjectsCommand = new RelayCommand(WindowsCloseAllProjects, CanWindowsCloseAllProjects)); }
        }

        ICommand _windowsCloseAllWindowsCommand;

        private void WindowsCloseAllWindows() { }

        private bool CanWindowsCloseAllWindows() { return false; }

        public ICommand WindowsCloseAllWindowsCommand
        {
            get { return _windowsCloseAllWindowsCommand ?? (_windowsCloseAllWindowsCommand = new RelayCommand(WindowsCloseAllWindows, CanWindowsCloseAllWindows)); }
        }
    }//end class
}//end namespace
