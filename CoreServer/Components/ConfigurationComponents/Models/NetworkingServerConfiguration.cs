using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace CoreServer.Components.ConfigurationComponents.Models
{
    public class NetworkingServerConfiguration
    {
        private static ManualResetEvent ResetEvent = new ManualResetEvent(false);

        // # Constants.
        public static string LoggingConfigurationDirectory { get; set; } = Environment.CurrentDirectory + "\\Configuration\\";
        public static string NetworkingServerConfigurationPath { get; set; } = LoggingConfigurationDirectory + "NetworkingConfiguration.json";

        // # Properties.
        public int Port { get; set; }
        public string Ip { get; set; }

        /// <summary>
        /// Fetch networking configuration state from file.
        /// </summary>
        /// <returns></returns>
        public static NetworkingServerConfiguration RefreshSection()
        {
            NetworkingServerConfiguration networkingServerConfiguration = null;

            // If it doesnt exist then create it.
            if (!File.Exists(NetworkingServerConfigurationPath))
            {
                ResetEvent.Reset();

                networkingServerConfiguration = new NetworkingServerConfiguration();
                File.WriteAllText(NetworkingServerConfigurationPath, JsonConvert.SerializeObject(networkingServerConfiguration));

                ResetEvent.Set();

            }
            else
            {
                networkingServerConfiguration = JsonConvert.DeserializeObject<NetworkingServerConfiguration>(File.ReadAllText(NetworkingServerConfigurationPath));
            }

            // Configuration file read.
            return networkingServerConfiguration;
        }
    }
}
