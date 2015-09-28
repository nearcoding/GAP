using AutoMapper;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using Ninject;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class CustomizeToolbarViewModel : BaseViewModel<BaseEntity>
    {
        public CustomizeToolbarViewModel(string token)
            : base(token)
        {
            Mapper.CreateMap<ToolbarInfo, ToolbarInfo>();
            var lst = (List<ToolbarInfo>)Mapper.Map(GlobalDataModel.AvailableToolbarItems, typeof(List<ToolbarInfo>), typeof(List<ToolbarInfo>));
            AvailableToolbarItems = new ObservableCollection<ToolbarInfo>(lst);

            var lstCurrent = GlobalSerializer.DeserializeToolbar();
            if (lstCurrent == null) return;
            ToolbarItems = new ObservableCollection<ToolbarInfo>(lstCurrent);
                        
            foreach (var info in ToolbarItems)
            {
                AvailableToolbarItems.Remove(AvailableToolbarItems.SingleOrDefault(u => u.Text == info.Text));
            }
            foreach (var info in ToolbarItems)
            {
                info.DisplayText = IoC.Kernel.Get<IResourceHelper>().ReadResource(info.Text);
            }

            foreach (var info in AvailableToolbarItems)
            {
                info.DisplayText = IoC.Kernel.Get<IResourceHelper>().ReadResource(info.Text);
            }
        }

        ICommand _insertSeparatorCommand, _removeSeparatorCommand;

        public ICommand RemoveSeparatorCommand
        {
            get { return _removeSeparatorCommand ?? (_removeSeparatorCommand = new RelayCommand(RemoveSeparator, CanRemoveSeparator)); }
        }

        private bool CanRemoveSeparator()
        {
            return SelectedToolbarItem != null && SelectedToolbarItem.Control == ControlType.Separator;
        }

        private void RemoveSeparator()
        {
            ToolbarItems.Remove(SelectedToolbarItem);
        }

        public ICommand InsertSeparatorCommand
        {
            get { return _insertSeparatorCommand ?? (_insertSeparatorCommand = new RelayCommand(InsertSeparator, () => SelectedToolbarItem != null)); }
        }

        private void InsertSeparator()
        {
            ToolbarItems.Insert(ToolbarItems.IndexOf(SelectedToolbarItem), new ToolbarInfo
            {
                Text = "-------------------",
                Control = ControlType.Separator
            });
        }

        public ObservableCollection<ToolbarInfo> AvailableToolbarItems
        {
            get { return _availableToolbarItems; }
            set
            {
                _availableToolbarItems = value;
                NotifyPropertyChanged("AvailableToolbarItems");
            }
        }

        public ObservableCollection<ToolbarInfo> ToolbarItems
        {
            get { return _toolbarItems; }
            set
            {
                _toolbarItems = value;
                NotifyPropertyChanged("ToolbarItems");
            }
        }

        ToolbarInfo _selectedToolbarItem;
        public ToolbarInfo SelectedToolbarItem
        {
            get { return _selectedToolbarItem; }
            set
            {
                _selectedToolbarItem = value;
                NotifyPropertyChanged("SelectedToolbarItem");
            }
        }


        ToolbarInfo _selectedAvailableItem;
        public ToolbarInfo SelectedAvailableItem
        {
            get { return _selectedAvailableItem; }
            set
            {
                _selectedAvailableItem = value;
                NotifyPropertyChanged("SelectedAvailableItem");
            }
        }

        ObservableCollection<ToolbarInfo> _toolbarItems, _availableToolbarItems;

        ICommand _addItemToToolboxCommand, _addAllItemsToToolboxCommand, _removeItemFromToolboxCommand,
            _removeAllItemsFromToolboxCommand, _upCommand, _downCommand;

        public ICommand UpCommand
        {
            get { return _upCommand ?? (_upCommand = new RelayCommand(ButtonUpItem, () => SelectedToolbarItem != null)); }
        }

        private void ButtonUpItem()
        {
            var selectedIndex = ToolbarItems.IndexOf(SelectedToolbarItem);
            if (selectedIndex <= 0) return;

            var obj = (ToolbarInfo)Mapper.Map(SelectedToolbarItem, typeof(ToolbarInfo), typeof(ToolbarInfo));
            ToolbarItems.Remove(SelectedToolbarItem);
            ToolbarItems.Insert(selectedIndex - 1, obj);
            SelectedToolbarItem = obj;
        }
        private void ButtonDownItem()
        {
            var selectedIndex = ToolbarItems.IndexOf(SelectedToolbarItem);
            if (selectedIndex == -1 || selectedIndex >= ToolbarItems.Count - 1) return;
            var obj = (ToolbarInfo)Mapper.Map(SelectedToolbarItem, typeof(ToolbarInfo), typeof(ToolbarInfo));
            ToolbarItems.Remove(SelectedToolbarItem);
            ToolbarItems.Insert(selectedIndex + 1, obj);
            SelectedToolbarItem = obj;
        }

        public ICommand DownCommand
        {
            get { return _downCommand ?? (_downCommand = new RelayCommand(ButtonDownItem, () => SelectedToolbarItem != null)); }
        }

        public ICommand RemoveAllItemsFromToolboxCommand
        {
            get { return _removeAllItemsFromToolboxCommand ?? (_removeAllItemsFromToolboxCommand = new RelayCommand(RemoveAllItemsFromToolbox, () => ToolbarItems.Any())); }
        }

        private void RemoveAllItemsFromToolbox()
        {
            foreach (var info in ToolbarItems)
            {
                AvailableToolbarItems.Add(info);
            }
            ToolbarItems.Clear();
        }

        public ICommand RemoveItemFromToolboxCommand
        {
            get { return _removeItemFromToolboxCommand ?? (_removeItemFromToolboxCommand = new RelayCommand(RemoveItemsFromToolbox, () => SelectedToolbarItem != null)); }
        }

        private void RemoveItemsFromToolbox()
        {
            if (SelectedToolbarItem == null) return;
            AvailableToolbarItems.Add(SelectedToolbarItem);
            var selectedIndex = ToolbarItems.IndexOf(SelectedToolbarItem);
            ToolbarItems.Remove(SelectedToolbarItem);
            if (!ToolbarItems.Any()) return;
            SelectedToolbarItem = ToolbarItems.Count > selectedIndex ? ToolbarItems[selectedIndex] : ToolbarItems[selectedIndex - 1];
        }

        public ICommand AddItemToToolboxCommand
        {
            get { return _addItemToToolboxCommand ?? (_addItemToToolboxCommand = new RelayCommand(AddItemToToolbox, () => SelectedAvailableItem != null)); }
        }

        private void AddItemToToolbox()
        {
            if (SelectedAvailableItem == null) return;
            ToolbarItems.Add(SelectedAvailableItem);
            var selectedIndex = AvailableToolbarItems.IndexOf(SelectedAvailableItem);
            AvailableToolbarItems.Remove(SelectedAvailableItem);
            if (!AvailableToolbarItems.Any()) return;

            SelectedAvailableItem = AvailableToolbarItems.Count > selectedIndex
                ? AvailableToolbarItems[selectedIndex] : AvailableToolbarItems[selectedIndex - 1];
        }

        public ICommand AddAllItemsToToolboxCommand
        {
            get { return _addAllItemsToToolboxCommand ?? (_addAllItemsToToolboxCommand = new RelayCommand(AddAllItemsToToolbox, () => AvailableToolbarItems.Any())); }
        }

        private void AddAllItemsToToolbox()
        {
            foreach (var info in AvailableToolbarItems)
                ToolbarItems.Add(info);

            AvailableToolbarItems.Clear();
        }

        public override void Save()
        {
            for (var i = 0; i < ToolbarItems.Count; i++)
                ToolbarItems[i].DisplayIndex = i;
            string folderName = IoC.Kernel.Get<IGlobalDataModel>().GetAppDataFolder();
            GlobalSerializer.SerializeObject(ToolbarItems, folderName + "//ToolbarInfo.xml");
            IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.BindToolBox();
            IoC.Kernel.Get<ISendMessage>().MessageBoxWithInformation(Token,
                IoC.Kernel.Get<IResourceHelper>().ReadResource("ToolbarSavedSuccessfully"));
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseScreen);
        }
    }//end class
}//end namespace
