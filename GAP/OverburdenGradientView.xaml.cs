using GAP.MainUI.ViewModels.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GAP
{
    /// <summary>
    /// Interaction logic for OverburdenGradientView.xaml
    /// </summary>
    public partial class OverburdenGradientView 
    {
        public OverburdenGradientView()
        {
            InitializeComponent();
            _dataContext = new OverburdenGradientViewModel(Token);
            DataContext = _dataContext;
        }

        OverburdenGradientViewModel _dataContext;
    }//end class
}//end namespace
