using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace CoreServer.Components.ConfigurationComponents
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
        public int ListeningSockets { get; set; }

        public NetworkingServerConfiguration()
        {
            if (!Directory.Exists(LoggingConfigurationDirectory))
                Directory.CreateDirectory(LoggingConfigurationDirectory);
        }

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

                // Default values. 
                networkingServerConfiguration.Ip = "127.0.0.1";
                networkingServerConfiguration.ListeningSockets = 100;
                networkingServerConfiguration.Port = 11000;

                using (StreamWriter writer = new StreamWriter(NetworkingServerConfigurationPath))
                    writer.Write(JsonConvert.SerializeObject(networkingServerConfiguration));

                ResetEvent.Set();
            }
            else
            {
                using (StreamReader r = new StreamReader(NetworkingServerConfigurationPath))
                    networkingServerConfiguration = JsonConvert.DeserializeObject<NetworkingServerConfiguration>(r.ReadToEnd());
            }
            return networkingServerConfiguration;
        }
    }
}
