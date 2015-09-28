using GAP.BL;
using GAP.MainUI.ViewModels.ViewModel;

namespace GAP
{
    /// <summary>
    /// Interaction logic for SelectProject.xaml
    /// </summary>
    public partial class SelectDataset 
    {
        public SelectDataset()
        {
            InitializeComponent();
            DataContext = new SelectDatasetViewModel(Token);
            Closed += SelectDataset_Closed;
            AddKeyBindings<BaseEntity>();
        }

        void SelectDataset_Closed(object sender, System.EventArgs e)
        {
            Closed -= SelectDataset_Closed;
            DataContext = null;
        }

        public Dataset SelectedDataset { get; set; }
        protected override void ReceiveMessage(Helpers.NotificationMessageType messageType)
        {
            base.ReceiveMessage(messageType);
            switch(messageType.MessageType)
            {
                case Helpers.NotificationMessageEnum.SelectDataset:
                    SelectedDataset = messageType.MessageObject as Dataset;
                    Close();
                    break;
            }
        }
    }//end class
}//end namespace
