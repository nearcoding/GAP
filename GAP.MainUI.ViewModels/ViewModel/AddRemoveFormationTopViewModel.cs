using AutoMapper;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System;
using System.Reactive.Linq;
using System.ComponentModel;
using Ninject;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class AddRemoveFormationTopViewModel : BaseViewModel<FormationInfo>, IDisposable
    {
        IDisposable _disposableFormation, _disposableChart;
        Queue<InfoData<FormationInfo>> _queue = new Queue<InfoData<FormationInfo>>();
        ICommand _addFormationCommand;
        ICommand _deleteSelectedFormationCommand;

        public AddRemoveFormationTopViewModel(string token)
            : base(token)
        {
            Formations = new ExtendedBindingList<FormationInfo>();
            Mapper.CreateMap<FormationInfo, FormationInfo>();
            Mapper.Map(HelperMethods.Instance.GetAllFormations(), Formations);

            _disposableFormation = Observable.FromEventPattern<ListChangedEventArgs>(Formations, "ListChanged").Subscribe(
                 u =>
                 {
                     FormationListChanged();
                 });

            var dummyTracks = HelperMethods.Instance.GetChartsWithTracks();

            var lstCharts = dummyTracks.Distinct().ToList();
            CurrentObject = new FormationInfo
            {
                FormationColor = Color.FromArgb(255, 0, 0, 0),
                LineGrossor = 0,
                LineStyle = 0
            };

            Charts = new ExtendedBindingList<ChartSourceForMultipleSelection>();
            foreach (var chart in HelperMethods.Instance.GetChartsWithTracks())
            {
                Charts.Add(new ChartSourceForMultipleSelection
                {
                    ChartName = chart.Name,
                    IsChartSelected = false,
                    DisplayIndex = chart.DisplayIndex,
                    ID = chart.ID
                });
            }
            _disposableChart = Observable.FromEventPattern<ListChangedEventArgs>(Charts, "ListChanged").Subscribe(u =>
                   {
                       FillAddedFormations();
                   });

            AllRecordsSelected = false;
        }

        public ICommand DeleteSelectedFormationCommand
        {
            get { return _deleteSelectedFormationCommand ?? (_deleteSelectedFormationCommand = new RelayCommand(DeleteFormations)); }
        }

        public bool ShouldDeleteFormation { get; set; }

        private void DeleteFormations()
        {
            ShouldDeleteFormation = false;
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.ShouldDeleteFormation);
            if (!ShouldDeleteFormation) return;
            foreach (var formation in Formations.ToList().Where(u => u.IsFormationSelected))
            {
                _queue.Enqueue(new InfoData<FormationInfo>
                    {
                        InfoObject = formation,
                        IsAdded = false
                    });
                Formations.Remove(formation);
            }
        }

        bool? _allRecordsSelected;

        public bool? AllRecordsSelected
        {
            get { return _allRecordsSelected; }
            set
            {
                _allRecordsSelected = value;
                if (value != null) UpdateCheckboxes(value.Value);
                NotifyPropertyChanged("AllRecordsSelected");
            }
        }

        //if this flag is not used then Notify Property changed goes to stackoverflow exception
        private bool _updatingFormationIsSelected = false;

        private void UpdateCheckboxes(bool value)
        {
            _updatingFormationIsSelected = true;
            var lstSelectedCharts = Charts.Where(u => u.IsChartSelected).Select(v => v.ID);
            foreach (FormationInfo formation in Formations.Where(u => lstSelectedCharts.Contains(u.RefChart)))
            {
                formation.IsFormationSelected = value;
            }
            _updatingFormationIsSelected = false;
        }

        void Formations_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            FormationListChanged();
        }

        private void FormationListChanged()
        {
            FillAddedFormations();
            if (_updatingFormationIsSelected) return;

            if (Formations.Any(u => u.IsFormationSelected) && Formations.Any(u => !u.IsFormationSelected))
                AllRecordsSelected = null;
            else if (Formations.All(u => u.IsFormationSelected))
                AllRecordsSelected = true;
            else if (Formations.All(u => !u.IsFormationSelected))
                AllRecordsSelected = false;
        }

        public ICommand AddFormationCommand
        {
            get
            {
                return _addFormationCommand ??
                    (_addFormationCommand = new RelayCommand(AddFormation, CanAddFormation));
            }
        }

        private bool CanAddFormation()
        {
            return !string.IsNullOrWhiteSpace(CurrentObject.FormationName) && !string.IsNullOrWhiteSpace(CurrentObject.Depth.ToString());
        }

        public ExtendedBindingList<FormationInfo> Formations { get; set; }

        ObservableCollection<FormationInfo> _formationsToShow;

        public ObservableCollection<FormationInfo> FormationsToShow
        {
            get { return _formationsToShow; }
            set
            {
                _formationsToShow = value;
                NotifyPropertyChanged("FormationsToShow");
            }
        }

        private void AddFormation()
        {
            foreach (ChartSourceForMultipleSelection chart in Charts.Where(u => u.IsChartSelected))
            {
                if (Formations.Any(u => u.Depth == CurrentObject.Depth && u.RefChart == chart.ID))
                    continue;

                var formationObject = new FormationInfo
                {
                    Depth = CurrentObject.Depth,
                    IsFormationSelected = false,
                    RefChart = chart.ID,
                    FormationName = CurrentObject.FormationName,
                    FormationColor = CurrentObject.FormationColor,
                    LineStyle = CurrentObject.LineStyle,
                    LineGrossor = CurrentObject.LineGrossor,
                    DisplayIndex = GetDisplayIndexForNewlyAddedLithology(chart.ID)
                };
                FormationsToShow.Add(formationObject);

                _queue.Enqueue(new InfoData<FormationInfo>
                {
                    IsAdded = true,
                    InfoObject = formationObject
                });
            }

            CurrentObject.Depth = 0;
            CurrentObject.FormationName = string.Empty;
            NotifyPropertyChanged("CurrentObject");
        }

        private int GetDisplayIndexForNewlyAddedLithology(string chartID)
        {
            var currentList = Formations.Where(u => u.RefChart == chartID);
            return currentList.Any() ? currentList.Max(u => u.DisplayIndex) + 1 : 1;
        }

        public override void Save()
        {
            while (_queue.Count > 0)
            {
                var formationObject = _queue.Dequeue();
                var formation = formationObject.InfoObject as FormationInfo;
                var chartObject = HelperMethods.Instance.GetChartByID(formation.RefChart);
                if (formationObject.IsAdded)
                    FormationManager.Instance.AddFormationObject(formation);                    
                else
                {
                    var formationToDelete = chartObject.Formations.SingleOrDefault(u => u.Depth == formation.Depth);
                    if (formationToDelete != null)
                        FormationManager.Instance.RemoveFormationObject(formationToDelete);                        
                }
            }
            if (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.IsFormationVisible)
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.CalculateMinMaxVisibleRangeLimitForYAxis();
   
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.SaveFormations);
        }

        private void FillAddedFormations()
        {
            FormationsToShow = new ObservableCollection<FormationInfo>(Formations.Where(u => Charts.Where(v => v.IsChartSelected).Select(w => w.ID).Contains(u.RefChart)));
        }

        ExtendedBindingList<ChartSourceForMultipleSelection> _charts;
        public ExtendedBindingList<ChartSourceForMultipleSelection> Charts
        {
            get { return _charts; }
            set
            {
                _charts = value;
                NotifyPropertyChanged("Charts");
            }
        }

        public void Dispose()
        {
            _disposableFormation.Dispose();
            _disposableChart.Dispose();
        }
    }//end class
}//end namespace
