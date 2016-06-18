namespace MyBitly.BLL.Models
{
    using Newtonsoft.Json;

    public class Response
    {
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }
    }
}