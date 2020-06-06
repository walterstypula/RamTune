using MVVM;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class SettingsVm : ViewModelBase
    {
        public string DefinitionsFolder
        {
            get => Get<string>(nameof(DefinitionsFolder));
            set => Set(nameof(DefinitionsFolder), value);
        }

        public string LogOutputFolder
        {
            get => Get<string>(nameof(LogOutputFolder));
            set => Set(nameof(LogOutputFolder), value);
        }

        public ICommand ShowDefinitionsFolderPickerCommand
        {
            get
            {
                var showDefinitionsFolderPickerCommand = Get<RelayCommand>(nameof(ShowDefinitionsFolderPickerCommand));
                if (showDefinitionsFolderPickerCommand == null)
                {
                    showDefinitionsFolderPickerCommand = new RelayCommand(param =>
                    {
                        DefinitionsFolder = Common.SelectFolder();
                    });
                    Set(nameof(ShowDefinitionsFolderPickerCommand), showDefinitionsFolderPickerCommand);
                }

                return showDefinitionsFolderPickerCommand;
            }
        }

        public ICommand ShowLogFolderPickerCommand
        {
            get
            {
                var showFolderPickerCommand = Get<RelayCommand>(nameof(ShowLogFolderPickerCommand));
                if (showFolderPickerCommand == null)
                {
                    showFolderPickerCommand = new RelayCommand(param =>
                    {
                        LogOutputFolder = Common.SelectFolder();
                    });
                    Set(nameof(ShowLogFolderPickerCommand), showFolderPickerCommand);
                }

                return showFolderPickerCommand;
            }
        }
    }
}