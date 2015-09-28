using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GAP.BL
{
    public class SubDataset : BaseEntity
    {
        public SubDataset()
        {
            Step = 1;
            Annotations = new List<AnnotationInfo>();
            DepthAndCurves = new ObservableCollection<DepthCurveInfo>();
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

        string _userNotes;

        public string UserNotes
        {
            get { return _userNotes; }
            set
            {
                _userNotes = value;
                NotifyPropertyChanged("UserNotes");
            }
        }
        public string Project { get; set; }
        public string Well { get; set; }
        public string Dataset { get; set; }

        public List<AnnotationInfo> Annotations { get; set; }

        bool _isNCT;
        public bool IsNCT
        {
            get { return _isNCT; }
            set
            {
                _isNCT = value;
                NotifyPropertyChanged("IsNCT");
                NotifyObject();
            }
        }

        decimal _step;
        public decimal Step
        {
            get { return _step; }
            set
            {
                _step = value;
                NotifyPropertyChanged("Step");
            }
        }

        int _lineStyle;
        public int LineStyle
        {
            get { return _lineStyle; }
            set
            {
                _lineStyle = value;
                NotifyPropertyChanged("LineStyle");
            }
        }

        Colour _lineColor;
        public Colour LineColor
        {
            get { return _lineColor; }
            set
            {
                _lineColor = value;
                NotifyPropertyChanged("LineColor");
            }
        }
        int _lineGrossor;
        public int LineGrossor
        {
            get { return _lineGrossor; }
            set
            {
                _lineGrossor = value;
                NotifyPropertyChanged("LineGrossor");
            }
        }

        public ObservableCollection<DepthCurveInfo> DepthAndCurves { get; set; }
    }//end class
}//end namespace
