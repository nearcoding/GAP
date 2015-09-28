using System.Windows;
using Ninject;
using GAP.BL;
using GAP.Helpers;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.MainUI.ViewModels.Helpers;

namespace GAP
{
    /// <summary>
    /// Interaction logic for RemoveTrackNewView.xaml
    /// </summary>
    public partial class RemoveTrackView
    {
        RemoveTrackViewModel _dataContext;
        public RemoveTrackView()
        {
            InitializeComponent();
            _dataContext = new RemoveTrackViewModel(Token);
            DataContext = _dataContext;
            AddKeyBindings<BaseEntity>();
            Closed += RemoveTrackNewView_Closed;
        }

        void RemoveTrackNewView_Closed(object sender, System.EventArgs e)
        {
            _dataContext.Dispose();
            _dataContext = null;
        }

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.RemoveTrack:
                    var result = MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_TrackRemovalConfirmation"),
                        GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        _dataContext.TrackRemovalApproved = true;
                    break;             
            }
        }
    }//end class
}//end namespace
