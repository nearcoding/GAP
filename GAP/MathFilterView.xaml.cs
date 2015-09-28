using GAP.MainUI.ViewModels.ViewModel;

namespace GAP
{
    /// <summary>
    /// Interaction logic for CalculateMathFilterView.xaml
    /// </summary>
    public partial class MathFilterView 
    {
        public MathFilterView()
        {
            InitializeComponent();
            DataContext = new MathFilterViewModel(Token);            
        }
    }//end class
}//end namespace
