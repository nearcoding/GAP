using GAP.BL;
using GAP.ExtendedControls;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;

namespace GAP
{
    /// <summary>
    /// Interaction logic for AutoDept.xaml
    /// </summary>
    public partial class SpreadsheetSettingsView
    {
        public int Initial;
        public int Final;
        public int Step;        
        private Dataset _actualDataSet;
        #region Constructors
        /// <summary>
        /// Constructor, on
        /// </summary>
        public SpreadsheetSettingsView(Dataset dataset)
        {
            InitializeComponent();
            _actualDataSet = dataset;
            _autoDepth = new SpreadsheetSettingsViewModel(Token, dataset);
            DataContext = _autoDepth;
            AddKeyBindings<Dataset>();
        }

        SpreadsheetSettingsViewModel _autoDepth;
        #endregion

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.SaveAutoDepth:
                    IsSaved = true;
                    UpdateGlobalData();
                    Close();
                    break;
            }
        }

        private void UpdateGlobalData()
        {
            Dataset dataset = _autoDepth.CurrentObject;
            _actualDataSet.FinalDepth = dataset.FinalDepth;
            _actualDataSet.InitialDepth = dataset.InitialDepth;
            _actualDataSet.Step = dataset.Step;
        }
    }//end class
}//end namespace
