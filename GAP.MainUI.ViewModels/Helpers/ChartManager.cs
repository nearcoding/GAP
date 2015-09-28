using System.Linq;
using Ninject;
using GAP.BL;
using System.Collections.Generic;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class ChartManager
    {
        static ChartManager _instance = new ChartManager();

        private ChartManager() { }

        public static ChartManager Instance { get { return _instance; } }

        public void AddChartObject(Chart chart, IEnumerable<Chart> charts = null)
        {
            var chartObject = HelperMethods.Instance.GetChartByID(chart.ID, charts);
            if (chartObject == null && charts == null) //in case user tries to add same object twice
            {
                chart.DisplayIndex = GlobalCollection.Instance.Charts.Any() ? GlobalCollection.Instance.Charts.Max(u => u.DisplayIndex) + 1 : 0;
                GlobalCollection.Instance.Charts.Add(chart);
            }
            AddChartToShowObject(chart, charts);

            if (GlobalCollection.Instance.Charts.Count == 1 && charts == null)
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.First();
        }

        private void AddChartToShowObject(Chart chart, IEnumerable<Chart> charts = null)
        {
            var chartToShow = GetChartToShowObjectByID(chart.ID, charts);
            if (chartToShow != null) return; //in case user tries to add  chart to show object twice
            var chartToShowObject = new ChartToShow
            {
                ChartObject = chart
            };
            if (charts == null)
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Add(chartToShowObject);
            else
                GlobalDataModel.Instance.ChartToShowObjects.Add(chartToShowObject);
        }

        public ChartToShow GetChartToShowObjectByID(string chartId, IEnumerable<Chart> charts = null)
        {
            if (charts == null)
                return IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.SingleOrDefault(u => u.ChartObject.ID == chartId);
            else
                return GlobalDataModel.Instance.ChartToShowObjects.SingleOrDefault(u => u.ChartObject.ID == chartId);            
        }

        public void RemoveChartObject(Chart chart)
        {
            RemoveChartToShowObject(chart.ID);
            GlobalCollection.Instance.Charts.Remove(chart);
        }

        private void RemoveChartToShowObject(string chartID)
        {
            var chartToShowObject = GetChartToShowObjectByID(chartID);
            IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Remove(chartToShowObject);
        }

        public void SelectedChartChanged()
        {
            var selectedChart = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart;
            if (selectedChart == null) return;

            var chartObject = selectedChart.ChartObject;
            if (chartObject == null) return;

            if (chartObject.Tracks.Any())
            {
                foreach (var track in chartObject.Tracks.OrderBy(u => u.DisplayIndex))
                    TrackManager.Instance.AddTrackObject(track);
            }

            if (chartObject.Formations.Any())
            {
                foreach (var formation in chartObject.Formations.ToList())
                    FormationManager.Instance.AddFormationObject(formation);
            }

            if (GlobalDataModel.Instance.IsSubDatasetOpen == true)
            {
                GlobalDataModel.LineEditorViewModel.InitializeCurveToShowObjects();
                GlobalDataModel.LineEditorViewModel.Refresh();
            }
            IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SyncZoom();
        }
    }//end class
}//end namespace
