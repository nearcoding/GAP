using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class ResourceHelper : IResourceHelper
    {
        public string ReadResource(string resourceName)
        {
            if (!Application.Current.Resources.Contains(resourceName)) return string.Empty;
            if (Application.Current.FindResource(resourceName) != null)
                return (string)Application.Current.FindResource(resourceName);
            return string.Empty;
        }
    }

    public interface IResourceHelper
    {
        string ReadResource(string resourceName);
    }
}
