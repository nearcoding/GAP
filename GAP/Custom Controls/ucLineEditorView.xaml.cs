using GAP.BL;
using GAP.MainUI.ViewModels.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace GAP.Custom_Controls
{
    /// <summary>
    /// Interaction logic for ucLineEditorView.xaml
    /// </summary>
    public partial class ucLineEditorView : UserControl
    {
        string _token;
        public ucLineEditorView(string token)
        {
            InitializeComponent();
            _token = token;
            Loaded += ucLineEditorView_Loaded;
        }

        void ucLineEditorView_Loaded(object sender, RoutedEventArgs e)
        {
            _dataContext = DataContext as LineEditorViewModel;
          //  _dataContext.SourceDatasetStep = SourceDatasetStep;
        }

        LineEditorViewModel _dataContext;

        SubDataset _subDataset;
        public SubDataset SubDataset
        {
            get { return _subDataset; }
            set
            {
                _subDataset = value;
                _dataContext.SubDataset = value;
            }
        }

        public int SourceDatasetStep
        {
            get;
            set;
        }
    }//end class
}//end namespace
