using GalaSoft.MvvmLight.Command;
using GAP.BL;
using System;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class HelpViewModel : BaseViewModel<BaseEntity>
    {
        ISubject<char> _subject = new Subject<char>();
        IDisposable _disposable;
        public HelpViewModel(string token)
            : base(token)
        {
           _disposable = _subject.Buffer(TimeSpan.FromSeconds(2)).Where(u => u.Count > 0).ObserveOnDispatcher().Subscribe(v =>
                {
                    ListOfItems = GlobalSearch.Instance.ExecuteQuery(SearchableText);
                });
        }

        public void CleanUp()
        {
            _disposable.Dispose();
        }

        ICommand _openScreenCommand;
        public ICommand OpenScreenCommand
        {
            get { return _openScreenCommand ?? (_openScreenCommand = new RelayCommand(OpenScreen)); }
        }
        private void OpenScreen()
        {
            if (SelectedItem == null) return;
            GlobalDataModel.Instance.SendMessage(Token, GAP.Helpers.NotificationMessageEnum.WindowScreen, SelectedItem);
        }

        HelpData _selectedItem;
        public HelpData SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
        }

        string _searchableText;
        public string SearchableText
        {
            get { return _searchableText; }
            set
            {
                _searchableText = value;
                if (!string.IsNullOrWhiteSpace(value))
                    _subject.OnNext(value[value.Length - 1]);
            }
        }
        IEnumerable<HelpData> _listOfItems;
        public IEnumerable<HelpData> ListOfItems
        {
            get { return _listOfItems; }
            set
            {
                _listOfItems = value;
                NotifyPropertyChanged("ListOfItems"); ;
            }
        }

    }//end class
}//end namespace
