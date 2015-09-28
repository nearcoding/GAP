using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;
using Microsoft.Win32;
using Ninject;
using System;
using System.IO;
using System.Windows;
using GAP.MainUI.ViewModels.Helpers;

namespace GAP
{
    /// <summary>
    /// Interaction logic for LithologyProperties.xaml
    /// </summary>
    public partial class LithologyPropertiesView
    {
        public string imagepath;
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public LithologyPropertiesView()
        {
            InitializeComponent();
            _dataContext = new LithologyPropertiesViewModel(Token);
            DataContext = _dataContext;
        }

        LithologyPropertiesViewModel _dataContext;
        #endregion

        #region Events

        private void Add()
        {
            try
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "*.bmp|*.bmp";
                if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.FileName))
                {
                    if (dialog.FileName.EndsWith(".bmp"))                    
                        AddBitmapImageToLitsFolder(dialog);                                       
                    else                    
                        MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("WrongFileExtension"), 
                            GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Exclamation);                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddBitmapImageToLitsFolder(OpenFileDialog dialog)
        {
            string filename = dialog.SafeFileName;
            string applicationBinDirectory = AppDomain.CurrentDomain.BaseDirectory;
            applicationBinDirectory += "Lits\\";

            if (!Directory.Exists(applicationBinDirectory)) 
                Directory.CreateDirectory(applicationBinDirectory + "Lits");

            if (!File.Exists(applicationBinDirectory + filename))
            {
                File.Copy(dialog.FileName, applicationBinDirectory + filename);
                _dataContext.AddedFileName = applicationBinDirectory + filename;
                MessageBox.Show(string.Format("File {0} saved", filename), GlobalData.MESSAGEBOXTITLE,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("FileWithThisNameAlreadyExists"),
                    GlobalData.MESSAGEBOXTITLE, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.DeleteLithology:
                    DeleteLithologyImage();
                    break;
                case NotificationMessageEnum.AddLithology:
                    Add();
                    break;
            }
        }

        private void DeleteLithologyImage()
        {
            var result = MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("DoYouWantToDeleteTheSelectedLithologyImage")
                , GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _dataContext.DeleteLithologyApproved = true;
            }
        }

        #endregion
    }
}
