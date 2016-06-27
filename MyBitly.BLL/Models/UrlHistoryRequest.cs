namespace MyBitly.BLL.Models
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class UrlHistoryRequest
	{
		[JsonProperty(PropertyName = "hashes")]
		public IEnumerable<string> Hashes { get; set; }

		[JsonProperty(PropertyName = "hashes")]
		public int Limit { get; set; }

		[JsonProperty(PropertyName = "offset")]
		public int Offset { get; set; }
	}
}