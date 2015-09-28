using GAP.BL;
using GAP.MainUI.ViewModels.ViewModel;
namespace GAP
{
    /// <summary>
    /// Interaction logic for CustomizeToolbarView.xaml
    /// </summary>
    public partial class CustomizeToolbarView
    {
        public CustomizeToolbarView()
        {
            InitializeComponent();
            DataContext = new CustomizeToolbarViewModel(Token);
            AddKeyBindings<BaseEntity>();          
        }
    }//end class
}//end namespace
