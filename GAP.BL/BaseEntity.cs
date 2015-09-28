using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace GAP.BL
{
    public class BaseEntity : INotifyPropertyChanged
    {
        public bool IsValidInput(string value)
        {
            string regex = @"^[0-9a-zA-Z _]+$";
            return Regex.IsMatch(value, regex);            
        }
        public string ID { get; set; }
        public BaseEntity()
        {
            IsEntitySelected = false;
            ID = Guid.NewGuid().ToString();
            DisplayIndex = 0;
        }
        public delegate void EntitySelectionChanged(BaseEntity entity);

        public event EntitySelectionChanged OnEntitySelectionChanged;

        bool? _isEntitySelected;
        public bool? IsEntitySelected
        {
            get { return _isEntitySelected; }
            set
            {
                _isEntitySelected = value;
                NotifyPropertyChanged("IsEntitySelected");
                if (OnEntitySelectionChanged != null) OnEntitySelectionChanged(this);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        int _displayIndex;
        public int DisplayIndex
        {
            get { return _displayIndex; }
            set
            {
                _displayIndex = value;
                NotifyPropertyChanged("DisplayIndex");
            }
        }

        public void NotifyObject()
        {
            if (ObjectChanged != null)
                ObjectChanged();
        }
        string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                OldName = _name;
                _name = value;
                NotifyObject();
                NotifyPropertyChanged("Name");
            }
        }
        public string OldName { get; set; }
                
        public event Action ObjectChanged;
        
    }//end class

}//end namespace
