using GAP.BL.CollectionEntities;
using GAP.Helpers;
using System.Linq;

namespace GAP.BL
{
    public class GlobalCollection
    {
        public static GlobalCollection _instance = new GlobalCollection();
        public bool IsLoading { get; set; }
        public static GlobalCollection Instance
        {
            get
            {
                return _instance;
            }
        }

        public string UndoBatchName { get; set; }

        public LithologyInfo DeletedLithology { get; set; }
        public Chart DeletedChart { get; set; }
        public FormationInfo DeletedFormation { get; set; }
        public Track DeletedTrack { get; set; }

        public void ClearAll()
        {
            DeletedChart = null;
            DeletedCurve = null;
            DeletedFormation = null;
            DeletedLithology = null;
            DeletedTrack = null;
            ChartCollection.Instance.DeletedEntity = null;
            ProjectCollection.Instance.DeletedEntity = null;
            foreach (var project in ProjectCollection.Instance.CurrentList)
            {
                foreach (var well in project.WellCollection.CurrentList)
                {
                    foreach (var dataset in well.DatasetCollection.CurrentList)
                    {
                        dataset.DepthAndCurves.Clear();
                        dataset.DepthAndCurves = null;
                    }
                    well.DatasetCollection.CurrentList.Clear();

                    well.DatasetCollection.DeletedEntity = null;
                }
                project.WellCollection.CurrentList.Clear();
                project.WellCollection.DeletedEntity = null;
            }

            ProjectCollection.Instance.CurrentList.Clear();

            foreach (var chart in ChartCollection.Instance.CurrentList)
            {
                foreach (var track in chart.TrackCollection.CurrentList)
                {
                    if (track.Curves != null)
                    {
                        track.Lithologies.Clear();
                        track.Lithologies = null;
                        track.Curves.Clear();
                        track.Curves = null;
                    }
                    if (track.ChartAnnotations != null)
                    {
                        track.ChartAnnotations.Clear();
                        track.ChartAnnotations = null;
                    }
                }
                chart.TrackCollection.CurrentList.Clear();
            }

            ChartCollection.Instance.CurrentList.Clear();
        }

        public Curve DeletedCurve { get; set; }

        private GlobalCollection()
        {
            Projects = ProjectCollection.Instance.CurrentList;
            Charts = ChartCollection.Instance.CurrentList;
        }

        public ExtendedBindingList<Chart> Charts { get; set; }

        public ExtendedBindingList<Project> Projects { get; set; }

        public void SortItems<T>(ExtendedBindingList<T> currentList, bool raiseListChangedEvents = false) where T : BaseEntity
        {
            currentList.RaiseListChangedEvents = raiseListChangedEvents;

            var lst = currentList.ToList();
            while (currentList.Any())
                currentList.RemoveAt(0);

            lst = lst.OrderBy(u => u.DisplayIndex).ToList();

            lst.ForEach(u => currentList.Add(u));

            currentList.RaiseListChangedEvents = true;
        }
    }//end class
}//end namespace
