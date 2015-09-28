using GalaSoft.MvvmLight.Command;
using GAP.BL;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Ninject;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class GoToMarkViewModel : BaseViewModel<BaseEntity>
    {
        public GoToMarkViewModel(string token)
            : base(token)
        {
            Charts = GlobalCollection.Instance.Charts.ToList();
            if (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart != null)
            {
                SelectedChart = Charts.SingleOrDefault(u => u.Name == IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject.Name);
            }
            else if (Charts.Any())
                SelectedChart = Charts.First();
        }

        public IEnumerable<Chart> Charts { get; set; }

        Chart _selectedChart;
        public Chart SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                _selectedChart = value;
                Tracks = _selectedChart.Tracks;
                if (Tracks.Any()) SelectedTrack = Tracks.First();
                NotifyPropertyChanged("Tracks");
                NotifyPropertyChanged("SelectedChart");
            }
        }

        public IEnumerable<Track> Tracks { get; set; }

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

        ICommand _goToMarkCommand;
        public ICommand GoToMarkCommand
        {
            get { return _goToMarkCommand ?? (_goToMarkCommand = new RelayCommand(GoToMark)); }
        }

        private void GoToMark()
        {
            using (new WaitCursor())
            {
                var chart = HelperMethods.Instance.GetChartByID(SelectedTrack.RefChart);
                if (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject.Name != SelectedChart.Name)
                {
                    var selectedChartToShow = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Single(u => u.ChartObject.ID == SelectedTrack.RefChart);
                    IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart = selectedChartToShow;
                }
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.SelectedTrack = _selectedTrack.Name;
                foreach (var track in IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.Tracks)
                    track.IsTrackSelected = false;
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.Tracks.Single(u => u.TrackObject.Name == _selectedTrack.Name).IsTrackSelected = true;
                //GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseScreen);
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.GoToMark);
            }
        }
    }//end class
}//end namespace
