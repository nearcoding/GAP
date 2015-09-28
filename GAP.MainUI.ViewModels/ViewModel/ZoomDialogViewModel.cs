using GalaSoft.MvvmLight.Command;
using GAP.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Ninject;
using GAP.Helpers;
using Abt.Controls.SciChart;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class ZoomDialogViewModel : BaseViewModel<BaseEntity>
    {
        public ZoomDialogViewModel(string token)
            : base(token)
        {
            Charts = GlobalCollection.Instance.Charts.ToList();
            if (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart != null)
                SelectedChart = HelperMethods.Instance.GetChartByID(IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject.ID);
            else if (Charts.Any())
                SelectedChart = Charts.First();
        }

        public List<Chart> Charts { get; set; }
        ICommand _zoomCommand;
        public ICommand ZoomCommand
        {
            get { return _zoomCommand ?? (_zoomCommand = new RelayCommand(Zoom, () => CanZoom())); }
        }

        Chart _selectedChart;
        public Chart SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                _selectedChart = value;
                var chart = Charts.Single(u => u.Name == _selectedChart.Name);
                Tracks = chart.Tracks;
                NotifyPropertyChanged("SelectedChart");
            }
        }

        IEnumerable<Track> _tracks;
        public IEnumerable<Track> Tracks
        {
            get { return _tracks; }
            set
            {
                _tracks = value;
                NotifyPropertyChanged("Tracks");
            }
        }

        private void Zoom()
        {
            var tracks = Tracks.Where(u => u.IsEntitySelected.Value);

            IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.IsSyncZoom = false;
            foreach (var track in tracks)
            {
                var chartToShow = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.SingleOrDefault(u => u.ChartObject.ID == track.RefChart);
                var trackToShow = chartToShow.Tracks.SingleOrDefault(u => u.TrackObject.ID == track.ID);
                trackToShow.YAxis.VisibleRange = new DoubleRange(MinValue, MaxValue);
            }

            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseScreen);
        }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        private bool CanZoom()
        {
            return Tracks.Any(u => u.IsEntitySelected.Value);
        }
    }//end class
}//end namespace
