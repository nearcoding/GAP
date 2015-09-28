using GAP.BL;
using GAP.Helpers;
using System.Linq;
using Ninject;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class ChartPropertiesViewModel : ChartTrackPropertiesBaseViewModel
    {
        Chart _selectedChart;

        public ChartPropertiesViewModel(string token)
            : base(token)
        {
            Charts = new ExtendedBindingList<Chart>(GlobalCollection.Instance.Charts.OrderBy(u => u.DisplayIndex).ToList());
        }

        public ExtendedBindingList<Chart> Charts { get; set; }

        public Chart SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                _selectedChart = value;
                NotifyPropertyChanged("SelectedChart");
            }
        }

        protected override bool CanSave()
        {
            return Charts.Any();
        }

        public override void Save()
        {
            UpdateDisplayIndexOfChartsToShowObject();
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        private void UpdateDisplayIndexOfChartsToShowObject()
        {
            var chartsToShow = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.ToList();
            IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Clear();
            foreach(var item in GlobalCollection.Instance.Charts.OrderBy(u=>u.DisplayIndex))
            {
                var objectToAdd = chartsToShow.Single(u=>u.ChartObject.DisplayIndex==item.DisplayIndex);
                if (objectToAdd != null)
                    ChartManager.Instance.AddChartObject(objectToAdd.ChartObject);
            }
        }

        protected override void Up()
        {
            var currentIndex = Charts.IndexOf(SelectedChart);
            Charts[currentIndex - 1].DisplayIndex = currentIndex;
            SelectedChart.DisplayIndex = currentIndex - 1;

            Charts = new ExtendedBindingList<Chart>(Charts.OrderBy(u => u.DisplayIndex).ToList());
            SelectedChart = Charts[currentIndex - 1];
            NotifyPropertyChanged("Charts");
        }

        protected override void Down()
        {
            var currentIndex = Charts.IndexOf(SelectedChart);
            Charts[currentIndex + 1].DisplayIndex = currentIndex;
            SelectedChart.DisplayIndex = currentIndex + 1;

            Charts = new ExtendedBindingList<Chart>(Charts.OrderBy(u => u.DisplayIndex).ToList());
            SelectedChart = Charts[currentIndex + 1];
            NotifyPropertyChanged("Charts");
        }

        protected override bool CanUp()
        {
            return SelectedChart != null && Charts.IndexOf(SelectedChart) > 0;
        }

        protected override bool CanDown()
        {
            return SelectedChart != null && Charts.IndexOf(SelectedChart) < Charts.Count - 1;
        }
    }//end class
}//end namespace
