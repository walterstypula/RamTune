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
        public static ObservableCollection<ObservableCollection<CellVm>> ToCellObservableCollection(this IEnumerable<IEnumerable<byte[]>> tableData, Scaling scaling, bool? isStaticAxis, int? address)
        {
            if (tableData == null)
            {
                return null;
            }

            var rows = tableData.Select(o =>
            {
                var columns = o.Select(s =>
                {
                    var cell = new CellVm
                    {
                        ByteValue = s,
                        Scaling = scaling,
                        IsStatic = isStaticAxis.GetValueOrDefault(),
                        Address = address.GetValueOrDefault()
                    };

                    if (address.HasValue)
                    {
                        address++;
                    }

                    return cell;
                });

                return new ObservableCollection<CellVm>(columns);
            });

            return new ObservableCollection<ObservableCollection<CellVm>>(rows);
        }
    }

    public class TableDisplayVm : ViewModelBase
    {
        private RelayCommand _fineIncrementCommand;
        private RelayCommand _fineDecrementCommand;
        private RelayCommand _coarseIncrementCommand;
        private RelayCommand _coarseDecrementCommand;
        private Table _table;
        private ITableReader _tableReader;

        public TableDisplayVm(Table table, ITableReader tableReader)
        {
            _table = table;
            _tableReader = tableReader;
            IsVisible = true;
            Load();
        }

        public ObservableCollection<ObservableCollection<CellVm>> ColumnHeaders { get; set; }

        public ObservableCollection<ObservableCollection<CellVm>> RowHeaders { get; set; }

        public ObservableCollection<ObservableCollection<CellVm>> TableData { get; set; }

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

        private bool CheckDirty(ObservableCollection<ObservableCollection<CellVm>> cells)
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

        private void ModifyCells(ObservableCollection<ObservableCollection<CellVm>> cells, Direction direction, ChangeType changeType)
        {
            var selectedCells = cells?.SelectMany(columns => columns)
                                     .Where(cell => cell.IsSelected);

            selectedCells ??= Enumerable.Empty<CellVm>();

            foreach (var cell in selectedCells)
            {
                cell.ChangeValue(direction, changeType);
            }

            Apply();
        }

        private void Apply()
        {
            CheckDirty();

            var dirtyCells = TableData.SelectMany(s => s).Where(s => s.IsDirty);

            foreach (var cell in dirtyCells)
            {
                var endian = this._table.Scaling.Endian;
                var bytes = cell.ByteValue.ToArray();

                bytes.ReverseBytes(endian);

                _tableReader.ApplyChanges(cell.Address.GetValueOrDefault(), bytes);
            }
        }

        private void ResetCollection(ObservableCollection<ObservableCollection<CellVm>> cells)
        {
            var dirtyCells = cells?.SelectMany(columns => columns)
                                   .Where(cell => cell.IsDirty) ?? Enumerable.Empty<CellVm>();

            foreach (var cell in dirtyCells)
            {
                cell.ResetValue();
            }
        }

        private void ResetSelectedCollection(ObservableCollection<ObservableCollection<CellVm>> cells)
        {
            var dirtyCells = cells?.SelectMany(columns => columns)
                                  .Where(cell => cell.IsDirty && cell.IsSelected) ?? Enumerable.Empty<CellVm>();

            foreach (var cell in dirtyCells)
            {
                cell.ResetValue();
            }

            Apply();
        }
    }
}