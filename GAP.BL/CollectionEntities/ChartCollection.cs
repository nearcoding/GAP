using GAP.Helpers;
using GAP.BL.CollectionEntities;
namespace GAP.BL
{
    internal class ChartCollection : BaseEntityCollection<Chart>
    {
        private ChartCollection()
            : base(UndoRedoType.Chart)
        {

        }
        static ChartCollection _instance = new ChartCollection();

        public static ChartCollection Instance { get { return _instance; } }

    }//end class
}//end namespace
