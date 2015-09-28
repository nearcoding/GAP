using Ninject;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AutoMapper;
using System.Drawing;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class FormationTopImportViewModel : BaseViewModel<FormationInfo>
    {
        public FormationTopImportViewModel(string token)
            : base(token)
        {
            InitializeDataSources();
            Mapper.CreateMap<FormationInfo, FormationInfo>();
            Charts = GlobalCollection.Instance.Charts.Select(u =>
                new ChartSourceForMultipleSelection
                {
                    ChartName = u.Name,
                    ID = u.ID
                }).ToList();
        }

        List<string> _dataSourceItems;
        public List<string> DataSourceItems
        {
            get { return _dataSourceItems; }
            set
            {
                _dataSourceItems = value;
                NotifyPropertyChanged("DataSourceItems");
            }
        }
        private void InitializeDataSources()
        {
            DataSourceItems = new List<string> { "Excel Spreadsheet" };
            SelectedDataSource = DataSourceItems.First();
        }

        string _selectedDataSource;
        public string SelectedDataSource
        {
            get { return _selectedDataSource; }
            set
            {
                _selectedDataSource = value;
                FileName = string.Empty;
                NotifyPropertyChanged("SelectedDataSource");
            }
        }

        string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }
        public List<ChartSourceForMultipleSelection> Charts { get; set; }

        ICommand _browseCommand;
        public ICommand BrowseCommand
        {
            get { return _browseCommand ?? (_browseCommand = new RelayCommand(Browse)); }
        }

        private void Browse()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.BrowseFiles);
        }

        public StringBuilder RowNumberDepths { get; set; }
        public StringBuilder RowNumberValues { get; set; }

        public override void Save()
        {
            if (!Charts.Any(u => u.IsChartSelected))
            {
                //no chart is selected
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError
                    (Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("AtleastOneChartMustBeSelectedToImportData"));
                return;
            }

            RowNumberDepths = new StringBuilder();
            RowNumberValues = new StringBuilder();
            _formationCount = 0;
            var ifSavedSuccessfully = false;

            if (File.Exists(FileName))
            {
                var fileInfo = new FileInfo(FileName);
                if (fileInfo.Extension == ".xls" || fileInfo.Extension == ".xlsx")
                    ifSavedSuccessfully = SaveExcelFile();
            }
            else
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "File not found");
                return;
            }
            if (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.IsFormationVisible)
            {
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.CalculateMinMaxVisibleRangeLimitForYAxis();
            }
            if (ifSavedSuccessfully)
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.FormationsImportedSuccesfully, _formationCount);
        }

        private bool SaveExcelFile()
        {
            var ds = GlobalDataModel.GetDatasetFromExcelFile(FileName, Token);
            if (ds != null && ds.Tables.Count > 0) return ReadDataFromDataTable(ds.Tables[0]);
            return false;
        }

        private bool IfExcelSheetHasValidColumns(DataTable dataTable)
        {
            var firstRow = dataTable.Rows[0];
            var mandatoryColumns = new List<string> { "depth", "formation name", "color", "style", "grossor" };
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (mandatoryColumns.Contains(firstRow[i].ToString().ToLower()))
                    mandatoryColumns.Remove(firstRow[i].ToString().ToLower());
                else
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("InvalidColumnsFoundInExcelSheet"));
                    return false;
                }
            }

            return true;
        }

        private bool ReadDataFromDataTable(DataTable dt)
        {
            //must have atleast depth and value columns
            if (dt.Columns.Count < 5)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("RequiredColumnsNotFoundInExcelSheet"));
                return false;
            }

            if (!IfExcelSheetHasValidColumns(dt)) return false;
            GlobalDataModel.ReplaceFirstRowWithDataColumnsInDataTable(dt);
            foreach (DataRow row in dt.Rows)
            {
                decimal decVal = 0;
                Decimal.TryParse(row["depth"].ToString(), out decVal);

                SaveFormationInformation(Int32.Parse(Math.Round(decVal).ToString()),
                    row["color"].ToString(),
                    row["style"].ToString(),
                    row["grossor"].ToString(),
                    row["formation name"].ToString());
            }

            return true;
        }

        private void SaveFormationInformation(int depth, string colorName, string style, string grossor, string formationName)
        {
            var col = ColorTranslator.FromHtml(colorName);

            var info = new FormationInfo
            {
                Depth = depth,
                IsFormationSelected = false,
                FormationName = formationName,
                FormationColor = new Colour
                {
                    A = col.A,
                    R = col.R,
                    G = col.G,
                    B = col.B
                },

                LineStyle = GetFormationLineIndexFromString(style),
                LineGrossor = GetGrossorIndexFromString(grossor)
            };
            ImportFormationInSelectedCharts(info);
        }

        int _formationCount = 0;

        private void ImportFormationInSelectedCharts(FormationInfo info)
        {
            foreach (var chart in Charts.Where(u => u.IsChartSelected))
            {
                var chartObject = HelperMethods.Instance.GetChartByID(chart.ID);
                if (chartObject.Formations.Any(u => u.Depth == info.Depth)) continue;
                _formationCount += 1;
                //create new formation info object, so it could be assigned to different chart each time
                var newFormationInfo = (FormationInfo)HelperMethods.GetNewObject<FormationInfo>(info);
                newFormationInfo.RefChart = chart.ID;
                FormationManager.Instance.AddFormationObject(newFormationInfo);                
            }
        }

        private static int GetGrossorIndexFromString(string grossor)
        {
            switch (grossor)
            {
                case "Thick":
                    return 1;
                case "Thicker":
                    return 2;
                case "Thickest":
                    return 3;
                default:
                    return 0;
            }
        }

        private static int GetFormationLineIndexFromString(string style)
        {
            switch (style)
            {
                case "Dashed":
                    return 1;
                case "Dotted":
                    return 2;
                default:
                    return 0;
            }
        }

    }//end class
}//end namespace
