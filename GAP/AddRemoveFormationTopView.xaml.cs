using System.Windows;
using Ninject;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using GAP.BL;

namespace GAP
{
    /// <summary>
    /// Interaction logic for AddRemoveFormationTopView.xaml
    /// </summary>
    public partial class AddRemoveFormationTopView
    {
        public AddRemoveFormationTopView()
        {
            InitializeComponent();
            _dataContext = new AddRemoveFormationTopViewModel(Token);
            DataContext = _dataContext;
            AddKeyBindings<FormationInfo>();
            Closed += AddRemoveFormationTopView_Closed;
        }

        void AddRemoveFormationTopView_Closed(object sender, System.EventArgs e)
        {
            _dataContext.Dispose();
            _dataContext = null;
        }

        AddRemoveFormationTopViewModel _dataContext;
        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.ShouldDeleteFormation:
                    DeleteFormation();
                    break;
                case NotificationMessageEnum.SaveFormations:
                    SaveFormations();
                    break;
            }
        }

        private void SaveFormations()
        {
            MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("FormationsSavedSuccessfully")
                , GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
            GlobalData.ShouldSave = true;
            Close();
            //FormationTopHelper.ShowHideFormation();
        }

        private void DeleteFormation()
        {
            var result = MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("DoYouWantToDeleteTheSelectedFormations"),
                GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
                _dataContext.ShouldDeleteFormation = true;
            else
                _dataContext.ShouldDeleteFormation = false;
        }
    }//end class
}//end namespace
