using GAP.Helpers;
using System.Linq;

namespace GAP.BL.CollectionEntities
{
    internal class WellCollection : BaseEntityCollection<Well>
    { 
        public WellCollection()
            :base(UndoRedoType.Well)
        {
             
        }        
    }//end class
}//end namespace