using Abt.Controls.SciChart.Visuals;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using System.Windows;
using Ninject;
using Microsoft.Win32;
using GAP.BL;
namespace GAP
{
    /// <summary>
    /// Interaction logic for TrackAnalysis.xaml
    /// </summary>
    public partial class TrackAnalysis
    {
        public TrackAnalysis()
        {
            InitializeComponent();
            Loaded += TrackAnalysis_Loaded;
        }

        public Track Track { get; set; }

        void TrackAnalysis_Loaded(object sender, RoutedEventArgs e)
        {
            if (Track == null)
            {
                MessageBox.Show("Track object must not be null", GlobalData.CompanyName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Close();
                return;
            }
            DataContext = new TrackAnalysisViewModel(Token, Track);
        }

        private void ButtonFullSnapshot_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { Filter = "Image File (*.png)|*.png" };

            if (dialog.ShowDialog() != true) return;
            this.SciChartControl1.ExportToFile(dialog.FileName, ExportType.Png);
            MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("ImageSavedSuccessfully"), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }//end class
}//end namespace
