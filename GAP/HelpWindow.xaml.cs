using GAP.BL;
using GAP.ExtendedControls;
using GAP.MainUI.ViewModels.ViewModel;
using System;
using System.Reflection;

namespace GAP
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow
    {
        public HelpWindow()
        {
            InitializeComponent();
            _dataContext = new HelpViewModel(Token);
            DataContext = _dataContext;
            Closed += HelpWindow_Closed;
            AddKeyBindings<BaseEntity>();
        }

        HelpViewModel _dataContext;

        void HelpWindow_Closed(object sender, EventArgs e)
        {
            _dataContext.CleanUp();
            _dataContext = null;
            DataContext = null;
        }

        protected override void ReceiveMessage(Helpers.NotificationMessageType messageType)
        {
            var helpData = messageType.MessageObject as HelpData;
            var assembly = Assembly.GetExecutingAssembly();
            assembly.GetType(helpData.ViewName);
            var type = assembly.GetType("GAP." + helpData.ViewName);
            if (type != null)
            {
                var obj = assembly.CreateInstance(type.FullName);
                BaseWindow window = obj as BaseWindow;
                //var window = (BaseWindow)type;
                if (window != null)
                {
                    window.ShowDialog();
                }
                else
                {
                    if (helpData.ViewName.ToLower().Contains("project"))
                    {
                        var projectWindow = obj as BaseWindow<Project>;
                        if (projectWindow != null)
                            projectWindow.ShowDialog();
                    }
                    else if (helpData.ViewName.ToLower().Contains("well"))
                    {
                        var wellWindow = obj as BaseWindow<Well>;
                         if (wellWindow != null)
                            wellWindow.ShowDialog();
                    }
                    else if (helpData.ViewName.ToLower().Contains("dataset"))
                    {
                        var datasetWindow = obj as BaseWindow<Dataset>;
                        if (datasetWindow != null)
                            datasetWindow.ShowDialog();
                    }
                    else if (helpData.ViewName.ToLower().Contains("chart"))
                    {
                        var chartWindow = obj as BaseWindow<Chart>;
                        if (chartWindow != null)
                            chartWindow.ShowDialog();
                    }
                    else if (helpData.ViewName.ToLower().Contains("track"))
                    {
                        var trackWindow = obj as BaseWindow<Track>;
                        if (trackWindow != null)
                            trackWindow.ShowDialog();
                    }
                }
            }
        }
    }//end class
}//end namespace
