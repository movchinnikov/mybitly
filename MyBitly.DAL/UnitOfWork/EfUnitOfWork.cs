namespace MyBitly.DAL.UnitOfWork
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using Factory;

    public class EfUnitOfWork : IUnitOfWork
    {
        private DbContextTransaction _transaction;
        private bool _disposed;
        private readonly ISessionFactory _factory;

        public EfUnitOfWork(ISessionFactory sessionFactory)
        {
            this._factory = sessionFactory;
        }

        public DbContext Session { get; private set; }

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
            this.Session = this._factory.OpenSession();
            this._transaction = this.Session.Database.BeginTransaction(lvl);
        }

        public void Commit()
        {
            this.Session.SaveChanges();
            this._transaction.Commit();
            this._transaction.Dispose();
        }

        public void Rollback()
        {
            this._transaction.Rollback();
            this._transaction.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this.Session.Dispose();
                }
            }
            this._disposed = true;
        }
    }
}