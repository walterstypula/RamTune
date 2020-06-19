using MVVM;
using System.Windows.Input;

namespace RamTune.UI.ViewModels
{
    public class DirectoryVm : ViewModelBase
    {
        public string Title
        {
            get { return Get<string>(nameof(Title)); }
            internal set { Set(nameof(Title), value); }
        }

        public string Directory
        {
            get { return Get<string>(nameof(Directory)); }
            set { Set(nameof(Directory), value); }
        }

        public ICommand ShowPickerCommand
        {
            get
            {
                var showFolderPickerCommand = Get<RelayCommand>(nameof(ShowPickerCommand));
                if (showFolderPickerCommand != null)
                {
                    return showFolderPickerCommand;
                }

                showFolderPickerCommand = new RelayCommand(param =>
                {
                    var selectedFolder = Common.SelectFolder(param as string);

                    Directory = string.IsNullOrEmpty(selectedFolder)
                                    ? Directory
                                    : selectedFolder;
                });
                Set(nameof(ShowPickerCommand), showFolderPickerCommand);

                return showFolderPickerCommand;
            }
        }
    }
}