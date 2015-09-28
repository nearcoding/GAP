using GAP.BL;
using GAP.ExtendedControls;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;

namespace GAP
{
    /// <summary>
    /// Interaction logic for ChartPropertiesView.xaml
    /// </summary>
    public partial class ChartPropertiesView
    {
        public ChartPropertiesView()
        {
            InitializeComponent();
            DataContext = new ChartPropertiesViewModel(Token);
            AddKeyBindings<BaseEntity>();
        }

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.SaveChartProperties:                   
                    GlobalData.ShouldSave = true;
                    Close();
                    break;
            }
        }
    }
}
