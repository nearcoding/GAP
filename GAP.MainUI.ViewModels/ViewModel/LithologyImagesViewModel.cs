using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Ninject;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class LithologyImagesViewModel : BaseViewModel<BaseEntity>
    {
        public LithologyImagesViewModel(string token)
            : base(token)
        {
            ImagesCollection = new ObservableCollection<Image>();
            FillLithologies();
        }

        ICommand _selectImageCommand;
        public ICommand SelectImageCommand
        {
            get { return _selectImageCommand ?? (_selectImageCommand = new RelayCommand(SelectImage,  CanSelectImage)); }
        }

        private bool CanSelectImage()
        {
            return SelectedLithology != null;
        }

        private void SelectImage()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.SelectLithologyImage, SelectedLithology);            
        }

        public Image SelectedLithology { get; set; }

        /// <summary>
        /// Goes to the Lithologies directory and creates an image object and container for each image file there
        /// </summary>
        private void FillLithologies()
        {
            try
            {
                string bdir = AppDomain.CurrentDomain.BaseDirectory;

                bdir += "Lits";

                if (!Directory.Exists(bdir))
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                        IoC.Kernel.Get<IResourceHelper>().ReadResource("LithologyDirectoryNotFound"));
                    return;
                }

                foreach (string filename in Directory.GetFiles(bdir))
                {
                    if (!filename.Contains(".bmp")) continue;
                    Image i = AddFileToCollection(filename);
                    ImagesCollection.Add(i);
                }
            }
            catch (Exception ex)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, ex.ToString());
            }
        }

        public ObservableCollection<Image> ImagesCollection { get; set; }


        private static Image AddFileToCollection(string filename)
        {
            var i = new Image();

            var byteStream = new MemoryStream(File.ReadAllBytes(filename));

            var bi = new BitmapImage();
            bi.BeginInit();
            bi.DecodePixelWidth = 30;
            bi.StreamSource = byteStream;
            bi.EndInit();

            i.Source = bi;
            i.Width = 20;
            i.Height = 20;
            i.ToolTip = filename;
            return i;
        }
    }//end class
}//end namespace
