using GAP.BL;
using GAP.MainUI.ViewModels.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive;
using System.IO;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class DefaultChartCollection
    {
        private DefaultChartCollection()
        {

        }

        static DefaultChartCollection _instance;

        public static DefaultChartCollection Instance
        {
            get
            {
                if (_instance == null) _instance = new DefaultChartCollection();
                return _instance;
            }
        }

        public void InitializeDefaultCharts()
        {
            var lst = GlobalSerializer.DeserializeChart();

            IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Charts.Clear();
            GlobalCollection.Instance.Charts.Clear();

            if (lst == null)
            {
                WriteChartFile();
                InitializeDefaultCharts();
            }
            else
                lst.ForEach(u => ChartManager.Instance.AddChartObject(u));
        }

        private static void WriteChartFile()
        {
            var logChartName = IoC.Kernel.Get<IResourceHelper>().ReadResource("Logs");
            var logChart = new Chart
            {
                Name = logChartName,
                DisplayIndex = 0,
                ID = Guid.NewGuid().ToString()
            };
            GlobalCollection.Instance.Charts.Add(logChart);

            InitializeLogCharts(logChart);

            var overburdenChartName = IoC.Kernel.Get<IResourceHelper>().ReadResource("Overburden");
            var overBurderChart = new Chart
              {
                  Name = overburdenChartName,
                  DisplayIndex = 1,
                  ID = Guid.NewGuid().ToString()
              };
            GlobalCollection.Instance.Charts.Add(overBurderChart);
            InitializeOverburdenCharts(overBurderChart);

            var porePressureChartName = IoC.Kernel.Get<IResourceHelper>().ReadResource("PorePressure");
            var porePressureChart = new Chart
             {
                 Name = porePressureChartName,
                 DisplayIndex = 2,
                 ID = Guid.NewGuid().ToString()
             };
            GlobalCollection.Instance.Charts.Add(porePressureChart);
            InitializePorePressure(porePressureChart);

            string fracGradientChartName = IoC.Kernel.Get<IResourceHelper>().ReadResource("Frac.Gradient");
            var fracGradientChart = new Chart
               {
                   Name = fracGradientChartName,
                   DisplayIndex = 3,
                   ID = Guid.NewGuid().ToString()
               };
            GlobalCollection.Instance.Charts.Add(fracGradientChart);
            InitializeFracGradient(fracGradientChart);
            string fileName = IoC.Kernel.Get<IGlobalDataModel>().GetAppDataFolder();

            GlobalSerializer.SerializeObject<List<Chart>>(GlobalCollection.Instance.Charts.ToList(), fileName + "\\DefaultCharts.xml");
        }

        private static void InitializeFracGradient(Chart chart)
        {
            chart.Tracks.Add(new Track
             {
                 DisplayIndex = 0,
                 RefChart = chart.ID,
                 Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("Overburden"),
                 ID = Guid.NewGuid().ToString()
             });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 1,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("PorePressure"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 2,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("Lithology-Events"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 3,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("Frac.Gradient"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 4,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("Sonic-GammaRay-RHOB"),
                ID = Guid.NewGuid().ToString()
            });
        }

        private static void InitializePorePressure(Chart chart)
        {
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 0,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("Overburden"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 1,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("RHOB-GammaRay"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 2,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("Sonic-DT"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 3,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("Lithology-Events"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 4,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("PorePressure"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 5,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("OtherLogs"),
                ID = Guid.NewGuid().ToString()
            });
        }

        private static void InitializeOverburdenCharts(Chart chart)
        {
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 0,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("Sonic-DT"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 1,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("RHOB"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 2,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("GammaRay"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 3,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("Overburden"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 4,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("Lithology"),
                ID = Guid.NewGuid().ToString()
            });
        }

        private static void InitializeLogCharts(Chart chart)
        {
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 0,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("GammaRay"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 1,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("Sonic-DT"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 2,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("RHOB"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 3,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("Events"),
                ID = Guid.NewGuid().ToString()
            });
            chart.Tracks.Add(new Track
            {
                DisplayIndex = 4,
                RefChart = chart.ID,
                Name = IoC.Kernel.Get<IResourceHelper>().ReadResource("OtherLogs"),
                ID = Guid.NewGuid().ToString()
            });
        }
    }//end class
}//end namespace
