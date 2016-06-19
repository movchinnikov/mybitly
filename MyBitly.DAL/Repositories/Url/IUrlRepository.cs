namespace MyBitly.DAL.Repositories
{
    using Entities;
    using Filters;
    using Models;

    public interface IUrlRepository
    {
        UrlEntity Create(UrlEntity entity);
        UrlEntity Get(string hash);
        ListPage<UrlEntity> GetList(UrlListFilter filter);
        UrlEntity Increment(string hash);
        void SetPageTitle(UrlEntity entity);
    }
}