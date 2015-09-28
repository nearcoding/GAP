using AutoMapper;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Media;
using Ninject;
using System;
using GAP.BL.CollectionEntities;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class MaintainDatasetViewModel : DatasetBaseViewModel
    {
        string _originalDatasetName, _saveButtonText;
        int _internalCurrentUnits;

        public MaintainDatasetViewModel(string token, Dataset dataset)
            : base(token)
        {
            //if (dataset != null && (!string.IsNullOrWhiteSpace(dataset.DatasetName) &&
            //    !string.IsNullOrWhiteSpace(dataset.RefProject) && !string.IsNullOrWhiteSpace(dataset.RefWell)))
            //    InitializeExistingDataset(dataset);
            //else
            //    InitializeNewDataset();
        }

        //private void InitializeExistingDataset(Dataset dataset)
        //{
        //    IsNew = false;
        //    SaveButtonText = IoC.Kernel.Get<IResourceHelper>().ReadResource("SaveButton");
        //    Title = IoC.Kernel.Get<IResourceHelper>().ReadResource("EditDataset");
        //    _originalDatasetName = dataset.DatasetName;

        //    if (!GetCurrentObjectFromCollection(dataset)) return;

        //    if (CurrentObject.TVDMD == 0)
        //        TVDSelected = true;
        //    else
        //        MDSelected = true;

        //    Projects = new List<string>() { dataset.RefProject };
        //    Wells = new List<string>() { dataset.RefWell };

        //    var currentUnits = CurrentObject.Units;
        //    FamilySelectionChanged();
        //    CurrentObject.Units = currentUnits;
        //}

        //private bool GetCurrentObjectFromCollection(Dataset dataset)
        //{
        //    var result = DatasetCollection.Instance.DatasetList.Where(u => u.RefProject == dataset.RefProject &&
        //                            u.RefWell == dataset.RefWell && u.DatasetName == dataset.DatasetName).ToList();

        //    if (result.Count > 1)
        //        throw new InvalidOperationException(IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_MultipleDatasetsWithSameName"));
        //    else if (result.Count == 1)
        //        OriginalObject = result[0];

        //    if (OriginalObject == null)
        //    {
        //        IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_DatasetNotFound"), dataset.DatasetName, dataset.RefProject, dataset.RefWell));
        //        return false;
        //    }
        //    //this line makes a new copy of the dataset which is not part of the global data model list
        //    OriginalObject = Mapper.Map<Dataset>(OriginalObject);
        //    CurrentObject = Mapper.Map<Dataset>(OriginalObject);
        //    CurrentObject.ObjectChanged += CurrentObject_ObjectChanged;
        //    IsDirty = false;

        //    return true;
        //}

        //private void InitializeNewDataset()
        //{
        //    IsNew = true;
        //    Title = IoC.Kernel.Get<IResourceHelper>().ReadResource("NewDataset");
        //    SaveButtonText = IoC.Kernel.Get<IResourceHelper>().ReadResource("SaveButton");

        //    Projects = ProjectCollection.Instance.CurrentList.Where(u => u.Wells.Any()).Select(u => u.ProjectName);

        //    CreateNewCurrentObject();

        //    if (Projects.Any())
        //        CurrentObject.RefProject = Projects.First();

        //    TVDSelected = true;

        //    NotifyPropertyChanged("CurrentObject");
        //    //todo need to remove this code, this thing should happen automatically
        //    FamilySelectionChanged();
        //    ProjectSelectionChanged();
        //}

        //private void CreateNewCurrentObject()
        //{
        //    CurrentObject = new Dataset
        //    {
        //        Family = Families[0],
        //        RefProject = Projects.First(),
        //        LineStyle = 1,
        //        FinalDepth = 20,
        //        Step = 1,
        //        InitialDepth = 1,
        //        LineColor = Color.FromArgb(255, 0, 0, 0),
        //        MarkerColor = Color.FromArgb(255, 0, 0, 0),
        //        ShouldApplyBorderColor = false,
        //        MinUnitValue = 0,
        //        MaxUnitValue = 100
        //    };
        //}

        //[ContractInvariantMethod]
        //private void CodeInvariants()
        //{
        //    Contract.Invariant(CurrentObject != null, "Dataset Object should not be null");
        //    Contract.Invariant(Projects != null, "Project list should not be null");
        //    Contract.Invariant(Projects.Any(), "Project list must have some projects in it");
        //    Contract.Invariant(!string.IsNullOrWhiteSpace(Title), "Title should not be empty");
        //    Contract.Invariant(Wells != null, "Well list should not be null");
        //    Contract.Invariant(Wells.Any(), "Well list must have some wells in it");
        //}

        //protected override bool CanSave()
        //{
        //    return !string.IsNullOrWhiteSpace(CurrentObject.DatasetName) &&
        //            !string.IsNullOrWhiteSpace(CurrentObject.RefWell) &&
        //            !string.IsNullOrWhiteSpace(CurrentObject.RefProject) &&
        //            (IsDirty || IsNew);
        //}

        //private void UpdateDataset()
        //{
        //    //todo original dataset name has less visibility than the enclosed method
        //    //Contract.Requires(!string.IsNullOrWhiteSpace(_originalDatasetName), "Original Dataset name must be passed");
        //    var originalDataset = DatasetCollection.Instance.DatasetList.SingleOrDefault(u => u.RefProject == CurrentObject.RefProject
        //        && u.RefWell == CurrentObject.RefWell && u.DatasetName == _originalDatasetName);

        //    if (originalDataset != null)
        //        Mapper.Map(CurrentObject.TrimStringProperties(), originalDataset);
        //    else
        //    {
        //        IoC.Kernel.Get<ISendMessage>()
        //            .MessageBoxWithExclamation(Token, "Original dataset not found while updating");
        //        return;
        //    }

        //    if (GlobalDataModel.MainViewModel == null ||
        //        GlobalDataModel.MainViewModel.Projects == null || !GlobalDataModel.MainViewModel.Projects.Any())
        //        return;

        //    var projectToShow = GlobalDataModel.MainViewModel.Projects.SingleOrDefault
        //        (u => u.ProjectName == originalDataset.RefProject);

        //    IoC.Kernel.Get<IUndoRedoObject>().AddEntityToUndoRedoOperation<Dataset>(OriginalObject, CurrentObject);

        //    // UpdateTreeViewForDatasetName(originalDataset, projectToShow);
        //}

        ////private void UpdateTreeViewForDatasetName(Dataset originalDataset, ProjectsToShow projectToShow)
        ////{
        ////    if (projectToShow == null) return;

        ////    var wellToShow = projectToShow.Wells.SingleOrDefault(u => u.WellName == originalDataset.RefWell);
        ////    if (wellToShow == null) return;

        ////    var dataset = wellToShow.Datasets.SingleOrDefault(u => u.DatasetName == _originalDatasetName);
        ////    if (dataset != null) dataset.DatasetName = CurrentObject.DatasetName;
        ////}

        //private void UpdateDatasetObject()
        //{
        //    if (CurrentObject.DepthAndCurves.Any())
        //    {
        //        var min = CurrentObject.DepthAndCurves.Except(CurrentObject.DepthAndCurves.Where(u => u.Curve == decimal.Parse("-999.25") || u.Curve == decimal.Parse("-999"))).Min(v => v.Curve);
        //        var max = CurrentObject.DepthAndCurves.Max(u => u.Curve);

        //        if (min < CurrentObject.MinUnitValue || max > CurrentObject.MaxUnitValue)
        //        {
        //            ShouldSaveDataset = false;
        //            IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.UpdateDatasetWithMinMaxWarning);
        //            if (!ShouldSaveDataset) return; //else after executing it'll run the update dataset                     
        //        }
        //    }
        //    UpdateDataset();
        //    IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.UpdateDataset);
        //}

        //protected override void SaveDatasetObject()
        //{
        //    if (IsNew)
        //    {
        //        if (IsDatasetNameUniqueForAdding())
        //            SaveDataset();
        //    }
        //    else if (IsDatasetNameUniqueForEditing())
        //    {
        //        UpdateDatasetObject();
        //    }
        //}

        //private bool IsDatasetNameUniqueForEditing()
        //{
        //    if (DatasetCollection.Instance.DatasetList.Any(u => u.RefProject == CurrentObject.RefProject
        //        && u.RefWell == CurrentObject.RefWell && u.DatasetName == CurrentObject.DatasetName.Trim() && u.DatasetName != _originalDatasetName))
        //    {
        //        DatasetNameAlreadyInUse();
        //        return false;
        //    }
        //    return true;
        //}

        public bool ShouldSaveDataset { get; set; }

        //private void SaveDataset()
        //{
        //    ShouldSaveDataset = false;
        //    var displayIndex = 0;

        //    if (DatasetCollection.Instance.DatasetList.Any(u => u.RefProject == CurrentObject.RefProject && u.RefWell == CurrentObject.RefWell))
        //        displayIndex = DatasetCollection.Instance.DatasetList.Where(u => u.RefProject == CurrentObject.RefProject && u.RefWell == CurrentObject.RefWell).Select(v => v.DisplayIndex).Max() + 1;

        //    CurrentObject.DisplayIndex = displayIndex;

        //    IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.SaveDataset);

        //    if (ShouldSaveDataset)
        //        IoC.Kernel.Get<IUndoRedoObject>().UndoRedoOperationDone();
        //}

        //private void IncrementalDatasetName()
        //{
        //    Contract.Ensures(!string.IsNullOrWhiteSpace(CurrentObject.DatasetName), "Incremental dataset name must have some values");

        //    var lstDatasets = DatasetCollection.Instance.DatasetList.Where(u => u.RefProject == CurrentObject.RefProject &&
        //        u.RefWell == CurrentObject.RefWell && u.DatasetName.StartsWith("Dataset_")).Select(v => v.DatasetName);

        //    CurrentObject.DatasetName = GlobalDataModel.GetIncrementalEntityName(lstDatasets, "Dataset_");
        //}

        //protected override void ProjectSelectionChanged()
        //{
        //    FillAndSelectWellBasedOnProjectSelection();

        //    var projectObject =
        //        ProjectCollection.Instance.CurrentList.SingleOrDefault(u => u.ProjectName == CurrentObject.RefProject);
        //    if (projectObject == null) return;
        //    _internalCurrentUnits = projectObject.Units;
        //    CurrentObject.Family = Families[0];

        //    //todo this method supposed to be called automatically
        //    IncrementalDatasetName();
        //}

        //private void FillAndSelectWellBasedOnProjectSelection()
        //{
        //    Wells = ProjectCollection.Instance.CurrentList.Single(u => u.ProjectName == CurrentObject.RefProject).Wells.Select(v => v.WellName);// WellCollection.Instance.CurrentList.Where(u => u.RefProject == CurrentObject.RefProject).Select(v => v.WellName).ToList());
        //    if (!Wells.Any()) return;

        //    CurrentObject.RefWell = Wells.First();
        //    NotifyPropertyChanged("CurrentObject");
        //}
        //protected override void WellSelectionChanged()
        //{
        //    IncrementalDatasetName();
        //}

        //public string SaveButtonText
        //{
        //    get { return _saveButtonText; }
        //    set
        //    {
        //        _saveButtonText = value;
        //        NotifyPropertyChanged("SaveButtonText");
        //    }
        //}
    }//end class
}//end namespace