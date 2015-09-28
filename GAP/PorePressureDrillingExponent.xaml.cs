using GAP.ExtendedControls;
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
    /// Interaction logic for PorePressureDrillingExponent.xaml
    /// </summary>
    public partial class PorePressureDrillingExponent 
    {
        public PorePressureDrillingExponent()
        {            
            InitializeComponent();
            var window  = FindAncestor<BaseWindow>(this);
            if (window == null)
                throw new Exception("Parent window not found");
            _dataContext = new PorePressureDrillingExponentViewModel(window.Token);
            DataContext = _dataContext;
        }
        public static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                if (current is Visual)
                    current = VisualTreeHelper.GetParent(current);
                else
                    continue;
            }
            while (current != null);
            return null;
        }
        PorePressureDrillingExponentViewModel _dataContext;
    }//end class
}//end namespace
