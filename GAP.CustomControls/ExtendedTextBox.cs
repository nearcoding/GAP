using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace GAP.CustomControls
{
    public class ExtendedTextBox : TextBox
    {
        public ExtendedTextBox()
        {
            this.LostFocus += ExtendedTextBox_LostFocus;
            this.PreviewTextInput += ExtendedTextBox_PreviewTextInput;
        }

        void ExtendedTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text != "." && e.Text!="-")
            {               

                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            else
            {
                if (this.Text.IndexOf(".") != -1)
                {                    
                    e.Handled = true;
                }
            }
        }


        public int DecimalPlaces { get; set; }

        void ExtendedTextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {

            decimal decValue = 0;
            if (!decimal.TryParse(this.Text, out decValue))
            {
                this.Text = "0";
            }
            else if (this.Text.IndexOf(".") >= 0)
            {
                var valsAfterDecimal = this.Text.Substring(this.Text.IndexOf("."));
                if (valsAfterDecimal.Length > 2)
                {
                    this.Text = this.Text.Substring(0, this.Text.IndexOf(".") + 3);                    
                }
            }
        }
    }
}
