using GAP.BL.CollectionEntitiess;
using GAP.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace GAP.BL
{   

    public class Well : BaseEntity
    {
        string _refProject, _location, _operator, _country, _angularLatitudeDegrees, _angularLongitudeDegrees,
              _angularLatitudeMinutes, _angularLongitudeMinutes, _angularLongitudeSeconds, _angularLatitudeSeconds,
              _polarLatitude, _datum, _airgap, _waterDepth, _polarLongitude, _waterDensity, _normalPore, _regionalFract, _regionalOver;

        int _mftWaterDepth, _mftAirGap, _mftPolarLongitude, _mftPolarLatitude, _esteOeste, _norteSur;
        bool _onOffShore;
        public  Well()
        {
            _refProject = string.Empty;
            _location = string.Empty;
            _operator = string.Empty;
            _country = string.Empty;
            _angularLatitudeDegrees = string.Empty;
            _angularLongitudeDegrees = string.Empty;
            _angularLatitudeMinutes = string.Empty;
            _angularLongitudeMinutes = string.Empty;
            _angularLatitudeSeconds = string.Empty;
            _angularLongitudeSeconds = string.Empty;
            _polarLatitude = string.Empty;
            _datum = string.Empty;
            _airgap = string.Empty;
            _waterDensity = string.Empty;
            _polarLongitude = string.Empty;
            _waterDepth = string.Empty;
            _normalPore = string.Empty;
            _regionalFract = string.Empty;
            _regionalOver = string.Empty;
            //reason for setting value to -1, default value of int is 0. 
            //So default object comparison in Equals method always says 0 is not equal to -1
            _mftAirGap = -1;
            _mftWaterDepth = -1;
            _mftPolarLongitude = -1;
            _mftPolarLatitude = -1;
            _esteOeste = -1;
            _norteSur = -1;
            EngineerNames = new ObservableCollection<string>();
            EngineerNames.CollectionChanged += EngineerNames_CollectionChanged;

            DatasetCollection collection = new DatasetCollection();
            Datasets = collection.CurrentList;
            DatasetCollection = collection;
           // OnEntitySelectionChanged += Well_OnEntitySelectionChanged;
        }

        internal DatasetCollection DatasetCollection { get; set; }
        ExtendedBindingList<Dataset> _datasets;
        public ExtendedBindingList<Dataset> Datasets
        {
            get { return _datasets; }
            set
            {
                _datasets = value;
                NotifyPropertyChanged("Datasets");
            }
        }
        void EngineerNames_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyObject();
        }

        public override bool Equals(object obj)
        {
            Well well = obj as Well;
            if (well == null) return false;
            return Name.Equals(well.Name) && RefProject.Equals(well.RefProject) && Location.Equals(well.Location)
                && Datum.Equals(well.Datum) && FinalDepth.Equals(well.FinalDepth)
                && Operator.Equals(well.Operator) && Country.Equals(well.Country) && AngularLatitudeDegrees.Equals(_angularLatitudeDegrees)
                && AngularLongitudDegrees.Equals(well.AngularLongitudDegrees) && AngularLatitudeMinutes.Equals(well.AngularLatitudeMinutes)
                && AngularLatitudeSeconds.Equals(well.AngularLatitudeSeconds) && PolarLatitude.Equals(well.PolarLatitude)
                && WaterDensity.Equals(well.WaterDensity) && WaterDepth.Equals(well.WaterDepth) && NormalPore.Equals(well.NormalPore)
                && RegionalFract.Equals(well.RegionalFract) && RegionalOver.Equals(well.RegionalOver)
                && MftWaterDepth.Equals(well.MftWaterDepth) && MftAirGap.Equals(well.MftAirGap) && EngineerNames.SequenceEqual(well.EngineerNames)
                && MftPolarLatitude.Equals(well.MftPolarLatitude) && MftPolarLongitude.Equals(well.MftPolarLongitude)
                && PolarLatitude.Equals(well.PolarLatitude) && PolarLongitude.Equals(well.PolarLongitude)
                && EsteOeste.Equals(well.EsteOeste) && OnOffShore.Equals(well.OnOffShore) && NorteSur.Equals(well.NorteSur)
                && AirGap.Equals(well.AirGap);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        decimal _finalDepth;
        public decimal FinalDepth
        {
            get { return _finalDepth; }
            set
            {
                if (!decimal.TryParse(value.ToString(), out _finalDepth))
                    _finalDepth = 0;
                NotifyObject();
                NotifyPropertyChanged("FinalDepth");
            }
        }

        public string RefProject
        {
            get { return _refProject; }
            set
            {
                _refProject = value;
                NotifyObject();
            }
        }

        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                NotifyObject();
            }
        }

        public string Operator
        {
            get { return _operator; }
            set
            {
                _operator = value;
                NotifyObject();
            }
        }

        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                NotifyObject();
            }
        }

        public bool OnOffShore
        {
            get { return _onOffShore; }
            set
            {
                _onOffShore = value;
                NotifyPropertyChanged("OnOffShore");//enable disable the offshore data in maintain well group box
                NotifyObject();
            }
        }

        public string AngularLongitudDegrees
        {
            get { return _angularLongitudeDegrees; }
            set
            {
                _angularLongitudeDegrees = value;
                NotifyObject();
            }
        }

        public string AngularLongitudMinutes
        {
            get { return _angularLongitudeMinutes; }
            set
            {
                _angularLongitudeMinutes = value;
                NotifyObject();
            }
        }

        public string AngularLongitudSeconds
        {
            get { return _angularLongitudeSeconds; }
            set
            {
                _angularLongitudeSeconds = value;
                NotifyObject();
            }
        }

        public int EsteOeste
        {
            get { return _esteOeste; }
            set
            {
                _esteOeste = value;
                NotifyObject();
            }
        }
        public string AngularLatitudeDegrees
        {
            get { return _angularLatitudeDegrees; }
            set
            {
                _angularLatitudeDegrees = value;
                NotifyObject();
            }
        }

        public string AngularLatitudeMinutes
        {
            get { return _angularLatitudeMinutes; }
            set
            {
                _angularLatitudeMinutes = value;
                NotifyObject();
            }
        }

        public string AngularLatitudeSeconds
        {
            get { return _angularLatitudeSeconds; }
            set
            {
                _angularLatitudeSeconds = value;
                NotifyObject();
            }
        }

        public int NorteSur
        {
            get { return _norteSur; }
            set
            {
                _norteSur = value;
                NotifyObject();
            }
        }

        public string PolarLongitude
        {
            get { return _polarLongitude; }
            set
            {
                _polarLongitude = value;
                NotifyObject();
            }
        }

        public int MftPolarLongitude
        {
            get { return _mftPolarLongitude; }
            set
            {
                _mftPolarLongitude = value;
                NotifyObject();
            }
        }
        public string PolarLatitude
        {
            get { return _polarLatitude; }
            set
            {
                _polarLatitude = value;
                NotifyObject();
            }
        }
        public int MftPolarLatitude
        {
            get { return _mftPolarLatitude; }
            set
            {
                _mftPolarLatitude = value;
                NotifyObject();
            }
        }

        public string Datum
        {
            get { return _datum; }
            set
            {
                _datum = value;
                NotifyObject();
            }
        }
        public string AirGap
        {
            get { return _airgap; }
            set
            {
                _airgap = value;
                NotifyObject();
            }
        }

        public int MftAirGap
        {
            get { return _mftAirGap; }
            set
            {
                _mftAirGap = value;
                NotifyObject();
            }
        }
        public string WaterDepth
        {
            get { return _waterDepth; }
            set
            {
                _waterDepth = value;
                NotifyObject();
            }
        }
        public int MftWaterDepth
        {
            get { return _mftWaterDepth; }
            set
            {
                _mftWaterDepth = value;
                NotifyObject();
            }
        }

        public string WaterDensity
        {
            get { return _waterDensity; }
            set
            {
                _waterDensity = value;
                NotifyObject();
            }
        }

        public ObservableCollection<string> EngineerNames { get; set; }

        public string NormalPore
        {
            get { return _normalPore; }
            set
            {
                _normalPore = value;
                NotifyObject();
            }
        }
        public string RegionalFract
        {
            get { return _regionalFract; }
            set
            {
                _regionalFract = value;
                NotifyObject();
            }
        }
        public string RegionalOver
        {
            get { return _regionalOver; }
            set
            {
                _regionalOver = value;
                NotifyObject();
            }
        }
    }//end class
}//end namespace
