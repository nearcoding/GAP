using GalaSoft.MvvmLight.Messaging;
using GAP.BL;
using GAP.HelperClasses;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ninject;
namespace GAP.ExtendedControls
{
    public class BaseWindow<T> : MetroWindow where T : BaseEntity
    {
        public BaseWindow()
        {
            Token = Guid.NewGuid().ToString();
            Messenger.Default.Register<NotificationMessageType>(this, Token, MessageProcessor);
            MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            SetResourceReference(Window.BorderBrushProperty, "AccentColorBrush");
            BorderThickness = new Thickness(2);
            var image = new BitmapImage(new Uri("pack://application:,,,/Images/Earth.ico"));
            Icon = image;
            DataContextChanged += BaseWindow_DataContextChanged;

            Loaded += BaseWindow_Loaded;
        }

        public Type ScreenType { get; set; }

        void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        void BaseWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null) AddKeyBindings();
        }

        private void AddKeyBindings()
        {
            KeyBinding saveKeyBinding =
                new KeyBinding((DataContext as BaseViewModel<T>).SaveCommand, Key.S, ModifierKeys.Control);
            this.InputBindings.Add(saveKeyBinding);

            KeyBinding cancelKeyBinding =
                new KeyBinding((DataContext as BaseViewModel<T>).CancelCommand, Key.Escape, ModifierKeys.None);
            this.InputBindings.Add(cancelKeyBinding);
        }
        public bool IsSaved { get; set; }

        private void MessageProcessor(NotificationMessageType message)
        {
            switch (message.MessageType)
            {
                case NotificationMessageEnum.MessageBox:
                    MessageBox.Show(message.MessageObject.ToString(), GlobalData.MESSAGEBOXTITLE);
                    break;
                case NotificationMessageEnum.MessageBoxWithExclamation:
                    MessageBox.Show(message.MessageObject.ToString(), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case NotificationMessageEnum.MessageBoxWithError:
                    MessageBox.Show(message.MessageObject.ToString(), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case NotificationMessageEnum.MessageBoxWithInformation:
                    MessageBox.Show(message.MessageObject.ToString(), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case NotificationMessageEnum.CloseScreen:
                    Close();
                    break;
                case NotificationMessageEnum.MessageAndClose:
                    MessageBox.Show(message.MessageObject.ToString(), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                    break;
                case NotificationMessageEnum.CloseWithGlobalDataSave:
                    GlobalData.ShouldSave = true;
                    Close();
                    break;
                case NotificationMessageEnum.MessageAndCloseWithGlobalDataSave:
                    GlobalData.ShouldSave = true;
                    MessageBox.Show(message.MessageObject.ToString(), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                    break;
                case NotificationMessageEnum.CancelScreen:
                    var result = MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("SaveChangesBeforeClosingTheScreen"),
                   GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        (DataContext as BaseViewModel<T>).AutoSave = true;
                    break;
                default:
                    ReceiveMessage(message);
                    break;
            }
        }

        protected virtual void ReceiveMessage(NotificationMessageType messageType)
        { }

        public string Token { get; set; }
    }
    public class BaseWindow : MetroWindow
    {
        public BaseWindow()
        {
            Token = Guid.NewGuid().ToString();
            Messenger.Default.Register<NotificationMessageType>(this, Token, MessageProcessor);
            MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            SetResourceReference(Window.BorderBrushProperty, "AccentColorBrush");
            BorderThickness = new Thickness(2);
            BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/Images/Earth.ico"));
            this.Icon = image;
        }

        //  public BaseEntity EntityType { get; set; }

        public void AddKeyBindings<T>() where T : BaseEntity
        {
            KeyBinding saveKeyBinding =
                new KeyBinding((DataContext as BaseViewModel<T>).SaveCommand, Key.S, ModifierKeys.Control);
            this.InputBindings.Add(saveKeyBinding);

            KeyBinding cancelKeyBinding =
                new KeyBinding((DataContext as BaseViewModel<T>).CancelCommand, Key.Escape, ModifierKeys.None);
            this.InputBindings.Add(cancelKeyBinding);
        }
        public bool IsSaved { get; set; }

        private void MessageProcessor(NotificationMessageType message)
        {
            switch (message.MessageType)
            {
                case NotificationMessageEnum.MessageBox:
                    MessageBox.Show(message.MessageObject.ToString(), GlobalData.MESSAGEBOXTITLE);
                    break;
                case NotificationMessageEnum.MessageBoxWithExclamation:
                    MessageBox.Show(message.MessageObject.ToString(), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case NotificationMessageEnum.MessageBoxWithError:
                    MessageBox.Show(message.MessageObject.ToString(), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case NotificationMessageEnum.MessageBoxWithInformation:
                    MessageBox.Show(message.MessageObject.ToString(), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case NotificationMessageEnum.CloseScreen:
                    Close();
                    break;
                case NotificationMessageEnum.MessageAndClose:
                    MessageBox.Show(message.MessageObject.ToString(), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                    break;
                case NotificationMessageEnum.CloseWithGlobalDataSave:
                    GlobalData.ShouldSave = true;
                    Close();
                    break;
                case NotificationMessageEnum.MessageAndCloseWithGlobalDataSave:
                    GlobalData.ShouldSave = true;
                    MessageBox.Show(message.MessageObject.ToString(), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                    break;
                default:
                    ReceiveMessage(message);
                    break;
            }
        }

        protected virtual void ReceiveMessage(NotificationMessageType messageType)
        { }

        public string Token { get; set; }
    }
}
