using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Ninject;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class ImportLithologyViewModel : BaseViewModel<BaseEntity>
    {
        public ImportLithologyViewModel(string token)
            : base(token)
        {
            InitializeChartsCollection();
            InitializeDataSources();
            LithologyList = new List<LithologyInfo>();
            SkipInvalidRecords = true;
            ImageDirectory = SetImageDirectory();
            AllRecordsSelected = false;
        }

        string _selectedDataSource, _fileName;
        ChartSourceForMultipleSelection _selectedChart;
        bool _skipInvalidRecords;
        List<string> _dataSourceItems;
        ExtendedBindingList<TrackSourceForMultipleSelection> _tracks;
        ICommand _browseCommand;
        ICommand _checkBoxCheckedCommand;

        public ICommand CheckBoxCheckedCommand
        {
            get { return _checkBoxCheckedCommand ?? (_checkBoxCheckedCommand = new RelayCommand(ChartSelectionChanged)); }
        }

        public bool SkipInvalidRecords
        {
            get { return _skipInvalidRecords; }
            set
            {
                _skipInvalidRecords = value;
                NotifyPropertyChanged("SkipInvalidRecords");
            }
        }
        bool? _allRecordsSelected;

        public bool? AllRecordsSelected
        {
            get { return _allRecordsSelected; }
            set
            {
                _allRecordsSelected = value;
                if (value != null)
                    UpdateCheckboxes(value.Value);
                NotifyPropertyChanged("AllRecordsSelected");
            }
        }

        //if this flag is not used then Notify Property changed goes to stackoverflow exception
        private bool _updatingTracksIsSelected;

        protected void UpdateCheckboxes(bool value)
        {
            _updatingTracksIsSelected = true;
            foreach (var track in Tracks)
                track.IsTrackSelected = value;

            _updatingTracksIsSelected = false;
        }

        private void InitializeDataSources()
        {
            DataSourceItems = new List<string> { "Excel Spreadsheet", "LAS" };
            SelectedDataSource = "Excel Spreadsheet";
        }

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

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        public List<string> DataSourceItems
        {
            get { return _dataSourceItems; }
            set
            {
                _dataSourceItems = value;
                NotifyPropertyChanged("DataSourceItems");
            }
        }

        private void InitializeChartsCollection()
        {
            //this line makes sure we list only those charts which have some tracks in it
            if (GlobalCollection.Instance.Charts == null)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("ChartListIsNull"));
                return;
            }
            var charts = HelperMethods.Instance.GetChartsWithTracks().ToList();
            Charts = new ExtendedBindingList<ChartSourceForMultipleSelection>(charts.Select(u => new ChartSourceForMultipleSelection
             {
                 ChartName = u.Name,
                 DisplayIndex = u.DisplayIndex,
                 IsChartSelected = false,
                 ID = u.ID
             }).ToList());
            Tracks = new ExtendedBindingList<TrackSourceForMultipleSelection>();
    
            Charts.ListChanged += Charts_ListChanged;
            Tracks.ListChanged += Tracks_ListChanged;
        }

        void Charts_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            ChartSelectionChanged();
        }

        void Tracks_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (_updatingTracksIsSelected)
                return;

            if (Tracks.Any(u => u.IsTrackSelected) && Tracks.Any(u => !u.IsTrackSelected))
                AllRecordsSelected = null;
            else if (Tracks.All(u => u.IsTrackSelected))
                AllRecordsSelected = true;
            else if (Tracks.All(u => !u.IsTrackSelected))
                AllRecordsSelected = false;
        }

        public ExtendedBindingList<TrackSourceForMultipleSelection> Tracks
        {
            get { return _tracks; }
            set
            {
                _tracks = value;
                NotifyPropertyChanged("Tracks");
            }
        }
        TrackSourceForMultipleSelection _selectedTrack;
        public TrackSourceForMultipleSelection SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                _selectedTrack = value;
                NotifyPropertyChanged("SelectedTrack");
            }
        }

        public ExtendedBindingList<ChartSourceForMultipleSelection> Charts { get; set; }

        public ChartSourceForMultipleSelection SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                _selectedChart = value;
                NotifyPropertyChanged("SelectedChart");
                ChartSelectionChanged();
            }
        }
        bool _isChartSelected;
        public bool IsChartSelected
        {
            get { return _isChartSelected; }
            set
            {
                _isChartSelected = value;
            }
        }

        private void ChartSelectionChanged()
        {
            Tracks.Clear();

            foreach (var chart in Charts.Where(u => u.IsChartSelected))
            {
                var tracks = HelperMethods.Instance.GetTracksByChartID(chart.ID);
                foreach (var track in tracks.OrderBy(u=>u.DisplayIndex))
                {
                    Tracks.Add(new TrackSourceForMultipleSelection
                    {
                        TrackName = track.Name,
                        RefChart = track.RefChart,
                        ID = track.ID
                    });
                }
            }
            if (Tracks.Any()) SelectedTrack = Tracks[0];
        }

        public ICommand BrowseCommand
        {
            get { return _browseCommand ?? (_browseCommand = new RelayCommand(Browse)); }
        }

        private void Browse()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.BrowseFiles);
        }

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(FileName) && Tracks.Any(u => u.IsTrackSelected);
        }

        public string ImageDirectory { get; set; }

        public override void Save()
        {
            RowNumberDepths = new StringBuilder();
            RowNumberValues = new StringBuilder();
            LithologyList.Clear();
            bool ifSavedSuccessfully = false;
            if (File.Exists(FileName))
            {
                if (string.IsNullOrWhiteSpace(ImageDirectory))
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("NoImageDirectoryFound"));
                    return;
                }
                var fileInfo = new FileInfo(FileName);
                switch (fileInfo.Extension)
                {
                    case ".xlsx":
                    case ".xls":
                        ifSavedSuccessfully = SaveExcelFile();
                        break;
                    case ".las":
                        ifSavedSuccessfully = SaveLasFile();
                        break;
                }
            }
            if (ifSavedSuccessfully)
            {
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.LithologiesImportedSuccessfully, LithologyList.Count);
                if (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.IsLithologyVisible || IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.IsFullLithology)
                {
                    IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.CalculateMinMaxVisibleRangeLimitForYAxis();
                    IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.SetHasCurvesInCaseOfLithology(true);
                }
            }
        }

        private bool SaveLasFile()
        {
            var foundAsciiData = false;
            int initialDepth = 0, finalDepth = 0, currentValue = -1;
            using (File.OpenText(FileName))
            {
                foreach (var content in File.ReadAllLines(FileName))
                {
                    if (!content.Contains("~A") && !foundAsciiData) continue;
                    if (content.Contains("~") && foundAsciiData) break;

                    if (!foundAsciiData)
                    {
                        foundAsciiData = true;
                        continue;
                    }

                    string[] values = content.Trim().Split(' ');
                    decimal decVal = 0, decDepth = 0;

                    if (!values[0].ToString().IsDecimal())
                    {
                        IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                          string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("NotAValidValueForLithologyDepth"), values[0].ToString()));
                        return false;
                    }

                    if (!values[values.Length - 1].ToString().IsDecimal())
                    {
                        IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                          string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("NotAValidValueForLithologyImage"), values[values.Length - 1].ToString()));
                        return false;
                    }

                    if (!Decimal.TryParse(values[0].ToString(), out decDepth))
                    {
                        RowNumberDepths.Append(values[0].ToString() + ", ");
                        continue;
                    }

                    if (!Decimal.TryParse(values[values.Length - 1].ToString(), out decVal))
                    {
                        RowNumberValues.Append(values[values.Length - 1].ToString() + ", ");
                        continue;
                    }

                    int presentCurrentValue = Int32.Parse(Math.Round(decVal).ToString());
                    if (initialDepth == 0) initialDepth = Int32.Parse(values[0].ToString());
                    if (currentValue == -1) currentValue = presentCurrentValue;
                    if (currentValue != presentCurrentValue)
                    {
                        if (!ValidateLithologyInformation(initialDepth, finalDepth, currentValue)) return false;
                        initialDepth = Int32.Parse(values[0].ToString());
                        finalDepth = 0;
                        currentValue = -1;
                    }
                    else
                        finalDepth = Int32.Parse(decDepth.ToString());
                }
            }
            if (foundAsciiData && currentValue == -1)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation
                    (Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("NoValidRecord"));
                return false;
            }
            else if (currentValue == -1)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation
                    (Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("NoValidAttributeFoundInLasFile"));
                return false;
            }
            return ValidateLithologyInformation(initialDepth, finalDepth, currentValue) && RaiseExceptionIfNeeded();
        }

        private bool RaiseExceptionIfNeeded()
        {
            if (!SkipInvalidRecords)
            {
                string depth = string.Empty, value = string.Empty;
                depth = RowNumberDepths.ToString();
                value = RowNumberValues.ToString();
                if (string.IsNullOrWhiteSpace(depth) && string.IsNullOrWhiteSpace(value))                
                    LithologyList.ForEach(u => LithologyManager.Instance.AddLithologyObject(u));                                    
                else
                {
                    depth = depth.Trim();
                    value = value.Trim();

                    if (depth.Length > 1) depth = depth.Substring(0, depth.Length - 1);
                    if (value.Length > 1) value = value.Substring(0, value.Length - 1);

                    if (string.IsNullOrWhiteSpace(depth) && string.IsNullOrWhiteSpace(value)) return true;
                    GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.MessageBoxWithError,
                        string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("InvalidValuesAgainstDepths"), depth.ToString(), value.ToString()));
                    return false;
                }
            }
            else
            {
                if (!LithologyList.Any())
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("NoValidRecordFoundToProcess"));
                    return false;
                }
                LithologyList.ForEach(u => LithologyManager.Instance.AddLithologyObject(u));
            }
            return true;
        }

        public StringBuilder RowNumberDepths { get; set; }
        public StringBuilder RowNumberValues { get; set; }

        private bool DataTabletHasValidColumns(DataTable dt)
        {
            if (dt.Columns.Count < 2) return false;
            string columnName;
            columnName = dt.Columns[0].ColumnName;
            if (!columnName.ToLower().Contains("depth")) return false;
            columnName = dt.Columns[1].ColumnName;
            if (!columnName.ToLower().Contains("lithology")) return false;
            return true;
        }
        List<decimal> _lstDepths = new List<decimal>();
        private bool ReadDataFromDataTable(DataTable dt)
        {
            _lstDepths.Clear();
            if (!ValidateDataTable(ref dt)) return false;

            decimal decVal = 0;
            int initialDepth = 0, finalDepth = 0, currentValue = -1, intVal = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!Decimal.TryParse(dt.Rows[i][0].ToString(), out decVal)) RowNumberDepths.Append((i + 1) + ",");
                if (!Decimal.TryParse(dt.Rows[i][1].ToString(), out decVal)) RowNumberValues.Append((i + 1) + ",");
                
                if (!ValidateDataRow(dt, i)) return false;
                
                if (initialDepth == 0) initialDepth = Convert.ToInt32(dt.Rows[i][0].ToString());
                if (currentValue == -1) currentValue = Convert.ToInt32(dt.Rows[i][1].ToString());
                _lstDepths.Add(initialDepth);
                if (Int32.TryParse(dt.Rows[i][1].ToString(), out intVal))
                {
                    if (currentValue != intVal)
                    {
                        decimal dummyDecValue = 0;
                        if (Decimal.TryParse(dt.Rows[i - 1][0].ToString(), out dummyDecValue))
                        {
                            finalDepth = Convert.ToInt32(Math.Round(dummyDecValue));
                            //save previous lithology
                            if (!ValidateLithologyInformation(initialDepth, finalDepth, currentValue)) return false;
                            initialDepth = Convert.ToInt32(dt.Rows[i][0].ToString());
                            finalDepth = 0;
                            currentValue = -1;
                        }
                    }
                    else
                    {
                        decimal dec = 0;
                        if (Decimal.TryParse(dt.Rows[i][0].ToString(), out dec))
                            finalDepth = Convert.ToInt32(Math.Round(dec));
                    }
                }
            }
            if (!ValidateLithologyInformation(initialDepth, finalDepth, currentValue)) return false;
            return RaiseExceptionIfNeeded();
        }

        private bool ValidateDataRow(DataTable dt, int i)
        {
            if (!dt.Rows[i][1].ToString().IsInteger())
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                  string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("NotAValidValueForLithologyImage"), dt.Rows[i][1].ToString()));
                return false;
            }

            if (!dt.Rows[i][0].ToString().IsDecimal())
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                      string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("NotAValidValueForLithologyDepth"), dt.Rows[i][0].ToString()));
                return false;
            }

            if(_lstDepths.Contains(decimal.Parse(dt.Rows[i][0].ToString())))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "Duplicate lithology found in data source. Unable to proceed");
                return false;
            }
            return true;
        }

        private bool ValidateDataTable(ref DataTable dt)
        {
            //must have atleast depth and value columns
            if (dt.Columns.Count < 2)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("RequiredColumnsNotFoundInExcelSheet"));
                return false;
            }
            dt = GlobalDataModel.ReplaceFirstRowWithDataColumnsInDataTable(dt);

            if (!DataTabletHasValidColumns(dt))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("DepthOrLithologyColumnIsMissingInTheExcelSheet"));
                return false;
            }
            return true;
        }

        private bool SaveExcelFile()
        {
            var ds = GlobalDataModel.GetDatasetFromExcelFile(FileName, Token);
            if (ds != null && ds.Tables.Count > 0)
                return ReadDataFromDataTable(ds.Tables[0]);

            IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                IoC.Kernel.Get<IResourceHelper>().ReadResource("NoDataReadFromExcelSheet"));
            return false;
        }

        private bool IsLithologyUnique(int initialDepth, int finalDepth)
        {
            var selectedTracks = Tracks.Where(u => u.IsTrackSelected);
            foreach (var item in selectedTracks)
            {
                var lithologies = HelperMethods.Instance.GetTrackByID(item.ID).Lithologies;

                var obj = lithologies.Where(u =>
                            (decimal.Parse(u.InitialDepth.ToString()) <= initialDepth && decimal.Parse(u.FinalDepth.ToString()) >= initialDepth) ||
                            (decimal.Parse(u.InitialDepth.ToString()) >= initialDepth && decimal.Parse(u.FinalDepth.ToString()) <= finalDepth) ||
                            (decimal.Parse(u.InitialDepth.ToString()) >= initialDepth && decimal.Parse(u.InitialDepth.ToString()) <= finalDepth) ||
                            (decimal.Parse(u.FinalDepth.ToString()) <= initialDepth && decimal.Parse(u.FinalDepth.ToString()) >= finalDepth));

                if (obj != null && obj.Any())
                {
                    var names = obj.Select(u => u.LithologyName).ToCSV();
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                        IoC.Kernel.Get<IResourceHelper>().ReadResource("OverlappingLithologiesFoundIntheSystem"));
                    return false;
                }
            }
            return true;
        }

        private bool ValidateLithologyInformation(int initialDepth, int finalDepth, int currentValue)
        {
            if (finalDepth < initialDepth)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("InitialDepthShouldbeSmallerThanFinalDepth"));
                return false;
            }

            if (!IsLithologyUnique(initialDepth, finalDepth)) return false;
            if (currentValue >= 0) SaveLithologyInformation(initialDepth, finalDepth, currentValue);
            return true;
        }

        private void SaveLithologyInformation(int initialDepth, int finalDepth, int currentValue)
        {
            foreach (var item in Tracks.Where(u => u.IsTrackSelected))
            {
                //if user provide invalid lithology image then replace it with no information image
                if (!LithologyImageFiles.Images.ContainsKey(currentValue)) currentValue = 0;
                string lithologyImage = LithologyImageFiles.Images[currentValue];

                var info = new LithologyInfo
                {
                    DisplayIndex = GetDisplayIndexForLithology(item.RefChart, item.ID),
                    FinalDepth = decimal.Parse(finalDepth.ToString()),
                    ImageFile = lithologyImage + ".bmp",
                    InitialDepth = decimal.Parse(initialDepth.ToString()),
                    IsLithologySelected = false,
                    LithologyName = lithologyImage,
                    RefChart = item.RefChart,
                    RefTrack = item.ID
                };

                LithologyList.Add(info);
            }
        }

        private List<LithologyInfo> LithologyList { get; set; }

        private string SetImageDirectory()
        {
            string imageDirectory = AppDomain.CurrentDomain.BaseDirectory;
            imageDirectory += "Lits";
            if (!Directory.Exists(imageDirectory))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithInformation(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("LithologyDirectoryNotFound"));
                return string.Empty;
            }
            return imageDirectory;
        }

        private int GetDisplayIndexForLithology(string chartID, string trackID)
        {
            int displayIndex = 0;
            var lithologyList = LithologyList.Where(u => u.RefChart == chartID && u.RefTrack == trackID);
            if (lithologyList.Any())
                displayIndex = lithologyList.Max(v => v.DisplayIndex) + 1;
            else
                displayIndex = 1;
            return displayIndex;
        }

        private static string GetNumberForLithologyName(string currentLithologyName)
        {
            int numberField = 1;
            if (!string.IsNullOrWhiteSpace(currentLithologyName) && currentLithologyName.Contains("_"))
            {
                string[] nameSplitter = currentLithologyName.Split('_');

                if (Int32.TryParse(nameSplitter[nameSplitter.Length - 1].ToString(), out numberField))
                    numberField += 1;
            }

            return PeelLithologyNameFromFullName(currentLithologyName) + "_" + numberField;
        }

        private static string PeelLithologyNameFromFullName(string lithologyName)
        {
            if (!string.IsNullOrWhiteSpace(lithologyName) && lithologyName.Contains("_"))
            {
                var lithologies = lithologyName.Split('_');
                return lithologyName.Substring(0, lithologyName.LastIndexOf('_'));
            }
            return lithologyName;
        }
    }//end class
}//end namespace
