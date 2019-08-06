using Newtonsoft.Json;

namespace MessengerService.Responses
{
    public class ResponseServerVersion : ResponseBase
    {
        public string Version { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static new ResponseServerVersion Deserialize(string message)
        {
            return JsonConvert.DeserializeObject<ResponseServerVersion>(message);
        }
    }
}
