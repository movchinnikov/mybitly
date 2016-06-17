namespace MyBitly.DAL.Repositories
{
    using System;
    using System.Data.Entity;
    using Entities;
    using Attributes;

    public class UrlRepository : IUrlRepository, IDisposable
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

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}