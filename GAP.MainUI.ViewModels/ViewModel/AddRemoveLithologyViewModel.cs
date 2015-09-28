using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using Ninject;
using AutoMapper;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using GAP.BL.CollectionEntities;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using Abt.Controls.SciChart.Model.DataSeries;
using System.Text;
using Abt.Controls.SciChart.Visuals.Annotations;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class AddRemoveLithologyViewModel : BaseViewModel<LithologyInfo>
    {
        ICommand _addLithologyCommand;
        ICommand _deleteSelectedLithologiesCommand;
        Queue<InfoData<LithologyInfo>> _queue = new Queue<InfoData<LithologyInfo>>();
        public AddRemoveLithologyViewModel(string token)
            : base(token)
        {
            Lithologies = new ExtendedBindingList<LithologyInfo>();

            Mapper.Map(HelperMethods.Instance.GetAllLithologies(), Lithologies);

            Lithologies.ListChanged += Lithologies_ListChanged;
            Charts = HelperMethods.Instance.GetChartsWithTracks();
            CurrentObject = new LithologyInfo();
            if (Charts.Any())
            {
                if (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart != null)
                    SelectedChart = Charts.SingleOrDefault(u => u.Name == IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject.Name);
                else
                    SelectedChart = Charts.First();
            }
            AllRecordsSelected = false;
        }

        public override void Save()
        {
            while (_queue.Count > 0)
            {
                var infoObject = _queue.Dequeue();
                var lithology = infoObject.InfoObject;
                var trackObject = HelperMethods.Instance.GetTrackByID(lithology.RefTrack);
                if (trackObject == null) continue;
                if (infoObject.IsAdded)
                    LithologyManager.Instance.AddLithologyObject(lithology);
                else
                {
                    var test = trackObject.Lithologies.GroupBy(u => u.ID).Where(v => v.Count() > 1).Select(w => new Tuple<int, string>(w.Count(), w.Key)).ToList();
                    //w.Count(),w.Key));
                    var lithologyToRemove = trackObject.Lithologies.Where(u => u.ID == lithology.ID);
                    foreach (var obj in lithologyToRemove.ToList())
                        LithologyManager.Instance.RemoveLithologyObject(obj);
                }
            }
            if (IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.IsLithologyVisible || IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.IsFullLithology)
            {
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.GeologyMenu.SetHasCurvesInCaseOfLithology(true);
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.CalculateMinMaxVisibleRangeLimitForYAxis();
            }
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.SaveLithologies);
        }

        void Lithologies_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemDeleted) return;
            FillAddedLithologies();

            if (Lithologies.Any(u => u.IsLithologySelected) && Lithologies.Any(u => !u.IsLithologySelected))
                AllRecordsSelected = null;
            else if (Lithologies.All(u => u.IsLithologySelected))
                AllRecordsSelected = true;
            else if (Lithologies.All(u => !u.IsLithologySelected))
                AllRecordsSelected = false;
        }

        public ICommand AddLithologyCommand
        {
            get
            {
                return _addLithologyCommand ??
                    (_addLithologyCommand = new RelayCommand(AddLithology, CanAddLithology));
            }
        }

        //reason to use this property
        //this property used as replica for global data lithology property
        //upon saving this screen, we update global lithology list with this list
        //so all the addition deltetions will be part of the original lithology list
        public ExtendedBindingList<LithologyInfo> Lithologies { get; set; }

        ObservableCollection<LithologyInfo> _lithologiesToShow;
        public ObservableCollection<LithologyInfo> LithologiesToShow
        {
            get { return _lithologiesToShow; }
            set
            {
                _lithologiesToShow = value;
                NotifyPropertyChanged("LithologiesToShow");
            }
        }

        ICommand _editCommand;
        public ICommand EditCommand
        {
            get { return _editCommand ?? (_editCommand = new RelayCommand<LithologyInfo>(Edit)); }
        }

        private void Edit(LithologyInfo info)
        {
            CurrentObject = info;
            FullImageName = CurrentObject.ImageFile;
            NotifyPropertyChanged("CurrentObject");
        }
        private void AddLithology()
        {
            if (!Tracks.Any(u => u.IsTrackSelected))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "Please select atleast one track where lithology should be added");
                return;
            }
            if (!ValidateLithologies()) return;

            // bool atleastOneValidLitholofyFoundToAdd = false;
            foreach (var track in Tracks.Where(u => u.IsTrackSelected))
            {
                var obj = LithologiesToShow.Where(u =>
                (decimal.Parse(u.InitialDepth.ToString()) <= decimal.Parse(CurrentObject.InitialDepth.ToString()) && decimal.Parse(u.FinalDepth.ToString()) >= decimal.Parse(CurrentObject.InitialDepth.ToString())) ||
                (decimal.Parse(u.InitialDepth.ToString()) >= decimal.Parse(CurrentObject.InitialDepth.ToString()) && decimal.Parse(u.FinalDepth.ToString()) <= decimal.Parse(CurrentObject.FinalDepth.ToString())) ||
                (decimal.Parse(u.InitialDepth.ToString()) >= decimal.Parse(CurrentObject.InitialDepth.ToString()) && decimal.Parse(u.InitialDepth.ToString()) <= decimal.Parse(CurrentObject.FinalDepth.ToString())) ||
                (decimal.Parse(u.FinalDepth.ToString()) <= decimal.Parse(CurrentObject.InitialDepth.ToString()) && decimal.Parse(u.FinalDepth.ToString()) >= decimal.Parse(CurrentObject.FinalDepth.ToString())));

                if (obj.Any())
                {
                    var names = obj.Select(u => u.LithologyName).ToCSV();
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                        string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("InitialAndFinalDepthAreOverlappingLithology"), names));
                    return;
                }
                else
                {
                    var displayIndex = GetDisplayIndexForNewlyAddedLithology(SelectedChart.ID, track.ID);

                    var lithologyInfo = new LithologyInfo
                    {
                        ImageFile = FullImageName,
                        IsLithologySelected = false,
                        LithologyName = CurrentObject.LithologyName.Trim(),
                        RefChart = SelectedChart.ID,
                        RefTrack = track.ID,
                        InitialDepth = CurrentObject.InitialDepth,
                        FinalDepth = CurrentObject.FinalDepth,
                        DisplayIndex = displayIndex
                    };
                    _queue.Enqueue(new InfoData<LithologyInfo>
                    {
                        InfoObject = lithologyInfo,
                        IsAdded = true
                    });
                    Lithologies.Add(lithologyInfo);
                }
            }

            CurrentObject.InitialDepth = decimal.Parse(CurrentObject.FinalDepth.ToString()) + 1;
            CurrentObject.FinalDepth = 0;
            NotifyPropertyChanged("CurrentObject");
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.SetFocus);
        }

        private int GetDisplayIndexForNewlyAddedLithology(string chartName, string trackName)
        {
            var currentList = Lithologies.Where(u => u.RefTrack == trackName && u.RefChart == chartName);
            if (currentList.Any()) return currentList.Max(u => u.DisplayIndex) + 1;

            return 1;
        }

        private bool ValidateLithologies()
        {
            if (string.IsNullOrWhiteSpace(CurrentObject.LithologyName))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("PleaseInputValidLithologyName"));
                return false;
            }
            if (string.IsNullOrWhiteSpace(FullImageName))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("PleaseSelectAppropriateImage"));
                return false;
            }

            if (decimal.Parse(CurrentObject.InitialDepth.ToString()) > decimal.Parse(CurrentObject.FinalDepth.ToString()))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("InitialDepthShouldBeSmallerThanFinalDepth"));
                return false;
            }
            return true;
        }

        public ICommand DeleteSelectedLithologiesCommand
        {
            get { return _deleteSelectedLithologiesCommand ?? (_deleteSelectedLithologiesCommand = new RelayCommand(DeleteLithologies)); }
        }

        public bool ShouldDeleteLithology { get; set; }

        private void DeleteLithologies()
        {
            ShouldDeleteLithology = false;
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.ShouldDeleteLithology);
            if (!ShouldDeleteLithology) return;

            var lithologyList = Lithologies.Where
                (u => u.IsLithologySelected && u.RefChart == CurrentObject.RefChart);// && u.RefTrack == CurrentObject.RefTrack);
            Lithologies.ListChanged -= Lithologies_ListChanged;
            foreach (var lithology in lithologyList.ToList())
            {
                _queue.Enqueue(new InfoData<LithologyInfo>
                {
                    InfoObject = lithology,
                    IsAdded = false
                });
                Lithologies.Remove(lithology);
            }

            Lithologies.ListChanged += Lithologies_ListChanged;
            FillAddedLithologies();
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

        private void UpdateCheckboxes(bool value)
        {
            Lithologies.ListChanged -= Lithologies_ListChanged;
            for (int i = 0; i < LithologiesToShow.ToList().Count; i++)
            {
                LithologiesToShow[i].IsLithologySelected = value;
            }
            Lithologies.ListChanged += Lithologies_ListChanged;
        }

        string _fullImageName;
        public string FullImageName
        {
            get { return _fullImageName; }
            set
            {
                _fullImageName = value;
                NotifyPropertyChanged("FullImageName");
            }
        }
        private bool CanAddLithology()
        {
            return !string.IsNullOrWhiteSpace(CurrentObject.LithologyName) && !string.IsNullOrWhiteSpace(CurrentObject.InitialDepth.ToString())
                && !string.IsNullOrWhiteSpace(CurrentObject.FinalDepth.ToString()) && !string.IsNullOrWhiteSpace(FullImageName);
        }

        public BitmapImage SelectedImage { get; set; }

        ICommand _selectImageCommand;
        public ICommand SelectImageCommand
        {
            get { return _selectImageCommand ?? (_selectImageCommand = new RelayCommand(SelectImage)); }
        }

        private void SelectImage()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.BrowseFiles);
        }

        Chart _selectedChart;
        public Chart SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                _selectedChart = value;
                CurrentObject.RefChart = _selectedChart.ID;
                SelectedChartChanged();
                NotifyPropertyChanged("SelectedChart");
            }
        }

        private void SelectedChartChanged()
        {
            Tracks = new ExtendedBindingList<TrackSourceForMultipleSelection>(SelectedChart.Tracks.Select(u => new TrackSourceForMultipleSelection
            {
                DisplayIndex = u.DisplayIndex,
                IsTrackSelected = false,
                RefChart = u.RefChart,
                TrackName = u.Name,
                ID = u.ID
            }).ToList());
            SelectedTrack = Tracks.First();
            Tracks.ListChanged += Tracks_ListChanged;
        }

        void Tracks_ListChanged(object sender, ListChangedEventArgs e)
        {
            FillAddedLithologies();
        }

        TrackSourceForMultipleSelection _selectedTrack;
        public TrackSourceForMultipleSelection SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                FillAddedLithologies();
                if (value == null) return;
                _selectedTrack = value;
                CurrentObject.RefTrack = _selectedTrack.Name;
                NotifyPropertyChanged("SelectedTrack");
            }
        }

        private void FillAddedLithologies()
        {
            if (LithologiesToShow == null) LithologiesToShow = new ObservableCollection<LithologyInfo>();
            LithologiesToShow.Clear();

            var annotations = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.First().Tracks.First().Annotations.Where(u => u.GetType() == typeof(BoxAnnotation));

            foreach (var track in Tracks.Where(u => u.RefChart == SelectedChart.ID && u.IsTrackSelected))
            {
                var subLithologies = Lithologies.Where(u => u.RefChart == SelectedChart.ID && u.RefTrack == track.ID);
                foreach (var lithology in subLithologies.OrderBy(u => u.RefTrack).OrderBy(u => u.InitialDepth))
                {
                    LithologiesToShow.Add(lithology);
                }
            }
        }

        public IEnumerable<Chart> Charts { get; set; }

        ExtendedBindingList<TrackSourceForMultipleSelection> _tracks;
        public ExtendedBindingList<TrackSourceForMultipleSelection> Tracks
        {
            get { return _tracks; }
            set
            {
                _tracks = value;
                NotifyPropertyChanged("Tracks");
            }
        }
    }//end class
}//end namespace
