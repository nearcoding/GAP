using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Reflection;
using Ninject;
using MahApps.Metro;
using MahApps.Metro.Controls;
using Ninject.Parameters;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using GAP.BL;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels;
using GAP.MainUI.ViewModels.ViewModel;
using System.ComponentModel;
using MixModes.Synergy.VisualFramework.Windows;
using GAP.Custom_Controls;
using Abt.Controls.SciChart.Visuals;

namespace GAP
{
    public partial class MainWindow
    {
        #region Internal variables

        public ucTabCharts _ucTabCharts;
        public ucSolutionExplorer _solutionExplorer;
        string filepath = string.Empty;
        Task _taskProjectSaving;

        IMainScreenViewModel _dataContext;

        #endregion

        Dictionary<NotificationMessageEnum, Action> _dicReceiveMessages;
        public TabControl TabControl { get; set; }
        #region Constructors
        public MainWindow()
        {
            InitializeComponent();

            ShowCurrentCulture();

            AllowsTransparency = true;

            _ucTabCharts = new ucTabCharts();
            _solutionExplorer = new ucSolutionExplorer();

            TabControl = _ucTabCharts.TabCharts;
            GlobalData.MainWindow = this;

            SetReceiveMessages();

            var obj = Assembly.GetExecutingAssembly().GetName().Version;

            _dataContext = IoC.Kernel.Get<IMainScreenViewModel>(new ConstructorArgument("token", Token));
            DataContext = _dataContext;
            _dataContext.ApplicationVersion = string.Format("{0}.{1}.{2}", obj.Minor, obj.Build, obj.Revision);

            WindowsManagerCode();
            Loaded += MainWindow_Loaded;
        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _dataContext.ShowDefaultCharts();
            Flyouts = new FlyoutsControl();
            Flyout control = new Flyout();
            _lineEditorView = new ucLineEditorView(Token);
            _lineEditorView.DataContext = new LineEditorViewModel(Token);
            GlobalDataModel.LineEditorViewModel = _lineEditorView.DataContext as LineEditorViewModel;
            control.Content = _lineEditorView;

            control.Position = Position.Right;
            control.Width = 195;
            control.IsOpenChanged += control_IsOpenChanged;
            Flyouts.Items.Add(control);
        }

        ucLineEditorView _lineEditorView;
        ucShalePointFilterView _shalePointFilterView;

        void shalePoint_IsOpenChanged(object sender, EventArgs e)
        {

            if (!(sender as Flyout).IsOpen)
            {
                this.MainMenu.IsEnabled = true;
                this._solutionExplorer.IsEnabled = true;
            }
            else
            {
                this.MainMenu.IsEnabled = false;
                this._solutionExplorer.IsEnabled = false;
                if (_shalePointFilterView.DataContext as ShalePointFilterViewModel != null)
                    (_shalePointFilterView.DataContext as ShalePointFilterViewModel).Refresh();
            }
        }

        void control_IsOpenChanged(object sender, EventArgs e)
        {
            if (!(sender as Flyout).IsOpen)
            {
                this.MainMenu.IsEnabled = true;
                this._solutionExplorer.IsEnabled = true;
                (_lineEditorView.DataContext as LineEditorViewModel).Refresh();
            }
            else
            {
                this.MainMenu.IsEnabled = false;
                this._solutionExplorer.IsEnabled = false;
            }
        }
        DockPane pane = new DockPane();
        private void WindowsManagerCode()
        {
            pane.MinHeight = 24;
            pane.MinWidth = 100;
            pane.MaxWidth = 300;
            pane.Header = IoC.Kernel.Get<IResourceHelper>().ReadResource("ObjectNavigator");
            pane.Content = _solutionExplorer;

            WindowsManager.AddPinnedWindow(pane, Dock.Left);
            WindowsManager.DocContainerProp.Content = _ucTabCharts;
        }

