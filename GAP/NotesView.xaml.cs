using GAP.HelperClasses;
using GAP.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using Ninject;
using System.Windows;
using GAP.MainUI.ViewModels.Helpers;
using GAP.BL;

namespace GAP
{
    /// <summary>
    /// Interaction logic for NotesView.xaml
    /// </summary>
    public partial class NotesView 
    {
        public NotesView()
        {
            InitializeComponent();
            _dataContext = new NotesViewModel(Token);
            DataContext = _dataContext;
        }

        NotesViewModel _dataContext;

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch(messageType.MessageType)
            {
                case NotificationMessageEnum.AddNote:
                    AddNewNoteView view = new AddNewNoteView();
                    view.ShowDialog();
                    break;
                case NotificationMessageEnum.ShouldDeleteNotes:
                    var result = MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("DoYouWantToRemoveTheSelectedNotes"),
                        GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes) _dataContext.ShouldDeleteNotes = true;
                    break;
                case NotificationMessageEnum.EditNote:
                    AddNewNoteView notesView = new AddNewNoteView();
                    notesView.SelectedNote = messageType.MessageObject as NotesInfo;
                    notesView.ShowDialog();
                    break;
            }
        }
    }//end class
}//end namespace
