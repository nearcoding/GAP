using GAP.HelperClasses;
using GAP.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using System.Text;
using System.Windows;

namespace GAP
{
    /// <summary>
    /// Interaction logic for EquationView.xaml
    /// </summary>
    public partial class EquationView
    {
        public EquationView()
        {
            InitializeComponent();
            _dataContext = new EquationViewModel(Token);
            DataContext = _dataContext;
        }

        EquationViewModel _dataContext;

        protected override void ReceiveMessage(Helpers.NotificationMessageType messageType)
        {
            base.ReceiveMessage(messageType);
            var view = new SelectDataset();
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.SelectDatasetForOperand1:
                    view.ShowDialog();
                    if (view.SelectedDataset == null) return;
                    _dataContext.SetOperand1(view.SelectedDataset.ID);
                    break;
                case NotificationMessageEnum.SelectDatasetForOperand2:
                    view.ShowDialog();
                    if (view.SelectedDataset == null) return;
                    _dataContext.SetOperand2(view.SelectedDataset.ID);
                    break;
                case NotificationMessageEnum.ShouldDeleteEquation:
                    var sbuilder = new StringBuilder();
                    sbuilder.AppendLine("This equation has been used with another equations");
                    sbuilder.AppendLine("Deleting this equation could cause a chain reaction and might remove several equations.");
                    sbuilder.Append("Are you sure you want to continue");
                    if (MessageBox.Show(sbuilder.ToString(), GlobalData.CompanyName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        _dataContext.ShouldDelete = true;
                    }
                    break;
            }
        }
    }//end class
}//end namespace
