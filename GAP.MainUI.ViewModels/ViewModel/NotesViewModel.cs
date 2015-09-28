using GalaSoft.MvvmLight.Command;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Ninject;
using GAP.BL;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class NotesViewModel : MultipleRemoveBaseViewModel
    {
        public NotesViewModel(string token)
            : base(token)
        {
            GetNotesInfo();
            AllRecordsSelected = false;
        }

        public bool ShouldDeleteNotes { get; set; }
        public ExtendedBindingList<NotesInfo> Notes { get; set; }

        List<NotesInfo> _notesList;
        public List<NotesInfo> NotesList
        {
            get { return _notesList; }
            set
            {
                _notesList = value;
                NotifyPropertyChanged("NotesList");
            }
        }

        NotesInfo _selectedNote;
        public NotesInfo SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                NotifyPropertyChanged("SelectedNote");
            }
        }

        ICommand _editNoteCommand;

        public ICommand EditNoteCommand
        {
            get { return _editNoteCommand ?? (_editNoteCommand = new RelayCommand(EditNote)); }
        }

        private void EditNote()
        {
            if (SelectedNote == null) return;
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.EditNote, SelectedNote);
            GetNotesInfo();
            NotifyPropertyChanged("Notes");
        }

        private void GetNotesInfo()
        {
            Notes = new ExtendedBindingList<NotesInfo>();
            Notes.ListChanged += Notes_ListChanged;
            foreach (NotesInfo info in NotesCollection.Instance.NotesList.OrderBy(u => u.DisplayIndex))
            {
                Notes.Add(new NotesInfo
                {
                    DisplayIndex = info.DisplayIndex,
                    IsNoteSelected = info.IsNoteSelected,
                    NotesText = info.NotesText.Length > 40 ? info.NotesText.Substring(0, 39) + "..." : info.NotesText,
                    UpdatedOn = info.UpdatedOn
                });
            }
            NotesList = Notes.ToList();
        }

        void Notes_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (_updatingNotesIsSelected) return;
            if (Notes.Any(u => u.IsNoteSelected) && Notes.Any(u => !u.IsNoteSelected))
                AllRecordsSelected = null;
            else if (Notes.All(u => u.IsNoteSelected))
                AllRecordsSelected = true;
            else if (Notes.All(u => !u.IsNoteSelected))
                AllRecordsSelected = false;
        }

        ICommand _addNewNoteCommand;
        public ICommand AddNewNoteCommand
        {
            get { return _addNewNoteCommand ?? (_addNewNoteCommand = new RelayCommand(AddNote)); }
        }

        private void AddNote()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.AddNote);
            GetNotesInfo();
            NotifyPropertyChanged("Notes");
        }

        ICommand _deleteRecordsCommand;
        public ICommand DeleteRecordsCommand
        {
            get { return _deleteRecordsCommand ?? (_deleteRecordsCommand = new RelayCommand(DeleteRecords, CanDelete)); }
        }

        private void DeleteRecords()
        {
            ShouldDeleteNotes = false;
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.ShouldDeleteNotes);
            if (ShouldDeleteNotes)
            {
                foreach (NotesInfo info in NotesList.Where(u => u.IsNoteSelected))
                {
                    NotesCollection.Instance.NotesList.Remove(NotesCollection.Instance.NotesList.SingleOrDefault(u => u.NotesText == info.NotesText && u.DisplayIndex == info.DisplayIndex));
                }
            }
            GetNotesInfo();
        }

        private bool CanDelete()
        {
            return NotesList.Any(u => u.IsNoteSelected);
        }

        private bool _updatingNotesIsSelected = false;

        protected override void UpdateCheckboxes(bool value)
        {
            _updatingNotesIsSelected = true;
            
            foreach (NotesInfo notes in Notes)
            {
                notes.IsNoteSelected = value;
            }
            _updatingNotesIsSelected = false;

            //  UpdateButtonText();
        }
    }//end class
}//end namespace
