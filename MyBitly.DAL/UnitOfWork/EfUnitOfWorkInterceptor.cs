namespace MyBitly.DAL.UnitOfWork
{
    using System.Reflection;
    using Attributes;
    using Castle.DynamicProxy;
    using Factory;

    public class EfUnitOfWorkInterceptor : IInterceptor
    {
        private readonly ISessionFactory _factory;

        public EfUnitOfWorkInterceptor(ISessionFactory factory)
        {
            _factory = factory;
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
                EfUnitOfWork.Current = new EfUnitOfWork(_factory);
                EfUnitOfWork.Current.BeginTransaction(unitOfWorkAttr.IsolationLevel);

                invocation.Proceed();
                EfUnitOfWork.Current.Commit();
            }
            finally
            {
                if (EfUnitOfWork.Current != null)
                {
                    EfUnitOfWork.Current.Dispose();
                    EfUnitOfWork.Current = null;
                }
            }
        }
    }
}