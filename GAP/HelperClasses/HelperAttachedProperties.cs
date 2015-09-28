using System.Windows;
using System.Windows.Controls;

namespace GAP.HelperClasses
{
    public class HelperAttachedProperties : DependencyObject
    {

        public static Grid GetParentGrid(DependencyObject obj)
        {
            return (Grid)obj.GetValue(ParentGridProperty);
        }

        public static void SetParentGrid(DependencyObject obj, Grid value)
        {
            obj.SetValue(ParentGridProperty, value);
        }

        public static readonly DependencyProperty ParentGridProperty =
            DependencyProperty.RegisterAttached("ParentGrid", typeof(Grid), typeof(GridHelper), new PropertyMetadata(null));

        public static object GetAssociatedObject(DependencyObject obj)
        {
            return (object)obj.GetValue(AssociatedObjectProperty);
        }

        public static void SetAssociatedObject(DependencyObject obj, object value)
        {
            obj.SetValue(AssociatedObjectProperty, value);
        }

        public static readonly DependencyProperty AssociatedObjectProperty =
            DependencyProperty.RegisterAttached("AssociatedObject", typeof(object), typeof(GridHelper), new PropertyMetadata(null));

    }//end class
}//end namespace
