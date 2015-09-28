using GAP.Helpers;

namespace GAP.BL
{
    public interface IUndoRedoObject
    {
        void AddEntityToUndoRedoOperation<T>(T originalObject, T currentObject) where T : BaseEntity;
        void AddNewObjectToUndoStack(UndoRedoData obj);

        void AddNewObjectToRedoStack(UndoRedoData obj);
        void AddActualObjectForSorting(object actualObject);
        void UndoRedoOperationBegin(UndoRedoType undoRedoType);
        void UndoRedoOperationDone();

        void ClearMultipleItemsFromTheObject();

        void KillTheUndoRedoOperationObject();
    }
}
