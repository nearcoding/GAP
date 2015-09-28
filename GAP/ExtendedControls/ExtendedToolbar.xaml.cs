using GAP.MainUI.ViewModels.ViewModel;
using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Ninject;
using GAP.MainUI.ViewModels.Helpers;
namespace GAP.ExtendedControls
{
    public class ExtendedToolbar : ToolBar
    {
        public ExtendedToolbar()
        {
            DataContextChanged += ExtendedToolbar_DataContextChanged;
        }

        void ExtendedToolbar_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext == null) return;
            var toolBarItems = DataContext as ObservableCollection<ToolbarInfo>;
            Items.Clear();
            foreach (ToolbarInfo info in toolBarItems)
            {
                switch (info.Control)
                {
                    case ControlType.Checkbox:
                        AddCheckBoxControls(info);
                        break;
                    case ControlType.Button:
                        AddButtonControls(info);
                        break;
                    case ControlType.Separator:
                        Items.Add(new Separator());
                        break;
                }
            }
        }

        private void AddButtonControls(ToolbarInfo info)
        {
            var image = new Image
            {
                Source = new BitmapImage(new Uri(info.Image, UriKind.Relative))
            };

            var button = new Button
            {
                Content = image,
                ToolTip = IoC.Kernel.Get<IResourceHelper>().ReadResource(info.Text)
            };

            var binding = new Binding("DataContext." + info.Command)
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(BaseWindow), 1),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            button.SetBinding(Button.CommandProperty, binding);
            Items.Add(button);
        }

        private void AddCheckBoxControls(ToolbarInfo info)
        {
            var check = new ToggleSwitch
            {
                Content = IoC.Kernel.Get<IResourceHelper>().ReadResource(info.Text),
                Tag = info.Text,
                IsChecked = info.Value
            };
            var label = new Label
            {
                VerticalAlignment = VerticalAlignment.Center,
                Content = check
            };

            if (!string.IsNullOrWhiteSpace(info.Command))
            {
                var checkBoxBinding = new Binding("DataContext." + info.Command)
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(MainWindow), 1),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                check.SetBinding(ToggleSwitch.IsCheckedProperty, checkBoxBinding);
            }

            Items.Add(label);
        }
    }//end class
}//end namespace
