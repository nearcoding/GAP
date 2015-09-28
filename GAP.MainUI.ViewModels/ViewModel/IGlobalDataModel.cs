using GAP.Helpers;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public interface IGlobalDataModel
    {
        void ClearAll();
        IMainScreenViewModel MainViewModel { get; set; }
        string GetAppDataFolder();
        void SendMessage(string token, NotificationMessageEnum messageType);
        void SendMessage(string token, NotificationMessageEnum messageType, object notificationObject);
    }//end class
}//end namespace
