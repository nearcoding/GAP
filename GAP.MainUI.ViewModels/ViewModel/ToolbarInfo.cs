
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class ToolbarInfo
    {
        public ToolbarInfo()
        {

        }
        public ToolbarInfo(string text, string image, int displayIndex,
            ControlType control = ControlType.Button, string command = "", bool defaultValue = false)
        {
            Text = text;
            Image = image;
            DisplayIndex = displayIndex;
            Control = control;
            Command = command;
            Value = defaultValue;
        }
        public bool Value { get; set; }

        public string DisplayText { get; set; }

        string _command;

        public string Command
        {
            get
            {
                return _command;
            }
            set
            {
                _command = value;
            }
        }

        public string Image { get; set; }
        public string Text { get; set; }

        public int DisplayIndex { get; set; }

        public ControlType Control { get; set; }
    }

    public enum ControlType
    {
        Button,
        Checkbox,
        Separator
    }
}
