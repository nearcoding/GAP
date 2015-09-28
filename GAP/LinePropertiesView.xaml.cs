using GAP.BL;
using GAP.Custom_Controls;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace GAP
{
    /// <summary>
    /// Interaction logic for LineProperties.xaml
    /// </summary>
    public partial class LinePropertiesView
    {
        public LinePropertiesView()
        {
            InitializeComponent();
            _dataContext = new LinePropertiesViewModel(Token);
            DataContext = _dataContext;
            AddKeyBindings<SubDataset>();
        }

        public LinePropertiesView(SubDataset subDataset)
        {
            InitializeComponent();
            _dataContext = new LinePropertiesViewModel(subDataset, Token);
            DataContext = _dataContext;
            AddKeyBindings<SubDataset>();
        }
        LinePropertiesViewModel _dataContext;

        protected override void ReceiveMessage(Helpers.NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case Helpers.NotificationMessageEnum.OpenFlyoutWindow:
                    GlobalData.ShouldSave = true;
                    Close();
                    var flyoutControl = (GlobalData.MainWindow.Flyouts.Items[0] as Flyout);
                    var ucLineEditorView = flyoutControl.Content as ucLineEditorView;
                    ucLineEditorView.SubDataset = _dataContext.CurrentObject;
                    //ucLineEditorView.SourceDatasetStep = _dataContext.SelectedDataset.Step;
                    flyoutControl.IsOpen = true;
                    GlobalDataModel.Instance.IsSubDatasetOpen = true;
                    break;
            }
        }
    }//end class
}//end namespace
