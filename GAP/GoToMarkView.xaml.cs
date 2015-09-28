using GAP.BL;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using System.Windows.Threading;

namespace GAP
{
    public partial class GoToMarkView
    {
        public GoToMarkView()
        {
            InitializeComponent();
            _dataContext = new GoToMarkViewModel(Token);
            DataContext = _dataContext;
            AddKeyBindings<BaseEntity>();
        }
        GoToMarkViewModel _dataContext;
        protected override void ReceiveMessage(Helpers.NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case Helpers.NotificationMessageEnum.GoToMark:
                    DoEvents();
                    GlobalData.TrackItemsControl.ScrollToCenter(_dataContext.SelectedTrack.Name);
                    Close();
                    break;
            }
        }

        public void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        public object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;

            return null;
        }
    }//end class
}//end namespace
