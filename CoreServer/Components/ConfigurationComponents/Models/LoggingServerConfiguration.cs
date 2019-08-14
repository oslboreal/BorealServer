using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace CoreServer.Components.ConfigurationComponents
{
    public class LoggingServerConfiguration
    {
        private static Mutex mutex = new Mutex();

        // # Logging service configuration file properties.
        public static string LoggingConfigurationDirectory { get; set; } = Environment.CurrentDirectory + "\\Configuration\\";
        public static string LoggingConfigurationPath { get; set; } = LoggingConfigurationDirectory + "LoggingConfiguration.json";

        // # Log creation properties.
        public string LogDirectory { get; set; } = Environment.CurrentDirectory + "\\Logs\\";
        public string LogFilename { get; set; }
        public string LogFullPath { get; set; }

        static LoggingServerConfiguration()
        {
            if (!Directory.Exists(LoggingConfigurationDirectory))
                Directory.CreateDirectory(LoggingConfigurationDirectory);
        }

        /// <summary>
        /// Set up the configuration.
        /// </summary>
        public LoggingServerConfiguration()
        {
            LogFilename = DateTime.Now.ToString("d-M-yyyy");
            LogFullPath = LogDirectory + LogFilename;

            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);
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

                // Validate again if the current directory exists.
                if (!Directory.Exists(Environment.CurrentDirectory + "\\Configuration\\"))
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\Configuration\\");

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
