using GAP.BL;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class MultipleRemoveBaseViewModel : BaseViewModel<BaseEntity>
    {
        public MultipleRemoveBaseViewModel(string token)
            :base(token)
        {

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

        protected virtual void UpdateCheckboxes(bool value)
        {

        }

        string _saveButtonText;

        public string SaveButtonText
        {
            get { return _saveButtonText; }
            set
            {
                _saveButtonText = value;
                NotifyPropertyChanged("SaveButtonText");
            }
        }
    }
}
