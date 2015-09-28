using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GAP.Helpers
{
    public class ExtendedBindingList<T> : BindingList<T>
    {
        //void ExtendedBindingList_AddingNew(object sender, AddingNewEventArgs e)
        //{
        //    BeforeItemAdded(e); 
        //}

        //public delegate void BeforeItemAddedDelegate(AddingNewEventArgs e);
        //public event BeforeItemAddedDelegate BeforeItemAdded;


        public delegate void DeletingItemDelegate(T deletedObject);
        public event DeletingItemDelegate DeletingItem;
        protected override void RemoveItem(int index)
        {
            T itemToRemoved = Items[index];
            if (itemToRemoved != null)
            {
                if (BeforeItemDelete != null)
                {
                    BeforeItemDelete(itemToRemoved);
                }
                if (DeletingItem != null)
                    DeletingItem(itemToRemoved);
            }
            base.RemoveItem(index);
        }

        public bool ShouldUndoRedo { get; set; }
        /// <summary>
        /// by default this flag is false so we have added the object in undo stack
        /// if we ar doing undo then we set this flag to true thus the object will be added to redo stack
        /// </summary>

        OperationType _operationType;
        public OperationType OperationType
        {
            get { return _operationType; }
            set
            {
                _operationType = value;
            }
        }

        public ExtendedBindingList()
            : base()
        {
          //AddingNew += ExtendedBindingList_AddingNew;
        }

        public ExtendedBindingList(List<T> lst)
            : base(lst)
        {

        }
        public delegate void BeforeDeleteDelegate(T objectToBeDeleted);

        public event BeforeDeleteDelegate BeforeItemDelete;

       
    }//end class
}//end namespace
