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
                        address++;
                    }

                    return cell;
                });

                return new ObservableCollection<CellVM>(columns);
            });

            return new ObservableCollection<ObservableCollection<CellVM>>(rows);
        }
    }

    public class TableDisplayVM : ViewModelBase
    {
        private RelayCommand _addValueCommand;
        private RelayCommand _subtractValueCommand;
        private Table _table;
        private ITableReader _tableReader;

        public TableDisplayVM(Table table, ITableReader tableReader)
        {
            _table = table;
            _tableReader = tableReader;
            IsVisible = true;
            Load();
        }

        public ICommand AddValueCommand => this.InitCommand(ref _addValueCommand, AddValue, delegate { return true; });

        public string Category { get { return _table.Category; } }

        public string ColumnDescription { get; private set; }

        public ObservableCollection<ObservableCollection<CellVM>> ColumnHeaders { get; set; }

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

        public ObservableCollection<ObservableCollection<CellVM>> RowHeaders { get; set; }

        public ICommand SubtractValueCommand => this.InitCommand(ref _subtractValueCommand, SubtractValue, delegate { return true; });

        public ObservableCollection<ObservableCollection<CellVM>> TableData { get; set; }

        public object TableDescription { get; private set; }

        public void AddValue(object parm)
        {
            AddValue(TableData);
            AddValue(ColumnHeaders);
            AddValue(RowHeaders);
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

        public void SubtractValue(object parm)
        {
            SubtractValue(TableData);
            SubtractValue(ColumnHeaders);
            SubtractValue(RowHeaders);
            Apply();
        }

        private void AddValue(ObservableCollection<ObservableCollection<CellVM>> cells)
        {
            ModifyCells(cells, Direction.Increment);
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

        private void ModifyCells(ObservableCollection<ObservableCollection<CellVM>> cells, Direction direction)
        {
            var selectedCells = cells?.SelectMany(columns => columns)
                                     .Where(cell => cell.IsSelected);

            selectedCells ??= Enumerable.Empty<CellVM>();

            foreach (var cell in selectedCells)
            {
                cell.ChangeValue(direction);
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

        private void SubtractValue(ObservableCollection<ObservableCollection<CellVM>> cells)
        {
            ModifyCells(cells, Direction.Decrement);
        }
    }
}