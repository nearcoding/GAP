using GAP.BL;
using GAP.MainUI.ViewModels.ViewModel;

namespace GAP
{
    public partial class WellView 
    {
        public WellView(Well wellObject)
        {
            InitializeComponent();
            DataContext = new WellViewModel(Token, wellObject);
        }

        public WellView()
        {
            InitializeComponent();
            DataContext = new WellViewModel(Token);
            ScreenType = typeof(Well);
        }
    }//end class
}//end namespace
