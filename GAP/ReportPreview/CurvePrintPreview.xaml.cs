using GAP.CustomControls;
using GAP.Reports;
using System.Windows;

namespace GAP.ReportPreview
{
    /// <summary>
    /// Interaction logic for CurvePrintPreview.xaml
    /// </summary>
    public partial class CurvePrintPreview : Window
    {
         public CurvePrintPreview(rptCurvePrinting rpt)
        {
            InitializeComponent();
            this.ReportViewer1.ReportSource = rpt;
        }

         public CurvePrintPreview(rptCurvePrintingWithDataset rpt)
        {
            InitializeComponent();
            this.ReportViewer1.ReportSource = rpt;
        }
    }//end class
}//end namespace
