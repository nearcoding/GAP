using GAP.BL;
using GAP.MainUI.ViewModels.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
namespace GAP.MainUI.ViewModels.Helpers
{
    public class DatasetManager
    {
        private DatasetManager() { }

        static DatasetManager _instance = new DatasetManager();
        public static DatasetManager Instance { get { return _instance; } }

        public void RemoveDatasetObject(Dataset dataset)
        {
            var wellObject = HelperMethods.Instance.GetWellByID(dataset.RefWell);
            if (wellObject == null) return;
            RemoveCurvesByDataset(dataset);
            RemoveSubDatasetByDataset(dataset);
            wellObject.Datasets.Remove(dataset);
        }

        private void RemoveSubDatasetByDataset(Dataset dataset)
        {
            foreach (var subDataset in dataset.SubDatasets.ToList())
            {
                TrackManager.Instance.RemoveAllAnnotationsBySubDataset(subDataset);
            }
        }

        private void RemoveCurvesByDataset(Dataset dataset)
        {
            var curves = HelperMethods.Instance.GetCurvesByDatasetID(dataset.ID);
            while (curves.Any())
            {
                var curve = curves.First();
                var track = HelperMethods.Instance.GetTrackByID(curve.RefTrack);
                track.Curves.ShouldUndoRedo = false;
                CurveManager.Instance.RemoveCurveObject(curve);
                track.Curves.ShouldUndoRedo = true;
            }
        }

        public void AddDatasetObject(Dataset dataset)
        {
            var wellObject = HelperMethods.Instance.GetWellByID(dataset.RefWell);
            wellObject.Datasets.Add(dataset);
            AddCurvesByDataset(dataset);
        }

        private static void AddCurvesByDataset(Dataset dataset)
        {
            foreach (var curveObject in dataset.Curves)
            {
                var requiredCharts = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Where(u => u.Tracks != null && u.ChartObject.ID == curveObject.RefChart);
                var requiredTracks = requiredCharts.SelectMany(u => u.Tracks.Where(v => v.TrackObject.ID == curveObject.RefTrack));
                foreach (var track in requiredTracks)
                {
                    var curve = new Curve
                    {                        
                        RefChart = curveObject.RefChart,
                        RefTrack = curveObject.RefTrack,
                        RefDataset = dataset.ID,
                        RefProject = dataset.RefProject,
                        RefWell = dataset.RefWell,
                        IsSeriesVisible = true
                    };
                    var trackObj = HelperMethods.Instance.GetTrackByID(curve.RefTrack);
                    trackObj.Curves.ShouldUndoRedo = false;
                    CurveManager.Instance.AddCurveObject(curve);
                    ///trackObj.Curves.Add(curve);
                    trackObj.Curves.ShouldUndoRedo = true;
                }
            }
        }
    }//end class
}//end namespace
