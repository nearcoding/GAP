using GAP.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAP.BL
{
    public class FormationInfo : BaseEntity
    {
        bool _isFormationSelected;
        public bool IsFormationSelected
        {
            get { return _isFormationSelected; }
            set
            {
                _isFormationSelected = value;
                NotifyPropertyChanged("IsFormationSelected");
            }
        }

        string _formationName;
        public string FormationName
        {
            get { return _formationName; }
            set
            {
                _formationName = value;
                NotifyPropertyChanged("FormationName");
            }
        }

        public decimal Depth { get; set; }

        public string RefChart { get; set; }
        public Colour FormationColor { get; set; }

        public int LineStyle { get; set; }

        public int LineGrossor { get; set; }

    }//end class
}//end namespace
