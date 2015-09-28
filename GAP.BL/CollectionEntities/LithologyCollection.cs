
namespace GAP.BL.CollectionEntities
{
    internal class LithologyCollection : BaseEntityCollection<LithologyInfo>
    {
        public LithologyCollection()
        {
                  
        }

        static LithologyCollection _instance = new LithologyCollection();

        public static LithologyCollection Instance { get { return _instance; } }

        //protected override void ItemDeleted()
        //{
        // //we  dont have to remove the annotation here, we just need to remove the associated dataseries
        //}


    }//end class
}//end namespace
