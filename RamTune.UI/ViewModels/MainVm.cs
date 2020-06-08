using MVVM;
using RamTune.Core.Metadata;
using System.IO;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class MainVm : ViewModelBase
    {
        private readonly RomEditorVm _romEditorVm;
        private readonly SettingsVm _settingsVm;

        public MainVm()
        {
            var loader = new DefinitionLoader();

            var paths = Directory.GetFiles(Configuration.EcuFlashDefinitions, "*.xml", SearchOption.AllDirectories);
            loader.LoadDefinitions(paths);

            _romEditorVm = new RomEditorVm(loader);
            _settingsVm = new SettingsVm();

            SetViewContext(_romEditorVm);

            ActionInvoker actionHandler = OnAction;
            MessageBus.Instance.Subscribe(SettingsVm.Actions.SettingsSave, actionHandler);
            MessageBus.Instance.Subscribe(SettingsVm.Actions.SettingsCancel, actionHandler);
        }

        private void OnAction(ActionItem action)
        {
            switch (action.ActionName)
            {
                case SettingsVm.Actions.SettingsSave:
                    ViewContext = _romEditorVm;
                    break;
                case SettingsVm.Actions.SettingsCancel:
                    ViewContext = _romEditorVm;
                    break;
            }
        }

        public ICommand OpenRomCommand
        {
            get
            {
                var openCommand = Get<RelayCommand>(nameof(OpenRomCommand));
                if (openCommand == null)
                {
                    openCommand = new RelayCommand(param =>
                    {
                        var romStream = OpenRomFile();
                        MessageBus.Instance.Publish(new ActionItem(RomEditorVm.Actions.ROMEDITOR_OPEN_ROM, this, romStream));
                    });
                    Set(nameof(OpenRomCommand), openCommand);
                }

                return openCommand;
            }
        }

        public ICommand ResetAllTableCellsCommand
        {
            get
            {
                var resetAllTableCellsCommand = Get<RelayCommand>(nameof(ResetAllTableCellsCommand));
                if (resetAllTableCellsCommand == null)
                {
                    resetAllTableCellsCommand = new RelayCommand(param =>
                    {
                        MessageBus.Instance.Publish(new ActionItem(RomEditorVm.Actions.ROMEDITOR_RESET_ALL_TABLE_CELLS, this));
                    });
                    Set(nameof(ResetAllTableCellsCommand), resetAllTableCellsCommand);
                }

                return resetAllTableCellsCommand;
            }
        }

        public ICommand ResetSelectedTableCellsCommand
        {
            get
            {
                var resetSelectedTableCellsCommand = Get<RelayCommand>(nameof(ResetSelectedTableCellsCommand));
                if (resetSelectedTableCellsCommand == null)
                {
                    resetSelectedTableCellsCommand = new RelayCommand(param =>
                    {
                        MessageBus.Instance.Publish(new ActionItem(RomEditorVm.Actions.ROMEDITOR_RESET_SELECTED_TABLE_CELLS, this));
                    });
                    Set(nameof(ResetSelectedTableCellsCommand), resetSelectedTableCellsCommand);
                }

                return resetSelectedTableCellsCommand;
            }
        }

        public ICommand SaveRomCommand
        {
            get
            {
                var saveCommand = Get<RelayCommand>(nameof(SaveRomCommand));
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(param =>
                    {
                        var filePath = Common.SaveFile("bin files|*.bin");
                        MessageBus.Instance.Publish(new ActionItem(RomEditorVm.Actions.ROMEDITOR_SAVE_ROM, this, filePath));
                    });
                    Set(nameof(SaveRomCommand), saveCommand);
                }

                return saveCommand;
            }
        }

        public ICommand SettingsCommand
        {
            get
            {
                var settingsCommand = Get<RelayCommand>(nameof(SettingsCommand));
                if (settingsCommand == null)
                {
                    settingsCommand = new RelayCommand(param => SetViewContext(_settingsVm));
                    Set(nameof(SettingsCommand), settingsCommand);
                }

                return settingsCommand;
            }
        }

        public object ViewContext
        {
            get { return Get<object>(nameof(ViewContext)); }
            set { Set(nameof(ViewContext), value); }
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

        private void SetViewContext(ViewModelBase viewModel)
        {
            ViewContext = viewModel;
        }
    }
}