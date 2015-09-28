using Abt.Controls.SciChart;
using GAP.BL;
using GAP.HelperClasses;
using GAP.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using System.Linq;

namespace GAP
{
    /// <summary>
    /// Interaction logic for ZoomDialogView.xaml
    /// </summary>
    public partial class ZoomDialogView
    {
        public ZoomDialogView()
        {
            InitializeComponent();
            DataContext = new ZoomDialogViewModel(Token);
            AddKeyBindings<BaseEntity>();
        }

    }//end class
}//end namespace
