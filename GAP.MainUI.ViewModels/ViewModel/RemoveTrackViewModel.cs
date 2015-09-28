using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using System;
using System.Reactive.Linq;
using System.ComponentModel;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class RemoveTrackViewModel : MultipleRemoveBaseViewModel, IDisposable
    {
        public ExtendedBindingList<TrackRemoveInfo> Tracks { get; set; }

        public List<TrackRemoveInfo> TracksList { get; set; }

        public RemoveTrackViewModel(string token)
            : base(token)
        {
            GetTracksInfo();
        }

        IDisposable _disposableTracks;
        private void GetTracksInfo()
        {
            Tracks = new ExtendedBindingList<TrackRemoveInfo>();
            _disposableTracks = Observable.FromEventPattern<ListChangedEventArgs>(Tracks, "ListChanged").Subscribe(u =>
                 {
                     Tracks_ListChanged(u.Sender, u.EventArgs);
                 });

            foreach (var chart in GlobalCollection.Instance.Charts.OrderBy(u => u.DisplayIndex))
            {
                foreach (var track in chart.Tracks.OrderBy(u => u.DisplayIndex))
                {
                    int curveCount = 0;

                    var info = new TrackRemoveInfo
                    {
                        ID = track.ID,
                        Track = track.Name,
                        DisplayIndex = track.DisplayIndex,
                        RefChart = track.RefChart,
                        Curves = string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("NoOfCurvesInThisTrack"), curveCount)
                    };
                    Tracks.Add(info);
                }
            }
            TracksList = Tracks.ToList();
        }

        //if this flag is not used then Notify Property changed goes to stackoverflow exception
        private bool _updatingTracksIsSelected = false;

        protected override void UpdateCheckboxes(bool value)
        {
            _updatingTracksIsSelected = true; ;
            foreach (TrackRemoveInfo track in Tracks)
            {
                track.IsTrackSelected = value;
            }
            _updatingTracksIsSelected = false;
            UpdateButtonText();
        }

        private void UpdateButtonText()
        {
            int count = Tracks.Count(u => u.IsTrackSelected);
            SaveButtonText = string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("RemoveNoOfTracks"), count);
        }

        protected override bool CanSave()
        {
            return Tracks.Any(u => u.IsTrackSelected);
        }

        public bool TrackRemovalApproved { get; set; }

        public override void Save()
        {
            TrackRemovalApproved = false;
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.RemoveTrack);

            if (!TrackRemovalApproved) return;
            var selectedTracks = Tracks.OrderByDescending(v => v.DisplayIndex).Where(u => u.IsTrackSelected);
            foreach (var info in selectedTracks)
            {
                var track = HelperMethods.Instance.GetTrackByID(info.ID);
                if (track == null) continue;
                var chart = GlobalCollection.Instance.Charts.Single(u => u.ID == info.RefChart);
                TrackManager.Instance.RemoveTrackObject(track);
                for (int i = 0; i < chart.Tracks.Count; i++)
                {
                    chart.Tracks[i].DisplayIndex = i;
                }
            }
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        void Tracks_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (_updatingTracksIsSelected)
                return;

            UpdateButtonText();

            if (Tracks.Any(u => u.IsTrackSelected) && Tracks.Any(u => !u.IsTrackSelected))
                AllRecordsSelected = null;
            else if (Tracks.All(u => u.IsTrackSelected))
                AllRecordsSelected = true;
            else if (Tracks.All(u => !u.IsTrackSelected))
                AllRecordsSelected = false;
        }

        public void Dispose()
        {
            _disposableTracks.Dispose();
        }
    }//end class
}//end namespace