using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Ninject;
using GalaSoft.MvvmLight.Messaging;
using GAP.Helpers;
using Excel;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using Abt.Controls.SciChart.Visuals;
using System.Linq;
using System.Windows.Media;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.PointMarkers;
using System.Windows.Markup;
using System.Windows.Controls;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Visuals.Axes;
using System.Windows.Data;
using System.Collections;
using System.Text.RegularExpressions;
using AutoMapper;
using Abt.Controls.SciChart.Rendering.Common;
using System.Windows;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class GlobalDataModel : IGlobalDataModel
    {
        List<ChartToShow> _chartToShowObjects;
        public List<ChartToShow> ChartToShowObjects
        {
            get
            {
                if (_chartToShowObjects == null) _chartToShowObjects = new List<ChartToShow>();
                return _chartToShowObjects;
            }
            set
            {
                _chartToShowObjects = value;
            }
        }

        public ImportDataType ImportDataType { get; set; }
        public bool IsSubDatasetOpen { get; set; }
        public static LineEditorViewModel LineEditorViewModel { get; set; }
        public string GetAppDataFolder()
        {
            string folderName = string.Empty;
            try
            {
                //folderName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                //folderName = folderName + "\\GAP";
                //if (!Directory.Exists(folderName))
                //    Directory.CreateDirectory(folderName);
                return System.Environment.CurrentDirectory;

            }
            catch (Exception ex)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(
                    IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Token, ex.ToString());
            }
            return folderName;
        }

        public void StylingOfScaleControl(Dataset dataset, CurveToShow curveToShow)
        {
            double minValueXAxis, maxValueXAxis;

            minValueXAxis = GlobalDataModel.GetValidMinUnitValue(dataset);
            maxValueXAxis = Convert.ToDouble(dataset.MaxUnitValue);

            curveToShow.MinValue = decimal.Parse(minValueXAxis.ToString());
            curveToShow.MaxValue = decimal.Parse(maxValueXAxis.ToString());

            UpdateVisibleRangeForXAxis(curveToShow);
        }

        private void UpdateVisibleRangeForXAxis(CurveToShow curveToShow)
        {
            var trackToShow = curveToShow.TrackToShowObject;
            var xAxis = trackToShow.XAxisCollection.SingleOrDefault(u => u.Id == curveToShow.CurveObject.ID);
            if (xAxis == null) return;

            xAxis.VisibleRange = new DoubleRange(double.Parse(curveToShow.MinValue.ToString()), double.Parse(curveToShow.MaxValue.ToString()));
            xAxis.VisibleRangeLimit = new DoubleRange(double.Parse(curveToShow.MinValue.ToString()), double.Parse(curveToShow.MaxValue.ToString()));
        }

        public void CheckForHasCurves(TrackToShow trackToShow)
        {
            if (MainViewModel.GeologyMenu.IsLithologyVisible || MainViewModel.GeologyMenu.IsFullLithology)
            {
                if (trackToShow.TrackObject.Lithologies.Any())
                {
                    trackToShow.HasCurves = trackToShow.CurveRenderableSeries.Any
                        (u => u.XAxisId != "DefaultAxisID" && u.XAxisId != "Formation");
                    return;
                }
            }
            trackToShow.HasCurves = trackToShow.CurveRenderableSeries.Any
                (u => u.XAxisId != "Lithology" && u.XAxisId != "DefaultAxisID" && u.XAxisId != "Formation");
        }

        public List<DepthCurveInfo> GetValidListOfDepthAndCurves(IEnumerable<DepthCurveInfo> depthAndCurves)
        {
            return depthAndCurves.Except(depthAndCurves.Where(u => u.Curve <= -999 && u.Curve >= (decimal)-999.99)).ToList();
        }

        public string GetNotesFirstAndLastValidData(List<DepthCurveInfo> lst)
        {
            if (!lst.Any()) return string.Empty;
            string notes = string.Format("Starting Depth : {0}", lst.First().Depth);
            notes = notes + Environment.NewLine + string.Format("Ending Depth : {0}", lst.Last().Depth);
            return notes;
        }

        public FastLineRenderableSeries AddDataseriesInformationToCurve(Dataset dataset, CurveToShow curveToShow)
        {
            var lstToBeUsed = dataset.DepthAndCurves.Except(dataset.DepthAndCurves.Where(u => u.Curve <= -999 && u.Curve >= (decimal)-999.99)).ToList();
            var dataSeries = new XyDataSeries<double, double>();
            dataSeries.AcceptsUnsortedData = true;
            FastLineRenderableSeries fastLineSeries;

            if (curveToShow.FastLineRenderableSeries != null)
                fastLineSeries = curveToShow.FastLineRenderableSeries;
            else
            {
                fastLineSeries = new FastLineRenderableSeries();
                curveToShow.TrackToShowObject.CurveRenderableSeries.Add(fastLineSeries);
            }

            lstToBeUsed.ForEach(u => dataSeries.Append((double)u.Curve, (double)u.Depth));

            curveToShow.FastLineRenderableSeries = fastLineSeries;
            fastLineSeries.DataSeries = dataSeries;

            return fastLineSeries;
        }

        public static double GetValidMinUnitValue(Dataset dataset)
        {
            decimal minUnitValue;
            if (dataset.MinUnitValue >= (decimal)-999.999 && dataset.MinUnitValue <= -999)
                minUnitValue = dataset.DepthAndCurves.Except(dataset.DepthAndCurves.Where(u => u.Curve >= ((decimal)-999.99) && u.Curve <= -999)).Min(u => u.Curve);
            else
                minUnitValue = dataset.MinUnitValue;
            return double.Parse(minUnitValue.ToString());
        }

        public static CurveToShow GetCurveToShowByCurve(Curve curve)
        {
            var allTracks = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Where(u => u.Tracks != null).SelectMany(v => v.Tracks);
            if (allTracks == null) return null;
            var allCurvesToShow = allTracks.Where(w => w.Curves != null).SelectMany(x => x.Curves);
            if (allCurvesToShow == null) return null;
            return allCurvesToShow.SingleOrDefault(u => u.RefChart == curve.RefChart && u.RefTrack == curve.RefTrack && u.RefDataset == curve.RefDataset);
        }
        public static IEnumerable<CurveToShow> GetCurveToShowBySubdataset(SubDataset subDataset)
        {
            var tracks = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Where(u => u.Tracks != null).SelectMany(v => v.Tracks);
            if (tracks == null) return null;
            var curves = tracks.Where(u => u.Curves != null).SelectMany(v => v.Curves);
            if (curves == null) return null;
            var curvesWithSubDataset = curves.Where(u => u.SubDatasets.Any());
            var lst = new List<CurveToShow>();
            foreach (var curveToShow in curvesWithSubDataset)
            {
                if (curveToShow.SubDatasets.Any(u => u.Project == subDataset.Project && u.Well == subDataset.Well && u.Dataset == subDataset.Dataset && u.Name == subDataset.Name))
                {
                    lst.Add(curveToShow);
                }
            }
            return lst;
        }

        private GlobalDataModel()
        {
            Mapper.CreateMap<UndoRedoData, UndoRedoData>();
            Mapper.CreateMap<Queue<UndoRedoData>, Queue<UndoRedoData>>();
            Task task = Task.Factory.StartNew(() =>
            {
                string executableDirectory = AppDomain.CurrentDomain.BaseDirectory;
                executableDirectory += "Lits\\";
                LithologyImageFolder = executableDirectory;
            });
        }

        public static void LoadFamilies()
        {
            if (!File.Exists("Families.xml"))
            {
                WriteFamilyFile.Write();
            }
            Families = GlobalSerializer.DeserializeFamily();
        }

        public static List<Family> Families { get; set; }

        static readonly GlobalDataModel _instance = new GlobalDataModel();
        public static bool ShouldSave { get; set; }
        public static GlobalDataModel Instance
        {
            get { return _instance; }
        }

        public static DataTable ReplaceFirstRowWithDataColumnsInDataTable(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0) return dataTable;
            foreach (DataColumn column in dataTable.Columns)
            {
                column.ColumnName = dataTable.Rows[0][column.ColumnName].ToString();
            }
            dataTable.Rows.RemoveAt(0);
            return dataTable;
        }

        public static DataSet GetDatasetFromExcelFile(string fileName, string token)
        {
            try
            {
                var stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader;
                DataSet result;
                if (fileName.EndsWith(".xls"))
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream, true);
                    result = excelReader.AsDataSet(true);
                }
                else
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    result = excelReader.AsDataSet();
                }
                if (result == null)
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(token,
                      IoC.Kernel.Get<IResourceHelper>().ReadResource("ExcelSheetSeemsToBeCorrupt"));
                }
                excelReader.Close();
                return result;
            }
            catch (IOException ex)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(token, ex.Message);
                return null;
            }
        }

        public static string LithologyImageFolder { get; set; }

        public void SendMessage(string token, NotificationMessageEnum messageType)
        {
            SendMessage(token, messageType, null);
        }
        public void SendMessage(string token, NotificationMessageEnum messageType, object notificationObject)
        {
            Messenger.Default.Send(new NotificationMessageType
            {
                MessageType = messageType,
                MessageObject = notificationObject
            }, token);
        }

        public IMainScreenViewModel MainViewModel { get; set; }

        public void ClearAll()
        {
            MainViewModel.SelectedSubDataset = null;
            MainViewModel.SelectedDataset = null;
            MainViewModel.SelectedProject = null;
            MainViewModel.FileMenu.SelectedWell = null;
            GlobalCollection.Instance.ClearAll();
            UndoRedoObject.GlobalUndoStack.Clear();
            UndoRedoObject.GlobalRedoStack.Clear();
        }

        public static string GetIncrementalEntityName<T>(IEnumerable<string> lst)
        {
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()), "Unable to return incremental entity name");

            var currentNumber = 0;

            string entityName = typeof(T).Name;
            int entityNameLength = entityName.Length + 1;
            foreach (var obj in lst)
            {
                int returnNumber;
                if (!Int32.TryParse(obj.Substring(entityNameLength), out returnNumber)) continue;
                if (currentNumber < returnNumber) currentNumber = returnNumber;
            }
            currentNumber += 1;
            return string.Format("{0}_{1}", entityName, currentNumber.ToString());
        }

        public static void DeleteFilesInTempFolder()
        {
            if (Directory.Exists("Temp"))
            {
                var files = Directory.GetFiles("Temp");
                foreach (var file in files.ToList())
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
        public static void ApplyGrossStyleToLine(LineAnnotationExtended annotation, int grossStyle)
        {
            switch (grossStyle)
            {
                case 0:
                    annotation.StrokeThickness = 2;
                    break;
                case 1:
                    annotation.StrokeThickness = 4;
                    break;
                case 2:
                    annotation.StrokeThickness = 6;
                    break;
                case 3:
                    annotation.StrokeThickness = 8;
                    break;
            }
        }

        public static void ApplyStyleToLine(LineAnnotationExtended annotation, int lineStyle)
        {

            var lst = new List<double>();
            switch (lineStyle)
            {
                case 1:
                    lst.Add(4);
                    lst.Add(2);
                    annotation.StrokeDashArray = new DoubleCollection(lst);
                    break;
                case 2:
                    lst.Add(2);
                    lst.Add(2);
                    annotation.StrokeDashArray = new DoubleCollection(lst);
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }

        public void AddNamedAxisInChart(TrackToShow trackToShowObject, string axisName)
        {
            var numericAxis = new NumericAxis
            {
                DrawLabels = false,
                DrawMajorBands = false,
                DrawMajorGridLines = false,
                DrawMajorTicks = false,
                Id = axisName
            };
            var fastLineRenderableSeries = new FastLineRenderableSeries
            {
                Name = axisName,
                XAxisId = axisName
            };

            trackToShowObject.CurveRenderableSeries.Add(fastLineRenderableSeries);
            
            var dataSeries = new XyDataSeries<double, double>();
            dataSeries.AcceptsUnsortedData = true;           
            fastLineRenderableSeries.DataSeries = dataSeries;

            if (axisName == "Lithology" && trackToShowObject.Curves.Any()) //this is just for legend control
            {
                trackToShowObject.Curves[0].FastLineRenderableSeries = fastLineRenderableSeries;
                dataSeries.Append(0, 0);
                dataSeries.Append(10, 0);
            }
            trackToShowObject.XAxisCollection.Add(numericAxis);

            numericAxis.VisibleRange = new DoubleRange(0, 10);
            numericAxis.VisibleRangeLimit = new DoubleRange(0, 10);
        }

        static List<ToolbarInfo> _availableToolbarItems;
        public static List<ToolbarInfo> AvailableToolbarItems
        {
            get
            {
                if (_availableToolbarItems == null)
                    InitializeAvailableToolbarItems();
                return _availableToolbarItems;
            }
            set
            {
                _availableToolbarItems = value;
            }
        }

        public static void InitializeAvailableToolbarItems()
        {
            AvailableToolbarItems = new List<ToolbarInfo>
            {
                new ToolbarInfo("NewProject", @"..\Images\imgNewProjectTransparent.png", 0, command: "FileMenu.FileNewProjectCommand"),
                new ToolbarInfo("LoadProject", @"..\Images\imgLoadProjectTransparent.png", 1, command: "FileMenu.FileLoadFileCommand"),
                new ToolbarInfo("SaveProject", @"..\Images\imgSaveProjectTransparent.png", 2, command: "FileMenu.FileSaveProjectCommand"),
                new ToolbarInfo("Print", @"..\Images\imgPrint.png", 3, command: "FileMenu.FilePrintCommand"),
                new ToolbarInfo("WellProperties", @"..\Images\imgTowerTransparent.png", 4, command: "FileMenu.FileWellPropertiesCommand"),
                new ToolbarInfo("CalculateOverburden", @"..\Images\imgOB.png", 5, command: "CalculateMenu.CalculateOverburdenGradientCommand"),
                new ToolbarInfo("PorePressure", @"..\Images\imgPP.png", 6, command: "CalculateMenu.CalculatePorePressureGradientCommand"),
                new ToolbarInfo("CalculateFracture", @"..\Images\imgFG.png", 7, command: "CalculateMenu.CalculateFractureGradientCommand"),
                new ToolbarInfo("ShalePointFilter",@"..\Images\SPF.png",8,ControlType.Button, "CalculateMenu.CalculateShalePointFilterCommand", true),
                new ToolbarInfo("GeologyLithology",@"..\Images\imgLithologyTransparent.png", 8,command: "GeologyMenu.GeologyLithologyAddRemoveCommand"),
                new ToolbarInfo("PrintCurve",@"..\Images\imgCurvePrintingRound.png",9,command:"GraphicMenu.GraphicsPrintCurveCommand"),
                new ToolbarInfo("PrintDataset",@"..\Images\imgDatasetPrintingRound.png",10,command:"DataMenu.DataPrintDatasetCommand"),
                new ToolbarInfo("DrawLine", @"..\Images\imgLinesPrinting.png", 11,command: "DrawLineCommand"),
                new ToolbarInfo("Notes", @"..\Images\imgNotesTransparent.png", 12, command: "EditMenu.NotesWindowCommand"),                
                new ToolbarInfo("Undo", @"..\Images\imgUndoTransparent.png", 13, command: "EditMenu.EditUndoCommand"),
                new ToolbarInfo("Redo", @"..\Images\imgRedoTransparent.png", 14, command: "EditMenu.EditRedoCommand"),
                new ToolbarInfo("Help", @"..\Images\imgHelpTransparent.png", 15, command: "HelpMenu.HelpContentsCommand"),
                new ToolbarInfo("SyncDepth", @"..\Images\imgCheckBox.png", 16, ControlType.Checkbox, "IsSyncZoom"),
                new ToolbarInfo("Tooltip", @"..\Images\imgCheckBox.png", 18, ControlType.Checkbox, "IsTooltipVisible",true),
                new ToolbarInfo("ShowFullLithology", @"..\Images\imgCheckBox.png", 19, ControlType.Checkbox, "GeologyMenu.IsFullLithology"),
                new ToolbarInfo("ShowHideLithologyToolbar", @"..\Images\imgCheckBox.png", 20, ControlType.Checkbox, "GeologyMenu.IsLithologyVisible"),
                new ToolbarInfo("ShowHideFormationToolbar", @"..\Images\imgCheckBox.png", 21, ControlType.Checkbox,
                    "GeologyMenu.IsFormationVisible"),
                new ToolbarInfo("ShowHideFTToolTip", @"..\Images\imgCheckBox.png", 22, ControlType.Checkbox,
                    "GeologyMenu.IsFTTooltipVisible"),
                new ToolbarInfo("ShowHideFTName", @"..\Images\imgCheckBox.png", 23, ControlType.Checkbox,
                    "GeologyMenu.IsFTNameVisible", true),
                new ToolbarInfo("ShowHideTrackControls",@"..\Images\imgCheckBox.png",24,ControlType.Checkbox,
                    "IsTrackControlsVisible", true)
            };
        }
    }//end class

    public static class DecimalExtensions
    {
        public static decimal ToTwoDigits(this decimal dec)
        {
            return decimal.Parse(dec.ToString("0.##"));
        }
    }
}//end namespace
