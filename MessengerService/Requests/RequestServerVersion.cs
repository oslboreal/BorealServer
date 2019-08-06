using Newtonsoft.Json;

namespace MessengerService.Requests
{
    public class RequestServerVersion : RequestBase
    {
        public override string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static RequestServerVersion Deserialize(string message)
        {
            return JsonConvert.DeserializeObject<RequestServerVersion>(message);
        }
    }
}
