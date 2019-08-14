namespace CoreServer.Components.ConfigurationComponents
{
    public static class Configuration
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
    }
}
