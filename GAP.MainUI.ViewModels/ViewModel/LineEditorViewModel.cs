using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Ninject;
using AutoMapper;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class LineEditorViewModel : BaseViewModel<BaseEntity>, IDisposable
    {
        public LineEditorViewModel(string token)
            : base(token)
        {
            ListItems = new ObservableCollection<AnnotationErrorInfo>();
            Mapper.CreateMap<LineAnnotationExtended, LineAnnotationExtended>();
        }
        //  public int SourceDatasetStep { get; set; }
        /// <summary>
        /// This method is being called every time this screen gets opened as constructor fires only once in the application
        /// life cycle
        /// </summary>
        public void Refresh()
        {
            CanEditFreely = false;
            CanEditLineSegment = false;
            CanAddLineSegment = false;
            CanDeleteLineSegment = false;
        }
        //public void RefreshUponChartSelectionChanged()
        //{
        //    if (CanEditLineSegment)
        //    {
        //        CanEditLineSegment = true; //this will bind the event handlers for newly selected chart
        //        return;
        //    }
        //    if (CanAddLineSegment)
        //    {
        //        CanAddLineSegment = true;
        //        return;
        //    }
        //    if (CanDeleteLineSegment)
        //    {
        //        CanDeleteLineSegment = true;
        //        return;
        //    }
        //    if (CanEditFreely)
        //    {
        //        CanEditFreely = true;
        //        return;
        //    }
        //}


        public void InitializeCurveToShowObjects()
        {
            var curves = HelperMethods.Instance.GetCurvesByDatasetID(SubDataset.Dataset);

            EffectedCurves = GetCurveToShowObjectByCurves(curves).Distinct();
            foreach (var curveToShow in EffectedCurves)
            {
                if (!curveToShow.SubDatasets.Any(u => u.Project == SubDataset.Project &&
                    u.Well == SubDataset.Well && u.Dataset == SubDataset.Dataset && u.Name == SubDataset.Name))
                {
                    curveToShow.SubDatasets.Add(SubDataset);
                }
                curveToShow.CurrentSubDataset = SubDataset;

                curveToShow.AnnotationAdded -= curveToShow_AnnotationAdded;
                curveToShow.AnnotationAdded += curveToShow_AnnotationAdded;
            }
            UndoStack = new Stack<AnnotationRecorder>();
            RedoStack = new Stack<AnnotationRecorder>();
        }

        Stack<AnnotationRecorder> UndoStack { get; set; }
        Stack<AnnotationRecorder> RedoStack { get; set; }

        void curveToShow_AnnotationAdded(AnnotationRecorder annotation, CurveToShow curveToShow, LineAnnotationExtended lineAnnotation)
        {
            UndoStack.Push(annotation);
            var subDataset = HelperMethods.Instance.GetSubDatasetObjectBySubdatasetDetails(SubDataset);
            subDataset.Annotations.Add(annotation.AnnotationInfo);
            //add annotations in other curve to show objects

            var curvesToShow = GlobalDataModel.GetCurveToShowBySubdataset(subDataset);
            var curvesToRemove = new CurveToShow[] { curveToShow };

            curvesToShow = curvesToShow.Except(curvesToRemove);
            foreach (var curveToShowObject in curvesToShow)
            {
                curveToShowObject.AddAnnotationByAnnotationAndSubDatasetObject(annotation.AnnotationInfo, subDataset, curveToShowObject, annotation.LineAnnotation.Id);
            }
        }

        public IEnumerable<CurveToShow> EffectedCurves { get; set; }

        SubDataset _subDataset;
        public SubDataset SubDataset
        {
            get { return _subDataset; }
            set
            {
                _subDataset = value;
                if (value == null) return;
                InitializeCurveToShowObjects();
                NotifyPropertyChanged("SubDataset");
            }
        }

        bool _canAddLineSegment, _canEditLineSegment, _canEditFreely, _canDeleteLineSegment;

        public bool CanAddLineSegment
        {
            get { return _canAddLineSegment; }
            set
            {
                _canAddLineSegment = value;
                if (value)
                {
                    if (CanDeleteLineSegment) CanDeleteLineSegment = false;
                    if (CanEditFreely) CanEditFreely = false;
                    if (CanEditLineSegment) CanEditLineSegment = false;
                }
                AddLineSegment();
                NotifyPropertyChanged("CanAddLineSegment");
            }
        }

        private void AddLineSegment()
        {
            var effectedCurves = EffectedCurves.Where(u => u.CurveObject.RefChart == IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.SelectedChart.ChartObject.ID).ToList();
            foreach (var curveToShow in effectedCurves)
            {
                curveToShow.AnnotationModifier.IsEnabled = CanAddLineSegment;
            }
        }

        private void EnableDisableAnnotations(bool shouldEnable)
        {
            foreach (LineAnnotationExtended annotation in GetEffectedAnnotations())
            {
                annotation.IsEditable = shouldEnable;
                annotation.IsEnabled = shouldEnable;
                annotation.Selected -= annotation_Selected;
                if (shouldEnable)
                {
                    annotation.Selected += annotation_Selected;
                }
            }
        }

        /// <summary>
        /// this method returns all the line annotations which are related to this subset from all the possible curves
        /// </summary>
        /// <returns></returns>
        private IEnumerable<LineAnnotationExtended> GetEffectedAnnotations()
        {
            var annotations = EffectedCurves.Where(u => u.TrackToShowObject.Annotations != null).SelectMany(v => v.TrackToShowObject.Annotations);
            var lineAnnotations = annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended)).Select(v => v as LineAnnotationExtended);
            lineAnnotations = lineAnnotations.Where(u => u.SubDataset != null);
            return lineAnnotations.Where(u => u.SubDataset.Name == SubDataset.Name
                && u.SubDataset.Project == SubDataset.Project && u.SubDataset.Well == SubDataset.Well && u.SubDataset.IsNCT == SubDataset.IsNCT);
        }

        private TrackToShow GetTrackToShowByLineAnnotation(LineAnnotationExtended lineAnnotation)
        {
            var tracksToShow = EffectedCurves.Select(u => u.TrackToShowObject);
            return tracksToShow.SingleOrDefault(u => u.Annotations.Contains(lineAnnotation));
        }

        decimal _originalSlope;
        private decimal CalculateSlope(LineAnnotationExtended lineAnnotation)
        {
            //formular to get the slope is y=mx+b
            //formala to get m is (y2-y1)/(x2-x1)
            var y1 = decimal.Parse(lineAnnotation.Y1.ToString());
            var y2 = decimal.Parse(lineAnnotation.Y2.ToString());
            var x1 = decimal.Parse(lineAnnotation.X1.ToString());
            var x2 = decimal.Parse(lineAnnotation.X2.ToString());

            var slope = (y2 - y1) / (x2 - x1);
            return decimal.Parse(slope.ToString("0.000"));
        }

        public bool ShouldDeleteLine { get; set; }
        void annotation_Selected(object sender, EventArgs e)
        {

            var lineAnnotation = sender as LineAnnotationExtended;

            if (CanEditLineSegment)
            {
                EditLineSegment(lineAnnotation);
            }
            else if (CanEditFreely)
            {
                EditLineFreely(lineAnnotation);
            }
            else if (CanDeleteLineSegment)
            {
                DeleteLineSegment(lineAnnotation);
            }
        }

        private void DeleteLineSegment(LineAnnotationExtended lineAnnotation)
        {
            ShouldDeleteLine = false;
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.ShouldDeleteLine, this);
            if (!ShouldDeleteLine) return;

            var tracksToShow = EffectedCurves.Select(u => u.TrackToShowObject);

            RemoveLineAnnotationFromTracksToShow(lineAnnotation, tracksToShow);

            var trackToShow = tracksToShow.First();

            var annotationRecorder = GetAnnotationRecorderObjectFromLineAnnotation(lineAnnotation, trackToShow);

            var annotationToRemove = SubDataset.Annotations.SingleOrDefault(u => u.ID == lineAnnotation.Id);
            if (annotationToRemove != null)
            {
                var subDataset = HelperMethods.Instance.GetSubDatasetObjectBySubdatasetDetails(SubDataset);
                subDataset.Annotations.Remove(annotationToRemove);
            }
            UndoStack.Push(annotationRecorder);
        }

        private static AnnotationRecorder GetAnnotationRecorderObjectFromLineAnnotation(LineAnnotationExtended lineAnnotation, TrackToShow trackToShow)
        {
            AnnotationInfo info = new AnnotationInfo
            {
                X1 = lineAnnotation.X1.ToString(),
                X2 = lineAnnotation.X2.ToString(),
                Y1 = lineAnnotation.Y1.ToString(),
                Y2 = lineAnnotation.Y2.ToString()
            };
            var annotationRecorder = new AnnotationRecorder
            {
                AnnotationInfo = info,
                TrackToShow = trackToShow,
                ActionPerformed = ActionPerformed.ItemDeleted,
                LineAnnotation = lineAnnotation
            };
            return annotationRecorder;
        }

        private void RemoveLineAnnotationFromTracksToShow(LineAnnotationExtended lineAnnotation, IEnumerable<TrackToShow> tracksToShow)
        {
            foreach (var obj in tracksToShow)
            {
                var lines = obj.Annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended)).Select(u => u as LineAnnotationExtended);
                var lineToRemove = lines.SingleOrDefault(u => u.Id == lineAnnotation.Id);
                if (lineToRemove == null) continue;
                lineToRemove.Selected -= annotation_Selected;
                obj.Annotations.Remove(lineToRemove);
            }
        }

        Dictionary<string, IDisposable> _dicAnnotation = new Dictionary<string, IDisposable>();
        private void EditLineFreely(LineAnnotationExtended lineAnnotation)
        {
            if (_dicAnnotation.ContainsKey(lineAnnotation.Id))
            {
                _dicAnnotation[lineAnnotation.Id].Dispose();
                _dicAnnotation.Remove(lineAnnotation.Id);
            }

            var disposable = Observable.FromEventPattern<SizeChangedEventArgs>(lineAnnotation, "SizeChanged").Subscribe(u =>
                {
                    annotation_SizeChanged(u.Sender, u.EventArgs);
                });

            _dicAnnotation.Add(lineAnnotation.Id, disposable);

            lineAnnotation.LineDraggingEnded -= lineAnnotation_LineDraggingEnded;
            lineAnnotation.LineDraggingEnded += lineAnnotation_LineDraggingEnded;
        }

        private void EditLineSegment(LineAnnotationExtended lineAnnotation)
        {
            _originalSlope = CalculateSlope(lineAnnotation);

            if (_dicAnnotation.ContainsKey(lineAnnotation.Id))
            {
                _dicAnnotation[lineAnnotation.Id].Dispose();
                _dicAnnotation.Remove(lineAnnotation.Id);
            }

            var disposable = Observable.FromEventPattern<SizeChangedEventArgs>(lineAnnotation, "SizeChanged").Subscribe(u =>
            {
                annotation_SizeChanged(u.Sender, u.EventArgs);
            });

            _dicAnnotation.Add(lineAnnotation.Id, disposable);


            lineAnnotation.SlopeNotValid -= lineAnnotation_SlopeNotValid;
            lineAnnotation.SlopeNotValid += lineAnnotation_SlopeNotValid;

            lineAnnotation.LineDraggingEnded -= lineAnnotation_LineDraggingEnded;
            lineAnnotation.LineDraggingEnded += lineAnnotation_LineDraggingEnded;
        }

        void lineAnnotation_LineDraggingEnded(LineAnnotationExtended sender)
        {
            //at this point we need to put it in undo redo stack       
            var currentLine = SubDataset.Annotations.SingleOrDefault(u => u.ID == sender.Id);

            var lineAnnotation = GetEffectedAnnotations().Where(u => u.Id == currentLine.ID).FirstOrDefault();
            var originalLine = HelperMethods.GetNewObject<LineAnnotationExtended>(lineAnnotation);
            originalLine.X1 = currentLine.X1;
            originalLine.X2 = currentLine.X2;
            originalLine.Y1 = currentLine.Y1;
            originalLine.Y2 = currentLine.Y2;

            var annotationRecorder = GetAnnotationRecorderObjectFromLineAnnotation(originalLine, GetTrackToShowByLineAnnotation(originalLine));
            annotationRecorder.ActionPerformed = ActionPerformed.ItemUpdated;
            UndoStack.Push(annotationRecorder);
        }

        public bool ShouldFixSlope { get; set; }
        void lineAnnotation_SlopeNotValid(LineAnnotationExtended sender)
        {
            ShouldFixSlope = false;

            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.InvalidSlope);
            if (ShouldFixSlope)
            {
                sender.X2 = ((decimal.Parse(sender.Y2.ToString()) - decimal.Parse(sender.Y1.ToString())) / _originalSlope) + decimal.Parse(sender.X1.ToString());
            }
            else
            {
                var currentLine = SubDataset.Annotations.SingleOrDefault(u => u.ID == sender.Id);
                sender.X2 = currentLine.X2;
                sender.Y2 = currentLine.Y2;
                sender.X1 = currentLine.X1;
                sender.Y1 = currentLine.Y1;
            }
        }

        public IEnumerable<CurveToShow> GetCurveToShowObjectByCurves(IEnumerable<Curve> curves)
        {
            var curvesToShow = IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Where(u => u.Tracks != null).SelectMany(u => u.Tracks)
                .Where(track => track.Curves != null).SelectMany(v => v.Curves);

            return curvesToShow.Where(u => curves.Contains(u.CurveObject));
        }

        public bool CanDeleteLineSegment
        {
            get { return _canDeleteLineSegment; }
            set
            {
                _canDeleteLineSegment = value;
                if (value)
                {
                    if (CanAddLineSegment) CanAddLineSegment = false;
                    if (CanEditFreely) CanEditFreely = false;
                    if (CanEditLineSegment) CanEditLineSegment = false;
                }
                EnableDisableAnnotations(value);
                NotifyPropertyChanged("CanDeleteLineSegment");
            }
        }

        public bool CanEditFreely
        {
            get { return _canEditFreely; }
            set
            {
                _canEditFreely = value;
                if (value)
                {
                    if (CanAddLineSegment) CanAddLineSegment = false;
                    if (CanDeleteLineSegment) CanDeleteLineSegment = false;
                    if (CanEditLineSegment) CanEditLineSegment = false;
                }
                EnableDisableAnnotations(value);
                NotifyPropertyChanged("CanEditFreely");
            }
        }

        void annotation_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            var currentLine = sender as LineAnnotationExtended;
            if (string.IsNullOrWhiteSpace(currentLine.Id))
                throw new Exception("Line annotation does not have unique name");
            if (CanEditLineSegment)
            {
                var slope = CalculateSlope(currentLine);
                currentLine.ActualSlope = _originalSlope;
                currentLine.CurrentSlope = slope;
                currentLine.EditLineSegment = true;
                currentLine.IsValidSlope = slope == _originalSlope;
                // return;  //comment this return because lines on another  annotations are not taking effect as this following code is not being executed
            }
            var annotations = GetEffectedAnnotations().Where(u => u.Id == currentLine.Id);
            var linesToExcept = new LineAnnotationExtended[] { currentLine };
            foreach (var annotation in annotations.Except(linesToExcept))
            {
                annotation.X1 = currentLine.X1;
                annotation.X2 = currentLine.X2;
                annotation.Y1 = currentLine.Y1;
                annotation.Y2 = currentLine.Y2;
            }
        }

        public bool CanEditLineSegment
        {
            get { return _canEditLineSegment; }
            set
            {
                _canEditLineSegment = value;
                if (value)
                {
                    if (CanAddLineSegment) CanAddLineSegment = false;
                    if (CanDeleteLineSegment) CanDeleteLineSegment = false;
                    if (CanEditFreely) CanEditFreely = false;
                }
                EnableDisableAnnotations(value);
                NotifyPropertyChanged("CanEditLineSegment");
            }
        }

        ICommand _undoCommand, _redoCommand;
        public ICommand UndoCommand
        {
            get { return _undoCommand ?? (_undoCommand = new RelayCommand(Undo, () => CanUndo())); }
        }

        private void Undo()
        {
            AnnotationRecorder annotationObject = UndoStack.Pop();

            switch (annotationObject.ActionPerformed)
            {
                case ActionPerformed.ItemAdded:
                    AddWhileUndoing(annotationObject);
                    break;
                case ActionPerformed.ItemDeleted:
                    DeleteWhileUndoing(annotationObject);
                    break;
                case ActionPerformed.ItemUpdated:
                    UpdateWhileUndoing(annotationObject);
                    break;
            }

            RedoStack.Push(annotationObject);
        }

        private void UpdateWhileUndoing(AnnotationRecorder annotationObject)
        {
            var annotationToStore = new LineAnnotationExtended();

            foreach (var curveToShowObject in EffectedCurves)
            {
                var annotations = curveToShowObject.TrackToShowObject.Annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended)).Select(v => v as LineAnnotationExtended);
                annotations = annotations.Where(u => u.SubDataset != null);

                var targetAnnotation = annotations.SingleOrDefault(u => u.Id == annotationObject.LineAnnotation.Id);
                annotationToStore = HelperMethods.GetNewObject<LineAnnotationExtended>(targetAnnotation);

                targetAnnotation.X1 = annotationObject.LineAnnotation.X1;
                targetAnnotation.X2 = annotationObject.LineAnnotation.X2;
                targetAnnotation.Y1 = annotationObject.LineAnnotation.Y1;
                targetAnnotation.Y2 = annotationObject.LineAnnotation.Y2;
            }

            annotationObject.LineAnnotation = annotationToStore;
        }

        private void DeleteWhileUndoing(AnnotationRecorder annotationObject)
        {
            foreach (var curveToShowObject in EffectedCurves)
            {
                curveToShowObject.AddAnnotationByAnnotationAndSubDatasetObject
                    (annotationObject.AnnotationInfo, SubDataset, curveToShowObject, annotationObject.LineAnnotation.Id);
            }
            annotationObject.ActionPerformed = ActionPerformed.ItemAdded;
        }

        private void AddWhileUndoing(AnnotationRecorder annotationObject)
        {
            var tracksToShow = EffectedCurves.Select(u => u.TrackToShowObject);
            foreach (var obj in tracksToShow)
            {
                var lineAnnotation = annotationObject.LineAnnotation;
                var lines = obj.Annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended)).Select(u => u as LineAnnotationExtended);
                var lineToRemove = lines.SingleOrDefault(u => u.Id == lineAnnotation.Id);
                if (lineToRemove == null) continue;
                lineToRemove.Selected -= annotation_Selected;
                obj.Annotations.Remove(lineToRemove);
            }
            annotationObject.ActionPerformed = ActionPerformed.ItemDeleted;
        }

        private void Redo()
        {
            var tracksToShow = EffectedCurves.Select(u => u.TrackToShowObject);

            AnnotationRecorder annotationObject = RedoStack.Pop();
            switch (annotationObject.ActionPerformed)
            {
                case (ActionPerformed.ItemAdded):
                    AddWhileUndoing(annotationObject);
                    break;
                case (ActionPerformed.ItemDeleted):
                    DeleteWhileUndoing(annotationObject);
                    break;
                case (ActionPerformed.ItemUpdated):
                    UpdateWhileUndoing(annotationObject);
                    break;
            }
            UndoStack.Push(annotationObject);
        }

        public ICommand RedoCommand
        {
            get { return _redoCommand ?? (_redoCommand = new RelayCommand(Redo, () => CanRedo())); }
        }

        private bool CanUndo()
        {
            return UndoStack != null && UndoStack.Any();
        }

        private bool CanRedo()
        {
            return RedoStack != null && RedoStack.Any();
        }
        private void FixErrors()
        {
            if (EffectedCurves == null || !EffectedCurves.Any()) return;
            foreach (var curve in EffectedCurves)
            {
                SortedList<double, LineAnnotationExtended> lst;
                if (!GetAnnotationsToValidateByCurve(curve, out lst)) return;

                StringBuilder sBuilder = new StringBuilder();
                for (int i = 1; i < lst.Count; i++)
                {
                    var previousLine = lst.ElementAt(i - 1).Value as LineAnnotationExtended;
                    var currentLine = lst.ElementAt(i).Value as LineAnnotationExtended;

                    if (decimal.Parse(previousLine.Y2.ToString()) + SubDataset.Step > decimal.Parse(currentLine.Y1.ToString()))
                    {
                        if (_errorInfo.ErrorID == i)
                            currentLine.Y1 = decimal.Parse(previousLine.Y2.ToString()) + SubDataset.Step;
                    }
                    if (decimal.Parse(currentLine.Y1.ToString()) > decimal.Parse(previousLine.Y2.ToString()) + SubDataset.Step)
                    {
                        if (_errorInfo.ErrorID == i)
                            currentLine.Y1 = decimal.Parse(previousLine.Y2.ToString()) + SubDataset.Step;
                    }
                }
            }
        }

        private bool GetAnnotationsToValidateByCurve(CurveToShow curve, out SortedList<double, LineAnnotationExtended> lst)
        {
            var annotations = curve.TrackToShowObject.Annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended)).Select(v => v as LineAnnotationExtended);
            annotations = annotations.Where(u => u.SubDataset != null);
            var lineAnnotations = annotations.Where(u => u.SubDataset.Dataset == SubDataset.Dataset &&
                u.SubDataset.Project == SubDataset.Project && u.SubDataset.Well == SubDataset.Well &&
                u.SubDataset.Name == SubDataset.Name && u.SubDataset.IsNCT == SubDataset.IsNCT);
            lst = new SortedList<double, LineAnnotationExtended>();

            foreach (var lineAnnotation in lineAnnotations)
            {
                if (!lst.ContainsKey(double.Parse(lineAnnotation.Y1.ToString())))
                    lst.Add(double.Parse(lineAnnotation.Y1.ToString()), lineAnnotation);
                else
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token,
                        IoC.Kernel.Get<IResourceHelper>().ReadResource("ParallelLineInSystem"));
                    return false;
                }
            }

         //   if (lst.Count < 2) return false;
            return true;
        }

        private bool IsValidated()
        {
            if (EffectedCurves == null || !EffectedCurves.Any()) return false;
            ListItems.Clear();
            CurveToShow curve = EffectedCurves.First();
            SortedList<double, LineAnnotationExtended> lst;

            if (!GetAnnotationsToValidateByCurve(curve, out lst)) return false;

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 1; i < lst.Count; i++)
            {
                var previousLine = lst.ElementAt(i - 1).Value as LineAnnotationExtended;
                var currentLine = lst.ElementAt(i).Value as LineAnnotationExtended;

                if (decimal.Parse(previousLine.Y2.ToString()) + SubDataset.Step > decimal.Parse(currentLine.Y1.ToString()))
                {
                    if (decimal.Parse(previousLine.Y2.ToString()) + SubDataset.Step > decimal.Parse(currentLine.Y2.ToString()))
                    {
                        IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token,
                            IoC.Kernel.Get<IResourceHelper>().ReadResource("ParallelLineInSystem"));
                        ListItems.Clear();
                        return false;
                    }
                    ListItems.Add(new AnnotationErrorInfo
                    {
                        ErrorID = i,
                        ErrorDescription = string.Format(
                            IoC.Kernel.Get<IResourceHelper>().ReadResource("OverlappedLine"), i + 1, i)
                    });
                }
                if (decimal.Parse(currentLine.Y1.ToString()) > decimal.Parse(previousLine.Y2.ToString()) + SubDataset.Step)
                {
                    ListItems.Add(new AnnotationErrorInfo
                    {
                        ErrorID = i,
                        ErrorDescription = string.Format(
                        IoC.Kernel.Get<IResourceHelper>().ReadResource("GappedLine"), i, i + 1)
                    });
                }
            }
            return !ListItems.Any();
        }

        ICommand _fixCommand;
        public ICommand FixCommand
        {
            get { return _fixCommand ?? (_fixCommand = new RelayCommand<AnnotationErrorInfo>(FixError)); }
        }

        AnnotationErrorInfo _errorInfo;

        private void FixError(AnnotationErrorInfo errorInfo)
        {
            _errorInfo = errorInfo;
            FixErrors();
            IsValidated();
        }

        ObservableCollection<AnnotationErrorInfo> _errors;
        public ObservableCollection<AnnotationErrorInfo> ListItems
        {
            get { return _errors; }
            set
            {
                _errors = value;
                NotifyPropertyChanged("ListItems");
            }
        }

        public override void Save()
        {
            if (!IsValidated())
            {
                NotifyPropertyChanged("ListItems");
                return;
            }
            UpdateAllAnnotationObjectsWithCurrentValues();
            CreateSubDataset();
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CancelLineEditor);
        }

        private void CreateSubDataset()
        {
            foreach (var annotation in SubDataset.Annotations)
            {
                decimal x1, x2, y1, y2;
                x1 = decimal.Parse(annotation.X1);
                x2 = decimal.Parse(annotation.X2);
                y1 = decimal.Parse(annotation.Y1);
                y2 = decimal.Parse(annotation.Y2);

                var m = (x2 - x1) / (y2 - y1);
                decimal initialCurve = x1;
                for (decimal i = y1; i < y2; i += SubDataset.Step)
                {
                    SubDataset.DepthAndCurves.Add(new DepthCurveInfo
                    {
                        Depth = Int32.Parse(Math.Round(i).ToString()),
                        Curve = decimal.Parse(initialCurve.ToString("0.##"))
                    });
                    initialCurve += m;
                }
            }
            var lst = GlobalDataModel.Instance.GetValidListOfDepthAndCurves(SubDataset.DepthAndCurves);
            SubDataset.SystemNotes = GlobalDataModel.Instance.GetNotesFirstAndLastValidData(lst) +  Environment.NewLine + SubDataset.SystemNotes;
        }

        private void UpdateAllAnnotationObjectsWithCurrentValues()
        {
            var subset = HelperMethods.Instance.GetSubDatasetObjectBySubdatasetDetails(SubDataset);
            if (subset == null) return;
            subset.Annotations.Clear();
            var curve = EffectedCurves.First();

            IEnumerable<LineAnnotationExtended> annotations = curve.TrackToShowObject.Annotations.Where(u => u.GetType() == typeof(LineAnnotationExtended)).Select(v => (LineAnnotationExtended)v);
            annotations = annotations.Where(u => u.SubDataset != null);
            annotations = annotations.Where(u => u.SubDataset.Project == SubDataset.Project
                && u.SubDataset.Well == SubDataset.Well && SubDataset.Dataset == u.SubDataset.Dataset && u.SubDataset.Name == SubDataset.Name);

            if (annotations == null || !annotations.Any())
            {
                var dataset = HelperMethods.Instance.GetDatasetByID(subset.Dataset);
                dataset.SubDatasets.Remove(subset);
                return;
            }
            foreach (var annotation in annotations)
            {
                subset.Annotations.Add(new AnnotationInfo
                {
                    X1 = annotation.X1.ToString(),
                    X2 = annotation.X2.ToString(),
                    Y1 = annotation.Y1.ToString(),
                    Y2 = annotation.Y2.ToString(),
                    SubDatasetName = subset.Name,
                    ID = annotation.Id
                });
            }
        }

        protected override bool CanSave()
        {
            return true;
        }

        public bool ShouldCancel { get; set; }
        public override void Cancel()
        {
            ShouldCancel = false;

            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.ShouldCancel, this);

            if (!ShouldCancel) return;
            while (UndoStack.Count > 0)
            {
                Undo();
            }

            if (!GetEffectedAnnotations().Any())
            {
                var subdataset = HelperMethods.Instance.GetSubDatasetObjectBySubdatasetDetails(SubDataset);
                if (subdataset != null)
                {
                    var curves = HelperMethods.Instance.GetCurvesByDatasetID(SubDataset.Dataset);

                    EffectedCurves = GetCurveToShowObjectByCurves(curves).Distinct();
                    foreach (var curveToShow in EffectedCurves)
                    {
                        if (curveToShow.SubDatasets.Contains(subdataset))
                            curveToShow.SubDatasets.Remove(subdataset);
                    }
                    var dataset = HelperMethods.Instance.GetDatasetByID(subdataset.Dataset);
                    dataset.SubDatasets.Remove(subdataset);
                }
            }
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CancelLineEditor);
        }

        public void Dispose()
        {
            foreach (var obj in _dicAnnotation.ToList())
            {
                obj.Value.Dispose();
                _dicAnnotation.Remove(obj.Key);
            }
        }
    }//end class
}//end namespace
