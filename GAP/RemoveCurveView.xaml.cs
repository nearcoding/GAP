using GAP.HelperClasses;
using GAP.MainUI.ViewModels.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;
using System.Windows;
using Ninject;
using GAP.BL;
namespace GAP
{
    public partial class RemoveCurveView
    {
        RemoveCurveViewModel _dataContext;
        public RemoveCurveView()
        {
            InitializeComponent();
            _dataContext = new RemoveCurveViewModel(Token);
            DataContext = _dataContext;
            AddKeyBindings<BaseEntity>();
            Closed += RemoveCurveNewView_Closed;
        }

        void RemoveCurveNewView_Closed(object sender, System.EventArgs e)
        {
            _dataContext.Dispose();
            _dataContext = null;
        }

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.RemoveCurve:
                    var result = MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_CurveRemovalConfirmation"), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        _dataContext.CurveRemovalApproved = true;
                    break;
            }
        }
    }//end class
}//end namespace
