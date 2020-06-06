using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace RamTune.UI
{
    /// <summary>
    /// Description of Common.
    /// </summary>
    public class Common
    {
        public static string SelectFolder()
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            var result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }

            return string.Empty;
        }

        public static string SelectFile(string filter)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = filter;
            dialog.DefaultExt = filter;

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value == true)
            {
                return dialog.FileName;
            }

            return string.Empty;
        }

        public static string SaveFile(string filter = "text files|*.txt")
        {
            var dialog = new SaveFileDialog();

            dialog.Filter = filter;
            dialog.DefaultExt = filter;

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value == true)
            {
                return dialog.FileName;
            }

            return string.Empty;
        }
    }
}