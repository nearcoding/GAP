using GAP.BL;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class CurveRemoveInfo : BaseEntity
    {
        public bool _isCurveSelected;

        public bool IsCurveSelected
        {
            get { return _isCurveSelected; }
            set
            {
                _isCurveSelected = value;
                NotifyPropertyChanged("IsCurveSelected");
            }
        }

        public string RefChart { get; set; }

        public string RefTrack { get; set; }

        public string RefProject { get; set; }

        public string RefWell { get; set; }

        public string DatasetCurveName { get; set; }
    }
}
