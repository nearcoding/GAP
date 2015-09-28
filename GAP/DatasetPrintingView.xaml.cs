using GAP.BL;
using GAP.HelperClasses;
using GAP.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.ReportPreview;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace GAP
{
    /// <summary>
    /// Interaction logic for DatasetPrintingView.xaml
    /// </summary>
    public partial class DatasetPrintingView
    {
        public DatasetPrintingView()
        {
            InitializeComponent();
            _dataContext = new DatasetPrintingViewModel(Token);
            DataContext = _dataContext;
            AddKeyBindings<BaseEntity>();
            SettingsOfPrinter = new PrinterSettings();
        }

        DatasetPrintingViewModel _dataContext;

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.PrintPreviewDataset:
                    PrintReports(messageType.MessageObject as IEnumerable<Dataset>);
                    break;
                case NotificationMessageEnum.PrintDataset:
                    PrintReports(messageType.MessageObject as IEnumerable<Dataset>, true);
                    break;
                case NotificationMessageEnum.PrinterSettings:
                    PrinterSettings();
                    break;
            }
        }

        private void PrintReports(IEnumerable<Dataset> lstDataset, bool shouldPrint = false)
        {
            foreach (var dataset in lstDataset)
            {
                var cls = new DatasetPrintingClass(dataset, _dataContext.IncludeSpreadsheetData);
                if (!shouldPrint)
                {
                    var view = new DatasetPrintPreviewView(cls.Report);
                    view.Show();
                }
                else
                {
                    cls.Report.PrintToPrinter(SettingsOfPrinter, SettingsOfPrinter.DefaultPageSettings, false);
                }
            }
        }
        public IEnumerable<Dataset> SelectedDatasets { get; set; }

        PrinterSettings SettingsOfPrinter { get; set; }

        private void PrinterSettings()
        {
            System.Windows.Forms.PrintDialog d = new System.Windows.Forms.PrintDialog();
            var result = d.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
            {
                SettingsOfPrinter = d.PrinterSettings;
            }
        }
    }//end class
}//end namespace
