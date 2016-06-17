namespace MyBitly.DAL.UnitOfWork
{
    using System;
    using System.Data.Entity;
    using System.Reflection;
    using Attributes;
    using Castle.DynamicProxy;

    public class EfUnitOfWorkInterceptor : IInterceptor
    {
        private readonly DbContext _dbContext;

        public EfUnitOfWorkInterceptor(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void Intercept(IInvocation invocation)
        {
            var unitOfWorkAttr = invocation.MethodInvocationTarget.GetCustomAttribute<UnitOfWorkAttribute>();

            if (EfUnitOfWork.Current != null || unitOfWorkAttr == null)
            {
                invocation.Proceed();
                return;
            }

            try
            {
                EfUnitOfWork.Current = new EfUnitOfWork(this._dbContext);
                EfUnitOfWork.Current.BeginTransaction(unitOfWorkAttr.IsolationLevel);

                try
                {
                    invocation.Proceed();
                    EfUnitOfWork.Current.Commit();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            finally
            {
                EfUnitOfWork.Current = null;
            }
        }
    }
}