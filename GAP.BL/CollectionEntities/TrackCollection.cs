using GAP.Helpers;

namespace GAP.BL.CollectionEntities
{
    internal class TrackCollection : BaseEntityCollection<Track>
    {
        internal TrackCollection()
            : base(UndoRedoType.Track)
        {
        }
    }//end class
}//end namespace
