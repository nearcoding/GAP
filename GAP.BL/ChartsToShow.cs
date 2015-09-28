using Abt.Controls.SciChart;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
namespace GAP.BL
{
    public class SubBaseEntity : INotifyPropertyChanged
    {
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
    public class ChartToShow : SubBaseEntity
    {
        public bool HasTracks
        {
            get { return Tracks.Any(); }
        }

        void Tracks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("TracksCount");
            NotifyPropertyChanged("Tracks");
        }
        string _selectedTrack;
        public string SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                _selectedTrack = value;
                NotifyPropertyChanged("SelectedTrack");
            }
        }
        IRange _visibleRangeLimitYAxis;
        public IRange VisibleRangeLimitYAxis
        {
            get { return _visibleRangeLimitYAxis; }
            set
            {
                if (_visibleRangeLimitYAxis == value) return;
                _visibleRangeLimitYAxis = value;
                NotifyPropertyChanged("VisibleRangeLimitYAxis");
            }
        }
        
        IRange _visibleRangeYAxis;
        public IRange VisibleRangeYAxis
        {
            get { return _visibleRangeYAxis; }
            set
            {
                if (_visibleRangeYAxis == value) return;
                _visibleRangeYAxis = value;
                NotifyPropertyChanged("VisibleRangeYAxis");
            }
        }

        public Chart ChartObject { get; set; }

        ObservableCollection<TrackToShow> _tracks;
        public ObservableCollection<TrackToShow> Tracks
        {
            get
            {
                if (_tracks == null)
                {
                    _tracks = new ObservableCollection<TrackToShow>();
                    Tracks.CollectionChanged += Tracks_CollectionChanged;
                }
                return _tracks;
            }
            set
            {
                _tracks = value;
            }
        }
    }
}//end namespace
