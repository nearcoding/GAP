using GAP.Helpers;
using System.Linq;

namespace GAP.BL.CollectionEntities
{
    internal class ProjectCollection : BaseEntityCollection<Project>
    {
        public ProjectCollection()
            : base(UndoRedoType.Project) { }

        static ProjectCollection _projectCollection = new ProjectCollection();

        public static ProjectCollection Instance { get { return _projectCollection; } }
    }//end class
}//end namespace
