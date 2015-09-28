using GAP.BL;
using GAP.ExtendedControls;
using GAP.MainUI.ViewModels.ViewModel;

namespace GAP
{
    /// <summary>
    /// Interaction logic for SubDatasetView.xaml
    /// </summary>
    public partial class SubDatasetView : BaseWindow
    {
        public SubDatasetView(SubDataset subDataset)
        {
            InitializeComponent();
            DataContext = new SubDatasetViewModel(Token, subDataset);
            AddKeyBindings<SubDataset>();
        }

    }//end class
}//end namespace
