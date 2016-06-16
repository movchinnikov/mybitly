namespace MyBitly.BLL.Services
{
    using System;
    using Exceptions;
    using Models;
    using Resources;

    public class UrlService : IUrlService
    {
        public ShortenResponse Shorten(string longUrl)
        {
            if (string.IsNullOrWhiteSpace(longUrl))
                throw new MyBitlyException(MyBitleResources.UrlIsNullOrEmptyException)
                {
                    Code = MyBitleResources.EMPTY_ARG_URL,
                    StatusCode = 100
                };

            Uri uriResult;
            var result = Uri.TryCreate(longUrl, UriKind.Absolute, out uriResult);

            if (!result)
                throw new MyBitlyException(MyBitleResources.ShortenUrlException)
                {
                    Code = MyBitleResources.INVALID_ARG_URL,
                    StatusCode = 101
                };

            var hash = Guid.NewGuid().GetHashCode().ToString("x");

            return new ShortenResponse {LongUrl = longUrl, Hash = hash};
        }
    }
}