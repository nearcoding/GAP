using GAP.BL;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class ChartRemoveInfo : BaseEntity
    {
        bool _isChartSelected;
        public bool IsChartSelected
        {
            get { return _isChartSelected; }
            set
            {
                _isChartSelected = value;
                NotifyPropertyChanged("IsChartSelected");
            }
        }

        public string Chart { get; set; }

        public string Tracks { get; set; }

        public string Curves { get; set; }
    }//end class
}//end namespace
