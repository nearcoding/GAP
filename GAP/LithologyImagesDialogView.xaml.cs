using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GAP.BL;
using GAP.ExtendedControls;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;

namespace GAP
{
    /// <summary>
    /// Interaction logic for LithologyImagesDialogView.xaml
    /// </summary>
    public partial class LithologyImagesDialogView
    {
        public LithologyImagesDialogView()
        {
            InitializeComponent();
            _dataContext = new LithologyImagesViewModel(Token);
            DataContext = _dataContext;
        }

        LithologyImagesViewModel _dataContext;

        //this property is used to return the image to parent screen
        public Image SelectedImage { get; set; }

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.SelectLithologyImage:
                    SelectedImage = messageType.MessageObject as Image;
                    Close();
                    break;
            }
        }
    }
}
