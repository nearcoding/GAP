using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using System.Reactive.Linq;
using System.ComponentModel;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class RemoveChartViewModel : MultipleRemoveBaseViewModel, IDisposable
    {
        public RemoveChartViewModel(string token)
            : base(token)
        {
            GetChartRemoveInfo();
            SaveButtonText = IoC.Kernel.Get<IResourceHelper>().ReadResource("RemoveCharts");
            AllRecordsSelected = false;
        }

        protected override bool CanSave()
        {
            return Charts.Any(u => u.IsChartSelected);
        }

        //if this flag is not used then Notify Property changed goes to stackoverflow exception
        private bool _updatingChartsIsSelected;

        protected override void UpdateCheckboxes(bool value)
        {
            _updatingChartsIsSelected = true; ;
            foreach (var chart in Charts)
                chart.IsChartSelected = value;

            _updatingChartsIsSelected = false;
            UpdateButtonText();
        }

        public override void Save()
        {
            ChartRemovalApproved = false;
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.RemoveChart);
            if (!ChartRemovalApproved) return;

            var selectedCharts = Charts.OrderByDescending(v => v.DisplayIndex).Where(u => u.IsChartSelected).Select(v => v.ID);
            foreach (var chartID in selectedCharts)
            {
                var chart = HelperMethods.Instance.GetChartByID(chartID);
                if (chart != null) ChartManager.Instance.RemoveChartObject(chart);
            }
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        public bool ChartRemovalApproved { get; set; }

        public ExtendedBindingList<ChartRemoveInfo> Charts { get; set; }
        
        /// <summary>
        /// only reason of having this separate list is because this collection supports sorting 
        /// while other collection raised notification upon selection changed
        /// </summary>
       public List<ChartRemoveInfo> ChartsList { get; set; }

        private void GetChartRemoveInfo()
        {
            Charts = new ExtendedBindingList<ChartRemoveInfo>();
            Charts.ListChanged += Charts_ListChanged;

            foreach (var chart in GlobalCollection.Instance.Charts.OrderBy(u => u.DisplayIndex))
            {
                var info = new ChartRemoveInfo
                {
                    Chart = chart.Name,
                    ID = chart.ID,
                    DisplayIndex = chart.DisplayIndex
                };

                var tracksCount = chart.Tracks.Count;
                info.Tracks = string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("NoOfTracksInThisChart"), tracksCount);

                var curveCount = HelperMethods.Instance.GetCurvesByChart(chart.Name).Count();
                info.Curves = string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("NoOfCurvesInThisChart"), curveCount);
                Charts.Add(info);
            }
           ChartsList = Charts.ToList();

            _listFull = true;
        }

        bool _listFull;

        void Charts_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (_updatingChartsIsSelected) return;
            if (!_listFull) return;

            UpdateButtonText();

            if (Charts.Any(u => u.IsChartSelected) && Charts.Any(u => !u.IsChartSelected))
                AllRecordsSelected = null;
            else if (Charts.All(u => u.IsChartSelected))
                AllRecordsSelected = true;
            else if (Charts.All(u => !u.IsChartSelected))
                AllRecordsSelected = false;
        }

        private void UpdateButtonText()
        {
            var selectedCharts = Charts.Count(u => u.IsChartSelected);
            SaveButtonText = string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("RemoveNoOfCharts"), selectedCharts);
        }

        public void Dispose()
        {
            while (Charts.Any())
                Charts.RemoveAt(0);

            while (ChartsList.Any())
                ChartsList.RemoveAt(0);

            Charts.ListChanged -= Charts_ListChanged;       
        }
    }//end class
}//end namespace
