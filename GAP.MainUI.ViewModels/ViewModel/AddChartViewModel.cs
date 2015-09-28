using System.Linq;
using Ninject;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class AddChartViewModel : ExtendedBaseViewModel<Chart>
    {
        public AddChartViewModel(string token) : base(token) { }

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(CurrentObject.Name);
        }

        public override void Save()
        {
            if (!AddObjectValidation()) return;
            ChartManager.Instance.AddChartObject(CurrentObject);
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseWithGlobalDataSave);
        }

        protected override bool UpdateObjectValidation() { return true; }

        protected override bool CommonValidation() { return true; }

        protected override bool AddObjectValidation()
        {
            if (GlobalCollection.Instance.Charts.Any(u => u.Name == CurrentObject.Name))
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("Msg_ChartNameAlreadyInUse"));
                return false;
            }
            return true;
        }

        protected override string GetValidName()
        {
            var lstCharts = GlobalCollection.Instance.Charts.Where(u => u.Name.StartsWith("Chart_")).Select(v => v.Name);
            return GlobalDataModel.GetIncrementalEntityName<Chart>(lstCharts);
        }
    }//end class
}//end namespace
