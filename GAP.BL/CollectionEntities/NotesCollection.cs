using GAP.Helpers;

namespace GAP.BL
{
    public class NotesCollection
    {
        private NotesCollection()
        {
            NotesList = new ExtendedBindingList<NotesInfo>();
        }

        static NotesCollection _instance = new NotesCollection();

        public ExtendedBindingList<NotesInfo> NotesList { get; set; }
        public static NotesCollection Instance { get { return _instance; } }

    }//end class
}//end namespace
