namespace MyBitly.BLL.Models
{
    using System;
    using Newtonsoft.Json;
    using DAL.Entities;

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

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "clicks")]
        public int Clicks { get; set; }

        [JsonProperty(PropertyName = "create_date")]
        public DateTime CreateDate { get; set; }

        #region Конвертация

        public static explicit operator ShortenResponse(UrlEntity entity)
        {
            if (entity == null) return null;

            return new ShortenResponse
            {
                Id = entity.Id,
                LongUrl = entity.LongUrl,
                Hash = entity.Hash,
                Title = entity.Title,
                Clicks = entity.Clicks,
                CreateDate = entity.CreateDate
            };
        }

        #endregion
    }
}