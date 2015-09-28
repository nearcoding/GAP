using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Ninject;
using System.Collections.ObjectModel;
using System.Windows.Media;
using GAP.MainUI.ViewModels.Helpers;
using System.Threading.Tasks;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class ShalePointFilterViewModel : BaseViewModel<BaseEntity>
    {
        SortedDictionary<decimal, decimal> _tempSource = new SortedDictionary<decimal, decimal>();
        SortedDictionary<decimal, decimal> _tempDestination = new SortedDictionary<decimal, decimal>();
        SortedDictionary<decimal, decimal> _dicSource = new SortedDictionary<decimal, decimal>();
        SortedDictionary<decimal, decimal> _dicDestination = new SortedDictionary<decimal, decimal>();
        SortedDictionary<decimal, decimal> _tempLine = new SortedDictionary<decimal, decimal>();
        SortedDictionary<decimal, decimal> _dicLine = new SortedDictionary<decimal, decimal>();
        public ShalePointFilterViewModel(string token)
            : base(token)
        {
            Refresh();
        }

        public void Refresh()
        {
            VShaleUnitValue = 0;
            SelectedDestinationDataset = null;
            SelectedSourceDataset = null;
            CurrentObject = new Dataset();
            SHPTFromLine = true;
            IsGreaterThan = true;
            NotifyPropertyChanged("CurrentObject");
        }

        bool _isGreaterThan;

        public bool IsGreaterThan
        {
            get { return _isGreaterThan; }
            set
            {
                _isGreaterThan = value;
                NotifyPropertyChanged("IsGreaterThan");
            }
        }

        string _sourceDatasetText;
        public string SourceDatasetText
        {
            get { return _sourceDatasetText; }
            set
            {
                _sourceDatasetText = value;
                NotifyPropertyChanged("SourceDatasetText");
            }
        }

        bool _shptFromLine;
        public bool SHPTFromLine
        {
            get { return _shptFromLine; }
            set
            {
                _shptFromLine = value;
                if (value)
                {
                    SourceDatasetText = "Select the SHTP Line";
                    var datasets = HelperMethods.Instance.GetDatasetsByCurves().Where(u => u.Family == "Gamma Ray");
                    datasets = datasets.Where(u => u.SubDatasets.Where(v => v.IsNCT == true).Count() == 1);
                    SourceDatasets = new ObservableCollection<Dataset>(datasets);
                }
                else
                {
                    SourceDatasetText = "Select VShale Dataset";
                    SourceDatasets = new ObservableCollection<Dataset>(HelperMethods.Instance.GetAllDatasets().ToList().Where(u => u.Family == "VShale").ToList());
                }

                if (SourceDatasets.Any()) SelectedSourceDataset = SourceDatasets.First();
                NotifyPropertyChanged("SourceDatasets");
                NotifyPropertyChanged("SHPTFromLine");
            }
        }

        Dataset _selectedSourceDataset;
        public Dataset SelectedSourceDataset
        {
            get { return _selectedSourceDataset; }
            set
            {
                _selectedSourceDataset = value;
                SetDestinationDatasets();
                NotifyPropertyChanged("SelectedSourceDataset");
            }
        }

        ICommand _datasetPropertiesCommand;
        public ICommand DatasetPropertiesCommand
        {
            get { return _datasetPropertiesCommand ?? (_datasetPropertiesCommand = new RelayCommand(DatasetProperties)); }
        }

        private void DatasetProperties()
        {
            Dataset dataset = new Dataset
            {
                Family = "VShale",
                Units = "%",
                RefProject = SelectedSourceDataset.RefProject,
                RefWell = SelectedSourceDataset.RefWell
            };
            IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.DatasetPropertiesScreen, dataset);
        }

        public new Dataset CurrentObject { get; set; }

        Dataset _selectedDestinationDataset;
        public Dataset SelectedDestinationDataset
        {
            get { return _selectedDestinationDataset; }
            set
            {
                _selectedDestinationDataset = value;
                if (value == null) return;
                ValidateStep();
                if (CurrentObject != null)
                    CurrentObject.Name = string.Format("SHPT_{0}", SelectedDestinationDataset.Name);
                NotifyPropertyChanged("SelectedDestinationDataset");
            }
        }

        private void ValidateStep()
        {
            ListItems.Clear();
            if (SelectedSourceDataset.Step != SelectedDestinationDataset.Step)
            {
                ListItems.Add("Steps are not same in both datasets");
            }
        }

        private void SetDestinationDatasets()
        {
            if (SelectedSourceDataset == null)
            {
                DestinationDatasets = null;
                return;
            }
            IEnumerable<Dataset> destinationDatasets;
            if (SHPTFromLine)
                destinationDatasets = HelperMethods.Instance.GetAllDatasets().Where(u => u.Family != "VShale");
            else
                destinationDatasets = HelperMethods.Instance.GetAllDatasets().Where(u => u.Family != "Gamma Ray");

            DestinationDatasets = new ObservableCollection<Dataset>(destinationDatasets.Where(u => u.ID != SelectedSourceDataset.ID).ToList());
            if (DestinationDatasets.Any())
            {
                SelectedDestinationDataset = DestinationDatasets.First();

            }
            NotifyPropertyChanged("DestinationDatasets");
        }

        ObservableCollection<string> _errors;
        public ObservableCollection<string> ListItems
        {
            get
            {
                if (_errors == null) _errors = new ObservableCollection<string>();
                return _errors;
            }
            set
            {
                _errors = value;
                NotifyPropertyChanged("ListItems");
            }
        }
        public ObservableCollection<Dataset> DestinationDatasets
        {
            get { return _destinationDatasets; }
            set
            {
                _destinationDatasets = value;
                NotifyPropertyChanged("DestinationDatasets");
            }
        }

        ObservableCollection<Dataset> _sourceDatasets, _destinationDatasets;
        public ObservableCollection<Dataset> SourceDatasets
        {
            get { return _sourceDatasets; }
            set
            {
                _sourceDatasets = value;
                NotifyPropertyChanged("SourceDatasets");
            }
        }

        public override void Save()
        {
            if (SelectedDestinationDataset.Step != SelectedSourceDataset.Step)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "Steps are different in selected datasets");
                return;
            }
            var commonDepths = GetCommonDepths();
            if (!commonDepths.Any())
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "No common depths found in the system");
                return;
            }
            CurrentObject.DepthAndCurves = new ObservableCollection<DepthCurveInfo>();

            CalculateDepthAndCurves(commonDepths);


            if (!CurrentObject.DepthAndCurves.Any())
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    "There are common depths in the selected datasets but no record satisfying your selection of Greater/Less than");
                return;
            }

            CurrentObject.RefProject = SelectedDestinationDataset.RefProject;
            CurrentObject.RefWell = SelectedDestinationDataset.RefWell;
            CurrentObject.Family = SelectedDestinationDataset.Family;
            CurrentObject.Units = SelectedDestinationDataset.Units;
            var parentWell = HelperMethods.Instance.GetWellByID(CurrentObject.RefWell);
            if (!parentWell.Datasets.Any(u => u.Name == CurrentObject.Name))
            {
                parentWell.Datasets.Add(CurrentObject);
                Cancel();
            }
            else
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "Another dataset with this name already exists");
        }

        decimal _vShaleUnitValue;
        public decimal VShaleUnitValue
        {
            get { return _vShaleUnitValue; }
            set
            {
                _vShaleUnitValue = value;
                NotifyPropertyChanged("VShaleUnitValue");
            }
        }

        private void FillSourceDestinationDictionaries(IEnumerable<decimal> commonDepths)
        {
            foreach (var element in commonDepths)
            {
                decimal obj = _tempSource[element];
                _dicSource.Add(element, obj);

                decimal dest = _tempDestination[element];
                _dicDestination.Add(element, dest);
                if (SHPTFromLine)
                {
                    decimal line = _tempLine[element];
                    _dicLine.Add(element, line);
                }
            }
        }

        private void FillTempDictionaries()
        {
            if (SHPTFromLine)
            {
                var subDataset = SelectedSourceDataset.SubDatasets.First(u => !u.IsNCT);

                foreach (var element in subDataset.DepthAndCurves)
                {
                    _tempLine.Add(element.Depth, element.Curve);
                }
            }

            foreach (var element in SelectedSourceDataset.DepthAndCurves)
            {
                _tempSource.Add(element.Depth, element.Curve);
            }

            foreach (var element in SelectedDestinationDataset.DepthAndCurves)
            {
                _tempDestination.Add(element.Depth, element.Curve);
            }
        }

        private void CalculateDepthAndCurves(IEnumerable<decimal> commonDepths)
        {
            RefreshDictionary();
            FillTempDictionaries();
            FillSourceDestinationDictionaries(commonDepths);

            bool havePreviousValue = false;
            decimal previousValue = 0;
            foreach (var info in _dicSource)
            {
                decimal sourceDatasetValue = 0, destinationDatasetValue = 0, lineDatasetValue = 0;
                sourceDatasetValue = _dicSource[info.Key];
                destinationDatasetValue = _dicDestination[info.Key];
                if (SHPTFromLine)
                    lineDatasetValue = _dicLine[info.Key];

                bool isTrue = false;

                if (SHPTFromLine)
                    isTrue = IsGreaterThan ? sourceDatasetValue >= lineDatasetValue : sourceDatasetValue <= lineDatasetValue;
                else
                    isTrue = IsGreaterThan ? sourceDatasetValue >= VShaleUnitValue : sourceDatasetValue <= VShaleUnitValue;

                if (isTrue)
                {
                    havePreviousValue = true;
                    previousValue = destinationDatasetValue;
                    CurrentObject.DepthAndCurves.Add(new DepthCurveInfo
                    {
                        Depth = info.Key,
                        Curve = destinationDatasetValue
                    });
                }
                else if (havePreviousValue)
                {
                    CurrentObject.DepthAndCurves.Add(new DepthCurveInfo
                    {
                        Depth = info.Key,
                        Curve = previousValue
                    });
                }
            }
        }

        private void RefreshDictionary()
        {
            _tempLine.Clear();
            _tempDestination.Clear();
            _tempSource.Clear();
            _dicLine.Clear();
            _dicDestination.Clear();
            _dicSource.Clear();
        }

        private IEnumerable<decimal> GetCommonDepths()
        {
            IEnumerable<decimal> sourceDepths;
            if (SHPTFromLine)
                sourceDepths = SelectedSourceDataset.SubDatasets.First(u => !u.IsNCT).DepthAndCurves.Where(u => u.Curve != -999 && u.Curve != decimal.Parse("-999.25")).Select(u => u.Depth);
            else
                sourceDepths = SelectedSourceDataset.DepthAndCurves.Where(u => u.Curve != -999 && u.Curve != decimal.Parse("-999.25")).Select(u => u.Depth);
            var destinationDepths = SelectedDestinationDataset.DepthAndCurves.Where(u => u.Curve != -999 && u.Curve != decimal.Parse("-999.25")).Select(u => u.Depth);
            var overlappingDepths = sourceDepths.Intersect(destinationDepths);

            return overlappingDepths;
        }

        protected override bool CanSave()
        {
            return SelectedDestinationDataset != null && SelectedSourceDataset != null
                && !string.IsNullOrWhiteSpace(CurrentObject.Name);
        }

        public override void Cancel()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CancelShalePointFilter);
        }
    }//end class
}//end namespace
