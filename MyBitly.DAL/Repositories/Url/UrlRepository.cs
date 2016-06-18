namespace MyBitly.DAL.Repositories
{
    using System.Data.Entity;
    using System.Linq;
    using Attributes;
    using Entities;

    public class UrlRepository : IUrlRepository
    {
        private readonly DbSet<UrlEntity> _dbSet;
        private readonly DbContext _context;

        public UrlRepository(DbContext context)
        {
            this._context = context;
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
    }
}