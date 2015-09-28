
using GAP.BL;
namespace GAP.MainUI.ViewModels.Helpers
{
    public class InfoData<T> : BaseEntity
    {
        public T InfoObject { get; set; }
        public bool IsAdded { get; set; }
    }//end class
}//end namespace
