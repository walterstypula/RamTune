using MVVM;
using RamTune.Core;
using RamTune.Core.Metadata;
using System.Collections.Generic;

namespace RamTune.UI.ViewModels
{
    /// <summary>
    /// One table matched by name between the left and right ROM in the compare window.
    /// </summary>
    public class TableComparisonVM : ViewModelBase
    {
        private readonly TableComparison _comparison;
        private readonly ITableReader _leftReader;
        private readonly ITableReader _rightReader;
        private TableDisplayVM _leftTable;
        private TableDisplayVM _rightTable;

        public TableComparisonVM(TableComparison comparison, ITableReader leftReader, ITableReader rightReader)
        {
            _comparison = comparison;
            _leftReader = leftReader;
            _rightReader = rightReader;
            Status = comparison.Status;
            IsVisible = true;
        }

        public string Name => _comparison.Name;

        public string Category => _comparison.Category;

        public Table Left => _comparison.Left;

        public Table Right => _comparison.Right;

        public ComparisonStatus Status
        {
            get { return Get<ComparisonStatus>(nameof(Status)); }
            private set
            {
                Set(nameof(Status), value);
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public string StatusText => Status switch
        {
            ComparisonStatus.Identical => "Identical",
            ComparisonStatus.Different => DifferentCells > 0 ? $"{DifferentCells} cell(s) differ" : "Different",
            ComparisonStatus.SizeMismatch => "Different size in each ROM",
            ComparisonStatus.OnlyLeft => "Only in the left ROM's definition",
            ComparisonStatus.OnlyRight => "Only in the right ROM's definition",
            _ => string.Empty,
        };

        public int DifferentCells
        {
            get { return Get<int>(nameof(DifferentCells)); }
            private set
            {
                Set(nameof(DifferentCells), value);
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public bool IsVisible
        {
            get { return Get<bool>(nameof(IsVisible)); }
            set { Set(nameof(IsVisible), value); }
        }

        public TableDisplayVM LeftTable => _leftTable ??= Left == null ? null : new TableDisplayVM(Left, _leftReader);

        public TableDisplayVM RightTable => _rightTable ??= Right == null ? null : new TableDisplayVM(Right, _rightReader);

        public string LeftNote => Left == null ? "Not defined for the left ROM" : null;

        public string RightNote => Right == null ? "Not defined for the right ROM" : null;

        public IEnumerable<string> AxisAddresses(bool right)
        {
            var table = right ? Right : Left;

            foreach (var axis in table?.Axis ?? new List<Axis>())
            {
                if (!string.IsNullOrEmpty(axis.Address))
                {
                    yield return axis.Address.ToLowerInvariant();
                }
            }
        }

        /// <summary>
        /// Re-marks differing cells and updates the status from the loaded cells.
        /// </summary>
        public void Refresh()
        {
            var left = LeftTable;
            var right = RightTable;

            if (left == null || right == null)
            {
                left?.MarkDifferences(null);
                right?.MarkDifferences(null);
                return;
            }

            right.MarkDifferences(left);
            var count = left.MarkDifferences(right);

            Status = !left.HasSameShape(right)
                ? ComparisonStatus.SizeMismatch
                : count == 0 ? ComparisonStatus.Identical : ComparisonStatus.Different;

            DifferentCells = left.HasSameShape(right) ? count : 0;
        }

        /// <summary>
        /// Drops the loaded cells for one side, so they are read again from the ROM, and recomputes the status.
        /// Used when a copy into another table changed an axis this table shares.
        /// </summary>
        public void Reload(bool right)
        {
            if (right)
            {
                _rightTable = null;
            }
            else
            {
                _leftTable = null;
            }

            OnPropertyChanged(right ? nameof(RightTable) : nameof(LeftTable));

            if (Left != null && Right != null)
            {
                Status = RomComparer.CompareTable(Left, _leftReader, Right, _rightReader);
                DifferentCells = 0;
            }
        }
    }

    public class CompareGroupVM : ViewModelBase
    {
        public string Name { get; set; }

        public List<TableComparisonVM> Tables { get; set; }

        public bool IsVisible
        {
            get { return Get<bool>(nameof(IsVisible)); }
            set { Set(nameof(IsVisible), value); }
        }
    }
}
