using GAP.BL;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class DatasetImportInfo:SubBaseEntity
    {
        public Dataset Dataset { get; set; }

        bool _isAccepted;
        public bool IsAccepted
        {
            get { return _isAccepted; }
            set
            {
                _isAccepted = value;
                NotifyPropertyChanged("IsAccepted");
            }
        }
    }//end class
}//end namespace
