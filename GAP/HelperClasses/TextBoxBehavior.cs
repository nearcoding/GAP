using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GAP.HelperClasses
{
    // public class TextBoxBehavior:Behavior<TextBox>
    public class TextBoxExtended: TextBox
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

        //protected override void OnAttached()
        //{
        //    base.OnAttached();
        //    this.AssociatedObject.GotFocus += AssociatedObject_GotFocus;
        //}   

        //void AssociatedObject_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    var txt = sender as TextBox;
        //    txt.SelectionStart = 0;
        //    txt.SelectionLength = txt.Text.Length;
        //}

        //protected override void OnDetaching()
        //{
        //    base.OnDetaching();
        //}
    }//end class
}//end namespace
