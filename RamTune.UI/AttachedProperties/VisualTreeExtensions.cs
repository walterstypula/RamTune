using System.Windows;
using System.Windows.Media;

namespace RamTune.UI.AttachedProperties
{
    public static class VisualTreeExtensions
    {
        public static T FindAncestor<T>(DependencyObject obj) where T : DependencyObject
        {
            var target = obj;
            do
            {
                target = GetParent(target);
            }
            while (target != null && !(target is T));

            return target as T;
        }

        public static T FindChild<T>(this DependencyObject reference) where T : DependencyObject
        {
            if (reference == null)
            {
                return null;
            }

            T foundChild = null;

            var childrenCount = VisualTreeHelper.GetChildrenCount(reference);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(reference, i);
                // If the child is not of the request child type child
                if (!(child is T))
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child);
                }
                else
                {
                    // child element found.
                    foundChild = child as T;
                    break;
                }
            }

            return foundChild;
        }

        public static DependencyObject GetParent(DependencyObject obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (!(obj is ContentElement ce))
            {
                return VisualTreeHelper.GetParent(obj);
            }

            var parent = ContentOperations.GetParent(ce);
            if (parent != null)
            {
                return parent;
            }

            return ce is FrameworkContentElement fce
                ? fce.Parent
                : null;
        }
    }
}