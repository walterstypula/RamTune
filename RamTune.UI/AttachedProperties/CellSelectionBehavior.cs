using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RamTune.UI.AttachedProperties
{
    public class CellSelectionBehavior : FrameworkElement
    {
        private static bool _isMouseDown;
        private static UIElement _test;

        public static readonly DependencyProperty MouseDownPrecedenceProperty =
           DependencyProperty.RegisterAttached("MouseDownPrecedence", typeof(bool), typeof(CellSelectionBehavior), new UIPropertyMetadata(false, OnMouseDownChanged));

        public static bool GetMouseDownPrecedence(UIElement obj)
        {
            return (bool)obj.GetValue(MouseDownPrecedenceProperty);
        }

        public static void SetMouseDownPrecedence(UIElement obj, bool value)
        {
            obj.SetValue(MouseDownPrecedenceProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(CellSelectionBehavior), new PropertyMetadata(false, IsSelectedChanged));

        public static string GetIsSelected(DependencyObject obj)
        {
            return (string)obj.GetValue(IsSelectedProperty);
        }

        public static void SetIsSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectedProperty, value);
        }

        private static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public static int SelectionRangeColumnStart { get; private set; }
        public static int SelectionRangeColumnEnd { get; private set; }
        public static int SelectionRangeRowStart { get; private set; }
        public static int SelectionRangeRowEnd { get; private set; }
        public static int InitialRowStart { get; private set; }
        public static int InitialColumnStart { get; private set; }

        private static void OnMouseDownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((UIElement)d).MouseDown += IsMouseDownBehavior_MouseDown;
            ((UIElement)d).MouseUp += IsMouseDownBehavior_MouseUp;
            ((UIElement)d).MouseEnter += IsMouseDownBehavior_MouseEnter;
        }

        private static void IsMouseDownBehavior_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is UIElement uiElement)
            {
                SelectionOnMouseMove(uiElement);
            }
        }

        private static void IsMouseDownBehavior_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement uiElement)
            {
                _isMouseDown = true;

                if (_test != null)
                {
                    ForceDeselect(_test);
                }

                _test = FindParentItemsControl(uiElement);
                SelectionOnMouseDown(uiElement);
            }
        }

        private static ItemsControl FindParentItemsControl(UIElement uiElement)
        {
            var first = VisualTreeExtensions.FindAncestor<ItemsControl>(uiElement);
            return VisualTreeExtensions.FindAncestor<ItemsControl>(first);
        }

        private static void SelectionOnMouseDown(UIElement uiElement)
        {
            if (_isMouseDown && _test == FindParentItemsControl(uiElement))
            {
                var position = GetControlPositionInGrid(uiElement);

                InitialColumnStart = position.Column;
                InitialRowStart = position.Row;

                SelectionRangeColumnStart = position.Column;
                SelectionRangeColumnEnd = position.Column;
                SelectionRangeRowStart = position.Row;
                SelectionRangeRowEnd = position.Row;

                IsInSelectionRange(uiElement);
            }
        }

        private static (int Row, int Column) GetControlPositionInGrid(UIElement uiElement)
        {
            var columnContentPresenter = VisualTreeExtensions.FindAncestor<ContentPresenter>(uiElement);
            var columnUniformGrid = VisualTreeExtensions.FindAncestor<UniformGrid>(columnContentPresenter);
            var column = columnUniformGrid.Children.IndexOf(columnContentPresenter);

            var rowContentPresenter = VisualTreeExtensions.FindAncestor<ContentPresenter>(columnUniformGrid);
            var rowUniformGrid = VisualTreeExtensions.FindAncestor<UniformGrid>(rowContentPresenter);
            var row = rowUniformGrid.Children.IndexOf(rowContentPresenter);

            return (row, column);
        }

        private static void IsMouseDownBehavior_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

        private static void SelectionOnMouseMove(UIElement uiElement)
        {
            if (_isMouseDown && _test == FindParentItemsControl(uiElement))
            {
                var position = GetControlPositionInGrid(uiElement);

                if (position.Column <= InitialColumnStart)
                {
                    SelectionRangeColumnStart = position.Column;
                }

                if (position.Column >= InitialColumnStart)
                {
                    SelectionRangeColumnEnd = position.Column;
                }

                if (position.Row <= InitialRowStart)
                {
                    SelectionRangeRowStart = position.Row;
                }

                if (position.Row >= InitialRowStart)
                {
                    SelectionRangeRowEnd = position.Row;
                }

                IsInSelectionRange(uiElement);
            }
        }

        private static void ForceDeselect(UIElement uiElement)
        {
            var rowUniformGrid = VisualTreeExtensions.FindChild<UniformGrid>(uiElement);
            
            for (int i = 0; i < rowUniformGrid.Children.Count; i++)
            {
                var item = VisualTreeExtensions.FindChild<UniformGrid>(rowUniformGrid.Children[i]);

                for (int j = 0; j < item.Children.Count; j++)
                {
                    var grid = VisualTreeExtensions.FindChild<Grid>(item.Children[j]);
                    SetIsSelected(grid, false);
                }
            }
        }

        private static void IsInSelectionRange(UIElement uiElement)
        {
            if (_test != FindParentItemsControl(uiElement))
            {
                return;
            }

            var columnContentPresenter = VisualTreeExtensions.FindAncestor<ContentPresenter>(uiElement);
            var columnUniformGrid = VisualTreeExtensions.FindAncestor<UniformGrid>(columnContentPresenter);

            var rowContentPresenter = VisualTreeExtensions.FindAncestor<ContentPresenter>(columnUniformGrid);
            var rowUniformGrid = VisualTreeExtensions.FindAncestor<UniformGrid>(rowContentPresenter);

            for (int i = 0; i < rowUniformGrid.Children.Count; i++)
            {
                var item = VisualTreeExtensions.FindChild<UniformGrid>(rowUniformGrid.Children[i]);

                for (int j = 0; j < item.Children.Count; j++)
                {
                    var grid = VisualTreeExtensions.FindChild<Grid>(item.Children[j]);
                    if (j >= SelectionRangeColumnStart && j <= SelectionRangeColumnEnd && i >= SelectionRangeRowStart && i <= SelectionRangeRowEnd)
                    {
                        SetIsSelected(grid, true);
                    }
                    else
                    {
                        SetIsSelected(grid, false);
                    }
                }
            }
        }
    }
}