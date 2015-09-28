using GalaSoft.MvvmLight.Command;
using GAP.BL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Ninject;
using GAP.Helpers;
using System.Linq;
using GAP.MainUI.ViewModels.Helpers;
using System.Text;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Controls;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class EquationViewModel : DatasetBaseViewModel
    {
        IEnumerable<Dataset> _allDatasets;
        public EquationViewModel(string token)
            : base(token)
        {
            CurrentObject = new Dataset();
            Operators = new List<string>();
            Operators.Add("Addition");
            Operators.Add("Subtraction");
            Operators.Add("Multiplication");
            Operators.Add("Division");
            Operators.Add("Log Number");
            Equations = new ObservableCollection<EquationInfo>();

            Equations.CollectionChanged += Equations_CollectionChanged;
            var newEquation = new EquationInfo(Equations.Count);
            Equations.Add(newEquation);
            newEquation.OperandChanged += equation_OperandChanged;
            _allDatasets = HelperMethods.Instance.GetFilteredDatasets(u => u.Units == "API");
            Operand1 = new List<BaseEntity>();
            foreach (var dataset in _allDatasets)
            {
                var well = HelperMethods.Instance.GetWellByID(dataset.RefWell).Name;
                string name = string.Format("{0}/{1}", well, dataset.Name);
                Operand1.Add(new BaseEntity
                {
                    ID = dataset.ID,
                    Name = name
                });
                Operand2.Add(new BaseEntity
                {
                    ID = dataset.ID,
                    Name = name
                });
            }
            if (Families.Any())
                SelectedFamily = Families.First();
            ViewSpreadsheet = false;
            CalculationContent = new FlowDocument();
        }

        public void SetOperand1(string id)
        {
            SelectedEquation.Operand1 = Operand1.Single(u => u.ID == id);
        }

        public void SetOperand2(string id)
        {
            SelectedEquation.Operand2 = Operand2.Single(u => u.ID == id);
        }

        private bool ValidateCurrentEquation()
        {
            return true;
        }
        string _computedCalculation;
        public string ComputedCalculation
        {
            get { return _computedCalculation; }
            set
            {
                _computedCalculation = value;
                NotifyPropertyChanged("ComputedCalculation");
            }
        }

        FlowDocument _calculationContent;
        public FlowDocument CalculationContent
        {
            get { return _calculationContent; }
            set
            {
                _calculationContent = value;
                NotifyPropertyChanged("CalculationContent");
            }
        }

        List<BaseEntity> _operand1 = new List<BaseEntity>();
        public List<BaseEntity> Operand1
        {
            get { return _operand1; }
            set
            {
                _operand1 = value;
                NotifyPropertyChanged("Operand1");
            }
        }
        List<BaseEntity> _operand2 = new List<BaseEntity>();
        public List<BaseEntity> Operand2
        {
            get { return _operand2; }
            set
            {
                _operand2 = value;
                NotifyPropertyChanged("Operand2");
            }
        }

        ICommand _validateEquationCommand;
        public ICommand ValidateEquationCommand
        {
            get { return _validateEquationCommand ?? (_validateEquationCommand = new RelayCommand<EquationInfo>(ValidateCommand)); }
        }
        SortedDictionary<decimal, decimal> _dicOperand1 = new SortedDictionary<decimal, decimal>();
        SortedDictionary<decimal, decimal> _dicOperand2 = new SortedDictionary<decimal, decimal>();
        SortedDictionary<decimal, decimal> _dicTempOperand2 = new SortedDictionary<decimal, decimal>();
        SortedDictionary<decimal, decimal> _dicTempOperand1 = new SortedDictionary<decimal, decimal>();
        private void UpdateDepthAndCurves(EquationInfo e)
        {
            FillTempList(e);
            _dicOperand1.Clear();
            _dicOperand2.Clear();
            var commonDepths = GetCommonDepths(e);
            foreach (var element in commonDepths)
            {
                decimal operand1 = _dicTempOperand1[element];
                _dicOperand1.Add(element, operand1);

                if (e.Operand2 != null)
                {
                    decimal operand2 = _dicTempOperand2[element];
                    _dicOperand2.Add(element, operand2);
                }
            }
            switch (e.Operator)
            {
                case "Addition":
                    ApplyAddition(e);
                    break;
                case "Subtraction":
                    ApplySubtraction(e);
                    break;
                case "Multiplication":
                    ApplyMultiplication(e);
                    break;
                case "Division":
                    ApplyDivision(e);
                    break;
                case "Log Number":
                    ApplyLog(e);
                    break;
            }
        }

        private void FillTempList(EquationInfo e)
        {
            _dicTempOperand1.Clear();
            _dicTempOperand2.Clear();
            var dataset1 = HelperMethods.Instance.GetDatasetByID(e.Operand1.ID);
            if (dataset1 == null) dataset1 = CurrentObject;
            foreach (var item in dataset1.DepthAndCurves)
                _dicTempOperand1.Add(item.Depth, item.Curve);

            // if (e.Operator == "Log Number") return;
            if (e.Operand2 == null) return;
            var dataset2 = HelperMethods.Instance.GetDatasetByID(e.Operand2.ID);

            foreach (var item in dataset2.DepthAndCurves)
                _dicTempOperand2.Add(item.Depth, item.Curve);
        }
        private bool ProcessCommand(EquationInfo e)
        {
            if (!FirstOperandValidation(e)) return false;
            if (!OperatorsBasicValidations(e)) return false;
            if (e.Operator != "Log Number" && !GetCommonDepths(e).Any())
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "No common depths between these datasets");
                return false;
            }

            UpdateDepthAndCurves(e);
            return true;
        }

        private bool IsFirstOperandValid(EquationInfo equationInfo)
        {
            if (equationInfo.Operand1 == null) return true;
            //only custom operands should be considered
            if (!equationInfo.Operand1.IsEntitySelected.Value) return true;
            if (equationInfo.Operand1.DisplayIndex+1 > equationInfo.SrNo) return false;
            return true;
        }

        private void ValidateCommand(EquationInfo e)
        {
            CurrentObject.DepthAndCurves.Clear();

            foreach (var equation in Equations)
                if (!ProcessCommand(equation)) return;

            AddOperand1ForNextEquation(e);
            if (!Equations.Any(u => u.SrNo > e.SrNo))
            {
                var equation = new EquationInfo(Equations.Count)
                {
                    Operand1=Operand1.Last()
                };
                equation.OperandChanged += equation_OperandChanged;
                Equations.Add(equation);
            }

            ComputedCalculation = string.Empty;
            CalculationContent.Blocks.Clear();

            foreach (var equation in Equations)
                WriteComputedCalculation(equation);

            UpdateSystemNotes();
        }

        private void UpdateSystemNotes()
        {
            var sbuilder = new StringBuilder();
            sbuilder.AppendLine("This dataset has been generated using Equations");
            sbuilder.Append(ComputedCalculation);
            CurrentObject.SystemNotes = sbuilder.ToString();
        }

        private void AddOperand1ForNextEquation(EquationInfo e)
        {
            Operand1.Add(new BaseEntity()
            {
                Name = string.Format("Equation {0}", e.SrNo),
                IsEntitySelected = true,
                DisplayIndex = e.SrNo,
               
            });
        }

        private bool OperatorsBasicValidations(EquationInfo e)
        {
            if (e.Operator != "Log Number" && (e.Operand2 == null || !_allDatasets.Any(u => u.ID == e.Operand2.ID)))
            {
                if (e.RationalNumber == 0)
                {
                    SendErrorMessage("Operand 2 has invalid value");
                    return false;
                }
            }
            if (e.Operator == null)
            {
                SendErrorMessage("Operator must be selected");
                return false;
            }
            if (e.Operand2 == null)
            {
                int baseLogNumber = 0;
                if (!Int32.TryParse(e.RationalNumber.ToString(), out baseLogNumber) || e.RationalNumber == 0)
                {
                    SendErrorMessage("Base Log Number must be selected");
                    return false;
                }
            }
            return true;
        }

        private bool FirstOperandValidation(EquationInfo e)
        {
            if (e.SrNo == 1)
            {
                if (e.Operand1 == null || !_allDatasets.Any(u => u.ID == e.Operand1.ID))
                {
                    SendErrorMessage("Operand 1 has invalid value");
                    return false;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(CurrentObject.Name))
                    {
                        var datasetName = HelperMethods.Instance.GetDatasetByID(e.Operand1.ID).Name;
                        CurrentObject.Name = string.Format("E{0}", datasetName);
                    }
                }
            }
            return true;
        }

        private IEnumerable<decimal> GetCommonDepths(EquationInfo e)
        {
            IEnumerable<decimal> sourceDepths;
            Dataset operand1Dataset, operand2Dataset;
            operand1Dataset = HelperMethods.Instance.GetDatasetByID(e.Operand1.ID);
            if (operand1Dataset == null) operand1Dataset = CurrentObject;

            sourceDepths = GlobalDataModel.Instance.GetValidListOfDepthAndCurves(operand1Dataset.DepthAndCurves).Select(u => u.Depth);
            //if (e.Operator == "Log Number") return sourceDepths;
            if (e.Operand2 == null) return sourceDepths;

            operand2Dataset = HelperMethods.Instance.GetDatasetByID(e.Operand2.ID);

            var destinationDepths = GlobalDataModel.Instance.GetValidListOfDepthAndCurves(operand2Dataset.DepthAndCurves).Select(u => u.Depth);
            var overlappingDepths = sourceDepths.Intersect(destinationDepths);

            return overlappingDepths;
        }

        private void WriteComputedCalculation(EquationInfo e)
        {
            var txt = new TextBlock();
            string currentCalculation = string.Empty;
            switch (e.Operator)
            {
                case "Addition":
                    currentCalculation = AdditionText(e, txt);
                    break;
                case "Multiplication":
                    currentCalculation = MultiplicationText(e, txt);
                    break;
                case "Division":
                    currentCalculation = DivisionText(e, txt);
                    break;
                case "Subtraction":
                    currentCalculation = SubtractionText(e, txt);
                    break;
                case "Log Number":
                    currentCalculation = LogNumberText(e, txt);
                    break;
            }
            if (string.IsNullOrWhiteSpace(txt.Text)) return;
            if (CalculationContent.Blocks.Count == 0)
            {
                var par = new Paragraph();
                par.Inlines.Add(txt);
                CalculationContent.Blocks.Add(par);
            }
            else
            {
                if (e.Operator == "Log Number") return;

                var para = CalculationContent.Blocks.First();
                var inlines = (para as Paragraph).Inlines;
                inlines.Add(txt);
                inlines.InsertBefore(inlines.First(), new InlineUIContainer(new TextBlock
                {
                    Text = "(",
                    Foreground = new SolidColorBrush(e.Color)
                }));
            }
            UpdateSystemNotes();
        }

        private string LogNumberText(EquationInfo e, TextBlock txt)
        {
            string currentCalculation = string.Empty;
            if (string.IsNullOrWhiteSpace(ComputedCalculation))
            {
                currentCalculation = string.Format("Log {1}({0})", e.Operand1.Name, e.RationalNumber);
                txt.Text = currentCalculation;
                txt.Foreground = new SolidColorBrush(e.Color);
                ComputedCalculation = currentCalculation;
            }
            else
            {
                ComputedCalculation = string.Format("Log {1}({0})", ComputedCalculation, e.RationalNumber);

                var parag = CalculationContent.Blocks.First();
                var inlines = (parag as Paragraph).Inlines;
                inlines.Add(txt);
                inlines.InsertBefore(inlines.First(), new InlineUIContainer(new TextBlock
                {
                    Text = "Log" + e.RationalNumber,
                    Foreground = new SolidColorBrush(e.Color)
                }));
                inlines.Add(new TextBlock
                {
                    Foreground = new SolidColorBrush(e.Color),
                    Text = ")"
                });
            }
            return currentCalculation;
        }

        private string SubtractionText(EquationInfo e, TextBlock txt)
        {
            string currentCalculation = string.Empty;
            if (string.IsNullOrWhiteSpace(ComputedCalculation))
            {
                currentCalculation = string.Format("({0} - {1})", e.Operand1.Name, e.Operand2.Name);
                txt.Text = currentCalculation;
                txt.Foreground = new SolidColorBrush(e.Color);
                ComputedCalculation = currentCalculation;
            }
            else
            {
                ComputedCalculation = string.Format("({0}) - {1}", ComputedCalculation, e.Operand2 == null ? e.RationalNumber.ToString() : e.Operand2.Name);
                txt.Text = " - " + (e.Operand2 == null ? e.RationalNumber.ToString() : e.Operand2.Name) + ")";
                txt.Foreground = new SolidColorBrush(e.Color);
            }
            return currentCalculation;
        }

        private string DivisionText(EquationInfo e, TextBlock txt)
        {
            string currentCalculation = string.Empty;
            if (string.IsNullOrWhiteSpace(ComputedCalculation))
            {
                currentCalculation = string.Format("({0} / {1})", e.Operand1.Name, e.Operand2.Name);
                txt.Text = currentCalculation;
                txt.Foreground = new SolidColorBrush(e.Color);
                ComputedCalculation = currentCalculation;
            }
            else
            {
                ComputedCalculation = string.Format("({0}) / {1}", ComputedCalculation, e.Operand2 == null ? e.RationalNumber.ToString() : e.Operand2.Name);
                txt.Text = " / " + (e.Operand2 == null ? e.RationalNumber.ToString() : e.Operand2.Name) + ")";
                txt.Foreground = new SolidColorBrush(e.Color);
            }
            return currentCalculation;
        }

        private string MultiplicationText(EquationInfo e, TextBlock txt)
        {
            string currentCalculation = string.Empty;
            if (string.IsNullOrWhiteSpace(ComputedCalculation))
            {
                currentCalculation = string.Format("({0} * {1})", e.Operand1.Name, e.Operand2.Name);
                txt.Text = currentCalculation;
                txt.Foreground = new SolidColorBrush(e.Color);
                ComputedCalculation = currentCalculation;
            }
            else
            {
                ComputedCalculation = string.Format("({0}) * {1}", ComputedCalculation, e.Operand2 == null ? e.RationalNumber.ToString() : e.Operand2.Name);
                txt.Text = " * " + (e.Operand2 == null ? e.RationalNumber.ToString() : e.Operand2.Name) + ")";
                txt.Foreground = new SolidColorBrush(e.Color);
            }
            return currentCalculation;
        }

        private string AdditionText(EquationInfo e, TextBlock txt)
        {
            string currentCalculation = string.Empty;
            if (string.IsNullOrWhiteSpace(ComputedCalculation))
            {
                currentCalculation = string.Format("({0} + {1})", e.Operand1.Name, e.Operand2 == null ? e.RationalNumber.ToString() : e.Operand2.Name);
                txt.Text = currentCalculation;
                txt.Foreground = new SolidColorBrush(e.Color);
                ComputedCalculation = currentCalculation;
            }
            else
            {
                ComputedCalculation = string.Format("({0}) + {1}", ComputedCalculation, e.Operand2 == null ? e.RationalNumber.ToString() : e.Operand2.Name);
                txt.Text = " + " + (e.Operand2 == null ? e.RationalNumber.ToString() : e.Operand2.Name) + ")";
                txt.Foreground = new SolidColorBrush(e.Color);
            }
            return currentCalculation;
        }

        private void SendErrorMessage(string errorMessage)
        {
            IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, errorMessage);
        }
        void Equations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Equations[Equations.Count - 1].SrNo = Equations.Count;
                    break;
            }
        }
        public ObservableCollection<EquationInfo> Equations { get; set; }

        List<string> _lstOperators = new List<string>();
        public List<string> Operators
        {
            get { return _lstOperators; }
            set
            {
                _lstOperators = value;
                NotifyPropertyChanged("Operators");
            }
        }

        protected override bool AddObjectValidation()
        {
            var well = HelperMethods.Instance.GetWellByID(CurrentObject.RefWell);
            if (well == null)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, "No well assigned to the dataset object");
                return false;
            }
            var datasets = well.Datasets;
            if (datasets == null) return true; //no dataset means nothing to worry about
            if (datasets.Any(u => u.Name == CurrentObject.Name))
            {
                DatasetNameAlreadyInUse();
                return false;
            }
            return true;
        }

        protected override bool CommonValidation()
        {
            if (CurrentObject == null)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    "No dataset object has been generated by this screen");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurrentObject.Name))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    "Dataset name must be specified");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurrentObject.Family))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                     "A Family must be selected");
                return false;
            }
            if (!Families.Contains(SelectedFamily))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                     "Selected Family is not valid");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurrentObject.Units))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                     "A Unit must be selected");
                return false;
            }
            if (!SelectedFamily.Units.Contains(CurrentObject.Units))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                                   "Units must belongs to correct Family");
                return false;
            }

            var datasetObject = HelperMethods.Instance.GetDatasetByID(CurrentObject.ID);
            if (datasetObject != null)
            {
                //object is being edited
                if (OriginalObject != null && datasetObject.Name == OriginalObject.Name) return true;
                DatasetNameAlreadyInUse();
                return false;
            }
            if (string.IsNullOrWhiteSpace(ComputedCalculation))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, "No valid calculations found to compute");
                return false;
            }

            return true;
        }


        protected override bool UpdateObjectValidation()
        {
            throw new NotImplementedException();
        }

        ICommand _selectOperand1Command, _selectOperand2Command;
        public ICommand SelectOperand1Command
        {
            get { return _selectOperand1Command ?? (_selectOperand1Command = new RelayCommand(SelectOperand1)); }
        }

        private void SelectOperand1()
        {
            IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.SelectDatasetForOperand1);
        }

        EquationInfo _selectedEquation;
        public EquationInfo SelectedEquation
        {
            get { return _selectedEquation; }
            set
            {
                _selectedEquation = value;
                NotifyPropertyChanged("SelectedEquation");
            }
        }


        public ICommand SelectOperand2Command
        {
            get { return _selectOperand2Command ?? (_selectOperand2Command = new RelayCommand(SelectOperand2)); }
        }

        private void SelectOperand2()
        {
            IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.SelectDatasetForOperand2);
        }

        public override void Save()
        {
            var dataset = HelperMethods.Instance.GetDatasetByID(Equations.First().Operand1.ID);
            var well = HelperMethods.Instance.GetWellByID(dataset.RefWell);
            if (well != null)
            {
                var notes = GlobalDataModel.Instance.GetNotesFirstAndLastValidData(CurrentObject.DepthAndCurves.ToList());
                CurrentObject.RefProject = dataset.RefProject;
                CurrentObject.RefWell = dataset.RefWell;

                if (!AddObjectValidation()) return;

                CurrentObject.MinUnitValue = decimal.Parse(GlobalDataModel.GetValidMinUnitValue(CurrentObject).ToString());
                CurrentObject.MaxUnitValue = CurrentObject.DepthAndCurves.Max(u => u.Curve);
                well.Datasets.Add(CurrentObject);
                IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
            }
        }

        private void StartProcessing(ref string equation)
        {
            int endingPoint = equation.IndexOf(')') - 1;
            int startingPoint = equation.LastIndexOf('(') + 1;
            if (endingPoint < 0) endingPoint = equation.Length - startingPoint;
            string calculation = equation.Substring(startingPoint, endingPoint);
            _calculations.Add(calculation);
            string number = (_calculations.Count - 1).ToString();
            equation = equation.Replace(calculation, number);

        }
        List<string> _calculations = new List<string>();

        bool _viewSpreadSheet;
        public bool ViewSpreadsheet
        {
            get { return _viewSpreadSheet; }
            set
            {
                _viewSpreadSheet = value;
                if (value)
                    ViewSpreadsheetContent = "Hide Live Spreadsheet";
                else
                    ViewSpreadsheetContent = "View Live Spreadsheet";
                NotifyPropertyChanged("ViewSpreadsheet");
            }
        }

        string _viewSpreadsheetContent;
        public string ViewSpreadsheetContent
        {
            get { return _viewSpreadsheetContent; }
            set
            {
                _viewSpreadsheetContent = value;
                NotifyPropertyChanged("ViewSpreadsheetContent");
            }
        }

        private void ApplyAddition(EquationInfo e)
        {
            var lst = new List<DepthCurveInfo>();
            if (e.Operand2 != null)
            {
                foreach (var element in _dicOperand1)
                {
                    lst.Add(new DepthCurveInfo
                        {
                            Depth = element.Key,
                            Curve = (_dicOperand1[element.Key] + _dicOperand2[element.Key]).ToTwoDigits()
                        });
                }
            }
            else
            {
                foreach (var element in _dicOperand1)
                {
                    lst.Add(new DepthCurveInfo
                    {
                        Depth = element.Key,
                        Curve = (_dicOperand1[element.Key] + e.RationalNumber).ToTwoDigits()
                    });
                }
            }
            CurrentObject.DepthAndCurves = new ObservableCollection<DepthCurveInfo>(lst);
        }
        private void ApplySubtraction(EquationInfo e)
        {
            var lst = new List<DepthCurveInfo>();
            if (e.Operand2 != null)
            {
                foreach (var element in _dicOperand1)
                {
                    lst.Add(new DepthCurveInfo
                    {
                        Depth = element.Key,
                        Curve = (_dicOperand1[element.Key] - _dicOperand2[element.Key]).ToTwoDigits()
                    });
                }
            }
            else
            {
                foreach (var element in _dicOperand1)
                {
                    lst.Add(new DepthCurveInfo
                    {
                        Depth = element.Key,
                        Curve = (_dicOperand1[element.Key] - e.RationalNumber).ToTwoDigits()
                    });
                }
            }

            CurrentObject.DepthAndCurves = new ObservableCollection<DepthCurveInfo>(lst);
        }
        private void ApplyMultiplication(EquationInfo e)
        {
            var lst = new List<DepthCurveInfo>();
            if (e.Operand2 != null)
            {
                foreach (var element in _dicOperand1)
                {
                    lst.Add(new DepthCurveInfo
                    {
                        Depth = element.Key,
                        Curve = (_dicOperand1[element.Key] * _dicOperand2[element.Key]).ToTwoDigits()
                    });
                }
            }
            else
            {
                foreach (var element in _dicOperand1)
                {
                    lst.Add(new DepthCurveInfo
                    {
                        Depth = element.Key,
                        Curve = (_dicOperand1[element.Key] * e.RationalNumber).ToTwoDigits()
                    });
                }
            }
            CurrentObject.DepthAndCurves = new ObservableCollection<DepthCurveInfo>(lst);
        }

        private void ApplyLog(EquationInfo e)
        {
            var lst = new List<DepthCurveInfo>();

            foreach (var element in _dicOperand1)
            {
                double number, baseNumber;
                number = double.Parse(_dicOperand1[element.Key].ToTwoDigits().ToString());
                baseNumber = e.RationalNumber;

                lst.Add(new DepthCurveInfo
                {
                    Depth = element.Key,
                    Curve = GetValidLog(number, baseNumber)
                });
            }

            CurrentObject.DepthAndCurves = new ObservableCollection<DepthCurveInfo>(lst);
        }

        private decimal GetValidLog(double number, double baseNumber)
        {
            if (number == 0) return 0;
            if (Double.IsNaN(Math.Log(number, baseNumber)))
                return 0;
            else
                return decimal.Parse(Math.Log(number, baseNumber).ToString()).ToTwoDigits();
        }
        private void ApplyDivision(EquationInfo e)
        {
            var lst = new List<DepthCurveInfo>();
            if (e.Operand2 != null)
            {
                foreach (var element in _dicOperand1)
                {
                    lst.Add(new DepthCurveInfo
                    {
                        Depth = element.Key,
                        Curve = _dicOperand2[element.Key] == 0 ? 0 : (_dicOperand1[element.Key] / _dicOperand2[element.Key]).ToTwoDigits()
                    });
                }
            }
            else
            {
                foreach (var element in _dicOperand1)
                {
                    lst.Add(new DepthCurveInfo
                    {
                        Depth = element.Key,
                        Curve = (_dicOperand1[element.Key] / e.RationalNumber).ToTwoDigits()
                    });
                }
            }
            CurrentObject.DepthAndCurves = new ObservableCollection<DepthCurveInfo>(lst);
        }

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(ComputedCalculation);
        }

        ICommand _deleteEquationCommand;
        public ICommand DeleteEquationCommand
        {
            get { return _deleteEquationCommand ?? (_deleteEquationCommand = new RelayCommand<EquationInfo>(DeleteEquation)); }
        }

        private bool IsOperandUsedBefore(EquationInfo equationInfo)
        {
            var equations = Equations.Except(new List<EquationInfo> { equationInfo });
            if (equationInfo.Operand1 == null) return false;
            return equations.Any(u => u.Operand1.ID == equationInfo.Operand1.ID);
        }

        public bool ShouldDelete { get; set; }

        private void RemoveEquation(EquationInfo equationInfo)
        {
            //remove this equation from equations list
            //check if operand used somewhere else
            if (IsOperandUsedBefore(equationInfo))
            {
                RemoveEquation(equationInfo);
            }
            else
            {
                var objToRemove = Equations.Single(u => u.Operand1.ID == equationInfo.ID);
                Equations.Remove(objToRemove);
                RemoveOperandWithEquation(equationInfo);
            }
        }

        private void RemoveOperandWithEquation(EquationInfo equationInfo)
        {
            if (equationInfo.Operand1 == null) return;
            var operandToRemove = Operand1.SingleOrDefault(u => u.ID == equationInfo.Operand1.ID);
            Operand1.Remove(operandToRemove);
            NotifyPropertyChanged("Operand1");
        }

        private void DeleteEquation(EquationInfo equationInfo)
        {
            if (Equations.Any(u => u.SrNo == equationInfo.SrNo))
            {
                if (IsOperandUsedBefore(equationInfo))
                {
                    ShouldDelete = false;
                    IoC.Kernel.Get<IGlobalDataModel>().SendMessage(Token, NotificationMessageEnum.ShouldDeleteEquation);
                    if (ShouldDelete)
                        RemoveEquation(equationInfo);
                    else
                        return;
                }
                else
                {
                    var obj = Equations.Single(u => u.SrNo == equationInfo.SrNo);
                    Equations.Remove(obj);
                    RemoveOperandWithEquation(equationInfo);
                }
                UpdateSerialNoAndOperand1();

                //skip last equation because it is a new equation and thus raise exception for operand2
                foreach (var equation in Equations.Take(Equations.Count - 1))
                    ProcessCommand(equation);

                ComputedCalculation = string.Empty;
                CalculationContent.Blocks.Clear();
                foreach (var equation in Equations)
                    WriteComputedCalculation(equation);
            }
        }

        private void UpdateSerialNoAndOperand1()
        {
            if (!Equations.Any()) return;

            var userGeneratedOperands = Operand1.Where(u => u.Name.StartsWith("Equation "));
            while (userGeneratedOperands.Any())
            {
                Operand1.Remove(userGeneratedOperands.First());
            }

            for (int i = 1; i < Equations.Count; i++)
            {
                Equations[i].SrNo = i + 1;
                var equation = new EquationInfo(Equations.Count)
                {
                    Name = string.Format("Equation {0}", (Equations[i]).SrNo - 1),

                };
                Operand1.Add(equation);
                equation.OperandChanged += equation_OperandChanged;
                
                Equations[i].Operand1 = Operand1.Last();
            }
        }

        void equation_OperandChanged(EquationInfo e)
        {
            if (!IsFirstOperandValid(e))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, "Invalid equation selected for Operand 1");
                e.ShouldUpdateOperand = false;
                return;
            }
        }
    }//end class
}//end namespace
