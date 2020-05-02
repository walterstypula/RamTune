using RamTune.Core.Metadata;
using RamTune.UI.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace RamTune.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var loader = new DefinitionLoader();

            //TODO add definitions loading screen
            var paths = Directory.GetFiles(Configuration.EcuFlashDefinitions, "*.xml", SearchOption.AllDirectories);
            loader.LoadDefinitions(paths);

            var window = new MainWindow();
            var mainWindowVM = new MainWindowVM(loader);
            window.DataContext = mainWindowVM;
            window.Show();
        }

        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
            e.Handled = true;
        }
    }
}