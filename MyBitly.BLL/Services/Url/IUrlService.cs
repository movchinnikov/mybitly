namespace MyBitly.BLL.Services.Url
{
    using System;
    using Models;
    using Resources;

    public interface IUrlService
    {
        ShortenResponse Shorten(string longUrl);
    }

    public class UrlService : IUrlService
    {
        public ShortenResponse Shorten(string longUrl)
        {
            Uri uriResult;
            var result = Uri.TryCreate(longUrl, UriKind.Absolute, out uriResult);

            if (!result)
                throw new Exception(MyBitleResources.ShortenUrlException);

            var hash = Guid.NewGuid().GetHashCode().ToString("x");

            return new ShortenResponse {LongUrl = longUrl, Hash = hash};
        }
    }
}