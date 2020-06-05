﻿using MVVM;
using RamTune.Core;
using RamTune.Core.Metadata;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class RomEditorVM : ViewModelBase
    {
        private ActionInvoker _actionHandler = null;

        private DefinitionLoader _definitionLoader;

        private ITableReader _loaderRomManager;

        public RomEditorVM(DefinitionLoader loader)
        {
            _definitionLoader = loader;
            _actionHandler = new ActionInvoker(OnAction);
            MessageBus.Instance.Subscribe(Actions.OPEN_ROM, _actionHandler);
            MessageBus.Instance.Subscribe(Actions.SAVE_ROM, _actionHandler);
            MessageBus.Instance.Subscribe(Actions.RESET_SELECTED_TABLE_CELLS, _actionHandler);
            MessageBus.Instance.Subscribe(Actions.RESET_ALL_TABLE_CELLS, _actionHandler);
        }

        private void OnAction(ActionItem action)
        {
            switch (action.ActionName)
            {
                case Actions.OPEN_ROM:
                    OpenRom(action.Param as Stream);
                    break;

                case Actions.SAVE_ROM:
                    SaveRom(action.Param as string);
                    break;

                case Actions.RESET_ALL_TABLE_CELLS:
                    ResetAllTableCells();
                    break;

                case Actions.RESET_SELECTED_TABLE_CELLS:
                    ResetSelectedTableCells();
                    break;
            }
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
                var selectedItemChangedCommand = Get<RelayCommand>(nameof(SelectedItemChangedCommand));
                if (selectedItemChangedCommand == null)
                {
                    selectedItemChangedCommand = new RelayCommand(parm => SelectedItemChanged(parm as TableDisplayVM));
                    Set(nameof(SelectedItemChangedCommand), selectedItemChangedCommand);
                }

                return selectedItemChangedCommand;
            }
        }

        private void SaveRom(string filePath)
        {
            _loaderRomManager.Save(filePath);
        }

        private void OpenRom(Stream romStream)
        {
            _loaderRomManager = new LoadedRomManager(romStream, _definitionLoader);

            var tables = _loaderRomManager.Rom.Tables.Select(t => new TableDisplayVM(t, _loaderRomManager));

            var data = tables.GroupBy(g => g.Category)
                                  .Select(g => new GroupTableDisplayVM() { Name = g.Key, Tables = g.Select(a => a).ToList() })
                                  .ToList();

            GroupedTables = data;
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