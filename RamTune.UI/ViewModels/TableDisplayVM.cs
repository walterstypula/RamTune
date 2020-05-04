using MVVM;
using RamTune.Core;
using RamTune.Core.Metadata;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class TableDisplayVM : ViewModelBase
    {
        private Table _table;
        private ITableReader _tableReader;

        public ObservableCollection<ObservableCollection<CellVM>> ColumnHeaders { get; set; }

        public ObservableCollection<ObservableCollection<CellVM>> RowHeaders { get; set; }

        public ObservableCollection<ObservableCollection<CellVM>> TableData { get; set; }

        public string ColumnDescription { get; private set; }

        public string RowDescription { get; private set; }

        public object TableDescription { get; private set; }

        public TableDisplayVM(Table table, ITableReader tableReader)
        {
            _table = table;
            _tableReader = tableReader;
            Load();
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
                xAxis.Type =  yAxis.IsStaticAxis() ? TableType.StaticXAxis : TableType.XAxis;
                yAxis = null;

                columnElements = rowElements;
                rowElements = 1;
            }

            ColumnDescription = xAxis?.ToString();
            RowDescription = yAxis?.ToString();
            TableDescription = _table.ToString();

            ColumnHeaders = _tableReader.LoadAxisData(xAxis).ToCellObservableCollection(xAxis?.Scaling);
            RowHeaders = _tableReader.LoadAxisData(yAxis).ToCellObservableCollection(yAxis?.Scaling);
            TableData = _tableReader.LoadTableData(_table, columnElements, rowElements).ToCellObservableCollection(_table.Scaling);
        }

        private RelayCommand _addValueCommand;
        public ICommand AddValueCommand => this.InitCommand(ref _addValueCommand, AddValue, delegate { return true; });

        private RelayCommand _subtractValueCommand;
        public ICommand SubtractValueCommand => this.InitCommand(ref _subtractValueCommand, SubtractValue, delegate { return true; });

        public void AddValue(object parm)
        {
            AddValue(TableData);
            AddValue(ColumnHeaders);
            AddValue(RowHeaders);
        }

        public void SubtractValue(object parm)
        {
            SubtractValue(TableData);
            SubtractValue(ColumnHeaders);
            SubtractValue(RowHeaders);
        }

        private void AddValue(ObservableCollection<ObservableCollection<CellVM>> cells)
        {
            if(cells == null)
            {
                return;
            }

            foreach (var row in cells)
            {
                foreach (var column in row)
                {
                    if (column.IsSelected)
                    {
                        column.IncrementValue();
                    }
                }
            }
        }

        private void SubtractValue(ObservableCollection<ObservableCollection<CellVM>> cells)
        {
            if (cells == null)
            {
                return;
            }

            foreach (var row in cells)
            {
                foreach (var column in row)
                {
                    if (column.IsSelected)
                    {
                        column.DecrementValue();
                    }
                }
            }
        }
    }

    public static class ObservableCollectionExtension
    {
        public static ObservableCollection<ObservableCollection<CellVM>> ToCellObservableCollection(this IEnumerable<IEnumerable<string>> tableData, Scaling scaling)
        {
            if(tableData == null)
            {
                return null;
            }

            var rows = tableData.Select(o =>
            {
                var columns = o.Select(s => new CellVM { Value = s, Scaling = scaling });

                return new ObservableCollection<CellVM>(columns);
            });

            return new ObservableCollection<ObservableCollection<CellVM>>(rows);
        }
    }
}