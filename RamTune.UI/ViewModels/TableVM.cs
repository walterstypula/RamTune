using MVVM;
using RamTune.Core;
using RamTune.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class TableVM : ViewModelBase
    {
        private Table selectedTable;
        private ITableReader tableReader;

        public TableVM(Table selectedTable, ITableReader tableReader)
        {
            this.selectedTable = selectedTable;
            this.tableReader = tableReader;
            SelectedCellIndexes = new ObservableCollection<int[]>();
            PopulateTable();
        }

        private void PopulateTable()
        {
            var xAxis = selectedTable.GetAxis(AxisType.XAxis, AxisType.StaticXAxis);
            var yAxis = selectedTable.GetAxis(AxisType.YAxis, AxisType.StaticYAxis);
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

            ColumnDesc = xAxis?.ToString();
            RowDesc = yAxis?.ToString();
            TableDesc = selectedTable.ToString();

            ColumnHeaders = tableReader.LoadAxisData(xAxis);
            RowHeaders = tableReader.LoadAxisData(yAxis);
            Data = tableReader.LoadTableData(selectedTable, columnElements, rowElements);

            if (decimal.TryParse(selectedTable.Scaling.Inc, out var increment))
            {
                Increment = increment;
            }
        }

        public decimal Increment
        {
            get { return Get<decimal>(nameof(Increment)); }
            set { Set(nameof(Increment), value); }
        }

        public List<string> ColumnHeaders
        {
            get { return Get<List<string>>(nameof(ColumnHeaders)); }
            set { Set(nameof(ColumnHeaders), value); }
        }

        public List<string> RowHeaders
        {
            get { return Get<List<string>>(nameof(RowHeaders)); }
            set { Set(nameof(RowHeaders), value); }
        }

        public object Data
        {
            get { return Get<object>(nameof(Data)); }
            set { Set(nameof(Data), value); }
        }

        public string ColumnDesc
        {
            get { return Get<string>(nameof(ColumnDesc)); }
            set { Set(nameof(ColumnDesc), value); }
        }

        public string RowDesc
        {
            get { return Get<string>(nameof(RowDesc)); }
            set { Set(nameof(RowDesc), value); }
        }

        public string TableDesc
        {
            get { return Get<string>(nameof(TableDesc)); }
            set { Set(nameof(TableDesc), value); }
        }

        public void AddValue(object parm)
        {
            if (parm is decimal)
            {
                var value = (decimal)parm;
                ModifyMapAction(value);
            }
        }

        public void SubtractValue(object parm)
        {
            if (parm is decimal)
            {
                var value = (decimal)parm;
                ModifyMapAction(-value);
            }
        }

        public bool CanDoSomething(object parm) => true;

        private RelayCommand _addValueCommand;
        public ICommand AddValueCommand => this.InitCommand(ref _addValueCommand, AddValue, CanDoSomething);

        private RelayCommand _subtractValueCommand;
        public ICommand SubtractValueCommand => this.InitCommand(ref _subtractValueCommand, SubtractValue, CanDoSomething);

        public void ModifyMapAction(decimal value)
        {
            ModifyAddSelectedCells(value, Data);

            OnPropertyChanged(nameof(Data));
        }

        public ObservableCollection<int[]> SelectedCellIndexes
        {
            get { return Get<ObservableCollection<int[]>>("SelectedCellIndexes"); }
            set { Set("SelectedCellIndexes", value); }
        }

        private void ModifyAddSelectedCells(decimal value, object obj)
        {
            var table = obj as string[,];

            foreach (int[] cordinate in SelectedCellIndexes)
            {
                int x = cordinate[0];
                int y = cordinate[1];

                var cell = Convert.ToDecimal(table[x, y]);
                cell += value;
                table[x, y] = cell.ToString();
            }
        }
    }
}