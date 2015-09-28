using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.ViewModel;

namespace GAP
{
    /// <summary>
    /// Interaction logic for FullScreenSettingsView.xaml
    /// </summary>
    public partial class FullScreenSettingsView 
    {
        public FullScreenSettingsView()
        {
            InitializeComponent();            
            _dataContext = new FullScreenSettingsViewModel(Token);
            _dataContext.HideObjectNavigator = (bool)Properties.Settings.Default["HideObjectNavigator"];
            _dataContext.HideToolbar=(bool) Properties.Settings.Default["HideToolbar"];
            _dataContext.HideStatusBar = (bool)Properties.Settings.Default["HideStatusbar"];
            DataContext = _dataContext;
            AddKeyBindings<BaseEntity>();
        }

        FullScreenSettingsViewModel _dataContext;
        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch(messageType.MessageType)
            {
                case NotificationMessageEnum.SaveFullScreenSettings:
                    Properties.Settings.Default["HideObjectNavigator"] = _dataContext.HideObjectNavigator;
                    Properties.Settings.Default["HideToolbar"] = _dataContext.HideToolbar;
                    Properties.Settings.Default["HideStatusbar"] = _dataContext.HideStatusBar;
                    Properties.Settings.Default.Save();
                    Close();
                    break;                    
            }
        }
    }//end class
}//end namespace
