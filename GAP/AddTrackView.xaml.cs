using GAP.MainUI.ViewModels.ViewModel;

namespace GAP
{   
    public partial class AddTrackView 
    {
        public bool isSaved;

        public AddTrackView()
        {
            InitializeComponent();        
            DataContext = new AddTrackViewModel(Token);
            Closed += AddTrackView_Closed;
        }

        void AddTrackView_Closed(object sender, System.EventArgs e)
        {
            DataContext = null;
        }
   
    }//end class
}//end namespace
