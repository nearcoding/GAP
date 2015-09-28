using GAP.Helpers;

namespace GAP.BL.CollectionEntities
{
    internal class SubDatasetCollection:BaseEntityCollection<SubDataset>
    {
        public SubDatasetCollection()
            : base(UndoRedoType.SubDataset)
        {

        }

    }//end class
}//end namespace
