namespace MyBitly.BLL.Models
{
	using Newtonsoft.Json;

	public class ExceptionResponse
	{
		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }

		[JsonProperty(PropertyName = "status_txt")]
		public string Code { get; set; }

		[JsonProperty(PropertyName = "status_code")]
		public int StatusCode { get; set; }
	}
}