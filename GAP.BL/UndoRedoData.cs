using GAP.Helpers;

namespace GAP.BL
{
    public class UndoRedoData
    {
        public ActionPerformed ActionType { get; set; }
        public UndoRedoType EffectedType { get; set; }
             
        public object ActualObject { get; set; }
    }//end class
}//end namespace
