using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Ninject;
using AutoMapper;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart;
using System.Windows.Data;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Numerics;
using GAP.BL.HelperClasses;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public partial class MainScreenViewModel : BaseViewModel<BaseEntity>, IMainScreenViewModel
    {
        public ViewMenuCommands ViewMenu { get; set; }
        public HelpMenuCommands HelpMenu { get; set; }
        public GeologyMenuCommands GeologyMenu { get; set; }
        public DataMenuCommands DataMenu { get; set; }
        public CalculateMenuCommands CalculateMenu { get; set; }
        public EditMenuCommands EditMenu { get; set; }
        public FileMenuCommands FileMenu { get; set; }
        public GraphicMenuCommands GraphicMenu { get; set; }

        public WindowMenuCommands WindowMenu { get; set; }
        public ExtendedBindingList<ChartToShow> Charts { get; set; }
        public MainScreenViewModel(string token)
            : base(token)
        {
            Charts = new ExtendedBindingList<ChartToShow>();
            InitializeChartThemes();
            Task task = Task.Factory.StartNew(() =>
                {
                    Title = IoC.Kernel.Get<IResourceHelper>().ReadResource("GAP");
                    IoC.Kernel.Get<IGlobalDataModel>().MainViewModel = this;
                    EnableScreenshotWithControls = false;
                    IsTooltipVisible = true;
                    IsSyncZoom = true;
                    EditMenu = new EditMenuCommands(Token);
                    FileMenu = new FileMenuCommands(Token);
                    CalculateMenu = new CalculateMenuCommands(Token);
                    DataMenu = new DataMenuCommands(Token);
                    GeologyMenu = new GeologyMenuCommands(Token);
                    GraphicMenu = new GraphicMenuCommands(Token);
                    HelpMenu = new HelpMenuCommands(Token);
                    ViewMenu = new ViewMenuCommands(Token);
                    WindowMenu = new WindowMenuCommands(Token);
                    GeologyMenu.IsLithologyVisible = false;
                    GeologyMenu.IsFormationVisible = false;
                });

            task.ContinueWith(u =>
                {
                    //any change in global chart list notifies us
                    Projects = GlobalCollection.Instance.Projects;
                    ViewMenu.Opacity = 1;
                    ViewMenu.IsToolBarVisible = true;
                    GeologyMenu.IsFTNameVisible = true;
                    InitializeThemes();
                    Languages = new[] { "English", "Spanish" };
                    SelectedTheme = Themes.Single(theme => theme.ThemeName == "Blue");
                    SelectedLanguage = Languages.First();
                    GeologyMenu.LithologyX1 = 0.9;
                }, TaskScheduler.FromCurrentSynchronizationContext());
            IsTrackControlsVisible = true;
        }

        public void LoadLithologies()
        {
            var tracks = SelectedChart.Tracks;

            foreach (var track in tracks.Where(u => u.TrackObject.Lithologies.Any()))
            {
                LithologyManager.Instance.AddLithologiesToTrack(track);
            }
            CalculateMinMaxVisibleRangeLimitForYAxis();
        }

        #region Basic Properties

        string _searchableText;
        public string SearchableText
        {
            get { return _searchableText; }
            set
            {
                _searchableText = value;
                NotifyPropertyChanged("SearchableText");
            }
        }

        private void SearchText(string searchText)
        {
            ListOfItems = GlobalSearch.Instance.ExecuteQuery(searchText);
        }

        IEnumerable<HelpData> _listOfItems;
        public IEnumerable<HelpData> ListOfItems
        {
            get { return _listOfItems; }
            set
            {
                _listOfItems = value;
                NotifyPropertyChanged("ListOfItems");
            }
        }

        public bool IsSubDatasetSelected { get; set; }

        public bool IsDatasetSelected { get; set; }

        ExtendedBindingList<Project> _projects;
        public ExtendedBindingList<Project> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                NotifyPropertyChanged("Projects");
            }
        }

        bool _isSyncZoom;
        public bool IsSyncZoom
        {
            get { return _isSyncZoom; }
            set
            {
                _isSyncZoom = value;
                if (SelectedChart != null) SyncZoom();
                NotifyPropertyChanged("IsSyncZoom");
            }
        }

        public void SyncZoom()
        {
            if (IsSyncZoom)
            {
                CalculateMinMaxVisibleRangeLimitForYAxis();

                //we have to perform zoom extents y here
                //set the mouse event group and bind the visibility
                foreach (var trackToShow in SelectedChart.Tracks)
                {
                    // MouseManager.SetMouseEventGroup(trackToShow.ChartModifier, SelectedChart.ChartObject.ChartName);
                    BindVisibilityRangeOfTrackToShow(trackToShow);
                }
            }
            else
            {
                foreach (var trackToShow in SelectedChart.Tracks)
                {
                    //MouseManager.SetMouseEventGroup(trackToShow.ChartModifier, null);
                    var visibleRange = trackToShow.YAxis.VisibleRange;
                    var visibleRangeLimit = trackToShow.YAxis.VisibleRangeLimit;
                    BindingOperations.ClearBinding(trackToShow.YAxis, NumericAxis.VisibleRangeProperty);
                    BindingOperations.ClearBinding(trackToShow.YAxis, NumericAxis.VisibleRangeLimitProperty);
                    trackToShow.YAxis.VisibleRange = visibleRange;
                    trackToShow.YAxis.VisibleRangeLimit = visibleRangeLimit;
                }
                //set the mouse event group to null and clear the visibility
            }
        }

        public void BindVisibilityRangeOfTrackToShow(TrackToShow trackToShow)
        {
            BindingOperations.SetBinding(trackToShow.YAxis, NumericAxis.VisibleRangeProperty, new Binding("VisibleRangeYAxis")
            {
                Source = SelectedChart,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            BindingOperations.SetBinding(trackToShow.YAxis, NumericAxis.VisibleRangeLimitProperty, new Binding("VisibleRangeLimitYAxis")
            {
                Source = SelectedChart,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }

        ICommand _tabDropCommand;

        private void TabDrop(System.Windows.DragEventArgs e)
        {
            if (e == null) return;
            TextBlock tabItemTarget = e.OriginalSource as TextBlock;

            if (tabItemTarget == null) return;

            var tabItemSource = e.Data.GetData(typeof(string));
            if (tabItemSource == null) return;
            if (!tabItemTarget.Text.Equals(tabItemSource))
            {
                TabControl control = e.Source as TabControl;

                var sourceChart = Charts.SingleOrDefault(u => u.ChartObject.Name == tabItemSource.ToString());
                var sourceIndex = sourceChart.ChartObject.DisplayIndex;
                var targetChart = Charts.SingleOrDefault(u => u.ChartObject.Name == tabItemTarget.Text);
                if (targetChart != null)
                {
                    var targetIndex = targetChart.ChartObject.DisplayIndex;
                    var sourceItem = Charts.SingleOrDefault(u => u.ChartObject.DisplayIndex == sourceIndex);
                    Charts.Remove(sourceItem);

                    if (targetIndex > Charts.Count - 1)
                        Charts.Insert(Charts.Count - 1, sourceItem);
                    else
                        Charts.Insert(targetIndex, sourceItem);

                    UpdateDisplayIndex();
                }
            }
        }

        private void RemoveReferencesFromSelectedChart()
        {
            foreach (var track in SelectedChart.Tracks)
            {
                if (track.Annotations != null)
                {
                    track.Annotations.Clear();
                }
                if (track.TrackObject.Curves != null)
                    track.TrackObject.Curves.Clear();
            }
            SelectedChart.Tracks.Clear();
        }

        public void ShowDefaultCharts()
        {
            if (SelectedChart != null)
            {
                if (SelectedChart.Tracks != null) RemoveReferencesFromSelectedChart();
                SelectedChart = null;
            }

            DefaultChartCollection.Instance.InitializeDefaultCharts();
            UndoRedoObject.GlobalUndoStack.Clear();
        }

        ChartToShow _selectedChart;
        public ChartToShow SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                _selectedChart = value;
                if (value == null) return;
                if (_selectedChart.Tracks.Any()) _selectedChart.Tracks.Clear();
                ChartManager.Instance.SelectedChartChanged();
                NotifyPropertyChanged("SelectedChart");
            }
        }

        ICommand _addTabCommand;
        public ICommand AddTabCommand
        {
            get { return _addTabCommand ?? (_addTabCommand = new RelayCommand(AddChart)); }
        }

        private void AddChart()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.AddNewChart);
        }

        ICommand _addTrackCommand;
        public ICommand AddTrackCommand
        {
            get { return _addTrackCommand ?? (_addTrackCommand = new RelayCommand(AddTrack)); }
        }

        private void AddTrack()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.AddNewTrack);
        }
        public void BindToolBox()
        {
            var lst = (List<ToolbarInfo>)Mapper.Map(GlobalDataModel.AvailableToolbarItems, typeof(List<ToolbarInfo>), typeof(List<ToolbarInfo>));
            var lstObjects = new ObservableCollection<ToolbarInfo>(lst);

            var itemsInToolbox = GlobalSerializer.DeserializeToolbar();
            if (itemsInToolbox == null)
            {
                WindowMenu.WindowsResetToolbar();
                return;
            }

            ToolbarItems = new ObservableCollection<ToolbarInfo>(itemsInToolbox);

            ViewMenu.TransparentText = IoC.Kernel.Get<IResourceHelper>().ReadResource("MakeTransparent");
            ViewMenu.ShowHideObjectNavigatorText = IoC.Kernel.Get<IResourceHelper>().ReadResource("HideObjectNavigator");
        }

        ICommand _fileSyncZoomCommand;
        public ICommand FileSyncZoomCommand
        {
            get { return _fileSyncZoomCommand ?? (_fileSyncZoomCommand = new RelayCommand(FileSyncZoom)); }
        }

        private void FileSyncZoom()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.SyncZoom);
        }

        ObservableCollection<ToolbarInfo> _toolBarItems;
        public ObservableCollection<ToolbarInfo> ToolbarItems
        {
            get { return _toolBarItems; }
            set
            {
                _toolBarItems = value;
                NotifyPropertyChanged("ToolbarItems");
            }
        }

        private void ReadToolboxFromFile()
        {
            var lst = GlobalSerializer.DeserializeToolbar();
            if (lst != null)
                ToolbarItems = new ObservableCollection<ToolbarInfo>(lst);
        }

        bool _isProjectClosed;
        public bool IsProjectClosed
        {
            get { return _isProjectClosed; }
            set
            {
                _isProjectClosed = value;
            }
        }

        string _selectedTrackTheme;
        public string SelectedTrackTheme
        {
            get { return _selectedTrackTheme; }
            set
            {
                _selectedTrackTheme = value;
                NotifyPropertyChanged("SelectedTrackTheme");
            }
        }

        public IEnumerable<string> TrackThemes { get; set; }
        public void InitializeChartThemes()
        {
            TrackThemes = new string[] { "BlackSteel", "BrightSpark", "Chrome", "Electric", "ExpressionDark", "ExpressionLight", "Oscilloscope" };
            SelectedTrackTheme = "BrightSpark";
            NotifyPropertyChanged("ChartThemes");
        }

        public string ApplicationVersion { get; set; }

        bool _isTrackControlsVisible;

        ICommand _drawLineCommand;
        public ICommand DrawLineCommand
        {
            get { return _drawLineCommand ?? (_drawLineCommand = new RelayCommand(DrawLine, () => CanDrawLine())); }
        }

        private void DrawLine()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.LinePropertiesDialogScreen);
        }

        private bool CanDrawLine()
        {
            return HelperMethods.Instance.AnyCurveExists();
        }

        bool? _enableScreenshotWithControls;
        public bool? EnableScreenshotWithControls
        {
            get { return _enableScreenshotWithControls; }
            set
            {
                _enableScreenshotWithControls = value;
            }
        }


        bool _isTooltipVisible;

        public bool IsTooltipVisible
        {
            get { return _isTooltipVisible; }
            set
            {
                if (value == _isTooltipVisible) return;
                _isTooltipVisible = value;
                NotifyPropertyChanged("IsTooltipVisible");
            }
        }

        string _selectedLanguage;
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                if (_selectedLanguage == value) return;
                _selectedLanguage = value;
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.LanguageChanged);
                NotifyPropertyChanged("SelectedLanguage");
            }
        }
        IEnumerable<ThemeInfo> _lstThemes;
        IEnumerable<string> _lstLanguages;
        public IEnumerable<ThemeInfo> Themes
        {
            get { return _lstThemes; }
            set
            {
                _lstThemes = value;
                NotifyPropertyChanged("Themes");
            }
        }

        public IEnumerable<string> Languages
        {
            get { return _lstLanguages; }
            set
            {
                _lstLanguages = value;
                NotifyPropertyChanged("Languages");
            }
        }

        private void InitializeThemes()
        {
            Themes = new[]
            {
                new ThemeInfo
                {
                    ThemeName = "Amber",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(243, 181, 59))
                },
                new ThemeInfo
                {
                    ThemeName = "Blue",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(65, 117, 225))
                },
                new ThemeInfo
                {
                    ThemeName = "Brown",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(155, 123, 86))
                },
                new ThemeInfo
                {
                    ThemeName = "Cobalt",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(51, 115, 242))
                },
                new ThemeInfo
                {
                    ThemeName = "Crimson",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(181, 51, 81))
                },
                new ThemeInfo
                {
                    ThemeName = "Cyan",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(73, 180, 232))
                },
                new ThemeInfo
                {
                    ThemeName = "Emerald",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(51, 161, 51))
                },
                new ThemeInfo
                {
                    ThemeName = "Green",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(128, 186, 69))
                },
                new ThemeInfo
                {
                    ThemeName = "Indigo",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(136, 51, 255))
                },
                new ThemeInfo
                {
                    ThemeName = "Lime",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(182, 208, 51))
                },
                new ThemeInfo
                {
                    ThemeName = "Magenta",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(224, 51, 143))
                },
                new ThemeInfo
                {
                    ThemeName = "Mauve",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(145, 128, 161))
                },
                new ThemeInfo
                {
                    ThemeName = "Olive",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(138, 159, 131))
                },
                new ThemeInfo
                {
                    ThemeName = "Orange",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(251, 134, 51))
                },
                new ThemeInfo
                {
                    ThemeName = "Pink",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(246, 142, 217))
                },
                new ThemeInfo
                {
                    ThemeName = "Purple",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(131, 122, 229))
                },
                new ThemeInfo
                {
                    ThemeName = "Red",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(234, 67, 51))
                },
                new ThemeInfo
                {
                    ThemeName = "Sienna",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(179, 117, 87))
                },
                new ThemeInfo
                {
                    ThemeName = "Steel",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(131, 145, 159))
                },
                new ThemeInfo
                {
                    ThemeName = "Violet",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(187, 51, 255))
                },
                new ThemeInfo
                {
                    ThemeName = "Yellow",
                    ThemeColor = new SolidColorBrush(Color.FromRgb(254, 229, 56))
                }
            };
        }

        ThemeInfo _selectedTheme;
        public ThemeInfo SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                if (_selectedTheme == value) return;
                _selectedTheme = value;
                ThemeSelectionChanged();
                NotifyPropertyChanged("SelectedTheme");
            }
        }

        private void ThemeSelectionChanged()
        {
            if (SelectedTheme == null) return;
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.ThemeChanged);
        }
        #endregion

        public void CalculateMinMaxVisibleRangeLimitForYAxis(IEnumerable<IRenderableSeries> renderableSeries = null)
        {
            if (SelectedChart == null) return;
            bool hasValue = false;
            double _minValueYAxis = 0, _maxValueYAxis = 10;

            foreach (var trackToShow in SelectedChart.Tracks)
            {
                if (renderableSeries == null)
                    renderableSeries = GetRenderableSeriesToCalculateVisibileRange(trackToShow);
                foreach (var curve in renderableSeries)
                {
                    if ((curve.DataSeries as XyDataSeries<double, double>).Count == 0) continue;
                    if (hasValue)
                    {
                        double tempMinValue, tempMaxValue;
                        double.TryParse(curve.DataSeries.YMin.ToString(), out tempMinValue);
                        double.TryParse(curve.DataSeries.YMax.ToString(), out tempMaxValue);

                        if (tempMinValue < _minValueYAxis) _minValueYAxis = tempMinValue;
                        if (tempMaxValue > _maxValueYAxis) _maxValueYAxis = tempMaxValue;
                    }
                    else
                    {
                        hasValue = true;
                        double.TryParse(curve.DataSeries.YMin.ToString(), out _minValueYAxis);
                        double.TryParse(curve.DataSeries.YMax.ToString(), out _maxValueYAxis);
                    }
                }
            }

            if (_minValueYAxis == _maxValueYAxis)
                _minValueYAxis = _minValueYAxis - 10;

            if (GeologyMenu.IsFormationVisible && SelectedChart != null && SelectedChart.ChartObject != null && SelectedChart.ChartObject.Formations.Any())
            {
                var valueToUse = (_maxValueYAxis - _minValueYAxis) / 20;
                if (double.Parse(SelectedChart.ChartObject.Formations.First().Depth.ToString()) < _minValueYAxis + valueToUse)
                    _minValueYAxis = double.Parse(SelectedChart.ChartObject.Formations.First().Depth.ToString()) - valueToUse;

                if (double.Parse(SelectedChart.ChartObject.Formations.Last().Depth.ToString()) > _maxValueYAxis - valueToUse)
                    _maxValueYAxis = double.Parse(SelectedChart.ChartObject.Formations.Last().Depth.ToString()) + valueToUse;
            }

            SelectedChart.VisibleRangeYAxis = new DoubleRange(_minValueYAxis, _maxValueYAxis);
            SelectedChart.VisibleRangeLimitYAxis = new DoubleRange(_minValueYAxis, _maxValueYAxis);
        }

        private IEnumerable<IRenderableSeries> GetRenderableSeriesToCalculateVisibileRange(TrackToShow trackToShow)
        {
            //lithology & formation series also need to be part of visible range calculatation
            var chartToShow = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.SingleOrDefault
                (u => u.ChartObject.ID == trackToShow.TrackObject.RefChart);

            IEnumerable<IRenderableSeries> renderableSeries;
            if ((trackToShow.TrackObject.Lithologies.Any() && (GeologyMenu.IsLithologyVisible || GeologyMenu.IsFullLithology)) &&
                (chartToShow.ChartObject.Formations.Any() && GeologyMenu.IsFormationVisible))
            {
                renderableSeries = trackToShow.CurveRenderableSeries.Where(u =>
                  (u as FastLineRenderableSeries).Name != "DefaultAxis"
                    && (u as FastLineRenderableSeries).Name != "Lithology");
            }
            else if (trackToShow.TrackObject.Lithologies.Any() && (GeologyMenu.IsLithologyVisible || GeologyMenu.IsFullLithology) &&
                (!chartToShow.ChartObject.Formations.Any() || !GeologyMenu.IsFormationVisible))
            {
                renderableSeries = trackToShow.CurveRenderableSeries.Where(u =>
                  (u as FastLineRenderableSeries).Name != "DefaultAxis"
                   && (u as FastLineRenderableSeries).Name != "Formation" 
                   && (u as FastLineRenderableSeries).Name != "Lithology");
            }
            else if (!trackToShow.TrackObject.Lithologies.Any() || (!GeologyMenu.IsLithologyVisible || !GeologyMenu.IsFullLithology) &&
                (chartToShow.ChartObject.Formations.Any() && GeologyMenu.IsFormationVisible))
            {
                renderableSeries = trackToShow.CurveRenderableSeries.Where(u =>
                  (u as FastLineRenderableSeries).Name != "DefaultAxis"
                && (u as FastLineRenderableSeries).XAxisId != "Lithology"
                 && (u as FastLineRenderableSeries).Name != "Lithology");
            }
            else
            {
                //ignore lithology data in case there is no lithology or lithology is not being shown
                renderableSeries = trackToShow.CurveRenderableSeries.Where(u =>
                 (u as FastLineRenderableSeries).Name != "Lithology"
                && (u as FastLineRenderableSeries).Name != "DefaultAxis"
                  && (u as FastLineRenderableSeries).XAxisId != "Lithology"
                 && (u as FastLineRenderableSeries).Name != "Formation");
            }
            return renderableSeries;
        }

        public string SelectedTrack { get; set; }

        public Track EffectedTrack { get; set; }

        private void UpdateDisplayIndex()
        {
            int i = 0;
            foreach (var item in Charts)
            {
                item.ChartObject.DisplayIndex = i;
                i += 1;
            }
        }

        public Project SelectedProject { get; set; }

        public Dataset SelectedDataset { get; set; }

        public SubDataset SelectedSubDataset { get; set; }

        public ICommand DropTabItem
        {
            get { return _tabDropCommand ?? (_tabDropCommand = new RelayCommand<System.Windows.DragEventArgs>(TabDrop)); }
        }

        public ProjectsAndWells LoadProject(string fileName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(fileName), "File name must be passed before loading it");

            ProjectsAndWells projectsAndWells = DeserializeObject(fileName);

            if (projectsAndWells == null)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, "Unable to load project file");
                return null;
            }
            // FillProjectList(projectsAndWells.Proj);
            SettingTitleOfScreen(fileName);
            return projectsAndWells;
        }

        public void FillProjectList(List<Project> projects)
        {
            Projects.Clear();
            foreach (var project in projects)
                GlobalCollection.Instance.Projects.Add(project);
        }

        public void SettingTitleOfScreen(string fileName)
        {
            string appTitle = IoC.Kernel.Get<IResourceHelper>().ReadResource("GAP");
            Title = string.Format("{0} - {1}", appTitle, fileName);
            NotifyPropertyChanged("Title");
        }

        public void FillChartList(List<Chart> charts)
        {
            //if we run the loop on main collection then selected chart changed all the time and code gets executed over and over
            var lst = GlobalCollection.Instance.Charts.Where(u => u.Name != SelectedChart.ChartObject.Name);

            foreach (var element in lst.ToList())
                ChartManager.Instance.RemoveChartObject(element);

            //remove the  selected chart
            if (SelectedChart != null && SelectedChart.ChartObject != null)
                ChartManager.Instance.RemoveChartObject(SelectedChart.ChartObject);

            SelectedChart = null;

            foreach (var chart in charts.OrderBy(u => u.DisplayIndex))
                ChartManager.Instance.AddChartObject(chart);

            UndoRedoObject.GlobalUndoStack.Clear();
            UndoRedoObject.GlobalRedoStack.Clear();
        }

        private static ProjectsAndWells DeserializeObject(string fileName)
        {
            ProjectsAndWells projectsAndWells = new ProjectsAndWells();
            return GlobalSerializer.DeSerializeObject(fileName, projectsAndWells);
        }

        private void FixDisplayIndexOfList<T>(IEnumerable<T> lst)
            where T : BaseEntity
        {
            int i = 0;
            foreach (var item in lst.OrderBy(u => u.DisplayIndex).ToList())
            {
                item.DisplayIndex = i;
                if (item.GetType() == typeof(Project))
                {
                    var project = item as Project;
                    if (project.Wells.Any()) FixDisplayIndexOfList(project.Wells);
                }
                else if (item.GetType() == typeof(Well))
                {
                    var well = item as Well;
                    if (well.Datasets.Any()) FixDisplayIndexOfList(well.Datasets);
                }
                i += 1;
            }
        }

        public void SaveProject(string fileName)
        {
            FixDisplayIndexOfList(GlobalCollection.Instance.Projects);
            var paw = new ProjectsAndWells
            {
                Proj = GlobalCollection.Instance.Projects.ToList(),
                Charts = GlobalCollection.Instance.Charts.ToList(),
                AppTheme = SelectedTheme.ThemeName,
                Language = SelectedLanguage,
                FormationTopVisible = GeologyMenu.IsFormationVisible,
                LithologyVisible = GeologyMenu.IsLithologyVisible,
                FullLithologyVisible = GeologyMenu.IsFullLithology
            };

            GlobalSerializer.SerializeObject<ProjectsAndWells>(paw, fileName);
            string appName = IoC.Kernel.Get<IResourceHelper>().ReadResource("GAP");
            Title = string.Format("{0} - {1}", appName, fileName);
            NotifyPropertyChanged("Title");
        }

        public void DeleteProject(string projectID)
        {
            var project = GlobalCollection.Instance.Projects.SingleOrDefault(u => u.ID == projectID);
            if (project != null && project.Wells != null)
            {
                if (project.Wells.Any())
                {
                    foreach (var well in project.Wells.ToList())
                    {
                        DeleteDatasetsByWell(well);
                    }
                }
                GlobalCollection.Instance.Projects.Remove(project);
            }
        }

        public void DeleteWell(Well wellObject)
        {
            var projectObject = GlobalCollection.Instance.Projects.Single(u => u.ID == wellObject.RefProject);
            var well = projectObject.Wells.SingleOrDefault(u => u.Name == wellObject.Name);
            if (well != null)
            {
                DeleteDatasetsByWell(well);
                projectObject.Wells.Remove(wellObject);
            }
        }

        private void DeleteDatasetsByWell(Well well)
        {
            if (well.Datasets.Any())
            {
                foreach (var dataset in well.Datasets.ToList())
                {
                    DatasetManager.Instance.RemoveDatasetObject(dataset);
                }
            }
        }

        public void DeleteDataset(Dataset datasetObject)
        {
            var dataset = HelperMethods.Instance.GetDatasetByID(datasetObject.ID);
            if (dataset != null)
            {
                DatasetManager.Instance.RemoveDatasetObject(dataset);
            }
        }

        public void DeleteSubDataset(SubDataset subDatasetObject)
        {
            var subDataset = HelperMethods.Instance.GetSubDatasetObjectBySubdatasetDetails(subDatasetObject);

            if (subDataset != null)
            {
                var dataset = HelperMethods.Instance.GetDatasetByID(subDataset.Dataset);
                TrackManager.Instance.RemoveAllAnnotationsBySubDataset(subDataset);
                dataset.SubDatasets.Remove(subDataset);
            }
        }

        public bool? IsTrackControlsVisible
        {
            get { return _isTrackControlsVisible; }
            set
            {
                if (_isTrackControlsVisible == value) return;
                _isTrackControlsVisible = value.Value;
                NotifyPropertyChanged("IsTrackControlsVisible");
            }
        }

    }//end class
}//end namespace
