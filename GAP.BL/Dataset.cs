using System.Windows.Media;
using GAP.Helpers;
using System.Collections.ObjectModel;
using GAP.BL.CollectionEntities;
using System.Collections.Generic;

namespace GAP.BL
{
    public class Dataset : BaseEntity
    {
        public Dataset()
        {
            IsTVD = true;
            RefWell = string.Empty;
            Family = string.Empty;
            Units = string.Empty;
            ShouldApplyBorderColor = false;
            LineStyle = 1;
            FinalDepth = 20;
            Step = 1;
            InitialDepth = 1;
            LineColor = Color.FromArgb(255, 0, 0, 0);
            MarkerColor = Color.FromArgb(255, 0, 0, 0);
            ShouldApplyBorderColor = false;
            MinUnitValue = 0;
            MaxUnitValue = 100;
            DepthAndCurves = new ObservableCollection<DepthCurveInfo>();

            SubDatasetCollection collection = new SubDatasetCollection();
            SubDatasets = collection.CurrentList;
            SubDatasetCollection = collection;

            Curves = new List<DatasetCurveInfo>();
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(this.Name) && !(string.IsNullOrWhiteSpace(this.Family)) && !string.IsNullOrWhiteSpace(this.Units)
                &&!string.IsNullOrWhiteSpace(this.RefProject) && !string.IsNullOrWhiteSpace(this.RefWell);
        }

        string _userNotes;
        public string UserNotes
        {
            get { return _userNotes; }
            set
            {
                _userNotes= value;
                NotifyPropertyChanged("UserNotes");
                NotifyObject();
            }
        }

        string _systemNotes;
        public string SystemNotes
        {
            get { return _systemNotes; }
            set
            {
                _systemNotes = value;
                NotifyPropertyChanged("SystemNotes");
            }
        }

        /// <summary>
        /// this property will help us determine which curves were drawn when the dataset is being re-adding while doing undo/redo
        /// </summary>
        public List<DatasetCurveInfo> Curves { get; set; }

        internal SubDatasetCollection SubDatasetCollection { get; set; }
        ExtendedBindingList<SubDataset> _subDatasets;
        public ExtendedBindingList<SubDataset> SubDatasets
        {
            get { return _subDatasets; }
            set
            {
                _subDatasets = value;
                NotifyPropertyChanged("SubDatasets");
            }
        }

        public override bool Equals(object obj)
        {
            Dataset dataset = obj as Dataset;
            if (Units == null || dataset == null)
                return false;
            return RefProject.Equals(dataset.RefProject) && RefWell.Equals(dataset.RefWell) && Family.Equals(dataset.Family) &&
                Units.Equals(dataset.Units) && IsTVD == dataset.IsTVD && LineStyle == dataset.LineStyle && Name.Equals(dataset.Name) &&
                LineGrossor == dataset.LineGrossor && MarkerStyle == dataset.MarkerStyle && MarkerSize == dataset.MarkerSize &&
                Step == dataset.Step && MinUnitValue == dataset.MinUnitValue && MaxUnitValue == dataset.MaxUnitValue &&
                MarkerColor.Equals(dataset.MarkerColor) && ShouldApplyBorderColor.Equals(dataset.ShouldApplyBorderColor) &&
                string.Compare(UserNotes, dataset.UserNotes, true) == 0 &&
                string.Compare(SystemNotes, dataset.SystemNotes, true) == 0 &&
                InitialDepth == dataset.InitialDepth && FinalDepth == dataset.FinalDepth && LineColor.Equals(dataset.LineColor);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        string _refProject, _refWell, _family, _units;
        int _lineStyle, _lineGrosser, _markerStyle, _markerSize, _step;
        decimal _minUnitValue, _maxUnitValue, _initialDepth, _finalDepth;
        bool _isTVD;
        bool? _shouldApplyBorderColor;
        Colour _lineColor, _markerColor;

        public string RefProject
        {
            get { return _refProject; }
            set
            {
                _refProject = value;
                NotifyObject();
            }
        }

        public string RefWell
        {
            get { return _refWell; }
            set
            {
                _refWell = value;
                NotifyPropertyChanged("RefWell");
                NotifyObject();
            }
        }

        public string Family
        {
            get { return _family; }
            set
            {
                _family = value;
                NotifyPropertyChanged("Family");
                NotifyObject();
            }
        }

        public string Units
        {
            get { return _units; }
            set
            {
                _units = value;
                NotifyObject();
            }
        }

        public bool IsTVD
        {
            get { return _isTVD; }
            set
            {
                _isTVD = value;
                NotifyPropertyChanged("IsTVD");
                NotifyObject();
            }
        }

        public int LineStyle
        {
            get { return _lineStyle; }
            set
            {
                _lineStyle = value;
                NotifyPropertyChanged("LineStyle");
                NotifyObject();
            }
        }
        public int LineGrossor
        {
            get { return _lineGrosser; }
            set
            {
                _lineGrosser = value;
                NotifyPropertyChanged("LineGrossor");
                NotifyObject();
            }
        }

        public decimal MinUnitValue
        {
            get { return _minUnitValue; }
            set
            {
                _minUnitValue = value;
                NotifyObject();
            }
        }

        public decimal MaxUnitValue
        {
            get { return _maxUnitValue; }
            set
            {
                _maxUnitValue = value;
                NotifyObject();
            }
        }

        public int MarkerStyle
        {
            get { return _markerStyle; }
            set
            {
                _markerStyle = value;
                NotifyPropertyChanged("MarkerStyle");
                NotifyObject();
            }
        }

        public int MarkerSize
        {
            get { return _markerSize; }
            set
            {
                _markerSize = value;
                NotifyPropertyChanged("MarkerSize");
                NotifyObject();
            }
        }

        public Colour LineColor
        {
            get { return _lineColor; }
            set
            {
                _lineColor = value;
                NotifyPropertyChanged("LineColor");
                NotifyObject();
            }
        }
        public Colour MarkerColor
        {
            get { return _markerColor; }
            set
            {
                _markerColor = value;
                NotifyPropertyChanged("MarkerColor");
                NotifyObject();
            }
        }

        public bool? ShouldApplyBorderColor
        {
            get { return _shouldApplyBorderColor; }
            set
            {
                if (value == _shouldApplyBorderColor) return;
                _shouldApplyBorderColor = value;
                NotifyObject();
                NotifyPropertyChanged("ShouldApplyBorderColor");
            }
        }
        ObservableCollection<DepthCurveInfo> _depthAndCurves;
        public ObservableCollection<DepthCurveInfo> DepthAndCurves
        {
            get { return _depthAndCurves; }
            set
            {
                _depthAndCurves = value;
                NotifyPropertyChanged("DepthAndCurves");
            }
        }
        public decimal InitialDepth
        {
            get { return _initialDepth; }
            set
            {
                _initialDepth = value;
                NotifyPropertyChanged("DepthAndCurves");
                NotifyObject();
            }
        }

        public decimal FinalDepth
        {
            get { return _finalDepth; }
            set
            {
                _finalDepth = value;
                NotifyObject();
            }
        }
        public int Step
        {
            get { return _step; }
            set
            {
                _step = value;
                NotifyObject();
            }
        }
    }//end class
}//end namespace