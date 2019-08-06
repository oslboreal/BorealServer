using Newtonsoft.Json;
using System;

namespace MessengerService.Requests
{
    public enum RequestType
    {
        ExpectResponse,
        ExpectConversation,
        OnlyNotify
    }

    public abstract class RequestBase
    {
        public Guid RequestIdentifier { get; set; }
        public RequestType Type { get; set; }

        public virtual string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
