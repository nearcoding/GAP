using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Ninject;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using System.Collections.ObjectModel;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class ImportDataViewModel : BaseViewModel<BaseEntity>
    {
        string _duplicateDepth;
        bool _importAverageData, _importExactData;
        List<string> _dataSourceItems;
        string _fileName;
        ICommand _browseCommand;

        public ImportDataViewModel(string token)
            : base(token)
        {
            IsImportExactDataChecked = true;
            InitializeDataSourceItems();
            if (DataSourceItems != null && DataSourceItems.Any()) SelectedDataSource = DataSourceItems[0];
        }

        private void InitializeDataSourceItems()
        {
            DataSourceItems = new List<string> { "TXT", "Excel Spreadsheet", "WITSML", "ASCII", "LAS" };
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

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        public ICommand BrowseCommand
        {
            get { return _browseCommand ?? (_browseCommand = new RelayCommand(Browse)); }
        }

        private void Browse()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.BrowseFiles);
        }

        ICommand _importDataCommand;
        public ICommand ImportDataCommand
        {
            get { return _importDataCommand ?? (_importDataCommand = new RelayCommand(ImportData, CanSave)); }
        }

        public void ImportData()
        {
            using (new WaitCursor())
            {
                var result = false;
                ListOfDatasets = new List<Dataset>();

                string fullPath = Path.GetFullPath(FileName);
                if (!File.Exists(fullPath))
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                        IoC.Kernel.Get<IResourceHelper>().ReadResource("FileNotFoundInSystem"));
                    return;
                }
                if (FileName.ToLower().EndsWith("txt"))
                    result = SaveTextFile();
                else if (FileName.ToLower().EndsWith("xls") || FileName.ToLower().EndsWith("xlsx"))
                    result = SaveExcelFile();
                else if (FileName.ToLower().EndsWith("las"))
                    result = SaveLASFile();
                else if (FileName.ToLower().EndsWith("xml"))
                    result = SaveWITSMLFile();
                if (!result) return;

                if (!ValidateDatasets()) return;
            }
            //if average data option is selected on the screen
            if (IsImportAverageDataChecked)
            {
                OpenDepthImportView();
                if (IsDepthImportViewSaved)
                {
                    ImportAverageData();
                    if (IsImportAverageDataChecked) GlobalDataModel.Instance.ImportDataType = ImportDataType.AverageChecked;
                    if (IsExactOrCloserValueChecked) GlobalDataModel.Instance.ImportDataType = ImportDataType.ExactOrCloserValue;
                }
                else
                    return;
            }
            else
            {
                GlobalDataModel.Instance.ImportDataType = ImportDataType.ExactValue;
            }
            SavedDatasets = null;
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.SaveDatasetImport, ListOfDatasets);
        }

        private bool ValidateDatasets()
        {
            GetInitialAndFinalDepth();

            if (InitialDepth != 0 || FinalDepth != 0) return true;

            IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                IoC.Kernel.Get<IResourceHelper>().ReadResource("NoValidRecordFoundToProcess"));
            return false;
        }

        private void GetInitialAndFinalDepth()
        {
            foreach (var dataset in ListOfDatasets.Where(dataset => dataset.DepthAndCurves.Any()))
            {
                dataset.InitialDepth = dataset.DepthAndCurves.Min(u => u.Depth);
                dataset.FinalDepth = dataset.DepthAndCurves.Max(u => u.Depth);
            }
            if (!ListOfDatasets.Any()) return;

            InitialDepth = ListOfDatasets[0].InitialDepth;
            FinalDepth = ListOfDatasets[0].FinalDepth;
        }

        private bool ImportAverageListOrExactOrCloserValueList()
        {
            foreach (var dataSet in ListOfDatasets)
            {
                dataSet.DepthAndCurves = IsArithmeticValueChecked ?
                  GetAverageList(dataSet.DepthAndCurves) : GetExactOrCloserValueList(dataSet.DepthAndCurves);
                if (dataSet.DepthAndCurves == null) return false;
                if (!DuplicateDepthValidation(dataSet.DepthAndCurves)) return false;
            }
            return true;
        }

        private bool SaveWITSMLFile()
        {
            ListOfDatasets = WITSMLImporter.Import(_fileName, Token);
            return ListOfDatasets != null;
        }
        private bool SaveLASFile()
        {
            if (File.ReadAllText(FileName).IndexOf("~A") < 0)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("NoValidAttributeFoundInLasFile"));
                return false;
            }
            var foundAsciiData = false;
            using (TextReader reader = File.OpenText(FileName))
            {
                foreach (var content in File.ReadAllLines(FileName))
                {
                    if (!content.Contains("~A") && !foundAsciiData) continue;
                    if (content.Contains("~") && foundAsciiData) break;
                    foundAsciiData = true;
                    var values = content.Trim().Split('\t');
                    //this should raise the same issue as text file about duplcate datasets name
                    for (var i = 1; i < values.Length; i++)
                    {
                        ListOfDatasets.Add(new Dataset
                        {
                            Name = values[i]
                        });
                    }
                    FillDatasetsFromLASFile();
                    return true;
                }
            }
            return false;
        }

        private void FillDatasetsFromLASFile()
        {
            var foundAsciiData = false;

            using (TextReader reader = File.OpenText(FileName))
            {
                foreach (var content in File.ReadAllLines(FileName))
                {
                    //this is initial stages of file processsing
                    if (!content.Contains("~A") && !foundAsciiData) continue;
                    //if once we get the attribute A to process then upon encountering any other attribute, just skip the processing
                    if (content.Contains("~") && foundAsciiData) break;
                    //if we get the required attribute then process from the next line onwards
                    if (content.Contains("~A") && !foundAsciiData)
                    {
                        foundAsciiData = true;
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(content.Trim())) continue;
                    var values = content.Trim().Split('\t');

                    values = values.Except(values.Where(u => string.IsNullOrWhiteSpace(u))).ToArray();

                    for (var i = 1; i < values.Length; i++)
                    {
                        //there are two points in the value change it to our desired format

                        values[0] = GetSingleDecimalValues(values[0]);
                        values[i] = GetSingleDecimalValues(values[i]);

                        decimal depthValue = 0, curveValue = 0;
                        if (!ToDecimal(values[0], out depthValue))
                        {
                            if (SkipInvalidRecords) continue;
                            return;
                        }
                        if (!ToDecimal(values[1], out curveValue))
                        {
                            if (SkipInvalidRecords) continue;
                            return;
                        }
                        if (ListOfDatasets.Count <= i)
                        {
                            //there is no dataset at this position
                            continue; ;
                        }
                        ListOfDatasets[i - 1].DepthAndCurves.Add(new DepthCurveInfo
                        {
                            Depth = depthValue,
                            Curve = curveValue,
                            DisplayIndex = ListOfDatasets[0].DepthAndCurves.Count
                        });
                    }
                }
            }
        }

        private string GetSingleDecimalValues(string value)
        {
            while (value.ToCharArray().Count(u => u == '.') > 1)
            {
                value = value.Substring(0, value.LastIndexOf('.'));
            }
            return value;
        }

        public bool ToDecimal(string str, out decimal decVar)
        {
            if (!decimal.TryParse(str, out decVar))
            {
                if (!SkipInvalidRecords)
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                        IoC.Kernel.Get<IResourceHelper>().ReadResource("InvalidDataFoundInTheSystem"));

                return false;
            }
            return true;
        }
        //these are the datasets that are approved by mapping screen
        public List<Dataset> SavedDatasets { get; set; }

        private bool SaveExcelFile()
        {
            var importer = new ExcelImporter();
            var dataset = importer.Import(FileName, Token);
            if (dataset == null || dataset.Tables.Count <= 0 || dataset.Tables[0].Rows.Count <= 0) return false;
            ListOfDatasets = importer.ListOfDatasets;

            foreach (var ds in ListOfDatasets)
                ds.DepthAndCurves = importer.InsertDataInDataset(ds.Name);
            return true;
        }

        /// <summary>
        /// this method returns nearest decimal of mentioned decimal values from the list of decimals 
        /// </summary>
        /// <param name="lstAllTVDs"></param>
        /// <param name="averageTVD"></param>
        /// <returns></returns>
        private decimal GetNearestDecimal(List<decimal> lstAllTVDs, decimal averageTVD)
        {
            if (lstAllTVDs == null) return 0;
            if (lstAllTVDs.Count < 1) return 0;
            if (lstAllTVDs.Count == 1) return lstAllTVDs.First();
            var positiveList = new Dictionary<decimal, decimal>();
            var negativeList = new Dictionary<decimal, decimal>();
            for (int i = 0; i < lstAllTVDs.Count; i++)
            {
                //need to figure out if else short circuiting
                decimal val = lstAllTVDs[i] - averageTVD;
                if (val < 0)
                    if (!negativeList.ContainsKey(lstAllTVDs[i])) negativeList.Add(lstAllTVDs[i], val * -1);  //multiple by -1 to make it positive which later help us determine  if +ve is near or -ve
                    else
                        if (!positiveList.ContainsKey(lstAllTVDs[i])) positiveList.Add(lstAllTVDs[i], val);
            }

            decimal minNegativeValue = 0, minPositiveValue = 0;
            if (negativeList.Any()) minNegativeValue = negativeList.Min(v => v.Value);
            if (positiveList.Any()) minPositiveValue = positiveList.Min(v => v.Value);
            if (negativeList.Any() && positiveList.Any())
            {
                if (minNegativeValue < minPositiveValue)
                    return negativeList.FirstOrDefault(u => u.Value == minNegativeValue).Key;
                else
                    return positiveList.FirstOrDefault(u => u.Value == minPositiveValue).Key;
            }
            else if (negativeList.Any())
            {
                return negativeList.FirstOrDefault(u => u.Value == minNegativeValue).Key;
            }
            else if (positiveList.Any())
            {
                return positiveList.FirstOrDefault(u => u.Value == minPositiveValue).Key;
            }
            return 0;
        }

        private ObservableCollection<DepthCurveInfo> GetExactOrCloserValueListAgainstCustomStep(ObservableCollection<DepthCurveInfo> lst)
        {
            var newList = new ObservableCollection<DepthCurveInfo>();
            for (var i = 0; i < lst.Count; i += (int)Step)
            {
                decimal averageCurve = 0;
                decimal currentDepth = 0;
                var lstDepthCurve = new List<DepthCurveInfo>();
                for (var j = i; j < (int)Step + i; j++) //Step + i because to keep the inner loop moving 
                {
                    if (j < lst.Count)
                    {
                        currentDepth = Math.Round(lst[j].Depth);
                        lstDepthCurve.Add(lst[j]);
                        averageCurve += lst[j].Curve;
                    }
                    else
                    {
                        averageCurve += lst[lst.Count - 1].Curve;
                        currentDepth = Math.Round(lst[lst.Count - 1].Depth);
                        lstDepthCurve.Add(lst[lst.Count - 1]);
                    }
                }

                var sameValue = lstDepthCurve.FirstOrDefault(u => u.Depth == currentDepth);
                if (sameValue != null)
                {
                    averageCurve = sameValue.Curve;
                }
                else
                {
                    List<Decimal> lstTVD = lstDepthCurve.Select(u => u.Depth).ToList();
                    decimal whichTVDToChoose = GetNearestDecimal(lstTVD, currentDepth);
                    averageCurve = lstDepthCurve.FirstOrDefault(u => u.Depth == whichTVDToChoose).Curve;
                }
                newList.Add(new DepthCurveInfo
                {
                    Curve = averageCurve,
                    Depth = currentDepth,
                    DisplayIndex = newList.Count - 1
                });
            }
            return newList;
        }

        private ObservableCollection<DepthCurveInfo> GetExactOrCloserValueListAgainstSingleStep(ObservableCollection<DepthCurveInfo> lst)
        {
            decimal currentDepth = 0, averageValue = 0;
            var newList = new ObservableCollection<DepthCurveInfo>();
            foreach (var t in lst)
            {
                currentDepth = t.Depth;
                averageValue = t.Curve;
                newList.Add(new DepthCurveInfo
                {
                    Depth = Math.Round(currentDepth),
                    Curve = Math.Round(averageValue),
                    DisplayIndex = lst.Count - 1
                });
            }
            return newList;
        }

        private ObservableCollection<DepthCurveInfo> GetExactOrCloserValueList(ObservableCollection<DepthCurveInfo> lst)
        {
            return Step > 1 ? GetExactOrCloserValueListAgainstCustomStep(lst) : GetExactOrCloserValueListAgainstSingleStep(lst);
        }

        private ObservableCollection<DepthCurveInfo> GetAverageList(ObservableCollection<DepthCurveInfo> lst)
        {
            if (Step < 1)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("ValueOfStepMustBeGreaterThanOne"));
                return null;
            }
            return Step > 1 ? GetAverageListAgainstCustomStep(lst) : GetAverageListAgainstSingleStep(lst);
        }

        private ObservableCollection<DepthCurveInfo> GetAverageListAgainstSingleStep(ObservableCollection<DepthCurveInfo> lst)
        {
            decimal currentDepth = 0, averageValue = 0;
            var newList = new ObservableCollection<DepthCurveInfo>();
            foreach (var obj in lst)
            {
                currentDepth = obj.Depth;
                averageValue = obj.Curve;
                newList.Add(new DepthCurveInfo
                {
                    Depth = Math.Round(currentDepth),
                    Curve = (decimal)averageValue,
                    DisplayIndex = lst.Count - 1
                });
            }
            return newList;
        }

        private ObservableCollection<DepthCurveInfo> GetAverageListAgainstCustomStep(ObservableCollection<DepthCurveInfo> lst)
        {
            decimal currentDepth = 0, averageValue = 0;
            var newList = new ObservableCollection<DepthCurveInfo>();
            for (var i = 0; i < lst.Count; i += (int)Step)
            {
                currentDepth = 0;
                averageValue = 0;
                for (var j = i; j < i + Step; j++) //i+Step so we go ahead of the last processed depth
                {
                    if (j < lst.Count)
                    {
                        currentDepth = lst[j].Depth;
                        averageValue += lst[j].Curve;
                    }
                    else
                    {
                        currentDepth = lst[lst.Count - 1].Depth;
                        averageValue = lst[lst.Count - 1].Curve;
                    }
                }
                newList.Add(new DepthCurveInfo
                {
                    Depth = Math.Round(currentDepth),
                    Curve = (decimal)averageValue / Step,
                    DisplayIndex = lst.Count - 1
                });
            }
            return newList;
        }

        public bool SkipInvalidRecords { get; set; }

        private bool SaveTextFile()
        {
            using (TextReader tr = File.OpenText(FileName))
                GetListOfDatasetsFromTextFile(tr.ReadLine());

            if (!ListOfDatasets.Any())
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("NoValidDatasetFoundToImport"));
                return false;
            }
            var groupByDatasetName = ListOfDatasets.GroupBy(u => u.Name);
            if (groupByDatasetName.Any(v => v.Count() > 1))
            {
                var lstOfDatasetsName = groupByDatasetName.Where(u => u.Count() > 1).Select(v => v.Key).ToList().ToCSV();
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("DuplicateDatasetFoundInDataSource"), lstOfDatasetsName));
                return false;
            }

            foreach (var dataset in ListOfDatasets)
            {
                if (!FillDatasetFromTextFile(dataset)) return false;
            }
            return true;
        }

        /// <summary>
        /// this method opens the Open Depth Import view screen
        /// </summary>
        private void OpenDepthImportView()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.OpenDepthImportView);
        }

        /// <summary>
        /// this message loops through all the  datasets and appply average data and closer or exact value
        /// </summary>
        /// <returns></returns>
        private bool ImportAverageData()
        {
            //looping through all the datasets to fill them
            foreach (var dataSet in ListOfDatasets)
            {
                dataSet.DepthAndCurves = IsArithmeticValueChecked ?
                    GetAverageList(dataSet.DepthAndCurves) : GetExactOrCloserValueList(dataSet.DepthAndCurves);
                if (dataSet.DepthAndCurves == null) return false;
            }
            return true;
        }

        public List<Dataset> ListOfDatasets { get; set; }

        private List<string> ValidDepthColumns { get; set; }

        private bool TextFileHaveDepthColumn(string line)
        {
            ValidDepthColumns = new List<string>
            {
                "depth","tvd","md"
            };

            var headers = line.Trim().Split('\t');
            if (headers.Count() > 0 && ValidDepthColumns.Contains(headers[0].ToLower()))// (headers[0].ToLower() == "depth" || headers[0].ToLower() == "tvd"))
            {
                return true;
            }
            else
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("DepthColumnNotFoundInDataSource"));
                return false;
            }
        }

        private void GetListOfDatasetsFromTextFile(string line)
        {
            ListOfDatasets = new List<Dataset>();
            if (string.IsNullOrWhiteSpace(line)) return;
            if (!TextFileHaveDepthColumn(line)) return;
            var headers = line.Trim().Split('\t');

            for (var i = 1; i < headers.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(headers[i]))
                {
                    ListOfDatasets.Add(new Dataset
                    {
                        Name = headers[i]
                    });
                }
            }
        }

        private bool DuplicateDepthValidation(ObservableCollection<DepthCurveInfo> lstItems)
        {
            var obj = lstItems.GroupBy(u => u.Depth).Where(v => v.Count() > 1).Select(w => w.Key).ToList();
            if (!obj.Any()) return true;
            _duplicateDepth = obj.ToCSV();
            IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("ListOfDuplicateDepths"), _duplicateDepth));
            return false;
        }

        private bool FillDatasetFromTextFile(Dataset dataset)
        {
            var indexOfDataset = ListOfDatasets.FindIndex(u => u.Name == dataset.Name);
            if (indexOfDataset < 0)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("DatasetNotFoundInImportedList"), dataset.Name));
                return false;
            }
            indexOfDataset = indexOfDataset + 1;
            var lstItems = new ObservableCollection<DepthCurveInfo>();
            //read whole file except the header line using Skip(1)
            foreach (var item in File.ReadAllLines(FileName).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                var stringArray = item.Split('\t');
                var lst = stringArray.ToList();
                lst.RemoveAll(u => string.IsNullOrWhiteSpace(u));
                stringArray = lst.ToArray();
                decimal depth, curve;
                if (!decimal.TryParse(stringArray[0].ToString(), out depth))
                {
                    if (!SkipInvalidRecords)
                    {
                        IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("InvalidRecordFoundInTheDataSource"));
                        return false;
                    }
                    continue;
                }
                if (!decimal.TryParse(stringArray[indexOfDataset].ToString(), out curve))
                {
                    if (!SkipInvalidRecords)
                    {
                        IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("InvalidDataFoundInTheSystem"));
                        return false;
                    }
                    continue;
                }
                lstItems.Add(new DepthCurveInfo
                {
                    Depth = depth,
                    Curve = curve //i represents the dataset
                });
            }

            if (!lstItems.Any())
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("NoValidDepthAndCurveFoundInSystem"));
                return false;
            }

            dataset.DepthAndCurves = lstItems;
            if (indexOfDataset == 1)
                return DuplicateDepthValidation(dataset.DepthAndCurves);
            return true;
        }

        public bool IsDepthImportViewSaved { get; set; }

        public decimal InitialDepth { get; set; }

        public decimal FinalDepth { get; set; }

        public decimal Step { get; set; }

        public bool IsArithmeticValueChecked { get; set; }

        public bool IsExactOrCloserValueChecked { get; set; }

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

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(FileName);
        }

        public bool IsImportAverageDataChecked
        {
            get { return _importAverageData; }
            set
            {
                _importAverageData = value;
                NotifyPropertyChanged("IsImportAverageDataChecked");
            }
        }

        public bool IsImportExactDataChecked
        {
            get { return _importExactData; }
            set
            {
                _importExactData = value;
                NotifyPropertyChanged("IsImportExactDataChecked");
            }
        }
    }//end class
}//end namespace
