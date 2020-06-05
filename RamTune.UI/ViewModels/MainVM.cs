using MVVM;
using RamTune.Core.Metadata;
using System.IO;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class MainVM : ViewModelBase
    {
        public MainVM()
        {
            var loader = new DefinitionLoader();

            //TODO add definitions loading screen
            var paths = Directory.GetFiles(Configuration.EcuFlashDefinitions, "*.xml", SearchOption.AllDirectories);
            loader.LoadDefinitions(paths);

            this.ViewContext = new RomEditorVM(loader);
        }

        public ICommand OpenRomCommand
        {
            get
            {
                var openCommand = Get<RelayCommand>(nameof(OpenRomCommand));
                if (openCommand == null)
                {
                    openCommand = new RelayCommand(parm => OpenRom());
                    Set(nameof(OpenRomCommand), openCommand);
                }

                return openCommand;
            }
        }

        public ICommand SaveRomCommand
        {
            get
            {
                var saveCommand = Get<RelayCommand>(nameof(SaveRomCommand));
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(parm => SaveRom());
                    Set(nameof(SaveRomCommand), saveCommand);
                }

                return saveCommand;
            }
        }

        public ICommand ResetAllTableCellsCommand
        {
            get
            {
                var resetAllTableCellsCommand = Get<RelayCommand>(nameof(ResetAllTableCellsCommand));
                if (resetAllTableCellsCommand == null)
                {
                    resetAllTableCellsCommand = new RelayCommand(parm => ResetAllTableCells());
                    Set(nameof(ResetAllTableCellsCommand), resetAllTableCellsCommand);
                }

                return resetAllTableCellsCommand;
            }
        }

        private void ResetAllTableCells()
        {
            MessageBus.Instance.Publish(new ActionItem(Actions.RESET_ALL_TABLE_CELLS, this));
        }

        public ICommand ResetSelectedTableCellsCommand
        {
            get
            {
                var resetSelectedTableCellsCommand = Get<RelayCommand>(nameof(ResetSelectedTableCellsCommand));
                if (resetSelectedTableCellsCommand == null)
                {
                    resetSelectedTableCellsCommand = new RelayCommand(parm => ResetSelectedTableCells());
                    Set(nameof(ResetSelectedTableCellsCommand), resetSelectedTableCellsCommand);
                }

                return resetSelectedTableCellsCommand;
            }
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

        public object ViewContext
        {
            get { return Get<object>(nameof(ViewContext)); }
            set { Set(nameof(ViewContext), value); }
        }
    }
}