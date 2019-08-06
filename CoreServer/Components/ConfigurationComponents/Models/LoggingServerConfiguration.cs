using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace CoreServer.Components.ConfigurationComponents.Models
{
    public class LoggingServerConfiguration
    {
        private static Mutex mutex = new Mutex();

        public static string LoggingConfigurationPath { get; set; } = Environment.CurrentDirectory + "\\Configuration\\" + "LoggingConfiguration.json";

        // # Logging configuration properties.
        public string Directory { get; set; } = Environment.CurrentDirectory + ;
        private string Filename { get; set; }
        public string Path { get; set; }

        public LoggingServerConfiguration()
        {
            Filename = DateTime.Now.ToString("d-M-yyyy");
            Path = Directory + Filename;
        }

        /// <summary>
        /// Fetch logging configuration state from file.
        /// </summary>
        /// <returns></returns>
        public static LoggingServerConfiguration RefreshSection()
        {
            LoggingServerConfiguration loggingServerConfiguration = null;

            // If it doesnt exist then create it.
            if (!File.Exists(LoggingConfigurationPath))
            {
                mutex.WaitOne();

                loggingServerConfiguration = new LoggingServerConfiguration();
                File.WriteAllText(LoggingConfigurationPath, JsonConvert.SerializeObject(loggingServerConfiguration));

                mutex.ReleaseMutex();
            }
            else
            {
                loggingServerConfiguration = JsonConvert.DeserializeObject<LoggingServerConfiguration>(File.ReadAllText(LoggingConfigurationPath));
            }

            // Configuration file read.
            return loggingServerConfiguration;
        }
    }
}
