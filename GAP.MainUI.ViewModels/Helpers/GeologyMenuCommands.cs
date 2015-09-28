using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using GAP.Helpers;
using Ninject;
using GAP.BL.CollectionEntities;
using GAP.MainUI.ViewModels.Helpers;
using GAP.BL;
using Abt.Controls.SciChart.Visuals.Annotations;
using System.Windows;
using System.Windows.Controls;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class GeologyMenuCommands : SubBaseEntity
    {
        string _token;
        public GeologyMenuCommands(string token)
        {
            _token = token;
        }

        ICommand _geologyLithologyPropertiesCommand;

        private void GeologyLithologyProperties()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.LithologyProperties);
        }

        private bool CanGeologyLithologyProperties()
        {
            return true;
        }

        public ICommand GeologyLithologyPropertiesCommand
        {
            get { return _geologyLithologyPropertiesCommand ?? (_geologyLithologyPropertiesCommand = new RelayCommand(GeologyLithologyProperties, CanGeologyLithologyProperties)); }
        }

        ICommand _geologyLithologyAddRemoveCommand;

        private void AddRemoveLithologies()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.AddRemoveLithologyScreen);
        }

        private bool CanAddRemoveLithologies()
        {
            return HelperMethods.Instance.AnyTrackExists();
        }

        public ICommand GeologyLithologyAddRemoveCommand
        {
            get { return _geologyLithologyAddRemoveCommand ?? (_geologyLithologyAddRemoveCommand = new RelayCommand(AddRemoveLithologies, CanAddRemoveLithologies)); }
        }

        ICommand _geologyLithologyShowHideCommand;

        bool _isLithologyVisible;
        public bool IsLithologyVisible
        {
            get { return _isLithologyVisible; }
            set
            {
                if (value == _isLithologyVisible) return;
                _isLithologyVisible = value;
                if (value && IsFullLithology)
                {
                    NotifyPropertyChanged("IsLithologyVisible");
                    return;
                }
                if (!value && IsFullLithology)
                {
                    IsFullLithology = false;
                }
                if (!GlobalDataModel.ShouldSave && GlobalCollection.Instance.Projects.Any()) GlobalDataModel.ShouldSave = true;
                SetHasCurvesInCaseOfLithology(value);
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.CalculateMinMaxVisibleRangeLimitForYAxis();
                NotifyPropertyChanged("IsLithologyVisible");
            }
        }

        bool _isFullLithology;

        public bool IsFullLithology
        {
            get { return _isFullLithology; }
            set
            {
                if (value == _isFullLithology) return;
                _isFullLithology = value;
                SetHasCurvesInCaseOfLithology(value);
                if (!GlobalDataModel.ShouldSave && GlobalCollection.Instance.Projects.Any()) GlobalDataModel.ShouldSave = true;
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.CalculateMinMaxVisibleRangeLimitForYAxis();
                LithologyX1 = value ? 0 : 0.9;
                if (value) IsLithologyVisible = true;
                NotifyPropertyChanged("IsFullLithology");
            }
        }

        double _lithologyX1;
        public double LithologyX1
        {
            get { return _lithologyX1; }
            set
            {
                _lithologyX1 = value;
                NotifyPropertyChanged("LithologyX1");
            }
        }

        public void SetHasCurvesInCaseOfLithology(bool value)
        {
            var tracks = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Where(u => u.Tracks != null).SelectMany(v => v.Tracks);
            tracks = tracks.Where(u => u.Curves.Any());
            tracks = tracks.Where(u => u.TrackObject.Curves.Any(v => v.RefDataset == "Lithology")).ToList();
            foreach (var track in tracks)
            {
                if (!track.TrackObject.Lithologies.Any()) continue;
                track.HasCurves = IsLithologyVisible == false && IsFullLithology == false ? track.Curves.Count > 1 : true;
                track.Curves[0].Visibility = (IsLithologyVisible || IsFullLithology) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        private void GeologyLithologyShowHide()
        {
            IsLithologyVisible = !IsLithologyVisible;
        }

        private bool CanGeologyLithologyShowHide()
        {
            return HelperMethods.Instance.AnyTrackExists();
        }

        public ICommand GeologyLithologyShowHideCommand
        {
            get { return _geologyLithologyShowHideCommand ?? (_geologyLithologyShowHideCommand = new RelayCommand(GeologyLithologyShowHide, CanGeologyLithologyShowHide)); }
        }

        ICommand _geologyFormationTopNewCommand;

        private void GeologyFormationTopNew()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.FormationTopScreen);
        }

        private bool CanGeologyFormationTopNew()
        {
            return HelperMethods.Instance.AnyTrackExists();
        }

        public ICommand GeologyFormationTopNewCommand
        {
            get { return _geologyFormationTopNewCommand ?? (_geologyFormationTopNewCommand = new RelayCommand(GeologyFormationTopNew, CanGeologyFormationTopNew)); }
        }

        ICommand _geologyFormationTopShowHideCommand;

        private void GeologyFormationTopShowHide()
        {
            IsFormationVisible = !IsFormationVisible;
        }

        private bool CanGeologyFormationTopShowHide()
        {
            return HelperMethods.Instance.AnyTrackExists();
        }

        public ICommand GeologyFormationTopShowHideCommand
        {
            get { return _geologyFormationTopShowHideCommand ?? (_geologyFormationTopShowHideCommand = new RelayCommand(GeologyFormationTopShowHide, CanGeologyFormationTopShowHide)); }
        }
        ICommand _geologyFormationImportFormationCommand;
        public ICommand GeologyFormationImportFormationCommand
        {
            get { return _geologyFormationImportFormationCommand ?? (_geologyFormationImportFormationCommand = new RelayCommand(GeologyFormationImportFormation)); }
        }
        private void GeologyFormationImportFormation()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.ImportFormation);
        }

        ICommand _geologyFormationExportFormationCommand;
        public ICommand GeologyFormationExportFormationCommand
        {
            get
            {
                return _geologyFormationExportFormationCommand ?? (_geologyFormationExportFormationCommand = new RelayCommand(GeologyFormationExportFormation,
                    () => HelperMethods.Instance.AnyFormationExists()));
            }
        }
        private void GeologyFormationExportFormation()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.ExportFormation);
        }

        ICommand _geologyRotaryTableElevationCommand;

        private void GeologyRotaryTableElevation()
        {

        }

        private bool CanGeologyRotaryTableElevation()
        {
            return false;
        }

        public ICommand GeologyRotaryTableElevationCommand
        {
            get { return _geologyRotaryTableElevationCommand ?? (_geologyRotaryTableElevationCommand = new RelayCommand(GeologyRotaryTableElevation, CanGeologyRotaryTableElevation)); }
        }


        ICommand _geologyGroundLevelElevationCommand;

        private void GeologyGroundLevelElevation()
        {

        }

        private bool CanGeologyGroundLevelElevation()
        {
            return false;
        }

        public ICommand GeologyGroundLevelElevationCommand
        {
            get { return _geologyGroundLevelElevationCommand ?? (_geologyGroundLevelElevationCommand = new RelayCommand(GeologyGroundLevelElevation, CanGeologyGroundLevelElevation)); }
        }

        ICommand _geologyLithologyImportLithologyCommand;
        public ICommand GeologyLithologyImportLithologyCommand
        {
            get
            {
                return _geologyLithologyImportLithologyCommand ??
                    (_geologyLithologyImportLithologyCommand = new RelayCommand(LithologyImport,
           () => HelperMethods.Instance.AnyTrackExists()));
            }
        }

        private void LithologyImport()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.ImportLithologyData);
        }

        ICommand _geologyLithologyExportLithologyCommand;
        public ICommand GeologyLithologyExportLithologyCommand
        {
            get
            {
                return _geologyLithologyExportLithologyCommand ??
                    (_geologyLithologyExportLithologyCommand = new RelayCommand(LithologyExport, () => HelperMethods.Instance.AnyLithologyExists()));
            }
        }

        private void LithologyExport()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.ExportLithologyData);
        }


        bool _isFormationVisible;

        public bool IsFormationVisible
        {
            get { return _isFormationVisible; }
            set
            {
                if (value == _isFormationVisible) return;
                _isFormationVisible = value;
                if (!GlobalDataModel.ShouldSave && GlobalCollection.Instance.Projects.Any()) GlobalDataModel.ShouldSave = true;
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.CalculateMinMaxVisibleRangeLimitForYAxis();
                NotifyPropertyChanged("IsFormationVisible");
            }
        }
        bool _isFTVisible, _isFTNameVisible;
        public bool IsFTNameVisible
        {
            get { return _isFTNameVisible; }
            set
            {
                if (_isFTNameVisible == value) return;
                _isFTNameVisible = value;
                NotifyPropertyChanged("IsFTNameVisible");
            }
        }

        public bool? IsFTTooltipVisible
        {
            get { return _isFTVisible; }
            set
            {
                if (_isFTVisible == value) return;
                _isFTVisible = value.Value;
                foreach (var chart in IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Where(u => u.ChartObject.Formations.Any()))
                {
                    foreach (var track in chart.Tracks)
                    {
                        var formationAxis = track.XAxisCollection.Single(u => u.Id == "Formation");
                        foreach (var annotation in track.Annotations)
                        {
                            var lineAnnotation = annotation as LineAnnotationExtended;
                            if (lineAnnotation == null) continue;
                            lineAnnotation.SetValue(ToolTipService.IsEnabledProperty, value);
                        }
                    }
                }
                NotifyPropertyChanged("IsFTTooltipVisible");
            }
        }

    }//end class
}//end namespace
