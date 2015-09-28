using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Ninject;
using System;
using System.Reactive.Linq;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class RemoveCurveViewModel : MultipleRemoveBaseViewModel, IDisposable
    {
        public RemoveCurveViewModel(string token)
            : base(token)
        {
            GetCurvesInfo();
            SaveButtonText = IoC.Kernel.Get<IResourceHelper>().ReadResource("RemoveCurves");
            AllRecordsSelected = false;
        }

        public bool CurveRemovalApproved { get; set; }

        public ExtendedBindingList<CurveRemoveInfo> Curves { get; set; }
        IDisposable _disposableCurve;
        public List<CurveRemoveInfo> CurvesList { get; set; }
        private void GetCurvesInfo()
        {

            Curves = new ExtendedBindingList<CurveRemoveInfo>();
            _disposableCurve = Observable.FromEventPattern<ListChangedEventArgs>(Curves, "ListChanged").Subscribe(u =>
                {
                    Curves_ListChanged(u.Sender, u.EventArgs);
                });

            foreach (var chart in GlobalCollection.Instance.Charts.OrderBy(u => u.DisplayIndex))
            {
                foreach (var track in chart.Tracks.OrderBy(u => u.DisplayIndex))
                {
                    var validCurves = track.Curves.Where(u => u.RefProject != "Lithology");
                    foreach (var curve in validCurves)
                    {
                        var curveRemoveInfo = new CurveRemoveInfo
                        {
                            DatasetCurveName = curve.RefDataset,
                            RefChart = curve.RefChart,
                            RefProject = curve.RefProject,
                            RefTrack = curve.RefTrack,
                            RefWell = curve.RefWell,
                            DisplayIndex = curve.DisplayIndex
                        };
                        Curves.Add(curveRemoveInfo);
                    }
                }
            }
            CurvesList = Curves.ToList();
        }

        private bool _updatingCurvesIsSelected = false;

        protected override void UpdateCheckboxes(bool value)
        {
            _updatingCurvesIsSelected = true;
            foreach (CurveRemoveInfo curve in Curves)
            {
                curve.IsCurveSelected = value;
            }
            _updatingCurvesIsSelected = false;
            UpdateButtonText();
        }

        private void UpdateButtonText()
        {
            int count = Curves.Count(u => u.IsCurveSelected);
            SaveButtonText = string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("RemoveNoOfCurves"), count);
        }

        private bool CanRemoveCurve()
        {
            return Curves.Any(u => u.IsCurveSelected);
        }

        protected override bool CanSave()
        {
            return Curves.Any(u => u.IsCurveSelected);
        }

        public override void Save()
        {
            CurveRemovalApproved = false;

            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.RemoveCurve);

            if (!CurveRemovalApproved) return;
            var selectedCurves = Curves.OrderByDescending(u => u.DisplayIndex).Where(u => u.IsCurveSelected);
            foreach (CurveRemoveInfo info in selectedCurves)
            {
                var chart = GlobalCollection.Instance.Charts.Single(u => u.ID == info.RefChart);
                var curves = HelperMethods.Instance.GetCurvesByTrackID(info.RefTrack);
                var curve = curves.SingleOrDefault
                    (u => u.RefProject == info.RefProject && u.RefWell == info.RefWell &&
                          u.RefDataset == info.DatasetCurveName);
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedTrack = curve.RefTrack;
                if (curve != null) CurveManager.Instance.RemoveCurveObject(curve);
            }

            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        void Curves_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (_updatingCurvesIsSelected)
                return;

            UpdateButtonText();

            if (Curves.Any(u => u.IsCurveSelected) && Curves.Any(u => !u.IsCurveSelected))
                AllRecordsSelected = null;
            else if (Curves.All(u => u.IsCurveSelected))
                AllRecordsSelected = true;
            else if (Curves.All(u => !u.IsCurveSelected))
                AllRecordsSelected = false;
        }

        public void Dispose()
        {
            _disposableCurve.Dispose();
        }
    }//end class
}//end namespace
