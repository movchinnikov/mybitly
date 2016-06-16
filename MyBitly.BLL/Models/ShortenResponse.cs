namespace MyBitly.BLL.Models
{
    using Newtonsoft.Json;

    public class ShortenResponse
    {
        [JsonProperty(PropertyName = "long_url")]
        public string LongUrl { get; set; }

        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }
    }

    public class ExceptionResponse
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }

    public class Response
    {
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }
    }
}