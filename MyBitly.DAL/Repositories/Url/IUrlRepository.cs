namespace MyBitly.DAL.Repositories
{
    using Entities;

    public interface IUrlRepository
    {
        UrlEntity Create(UrlEntity entity);
        UrlEntity Get(string hash);
    }
}