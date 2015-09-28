using System.Windows;

namespace GAP
{
    /// <summary>
    /// Interaction logic for Login.xaml - Login not been used
    /// </summary>
    public partial class Login : Window
    {
        #region Constructors

        /// <summary>
        /// Generic constructor
        /// </summary>
        public Login()
        {
            InitializeComponent();
        }
        #endregion

        #region Events

        /// <summary>
        /// Cancels de login and closes de App
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Logs into the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();

            mw.Show();
            this.Hide();
        }
        #endregion

    }
}
