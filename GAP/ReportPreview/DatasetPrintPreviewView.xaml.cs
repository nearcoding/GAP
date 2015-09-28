using GAP.Reports;
using System.Windows;

namespace GAP.ReportPreview
{
    /// <summary>
    /// Interaction logic for DatasetPrintPreviewView.xaml
    /// </summary>
    public partial class DatasetPrintPreviewView : Window
    {
        public DatasetPrintPreviewView(rptDatasetPrinting report)
        {
            InitializeComponent();
            rpt = report;
            Loaded += DatasetPrintPreviewView_Loaded;            
        }
        rptDatasetPrinting rpt;
        void DatasetPrintPreviewView_Loaded(object sender, RoutedEventArgs e)
        {            
            ReportViewer1.ReportSource = rpt;
        }

    }//end class
}//end namespace
