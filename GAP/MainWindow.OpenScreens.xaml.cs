
namespace GAP
{
    public partial class MainWindow
    {
        private void EditProject()
        {
            if (_dataContext.SelectedProject != null && !string.IsNullOrWhiteSpace(_dataContext.SelectedProject.Name))            
                new ProjectView(_dataContext.SelectedProject.ID).ShowDialog();            
        }

        private void EditDataset()
        {
            if (_dataContext.SelectedDataset != null && !string.IsNullOrWhiteSpace(_dataContext.SelectedDataset.Name))            
                new DatasetView(_dataContext.SelectedDataset).Show();            
        }

        private void EditSpreadsheet()
        {
            if (_dataContext.SelectedDataset != null && !string.IsNullOrWhiteSpace(_dataContext.SelectedDataset.Name))            
                new MaintainSpreadsheetView(_dataContext.SelectedDataset, true).ShowDialog();            
        }
    }//end class
}//end namespace
