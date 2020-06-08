using MVVM;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class DirectoryVm : ViewModelBase
    {
        public string Title
        {
            get => Get<string>(nameof(Title));
            set => Set(nameof(Title), value);
        }

        public string Directory
        {
            get => Get<string>(nameof(Directory));
            set => Set(nameof(Directory), value);
        }

        public ICommand ShowPickerCommand
        {
            get
            {
                var showFolderPickerCommand = Get<RelayCommand>(nameof(ShowPickerCommand));
                if (showFolderPickerCommand == null)
                {
                    showFolderPickerCommand = new RelayCommand(param =>
                    {
                        Directory = Common.SelectFolder();
                    });
                    Set(nameof(ShowPickerCommand), showFolderPickerCommand);
                }

                return showFolderPickerCommand;
            }
        }
    }

    public class SettingsVm : ViewModelBase
    {
        public class Actions
        {
            public const string SettingsSave = nameof(SettingsSave);
            public const string SettingsCancel = nameof(SettingsCancel);
        }

        public SettingsVm()
        {
            LogOutputDirectory = new DirectoryVm
            {
                Title = "Log Output Directory"
            };

            DefinitionsDirectory = new DirectoryVm
            {
                Title = "Definitions Directory"
            };
        }

        public DirectoryVm LogOutputDirectory
        {
            get => Get<DirectoryVm>(nameof(LogOutputDirectory));
            set => Set(nameof(LogOutputDirectory), value);
        }

        public DirectoryVm DefinitionsDirectory
        {
            get => Get<DirectoryVm>(nameof(DefinitionsDirectory));
            set => Set(nameof(DefinitionsDirectory), value);
        }

        public ICommand SaveCommand
        {
            get
            {
                var saveCommand = Get<RelayCommand>(nameof(SaveCommand));
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(param =>
                    {
                        MessageBus.Instance.Publish(SettingsVm.Actions.SettingsSave, this);
                    });
                    Set(nameof(SaveCommand), saveCommand);
                }

                return saveCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                var cancelCommand = Get<RelayCommand>(nameof(CancelCommand));
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand(param =>
                    {
                        MessageBus.Instance.Publish(SettingsVm.Actions.SettingsCancel, this);
                    });
                    Set(nameof(CancelCommand), cancelCommand);
                }

                return cancelCommand;
            }
        }
    }
}