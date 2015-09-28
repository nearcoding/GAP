using System.Windows.Media;

namespace GAP.BL
{
    public class EquationInfo : BaseEntity
    {
        public EquationInfo(int count)
        {
            this.Color = Colors.Black;
            DisplayIndex = count;
        }
        int _srNo;
        public int SrNo
        {
            get { return _srNo; }
            set
            {
                _srNo = value;
                IsFirstRow = _srNo == 1;
                NotifyPropertyChanged("SrNo");
            }
        }

        bool _isFirstRow;
        public bool IsFirstRow
        {
            get { return _isFirstRow; }
            set
            {
                _isFirstRow = value;
                NotifyPropertyChanged("IsFirstRow");
            }
        }

        Color _color;
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                NotifyPropertyChanged("Color");
            }
        }

        bool _canOperand1Edit;

        public bool CanOperand1Edit
        {
            get { return _canOperand1Edit; }
            set
            {
                _canOperand1Edit = value;
                NotifyPropertyChanged("CanOperand1Edit");
            }
        }

        BaseEntity _operand1, _operand2;
        public BaseEntity Operand1
        {
            get { return _operand1; }
            set
            {
                OldOperand = _operand1;
                if (_operand1 != value)
                {
                    _operand1 = value;
                    if (OperandChanged != null)
                    {
                        ShouldUpdateOperand = true;
                        OperandChanged(this);
                        if (!ShouldUpdateOperand)
                            Operand1 = OldOperand;
                    }
                }
                NotifyPropertyChanged("Operand1");
            }
        }
        public BaseEntity OldOperand { get; set; }
        public bool ShouldUpdateOperand { get; set; }

        public delegate void Operand1Changed(EquationInfo e);
        public event Operand1Changed OperandChanged;

        bool _isLogOperator;
        public bool IsLogOperator
        {
            get { return _isLogOperator; }
            set
            {
                _isLogOperator = value;
                if (value)
                    Operand2 = null;
                NotifyPropertyChanged("IsLogOperator");
            }
        }

        string _operator;
        public string Operator
        {
            get { return _operator; }
            set
            {
                _operator = value;

                NotifyPropertyChanged("Operator");
            }
        }

        public BaseEntity Operand2
        {
            get { return _operand2; }
            set
            {
                _operand2 = value;
                NotifyPropertyChanged("Operand2");
            }
        }

        bool _isOperand1Valid;
        public bool IsOperand1Valid
        {
            get { return _isOperand1Valid; }
            set
            {
                _isOperand1Valid = value;
                NotifyPropertyChanged("IsOperand1Valid");
            }
        }
        int _rationalNumber;
        public int RationalNumber
        {
            get { return _rationalNumber; }
            set
            {
                _rationalNumber = value;
                IsLogOperator = _rationalNumber > 0;
                NotifyPropertyChanged("RationalNumber");
            }
        }
    }//end class
}//end namespace
