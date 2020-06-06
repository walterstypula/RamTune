using MVVM;
using RamTune.Core.Metadata;
using System.IO;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class MainVm : ViewModelBase
    {
        private readonly RomEditorVM _romEditorVm;
        private readonly SettingsVM _settingsVm;

        public MainVm()
        {
            var loader = new DefinitionLoader();

            var paths = Directory.GetFiles(Configuration.EcuFlashDefinitions, "*.xml", SearchOption.AllDirectories);
            loader.LoadDefinitions(paths);

            _romEditorVm = new RomEditorVM(loader);
            _settingsVm = new SettingsVM();

            SetViewContext(_romEditorVm);
        }

        public ICommand OpenRomCommand
        {
            get
            {
                var openCommand = Get<RelayCommand>(nameof(OpenRomCommand));
                if (openCommand == null)
                {
                    openCommand = new RelayCommand(param => OpenRom());
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
                    resetAllTableCellsCommand = new RelayCommand(param => ResetAllTableCells());
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
                    resetSelectedTableCellsCommand = new RelayCommand(param => ResetSelectedTableCells());
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
                    saveCommand = new RelayCommand(param => SaveRom());
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

        private void OpenRom()
        {
            var romStream = OpenRomFile();
            MessageBus.Instance.Publish(new ActionItem(Actions.OPEN_ROM, this, romStream));
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

        private void ResetAllTableCells()
        {
            MessageBus.Instance.Publish(new ActionItem(Actions.RESET_ALL_TABLE_CELLS, this));
        }

        private void ResetSelectedTableCells()
        {
            MessageBus.Instance.Publish(new ActionItem(Actions.RESET_SELECTED_TABLE_CELLS, this));
        }

        private void SaveRom()
        {
            var filePath = Common.SaveFile("bin files|*.bin");
            MessageBus.Instance.Publish(new ActionItem(Actions.SAVE_ROM, this, filePath));
        }

        private void SetViewContext(ViewModelBase viewModel)
        {
            ViewContext = viewModel;
        }
    }
}