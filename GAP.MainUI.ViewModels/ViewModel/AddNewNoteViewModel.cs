using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using System;
using System.Linq;
using AutoMapper;
using Ninject;
using GAP.BL;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class AddNewNoteViewModel : BaseViewModel<NotesInfo>
    {
        public AddNewNoteViewModel(string token)
            : base(token)
        {
         
        }

        public AddNewNoteViewModel(string token, NotesInfo selectedNote)
            : base(token)
        {
            if (selectedNote == null) return;

            Mapper.CreateMap<NotesInfo, NotesInfo>();
            CurrentObject = Mapper.Map<NotesInfo, NotesInfo>(selectedNote);
            NotesText = selectedNote.NotesText;
        }

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(NotesText);
        }

        string _notesText;
        public string NotesText
        {
            get { return _notesText; }
            set
            {
                _notesText = value;
                NotifyPropertyChanged("NotesText");
            }
        }

        public override void Save()
        {
            if (CurrentObject == null)
            {
                NotesCollection.Instance.NotesList.Add(new NotesInfo
                    {
                        DisplayIndex = NotesCollection.Instance.NotesList.Any() ? NotesCollection.Instance.NotesList.Max(u => u.DisplayIndex) + 1 : 1,
                        NotesText = NotesText,
                        UpdatedOn = DateTime.Now
                    });
            }
            else
            {
                var noteToUpdate =  NotesCollection.Instance.NotesList.SingleOrDefault(u => u.NotesText == CurrentObject.NotesText
                    && u.DisplayIndex == CurrentObject.DisplayIndex && u.UpdatedOn == CurrentObject.UpdatedOn);
                if (noteToUpdate != null)
                {
                    noteToUpdate.UpdatedOn = DateTime.Now;
                    noteToUpdate.NotesText = NotesText;
                }
            }
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }
    }//end class
}//end namespace
