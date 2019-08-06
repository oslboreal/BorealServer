using Newtonsoft.Json;
using System;

namespace MessengerService.Responses
{
    public enum ResponseStatus
    {
        Ok,
        Error
    }

    public class ResponseBase
    {
        public Guid RequestIdentifier { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public string Novelty { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static ResponseBase Deserialize(string serializedResponse)
        {
            try
            {
                return JsonConvert.DeserializeObject<ResponseBase>(serializedResponse);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}
