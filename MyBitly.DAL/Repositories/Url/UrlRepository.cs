namespace MyBitly.DAL.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Attributes;
    using Entities;
    using Filters;
    using Models;

    public class UrlRepository : IUrlRepository
    {
        private readonly DbSet<UrlEntity> _dbSet;

        public UrlRepository(DbContext context)
        {
            this._dbSet = context.Set<UrlEntity>();
        }

        [UnitOfWork]
        public UrlEntity Create(UrlEntity entity)
        {
            return this._dbSet.Add(entity);
        }

        [UnitOfWork]
        public UrlEntity Get(string hash)
        {
            return this._dbSet.FirstOrDefault(x => x.Hash == hash);
        }

        [UnitOfWork]
        public ListPage<UrlEntity> GetList(UrlListFilter filter)
        {
            var totalCount = filter.ApplyCustom(this._dbSet).Select(x=>x.Id).Count();
            var data = filter.Apply(this._dbSet).ToArray();

            return new ListPage<UrlEntity>
            {
                Data = data,
                TotalCount = totalCount
            };
        }
    }
}