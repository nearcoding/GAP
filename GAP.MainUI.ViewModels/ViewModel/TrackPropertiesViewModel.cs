using GAP.BL;
using GAP.Helpers;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using AutoMapper;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class TrackPropertiesViewModel : ChartTrackPropertiesBaseViewModel
    {
        public TrackPropertiesViewModel(string token)
            : base(token)
        {
            Charts = GlobalCollection.Instance.Charts.OrderBy(u=>u.DisplayIndex).ToList();
            Tracks = new ExtendedBindingList<Track>();
            Mapper.CreateMap(typeof(Track),typeof(Track));
            if (Charts.Any() && IoC.Kernel.Get<IGlobalDataModel>().MainViewModel != null)
            {
                SelectedChart = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject != null ?
                    IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject : Charts.First();
            }
        }

        Chart _selectedChart;

        public Chart SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                if (_selectedChart == value) return;
                _selectedChart = value;
                ChartSelectionChanged();
                NotifyPropertyChanged("SelectedChart");
            }
        }

        public IEnumerable<Chart> Charts { get; set; }
        public ExtendedBindingList<Track> Tracks { get; set; }

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

        private void ChartSelectionChanged()
        {
            if (SelectedChart == null) return;
            Tracks.Clear();
            foreach (var track in SelectedChart.Tracks.OrderBy(u=>u.DisplayIndex).ToList())
            {
                Tracks.Add((Track)Mapper.Map(track, typeof(Track), typeof(Track)));
            }
            NotifyPropertyChanged("Tracks");
        }

        protected override bool CanSave()
        {
            return Tracks.Any();
        }

        public override void Save()
        {
            var chartToShowObject=ChartManager.Instance.GetChartToShowObjectByID(SelectedChart.ID);
            chartToShowObject.Tracks.Clear();
            foreach (var track in Tracks.OrderBy(u=>u.DisplayIndex))
            {
                var previousObject = HelperMethods.Instance.GetTrackByID(track.ID);
                if (previousObject != null)
                    previousObject.DisplayIndex = track.DisplayIndex;
                TrackManager.Instance.AddTrackObject(track);
            }

            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        protected override void Up()
        {
            int currentIndex = Tracks.IndexOf(SelectedTrack);
            Tracks[currentIndex - 1].DisplayIndex = currentIndex;
            SelectedTrack.DisplayIndex = currentIndex - 1;

            Tracks = new ExtendedBindingList<Track>(Tracks.OrderBy(u => u.DisplayIndex).ToList());
            NotifyPropertyChanged("Tracks");
            SelectedTrack = Tracks[currentIndex - 1];
        }

        protected override void Down()
        {
            int currentIndex = Tracks.IndexOf(SelectedTrack);
            Tracks[currentIndex + 1].DisplayIndex = currentIndex;
            SelectedTrack.DisplayIndex = currentIndex + 1;

            Tracks = new ExtendedBindingList<Track>(Tracks.OrderBy(u => u.DisplayIndex).ToList());
            NotifyPropertyChanged("Tracks");
            SelectedTrack = Tracks[currentIndex + 1];
        }

        protected override bool CanUp()
        {
            return SelectedTrack != null && Tracks.IndexOf(SelectedTrack) > 0;
        }

        protected override bool CanDown()
        {
            return SelectedTrack != null && Tracks.IndexOf(SelectedTrack) < Tracks.Count - 1;
        }
    }//end class
}//end namespace