        public void LoadingLanguages()
        {
            var dic = new ResourceDictionary();
            switch (_dataContext.SelectedLanguage)
            {
                case "English":
                    dic.Source = new Uri("..\\Languages\\English.xaml", UriKind.Relative);
                    break;
                case "Spanish":
                    dic.Source = new Uri("..\\Languages\\French.xaml", UriKind.Relative);
                    break;
            }
            Application.Current.Resources.MergedDictionaries.Add(dic);
        }

        #endregion

        #region Properties

        #endregion

        #region Private Methods
        private void InvalidSlope()
        {
            var result = MessageBox.Show(
                IoC.Kernel.Get<IResourceHelper>().ReadResource("InvalidSlope") + Environment.NewLine +
                IoC.Kernel.Get<IResourceHelper>().ReadResource("FixSlope") + Environment.NewLine +
                IoC.Kernel.Get<IResourceHelper>().ReadResource("RevertChanges"), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                (_lineEditorView.DataContext as LineEditorViewModel).ShouldFixSlope = true;
            }
        }
        private void SetReceiveMessages()
        {
            _dicReceiveMessages = new Dictionary<NotificationMessageEnum, Action>
           {  
                {NotificationMessageEnum.InvalidSlope,InvalidSlope},
                {NotificationMessageEnum.AddNewProject, () => new ProjectView().ShowDialog()},
                {NotificationMessageEnum.EditProject, EditProject},
                {NotificationMessageEnum.DeleteProject, DeleteProject},
                {NotificationMessageEnum.AddNewWell, () => new WellView().ShowDialog()},
                {NotificationMessageEnum.EditWell, ()=>new WellView(_dataContext.FileMenu.SelectedWell).ShowDialog()},
                {NotificationMessageEnum.DeleteWell, DeleteWell},
                {NotificationMessageEnum.AddNewDataset, () => new DatasetView(null).Show()},
                {NotificationMessageEnum.EditDataset, EditDataset},
                {NotificationMessageEnum.DeleteDataset, DeleteDataset},
                {NotificationMessageEnum.DeleteSubDataset, DeleteSubDataset},
                {NotificationMessageEnum.EditSpreadsheet, EditSpreadsheet},
                {NotificationMessageEnum.ImportFormation, () => new FormationTopImportView().ShowDialog()},
                {NotificationMessageEnum.ExportFormation, () => new FormationTopExportView().ShowDialog()},
                {NotificationMessageEnum.ImportLithologyData, () => new ImportLithologyView().ShowDialog()},
                {NotificationMessageEnum.ExportLithologyData, () => new LithologyExportDataView().ShowDialog()},
                {NotificationMessageEnum.CloseAllProjectsAndWindows, CloseAllProjectsAndWindows},
                {NotificationMessageEnum.RemoveChart, () => new RemoveChartView().ShowDialog()},
                {NotificationMessageEnum.RemoveCurveScreen, () => new RemoveCurveView().ShowDialog()},
                {NotificationMessageEnum.RemoveTrack, () => new RemoveTrackView().ShowDialog()},
                {NotificationMessageEnum.AddNewChart, () => new AddChartView().ShowDialog()},
                {NotificationMessageEnum.AddNewCurve, () => new AddCurveView().ShowDialog()},
                {NotificationMessageEnum.AddNewTrack, () => new AddTrackView().ShowDialog()},
                {NotificationMessageEnum.SaveTrackProperties, () => new TrackPropertiesView().ShowDialog()},
                {NotificationMessageEnum.SaveChartProperties, () => new ChartPropertiesView().ShowDialog()},
                {NotificationMessageEnum.LoadProject, LoadProject},
                {NotificationMessageEnum.SaveProject, SaveProject},
                {NotificationMessageEnum.SaveProjectAs, SaveProjectAs},
                {NotificationMessageEnum.ExitApplication, Application.Current.Shutdown},
                {NotificationMessageEnum.ImportDataset, () => new ImportDataView().ShowDialog()},
                {NotificationMessageEnum.ExportDataset, () => new ExportDataView().ShowDialog()},
                {NotificationMessageEnum.AddRemoveLithologyScreen, () => new AddRemoveLithologyView().ShowDialog()},
                {NotificationMessageEnum.LithologyProperties, () => new LithologyPropertiesView().ShowDialog()},
                {NotificationMessageEnum.StartProgressRing, () => ProgressRing.IsActive = true},
                {NotificationMessageEnum.StopProgressRing, () => ProgressRing.IsActive = false},
                {NotificationMessageEnum.LinePropertiesDialogScreen,()=>new LinePropertiesView().ShowDialog()},
                {NotificationMessageEnum.ShowHideObjectNavigator, ShowHideObjectNavigator},
                {NotificationMessageEnum.CustomizeToolbar, () => new CustomizeToolbarView().ShowDialog()},
                {NotificationMessageEnum.FormationTopScreen, () => new AddRemoveFormationTopView().ShowDialog()},
                {NotificationMessageEnum.NotesWindow, () => new NotesView().ShowDialog()},
                {NotificationMessageEnum.FullScreenSettings,()=>new FullScreenSettingsView().ShowDialog()},
                {NotificationMessageEnum.CurvePrinting,()=>new CurvePrintingView().ShowDialog()},
                {NotificationMessageEnum.PrintDataset,()=>new DatasetPrintingView().ShowDialog()},
                {NotificationMessageEnum.ZoomTracks,()=>new ZoomDialogView().ShowDialog()} ,
                {NotificationMessageEnum.GoToMarkScreen,()=>new GoToMarkView().ShowDialog()},
                {NotificationMessageEnum.EditSubdatasetSpreadsheet, EditSubDatasetSpreadsheet},
                {NotificationMessageEnum.EditSubdataset,()=>EditSubDataset()},
                {NotificationMessageEnum.WindowScreen,()=>new HelpWindow().ShowDialog()},
                {NotificationMessageEnum.RefreshTreeView,()=>_solutionExplorer.TreeView1.Items.Refresh()},
                {NotificationMessageEnum.MathFilter,()=>new MathFilterView().ShowDialog()},
                {NotificationMessageEnum.CalculateAddEquation, ()=>new EquationView().ShowDialog()}, 
                {NotificationMessageEnum.CalculateOverburdenGradient,()=>new OverburdenGradientView().ShowDialog()},
                {NotificationMessageEnum.CalculatePorePressure,()=>new PorePressureView().Show()},
                {NotificationMessageEnum.CalculateShalePointFilter,()=>CalculateShalePointFilter() }
            };
        }
        private void EditSubDatasetSpreadsheet()
        {
            if (_dataContext.SelectedSubDataset == null) return;
            new SubDatasetView(_dataContext.SelectedSubDataset).ShowDialog();
        }
        private void DatasetPropertiesScreen(Dataset dataset)
        {
            new DatasetView(dataset).ShowDialog();
        }

