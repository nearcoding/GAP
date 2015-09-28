using GAP.BL;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class SubDatasetViewModel : BaseViewModel<SubDataset>
    {
        public SubDatasetViewModel(string token, SubDataset subDataset)
            : base(token)
        {
            CurrentObject = subDataset;
            CurveHeaderText = string.Format("{0}({1})", subDataset.Name, subDataset.IsNCT ? "NCT" : "SHPT");
        }

        string _curveHeaderText;
        public string CurveHeaderText
        {
            get { return _curveHeaderText; }
            set
            {
                _curveHeaderText = value;
                NotifyPropertyChanged("CurveHeaderText");
            }
        }
    }//end class
}//end namespace
