using GAP.BL;
using GAP.Helpers;
using Ninject;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class FullScreenSettingsViewModel : BaseViewModel<BaseEntity>
    {
        public FullScreenSettingsViewModel(string token)
            : base(token)
        {

        }
        public bool HideStatusBar { get; set; }
        public bool HideObjectNavigator { get; set; }

        public bool HideToolbar { get; set; }

        public override void Save()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.SaveFullScreenSettings);
        }
    }//end class
}//end namespace
