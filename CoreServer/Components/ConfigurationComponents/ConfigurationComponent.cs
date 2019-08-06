using CoreServer.Components.ConfigurationComponents.Models;

namespace CoreServer.Components.ConfigurationComponents
{
    public static class ConfigurationComponent
    {
        public static LoggingServerConfiguration LoggingConfiguration
        {
            get
            {
                return LoggingServerConfiguration.RefreshSection();
            }
        }

        public static NetworkingServerConfiguration NetworkingConfiguration
        {
            get
            {
                return NetworkingServerConfiguration.RefreshSection();
            }
        }

        public static UserInterfaceServerConfiguration UserInterfaceConfiguration
        {
            get
            {
                return UserInterfaceServerConfiguration.RefreshSection();
            }
        }
    }
}
