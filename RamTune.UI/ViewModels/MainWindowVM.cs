﻿using MVVM;
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

        private void OpenRom()
        {
            _romStream = OpenRomFile();
            _loaderRomManager = new LoadedRomManager(_romStream, _definitionLoader);

            var tables = _loaderRomManager.Rom.Tables.Select(t => new TableDisplayVM(t, _loaderRomManager));

            var data = tables.GroupBy(g => g.Category)
                                  .Select(g => new GroupTableDisplayVM() { Name = g.Key, Tables = g.Select(a => a).ToList() })
                                  .ToList();

            GroupedTables = data;
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