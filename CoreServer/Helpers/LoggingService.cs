using System;
using System.IO;

namespace CoreServer.Helpers
{
    public enum LogType
    {
        Succes,
        Error,
        Warning
    }

    public static class LoggingService
    {
        public static string Directory { get; set; } = $"{Environment.CurrentDirectory}\\";
        public static string FileName { get; set; }
        public static string FullPath { get { return Directory + FileName; } }

        public static void Log(string message, LogType logType)
        {
            try
            {
                // Validates if the user.
                if (string.IsNullOrEmpty(FileName))
                    FileName = DateTime.Now.ToString("d-M-yyyy");

                // Streamwriter with user statemenet to close the current stream at end.
                using (StreamWriter streamWriter = new StreamWriter(FullPath + GetExtensionByLogType(logType)))
                    streamWriter.WriteLine($"[{DateTime.Now}] Incoming message: - {message}");
            }
            catch (Exception ex)
            {
                using (StreamWriter streamWriter = new StreamWriter(Directory + "LoggingService.err"))
                    streamWriter.WriteLine($"[{DateTime.Now}] Error message: - {ex.Message}");
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
