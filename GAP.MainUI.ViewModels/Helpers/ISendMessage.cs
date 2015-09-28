using System;
namespace GAP.MainUI.ViewModels.Helpers
{
    public interface ISendMessage
    {
        void MessageBox(string message, string token);
        void MessageBoxWithError(string token, string message);
        void MessageBoxWithExclamation(string token, string message);
        void MessageBoxWithInformation(string token, string message);

        void MessageBoxWithQuestion(string token, string message);
    }
}
