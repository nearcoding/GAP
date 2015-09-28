using GAP.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class TrackRemoveInfo : BaseEntity
    {
        public string Track { get; set; }

        public string RefChart { get; set; }

        public string Curves { get; set; }

        bool _isTrackSelected;
        public bool IsTrackSelected
        {
            get { return _isTrackSelected; }
            set
            {
                _isTrackSelected = value;
                NotifyPropertyChanged("IsTrackSelected");
            }
        }
    }//end class
}///end namespace