        private void CalculateShalePointFilter()
        {
            if (this.Flyouts.Items.Count == 1)
            {
                Flyout control = new Flyout
                {
                    Position = Position.Right,
                    Width = 405
                };
                control.IsOpenChanged += shalePoint_IsOpenChanged;
                _shalePointFilterView = new ucShalePointFilterView(Token);
                _shalePointFilterView.DataContext = new ShalePointFilterViewModel(Token);
                control.Content = _shalePointFilterView;
                Flyouts.Items.Add(control);
            }

            var dataContext = _shalePointFilterView.DataContext as ShalePointFilterViewModel;
            dataContext.Refresh();
            var flyoutControl = (Flyouts.Items[1] as Flyout);
            flyoutControl.IsOpen = true;
        }

        public void TakeScreenShot(SciChartSurface sciChartSurface)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog { Filter = "Image File (*.png)|*.png" };
            if (dialog.ShowDialog() != true) return;
            sciChartSurface.ExportToFile(dialog.FileName, ExportType.Png);
            MessageBox.Show("Screenshot saved successfully", GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditSubDataset()
        {
            if (_dataContext.SelectedSubDataset == null)
            {
                MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("NoSubDatasetHasBeenSelected")
                    , GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            new LinePropertiesView(_dataContext.SelectedSubDataset).ShowDialog();
        }

        #endregion

        private void ShowHideObjectNavigator()
        {
            if (_solutionExplorer.IsShowing)
            {
                _solutionExplorer.IsShowing = false;
                WindowsManager.HideWindow();
                _dataContext.ViewMenu.ShowHideObjectNavigatorText = IoC.Kernel.Get<IResourceHelper>().ReadResource("ShowObjectNavigator");
            }
            else
            {
                _solutionExplorer.IsShowing = true;
                WindowsManager.ShowWindow();
                _dataContext.ViewMenu.ShowHideObjectNavigatorText = IoC.Kernel.Get<IResourceHelper>().ReadResource("HideObjectNavigator");
            }
        }

        private void SaveProjectAs()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "GAP File (*.gap)|*.gap|All Files (*.*)|*.*"
            };

            dialog.FileName = filepath == string.Empty ? "Project.gap" : filepath;

            if (dialog.ShowDialog() != true) return;
            GlobalData.LoadedProjectPath = dialog.FileName;
            SaveProjectFileToDisk();
            GlobalData.ShouldSave = false;
        }

