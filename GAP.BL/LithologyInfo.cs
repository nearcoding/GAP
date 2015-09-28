using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAP.BL
{
    public class LithologyInfo : BaseEntity
    {
        bool _isLithologySelected;
        public bool IsLithologySelected
        {
            get { return _isLithologySelected; }
            set
            {
                _isLithologySelected = value;
                NotifyPropertyChanged("IsLithologySelected");
            }
        }
        
        string _lithologyName;
        public string LithologyName
        {
            get { return _lithologyName; }
            set
            {
                _lithologyName = value;
                NotifyPropertyChanged("LithologyName");
            }
        }

        public decimal InitialDepth { get; set; }

        public decimal FinalDepth { get; set; }

        public string RefChart { get; set; }

        public string RefTrack { get; set; }

        public string ImageFile { get; set; }
    }//end class
}//end namespace
