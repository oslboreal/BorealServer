using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreServer.Helpers
{
    public static class LoggingService
    {
        public static string Directory { get; set; } = $"{Environment.CurrentDirectory}\\";
        public static string FileName { get; set; }
        public static string FullPath { get; } = Directory + FileName;

        public static void Log(string message)
        {
            try
            {
                // Validates if the user.
                if (string.IsNullOrEmpty(FileName))
                    FileName = DateTime.Now.ToString("d-M-yyyy");

                // Append extension.
                FileName += ".log";

                // Streamwriter with user statemenet to close the current stream at end.
                using (StreamWriter streamWriter = new StreamWriter(FullPath))
                    streamWriter.WriteLine(message);
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
