using MVVM;
using RamTune.Core;
using RamTune.Core.Metadata;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class RomEditorVm : ViewModelBase
    {
        public class Actions
        {
            public const string ROMEDITOR_OPEN_ROM = nameof(ROMEDITOR_OPEN_ROM);
            public const string ROMEDITOR_SAVE_ROM = nameof(ROMEDITOR_SAVE_ROM);
            public const string ROMEDITOR_RESET_SELECTED_TABLE_CELLS = nameof(ROMEDITOR_RESET_SELECTED_TABLE_CELLS);
            public const string ROMEDITOR_RESET_ALL_TABLE_CELLS = nameof(ROMEDITOR_RESET_ALL_TABLE_CELLS);
        }

        private readonly DefinitionLoader _definitionLoader;

        private ITableReader _loaderRomManager;

        public RomEditorVm(DefinitionLoader loader)
        {
            _definitionLoader = loader;
            ActionInvoker actionHandler = OnAction;
            MessageBus.Instance.Subscribe(RomEditorVm.Actions.ROMEDITOR_OPEN_ROM, actionHandler);
            MessageBus.Instance.Subscribe(RomEditorVm.Actions.ROMEDITOR_SAVE_ROM, actionHandler);
            MessageBus.Instance.Subscribe(RomEditorVm.Actions.ROMEDITOR_RESET_SELECTED_TABLE_CELLS, actionHandler);
            MessageBus.Instance.Subscribe(RomEditorVm.Actions.ROMEDITOR_RESET_ALL_TABLE_CELLS, actionHandler);
        }

        private void OnAction(ActionItem action)
        {
            switch (action.ActionName)
            {
                case RomEditorVm.Actions.ROMEDITOR_OPEN_ROM:
                    OpenRom(action.Param as Stream);
                    break;

                case RomEditorVm.Actions.ROMEDITOR_SAVE_ROM:
                    SaveRom(action.Param as string);
                    break;

                case RomEditorVm.Actions.ROMEDITOR_RESET_ALL_TABLE_CELLS:
                    ResetAllTableCells();
                    break;

                case RomEditorVm.Actions.ROMEDITOR_RESET_SELECTED_TABLE_CELLS:
                    ResetSelectedTableCells();
                    break;
            }
        }

        public TableDisplayVm Table
        {
            get { return Get<TableDisplayVm>(nameof(Table)); }
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
                    selectedItemChangedCommand = new RelayCommand(param => SelectedItemChanged(param as TableDisplayVm));
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

            var tables = _loaderRomManager.Rom.Tables.Select(t => new TableDisplayVm(t, _loaderRomManager));

            var data = tables.GroupBy(g => g.Category)
                                  .Select(g => new GroupTableDisplayVM() { Name = g.Key, Tables = g.Select(a => a).ToList() })
                                  .ToList();

            GroupedTables = data;
        }

        private void SelectedItemChanged(TableDisplayVm selectedTable)
        {
            if (selectedTable == null)
            {
                return;
            }

            Table = selectedTable;
        }
    }
}