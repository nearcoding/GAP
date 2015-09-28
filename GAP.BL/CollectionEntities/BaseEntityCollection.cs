using GAP.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace GAP.BL.CollectionEntities
{
    public abstract class BaseEntityCollection<T> where T : BaseEntity
    {
        //this property is required while updating an old object to keep the collection updated
        //and store this old object in undo redo stack
        public T OldObject { get; set; }

        ExtendedBindingList<T> _currentList;
        public ExtendedBindingList<T> CurrentList
        {
            get
            {
                if (_currentList == null)
                {
                    _currentList = new ExtendedBindingList<T>();
                    _currentList.ListChanged -= CurrentList_ListChanged;
                    _currentList.ListChanged += CurrentList_ListChanged;
                    _currentList.BeforeItemDelete -= CurrentList_BeforeItemDelete;
                    _currentList.BeforeItemDelete += CurrentList_BeforeItemDelete;
                }
                return _currentList;
            }
            set
            {
                _currentList = value;
            }
        }
        public BaseEntityCollection()
        {

        }

        public BaseEntityCollection(UndoRedoType undoRedoType)
        {
            CurrentList.ShouldUndoRedo = true;
            UndoRedoType = undoRedoType;
        }

        private UndoRedoType UndoRedoType { get; set; }

        public void PeformUndoRedoOperation(UndoRedoData undoRedoObject)
        {
            switch (CurrentList.OperationType)
            {
                case OperationType.None:
                    UndoRedoObject.GlobalUndoStack.Push(undoRedoObject);
                    UndoRedoObject.GlobalRedoStack.Clear();
                    break;
                case OperationType.Redo:
                    UndoRedoObject.GlobalUndoStack.Push(undoRedoObject);
                    break;
                case OperationType.Undo:
                    UndoRedoObject.GlobalRedoStack.Push(undoRedoObject);
                    break;
            }
        }

        void CurrentList_BeforeItemDelete(T objectToBeDeleted)
        {
            DeletedEntity = objectToBeDeleted;
        }

        public delegate void ItemAddedInList(T item);

        private void SetDisplayIndexOfObject(int index)
        {
            var obj = CurrentList[index];

            if (obj.DisplayIndex == 0 && CurrentList.Count > 1)
            {
                int maxDisplayIndexInList = CurrentList.Max(u => u.DisplayIndex);
                obj.DisplayIndex = maxDisplayIndexInList + 1;
            }
            GlobalCollection.Instance.SortItems(CurrentList);
        }

        private void ItemAdded(int index)
        {
           // SetDisplayIndexOfObject(index);
            var obj = AddObjectFromStack(index);
            if (obj == null) return;
            if (GlobalCollection.Instance.IsLoading) return;
            var undoRedoObject = new UndoRedoData
            {
                ActionType = ActionPerformed.ItemAdded,
                ActualObject = obj,
                EffectedType = UndoRedoType
            };
            PeformUndoRedoOperation(undoRedoObject);
        }

        protected virtual object AddObjectFromStack(int index)
        {
            return CurrentList[index];
        }

        protected virtual void ItemChanged(int index)
        {

        }

        protected virtual void DeleteLinkedObjects() { }

        private void ItemDeleted()
        {
            if (DeletedEntity == null) return;
            //DeleteLinkedObjects();
            if (!CurrentList.ShouldUndoRedo) return;
            //need  to add  the object in the appropriate stack
            var undoRedoObject = new UndoRedoData
            {
                ActionType = ActionPerformed.ItemDeleted,
                ActualObject = DeletedEntity,
                EffectedType = UndoRedoType
            };
            PeformUndoRedoOperation(undoRedoObject);
        }

        T _deletedEntity;
        public T DeletedEntity
        {
            get { return _deletedEntity; }
            set
            {
                _deletedEntity = value;
                if (value == null) return;
                if (value.GetType() == typeof(Chart))
                {
                    GlobalCollection.Instance.DeletedChart = value as Chart;
                }
                else if (value.GetType() == typeof(Track))
                {
                    GlobalCollection.Instance.DeletedTrack = value as Track;
                }
                else if (value.GetType() == typeof(Curve))
                {
                    GlobalCollection.Instance.DeletedCurve = value as Curve;
                }
                else if (value.GetType() == typeof(LithologyInfo))
                {
                    GlobalCollection.Instance.DeletedLithology = value as LithologyInfo;
                }
                else if (value.GetType() == typeof(FormationInfo))
                {
                    GlobalCollection.Instance.DeletedFormation = value as FormationInfo;
                }
            }
        }

        void CurrentList_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    ItemAdded(e.NewIndex);
                    break;
                case ListChangedType.ItemDeleted:
                    ItemDeleted();
                    break;
            }
        }

        public int GetIndexToInsertElementInTheList(int currentDisplayIndex)
        {
            var elementsWithGreaterDisplayIndex = CurrentList.Where(u => u.DisplayIndex > currentDisplayIndex);
            if (elementsWithGreaterDisplayIndex.Any())
            {
                return GettingIndexFromGreaterElements(elementsWithGreaterDisplayIndex);
            }
            else
            {
                var elementsWithSmallerDisplayIndex = CurrentList.Where(u => u.DisplayIndex < currentDisplayIndex);
                if (elementsWithGreaterDisplayIndex.Any())
                {
                    return GettingIndexFromSmallerElements(elementsWithSmallerDisplayIndex);
                }
            }
            return 0;
        }

        private int GettingIndexFromSmallerElements(IEnumerable<T> elementsWithSmallerDisplayIndex)
        {
            int largestDisplayIndex = 0;
            int maximumIndexWhichIsSmallerThanOurIndex = elementsWithSmallerDisplayIndex.Max(u => u.DisplayIndex);
            var elementsWithMaxIndex = CurrentList.Where(u => u.DisplayIndex == maximumIndexWhichIsSmallerThanOurIndex);
            foreach (var element in elementsWithMaxIndex)
            {
                if (largestDisplayIndex == 0)
                {
                    largestDisplayIndex = CurrentList.IndexOf(element);
                    continue;
                }
                var currentIndex = CurrentList.IndexOf(element);
                if (largestDisplayIndex < currentIndex)
                    largestDisplayIndex = currentIndex;
            }
            if (largestDisplayIndex > CurrentList.Count) return CurrentList.Count - 1;
            return largestDisplayIndex;
        }

        private int GettingIndexFromGreaterElements(IEnumerable<T> elementsWithGreaterDisplayIndex)
        {
            int smallestIndex = 0;
            var minimumIndexWhichIsGreaterThanOurIndex = elementsWithGreaterDisplayIndex.Min(u => u.DisplayIndex);
            var elementsWithMinIndex = CurrentList.Where(u => u.DisplayIndex == minimumIndexWhichIsGreaterThanOurIndex);

            foreach (var element in elementsWithMinIndex)
            {
                if (smallestIndex == 0)
                {
                    smallestIndex = CurrentList.IndexOf(element);
                    continue;
                }
                var currentIndex = CurrentList.IndexOf(element);
                if (smallestIndex > currentIndex)
                    smallestIndex = currentIndex;
            }

            if (smallestIndex > 0) return smallestIndex - 1;
            return smallestIndex;
        }
    }//end class
}//end namespace