        private bool ClosingApplication()
        {
            var msgText = IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_SaveBeforeClosing");
            var result = MessageBox.Show(msgText, GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.No:
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.Yes:
                    SaveProject();
                    if (_taskProjectSaving != null) _taskProjectSaving.Wait();
                    Application.Current.Shutdown();
                    break;
                default:
                    return false;
            }
            return true;
        }

        #region Overridden Methods
        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            if (_dicReceiveMessages.ContainsKey(messageType.MessageType))
                _dicReceiveMessages[messageType.MessageType].Invoke();

            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.UndoRedoMessage:
                    TextBlockUndoRedoMessage.Text = messageType.MessageObject.ToString();
                    var storyBoard = Resources["test"] as Storyboard;
                    if (storyBoard != null) storyBoard.Begin();
                    break;
                case NotificationMessageEnum.ThemeChanged:
                    ThemeSelectionChanged();
                    break;
                case NotificationMessageEnum.LanguageChanged:
                    SetLanguage();
                    break;
                case NotificationMessageEnum.CancelLineEditor:
                    (this.Flyouts.Items[0] as Flyout).IsOpen = false;
                    GlobalDataModel.Instance.IsSubDatasetOpen = false;
                    break;
                case NotificationMessageEnum.CancelShalePointFilter:
                    if (this.Flyouts.Items.Count > 1)
                        (this.Flyouts.Items[1] as Flyout).IsOpen = false;
                    break;
                case NotificationMessageEnum.ShouldCancel:
                    var result = MessageBox.Show(
                        IoC.Kernel.Get<IResourceHelper>().ReadResource("ShouldCancel"),
                        GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.OK)
                    {
                        (messageType.MessageObject as LineEditorViewModel).ShouldCancel = true;
                    }
                    break;
                case NotificationMessageEnum.ShouldDeleteLine:
                    var shoulDeleteLine = MessageBox.Show(
                        IoC.Kernel.Get<IResourceHelper>().ReadResource("ShouldDeleteLine"), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (shoulDeleteLine == MessageBoxResult.OK)
                    {
                        (messageType.MessageObject as LineEditorViewModel).ShouldDeleteLine = true;
                    }
                    break;
                case NotificationMessageEnum.TakeScreenshot:
                    TakeScreenShot(messageType.MessageObject as SciChartSurface);
                    break;
                case NotificationMessageEnum.DatasetPropertiesScreen:
                    DatasetPropertiesScreen(messageType.MessageObject as Dataset);
                    break;
            }
        }

