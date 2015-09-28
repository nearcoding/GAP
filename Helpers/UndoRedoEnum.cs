
namespace GAP.Helpers
{
    public enum UndoRedoType
    {
        Chart,
        Track,
        Curve,
        Annotations,
        Sorting,
        Project,
        Well,
        Dataset,
        Lithology,
        SubDataset
    }

    public enum OperationType
    {
        None,
        Undo,
        Redo
    }
}
