using Newtonsoft.Json;

namespace MessengerService.Responses
{
    class TestResponse
    {
        public int RequestNumber { get; set; }
        public string Message { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static TestResponse Deserialize(string message)
        {
            return JsonConvert.DeserializeObject<TestResponse>(message);
        }
    }
}
