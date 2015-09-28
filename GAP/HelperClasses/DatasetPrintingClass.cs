using GAP.BL;
using GAP.Reports;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;

namespace GAP.HelperClasses
{
    public class DatasetPrintingClass
    {
        public DatasetPrintingClass(Dataset dataset, bool includeSpreadsheetData = false)
        {
            _includeSpreadsheetData = includeSpreadsheetData;

            if (dataset == null) return;
            var ds = GetReportDatasetFromGAPDataset(dataset);
            Report = new rptDatasetPrinting();
            Report.SetDataSource(ds);
        }

        bool _includeSpreadsheetData;

        private DataTable GetDepthCurvesTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Depth", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Curve", typeof(decimal)));
            return dt;
        }

        private DataTable GetDepthCurvesTableExtended()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Depth", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Curve", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Project", typeof(string)));
            dt.Columns.Add(new DataColumn("Well", typeof(string)));
            dt.Columns.Add(new DataColumn("Dataset", typeof(string)));
            return dt;
        }

        private void GetRowsExtended(DataTable dt, Dataset dataset)
        {
            foreach (var row in dataset.DepthAndCurves)
            {
                DataRow dataRow = dt.NewRow();
                dataRow["Depth"] = row.Depth;
                dataRow["Curve"] = row.Curve;
                dataRow["Project"] = HelperMethods.Instance.GetProjectByID(dataset.RefProject).Name;
                dataRow["Well"] = HelperMethods.Instance.GetWellByID(dataset.RefWell).Name;
                dataRow["Dataset"] = dataset.Name;
                dt.Rows.Add(dataRow);
            }
        }

        private void GetRows(DataTable dt, Dataset dataset)
        {
            foreach (var row in dataset.DepthAndCurves)
            {
                DataRow dataRow = dt.NewRow();
                dataRow["Depth"] = row.Depth;
                dataRow["Curve"] = row.Curve;
                dt.Rows.Add(dataRow);
            }
        }

        private string DatasetMarkerStyle(int markerStyle)
        {
            switch (markerStyle)
            {
                case 1:
                    return "Square";
                case 2:
                    return "Spade";
                case 3:
                    return "Triangle";
                case 4:
                    return "Ellipse";
                case 5:
                    return "Right Triangle";
                case 6:
                    return "Left Triangle";
                default:
                    return "None";
            }
        }

        private string DatasetLineGrossor(int lineGrossor)
        {
            switch (lineGrossor)
            {
                case 1:
                    return "Bold";
                case 2:
                    return "Bolder";
                case 3:
                    return "Boldest";
                default:
                    return "Normal";
            }
        }

        private string DatasetLineStyle(int lineStyle)
        {
            switch (lineStyle)
            {
                case 1:
                    return "Dashed Line";
                case 2:
                    return "Dotted Line";
                case 3:
                    return "Double Dashed Line";
                case 4:
                    return "Double Line";
                default:
                    return "Single Line";
            }
        }
        private DataTable CreateDatasetTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("CompanyName", typeof(string)));
            dt.Columns.Add(new DataColumn("DatasetName", typeof(string)));
            dt.Columns.Add(new DataColumn("WellName", typeof(string)));
            dt.Columns.Add(new DataColumn("ProjectName", typeof(string)));
            dt.Columns.Add(new DataColumn("Family", typeof(string)));
            dt.Columns.Add(new DataColumn("Units", typeof(string)));
            dt.Columns.Add(new DataColumn("MinUnitValue", typeof(string)));
            dt.Columns.Add(new DataColumn("MaxUnitValue", typeof(string)));
            dt.Columns.Add(new DataColumn("TVDMD", typeof(string)));
            dt.Columns.Add(new DataColumn("LineStyle", typeof(string)));
            dt.Columns.Add(new DataColumn("LineColor", typeof(string)));
            dt.Columns.Add(new DataColumn("Color", typeof(string)));
            dt.Columns.Add(new DataColumn("Grossor", typeof(string)));
            dt.Columns.Add(new DataColumn("MarkerStyle", typeof(string)));
            dt.Columns.Add(new DataColumn("MarkerColor", typeof(string)));
            dt.Columns.Add(new DataColumn("MarkerSize", typeof(string)));
            dt.Columns.Add(new DataColumn("BorderColor", typeof(string)));
            return dt;
        }

        private DataTable GetDataTableFromDataset(Dataset dataset)
        {
            DataTable dtDataset = CreateDatasetTable();
            AddDataRowFromDatasetToDataTable(dataset, dtDataset);
            return dtDataset;
        }

        //private DataTable GetBlankDataTableForCompany()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add(new DataColumn("CompanyName"));
        //    dt.Columns.Add(new DataColumn("DatasetName"));
        //    dt.Columns.Add(new DataColumn("WellName"));
        //    dt.Columns.Add(new DataColumn("ProjectName"));
        //    dt.Columns.Add(new DataColumn("Family"));
        //    dt.Columns.Add(new DataColumn("Units"));
        //    dt.Columns.Add(new DataColumn("MinUnitValue"));
        //    dt.Columns.Add(new DataColumn("MaxUnitValue"));
        //    dt.Columns.Add(new DataColumn("TVDMD"));
        //    dt.Columns.Add(new DataColumn("LineStyle"));
        //    dt.Columns.Add(new DataColumn("LineColor"));
        //    dt.Columns.Add(new DataColumn("Grossor"));
        //    dt.Columns.Add(new DataColumn("MarkerStyle"));
        //    dt.Columns.Add(new DataColumn("MarkerColor"));
        //    dt.Columns.Add(new DataColumn("MarkerSize"));
        //    dt.Columns.Add(new DataColumn("BorderColor"));
        //    return dt;
        //}

        private void AddDataRowFromDatasetToDataTable(Dataset dataset, DataTable dtDataset)
        {
            DataRow row = dtDataset.NewRow();
            row["CompanyName"] = GlobalData.CompanyName;
            row["DatasetName"] = dataset.Name;
            row["WellName"] = HelperMethods.Instance.GetWellByID(dataset.RefWell).Name;
            row["ProjectName"] = HelperMethods.Instance.GetProjectByID(dataset.RefProject).Name;
            row["Family"] = dataset.Family;
            row["Units"] = dataset.Units;
            row["MinUnitValue"] = dataset.MinUnitValue;
            row["MaxUnitValue"] = dataset.MaxUnitValue;
            row["TVDMD"] = dataset.IsTVD ? "TVD" : "MD";
            row["LineStyle"] = DatasetLineStyle(dataset.LineStyle);
            row["LineColor"] = "#" + dataset.LineColor.R.ToString("X2") + dataset.LineColor.G.ToString("X2") + dataset.LineColor.B.ToString("X2");
            row["Grossor"] = DatasetLineGrossor(dataset.LineGrossor);
            row["MarkerStyle"] = DatasetMarkerStyle(dataset.MarkerStyle);
            row["MarkerColor"] = "#" + dataset.MarkerColor.R.ToString("X2") + dataset.MarkerColor.G.ToString("X2") + dataset.MarkerColor.B.ToString("X2");
            row["MarkerSize"] = dataset.MarkerSize;
            row["BorderColor"] = dataset.ShouldApplyBorderColor.Value.ToString();
            dtDataset.Rows.Add(row);
        }

        public rptDatasetPrinting Report { get; set; }

        /// <summary>
        /// get GAP dataset and return ADO.Net dataset
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        public DataSet GetReportDatasetFromGAPDataset(Dataset dataset)
        {
            var dt = GetDataTableFromDataset(dataset);
            dt.TableName = "Dataset";

            DataSet ds = new DataSet();
            ds.DataSetName = "DatasetPrinting";
            ds.Tables.Add(dt);

            DataTable dtCurves = GetDepthCurvesTable();
            dtCurves.TableName = "DepthCurves";

            if (_includeSpreadsheetData) GetRows(dtCurves, dataset);
            ds.Tables.Add(dtCurves);
            ds.WriteXmlSchema("DatasetPrintingSchema.xsd");

            return ds;
        }

        public DataSet GetDatasetFromCurve(IEnumerable<Curve> curves, DataSet ds)
        {
            DataTable dtDataset = CreateDatasetTable();
            dtDataset.TableName = "Dataset";

            DataTable dtCurves = GetDepthCurvesTableExtended();
            dtCurves.TableName = "DepthCurves";
            if (curves != null)
            {
                foreach (var curve in curves)
                {
                    var dataset = HelperMethods.Instance.GetDatasetByID(curve.RefDataset);
                    GetRowsExtended(dtCurves, dataset);
                    AddDataRowFromDatasetToDataTable(dataset, dtDataset);
                }
            }
            ds.Tables.Add(dtCurves);
            //ds.WriteXmlSchema("DatasetPrintingSchema.xsd");
            ds.Tables.Add(dtDataset);
            return ds;
        }
    }//end class
}//end namespace
