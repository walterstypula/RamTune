using System.Configuration;

namespace RamTune.UI
{
    public static class Configuration
    {
        public static string EcuFlashDefinitions

        {
            get

            {
                var ecuFlashDefinitions = ConfigurationManager.AppSettings["EcuFlashDefinitions"];

                if (ecuFlashDefinitions == null)

                {
                    throw new ConfigurationErrorsException("EcuFlashDefinitions configuration is missing in config file");
                }

                return ecuFlashDefinitions;
            }
        }
    }
}