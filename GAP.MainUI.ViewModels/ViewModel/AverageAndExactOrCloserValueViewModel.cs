using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using Ninject;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class AverageAndExactOrCloserValueViewModel : BaseViewModel<BaseEntity>
    {
        public AverageAndExactOrCloserValueViewModel(string token, decimal initialDepth, decimal finalDepth)
            : base(token)
        {
            IsArithmeticAverageChecked = true;
            InitialDepth = initialDepth;
            FinalDepth = finalDepth;
            Step = 1;
        }

        decimal _first, _last, _step;
        public decimal InitialDepth
        {
            get { return _first; }
            set
            {
                _first = value;
                NotifyPropertyChanged("InitialDepth");
            }
        }
        public decimal FinalDepth
        {
            get { return _last; }
            set
            {
                _last = value;
                NotifyPropertyChanged("FinalDepth");
            }
        }

        public decimal Step
        {
            get { return _step; }
            set
            {
                _step = value;
                NotifyPropertyChanged("Step");
            }
        }

        bool _isArithmeticAverageChecked, _isArithmeticExactOrCloserValueChecked;

        public bool IsArithmeticAverageChecked
        {
            get { return _isArithmeticAverageChecked; }
            set
            {
                _isArithmeticAverageChecked = value;
                NotifyPropertyChanged("IsArithmeticAverageChecked");
            }
        }

        public bool IsArithmeticExactOrCloserValueChecked
        {
            get { return _isArithmeticExactOrCloserValueChecked; }
            set
            {
                _isArithmeticExactOrCloserValueChecked = value;
                NotifyPropertyChanged("IsArithmeticExactOrCloserValueChecked");
            }
        }

        public override void Save()
        {
            if (InitialDepth > FinalDepth)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation
                    (Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("InitialDepthShouldbeSmallerThanFinalDepth"));
                return;
            }

            if (Step > FinalDepth - InitialDepth)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation
                    (Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("StepIsTooHigh"));
                return;
            }

            if (InitialDepth <= 0)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation
                    (Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("InitialDepthCannotBeZeroOrNegative"));
                return;
            }

            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.SaveDepthImportView);
        }
    }//end class
}//end namespace