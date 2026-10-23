using RamTune.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RamTune.UI.Views
{
    /// <summary>
    /// Interaction logic for CompareWindow.xaml
    /// </summary>
    public partial class CompareWindow : Window
    {
        private ScrollViewer _syncTarget;

        public CompareWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));
        }

        /// <summary>
        /// Keeps the left and right tables scrolled to the same position.
        /// </summary>
        private void TableScroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender == _syncTarget)
            {
                // The other side catching up with our own sync; don't send it back.
                _syncTarget = null;
                return;
            }

            if (e.HorizontalChange == 0 && e.VerticalChange == 0)
            {
                // Only the content size changed (a different table was selected).
                return;
            }

            var other = sender == LeftScroll ? RightScroll : LeftScroll;
            var horizontal = Math.Min(e.HorizontalOffset, other.ScrollableWidth);
            var vertical = Math.Min(e.VerticalOffset, other.ScrollableHeight);

            if (Math.Abs(other.HorizontalOffset - horizontal) < 0.5 && Math.Abs(other.VerticalOffset - vertical) < 0.5)
            {
                return;
            }

            _syncTarget = other;
            other.ScrollToHorizontalOffset(horizontal);
            other.ScrollToVerticalOffset(vertical);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DataContext is CompareWindowVM vm && vm.HasUnsavedChanges)
            {
                var answer = MessageBox.Show(this, "Copied values haven't been saved. Close the compare window anyway?", "RamTune", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                e.Cancel = answer != MessageBoxResult.Yes;
            }

            base.OnClosing(e);
        }
    }
}
