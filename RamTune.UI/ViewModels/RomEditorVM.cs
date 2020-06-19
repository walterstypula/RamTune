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
            public const string OpenRom = nameof(OpenRom);
            public const string SaveRom = nameof(SaveRom);
            public const string ResetSelectedTableCells = nameof(ResetSelectedTableCells);
            public const string RomeditorResetAllTableCells = nameof(RomeditorResetAllTableCells);
        }

        private readonly DefinitionLoader _definitionLoader;

        private ITableReader _loaderRomManager;

        public RomEditorVm(DefinitionLoader loader)
        {
            _definitionLoader = loader;

            ActionInvoker actionHandler = OnAction;
            MessageBus.Instance.Subscribe(RomEditorVm.Actions.OpenRom, actionHandler);
            MessageBus.Instance.Subscribe(RomEditorVm.Actions.SaveRom, actionHandler);
            MessageBus.Instance.Subscribe(RomEditorVm.Actions.ResetSelectedTableCells, actionHandler);
            MessageBus.Instance.Subscribe(RomEditorVm.Actions.RomeditorResetAllTableCells, actionHandler);
        }

        private void OnAction(ActionItem action)
        {
            switch (action.ActionName)
            {
                case RomEditorVm.Actions.OpenRom:
                    OpenRom(action.Param as Stream);
                    break;

                case RomEditorVm.Actions.SaveRom:
                    SaveRom(action.Param as string);
                    break;

                case RomEditorVm.Actions.RomeditorResetAllTableCells:
                    ResetAllTableCells();
                    break;

                case RomEditorVm.Actions.ResetSelectedTableCells:
                    ResetSelectedTableCells();
                    break;
            }
        }

        public TableVm Table
        {
            get { return Get<TableVm>(nameof(Table)); }
            set { Set(nameof(Table), value); }
        }

        public IEnumerable<GroupTableVm> GroupedTables
        {
            get { return Get<IEnumerable<GroupTableVm>>(nameof(GroupedTables)); }
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
                    selectedItemChangedCommand = new RelayCommand(param => SelectedItemChanged(param as TableVm));
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

            var tables = _loaderRomManager.Rom.Tables.Select(t => new TableVm(t, _loaderRomManager));

            var data = tables.GroupBy(g => g.Category)
                                  .Select(g => new GroupTableVm() { Name = g.Key, Tables = g.Select(a => a).ToList() })
                                  .ToList();

            GroupedTables = data;
        }

        private void SelectedItemChanged(TableVm selectedTable)
        {
            if (selectedTable == null)
            {
                return;
            }

            Table = selectedTable;
        }
    }
}