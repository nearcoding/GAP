using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace GAP.Custom_Controls
{
    /// <summary>
    /// Interaction logic for ucDataset.xaml 
    /// </summary>
    public partial class ucDatasetMathFilter : UserControl
    {
        public ucDatasetMathFilter()
        {
            InitializeComponent();
            this.ToggleSwitchControl = this.ToggleSwitch1;
        }

        public delegate void IsToggleChecked(bool isChecked);

        public event IsToggleChecked IsToggleButtonChecked;
        private void ToggleSwitch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleSwitch button = sender as ToggleSwitch;
            if (button!=null)
            {
                if (IsToggleButtonChecked!=null) IsToggleButtonChecked(button.IsChecked.Value);                
            }
        }

        public ToggleSwitch ToggleSwitchControl { get; private set; }
    }//end class
}//end namespace
