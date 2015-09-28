using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using System.Collections.Generic;

namespace GAP.MainUI.ViewModels
{
    public class ProjectsAndWells
    {
        public List<SubDataset> SubDatasets { get; set; }
        public List<Project> Proj { get; set; }

        public List<Well> Well { get; set; }

        public List<Dataset> Dataset { get; set; }
        
        public List<Chart> Charts { get; set; }

        public List<Track> Tracks { get; set; }

        public List<Curve> Curves { get; set; }

        public List<LithologyInfo> Lithologies { get; set; }

        public List<FormationInfo> Formations { get; set; }

        public List<NotesInfo> Notes { get; set; }

        public string ChartTheme { get; set; }

        public string AppTheme { get; set; }

        public string Language { get; set; }

       // public List<ProjectsToShow> ProjectsToShow { get; set; }
        public ProjectsAndWells()
        {

        }


        public bool LithologyVisible { get; set; }

        public bool FullLithologyVisible { get; set; }

        public bool FormationTopVisible { get; set; }
    }
}
