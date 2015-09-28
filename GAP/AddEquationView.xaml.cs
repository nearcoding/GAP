using GAP.MainUI.ViewModels.ViewModel;

namespace GAP
{
    /// <summary>
    /// Interaction logic for AddEquationView.xaml
    /// </summary>
    public partial class AddEquationView
    {
        public AddEquationView()
        {
            InitializeComponent();
            DataContext = new AddEquationViewModel(Token);

        }
    }//end class
}//end namespace
