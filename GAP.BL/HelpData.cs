using GAP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAP.BL
{
    public class HelpData
    {
        public string BasicInfo { get; set; }
        public string Description { get; set; }
        public string ScreenName { get; set; }
        public string ViewName { get; set; }
    }//end class

    public class HelpDataRepository
    {
        public IEnumerable<HelpData> GetDataForIndexing()
        {
            List<HelpData> lst = new List<HelpData>();
            lst.Add(new HelpData
            {
                BasicInfo = "Add new note in the application",
                Description = "This screen is responsible for adding notes in the application, viewing all notes, removing all or selected notes are done by this screen",
                ScreenName = "Add New Note",
                ViewName = "AddNewNoteView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Add or remove formation tops in the charts",
                Description = "This screen is being used to view the formation tops by charts. We could view, add or remove single or multiple formation tops in the application from this screen",
                ScreenName = "Add/Remove Formation Top",
                ViewName = "AddRemoveFormationTopView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Add or remove lithologies in the tracks",
                Description = "This screen is being used to view the lithologies in tracks. We could view, add or remove single or multiple lithologies in application from this screen",
                ScreenName = "Add/Remove Lithologies",
                ViewName = "AddRemoveLithologyView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Settings for calculating average or exact values",
                Description = "This screen holds settings for average or exact values and this screen will show itself from dataset import screen, when user opt for the average values",
                ScreenName = "Average And Exact Or Closer Value",
                ViewName = "ImportDataView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Adding a new chart in the system",
                Description = "This screens= adds a new chart in the system, User could also add a new chart by clicking on Add (+) button on the main screen after the last chart",
                ScreenName = "Add New Chart",
                ViewName = "ChartNewView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Changing the display order for the charts",
                Description = "This screen decides which chart should appear at which position, user could also change the chart position by dragging the chart header from one position to another, You could find this screen under the name of Chart Properties",
                ScreenName = "Sort Chart",
                ViewName = "ChartPropertiesView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "This screens add a new curve in the application",
                Description = "Adding a new curve on the selected track of selected chart. User could also add a new curve by dragging a dataset from object explorer to desired track",
                ScreenName = "Add New Curve",
                ViewName = "CurveNewView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "This screens prints an existing curve on screen or on paper",
                Description = "Printing a single curve or multiple curves or even tracks without any curves",
                ScreenName = "Print Curve",
                ViewName = "CurvePrintingView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "This screen helps user manage the toolbar as they feels fit to their needs",
                Description = "Changing the position of buttons on toolbar or adding/removing buttons",
                ScreenName = "Customize Toolbar",
                ViewName = "CustomizeToolbarView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "This screen changes the properties  of the dataset while importing them from other data sources",
                Description = "User could set the properties like name, color, marker color etc from this screen while importing datasets in the system from excel sheets, text files or las files",
                ScreenName = "Map dataset properties",
                ViewName = "ImportDataView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Printing dataset with their properties or spreadsheet data",
                Description = "User could choose single or multiple datasets to print with their basic properties or spreadsheet data",
                ScreenName = "Print Dataset",
                ViewName = "DatasetPrintingView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Adding or Editing dataset",
                Description = "Adding a dataset or editing an exising dataset. User could edit any dataset by right clicking on it on object explorer",
                ScreenName = "Add/Edit Dataset",
                ViewName = "DatasetView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Exporting system dataset to other datasources like text or excel sheet",
                Description = "A single dataset can be exported from the system to other data sources like text files or excel sheets",
                ScreenName = "Export Dataset",
                ViewName = "ExportDataView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Exporting formation tops of a single chart to excel sheets",
                Description = "User could use this screen to export formation tops from the system to excel sheets for further processing",
                ScreenName = "Export Formation Tops",
                ViewName = "FormationTopExportView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Importing formation tops from excel sheets to system",
                Description = "User could import formation tops to multiple charts from other data sources like excel sheet",
                ScreenName = "Import Formation Tops",
                ViewName = "FormationTopImportView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Basic settings on how full screen application should perform",
                Description = "Settings like visibility of status bar, object explorer or toolbar is being performed here",
                ScreenName = "Full Screen Settings",
                ViewName = "FullScreenSettingsView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Go to a particular track",
                Description = "Find a particular track among all the charts and all the tracks",
                ScreenName = "Find a Track",
                ViewName = "GoToMarkView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Import dataset from different data sources",
                Description = "Import multiple datasets from excel, text and las files in to the system",
                ScreenName = "Import Dataset",
                ViewName = "ImportDataView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Settings that define sub datasets",
                Description = "This screen creates new sub datasets under the existing datasets and guide users draw lines on the tracks",
                ScreenName = "Line/SubDataset Properties",
                ViewName = "LinePropertiesView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Export lithologies from the system",
                Description = "Export lithologies from a single selected chart to outer data sources like excel sheets",
                ScreenName = "Export Lithologies",
                ViewName = "ExportLithologyView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Add/Remove images for lithologies in the system",
                Description = "This screens list all the lithology images in application",
                ScreenName = "Lithology Properties",
                ViewName = "LithologyPropertiesView"
            });
            lst.Add(new HelpData
                {
                    BasicInfo = "Add/Edit the curve details for an existing dataset",
                    Description = @"To edit the spreadsheet details, user could right click on dataset in object explorer and click on Edit Spreadsheet option 
This screen could only be opened by a dataset. Data in this screen has been generated while we are creating a new dataset",
                    ScreenName = "Maintain spreadsheet",
                    ViewName = "DatasetView"
                });
            lst.Add(new HelpData
                {
                    BasicInfo = "List all the notes of an existing project file",
                    Description = "This screen has all the notes and also gives you the ability the add more notes in the application",
                    ScreenName = "Notes Listing",
                    ViewName = "NotesView"
                });
            lst.Add(new HelpData
            {
                BasicInfo = "Add/Edit Projects in the application",
                Description = @"Project is the most basic component of the application, It holds wells which further holds datasets 
which are the most important part of the application. Deleting a project results in deleting all child entities in it",
                ScreenName = "Project Screen",
                ViewName = "ProjectView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Remove single or multiple charts from the application",
                Description = @"This screen lists all the charts in the application, including details like how many tracks and curves are under this chart
and based on user selection and requirement, user could choose single, multiple or all the charts in the application",
                ScreenName = "Remove Charts",
                ViewName = "RemoveChartNewView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Remove single or multiple curves from the application",
                Description = @"This screen lists all the curves in the application, including details like in which chart and track they are, and let
user make their choice",
                ScreenName = "Remove Curves",
                ViewName = "RemoveCurveNewView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Remove single or multiple  tracks from the application",
                Description = @"This screen lists all the tracks in the application, including details like from which chart they belongs and how many 
curves they have",
                ScreenName = "Remove Tracks",
                ViewName = "RemoveTrackNewView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Holds all the settings for spreadsheet screen",
                Description = @"This screen holds settings on how dataset spreadsheet screen should behave like what should be the initial and final depth and what would be the step",
                ScreenName = "Spreadsheet  Settings",
                ViewName = "DatasetView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Spreadsheet for sub datasets",
                Description = "This screen holds the details for the sub dataset that has been generated by drawing lines using the line editor view",
                ScreenName = "Sub Dataset spreadsheet view",
                ViewName = "SubDatasetView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Add new tracks in the system",
                Description = @"This screen helps user add a new track in the system by choosing against which chart they need to add it and providing a name for it, 
User could also add new tracks in system by clicking the '+' sign on each chart",
                ScreenName = "Add New Track",
                ViewName = "TrackNewView"
            });
            lst.Add(new HelpData
                {
                    BasicInfo = "Changing the display order for the tracks",
                    Description = "This screen decides which track should appear at which position,  You could find this screen under the name of Track Properties in Graphics Menu",
                    ScreenName = "Sort Track",
                    ViewName = "TrackPropertiesView"
                });
            lst.Add(new HelpData
            {
                BasicInfo = "Add/Edit Well in the application",
                Description = "Add/Edit well in the application, A well could hold multiple Datasets inside it but could have only one parent Project under which it could  be find, User could delete the well by right clicking on it in object explorer and choose the desired operation from the context menu",
                ScreenName = "Well View",
                ViewName = "WellView"
            }); ;
            lst.Add(new HelpData
            {
                BasicInfo = "Zoom selected track up to the mentioned level",
                Description = "User have options to zoom in and out using Mouse wheel or Slider control, but if user want to pin point a zooming range then this is the option for them",
                ScreenName = "Zoom Dialog",
                ViewName = "ZoomDialogView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Shale Point Filter",
                Description = "Shale Point Filter",
                ScreenName = "Shale Point Filter",
                ViewName = "ShalePointFilterView"
            });
            lst.Add(new HelpData
            {
                BasicInfo = "Add a new dataset by applying mathematial equations against aother dataset",
                Description = "Add a new dataset by applying mathematial equations against aother dataset",
                ScreenName = "Equation",
                ViewName = "AddEquationView"
            });
            lst.Add(new HelpData
                {
                    BasicInfo = "Math Filter",
                    Description = "Math Filter",
                    ScreenName = "Math Filter",
                    ViewName = "MathFilter"
                });
            return lst;
        }

    }
}//end namespace
