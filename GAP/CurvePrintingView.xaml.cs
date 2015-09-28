using GAP.BL;
using GAP.CustomControls;
using GAP.HelperClasses;
using GAP.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.ReportPreview;
using GAP.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Ninject;
using Abt.Controls.SciChart.Visuals;
namespace GAP
{
    public partial class CurvePrintingView
    {
        public CurvePrintingView()
        {
            InitializeComponent();
            _dataContext = new CurvePrintingViewModel(Token);
            DataContext = _dataContext;
            SettingsOfPrinter = new PrinterSettings();
            AddKeyBindings<BaseEntity>();
        }

        private void PrintWithDataset(bool printPreview, DataSet ds)
        {
            var rpt = new rptCurvePrintingWithDataset();
            rpt.SetDataSource(ds);
            if (printPreview)
                new CurvePrintPreview(rpt).Show();
            else
                rpt.PrintToPrinter(SettingsOfPrinter, SettingsOfPrinter.DefaultPageSettings, false);
        }

        private void Print(bool printPreview, DataSet ds)
        {
            var rpt = new rptCurvePrinting();
            rpt.SetDataSource(ds);
            if (printPreview)
                new CurvePrintPreview(rpt).Show();
            else
                rpt.PrintToPrinter(SettingsOfPrinter, SettingsOfPrinter.DefaultPageSettings, false);
        }

        private static string GetCurveNames(TrackToShow trackToShow)
        {
            var sBuilder = new StringBuilder();
            foreach (var curve in trackToShow.TrackObject.Curves)
            {
                if (curve.IsEntitySelected == null || curve.IsEntitySelected.Value)
                {
                    if (curve.RefDataset.ToLower() == "lithology" || curve.RefWell.ToLower() == "lithology") continue;
                    sBuilder.AppendLine(string.Format("{0}/{1}", HelperMethods.Instance.GetWellByID(curve.RefWell).Name,
                                                                 HelperMethods.Instance.GetDatasetByID(curve.RefDataset).Name));
                }
            }
            return sBuilder.ToString();
        }

        private byte[] GetImageFile(TrackToShow track)
        {
            var folder = IoC.Kernel.Get<IGlobalDataModel>().GetAppDataFolder();

            if (!Directory.Exists(Path.Combine(folder, "Temp")))
                Directory.CreateDirectory(Path.Combine(folder, "Temp"));

            string tempFolder = Path.Combine(folder, "Temp");
            string fileName = Path.Combine(tempFolder, Guid.NewGuid().ToString() + ".png");

            var surface = new SurfaceTestWindow(track);
            surface.Show();
            surface.sciChartExtended.ExportToFile(fileName, ExportType.Png);
            surface.Close();
            var fs = new FileStream(fileName, FileMode.Open);
            var bReader = new BinaryReader(fs);
            byte[] imgbyte = bReader.ReadBytes(Convert.ToInt32((fs.Length)));

            return imgbyte;
        }

        private void ReportWithoutDataset(TrackToShow trackToShow, bool printPreview)
        {
            var ds = GetDataset(trackToShow);
            Print(printPreview, ds);
        }

        private DataSet GetDataset(TrackToShow trackToShow)
        {
            var dataTable = GetDataTableWithBasicInfoPrinting(trackToShow);
            var ds = new DataSet();
            ds.Tables.Add(dataTable);
            return ds;
        }

        private void ReportWithDataset(TrackToShow trackToShow, bool printPreview)
        {
            var dataTable = GetDataTableWithBasicInfoPrinting(trackToShow);
            var curves = trackToShow.TrackObject.Curves.Where(u => u.RefProject != "Lithology");
            var cls = new DatasetPrintingClass(null, true);
            var ds = GetDataset(trackToShow);
            cls.GetDatasetFromCurve(curves, ds);
            PrintWithDataset(printPreview, ds);
        }

        private DataTable GetDataTableWithBasicInfoPrinting(TrackToShow track)
        {
            CreateDataTable();
            var dRow = _dataTable.NewRow();
            dRow["Image"] = GetImageFile(track);
            dRow["CompanyName"] = GlobalData.CompanyName;
            dRow["Chart"] = HelperMethods.Instance.GetChartByID(track.TrackObject.RefChart).Name;
            dRow["Track"] = track.TrackObject.Name;
            dRow["Curves"] = GetCurveNames(track);
            _dataTable.Rows.Add(dRow);
            return _dataTable;
        }
        DataTable _dataTable;
        private void CreateDataTable()
        {
            _dataTable = new DataTable("dtCurvePrinting");
            _dataTable.Columns.Add(new DataColumn("Image", System.Type.GetType("System.Byte[]")));
            _dataTable.Columns.Add(new DataColumn("CompanyName", typeof(string)));
            _dataTable.Columns.Add(new DataColumn("Chart", typeof(string)));
            _dataTable.Columns.Add(new DataColumn("Track", typeof(string)));
            _dataTable.Columns.Add(new DataColumn("Curves", typeof(string)));
        }

        CurvePrintingViewModel _dataContext;
        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            TrackToShow trackToShow = null;
            if (messageType.MessageObject != null)
                trackToShow = messageType.MessageObject as TrackToShow;
            if (trackToShow == null) return;
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.PrintChart:
                    if (!_dataContext.PrintDataset)
                        ReportWithoutDataset(trackToShow, false);
                    else
                        ReportWithDataset(trackToShow, false);
                    break;
                case NotificationMessageEnum.PrintPreviewChart:
                    if (!_dataContext.PrintDataset)
                        ReportWithoutDataset(trackToShow, true);
                    else
                        ReportWithDataset(trackToShow, true);
                    break;
                case NotificationMessageEnum.PrinterSettings:
                    PrinterSettings();
                    break;
            }
        }

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