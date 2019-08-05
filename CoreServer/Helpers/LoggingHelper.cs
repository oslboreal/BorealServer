using System;
using System.IO;

namespace CoreServer.Helpers
{
    public static class LoggingService
    {
        public static string Directory { get; set; } = $"{Environment.CurrentDirectory}\\";
        public static string FileName { get; set; }
        public static string FullPath { get { return Directory + FileName; } }

        public static void Log(string message)
        {
            try
            {
                // Validates if the user.
                if (string.IsNullOrEmpty(FileName))
                    FileName = DateTime.Now.ToString("d-M-yyyy") + ".log";

                // Streamwriter with user statemenet to close the current stream at end.
                using (StreamWriter streamWriter = new StreamWriter(FullPath))
                    streamWriter.WriteLine($"[{DateTime.Now}] Incoming message: - {message}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Log(string message, string fileName)
        {
            FileName = fileName;
            Log(message);
        }
    }
}
