using AutoMapper;
using GAP.BL;
using GAP.Helpers;
using System;
using Ninject;
using GAP.MainUI.ViewModels.Helpers;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class SpreadsheetSettingsViewModel : BaseViewModel<Dataset>
    {
        public SpreadsheetSettingsViewModel(string token, Dataset dataset)
            : base(token)
        {
            Mapper.CreateMap<Dataset, Dataset>();
            CurrentObject = Mapper.Map<Dataset>(dataset);
        }

        protected override bool CanSave()
        {
            int integerValue;
            if (!Int32.TryParse(CurrentObject.InitialDepth.ToString(), out integerValue))
                return false;

            return Int32.TryParse(CurrentObject.FinalDepth.ToString(), out integerValue) &&
                Int32.TryParse(CurrentObject.Step.ToString(), out integerValue);
        }

        public override void Save()
        {
            if (ValidateData())
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.SaveAutoDepth);
        }

        private bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(CurrentObject.Step.ToString()))
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("StepCanNotBeEmpty"));
            else if (CurrentObject.InitialDepth > CurrentObject.FinalDepth)
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("InitialDepthCannotBeHigherThanFinalDepth"));
            else if (CurrentObject.InitialDepth < 0)
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("InitialDepthCannotBeNegative"));
            else if (CurrentObject.Step <= 0)
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("ValueOfStepMustBeGreaterThanOne"));
            else
                return true;

            return false;
        }
    }//end class
}//end namespace
