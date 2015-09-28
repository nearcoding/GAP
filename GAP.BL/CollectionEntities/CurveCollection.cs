using GAP.BL.CollectionEntities;
using GAP.Helpers;
using System.Linq;

namespace GAP.BL
{
    internal class CurveCollection : BaseEntityCollection<Curve>
    {
        public CurveCollection()
           : base(UndoRedoType.Curve)
        {            
            ShouldRemoveCurvesFromDataset = true;
        }
        public bool ShouldRemoveCurvesFromDataset { get; set; }

        protected  void ItemDeleted()
        {
            var curveObject = DeletedEntity;
           
            var dataset = HelperMethods.Instance.GetDatasetByID(curveObject.RefDataset);
            if (dataset == null) return;

            var well = HelperMethods.Instance.GetWellByID(dataset.RefWell);
            if (well == null) return;

            var project = HelperMethods.Instance.GetProjectByID(well.RefProject);
            if (project == null) return;

            if (ShouldRemoveCurvesFromDataset)
            {
                if (dataset.Curves.Any(u => u.RefChart == curveObject.RefChart && u.RefTrack == curveObject.RefTrack))
                {
                    var curveInfo = dataset.Curves.SingleOrDefault(u => u.RefChart == curveObject.RefChart && u.RefTrack == curveObject.RefTrack);
                    dataset.Curves.Remove(curveInfo);
                }
            }
        }

        protected override object AddObjectFromStack(int index)
        {
            var curveObject = CurrentList[index];

            var dataset = HelperMethods.Instance.GetDatasetByID(curveObject.RefDataset);
            if (dataset != null)
            {
                if (!dataset.Curves.Any(u => u.RefChart == curveObject.RefChart && u.RefTrack == curveObject.RefTrack))
                {
                    dataset.Curves.Add(new DatasetCurveInfo
                    {
                        RefChart = curveObject.RefChart,
                        RefTrack = curveObject.RefTrack
                    });
                }
            }
            if (!CurrentList.ShouldUndoRedo) return null;
            if (curveObject.RefProject == "Lithology") return null;
            return curveObject;
        }    
    }//end class
}//end namespace
