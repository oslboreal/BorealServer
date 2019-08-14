using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace CoreServer.Components.ConfigurationComponents
{
    public class UserInterfaceServerConfiguration
    {
        private static Mutex mutex = new Mutex();

        // # Constants.
        public static string UserInterfaceServerConfigurationDirectory { get; set; } = Environment.CurrentDirectory + "\\Configuration\\";
        public static string UserInterfaceServerConfigurationPath { get; set; } = UserInterfaceServerConfigurationDirectory + "UserInterfaceConfiguration.json";

        // # Properties.
        public string Path { get; set; }

        /// <summary>
        /// Fetch UI configuration state from file.
        /// </summary>
        /// <returns></returns>
        public static UserInterfaceServerConfiguration RefreshSection()
        {
            UserInterfaceServerConfiguration userInterfaceServerConfiguration = null;

            // If it doesnt exist then create it.
            if (!File.Exists(UserInterfaceServerConfigurationPath))
            {
                // Safe threading.
                mutex.WaitOne();

                userInterfaceServerConfiguration = new UserInterfaceServerConfiguration();
                File.WriteAllText(UserInterfaceServerConfigurationPath, JsonConvert.SerializeObject(userInterfaceServerConfiguration));

                mutex.ReleaseMutex();
            }
            else
            {
                userInterfaceServerConfiguration = JsonConvert.DeserializeObject<UserInterfaceServerConfiguration>(File.ReadAllText(UserInterfaceServerConfigurationPath));
            }

            // Configuration file read.
            return userInterfaceServerConfiguration;
        }
    }
}
