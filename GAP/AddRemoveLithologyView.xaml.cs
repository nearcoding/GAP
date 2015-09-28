using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Ninject;
using GAP.BL;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using System.Windows.Controls;

namespace GAP
{
    /// <summary>
    /// Interaction logic for AddRemoveLithologyView.xaml
    /// </summary>
    public partial class AddRemoveLithologyView 
    {
        public AddRemoveLithologyView()
        {
            InitializeComponent();
            _dataContext = new AddRemoveLithologyViewModel(Token);
            DataContext = _dataContext;
            AddKeyBindings<LithologyInfo>();
            Closed += AddRemoveLithologyView_Closed;
        }

        void AddRemoveLithologyView_Closed(object sender, EventArgs e)
        {
            _dataContext = null; 
        }

        AddRemoveLithologyViewModel _dataContext;

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.BrowseFiles:
                    SelectImage();
                    break;
                case NotificationMessageEnum.SaveLithologies:
                    SaveLithologies();
                    break;
                case NotificationMessageEnum.ShouldDeleteLithology:
                    RemoveLithologies();
                    break;
                case NotificationMessageEnum.SetFocus:
                    ExtendedFinalDepth.Focus();
                    break;
            }
        }

        private void RemoveLithologies()
        {
            var result = MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("DoYouWantToDeleteTheSelectedLithologies"),
                GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
                _dataContext.ShouldDeleteLithology = true;
            else
                _dataContext.ShouldDeleteLithology = false;
        }

        private void SaveLithologies()
        {
            MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("LithologiesSavedSuccessfully")
                , GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Information);
            GlobalData.ShouldSave = true;
            Close();
        }

        private void SelectImage()
        {
            LithologyImagesDialogView view = new LithologyImagesDialogView();
            view.ShowDialog();
            if (view.SelectedImage != null)
            {
                string fullImageName = view.SelectedImage.ToolTip.ToString();
                Uri uri = new Uri(fullImageName);
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = uri;
                bmp.EndInit();

                UpdateImageFileNameInLithology(fullImageName);
            }
        }

        private void UpdateImageFileNameInLithology(string fullImageName)
        {
            FileInfo fileInfo = new FileInfo(fullImageName);
            _dataContext.FullImageName = fileInfo.Name;

            if (fileInfo.Name.Length > 4)
                _dataContext.CurrentObject.LithologyName = fileInfo.Name.Substring(0, fileInfo.Name.Length - 4);
        }

    }//end class
}//end namespace
