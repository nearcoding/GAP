using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.MainUI.ViewModels.ViewModel;

namespace GAP
{
    public partial class AddNewNoteView
    {
        public AddNewNoteView()
        {
            InitializeComponent();
            Loaded += AddNewNoteView_Loaded;
        }

        void AddNewNoteView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (SelectedNote == null)            
                _dataContext = new AddNewNoteViewModel(Token);
            else
                _dataContext = new AddNewNoteViewModel(Token, SelectedNote);
            
            DataContext = _dataContext;
            AddKeyBindings<NotesInfo>();
        }

        public NotesInfo SelectedNote { get; set; }

        AddNewNoteViewModel _dataContext;
    }//end class
}//end namespace
