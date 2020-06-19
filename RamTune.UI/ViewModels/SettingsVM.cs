using System.Diagnostics.Contracts;
using MVVM;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
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

        public SettingsVm(Settings settings)
            :this()
        {
            LogOutputDirectory.Directory = settings.LogOutputDirectory;
            DefinitionsDirectory.Directory = settings.DefinitionsDirectory;
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
                if (saveCommand != null)
                {
                    return saveCommand;
                }

                saveCommand = new RelayCommand(param =>
                {
                    MessageBus.Instance.Publish(Actions.SettingsSave, this, this);
                });
                Set(nameof(SaveCommand), saveCommand);

                return saveCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                var cancelCommand = Get<RelayCommand>(nameof(CancelCommand));
                if (cancelCommand != null)
                {
                    return cancelCommand;
                }

                cancelCommand = new RelayCommand(param =>
                {
                    MessageBus.Instance.Publish(Actions.SettingsCancel, this);
                });
                Set(nameof(CancelCommand), cancelCommand);

                return cancelCommand;
            }
        }
    }
}