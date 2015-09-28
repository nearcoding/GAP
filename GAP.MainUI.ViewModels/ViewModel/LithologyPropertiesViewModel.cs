using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Ninject;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class LithologyPropertiesViewModel : BaseViewModel<BaseEntity>
    {
        ICommand _addLithologyCommand, _deleteLithologyCommand;

        public LithologyPropertiesViewModel(string token)
            : base(token)
        {
            ImagesCollection = new ObservableCollection<Image>();
            FillLithologies();
        }

        public ICommand DeleteLithologyCommand
        {
            get { return _deleteLithologyCommand ?? (_deleteLithologyCommand = new RelayCommand(DeleteLithology)); }
        }

        public ICommand AddLithologyCommand
        {
            get { return _addLithologyCommand ?? (_addLithologyCommand = new RelayCommand(AddLithology)); }
        }

        private void AddLithology()
        {
            AddedFileName = string.Empty;
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.AddLithology);
            try
            {
                if (!string.IsNullOrWhiteSpace(AddedFileName))
                {
                    //file is added, update the collection thus the UI
                    ImagesCollection.Add(AddFileToCollection(AddedFileName));
                }
            }
            catch (UnauthorizedAccessException)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    @"You don't have enough permissions to add or remove images from this screen" + Environment.NewLine
                    + "To do that, you need to restart the application as an administrator");
            }
        }

        private void DeleteLithology()
        {
            if (SelectedLithology == null)
                return;

            DeleteLithologyApproved = false;
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.DeleteLithology);

            try
            {
                if (DeleteLithologyApproved)
                {
                    string imagepath = SelectedLithology.ToolTip.ToString();
                    if (File.Exists(imagepath))
                    {
                        var singleObject = ImagesCollection.SingleOrDefault(u => u.ToolTip.ToString() == SelectedLithology.ToolTip.ToString());
                        if (singleObject != null)
                        {
                            ImagesCollection.Remove(singleObject);
                            File.Delete(imagepath);
                        }
                    }
                    else
                        IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("ImageNotFound"));
                }
            }
            catch (UnauthorizedAccessException)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token,
                    @"You don't have enough permissions to add or remove images from this screen" + Environment.NewLine 
                    + "To do that, you need to restart the application as an administrator");
            }
        }

        public string AddedFileName { get; set; }

        public bool DeleteLithologyApproved { get; set; }

        public Image SelectedLithology { get; set; }

        public ObservableCollection<Image> ImagesCollection { get; set; }

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
                    if (filename.Contains(".bmp"))
                    {
                        Image i = AddFileToCollection(filename);
                       // i.Name = Path.GetFileNameWithoutExtension(i.ToolTip.ToString());
                        ImagesCollection.Add(i);
                        //lbLits.Items.Add(i);
                    }
                }
            }
            catch (Exception ex)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithError(Token, ex.ToString());
            }
        }

        private static Image AddFileToCollection(string filename)
        {
            Image i = new Image();

            MemoryStream byteStream = new MemoryStream(File.ReadAllBytes(filename));

            BitmapImage bi = new BitmapImage();
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
