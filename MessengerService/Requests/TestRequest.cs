using Newtonsoft.Json;

namespace MessengerService.Requests
{
    public class TestRequest
    {
        public int RequestNumber { get; set; }
        public string Message { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static TestRequest Deserialize(string message)
        {
            return JsonConvert.DeserializeObject<TestRequest>(message);
        }
    }


}
