using System;
using System.Data;
using System.Collections.Generic;
using Ninject;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using System.Globalization;
using System.Collections.ObjectModel;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class ExcelImporter
    {
        public ExcelImporter()
        {
            ListOfDatasets = new List<Dataset>();
        }
        public List<Dataset> ListOfDatasets { get; set; }
        readonly string PRIMARY_COLUMN = "Depth";

        public DataTable DataTable { get; set; }

        public decimal GetInitialDepth()
        {
            DataView dv = DataTable.DefaultView;
            dv.Sort = "depth";
            if (dv.Count > 0) return decimal.Parse(dv[0].Row.ItemArray[0].ToString());
            return 0;
        }

        public ObservableCollection<DepthCurveInfo> InsertDataInDataset(string datasetName)
        {
            var lst = new ObservableCollection<DepthCurveInfo>();
            foreach (DataRow row in DataTable.Rows)
            {
                lst.Add(new DepthCurveInfo
                    {
                        Depth = Decimal.Parse(row["Depth"].ToString()),
                        Curve = Decimal.Parse(row[datasetName].ToString(), NumberStyles.Float)
                    });
            }
            return lst;
        }

        public decimal GetFinalDepth()
        {
            DataView dv = DataTable.DefaultView;
            dv.Sort = "depth";
            if (dv.Count > 0) return decimal.Parse(dv[dv.Count - 1].Row.ItemArray[0].ToString());
            return 0;
        }

        public bool IsDepthValid { get; set; }

        public DataSet Import(string fileName, string token)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(token)) return null;

            var ds = GlobalDataModel.GetDatasetFromExcelFile(fileName, token);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable = ds.Tables[0];
                try
                {
                    DataTable.PrimaryKey = new DataColumn[1] { DataTable.Columns[PRIMARY_COLUMN] };
                }
                catch (ArgumentException)
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(token,
                        IoC.Kernel.Get<IResourceHelper>().ReadResource("DuplicateRowsFoundInExcelSheet"));
                    return null;
                }
                GenerateEmptyDatasets();
                return ds;
            }
            return null;
        }

        public void GenerateEmptyDatasets()
        {
            if (DataTable == null || DataTable.Rows.Count < 1) return;
            GlobalDataModel.ReplaceFirstRowWithDataColumnsInDataTable(DataTable);
            ListOfDatasets.Clear();
            for (int i = 1; i < DataTable.Columns.Count; i++)
            {
                ListOfDatasets.Add(new Dataset
                                    {
                                        Name = DataTable.Columns[i].ColumnName,
                                        DepthAndCurves = new ObservableCollection<DepthCurveInfo>()
                                    });
            }
        }
    }//end class
}//end namespace
