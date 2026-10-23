using Microsoft.Win32;
using System.IO;

namespace RamTune.UI
{
    /// <summary>
    /// Description of Common.
    /// </summary>
    public class Common
    {
        public const string RomFileFilter = "ROM files|*.bin;*.hex;*.rom|All files|*.*";

        /// <summary>
        /// Reads a ROM file into memory so edits never touch the file until it is saved.
        /// </summary>
        public static Stream ReadRomIntoMemory(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new FileNotFoundException($"Selected file: '{path}' not found");
            }

            return new MemoryStream(File.ReadAllBytes(path));
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

        public static string SaveFile(string filter = "text files|*.txt", string defaultFileName = null)
        {
            var dialog = new SaveFileDialog();

            dialog.Filter = filter;
            dialog.DefaultExt = filter;

            if (!string.IsNullOrEmpty(defaultFileName))
            {
                dialog.InitialDirectory = Path.GetDirectoryName(defaultFileName);
                dialog.FileName = Path.GetFileName(defaultFileName);
            }

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value == true)
            {
                return dialog.FileName;
            }

            return string.Empty;
        }
    }
}