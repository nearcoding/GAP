using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAP.MainUI.ViewModels
{
   public class NInjectBindings
    {
       public NInjectBindings()
       {
           IoC.Kernel = new StandardKernel();
           IoC.Kernel.Bind<IGlobalDataModel>().ToConstant(GlobalDataModel.Instance);
           IoC.Kernel.Bind<ISendMessage>().ToConstant(SendMessage.Instance);
           IoC.Kernel.Bind<IMainScreenViewModel>().To<MainScreenViewModel>();
           IoC.Kernel.Bind<IResourceHelper>().To<ResourceHelper>();
       }
    }

   public static class IoC
   {
       public static IKernel Kernel { get; set; }
   }
       
}
