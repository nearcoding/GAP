using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using Ninject;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class AddTrackViewModel : ExtendedBaseViewModel<Track>
    {
        ICommand _chartSelectionChangedCommand;
        Chart _selectedChart;

        public AddTrackViewModel(string token)
            : base(token)
        {
            Charts = GlobalCollection.Instance.Charts;

            if (Charts.Any() && IoC.Kernel.Get<IGlobalDataModel>().MainViewModel != null)
            {
                if (!Charts.Contains(IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject))                
                    SelectedChart = Charts.First();                
                else
                    SelectedChart = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject != null ?
                        IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject : Charts.First();
            }
            NotifyPropertyChanged("CurrentObject");
        }
        public Chart SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                _selectedChart = value;
                if (value == null) return;
                CurrentObject.RefChart = value.ID;
                CurrentObject.Name = GetValidName();
                NotifyPropertyChanged("CurrentObject");                
            }
        }

        public IEnumerable<Chart> Charts { get; set; }

        protected override bool CanSave()
        {
            return (!string.IsNullOrWhiteSpace(CurrentObject.RefChart) && !string.IsNullOrWhiteSpace(CurrentObject.Name));
        }

        protected override bool AddObjectValidation()
        {
            if (SelectedChart.Tracks.Any(u => u.Name == CurrentObject.Name))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBox(
                   IoC.Kernel.Get<IResourceHelper>().ReadResource("TrackNameIsInUse"), Token);
                return false;
            }
            return true;
        }

        protected override bool UpdateObjectValidation()
        {
            return true;
        }

        protected override bool CommonValidation()
        {
            return true;
        }

        public override void Save()
        {
            if (!AddObjectValidation()) return;
            TrackManager.Instance.AddTrackObject(CurrentObject);
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        protected override string GetValidName()
        {
            //executes the application, add a new track and then close all projects and windows
            //even the track new screen is being closed it still keeps the view model alive for some reason
            //which further throws null reference exception
            if (SelectedChart == null) return string.Empty;
            var lstTracks = SelectedChart.Tracks.Where
                (u => u.RefChart == CurrentObject.RefChart && u.Name.StartsWith("Track_")).Select(v => v.Name);

            return GlobalDataModel.GetIncrementalEntityName<Track>(lstTracks);
        }
    }//end class
}//end namespace
