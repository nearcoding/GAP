using Abt.Controls.SciChart;
using GAP.BL;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class ChartsBindingClass : INotifyPropertyChanged
    {
        public ChartsBindingClass()
        {
            Charts = new ObservableCollection<ChartsToShow>();
        }

        public ObservableCollection<ChartsToShow> Charts { get; set; }


        IRange _visibleRangeYAxis;
        public IRange VisibleRangeYAxis
        {
            get { return _visibleRangeYAxis; }
            set
            {
                _visibleRangeYAxis = value;
                NotifyPropertyChanged("VisibleRangeYAxis");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
