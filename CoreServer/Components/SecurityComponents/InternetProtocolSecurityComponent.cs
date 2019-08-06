using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CoreServer.Components.SecurityComponents
{
    public static class InternetProtocolSecurityComponent
    {
        public static List<IPAddress> InternetProtocolBlackList { get; } = new List<IPAddress>();

        static InternetProtocolSecurityComponent()
        {
            // TODO : Load BlackList.
        }

        public static bool IsBlocked(IPAddress address)
        {
            return InternetProtocolBlackList.Where(x => x.ToString() == address.ToString()).FirstOrDefault() != null;
        }

        public static void BlockAddres(IPAddress address)
        {
            // TODO : Add to BlackList.
        }
    }
}