        #endregion
        private void SaveProject()
        {
            if (string.IsNullOrWhiteSpace(GlobalData.LoadedProjectPath))
                SaveProjectAs();
            else
            {
                SaveProjectFileToDisk();
                GlobalData.ShouldSave = false;
            }
        }
        private void SaveProjectFileToDisk()
        {
            ProgressRing.IsActive = true;
            _taskProjectSaving = Task.Factory.StartNew(() => _dataContext.SaveProject(GlobalData.LoadedProjectPath));

            _taskProjectSaving.ContinueWith(u =>
            {
                ProgressRing.IsActive = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private bool SaveBeforeLoad()
        {
            var msgText = IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_ProjectSave1") + " " + IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_ProjectSave2");
            var result = MessageBox.Show(msgText, GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    SaveProject();
                    break;
                case MessageBoxResult.No:
                    GlobalData.ShouldSave = false;
                    _dataContext.IsProjectClosed = true;
                    break;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return true;
        }

        private void LoadProject()
        {
            if (GlobalData.ShouldSave)
            {
                if (!SaveBeforeLoad()) return;
            }
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "GAP File (*.gap)|*.gap|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                ProgressRing.IsActive = true;
                CloseAllProjectsAndWindows();

                GlobalData.LoadedProjectPath = dialog.FileName;

                var projectsAndWells = new ProjectsAndWells();
                IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Projects.Clear();

                Task task = Task.Factory.StartNew(() =>
                {
                    projectsAndWells = _dataContext.LoadProject(GlobalData.LoadedProjectPath);
                });
                task.ContinueWith(u =>
                    {
                        if (projectsAndWells == null)
                        {
                            ProgressRing.IsActive = false;
                            return;
                        }

                        _dataContext.FillProjectList(projectsAndWells.Proj);
                        _dataContext.FillChartList(projectsAndWells.Charts);
                        ProgressRing.IsActive = false;
                        if (projectsAndWells == null) return;
                        _dataContext.GeologyMenu.IsFormationVisible = projectsAndWells.FormationTopVisible;
                         _dataContext.SelectedLanguage =
                            string.IsNullOrWhiteSpace(projectsAndWells.Language) ? "English" : projectsAndWells.Language;
                        _dataContext.SelectedTheme = new ThemeInfo
                         {
                             ThemeName = projectsAndWells.AppTheme
                         };
                        _dataContext.SettingTitleOfScreen(GlobalData.LoadedProjectPath);
                        SetLanguage();
                        ProgressRing.IsActive = false;
                        GlobalData.ShouldSave = false;
                       // _dataContext.AddLithology();
                        _dataContext.LoadLithologies();
                        _dataContext.GeologyMenu.IsLithologyVisible = projectsAndWells.LithologyVisible;
                        _dataContext.GeologyMenu.IsFullLithology = projectsAndWells.FullLithologyVisible;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void CloseAllProjectsAndWindows()
        {
            (this.Flyouts.Items[0] as Flyout).IsOpen = false;
            if (GlobalData.ShouldSave)
            {
                if (SaveBeforeLoad()) return;
                _dataContext.IsProjectClosed = false;
                return;
            }
            _solutionExplorer.DetachEvents();
            
            _dataContext.IsProjectClosed = true;
            GlobalData.LoadedProjectPath = string.Empty;
            GlobalData.ShouldSave = false;
        }

        private void ShowCurrentCulture()
        {
            if (CultureInfo.CurrentCulture.Name == "en-US") return;

            MessageBox.Show(@"The current culture of the Operative System is different to English (United States).
                In order to use the Advance setting of this application, you must go to Control Panel, 
                Region and change it to English (United States)", GlobalData.MESSAGEBOXTITLE);
            Close();
        }

        private void MainWindows_Closing(object sender, CancelEventArgs e)
        {
            if (!GlobalData.ShouldSave) return;
            if (!ClosingApplication()) e.Cancel = true;
        }

        private void DeleteDataset()
        {
            if (_dataContext.SelectedDataset == null ||
                string.IsNullOrWhiteSpace(_dataContext.SelectedDataset.Name)) return;

            var result = MessageBox.Show(string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("RemoveDatasetConfirmation"),
                _dataContext.SelectedDataset.Name), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            _dataContext.DeleteDataset(_dataContext.SelectedDataset);
            GlobalData.ShouldSave = true;
        }

        private void DeleteSubDataset()
        {
            if (_dataContext.SelectedSubDataset == null ||
                string.IsNullOrWhiteSpace(_dataContext.SelectedSubDataset.Name)) return;

            var result = MessageBox.Show(string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("ShouldDeleteSubDataset"),
                _dataContext.SelectedSubDataset.Name), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            _dataContext.DeleteSubDataset(_dataContext.SelectedSubDataset);
            GlobalData.ShouldSave = true;
        }

        private void DeleteWell()
        {
            var result = MessageBox.Show(string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("RemoveWellConfirmation"),
                _dataContext.FileMenu.SelectedWell.Name), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;
            _dataContext.DeleteWell(_dataContext.FileMenu.SelectedWell);
            GlobalData.ShouldSave = true;
        }

        private void DeleteProject()
        {
            if (_dataContext.SelectedProject == null ||
                string.IsNullOrWhiteSpace(_dataContext.SelectedProject.Name)) return;

            var result = MessageBox.Show(string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("RemoveProjectConfirmation"),
                _dataContext.SelectedProject.Name), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            _dataContext.DeleteProject(_dataContext.SelectedProject.ID);
            GlobalData.ShouldSave = true;
        }

        private void ThemeSelectionChanged()
        {
            if (_dataContext == null) return;
            string selectedTheme = _dataContext.SelectedTheme.ThemeName;
            var accent = MahApps.Metro.ThemeManager.Accents.First(u => u.Name == selectedTheme);

            MahApps.Metro.ThemeManager.ChangeAppStyle(Application.Current, accent, MahApps.Metro.ThemeManager.AppThemes.First() as AppTheme);
            _dataContext.SelectedTheme = _dataContext.Themes.SingleOrDefault(u => u.ThemeName == selectedTheme);

        }

        private void SetLanguage()
        {
            if (_dataContext == null) return;
            LoadingLanguages();
            //_dataContext.SelectedLanguage = GlobalDataModel.Language;
            pane.Header = IoC.Kernel.Get<IResourceHelper>().ReadResource("ObjectNavigator");
            _dataContext.BindToolBox();
        }
        private void mnuFullScreen_Click(object sender, RoutedEventArgs e)
        {
            var hideObjectNavigator = (bool)Properties.Settings.Default["HideObjectNavigator"];
            var hideToolbar = (bool)Properties.Settings.Default["HideToolbar"];
            var hideStatusbar = (bool)Properties.Settings.Default["HideStatusbar"];
            if (ResizeMode == ResizeMode.CanResizeWithGrip)
            {
                ResizeMode = ResizeMode.NoResize;
             //   IgnoreTaskbarOnMaximize = true;
                if (hideStatusbar) StatusBar1.Visibility = System.Windows.Visibility.Collapsed;
                mnuFullScreen.Header = "Exit Full Screen";
            }
            else
            {
                ResizeMode = ResizeMode.CanResizeWithGrip;
             //   IgnoreTaskbarOnMaximize = false;
                mnuFullScreen.Header = "Full Screen";
                StatusBar1.Visibility = System.Windows.Visibility.Visible;
            }
            if (hideObjectNavigator) _dataContext.ViewMenu.ViewShowHideObjectNavigatorCommand.Execute(null);
            if (hideToolbar) _dataContext.ViewMenu.ViewShowHideToolbarCommand.Execute(null);

            Visibility = System.Windows.Visibility.Collapsed;
            InvalidateVisual();
            Visibility = System.Windows.Visibility.Visible;
        }

    }//end class
}//end namespace