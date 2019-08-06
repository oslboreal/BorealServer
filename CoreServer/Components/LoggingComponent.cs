using CoreServer.Components.ConfigurationComponents;
using System;
using System.IO;

namespace CoreServer.Components
{
    public enum LogType
    {
        Succes,
        Error,
        Warning
    }

    public static class LoggingComponent
    {
        public static void Log(string message, LogType logType)
        {
            try
            {
                // Streamwriter with user statemenet to close the current stream at end.
                using (StreamWriter streamWriter = new StreamWriter(ConfigurationComponent.LoggingConfiguration.Path + GetExtensionByLogType(logType)))
                    streamWriter.WriteLine($"[{DateTime.Now}] - {message}");
            }
            catch (Exception ex)
            {
                using (StreamWriter streamWriter = new StreamWriter(ConfigurationComponent.LoggingConfiguration.Directory + "LoggingService.err"))
                    streamWriter.WriteLine($"[{DateTime.Now}] - {ex.Message}");
            }
        }

        private static string GetExtensionByLogType(LogType logType)
        {
            switch (logType)
            {
                case LogType.Succes:
                    return ".succes";
                case LogType.Error:
                    return ".error";
                case LogType.Warning:
                    return ".warning";
                default:
                    return null;
            }
        }
    }
}
