using GAP.MainUI.ViewModels.ViewModel;

namespace GAP
{
    public partial class ProjectView 
    {
        public ProjectView(string projectName)
        {
            InitializeComponent();            
            DataContext =  new ProjectViewModel(Token, projectName);
        }
        public ProjectView()
        {
            InitializeComponent();
            DataContext = new ProjectViewModel(Token);
        }
    }//end class
}//end namespace