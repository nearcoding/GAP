using System.Collections.Generic;
using System.Windows;
using Ninject;
using GAP.BL;
using GAP.HelperClasses;
using GAP.MainUI.ViewModels.ViewModel;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using System.Windows.Input;
using System.Windows.Controls;

namespace GAP
{
    /// <summary>
    /// Interaction logic for DatasetImportMappingView.xaml
    /// </summary>
    public partial class DatasetImportMappingView
    {
        public DatasetImportMappingView(List<Dataset> dataset ) 
        {
            InitializeComponent();

            _dataContext = new DatasetImportMappingViewModel(Token, dataset);
            DataContext = _dataContext;
            AddKeyBindings<Dataset>();
            KeyBinding saveKeyBinding =
            new KeyBinding(_dataContext.SaveDatasetCommand, Key.S, ModifierKeys.Control);
            this.InputBindings.Add(saveKeyBinding);
        }

        DatasetImportMappingViewModel _dataContext;

        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch(messageType.MessageType)
            {
                case NotificationMessageEnum.RefreshListbox:
                    lbDatasets.Items.Refresh();
                    break;              
                case NotificationMessageEnum.UpdateDatasetWithMinMaxWarning:
                    UpdateDatasetWithMinMaxWarning();
                    break;
                case NotificationMessageEnum.ImportDataset:                    
                    IsSaved = true; //this property notifies import data screen to close itself as data has been imported
                    GlobalData.ShouldSave = true;
                    Close();
                    break;
            }            
        }

        private void UpdateDatasetWithMinMaxWarning()
        {
            var result = MessageBox.Show(IoC.Kernel.Get<IResourceHelper>().ReadResource("MinMaxValidation"), GlobalData.MESSAGEBOXTITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                _dataContext.UpdateListBoxWithMarkDatasetsFromView();
        }
    }
}
