using System.Windows;
using GAP.BL;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;
using GAP.ExtendedControls;
using System.Windows.Input;

namespace GAP
{
    public partial class DatasetView : BaseWindow<Dataset>
    {
        #region Internal Variables

        DatasetViewModel _dataContext;

        #endregion

        #region Constructors

        public DatasetView(Dataset dataset)
        {
            InitializeComponent();
            
            if (dataset == null)
                _dataContext = new DatasetViewModel(Token);
            else
                _dataContext = new DatasetViewModel(Token, dataset);
            DataContext = _dataContext;
            Loaded += DatasetView_Loaded;
        }
        public DatasetView()
        {
            InitializeComponent();
            _dataContext = new DatasetViewModel(Token);
            DataContext = _dataContext;
            Loaded += DatasetView_Loaded;
        
        }
        void DatasetView_Loaded(object sender, RoutedEventArgs e)
        {
            KeyBinding saveKeyBinding =
              new KeyBinding(_dataContext.SaveDatasetCommand, Key.S, ModifierKeys.Control);
            this.InputBindings.Add(saveKeyBinding);

        }

        #endregion

        #region Events

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case (NotificationMessageEnum.SaveDataset):
                    OpenSpreadsheet();
                    break;
                case(NotificationMessageEnum.AddCurve):
                   // GlobalData.CurveObject = messageType.MessageObject as Curve;   
                 AddCurve(messageType.MessageObject as Curve);
                    break;
                //case (NotificationMessageEnum.UpdateDatasetWithMinMaxWarning):
                //    UpdateDatasetWithMinMaxWarning();
                //    break;
            }
        }

        Curve curveObject = new Curve();
        private void AddCurve(Curve curve)
        {
            new AddCurveView(curve.RefProject, curve.RefWell, curve.RefDataset, curve.RefChart, curve.RefTrack);
        }

        private void OpenSpreadsheet()
        {
            if (!OpenSpreadsheet(_dataContext.CurrentObject)) return;
            GlobalData.ShouldSave = true;
            Close();
        }

        private bool OpenSpreadsheet(Dataset dataSet)
        {
            var spreadSheetView = new MaintainSpreadsheetView(dataSet, false);
            spreadSheetView.ShowDialog();
            return spreadSheetView.IsSaved;
        }
        #endregion
    }//end class
}//end namespace
