using GAP.BL.CollectionEntities;
using GAP.Helpers;
using System.Linq;

namespace GAP.BL.CollectionEntitiess
{
    internal class DatasetCollection : BaseEntityCollection<Dataset>
    {
        public DatasetCollection()
            : base(UndoRedoType.Dataset)
        {

        }

        protected override void DeleteLinkedObjects()
        {           
            //get all the curves and check which one needs to be deleted
            var curves = HelperMethods.Instance.GetCurvesByDatasetID(DeletedEntity.ID);
            while (curves.Any())
            {
                var curve = curves.First();
                var track = HelperMethods.Instance.GetTrackByID(curve.RefTrack);
                track.Curves.ShouldUndoRedo = false;
                track.Curves.Remove(curve);
                track.Curves.ShouldUndoRedo = true;                
            }
        }
    }//end class
}//end namespace
