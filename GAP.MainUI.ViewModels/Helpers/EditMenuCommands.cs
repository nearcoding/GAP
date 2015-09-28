using GalaSoft.MvvmLight.Command;
using GAP.BL;
using System.Windows.Input;
using GAP.MainUI.ViewModels.Helpers;
using Ninject;
using GAP.Helpers;
using AutoMapper;
using System.Linq;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class EditMenuCommands
    {
        public EditMenuCommands(string token)
        {
            _token = token;
        }
        ICommand _editUndoCommand;
        ICommand _editRedoCommand;
        ICommand _editCutCommand;

        static string _token;
        public ICommand EditUndoCommand
        {
            get { return _editUndoCommand ?? (_editUndoCommand = new RelayCommand<bool>(u => EditUndoRedo(true), u => CanUndo())); }
        }

        private void EditUndoRedo(bool isUndoOperation)
        {
            UndoRedoData currentObject;

            currentObject = isUndoOperation ? UndoRedoObject.GlobalUndoStack.Pop() : UndoRedoObject.GlobalRedoStack.Pop();

            switch (currentObject.EffectedType)
            {
                case UndoRedoType.Project:
                    GlobalCollection.Instance.Projects.OperationType = GetOperationType(isUndoOperation);
                    UndoRedoObjects(currentObject, isUndoOperation);
                    GlobalCollection.Instance.Projects.OperationType = OperationType.None;
                    break;
                case UndoRedoType.Well:
                    var well = currentObject.ActualObject as Well;
                    var projectObject = HelperMethods.Instance.GetProjectByID(well.RefProject);
                    projectObject.Wells.OperationType = GetOperationType(isUndoOperation);
                    UndoRedoObjects(currentObject, isUndoOperation);
                    projectObject.Wells.OperationType = OperationType.None;
                    break;
                case UndoRedoType.Dataset:
                    var dataset = currentObject.ActualObject as Dataset;
                    var wellObject = HelperMethods.Instance.GetWellByID(dataset.RefWell);
                    wellObject.Datasets.OperationType = GetOperationType(isUndoOperation);
                    UndoRedoObjects(currentObject, isUndoOperation);
                    wellObject.Datasets.OperationType = OperationType.None;
                    break;
                case UndoRedoType.SubDataset:
                    var subDataset = currentObject.ActualObject as SubDataset;
                    var datasetObject = HelperMethods.Instance.GetDatasetByID(subDataset.Dataset);
                    datasetObject.SubDatasets.OperationType = GetOperationType(isUndoOperation);
                    UndoRedoObjects(currentObject, isUndoOperation);
                    datasetObject.SubDatasets.OperationType = OperationType.None;
                    break;
                case UndoRedoType.Chart:
                    GlobalCollection.Instance.Charts.OperationType = GetOperationType(isUndoOperation);
                    UndoRedoObjects(currentObject, isUndoOperation);
                    GlobalCollection.Instance.Charts.OperationType = OperationType.None;
                    break;
                case UndoRedoType.Track:
                    var track = currentObject.ActualObject as Track;
                    var chartObject = HelperMethods.Instance.GetChartByID(track.RefChart);
                    chartObject.Tracks.OperationType = GetOperationType(isUndoOperation);
                    UndoRedoObjects(currentObject, isUndoOperation);
                    chartObject.Tracks.OperationType = OperationType.None;
                    break;
                case UndoRedoType.Curve:
                    var curve = currentObject.ActualObject as Curve;
                    var trackObjectForCurve = HelperMethods.Instance.GetTrackByID(curve.RefTrack);
                    trackObjectForCurve.Curves.OperationType = GetOperationType(isUndoOperation);
                    UndoRedoObjects(currentObject, isUndoOperation);
                    trackObjectForCurve.Curves.OperationType = OperationType.None;
                    break;
            }
        }

        private static OperationType GetOperationType(bool isUndoOperation)
        {
            return isUndoOperation ? OperationType.Undo : OperationType.Redo;
        }

        private static void UndoRedoObjects(UndoRedoData currentObject, bool isUndoOperation)
        {
            switch (currentObject.ActionType)
            {
                case ActionPerformed.ItemAdded:
                    UndoRedoDeleteObject(currentObject);
                    break;
                case ActionPerformed.ItemDeleted:
                    UndoRedoAddObject(currentObject);
                    break;
                //case ActionPerformed.ItemUpdated:
                //    UndoRedoUpdateObject(currentObject, isUndoOperation);
                //    return;
            }
        }

        //private static void ProjectUpdateForUndoRedo(UndoRedoData undoRedoDataObject, bool isUndoOperation)
        //{
        //    Mapper.CreateMap(typeof(Project), typeof(Project));

        //    var project = undoRedoDataObject.ActualObject as Project;
        //    var copyOfProject = (Project)Mapper.Map(project, typeof(Project), typeof(Project));
        //    var actualObject = HelperMethods.Instance.GetProjectByID(project.ID);
        //    if (actualObject == null) return;

        //    var newProject = (Project)Mapper.Map(project.PreviousObject, typeof(Project), typeof(Project));
        //    Mapper.Map(newProject, actualObject);
        //    actualObject.PreviousObject = copyOfProject;

        //    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
        //      string.Format("Project {0} updated in system", actualObject.Name));

        //    AddItemToUndoRedoStack(new UndoRedoData
        //    {
        //        ActionType = ActionPerformed.ItemUpdated,
        //        ActualObject = actualObject,
        //        EffectedType = UndoRedoType.Project
        //    }, isUndoOperation);
        //}

        //private static void WellUpdateForUndoRedo(UndoRedoData undoRedoDataObject, bool isUndoOperation)
        //{
        //    Mapper.CreateMap(typeof(Well), typeof(Well));
        //    var well = undoRedoDataObject.ActualObject as Well;
        //    var copyOfWell = (Well)Mapper.Map(well, typeof(Well), typeof(Well));
        //    var actualWellObject = HelperMethods.Instance.GetWellByID(well.ID);

        //    if (actualWellObject == null) return;
        //    var newWell = (Well)Mapper.Map(well.PreviousObject, typeof(Well), typeof(Well));
        //    Mapper.Map(newWell, actualWellObject);
        //    actualWellObject.PreviousObject = copyOfWell;

        //    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
        //        string.Format("Well {0} updated in system", actualWellObject.Name));

        //    AddItemToUndoRedoStack(new UndoRedoData
        //    {
        //        ActionType = ActionPerformed.ItemUpdated,
        //        ActualObject = actualWellObject,
        //        EffectedType = UndoRedoType.Well
        //    }, isUndoOperation);

        //}
        //private static void DatasetUpdateForUndoRedo(UndoRedoData undoRedoDataObject, bool isUndoOperation)
        //{
        //    Mapper.CreateMap(typeof(Dataset), typeof(Dataset));
        //    var dataset = undoRedoDataObject.ActualObject as Dataset;
        //    var copyOfDataset = (Dataset)Mapper.Map(dataset, typeof(Dataset), typeof(Dataset));
        //    var actualDatasetObject = HelperMethods.Instance.GetDatasetByID(dataset.ID);

        //    if (actualDatasetObject == null) return;
        //    var newDataset = (Dataset)Mapper.Map(dataset.PreviousObject, typeof(Dataset), typeof(Dataset));
        //    Mapper.Map(newDataset, actualDatasetObject);
        //    actualDatasetObject.PreviousObject = copyOfDataset;

        //    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
        //        string.Format("Dataset {0} updated in system", actualDatasetObject.Name));

        //    AddItemToUndoRedoStack(new UndoRedoData
        //    {
        //        ActionType = ActionPerformed.ItemUpdated,
        //        ActualObject = actualDatasetObject,
        //        EffectedType = UndoRedoType.Dataset
        //    }, isUndoOperation);

        //}
        //private static void UndoRedoUpdateObject(UndoRedoData undoRedoDataObject, bool isUndoOperation)
        //{
        //    switch (undoRedoDataObject.EffectedType)
        //    {
        //        case UndoRedoType.Project:
        //            ProjectUpdateForUndoRedo(undoRedoDataObject, isUndoOperation);
        //            break;
        //        case UndoRedoType.Well:
        //            WellUpdateForUndoRedo(undoRedoDataObject, isUndoOperation);
        //            break;
        //        case UndoRedoType.Dataset:
        //            DatasetUpdateForUndoRedo(undoRedoDataObject, isUndoOperation);
        //            break;
        //    }
        //}

        private static void AddItemToUndoRedoStack(UndoRedoData undoRedoData, bool isUndoOperation)
        {
            if (isUndoOperation)
                UndoRedoObject.GlobalRedoStack.Push(undoRedoData);
            else
                UndoRedoObject.GlobalUndoStack.Push(undoRedoData);
        }

        private static void UndoRedoAddObject(UndoRedoData currentObject)
        {
            switch (currentObject.EffectedType)
            {
                case UndoRedoType.Chart:
                    var chart = currentObject.ActualObject as Chart;
                    ChartManager.Instance.AddChartObject(chart);
                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                       string.Format("Chart {0} added to system", chart.Name));
                    break;
                case UndoRedoType.Track:
                    Track track = currentObject.ActualObject as Track;
                    TrackManager.Instance.AddTrackObject(track);
                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                        string.Format("Track {0} added to Chart {1}", track.Name,
                       HelperMethods.Instance.GetChartByID(track.RefChart).Name));
                    break;
                case UndoRedoType.Curve:
                    Curve curve = currentObject.ActualObject as Curve;
                    CurveManager.Instance.AddCurveObject(curve);
                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                        string.Format("Curve {0} added to Chart {1} and Track {2}",
                        HelperMethods.Instance.GetDatasetByID(curve.RefDataset).Name,
                       HelperMethods.Instance.GetChartByID(curve.RefChart).Name,
                       HelperMethods.Instance.GetTrackByID(curve.RefTrack).Name));
                    break;
                case UndoRedoType.Project:
                    var projectObject = currentObject.ActualObject as Project;
                    GlobalCollection.Instance.Projects.Add(projectObject);
                    foreach (var obj in projectObject.Wells)
                        AddWellObject(obj);

                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                            string.Format("Project {0} added", (currentObject.ActualObject as Project).Name));
                    break;
                case UndoRedoType.Well:
                    Well well = currentObject.ActualObject as Well;
                    AddWellObject(well);

                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                        string.Format("Well {0} added to Project {1}", well.Name,
                       HelperMethods.Instance.GetProjectByID(well.RefProject).Name));
                    break;
                case UndoRedoType.Dataset:
                    Dataset dataset = currentObject.ActualObject as Dataset;
                    DatasetManager.Instance.AddDatasetObject(dataset);

                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                        string.Format("Dataset {0} added to Well {1} and Project {2}", dataset.Name,
                       HelperMethods.Instance.GetWellByID(dataset.RefWell).Name,
                      HelperMethods.Instance.GetProjectByID(dataset.RefProject).Name));
                    break;
                case UndoRedoType.SubDataset:
                    SubDataset subDataset = currentObject.ActualObject as SubDataset;
                    var datasetObject = HelperMethods.Instance.GetDatasetByID(subDataset.Dataset);
                    datasetObject.SubDatasets.Add(subDataset);
                    AddSubDatasetAnnotationsToSelectedChart(subDataset, datasetObject);

                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                    string.Format("Subdataset {0} added {1}->{2}->{3}", subDataset.Name,
                   HelperMethods.Instance.GetProjectByID(subDataset.Project).Name,
                   HelperMethods.Instance.GetWellByID(subDataset.Well).Name,
                   HelperMethods.Instance.GetDatasetByID(subDataset.Dataset).Name));
                    break;
            }
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.RefreshTreeView);
        }

        private static void AddWellObject(Well well)
        {
            var project = HelperMethods.Instance.GetProjectByID(well.RefProject);
            if (!project.Wells.Any(u => u.ID == well.ID))
                project.Wells.Add(well);

            foreach (var datasetObj in well.Datasets)
                DatasetManager.Instance.AddDatasetObject(datasetObj);
        }
     
        private static void AddSubDatasetAnnotationsToSelectedChart(SubDataset subDataset, Dataset dataset)
        {
            foreach (var track in IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.Tracks)
            {
                var curveToShow = track.Curves.SingleOrDefault(u => u.RefDataset == dataset.ID);
                if (curveToShow != null)
                {
                    CurveManager.Instance.AddExistingSubDatasetToTrack(track, dataset, curveToShow);
                }
            }
        }

        private static void UndoRedoDeleteObject(UndoRedoData currentObject)
        {
            switch (currentObject.EffectedType)
            {
                case UndoRedoType.Chart:
                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                        string.Format("Chart {0} deleted", (currentObject.ActualObject as Chart).Name));
                    ChartManager.Instance.RemoveChartObject(currentObject.ActualObject as Chart);                    
                    break;
                case UndoRedoType.Track:
                    Track track = currentObject.ActualObject as Track;
                    TrackManager.Instance.RemoveTrackObject(track);
                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                        string.Format("Track {0} deleted from Chart {1}", track.Name,
                        HelperMethods.Instance.GetChartByID(track.RefChart).Name));
                    break;
                case UndoRedoType.Curve:
                    Curve curve = currentObject.ActualObject as Curve;
                    CurveManager.Instance.RemoveCurveObject(curve);
                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                        string.Format("Curve {0} deleted from Chart {1} and Track {2}",
                     HelperMethods.Instance.GetDatasetByID(curve.RefDataset).Name,
                     HelperMethods.Instance.GetChartByID(curve.RefChart).Name,
                     HelperMethods.Instance.GetTrackByID(curve.RefTrack).Name));
                    break;
                case UndoRedoType.Project:
                    IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.DeleteProject((currentObject.ActualObject as Project).ID);
                    //GlobalCollection.Instance.Projects.Remove(currentObject.ActualObject as Project);
                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                        string.Format("Project {0} deleted", (currentObject.ActualObject as Project).Name));
                    break;
                case UndoRedoType.Well:
                    Well well = currentObject.ActualObject as Well;
                    IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.DeleteWell(well);

                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                        string.Format("Well {0} deleted from Project {1}", well.Name,
                       HelperMethods.Instance.GetProjectByID(well.RefProject).Name));
                    break;
                case UndoRedoType.Dataset:
                    Dataset dataset = currentObject.ActualObject as Dataset;
                    DatasetManager.Instance.RemoveDatasetObject(dataset);
                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                        string.Format("Dataset {0} deleted from Project {1} and Well {2}", dataset.Name,
                       HelperMethods.Instance.GetProjectByID(dataset.RefProject).Name,
                      HelperMethods.Instance.GetWellByID(dataset.RefWell).Name));
                    break;
                case UndoRedoType.SubDataset:
                    SubDataset subDataset = currentObject.ActualObject as SubDataset;
                    var datasetObject = HelperMethods.Instance.GetDatasetByID(subDataset.Dataset);
                    datasetObject.SubDatasets.Remove(subDataset);
                    DeleteAnnotationsFromSubDatasetObject(subDataset);
                    GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.UndoRedoMessage,
                        string.Format("Subdataset {0} deleted from {1}->{2}->{3}", subDataset.Name,
                       HelperMethods.Instance.GetProjectByID(subDataset.Project).Name,
                      HelperMethods.Instance.GetWellByID(subDataset.Well).Name,
                      HelperMethods.Instance.GetDatasetByID(subDataset.Dataset).Name));
                    break;
            }
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.RefreshTreeView);
        }

        private static void DeleteAnnotationsFromSubDatasetObject(SubDataset subDataset)
        {
            TrackManager.Instance.RemoveAllAnnotationsBySubDataset(subDataset);
        }

        private bool CanUndo()
        {
            return UndoRedoObject.GlobalUndoStack.Count > 0;
        }

        public ICommand EditRedoCommand
        {
            get { return _editRedoCommand ?? (_editRedoCommand = new RelayCommand<bool>(u => EditUndoRedo(false), u => CanRedo())); }
        }

        private bool CanRedo()
        {
            return UndoRedoObject.GlobalRedoStack.Count > 0;
        }

        public ICommand EditCutCommand
        {
            get { return _editCutCommand ?? (_editCutCommand = new RelayCommand(EditCut, CanCut)); }
        }
        private void EditCut()
        {

        }

        private bool CanCut()
        {
            return false;
        }


        ICommand _editCopyCommand;
        public ICommand EditCopyCommand
        {
            get { return _editCopyCommand ?? (_editCopyCommand = new RelayCommand(EditCopy, CanCopy)); }
        }
        private void EditCopy()
        {

        }

        private bool CanCopy()
        {
            return false;
        }


        ICommand _editPasteCommand;
        public ICommand EditPasteCommand
        {
            get { return _editPasteCommand ?? (_editPasteCommand = new RelayCommand(EditPaste, CanPaste)); }
        }
        private void EditPaste()
        {

        }

        private bool CanPaste()
        {
            return false;
        }

        ICommand _editDeleteCommand;
        public ICommand EditDeleteCommand
        {
            get { return _editDeleteCommand ?? (_editDeleteCommand = new RelayCommand(EditDelete, CanDelete)); }
        }
        private void EditDelete()
        {

        }

        private bool CanDelete()
        {
            return false;
        }

        ICommand _notesWindowCommand;
        public ICommand NotesWindowCommand
        {
            get { return _notesWindowCommand ?? (_notesWindowCommand = new RelayCommand(NotesWindow)); }
        }

        private void NotesWindow()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.NotesWindow);
        }
    }//end class
}//end namespace
