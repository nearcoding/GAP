using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GAP.Custom_Controls;
using GAP.MainUI.ViewModels.Helpers;
using Ninject;
using GAP.MainUI.ViewModels.ViewModel;
using System.Collections.Generic;

namespace GAP.HelperClasses
{
    public static class GlobalData
    {
        public static string CompanyName { get; set; }
        public static MainWindow MainWindow { get; set; }

        public static ucTrackItemsControl TrackItemsControl { get; set; }

        static GlobalData()
        {
            CompanyName = "Data Log";
        }

        public static readonly string MESSAGEBOXTITLE = IoC.Kernel.Get<IResourceHelper>().ReadResource("GAP");
    
        public static string LoadedProjectPath { get; set; }

        static bool _shouldSave;
        public static bool ShouldSave
        {
            get { return _shouldSave; }
            set
            {
                _shouldSave = value;
                GlobalDataModel.ShouldSave = value;
            }
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

    }//end class       
}//end namespace
