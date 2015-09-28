using GAP.Helpers;
using GAP.BL.CollectionEntities;

namespace GAP.BL
{
    internal class AnnotationsCollection : BaseEntityCollection<AnnotationInfo>
    {
        public AnnotationsCollection()
            :base(UndoRedoType.Annotations)
        {

        }      
    }//end class
}//end namespace