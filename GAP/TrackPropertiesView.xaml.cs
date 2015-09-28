using GAP.BL;
using GAP.MainUI.ViewModels.ViewModel;

namespace GAP
{
    /// <summary>
    /// Interaction logic for TrackPropertiesView.xaml
    /// </summary>
    public partial class TrackPropertiesView 
    {
        public TrackPropertiesView()
        {
            InitializeComponent();
            DataContext = new TrackPropertiesViewModel(Token);
            AddKeyBindings<BaseEntity>();
        }//end class
    }//end namespace
}