
using GAP.BL;
namespace GAP.MainUI.ViewModels.Helpers
{
    public class TrackSourceForMultipleSelection:BaseEntity
    {
        public TrackSourceForMultipleSelection()
        {
            IsTrackSelected = false; ;        
        }
        public string RefChart { get; set; }

        bool _isTrackSelected;
        public string TrackName { get; set; }
        public bool IsTrackSelected
        {
            get { return _isTrackSelected; }
            set
            {
                _isTrackSelected = value;
                NotifyPropertyChanged("IsTrackSelected");
            }
        }
    }//end class
}//end namespace
