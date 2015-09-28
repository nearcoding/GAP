using GAP.BL;
using System.Windows;

namespace GAP
{
    /// <summary>
    /// Interaction logic for SurfaceTestWindow.xaml
    /// </summary>
    public partial class SurfaceTestWindow : Window
    {
        public SurfaceTestWindow(TrackToShow trackObject)
        {
            InitializeComponent();
            DataContext = trackObject;
        }

    }//end class
}//end namespace
