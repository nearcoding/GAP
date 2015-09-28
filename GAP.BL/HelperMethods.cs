using AutoMapper;
using GAP.BL;
using GAP.BL.CollectionEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace GAP.BL
{
    public class HelperMethods
    {
        public static T GetNewObject<T>(T originalObject)
        {
            Mapper.CreateMap<T, T>();
            return (T)Mapper.Map(originalObject, typeof(T), typeof(T));
        }

        public IEnumerable<SubDataset> GetSubDatasetsByDataset(string datasetID)
        {
            return HelperMethods.Instance.GetDatasetByID(datasetID).SubDatasets;
        }

        public LineAnnotationExtended GetLineAnnotationByAnnotationInfoAndSubDataset(AnnotationInfo annotation, SubDataset subDataset, CurveToShow curveToShow)
        {
            var lineAnnotation = new LineAnnotationExtended
            {
                X1 = annotation.X1,
                X2 = annotation.X2,
                Y1 = annotation.Y1,
                Y2 = annotation.Y2,
                SubDataset = subDataset,
                CurveToShow = curveToShow,
                XAxisId = curveToShow.CurveObject.ID
            };

            return lineAnnotation;
        }

        static HelperMethods _instance;

        private HelperMethods()
        {

        }

        public string GetAppDataFolder()
        {
            string folderName = string.Empty;
            try
            {

                //folderName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                //folderName = folderName + "\\GAP";
                //if (!Directory.Exists(folderName))
                //   Directory.CreateDirectory(folderName);
                return System.Environment.CurrentDirectory;

            }
            catch (Exception ex)
            {

            }
            return folderName;
        }

        public static HelperMethods Instance
        {
            get
            {
                if (_instance == null) _instance = new HelperMethods();
                return _instance;
            }
        }
        public Project GetProjectByID(string projectID)
        {
            return GlobalCollection.Instance.Projects.SingleOrDefault(u => u.ID == projectID.Trim());
        }

        public IEnumerable<Curve> GetCurvesByDatasetID(string datasetID)
        {
            var allCurves = GlobalCollection.Instance.Charts.SelectMany(u => u.Tracks.SelectMany(v => v.Curves));
            if (allCurves == null)
                return null;
            return allCurves.Where(u => u.RefDataset == datasetID);
        }

        public SubDataset GetSubDatasetObjectBySubdatasetDetails(SubDataset subDataset)
        {
            var dataset = HelperMethods.Instance.GetDatasetByID(subDataset.Dataset);
            return dataset.SubDatasets.SingleOrDefault(u => u.IsNCT == subDataset.IsNCT && u.Name == subDataset.Name);
        }

        public IEnumerable<Chart> GetChartsWithTracks()
        {
            return GlobalCollection.Instance.Charts.Where(u => u.Tracks.Any());
        }

        public IEnumerable<Dataset> GetDatasetsByCurves()
        {
            List<Dataset> lstDatasets = new List<Dataset>();
            foreach (var curve in GetAllCurves())
            {
                lstDatasets.Add(GetDatasetByID(curve.RefDataset));
            }
            return lstDatasets.Distinct().AsEnumerable();
        }

        public IEnumerable<Dataset> GetAllDatasets()
        {
            var wells = GetAllWells();
            return wells.Where(u => u.Datasets != null).SelectMany(u => u.Datasets);
        }

        public IEnumerable<Dataset> GetFilteredDatasets(Func<Project, bool> predicate)
        {
            var wells = GetFilteredWells(predicate);
            return wells.Where(u => u.Datasets != null).SelectMany(u => u.Datasets);
        }

        public IEnumerable<Project> GetAllProjects(Func<Project, bool> whereClause = null)
        {
            if (whereClause == null)
                return GlobalCollection.Instance.Projects;
            else
                return GlobalCollection.Instance.Projects.Where(whereClause);
        }

        public IEnumerable<Well> GetAllWells()
        {
            var projects = GetAllProjects();
            return projects.Where(u => u.Wells != null).SelectMany(u => u.Wells);
        }
        public IEnumerable<Well> GetFilteredWells(Func<Project, bool> whereClause)
        {
            var projects = GetAllProjects(whereClause);
            return projects.Where(u => u.Wells != null).SelectMany(u => u.Wells);
        }

        public IEnumerable<Curve> GetAllCurves(IEnumerable<Chart> charts = null, bool includeLithologyCurves = false)
        {
            if (charts == null)
                charts = GlobalCollection.Instance.Charts;
            if (!includeLithologyCurves)
                return charts.SelectMany(u => u.Tracks.SelectMany(v => v.Curves)).Where(u => u.RefProject != "Lithology");
            else       
                return charts.SelectMany(u => u.Tracks.SelectMany(v => v.Curves));
        }

        public IEnumerable<Well> GetWellsWithDatasetsByProjectID(string projectID)
        {
            return GlobalCollection.Instance.Projects.Single(u => u.ID == projectID).Wells.Where(v => v.Datasets.Any());
        }

        public IEnumerable<Project> GetProjectsWithDatasets(Func<Project, bool> whereClause = null)
        {
            if (whereClause == null)
                return GlobalCollection.Instance.Projects.Where(u => u.Wells.Any(v => v.Datasets.Any()));
            else
            {
                var projects = GlobalCollection.Instance.Projects.Where(whereClause);
                return projects.Where(u => u.Wells.Any(v => v.Datasets.Any()));
            }
        }

        public IEnumerable<Project> GetProjectsWithCurves()
        {
            var projectsWithCurves = GetDatasetsByCurves().Select(u => u.RefProject).Distinct();
            return GlobalCollection.Instance.Projects.Where(u => projectsWithCurves.Contains(u.ID));
        }

        public IEnumerable<Project> ProjectsWithWells()
        {
            return GlobalCollection.Instance.Projects.Where(u => u.Wells.Any());
        }
                        
        public IEnumerable<Project> ProjectsWithRHOBDatasets()
        {
            var datasets = HelperMethods.Instance.GetAllDatasets().Where(u => u.IsTVD && u.Family == "Density" && u.Units == "gr/cm3");
            var uniqueProjectIds=datasets.Select(u=>u.RefProject).Distinct();
            return HelperMethods.Instance.GetAllProjects(u => uniqueProjectIds.Contains(u.ID));
        }

        public IEnumerable<Dataset> GetRHOBDatasets(Well well)
        {
            var datasets = HelperMethods.Instance.GetAllDatasets().Where(u => u.IsTVD && u.Family == "Density" && u.Units == "gr/cm3");
            return datasets.Where(u => u.RefWell == well.ID);
        }

        public IEnumerable<Well> WellsWithRHOBDatasets(Project project)
        {
            var datasets = project.Wells.SelectMany(u => u.Datasets);
            var uniquewellIds = datasets.Select(u => u.RefWell).Distinct();
            return HelperMethods.Instance.GetAllWells().Where(u => uniquewellIds.Contains(u.ID));
        }

        public IEnumerable<LithologyInfo> GetAllLithologies()
        {
            return GlobalCollection.Instance.Charts.SelectMany(v => v.Tracks.SelectMany(u => u.Lithologies));
        }

        public IEnumerable<FormationInfo> GetAllFormations()
        {
            return GlobalCollection.Instance.Charts.SelectMany(u => u.Formations);
        }

        public bool AnyDatasetExist()
        {
            return GlobalCollection.Instance.Projects.Any(u => u.Wells.Any(v => v.Datasets.Any()));
        }

        public IEnumerable<Chart> ChartsWithLithologies()
        {
            return GlobalCollection.Instance.Charts.Where(u => u.Tracks.Any(v => v.Lithologies.Any()));
            //var chartNames = TracksWithLithologies().Select(v => v.RefChart).Distinct();
            //return GlobalCollection.Instance.Charts.Where(u => chartNames.Contains(u.ID));
        }

        public IEnumerable<Track> TracksWithLithologies()
        {
            var allChartsWithLithologies = GlobalCollection.Instance.Charts.Where(u => u.Tracks.Any(v => v.Lithologies.Any()));

            return allChartsWithLithologies.SelectMany(u => u.Tracks.Where(v => v.Lithologies.Any()));

            //foreach(var chart in allChartsWithLithologies)
            //{
            //    var tracks = chart.Tracks.Where(u=>u.Lithologies.Any());

            //}
            //var tracks = chartWithLithology.SelectMany(u => u.Tracks.Any(v => v.Lithologies.Any()));
            //return tracks.SelectMany(w => w.Tracks.Any(u=>u.Lithologies));
        }

        public bool AnyLithologyExists()
        {
            return GlobalCollection.Instance.Charts.Any(u => u.Tracks.Any(v => v.Lithologies.Any()));
        }

        public bool AnyFormationExistsUnderThisChart(string chartID)
        {
            var chartObject = GetChartByID(chartID);
            if (chartObject == null) return false;
            return chartObject.Formations.Any();
        }

        public bool AnyFormationExists()
        {
            return GlobalCollection.Instance.Charts.Any(u => u.Formations.Any());
        }

        public IEnumerable<Curve> GetCurvesByChart(string chartName)
        {
            var curves = GlobalCollection.Instance.Charts.Single(u => u.Name == chartName).Tracks.SelectMany(v => v.Curves);
            return curves.Except(curves.Where(u => u.RefProject == "Lithology"));
        }

        public IEnumerable<Curve> GetCurvesByTrackID(string trackID)
        {
            return GetTrackByID(trackID).Curves;
        }

        public void RemoveCurve(Curve curveToRemove)
        {
            var curveObject = GlobalCollection.Instance.Charts.Single(u => u.ID == curveToRemove.RefChart)
                   .Tracks.Single(v => v.ID == curveToRemove.RefTrack)
                   .Curves.Single(w => w.RefDataset == curveToRemove.RefDataset);

            GlobalCollection.Instance.Charts.Single(u => u.ID == curveToRemove.RefChart)
                   .Tracks.Single(v => v.ID == curveToRemove.RefTrack).Curves.Remove(curveToRemove);
        }

        public bool AnyCurveExists()
        {
            var allTracks = GlobalCollection.Instance.Charts.Where(u => u.Tracks != null).SelectMany(v => v.Tracks);
            if (allTracks == null) return false;
            var allCurves = allTracks.Where(u => u.Curves != null).SelectMany(v => v.Curves);
            if (allCurves == null) return false;
            return allCurves.Any(u => u.RefProject != "Lithology");
        }

        public IEnumerable<Chart> GetChartsWithFormations()
        {
            return GlobalCollection.Instance.Charts.Where(u => u.Formations.Any());
        }

        public IEnumerable<Track> GetAllTracks(IEnumerable<Chart> charts = null)
        {
            if (charts == null)
                return GlobalCollection.Instance.Charts.SelectMany(u => u.Tracks);
            else
                return charts.SelectMany(u => u.Tracks);
        }

        public bool AnyTrackExists()
        {
            return GlobalCollection.Instance.Charts.Any(u => u.Tracks.Any());
        }

        public Well GetWellByID(string wellID)
        {
            var wells = GlobalCollection.Instance.Projects.Where(u => u.Wells != null).SelectMany(v => v.Wells);
            return wells.SingleOrDefault(u => u.ID == wellID);
        }

        public Dataset GetDatasetByID(string datasetID)
        {
            var datasets = GlobalCollection.Instance.Projects.Where(u => u.Wells != null).SelectMany(v => v.Wells.Where(w => w.Datasets != null).SelectMany(x => x.Datasets));
            return datasets.SingleOrDefault(u => u.ID == datasetID);
        }

        public IEnumerable<Well> GetWellsByProjectID(string projectID)
        {
            return GetProjectByID(projectID).Wells;
        }

        public IEnumerable<Dataset> GetDatasetsByWellID(string wellID)
        {
            return GetWellByID(wellID).Datasets;
        }

        public Chart GetChartByID(string id, IEnumerable<Chart> charts = null)
        {
            if (charts == null)
                return GlobalCollection.Instance.Charts.SingleOrDefault(u => u.ID == id);
            else
                return charts.SingleOrDefault(u => u.ID == id);
        }

        public IEnumerable<Track> GetTracksByChartID(string chartID)
        {
            return GetChartByID(chartID).Tracks;
        }

        public Curve GetCurveById(string curveId,IEnumerable<Chart> charts=null, bool includeLithologyCurves = false)
        {
            return GetAllCurves(charts,includeLithologyCurves).SingleOrDefault(u => u.ID == curveId);
        }

        public Track GetTrackByID(string trackID, IEnumerable<Chart> charts = null)
        {
            return GetAllTracks(charts).SingleOrDefault(u => u.ID == trackID);
        }
    }//end class
}//end namespace
