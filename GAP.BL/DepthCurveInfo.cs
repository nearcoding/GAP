using System;

namespace GAP.BL
{

    [Serializable]
    public class DepthCurveInfo : BaseEntity
    {
        decimal _depth;
        decimal _curve;

        public Decimal Depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
                NotifyPropertyChanged("Depth");
            }
        }

        public Decimal Curve
        {
            get { return _curve; }
            set
            {
                _curve = value;
                NotifyPropertyChanged("Curve");
            }
        }
    }//end class
}//end namespace
