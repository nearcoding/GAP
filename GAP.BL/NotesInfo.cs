using GAP.BL;
using System;

namespace GAP.BL
{
    public class NotesInfo : BaseEntity
    {
        public string NotesText { get; set; }
        public DateTime UpdatedOn { get; set; }
        bool _isNoteSelected;

        public bool IsNoteSelected
        {
            get { return _isNoteSelected; }
            set
            {
                _isNoteSelected = value;
                NotifyPropertyChanged("IsNoteSelected");
            }
        }
    }//end class
}//end namespace