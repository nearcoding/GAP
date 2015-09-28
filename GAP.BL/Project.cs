using GAP.BL.CollectionEntities;
using GAP.Helpers;
using System;
namespace GAP.BL
{
    public class Project : BaseEntity
    {
        public Project()
        {            
            State = string.Empty;
            GeoBasin = string.Empty;
            Country = string.Empty;
        }

        internal WellCollection WellCollection { get; set; }
        ExtendedBindingList<Well> _wells;
        public ExtendedBindingList<Well> Wells
        {
            get 
            {
                if (_wells == null)
                {
                    WellCollection collection = new WellCollection();
                    _wells = collection.CurrentList;
                    WellCollection = collection;
                }
                return _wells; 
            }
            set
            {
                _wells = value;
                NotifyPropertyChanged("Wells");
            }
        }

        public override bool Equals(object obj)
        {
            Project project = obj as Project;
            if (project == null) return false;
            return Name.Equals(project.Name) && Country.Equals(project.Country)
                && State.Equals(project.State) && GeoBasin.Equals(project.GeoBasin) && Units == project.Units;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        string _country, _state, _geobasin, _units;

        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                NotifyObject();
            }
        }

        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                NotifyObject();
            }
        }

        public string GeoBasin
        {
            get { return _geobasin; }
            set
            {
                _geobasin = value;
                NotifyObject();
            }
        }

        public string Units
        {
            get { return _units; }
            set
            {
                _units = value;
                NotifyObject();
            }
        }
    }//end class
}//end namespace
