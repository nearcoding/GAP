using Ninject;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Input;
using GAP.MainUI.ViewModels.Helpers;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class LithologyExportDataViewModel : BaseViewModel<BaseEntity>
    {
        public LithologyExportDataViewModel(string token)
            : base(token)
        {
            InitializeDataSources();
            //this line makes sure we list only those charts which have some tracks in it and 
            //also which has some lithologies in them

            Charts = HelperMethods.Instance.ChartsWithLithologies().ToList();

            if (Charts.Any() && IoC.Kernel.Get<IGlobalDataModel>().MainViewModel != null)
            {
                SelectedChart = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart != null ? IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject : Charts.First();
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

        private void InitializeDataSources()
        {
            DataSourceItems = new List<string> { "Excel Spreadsheet" };
            SelectedDataSource = "Excel Spreadsheet";
        }

        public List<Chart> Charts { get; set; }

        List<Track> _tracks;
        public List<Track> Tracks
        {
            get { return _tracks; }
            set
            {
                _tracks = value;
                NotifyPropertyChanged("Tracks");
            }
        }

        Chart _selectedChart;
        public Chart SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                if (_selectedChart == value) return;
                _selectedChart = value;
                if (!string.IsNullOrWhiteSpace(_selectedChart.Name))
                {
                    Tracks = HelperMethods.Instance.TracksWithLithologies().Where(v => v.RefChart == _selectedChart.ID).ToList();

                    if (Tracks.Any()) SelectedTrack = Tracks.First();
                }
                NotifyPropertyChanged("SelectedChart");
            }
        }

        Track _selectedTrack;
        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                _selectedTrack = value;
                NotifyPropertyChanged("SelectedTrack");
            }
        }

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(FileName);
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
                var dataTable = GetDataTableFromLithologyList();

                if (!CreateExcelFile.CreateExcelDocument(dataTable, FileName)) return;

                var lithologiesCount = HelperMethods.Instance.GetTrackByID(SelectedTrack.ID).Lithologies.Count;
                var messageText = string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("LithologiesExportedFromSystem"), lithologiesCount);
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.MessageAndClose, messageText);
            }
            catch (Exception ex)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, ex.Message);
            }
        }

        private DataTable GetDataTableFromLithologyList()
        {
            var lst = HelperMethods.Instance.GetTrackByID(SelectedTrack.ID).Lithologies;
            var dataTable = new DataTable();
            var colDepth = new DataColumn("Depth");
            var colLithology = new DataColumn("Lithology");
            dataTable.Columns.Add(colDepth);
            dataTable.Columns.Add(colLithology);
            foreach (var info in lst)
            {
                decimal initialDepth, finalDepth;
                if (!decimal.TryParse(info.InitialDepth.ToString(), out initialDepth))
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("InvalidDataFoundInTheSystem"));
                    return null;
                }
                if (!decimal.TryParse(info.FinalDepth.ToString(), out finalDepth))
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("InvalidDataFoundInTheSystem"));
                    return null;
                }
                var lithologyNumber = 0;
                if (!string.IsNullOrWhiteSpace(info.ImageFile) && info.ImageFile.Length > 4)
                {
                    var lithologyName = info.ImageFile.Substring(0, info.ImageFile.Length - 4);
                    if (LithologyImageFiles.Images.ContainsValue(lithologyName))
                        lithologyNumber = LithologyImageFiles.Images.FirstOrDefault(u => u.Value == lithologyName).Key;
                }

                for (decimal i = initialDepth; i <= finalDepth; i++)
                {
                    var row = dataTable.NewRow();
                    row["Depth"] = i;
                    row["Lithology"] = lithologyNumber;
                    
                    dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }

        ICommand _browseCommand;
        public ICommand BrowseCommand
        {
            get { return _browseCommand ?? (_browseCommand = new RelayCommand(Browse)); }
        }

        private void Browse()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.BrowseFiles);
        }
    }//end class
}//end namespace
