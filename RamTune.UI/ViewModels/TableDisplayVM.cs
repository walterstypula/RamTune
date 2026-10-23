using MVVM;
using RamTune.Core;
using RamTune.Core.Metadata;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public static class ObservableCollectionExtension
    {
        public static ObservableCollection<ObservableCollection<CellVM>> ToCellObservableCollection(this IEnumerable<IEnumerable<byte[]>> tableData, Scaling scaling, bool? isStaticAxis, int? address)
        {
            if (tableData == null)
            {
                return null;
            }

            var elementSize = isStaticAxis.GetValueOrDefault() || scaling == null ? 1 : scaling.ParseStorageSize();

            var rows = tableData.Select(o =>
            {
                var columns = o.Select(s =>
                {
                    var cell = new CellVM
                    {
                        ByteValue = s,
                        Scaling = scaling,
                        IsStatic = isStaticAxis.GetValueOrDefault(),
                        Address = address.GetValueOrDefault()
                    };

                    if (address.HasValue)
                    {
                        address += elementSize;
                    }

                    return cell;
                }).ToList();

                return new ObservableCollection<CellVM>(columns);
            }).ToList();

            return new ObservableCollection<ObservableCollection<CellVM>>(rows);
        }
    }

    public class TableDisplayVM : ViewModelBase
    {
        private RelayCommand _fineIncrementCommand;
        private RelayCommand _fineDecrementCommand;
        private RelayCommand _coarseIncrementCommand;
        private RelayCommand _coarseDecrementCommand;
        private Table _table;
        private ITableReader _tableReader;

        public TableDisplayVM(Table table, ITableReader tableReader)
        {
            _table = table;
            _tableReader = tableReader;
            IsVisible = true;
            Load();
        }

        public ObservableCollection<ObservableCollection<CellVM>> ColumnHeaders { get; set; }

        public ObservableCollection<ObservableCollection<CellVM>> RowHeaders { get; set; }

        public ObservableCollection<ObservableCollection<CellVM>> TableData { get; set; }

        public string Category { get { return _table.Category; } }

        public string ColumnDescription { get; private set; }

        public bool IsDirty
        {
            get
            {
                return Get<bool>(nameof(IsDirty));
            }
            private set
            {
                Set(nameof(IsDirty), value);
            }
        }

        public bool IsVisible
        {
            get { return Get<bool>(nameof(IsVisible)); }
            set { Set(nameof(IsVisible), value); }
        }

        public string Name { get { return _table.Name; } }

        public string RowDescription { get; private set; }

        public object TableDescription { get; private set; }

        public ICommand FineDecrementCommand => this.InitCommand(ref _fineDecrementCommand, FineDecrement, delegate { return true; });

        public ICommand FineIncrementCommand => this.InitCommand(ref _fineIncrementCommand, FineIncrement, delegate { return true; });

        public ICommand CoarseDecrementCommand => this.InitCommand(ref _coarseDecrementCommand, CoarseDecrement, delegate { return true; });

        public ICommand CoarseIncrementCommand => this.InitCommand(ref _coarseIncrementCommand, CoarseIncrement, delegate { return true; });

        public void CoarseDecrement(object parm)
        {
            ModifyCells(TableData, Direction.Decrement, ChangeType.Coarse);
            ModifyCells(ColumnHeaders, Direction.Decrement, ChangeType.Coarse);
            ModifyCells(RowHeaders, Direction.Decrement, ChangeType.Coarse);
            Apply();
        }

        public void CoarseIncrement(object parm)
        {
            ModifyCells(TableData, Direction.Increment, ChangeType.Coarse);
            ModifyCells(ColumnHeaders, Direction.Increment, ChangeType.Coarse);
            ModifyCells(RowHeaders, Direction.Increment, ChangeType.Coarse);
            Apply();
        }

        public void FineDecrement(object parm)
        {
            ModifyCells(TableData, Direction.Decrement, ChangeType.Fine);
            ModifyCells(ColumnHeaders, Direction.Decrement, ChangeType.Fine);
            ModifyCells(RowHeaders, Direction.Decrement, ChangeType.Fine);
            Apply();
        }

        public void FineIncrement(object parm)
        {
            ModifyCells(TableData, Direction.Increment, ChangeType.Fine);
            ModifyCells(ColumnHeaders, Direction.Increment, ChangeType.Fine);
            ModifyCells(RowHeaders, Direction.Increment, ChangeType.Fine);
            Apply();
        }

        public void ResetAllTableCells()
        {
            ResetCollection(RowHeaders);
            ResetCollection(ColumnHeaders);
            ResetCollection(TableData);
            Apply();
        }

        public void ResetSelectedTableCells()
        {
            ResetSelectedCollection(RowHeaders);
            ResetSelectedCollection(ColumnHeaders);
            ResetSelectedCollection(TableData);
            Apply();
        }

        private void CheckDirty()
        {
            var isDirty = CheckDirty(ColumnHeaders) || CheckDirty(RowHeaders) || CheckDirty(TableData);
            IsDirty = isDirty;
        }

        private bool CheckDirty(ObservableCollection<ObservableCollection<CellVM>> cells)
        {
            var isDirty = cells?.SelectMany(columns => columns)
                                     .Any(cell => cell.IsDirty) ?? false;

            return isDirty;
        }

        private void Load()
        {
            var xAxis = _table.GetAxis(AxisType.XAxis, AxisType.StaticXAxis);
            var yAxis = _table.GetAxis(AxisType.YAxis, AxisType.StaticYAxis);

            var columnElements = xAxis?.Elements;
            var rowElements = yAxis?.Elements;

            if (xAxis == null && yAxis != null)
            {
                //Display YAxis in XAxis, display only.
                xAxis = yAxis;
                xAxis.Type = yAxis.IsStaticAxis() ? TableType.StaticXAxis : TableType.XAxis;
                yAxis = null;

                columnElements = rowElements;
                rowElements = 1;
            }

            ColumnDescription = xAxis?.ToString();
            RowDescription = yAxis?.ToString();
            TableDescription = _table.ToString();

            ColumnHeaders = _tableReader.LoadAxisData(xAxis).ToCellObservableCollection(xAxis?.Scaling, xAxis?.IsStaticAxis(), xAxis?.Address?.ConvertHexToInt());
            RowHeaders = _tableReader.LoadAxisData(yAxis).ToCellObservableCollection(yAxis?.Scaling, yAxis?.IsStaticAxis(), yAxis?.Address?.ConvertHexToInt());
            TableData = _tableReader.LoadTableData(_table, columnElements, rowElements).ToCellObservableCollection(_table.Scaling, false, _table?.Address?.ConvertHexToInt());
        }

        private void ModifyCells(ObservableCollection<ObservableCollection<CellVM>> cells, Direction direction, ChangeType changeType)
        {
            var selectedCells = cells?.SelectMany(columns => columns)
                                     .Where(cell => cell.IsSelected);

            selectedCells ??= Enumerable.Empty<CellVM>();

            foreach (var cell in selectedCells)
            {
                cell.ChangeValue(direction, changeType);
            }

            Apply();
        }

        private void Apply()
        {
            CheckDirty();

            var dirtyCells = new[] { TableData, ColumnHeaders, RowHeaders }
                                .Where(cells => cells != null)
                                .SelectMany(cells => cells.SelectMany(s => s))
                                .Where(s => s.IsDirty && !s.IsStatic);

            foreach (var cell in dirtyCells)
            {
                var endian = cell.Scaling.Endian;
                var bytes = cell.ByteValue.ToArray();

                bytes.ReverseBytes(endian);

                _tableReader.ApplyChanges(cell.Address.GetValueOrDefault(), bytes);
            }
        }

        /// <summary>
        /// True when data and axes have the same number of rows and columns as <paramref name="other"/>.
        /// </summary>
        public bool HasSameShape(TableDisplayVM other)
        {
            return SameShape(TableData, other.TableData)
                && SameShape(ColumnHeaders, other.ColumnHeaders)
                && SameShape(RowHeaders, other.RowHeaders);
        }

        public int SelectedCellCount
        {
            get { return AllCells().Count(c => c.IsSelected); }
        }

        /// <summary>
        /// Copies values from <paramref name="source"/> into this table, cell by cell at the same row and column,
        /// for the data and both axes. With <paramref name="selectedOnly"/> only the cells selected in the source are copied.
        /// The tables must have the same shape. Returns the number of cells whose value changed.
        /// </summary>
        public int CopyFrom(TableDisplayVM source, bool selectedOnly)
        {
            var changed = CopyCells(source.TableData, TableData, selectedOnly)
                        + CopyCells(source.ColumnHeaders, ColumnHeaders, selectedOnly)
                        + CopyCells(source.RowHeaders, RowHeaders, selectedOnly);

            Apply();

            return changed;
        }

        /// <summary>
        /// Flags every cell whose value differs from the cell at the same position in <paramref name="other"/>. Returns the number of differing cells.
        /// </summary>
        public int MarkDifferences(TableDisplayVM other)
        {
            return MarkDifferences(TableData, other?.TableData)
                 + MarkDifferences(ColumnHeaders, other?.ColumnHeaders)
                 + MarkDifferences(RowHeaders, other?.RowHeaders);
        }

        private IEnumerable<CellVM> AllCells()
        {
            return new[] { TableData, ColumnHeaders, RowHeaders }
                        .Where(cells => cells != null)
                        .SelectMany(cells => cells.SelectMany(s => s));
        }

        private static bool SameShape(ObservableCollection<ObservableCollection<CellVM>> a, ObservableCollection<ObservableCollection<CellVM>> b)
        {
            if (a == null || b == null)
            {
                return a == null && b == null;
            }

            return a.Count == b.Count && a.Zip(b, (ra, rb) => ra.Count == rb.Count).All(s => s);
        }

        private static int CopyCells(ObservableCollection<ObservableCollection<CellVM>> source, ObservableCollection<ObservableCollection<CellVM>> target, bool selectedOnly)
        {
            if (source == null || target == null)
            {
                return 0;
            }

            var changed = 0;

            for (var row = 0; row < source.Count && row < target.Count; row++)
            {
                for (var column = 0; column < source[row].Count && column < target[row].Count; column++)
                {
                    var from = source[row][column];
                    var to = target[row][column];

                    if ((selectedOnly && !from.IsSelected) || to.IsStatic || to.HasSameValueAs(from))
                    {
                        continue;
                    }

                    to.CopyValueFrom(from);
                    changed++;
                }
            }

            return changed;
        }

        private static int MarkDifferences(ObservableCollection<ObservableCollection<CellVM>> cells, ObservableCollection<ObservableCollection<CellVM>> other)
        {
            if (cells == null)
            {
                return 0;
            }

            var count = 0;

            for (var row = 0; row < cells.Count; row++)
            {
                for (var column = 0; column < cells[row].Count; column++)
                {
                    var cell = cells[row][column];
                    var match = other != null && row < other.Count && column < other[row].Count ? other[row][column] : null;

                    cell.IsDifferent = match == null || !cell.HasSameValueAs(match);

                    if (cell.IsDifferent)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void ResetCollection(ObservableCollection<ObservableCollection<CellVM>> cells)
        {
            var dirtyCells = cells?.SelectMany(columns => columns)
                                   .Where(cell => cell.IsDirty) ?? Enumerable.Empty<CellVM>();

            foreach (var cell in dirtyCells)
            {
                cell.ResetValue();
            }
        }

        private void ResetSelectedCollection(ObservableCollection<ObservableCollection<CellVM>> cells)
        {
            var dirtyCells = cells?.SelectMany(columns => columns)
                                  .Where(cell => cell.IsDirty && cell.IsSelected) ?? Enumerable.Empty<CellVM>();

            foreach (var cell in dirtyCells)
            {
                cell.ResetValue();
            }

            Apply();
        }
    }
}