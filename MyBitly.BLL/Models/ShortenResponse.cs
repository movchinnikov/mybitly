namespace MyBitly.BLL.Models
{
    using Newtonsoft.Json;

    public class ShortenResponse
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "long_url")]
        public string LongUrl { get; set; }

        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }

        [JsonProperty(PropertyName = "short_url")]
        public string ShortUrl { get; set; }
    }
}