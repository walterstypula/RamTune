using MVVM;
using RamTune.Core;
using RamTune.Core.Metadata;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        private DefinitionLoader _definitionLoader;

        private RelayCommand _openRomCommand;

        private RelayCommand _selectedItemChangedCommand;

        private Stream _romStream;
        private LoadedRomManager _loaderRomManager;

        public List<string> ColumnHeaders
        {
            get { return this.Get<List<string>>(nameof(ColumnHeaders)); }
            set { this.Set<List<string>>(nameof(ColumnHeaders), value); }
        }

        public List<string> RowHeaders
        {
            get { return this.Get<List<string>>(nameof(RowHeaders)); }
            set { this.Set<List<string>>(nameof(RowHeaders), value); }
        }

        public string[,] Data
        {
            get { return this.Get<string[,]>(nameof(Data)); }
            set { this.Set<string[,]>(nameof(Data), value); }
        }

        public MainWindowVM(DefinitionLoader loader)
        {
            _definitionLoader = loader;
        }

        public IEnumerable<IGrouping<string, Table>> GroupedTables
        {
            get { return Get<IEnumerable<IGrouping<string, Table>>>(nameof(GroupedTables)); }
            set { Set(nameof(GroupedTables), value); }
        }

        public ICommand OpenRomCommand
        {
            get
            {
                if (_openRomCommand == null)
                {
                    _openRomCommand = new RelayCommand(parm => OpenRom());
                }

                return _openRomCommand;
            }
        }

        public Definition LoadedRomDefinition
        {
            get { return Get<Definition>(nameof(LoadedRomDefinition)); }
            set { Set(nameof(LoadedRomDefinition), value); }
        }

        public ICommand SelectedItemChangedCommand
        {
            get
            {
                if (_selectedItemChangedCommand == null)
                {
                    _selectedItemChangedCommand = new RelayCommand(parm => SelectedItemChanged(parm as Table));
                }

                return _selectedItemChangedCommand;
            }
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

        private void OpenRom()
        {
            _romStream = OpenRomFile();
            _loaderRomManager = new LoadedRomManager(_romStream, _definitionLoader);
            GroupedTables = _loaderRomManager.Rom.Tables.GroupBy(g => g.Category);
        }

        private Stream OpenRomFile()
        {
            string path = Common.SelectFile("bin files|*.bin");

            if (string.IsNullOrEmpty(path))
            {
                throw new FileNotFoundException($"Selected file: '{path}' not found");
            }

            Stream openRom;
            using (FileStream fileStream = File.OpenRead(path))
            {
                MemoryStream ms = new MemoryStream();
                ms.SetLength(fileStream.Length);
                fileStream.Read(ms.GetBuffer(), 0, (int)fileStream.Length);
                openRom = ms;
            }

            return openRom;
        }

        private void SelectedItemChanged(Table selectedTable)
        {
            if (selectedTable == null)
            {
                return;
            }

            //Tables always start at zero
            //get data for x axis
            ColumnHeaders?.Clear();
            RowHeaders?.Clear();

            var xAxis = selectedTable.Axis.FirstOrDefault(t => t.Type == TableType.XAxis || t.Type == TableType.StaticXAxis);
            var yAxis = selectedTable.Axis.FirstOrDefault(t => t.Type == TableType.YAxis || t.Type == TableType.StaticYAxis);

            var xElements = xAxis?.Elements ?? 1;
            var yElements = yAxis?.Elements ?? 1;

            if (xAxis == null)
            {
                //Display YAxis in XAxis, display only.
                xAxis = yAxis;
                yAxis = null;

                xElements = yElements;
                yElements = 1;
            }

            ColumnDesc = xAxis != null ? $"{xAxis.Name}  -  {xAxis?.Scaling?.Units}" : string.Empty;
            RowDesc = yAxis != null ? $"{yAxis.Name}  -  {yAxis?.Scaling?.Units}" : string.Empty;
            TableDesc = $"{selectedTable.Name}  -  {selectedTable.Scaling.Units}";

            ColumnHeaders = xAxis != null
                                ? _loaderRomManager.LoadTableData(xAxis, xAxis.Elements).Cast<string>().ToList()
                                : new List<string>() { string.Empty };

            RowHeaders = yAxis != null
                                ? _loaderRomManager.LoadTableData(yAxis, yAxis.Elements).Cast<string>().ToList()
                                : new List<string>() { string.Empty };

            Data = _loaderRomManager.LoadTableData(selectedTable, xElements, yElements);
        }
    }
}