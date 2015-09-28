using System.Linq;
using Abt.Controls.SciChart;
using GAP.BL.CollectionEntities;
using GAP.Helpers;
namespace GAP.BL
{
    public class Chart : BaseEntity
    {

        public Chart()
        {
            TrackCollection collection = new TrackCollection();
            Tracks = collection.CurrentList;
            TrackCollection = collection;
            FormationCollection formationCollection = new FormationCollection();
            Formations = formationCollection.CurrentList;
            FormationCollection = formationCollection;
        }

        ExtendedBindingList<Track> _tracks;
        internal TrackCollection TrackCollection { get; set; }
        public ExtendedBindingList<Track> Tracks
        {
            get { return _tracks; }
            set
            {
                _tracks = value;
                NotifyPropertyChanged("Tracks");
            }
        }

        public ExtendedBindingList<FormationInfo> Formations { get; set; }

        internal FormationCollection FormationCollection { get; set; }
    }//end class
}//end namespace
