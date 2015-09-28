using GalaSoft.MvvmLight.Messaging;
using GAP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using GAP.MainUI.ViewModels.ViewModel;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class SendMessage : ISendMessage
    {
        private SendMessage()
        {

        }

        private static SendMessage _instance = new SendMessage();

        public static SendMessage Instance
        {
            get
            {
                return _instance;
            }
        }
                
        public void MessageBox(string message, string token)
        {
           GlobalDataModel.Instance.SendMessage(token, NotificationMessageEnum.MessageBox, message);
        }

        public void MessageBoxWithExclamation(string token, string message)
        {
            GlobalDataModel.Instance.SendMessage(token, NotificationMessageEnum.MessageBoxWithExclamation, message);
        }

        public void MessageBoxWithError(string token, string message)
        {
            GlobalDataModel.Instance.SendMessage(token, NotificationMessageEnum.MessageBoxWithError, message);
        }
        
        public void MessageBoxWithInformation(string token, string message)
        {
            GlobalDataModel.Instance.SendMessage(token, NotificationMessageEnum.MessageBoxWithInformation, message);
        }
        
        public void MessageBoxWithQuestion(string token, string message)
        {
            GlobalDataModel.Instance.SendMessage(token, NotificationMessageEnum.MessageBoxWithQuestion, message);
        }
    }
}
