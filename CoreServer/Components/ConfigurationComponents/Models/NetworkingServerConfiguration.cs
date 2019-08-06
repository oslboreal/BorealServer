using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace CoreServer.Components.ConfigurationComponents.Models
{
    public class NetworkingServerConfiguration
    {
        private static Mutex mutex = new Mutex();

        // # Constants.
        public const string NetworkingServerConfigurationFile = "NetworkingConfiguration.json";

        // # Properties.
        public string Path { get; set; }

        /// <summary>
        /// Fetch networking configuration state from file.
        /// </summary>
        /// <returns></returns>
        public static NetworkingServerConfiguration RefreshSection()
        {
            NetworkingServerConfiguration networkingServerConfiguration = null;

            // If it doesnt exist then create it.
            if (!File.Exists(NetworkingServerConfigurationFile))
            {
                mutex.WaitOne();

                networkingServerConfiguration = new NetworkingServerConfiguration();
                File.WriteAllText(NetworkingServerConfigurationFile, JsonConvert.SerializeObject(networkingServerConfiguration));

                mutex.ReleaseMutex();
            }
            else
            {
                networkingServerConfiguration = JsonConvert.DeserializeObject<NetworkingServerConfiguration>(File.ReadAllText(NetworkingServerConfigurationFile));
            }

            // Configuration file read.
            return networkingServerConfiguration;
        }
    }
}
