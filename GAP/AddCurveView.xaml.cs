using GAP.BL;
using GAP.MainUI.ViewModels.ViewModel;

namespace GAP
{
    public partial class AddCurveView
    {
        public AddCurveView()
        {
            InitializeComponent();
            _dataContext = new AddCurveViewModel(Token);
            DataContext = _dataContext;
            AddKeyBindings<Curve>();
            Closed += CurveNewView_Closed;
        }
        public AddCurveView(string projectID, string wellID, string datasetID, string chartID, string trackID)
        {
            InitializeComponent();
            _dataContext = new AddCurveViewModel(Token, projectID, wellID, datasetID, chartID, trackID);
            DataContext = _dataContext;
            Closed += CurveNewView_Closed;
        }

        AddCurveViewModel _dataContext;

        void CurveNewView_Closed(object sender, System.EventArgs e)
        {
            _dataContext.ReclaimMemory();
            _dataContext = null;
            DataContext = null;
        }
    }//end class
}//end namespace
