using CoreServer.Components.ConfigurationComponents;
using System;
using System.IO;
using System.Threading;

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
        private static Mutex mutex = new Mutex();

        /// <summary>
        /// Method that register exceptions message and their respective stack trace.
        /// </summary>
        /// <param name="ex"></param>
        public static void Log(Exception ex)
        {
            Log($"Message: {ex.Message}\n{ex.StackTrace}", LogType.Error);
        }

        /// <summary>
        /// Method that register custom event message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        public static void Log(string message, LogType logType)
        {
            mutex.WaitOne();
            try
            {
                // TODO : StreamWriter unique instance.
                // Streamwriter with user statemenet to close the current stream at end.
                using (StreamWriter streamWriter = new StreamWriter(ConfigurationComponent.LoggingConfiguration.LogFullPath + GetExtensionByLogType(logType), true))
                {
                    streamWriter.WriteLine($"[{DateTime.Now}] - {message}");
                    streamWriter.Flush();
                }

            }
            catch (Exception ex)
            {
                using (StreamWriter streamWriter = new StreamWriter(ConfigurationComponent.LoggingConfiguration.LogDirectory + "LoggingService.err", true))
                    streamWriter.WriteLine($"[{DateTime.Now}] - {ex.Message}");
            }
            mutex.ReleaseMutex();
        }

        /// <summary>
        /// Method that get a file extension according it LogType
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
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
