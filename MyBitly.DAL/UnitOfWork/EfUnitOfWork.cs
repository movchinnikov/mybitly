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
			_factory = sessionFactory;
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
			BeginTransaction(IsolationLevel.ReadCommitted);
		}

		public void BeginTransaction(IsolationLevel lvl)
		{
			Session = _factory.OpenSession();
			_transaction = Session.Database.BeginTransaction(lvl);
		}

		public void Commit()
		{
			Session.SaveChanges();
			_transaction.Commit();
			_transaction.Dispose();
		}

		public void Rollback()
		{
			_transaction.Rollback();
			_transaction.Dispose();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					Session.Dispose();
				}
			}
			_disposed = true;
		}
	}
}