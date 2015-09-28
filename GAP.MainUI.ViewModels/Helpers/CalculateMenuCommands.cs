using GalaSoft.MvvmLight.Command;
using GAP.BL;
using System.Windows.Input;
using Ninject;
using GAP.Helpers;
using System.Linq;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class CalculateMenuCommands
    {
        string _token;
        public CalculateMenuCommands(string token)
        {
            _token = token;
        }
        ICommand _calculateOverburdenGradientCommand;
        public ICommand CalculateOverburdenGradientCommand
        {
            get { return _calculateOverburdenGradientCommand ?? (_calculateOverburdenGradientCommand = new RelayCommand(CalculateOverburdenGradient, CanCalculateOverburdenGradient)); }
        }
        private void CalculateOverburdenGradient()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.CalculateOverburdenGradient);
        }

        private bool CanCalculateOverburdenGradient()
        {
            return HelperMethods.Instance.ProjectsWithRHOBDatasets().Any();
        }

        ICommand _calculatePorePressureGradientCommand;
        public ICommand CalculatePorePressureGradientCommand
        {
            get { return _calculatePorePressureGradientCommand ?? (_calculatePorePressureGradientCommand = new RelayCommand(CalculatePorePressureGradient, CanCalculatePorePressureGradient)); }
        }
        private void CalculatePorePressureGradient()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.CalculatePorePressure);
        }

        private bool CanCalculatePorePressureGradient()
        {
            return true;
        }


        ICommand _calculateFractureGradientCommand;

        private void CalculateFractureGradient()
        {

        }

        private bool CanCalculateFractureGradient()
        {
            return false;
        }

        public ICommand CalculateFractureGradientCommand
        {
            get { return _calculateFractureGradientCommand ?? (_calculateFractureGradientCommand = new RelayCommand(CalculateFractureGradient, CanCalculateFractureGradient)); }
        }


        ICommand _calculateShalePointFilterCommand;

        private void CalculateShalePointFilter()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.CalculateShalePointFilter);
        }

        private bool CanCalculateShalePointFilter()
        {
            return HelperMethods.Instance.GetAllDatasets().Where(u => u.Family == "Gamma Ray" || u.Family == "VShale").Any();
        }

        public ICommand CalculateShalePointFilterCommand
        {
            get { return _calculateShalePointFilterCommand ?? (_calculateShalePointFilterCommand = new RelayCommand(CalculateShalePointFilter, CanCalculateShalePointFilter)); }
        }


        ICommand _calculateMathFilterCommand;

        private void CalculateMathFilter()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.MathFilter);
        }

        private bool CanCalculateMathFilter()
        {
            return GlobalCollection.Instance.Projects.Any(u => u.Wells.Any()) && GlobalCollection.Instance.Projects.Any(u => u.Wells.Any(a => a.Datasets.Any()));
        }

        public ICommand CalculateMathFilterCommand
        {
            get { return _calculateMathFilterCommand ?? (_calculateMathFilterCommand = new RelayCommand(CalculateMathFilter, CanCalculateMathFilter)); }
        }


        ICommand _calculateAddEquationCommand;

        private void CalculateAddEquation()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.CalculateAddEquation);
        }

        private bool CanCalculateAddEquation()
        {
            return GlobalCollection.Instance.Projects.Any(u => u.Wells.Any(a => a.Datasets.Any()));
        }

        public ICommand CalculateAddEquationCommand
        {
            get { return _calculateAddEquationCommand ?? (_calculateAddEquationCommand = new RelayCommand(CalculateAddEquation, CanCalculateAddEquation)); }
        }

    }//end class
}//end namespace
