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
            try
            {
                return JsonConvert.DeserializeObject<RequestServerVersion>(message);
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
