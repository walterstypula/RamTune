using MVVM;
using RamTune.Core;
using RamTune.Core.Metadata;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        private DefinitionLoader _definitionLoader;

        private RelayCommand _openRomCommand;

        private RelayCommand _resetAllTableCellsCommand;

        private RelayCommand _resetSelectedTableCellsCommand;

        private RelayCommand _selectedItemChangedCommand;

        private RelayCommand _saveRomCommand;

        private RelayCommand _openCompareCommand;

        private ITableReader _loaderRomManager;

        public MainWindowVM(DefinitionLoader loader)
        {
            _definitionLoader = loader;
        }

        public TableDisplayVM Table
        {
            get { return Get<TableDisplayVM>(nameof(Table)); }
            set { Set(nameof(Table), value); }
        }

        public IEnumerable<GroupTableDisplayVM> GroupedTables
        {
            get { return Get<IEnumerable<GroupTableDisplayVM>>(nameof(GroupedTables)); }
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

        public ICommand SaveRomCommand
        {
            get
            {
                if (_saveRomCommand == null)
                {
                    _saveRomCommand = new RelayCommand(parm => SaveRom());
                }

                return _saveRomCommand;
            }
        }

        public ICommand OpenCompareCommand => this.InitCommand(ref _openCompareCommand, parm => OpenCompare(), delegate { return true; });

        public ICommand ResetAllTableCellsCommand
        {
            get
            {
                if (_resetAllTableCellsCommand == null)
                {
                    _resetAllTableCellsCommand = new RelayCommand(parm => ResetAllTableCells());
                }

                return _resetAllTableCellsCommand;
            }
        }

        public ICommand ResetSelectedTableCellsCommand
        {
            get
            {
                if (_resetSelectedTableCellsCommand == null)
                {
                    _resetSelectedTableCellsCommand = new RelayCommand(parm => ResetSelectedTableCells());
                }

                return _resetSelectedTableCellsCommand;
            }
        }

        private void ResetSelectedTableCells()
        {
            Table.ResetSelectedTableCells();
        }

        private void ResetAllTableCells()
        {
            Table.ResetAllTableCells();
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
                    _selectedItemChangedCommand = new RelayCommand(parm => SelectedItemChanged(parm as TableDisplayVM));
                }

                return _selectedItemChangedCommand;
            }
        }

        private void SaveRom()
        {
            if (_loaderRomManager == null)
            {
                return;
            }

            var filePath = Common.SaveFile(Common.RomFileFilter);

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            var checksumNote = _loaderRomManager.Save(filePath);
            MessageBox.Show($"Saved {filePath}.\n{checksumNote}", "RamTune");
        }

        private void OpenCompare()
        {
            var window = new Views.CompareWindow { DataContext = new CompareWindowVM(_definitionLoader) };
            window.Show();
        }

        private void OpenRom()
        {
            var romStream = OpenRomFile();
            _loaderRomManager = new LoadedRomManager(romStream, _definitionLoader);

            var tables = _loaderRomManager.Rom.Tables.Select(t => new TableDisplayVM(t, _loaderRomManager));

            var data = tables.GroupBy(g => g.Category)
                                  .Select(g => new GroupTableDisplayVM() { Name = g.Key, Tables = g.Select(a => a).ToList() })
                                  .ToList();

            GroupedTables = data;
        }

        private Stream OpenRomFile()
        {
            string path = Common.SelectFile(Common.RomFileFilter);
            return Common.ReadRomIntoMemory(path);
        }

        private void SelectedItemChanged(TableDisplayVM selectedTable)
        {
            if (selectedTable == null)
            {
                return;
            }

            Table = selectedTable;
        }
    }
}