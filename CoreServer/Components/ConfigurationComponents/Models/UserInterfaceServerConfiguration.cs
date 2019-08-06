using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace CoreServer.Components.ConfigurationComponents.Models
{
    public class UserInterfaceServerConfiguration
    {
        private static Mutex mutex = new Mutex();

        // # Constants.
        public const string UserInterfaceServerConfigurationFile = "UserInterfaceConfiguration.json";

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
            if (!File.Exists(UserInterfaceServerConfigurationFile))
            {
                // Safe threading.
                mutex.WaitOne();

                userInterfaceServerConfiguration = new UserInterfaceServerConfiguration();
                File.WriteAllText(UserInterfaceServerConfigurationFile, JsonConvert.SerializeObject(userInterfaceServerConfiguration));

                mutex.ReleaseMutex();
            }
            else
            {
                userInterfaceServerConfiguration = JsonConvert.DeserializeObject<UserInterfaceServerConfiguration>(File.ReadAllText(UserInterfaceServerConfigurationFile));
            }

            // Configuration file read.
            return userInterfaceServerConfiguration;
        }
    }
}
