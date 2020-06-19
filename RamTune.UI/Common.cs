using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using RamTune.UI.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RamTune.UI
{
    public class Settings
    {
        public string DefinitionsDirectory { get; set; }
        public string LogOutputDirectory { get; set; }
    }

    public class RamTuneCommon
    {
        public const string ApplicationFolder = ".ramtune";
        public const string SettingsFile = "settings.json";

        public static string GetApplicationSettingsFilePath()
        {
            var applicationSettings = Common.GetApplicationSettingsFile(ApplicationFolder, SettingsFile);
            return applicationSettings;
        }

        public static void SaveSettings(Settings settings)
        {
            var settingsFile = GetApplicationSettingsFilePath();
            Common.SaveJsonFile(settingsFile, settings);
        }

        public static Settings LoadSettings()
        {
            var settingsFile = GetApplicationSettingsFilePath();
            return Common.LoadJsonFile<Settings>(settingsFile);
        }

        public static IEnumerable<string> GetDefinitions(string definitionsDirectory)
        {
            var paths = Directory.GetFiles(definitionsDirectory, "*.xml", SearchOption.AllDirectories);

            return paths;
        }
    }

    /// <summary>
    /// Description of Common.
    /// </summary>
    public class Common
    {
        public static void SaveJsonFile<T>(string filePath, T obj)
        {
            var file = new FileInfo(filePath);
            file.Directory.Create();

            var json = JsonSerializer.Serialize(obj);
            File.WriteAllText(file.FullName, json);
        }

        public static T LoadJsonFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                var instance = Activator.CreateInstance<T>();
                SaveJsonFile(filePath, instance);
                return instance;
            }

            var json = File.ReadAllText(filePath);
            var obj = JsonSerializer.Deserialize<T>(json);

            return obj;
        }

        public static string GetApplicationFolder(string folderName)
        {
            var userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var settingsFolder = System.IO.Path.Join(userProfilePath, folderName);

            return settingsFolder;
        }

        public static string SelectFolder(string currentLocation = null)
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            dialog.InitialDirectory = currentLocation;
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

        public static void SaveTextToFile(string filePath, string text)
        {
            File.WriteAllText(filePath, text);
        }

        public static string GetApplicationSettingsFile(string applicationFolder, string settingFile)
        {
            applicationFolder = GetApplicationFolder(applicationFolder);
            var settingsFilePath = Path.Join(applicationFolder, settingFile);
            CreateFileFolderStructure(settingsFilePath);

            return settingsFilePath;
        }

        public static void CreateFileFolderStructure(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            fileInfo.Directory?.Create();
        }
    }
}