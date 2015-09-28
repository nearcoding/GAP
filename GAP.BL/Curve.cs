
namespace GAP.BL
{
    public class Curve : BaseEntity
    {
        public Curve()
        {
            IsSeriesVisible = true;
        }
             
        public string RefDataset { get; set; }

        public string RefWell { get; set; }

        public string RefProject { get; set; }

        public string RefTrack{get;set;}

        public string RefChart { get; set; }

        bool _isSeriesVisible;
        public bool IsSeriesVisible
        {
            get { return _isSeriesVisible; }
            set
            {
                _isSeriesVisible = value;
                NotifyPropertyChanged("IsSeriesVisible");
            }
        }
    }//end class
}//end namespace
