using GAP.BL;
using System;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public interface IProjectViewModel
    {
        void Save();
        bool AutoSave { get; set; }
    }//end class
}//end namespace
