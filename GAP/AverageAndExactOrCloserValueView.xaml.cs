using GAP.BL;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;

namespace GAP
{
    /// <summary>
    /// Interaction logic for AutoDepthImport.xaml
    /// </summary>
    public partial class AverageAndExactOrCloserValueView
    {
        public AverageAndExactOrCloserValueView(decimal first, decimal last)
        {
            InitializeComponent();

            _dataContext = new AverageAndExactOrCloserValueViewModel(Token, first, last);            
            DataContext = _dataContext;
            AddKeyBindings<BaseEntity>();
        }

        AverageAndExactOrCloserValueViewModel _dataContext;

        public ImportDataType ImportDataType { get; set; }

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch(messageType.MessageType)
            {
                case NotificationMessageEnum.SaveDepthImportView:
                    ImportDataType = _dataContext.IsArithmeticAverageChecked ? ImportDataType.AverageChecked : ImportDataType.ExactOrCloserValue;
                    IsSaved = true;
                    Close();
                    break;
            }
        }
    }//end class
}//ennd namespace
