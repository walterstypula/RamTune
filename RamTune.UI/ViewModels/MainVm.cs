using MVVM;
using RamTune.Core.Metadata;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class MainVm : ViewModelBase
    {
        private RomEditorVm _romEditorVm;
        private readonly SettingsVm _settingsVm;

        public MainVm()
        {
            MessageBus.Instance.Subscribe(SettingsVm.Actions.SettingsSave, OnAction);
            MessageBus.Instance.Subscribe(SettingsVm.Actions.SettingsCancel, OnAction);

            var settings = RamTuneCommon.LoadSettings();
			_settingsVm = new SettingsVm(settings);

            if (string.IsNullOrWhiteSpace(settings.DefinitionsDirectory))
            {
                SetViewContext(_settingsVm);
                return;
            }

            var definitionFiles = RamTuneCommon.GetDefinitions(_settingsVm.DefinitionsDirectory.Directory);

            _romEditorVm = LoadEditor(definitionFiles);
            SetViewContext(_romEditorVm);
        }

        private static RomEditorVm LoadEditor(IEnumerable<string> definitionFiles)
        {
            var loader = new DefinitionLoader();
            loader.LoadDefinitions(definitionFiles);

            return new RomEditorVm(loader);
        }

        private void OnAction(ActionItem action)
        {
            switch (action.ActionName)
            {
                case SettingsVm.Actions.SettingsSave:

                    if (action.Param is SettingsVm settingsVm)
                    {
                        if (_romEditorVm is null)
                        {
                            var definitions = RamTuneCommon.GetDefinitions(settingsVm.DefinitionsDirectory.Directory);
                            _romEditorVm = LoadEditor(definitions);
                        }

                        RamTuneCommon.SaveSettings(new Settings
                        {
                            DefinitionsDirectory = _settingsVm.DefinitionsDirectory.Directory,
                            LogOutputDirectory = _settingsVm.LogOutputDirectory.Directory
                        });
                    }

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
                if (openCommand != null)
                {
                    return openCommand;
                }

                openCommand = new RelayCommand(param =>
                {
                    var romStream = OpenRomFile();
                    MessageBus.Instance.Publish(new ActionItem(RomEditorVm.Actions.OpenRom, this, romStream));
                });
                Set(nameof(OpenRomCommand), openCommand);

                return openCommand;
            }
        }

        public ICommand ResetAllTableCellsCommand
        {
            get
            {
                var resetAllTableCellsCommand = Get<RelayCommand>(nameof(ResetAllTableCellsCommand));
                if (resetAllTableCellsCommand != null)
                {
                    return resetAllTableCellsCommand;
                }

                resetAllTableCellsCommand = new RelayCommand(param =>
                {
                    MessageBus.Instance.Publish(new ActionItem(RomEditorVm.Actions.RomeditorResetAllTableCells, this));
                });
                Set(nameof(ResetAllTableCellsCommand), resetAllTableCellsCommand);

                return resetAllTableCellsCommand;
            }
        }

        public ICommand ResetSelectedTableCellsCommand
        {
            get
            {
                var resetSelectedTableCellsCommand = Get<RelayCommand>(nameof(ResetSelectedTableCellsCommand));
                if (resetSelectedTableCellsCommand != null)
                {
                    return resetSelectedTableCellsCommand;
                }

                resetSelectedTableCellsCommand = new RelayCommand(param =>
                {
                    MessageBus.Instance.Publish(new ActionItem(RomEditorVm.Actions.ResetSelectedTableCells, this));
                });
                Set(nameof(ResetSelectedTableCellsCommand), resetSelectedTableCellsCommand);

                return resetSelectedTableCellsCommand;
            }
        }

        public ICommand SaveRomCommand
        {
            get
            {
                var saveCommand = Get<RelayCommand>(nameof(SaveRomCommand));
                if (saveCommand != null)
                {
                    return saveCommand;
                }

                saveCommand = new RelayCommand(param =>
                {
                    var filePath = Common.SaveFile("bin files|*.bin");
                    MessageBus.Instance.Publish(new ActionItem(RomEditorVm.Actions.SaveRom, this, filePath));
                });
                Set(nameof(SaveRomCommand), saveCommand);

                return saveCommand;
            }
        }

        public ICommand SettingsCommand
        {
            get
            {
                var settingsCommand = Get<RelayCommand>(nameof(SettingsCommand));
                if (settingsCommand != null)
                {
                    return settingsCommand;
                }

                settingsCommand = new RelayCommand(param => SetViewContext(_settingsVm));
                Set(nameof(SettingsCommand), settingsCommand);

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
            var path = Common.SelectFile("bin files|*.bin");

            if (string.IsNullOrEmpty(path))
            {
                throw new FileNotFoundException($"Selected file: '{path}' not found");
            }

            Stream openRom;
            using (var fileStream = File.OpenRead(path))
            {
                var ms = new MemoryStream();
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