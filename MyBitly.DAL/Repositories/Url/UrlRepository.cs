namespace MyBitly.DAL.Repositories
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using Attributes;
    using Entities;
    using Filters;
    using Models;
    using UnitOfWork;

    public class UrlRepository : IUrlRepository
    {
        protected DbContext Session { get { return EfUnitOfWork.Current.Session; } }

        [UnitOfWork]
        public UrlEntity Create(UrlEntity entity)
        {
            return Session.Set<UrlEntity>().Add(entity);
        }

        [UnitOfWork]
        public UrlEntity Get(string hash)
        {
            return Session.Set<UrlEntity>().FirstOrDefault(x => x.Hash == hash);
        }

        [UnitOfWork]
        public ListPage<UrlEntity> GetList(UrlListFilter filter)
        {
            var dbSet = Session.Set<UrlEntity>();

            var totalCount = filter.ApplyCustom(dbSet).Select(x => x.Id).Count();
            var data = filter.Apply(dbSet).ToArray();

            return new ListPage<UrlEntity>
            {
                Data = data,
                TotalCount = totalCount
            };
        }

        [UnitOfWork]
        public UrlEntity Increment(string hash)
        {
            var entity = Session.Set<UrlEntity>().FirstOrDefault(x => x.Hash == hash);
            if (entity == null) return null;
            ++entity.Clicks;

            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    Session.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);

            return entity;
        }

        [UnitOfWork]
        public void SetPageTitle(UrlEntity entity)
        {
            Session.Set<UrlEntity>().Attach(entity);
            Session.Entry(entity).Property(x => x.Title).IsModified = true;
        }
    }
}