using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace GAP.ExtendedControls
{
    public class TextBoxExtended : TextBox//, IDisposable
    {
        public TextBoxExtended()
        {
            GotFocus += TextBoxBehavior_GotFocus;
        }

        void TextBoxBehavior_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            txt.SelectionStart = 0;
            txt.SelectionLength = txt.Text.Length;
        }

        //public void Dispose()
        //{
        //    GotFocus -= TextBoxBehavior_GotFocus;
        //}
    }//end class
}///end namespace
