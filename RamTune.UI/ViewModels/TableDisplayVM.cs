using MVVM;
using RamTune.Core;
using RamTune.Core.Metadata;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RamTune.UI.ViewModels
{
    public class TableDisplayVM : ViewModelBase
    {
        private Table _table;
        private ITableReader _tableReader;

        public ObservableCollection<CellVM> ColumnHeaders { get; set; }

        public ObservableCollection<CellVM> RowHeaders { get; set; }

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

            if (xAxis == null)
            {
                //Display YAxis in XAxis, display only.
                xAxis = yAxis;
                yAxis = null;

                columnElements = rowElements;
                rowElements = 1;
            }

            ColumnDescription = xAxis?.ToString();
            RowDescription = yAxis?.ToString();
            TableDescription = _table.ToString();

            ColumnHeaders = _tableReader.LoadAxisData(xAxis).ToCellObservableCollection();
            RowHeaders = _tableReader.LoadAxisData(yAxis).ToCellObservableCollection();
            TableData = _tableReader.LoadTableData(_table, columnElements, rowElements).ToCellObservableCollection();
        }
    }

    public static class ObservableCollectionExtension
    {
        public static ObservableCollection<ObservableCollection<CellVM>> ToCellObservableCollection(this IEnumerable<IEnumerable<string>> tableData)
        {
            var rows = tableData.Select(o =>
            {
                var columns = o.Select(s => new CellVM { Value = s });

                return new ObservableCollection<CellVM>(columns);
            });

            return new ObservableCollection<ObservableCollection<CellVM>>(rows);
        }

        public static ObservableCollection<CellVM> ToCellObservableCollection(this IEnumerable<string> list)
        {
            var cellList = list.Select(s => new CellVM { Value = s });

            return new ObservableCollection<CellVM>(cellList);
        }
    }
}