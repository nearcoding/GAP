using GAP.BL;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;
using System.Windows;
using Ninject;
using GAP.MainUI.ViewModels.Helpers;
namespace GAP
{
    public partial class RemoveChartView 
    {
        RemoveChartViewModel _dataContext;
        public RemoveChartView()
        {
            InitializeComponent();
            _dataContext = new RemoveChartViewModel(Token);
            DataContext = _dataContext;
            AddKeyBindings<BaseEntity>();
            Closed += RemoveChartNewView_Closed;
        }

        void RemoveChartNewView_Closed(object sender, System.EventArgs e)
        {
            _dataContext.Dispose();
            _dataContext = null;
        }
         
        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.RemoveChart:
                    var result = MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_ChartRemovalConfirmation"), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    
                    if (result == MessageBoxResult.Yes)
                        _dataContext.ChartRemovalApproved = true;                   

                    break;
            }
        }
    }//end class
}//end namespace
