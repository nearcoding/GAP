using System.Collections.Generic;

namespace GAP.BL
{
    public class UndoRedoObject 
    {
        private static UndoRedoObject _instance = new UndoRedoObject();

        private UndoRedoObject()
        {
            GlobalUndoStack = new Stack<UndoRedoData>();
            GlobalRedoStack = new Stack<UndoRedoData>();
        }

        public static UndoRedoObject Instance
        {
            get
            {
                return _instance;
            }
        }

        static UndoRedoData _undoRedoLiveObject;
        public static UndoRedoData UndoRedoLiveObject
        {
            get { return _undoRedoLiveObject; }
            set { _undoRedoLiveObject = value; }
        }

        public static Stack<UndoRedoData> GlobalUndoStack { get; set; }
        public static Stack<UndoRedoData> GlobalRedoStack { get; set; }

    }//end class
}//end namespace
