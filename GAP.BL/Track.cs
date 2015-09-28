using Abt.Controls.SciChart.Visuals.Annotations;
using GAP.BL.CollectionEntities;
using GAP.Helpers;
namespace GAP.BL
{
    public class Track : BaseEntity
    {
        public Track()
        {
            CurveCollection collection = new CurveCollection();
            Curves = collection.CurrentList;
            CurveCollection = collection;
            LithologyCollection lithologyCollection = new LithologyCollection();
            Lithologies = lithologyCollection.CurrentList;
            LithologyCollection = lithologyCollection;
            Width = 400;
        }

        internal CurveCollection CurveCollection { get; set; }

        public string RefChart { get; set; }

        double _width;
        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                NotifyPropertyChanged("Width");
            }
        }

        ExtendedBindingList<Curve> _curves;
        public ExtendedBindingList<Curve> Curves
        {
            get { return _curves; }
            set
            {
                _curves = value;
                NotifyPropertyChanged("Curves");
            }
        }

        public ExtendedBindingList<AnnotationInfo> ChartAnnotations { get; set; }
        internal LithologyCollection LithologyCollection { get; set; }
        public ExtendedBindingList<LithologyInfo> Lithologies { get; set; }

    }//end class

    public class AnnotationInfo : BaseEntity
    {       
        public string X1 { get; set; }
        public string X2 { get; set; }
        public string Y1 { get; set; }
        public string Y2 { get; set; }
        //public string RefChart { get; set; }
        //public string RefTrack { get; set; }
        public string SubDatasetName { get; set; }
    }//end class
}//end namespace
