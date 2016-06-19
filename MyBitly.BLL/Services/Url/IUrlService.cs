namespace MyBitly.BLL.Services
{
    using Models;

    public interface IUrlService
    {
        ShortenResponse Shorten(string longUrl);
        ShortenResponse Get(string hash);
        ListResponse LinkHistory(UrlHistoryRequest request);
        ShortenResponse Increment(string hash);
    }
}