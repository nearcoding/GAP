using GAP.MainUI.ViewModels.ViewModel;

namespace GAP
{
    public partial class AddChartView 
    {
        public AddChartView()
        {
            InitializeComponent();
            DataContext = new AddChartViewModel(Token);
            Closed += AddChartView_Closed;
        }

        void AddChartView_Closed(object sender, System.EventArgs e)
        {
            DataContext = null;
        }
    }//end class
}//end namespace
