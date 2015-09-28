using GAP.BL;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class ChartSourceForMultipleSelection : BaseEntity
    {
        public ChartSourceForMultipleSelection()
        {
            IsChartSelected = false;
        }
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
        public string ChartName { get; set; }

    }//end class
}//end namespace
