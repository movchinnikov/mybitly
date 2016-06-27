namespace MyBitly.BLL.Models
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class ListResponse
	{
		[JsonProperty(PropertyName = "data")]
		public IEnumerable<object> Data { get; set; }

		[JsonProperty(PropertyName = "count")]
		public int Count { get; set; }
	}
}