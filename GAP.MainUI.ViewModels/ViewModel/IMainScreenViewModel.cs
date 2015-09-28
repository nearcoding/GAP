using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public interface IMainScreenViewModel
    {
        void LoadLithologies();
        void BindVisibilityRangeOfTrackToShow(TrackToShow trackToShow);
       // void DeleteCurveByDataset(Dataset dataset);
        string Token { get; set; }
        void SyncZoom();
        ViewMenuCommands ViewMenu { get; set; }
        WindowMenuCommands WindowMenu { get; set; }
        HelpMenuCommands HelpMenu { get; set; }
        GraphicMenuCommands GraphicMenu { get; set; }
        GeologyMenuCommands GeologyMenu { get; set; }
        DataMenuCommands DataMenu { get; set; }
        CalculateMenuCommands CalculateMenu { get; set; }
        FileMenuCommands FileMenu { get; set; }
        EditMenuCommands EditMenu { get; set; }
        void CalculateMinMaxVisibleRangeLimitForYAxis(IEnumerable<IRenderableSeries> renderableSeries = null);              
        Track EffectedTrack { get; set; }
        string SelectedTrack { get; set; }
        IEnumerable<string> TrackThemes { get; set; }
        string SelectedTrackTheme { get; set; }
        string ApplicationVersion { get; set; }
        ChartToShow SelectedChart { get; set; }
        void FillProjectList(List<Project> projects);
        void FillChartList(List<Chart> charts);
        bool? EnableScreenshotWithControls { get; set; }
        bool IsTooltipVisible { get; set; }
        bool IsDatasetSelected { get; set; }
        bool IsSubDatasetSelected { get; set; }
        ThemeInfo SelectedTheme { get; set; }        
        IEnumerable<ThemeInfo> Themes { get; set; }
        bool IsSyncZoom { get; set; }
        void BindToolBox();
        ICommand DrawLineCommand { get; }
        string SelectedLanguage { get; set; }
        IEnumerable<string> Languages { get; set; }
        void ShowDefaultCharts();
        ExtendedBindingList<ChartToShow> Charts { get; set; }
        void DeleteSubDataset(SubDataset subDataset);
        void DeleteDataset(Dataset datasetObject);
        void DeleteProject(string projectID);
        void DeleteWell(GAP.BL.Well wellObject);
        ICommand DropTabItem { get; }
        ICommand FileSyncZoomCommand { get; }
        bool IsProjectClosed { get; set; }
        ProjectsAndWells LoadProject(string fileName);
        ExtendedBindingList<Project> Projects { get; set; }
        void SaveProject(string fileName);
        Dataset SelectedDataset { get; set; }
        string Title { get; set; }
        SubDataset SelectedSubDataset { get; set; }
        Project SelectedProject { get; set; }
        void SettingTitleOfScreen(string fileName);
        ObservableCollection<ToolbarInfo> ToolbarItems { get; set; }
    }
}
