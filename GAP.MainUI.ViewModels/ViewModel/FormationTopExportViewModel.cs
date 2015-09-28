using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Input;
using Ninject;
using GAP.MainUI.ViewModels.Helpers;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class FormationTopExportViewModel : BaseViewModel<FormationInfo>
    {
        public FormationTopExportViewModel(string token)
            : base(token)
        {
            InitializeDataSources();
            var dummyCharts = HelperMethods.Instance.GetChartsWithFormations();

            Charts = dummyCharts.Distinct().ToList();

            if (Charts.Any() && IoC.Kernel.Get<IGlobalDataModel>().MainViewModel != null)
            {
                var mainChart = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart;
                if (mainChart != null && HelperMethods.Instance.AnyFormationExistsUnderThisChart(mainChart.ChartObject.ID))
                    SelectedChart = Charts.First(u => u.ID == IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject.ID);
                else
                {
                    //loop through all the charts until a chart with formation is found
                    foreach (var chart in Charts)
                    {
                        if (HelperMethods.Instance.AnyFormationExistsUnderThisChart(chart.ID))
                        {
                            SelectedChart = chart;
                            return;
                        }
                    }
                }
            }
            //if (SelectedChart==null)
        }

        Chart _selectedChart;
        public Chart SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                _selectedChart = value;
                NotifyPropertyChanged("SelectedChart");
            }
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

        public List<Chart> Charts { get; set; }


        ICommand _browseCommand;
        public ICommand BrowseCommand
        {
            get { return _browseCommand ?? (_browseCommand = new RelayCommand(Browse)); }
        }

        private void Browse()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.BrowseFiles);
        }

        public override void Save()
        {
            try
            {
                if (!FileName.ToLower().EndsWith("xlsx"))
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                        IoC.Kernel.Get<IResourceHelper>().ReadResource("FileTypeMustBeXLSX"));
                    return;
                }
                var dataTable = GetDataTableFromFormationList();
                if (!CreateExcelFile.CreateExcelDocument(dataTable, FileName)) return;
                var messageText = string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("FormationsExportedFromSystem"), dataTable.Rows.Count);
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.MessageAndClose, messageText);
            }
            catch (Exception ex)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, ex.Message);
            }
        }

        private DataTable GetDataTableFromFormationList()
        {
            if (SelectedChart == null) return null;
            var lst = HelperMethods.Instance.GetChartByID(SelectedChart.ID).Formations;
            var dataTable = CreateDataTableForFormationExport();
            foreach (var info in lst)
            {
                var row = dataTable.NewRow();
                row["Depth"] = info.Depth;
                row["Formation Name"] = info.FormationName;
                row["Color"] = "#" + info.FormationColor.R.ToString("X2") + info.FormationColor.G.ToString("X2") + info.FormationColor.B.ToString("X2");
                row["Style"] = GetFormationLineStyleFromIndex(info.LineStyle);
                row["Grossor"] = GetFormationLineGrossorFromIndex(info.LineGrossor);
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

        private static DataTable CreateDataTableForFormationExport()
        {
            var dataTable = new DataTable();
            var colDepth = new DataColumn("Depth");
            var colFormation = new DataColumn("Formation Name");
            var colColor = new DataColumn("Color");
            var colStyle = new DataColumn("Style");
            var colGrossor = new DataColumn("Grossor");
            dataTable.Columns.Add(colDepth);
            dataTable.Columns.Add(colFormation);
            dataTable.Columns.Add(colColor);
            dataTable.Columns.Add(colStyle);
            dataTable.Columns.Add(colGrossor);

            return dataTable;
        }

        private string GetFormationLineGrossorFromIndex(int lineGrossor)
        {
            switch (lineGrossor)
            {
                case 1:
                    return "Thick";
                case 2:
                    return "Thicker";
                case 3:
                    return "Thickest";
                default:
                    return "Thin";
            }
        }

        private string GetFormationLineStyleFromIndex(int lineStyle)
        {
            switch (lineStyle)
            {
                case 1:
                    return "Dashed";
                case 2:
                    return "Dotted";
                default:
                    return "Line";
            }
        }

    }//end class
}//end namespace

