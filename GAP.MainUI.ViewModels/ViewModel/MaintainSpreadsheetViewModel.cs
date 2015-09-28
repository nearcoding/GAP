using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using Ninject;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using System.Collections;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class MaintainSpreadsheetViewModel : BaseViewModel<Dataset>
    {
        Stack<List<DepthCurveInfo>> _undoStack = new Stack<List<DepthCurveInfo>>();
        Stack<List<DepthCurveInfo>> _redoStack = new Stack<List<DepthCurveInfo>>();
        List<DepthCurveInfo> _selectedItems;

        int _selectedIndex;

        ICommand _deleteCommand, _copyCommand, _cutCommand, _pasteCommand, _undoCommand, _redoCommand,
        _exportCommand, _automaticDepthScreenCommand, _dataGridSelectionChangedCommand, _collectionChangedCommand,
        _increaseDecimalCommand, _decreaseDecimalCommand, _rowEditEndingCommand;

        public MaintainSpreadsheetViewModel(string token, Dataset dataSet, bool isEdit)
            : base(token)
        {
            if (dataSet == null || string.IsNullOrWhiteSpace(dataSet.Name))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, "Dataset should not be null");
                return;
            }
            Mapper.CreateMap<DepthCurveInfo, DepthCurveInfo>();
            Title = string.Format(
              IoC.Kernel.Get<IResourceHelper>().ReadResource("SpreadsheetForDataset"), dataSet.Name);

            if (isEdit)
                InitializeExistingSpreadsheet(dataSet);
            else
                InitializeNewSpreadsheet(dataSet);

            IsAutoRowsDisabled = true;
            IsAutoRowsEnabled = false;

            CurveHeader = string.Format("{0} [{1}]", dataSet.Family, dataSet.Units);
            DepthHeader = CurrentObject.IsTVD ? "TVD" : "MD";
        }

        private void InitializeNewSpreadsheet(Dataset dataSet)
        {
            IsNew = true;
            OriginalObject = dataSet;
            CurrentObject = Mapper.Map<Dataset>(dataSet);
            FillInitialSpreadsheet(CurrentObject);
        }

        private void InitializeExistingSpreadsheet(Dataset dataSet)
        {
            IsNew = false;
            OriginalObject = HelperMethods.Instance.GetDatasetByID(dataSet.ID);// DatasetCollection.Instance.DatasetList.SingleOrDefault

            CurrentObject = Mapper.Map<Dataset>(OriginalObject);

            CurrentObject.DepthAndCurves = (ObservableCollection<DepthCurveInfo>)Mapper.Map(OriginalObject.DepthAndCurves,
                typeof(ObservableCollection<DepthCurveInfo>), typeof(ObservableCollection<DepthCurveInfo>));

            AttachEventToListOfItems();
        }

        private void AttachEventToListOfItems()
        {
            if (IsNew) return;
        }

        public string DepthHeader { get; set; }

        public string CurveHeader { get; set; }

        private void FillInitialSpreadsheet(Dataset dataSet)
        {
            for (decimal i = dataSet.InitialDepth; i <= dataSet.FinalDepth; i++)
            {
                CurrentObject.DepthAndCurves.Add(new DepthCurveInfo
                {
                    Depth = i
                });
            }
        }

        public ICommand DataGridSelectionChangedCommand
        {
            get
            {
                return _dataGridSelectionChangedCommand ??
                    (_dataGridSelectionChangedCommand = new RelayCommand(DataGridSelectionChanged));
            }
        }

        public bool IsAutoRowsEnabled { get; set; }

        public bool IsAutoRowsDisabled { get; set; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                NotifyPropertyChanged("SelectedIndex");
            }
        }

        public ICommand ExportCommand
        {
            get { return _exportCommand ?? (_exportCommand = new RelayCommand(Export)); }
        }

        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(Delete)); }
        }

        public List<DepthCurveInfo> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                NotifyPropertyChanged("SelectedItems");
            }
        }

        public ICommand UndoCommand
        {
            get { return _undoCommand ?? (_undoCommand = new RelayCommand(Undo, () => _undoStack.Count > 0)); }
        }

        public ICommand RedoCommand
        {
            get { return _redoCommand ?? (_redoCommand = new RelayCommand(Redo, () => _redoStack.Count > 0)); }
        }

        public ICommand AutomaticDepthScreenCommand
        {
            get
            {
                return _automaticDepthScreenCommand ??
                    (_automaticDepthScreenCommand = new RelayCommand(AutomaticDepthScreen));
            }
        }

        public ICommand CollectionChangedCommand
        {
            get
            {
                return _collectionChangedCommand ??
                    (_collectionChangedCommand = new RelayCommand<NotifyCollectionChangedEventArgs>(CollectionChanged));
            }
        }

        public ICommand CopyCommand
        {
            get { return _copyCommand ?? (_copyCommand = new RelayCommand<bool>(u => CutCopyData(false))); }
        }

        public ICommand CutCommand
        {
            get { return _cutCommand ?? (_cutCommand = new RelayCommand<bool>(u => CutCopyData(true))); }
        }

        public ICommand IncreaseDecimalCommand
        {
            get { return _increaseDecimalCommand ?? (_increaseDecimalCommand = new RelayCommand(IncreaseDecimal)); }
        }

        public ICommand DecreaseDecimalCommand
        {
            get { return _decreaseDecimalCommand ?? (_decreaseDecimalCommand = new RelayCommand(DecreaseDecimal)); }
        }

        public ICommand RowEditEndingCommand
        {
            get { return _rowEditEndingCommand ?? (_rowEditEndingCommand = new RelayCommand(RowEditEnding)); }
        }

        public ICommand PasteCommand
        {
            get { return _pasteCommand ?? (_pasteCommand = new RelayCommand(Paste)); }
        }

        private void Delete()
        {
            if (SelectedItems == null || !SelectedItems.Any()) return;
            AddToUndoStack();

            var lst = CurrentObject.DepthAndCurves.ToList();
            SelectedItems.ForEach(u => lst.Remove(u));

            CurrentObject.DepthAndCurves = new ObservableCollection<DepthCurveInfo>(lst.ToList());
            AttachEventToListOfItems();
            NotifyPropertyChanged("CurrentObject");
        }

        IList _selectedItemsList;
        public IList SelectedItemsList
        {
            get { return _selectedItemsList; }
            set
            {
                _selectedItemsList = value;
                DataGridSelectionChanged();
                SelectedItems = new List<DepthCurveInfo>();
                foreach (var item in SelectedItemsList)
                {
                    SelectedItems.Add(item as DepthCurveInfo);
                }
                NotifyPropertyChanged("SelectedItemsList");
            }
        }
        private void DataGridSelectionChanged()
        {
            //the below line making this screen behave weird on second row of the grid
           // if (SelectedIndex == 1 || !IsAutoRowsEnabled) return;
            if (!IsAutoRowsEnabled) return;
            int totalCount = CurrentObject.DepthAndCurves.Count;
            decimal maxNumber = 0;
            if (CurrentObject.DepthAndCurves.Any())
               maxNumber = CurrentObject.DepthAndCurves.Max(u => u.Depth);
            //https://trello.com/c/OEf3xy0f
            //if (maxNumber + OriginalObject.Step >= OriginalObject.FinalDepth)
            //    return;

            if (SelectedIndex + 1 != totalCount && SelectedIndex != totalCount) return;
            var item = new DepthCurveInfo
            {
                Depth = maxNumber + OriginalObject.Step
            };
            CurrentObject.DepthAndCurves.Add(item);
        }
        private void Undo()
        {
            AddToRedoStack();
            CurrentObject.DepthAndCurves = new ObservableCollection<DepthCurveInfo>(_undoStack.Pop());
            AttachEventToListOfItems();
            NotifyPropertyChanged("CurrentObject");
        }
        private void Redo()
        {
            AddToUndoStack();
            CurrentObject.DepthAndCurves = new ObservableCollection<DepthCurveInfo>(_redoStack.Pop());
            AttachEventToListOfItems();
            NotifyPropertyChanged("CurrentObject");
        }
        private void CollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                AddToUndoStack();
        }

        private void AutomaticDepthScreen()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.OpenAutomaticDepthScreen);
        }

        private void Export()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.SpreadsheetExcel);
        }

        private void AddToUndoStack()
        {
            DepthCurveInfo[] array = new DepthCurveInfo[CurrentObject.DepthAndCurves.Count];
            List<DepthCurveInfo> testItem = new List<DepthCurveInfo>();
            Mapper.Map(CurrentObject.DepthAndCurves, testItem);
            _undoStack.Push(testItem);
        }

        protected override bool CanSave()
        {
            return CurrentObject.DepthAndCurves.Any();
        }

        public override void Save()
        {
            if (DepthsShouldBeUnique())
            {
                if (IsNew)
                    SaveSpreadsheet();
                else
                    UpdateSpreadsheet();
            }
            else
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, "There are duplicate  depths in the collection");
        }

        private bool DepthsShouldBeUnique()
        {
            try
            {
                var dictionary = CurrentObject.DepthAndCurves.ToDictionary(u => u.Depth, u => u.Curve);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private void UpdateSpreadsheet()
        {
            OriginalObject.DepthAndCurves = new ObservableCollection<DepthCurveInfo>(CurrentObject.DepthAndCurves.ToList());
            OriginalObject.InitialDepth = CurrentObject.InitialDepth;
            OriginalObject.FinalDepth = CurrentObject.FinalDepth;
            OriginalObject.Step = CurrentObject.Step;
            //if this dataset is part of the curve which is being used in the selected dataset then update the charts
            var curves = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.Tracks.SelectMany(u => u.TrackObject.Curves);
            curves = curves.Where(u => u.RefDataset == CurrentObject.ID && u.RefProject == CurrentObject.RefProject && u.RefWell == CurrentObject.RefWell);
            if (curves.Any())
            {
                curves = curves.Where(u => u.RefChart == IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject.ID);
                foreach (Curve curve in curves.ToList())
                {
                    var curveToShow = GlobalDataModel.GetCurveToShowByCurve(curve);
                    if (curveToShow != null)
                    {

                        GlobalDataModel.Instance.AddDataseriesInformationToCurve(CurrentObject, curveToShow);
                        GlobalDataModel.Instance.StylingOfScaleControl(CurrentObject, curveToShow);
                    }
                }
            }
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        private void SaveSpreadsheet()
        {
            CurrentObject.DepthAndCurves = new ObservableCollection<DepthCurveInfo>(CurrentObject.DepthAndCurves.ToList());

            if (IsNew)
            {
                var wellObject = HelperMethods.Instance.GetWellByID(CurrentObject.RefWell);
                var lst = GlobalDataModel.Instance.GetValidListOfDepthAndCurves(CurrentObject.DepthAndCurves);
                CurrentObject.SystemNotes = GlobalDataModel.Instance.GetNotesFirstAndLastValidData(lst);
                wellObject.Datasets.Add(CurrentObject);
            }
            else
            {
                if (OriginalObject != null)
                    Mapper.Map(CurrentObject, OriginalObject);
            }

            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.SaveSpreadsheet);
        }

        private void AddToRedoStack()
        {
            Contract.Requires(CurrentObject.DepthAndCurves != null, "While Redo, ListOfItems should not be null");
            DepthCurveInfo[] array = new DepthCurveInfo[CurrentObject.DepthAndCurves.Count];
            CurrentObject.DepthAndCurves.CopyTo(array, 0);

            _redoStack.Push(new List<DepthCurveInfo>(array.ToList()));
        }

        private void CutCopyData(bool isCut = false)
        {
            if (SelectedItems == null || !SelectedItems.Any())
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "No row has been selected for the operation");
                return;
            }
            StringBuilder sbuilder = new StringBuilder();
            SelectedItems.ForEach(u => sbuilder.Append(u.Depth.ToString() + "," + u.Curve.ToString() + "\n"));

            if (isCut)
            {
                AddToUndoStack();
                SelectedItems.ForEach(u => CurrentObject.DepthAndCurves.Remove(u));
                NotifyPropertyChanged("ListOfItems");
            }

            Clipboard.SetData(DataFormats.CommaSeparatedValue, sbuilder.ToString());
        }

        private void IncreaseDecimal()
        {
            if (SelectedItems != null && SelectedItems.Any())
            {
                SelectedItems.ForEach(u => u.Curve = Decimal.Multiply(u.Curve, 1.0m));
            }
            else
            {
                IoC.Kernel.Get<ISendMessage>().MessageBox
                    (IoC.Kernel.Get<IResourceHelper>().ReadResource("NoRowHaveBeenSelected"), Token);
            }
        }
        private void RowEditEnding()
        {
            AddToUndoStack();
        }
        private void DecreaseDecimal()
        {
            if (SelectedItems != null && SelectedItems.Any())
            {
                foreach (var item in SelectedItems)
                {
                    if (item.Curve != Decimal.Truncate(item.Curve))
                    {
                        string lastdigit = item.Curve.ToString();
                        lastdigit = lastdigit.Remove(lastdigit.Length - 1);
                        item.Curve = Convert.ToDecimal(lastdigit);
                    }
                    else
                    {
                        string lastdigit = item.Curve.ToString();
                        if (lastdigit.EndsWith("0"))
                        {
                            lastdigit = lastdigit.Remove(lastdigit.Length - 1);
                            decimal decVal = 0;
                            Decimal.TryParse(lastdigit.ToString(), out decVal);
                            item.Curve = decVal;
                        }
                    }
                }
            }
            else
                IoC.Kernel.Get<ISendMessage>().MessageBox(IoC.Kernel.Get<IResourceHelper>().ReadResource("NoRowHaveBeenSelected"), Token);
        }

        private void Paste()
        {
            if (!ValidatePasteProcess()) return;

            var commaSeparatedObject = Clipboard.GetData(DataFormats.CommaSeparatedValue);

            if (commaSeparatedObject == null) return;
            string[] parse1;

            parse1 = commaSeparatedObject.ToString().Split('\n');
            int i = 1;
            //Change next line, to fix:
            //  Paste (from Exceln) in New Dataset when the grid is empty is not working.
            //decimal maxValue = CurrentObject.DepthAndCurves.Max(u => u.Depth);
            //
            //Chnge a variable unused (maxValue) to fix issue:
            //  Pasting a Depth or Unit value that is not valid (lets say a letter, or anything that is not a number)
            bool eachRowDecValidator = true;

            string _dirtyValues = string.Empty;

            Task task = Task.Factory.StartNew(() => LoopThroughEachRowWhilePasting(parse1, ref eachRowDecValidator, ref _dirtyValues));

            task.ContinueWith((u) =>
            {
                //We need to check if each row have valids decimals
                if (!ValidateEachRowDecimalPasting(eachRowDecValidator, _dirtyValues))
                    return;
                if (!ValidateDuplicateDepthWhilePasting(_dirtyValues))
                    return;
                AddToUndoStack();
                UpdatePastingInDataGrid(parse1, i);
                CurrentObject.DepthAndCurves = new ObservableCollection<DepthCurveInfo>(CurrentObject.DepthAndCurves.ToList());
                NotifyPropertyChanged("ListOfItems");
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void UpdatePastingInDataGrid(string[] clipboardData, int i)
        {
            string[] lineToRead, columnsToRead;
            foreach (string item in clipboardData)
            {
                lineToRead = item.Split('\r');
                if (string.IsNullOrWhiteSpace(lineToRead[0])) continue;
                columnsToRead = lineToRead[0].Split(',');
                decimal decVal = 0;
                if (!decimal.TryParse(columnsToRead[0], out decVal)) continue;
                DepthCurveInfo obj = new DepthCurveInfo
                {
                    Depth = Convert.ToDecimal(columnsToRead[0]),
                    Curve = Convert.ToDecimal(columnsToRead[1])
                };

                CurrentObject.DepthAndCurves.Insert(SelectedIndex, obj);
                i++;
            }
        }

        private bool ValidateEachRowDecimalPasting(bool eachRowDecValidator, string dirtyValues)
        {
            if (!eachRowDecValidator)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBox
                (string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("EachRowDecimalPasting"), dirtyValues), Token);
                return false;
            }
            return true;
        }

        private bool ValidateDuplicateDepthWhilePasting(string dirtyValues)
        {
            string _dirtyValues;
            if (string.IsNullOrWhiteSpace(dirtyValues)) return true;
            _dirtyValues = dirtyValues.Substring(2);
            IoC.Kernel.Get<ISendMessage>().MessageBox
                (string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("DuplicateDepthWhilePasting"), _dirtyValues), Token);
            return false;
        }

        private void LoopThroughEachRowWhilePasting(string[] parse1, ref bool eachRowDecValidator, ref string _dirtyValues)
        {
            string[] lineToRead;
            string[] columnsToRead;
            foreach (string item in parse1)
            {
                lineToRead = item.Split('\r');
                if (string.IsNullOrWhiteSpace(lineToRead[0])) continue;
                columnsToRead = lineToRead[0].Split(',');
                decimal decVal = 0;
                decimal decValTemp = 0;
                if (string.IsNullOrWhiteSpace(columnsToRead[0])) continue;
                if (string.IsNullOrWhiteSpace(columnsToRead[1])) continue;
                if (!decimal.TryParse(columnsToRead[0], out decVal))
                {
                    eachRowDecValidator = false;
                    _dirtyValues = lineToRead[0];
                    return;
                }
                if (!decimal.TryParse(columnsToRead[1], out decValTemp))
                {
                    eachRowDecValidator = false;
                    _dirtyValues = lineToRead[0];
                    return;
                }
                if (CurrentObject.DepthAndCurves.Any(u => u.Depth == decVal))
                {
                    _dirtyValues = _dirtyValues + ", " + decVal;
                }
            }
        }

        private bool ValidatePasteProcess()
        {
            if (SelectedIndex != -1) return true;
            IoC.Kernel.Get<ISendMessage>().MessageBox(
                IoC.Kernel.Get<IResourceHelper>().ReadResource("RowMustBeSelectedBeforePasting"), Token);
            return false;
        }
    }//end class
}//end namespace
