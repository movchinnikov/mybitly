namespace MyBitly.DAL.UnitOfWork
{
    using System;
    using System.Data;
    using System.Data.Entity;

    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private DbContextTransaction _transaction;

        public EfUnitOfWork(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public static EfUnitOfWork Current
        {
            get { return _current; }
            set { _current = value; }
        }
        [ThreadStatic]
        private static EfUnitOfWork _current;

        public void BeginTransaction()
        {
            this.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel lvl)
        {
            this._transaction = this._dbContext.Database.BeginTransaction(lvl);
        }

        public void Commit()
        {
            this._dbContext.SaveChanges();
            this._transaction.Commit();
            this._transaction.Dispose();
        }

        public void Rollback()
        {
            this._transaction.Rollback();
            this._transaction.Dispose();
        }
    }
}