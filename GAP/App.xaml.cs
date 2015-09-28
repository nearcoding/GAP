using System.Windows;
using Ninject;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.MainUI.ViewModels.Helpers;
using GAP.MainUI.ViewModels;
using System.Windows.Markup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GAP.BL;
using Abt.Controls.SciChart.Visuals;

namespace GAP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
            Task task = Task.Factory.StartNew(() =>
                {
                    IoC.Kernel = new StandardKernel();
                    BindNinjectReferences();
                    new NInjectBindings();
                });

            SciChartSurface.SetRuntimeLicenseKey(@"<LicenseContract>
  <Customer>Datalog</Customer>
  <OrderId>ABT140513-8370-86116</OrderId>
  <LicenseCount>1</LicenseCount>
  <IsTrialLicense>false</IsTrialLicense>
  <SupportExpires>05/13/2015 00:00:00</SupportExpires>
  <ProductCode>SC-WPF-PRO</ProductCode>
  <KeyCode>lwAAAAEAAABps4DqHg7QAWQAQ3VzdG9tZXI9RGF0YWxvZztPcmRlcklkPUFCVDE0MDUxMy04MzcwLTg2MTE2O1N1YnNjcmlwdGlvblZhbGlkVG89MTMtTWF5LTIwMTU7UHJvZHVjdENvZGU9U0MtV1BGLVBST3dcZhbfVop8el6KkdkGbXp8bZyyGR0579IC208wiKl/o8wWPgVk35YMDcsvtC70mg==</KeyCode>
</LicenseContract>");
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
          
        }

        private void BindNinjectReferences()
        {
            IoC.Kernel.Bind<IGlobalDataModel>().ToConstant<GlobalDataModel>(GlobalDataModel.Instance);
            // IoC.Kernel.Bind<IUndoRedoObject>().ToConstant(UndoRedoObject.Instance);
            IoC.Kernel.Bind<ISendMessage>().ToConstant(SendMessage.Instance);
            IoC.Kernel.Bind<IMainScreenViewModel>().To<MainScreenViewModel>().InSingletonScope();
            IoC.Kernel.Bind<IResourceHelper>().To<ResourceHelper>();
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
        }
    }

    public static class IoC
    {
        public static IKernel Kernel { get; set; }
    }

}//end namespace
