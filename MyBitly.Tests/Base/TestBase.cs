namespace MyBitly.Tests.Base
{
    using System.Data.Entity;
    using System.Transactions;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Common.Exceptions;
    using Common.Params;
    using DAL;
    using DAL.Factory;
    using DAL.Repositories;
    using DAL.UnitOfWork;
    using Fake;
    using NUnit.Framework;

    public class TestBase
    {
        protected IWindsorContainer Container;

        protected IWindsorContainer DefaultContainer;

        protected ISessionFactory Factory;

        private TransactionScope _scope;

        public TestBase()
        {
            DefaultContainer = new WindsorContainer();
        }

        ~TestBase()
        {
            DefaultContainer.Dispose();
        }

        protected virtual void RegisterParamsHelper(ParamsRaw paramsRaw)
        {
            Container.Register(
               Component.For<IParamsHelper>().ImplementedBy<FakeParamsHelper>()
                   .DependsOn(Dependency.OnValue("paramsRaw", paramsRaw)).LifestyleSingleton()
               );
        }

        protected void RegisterDbDependencies()
        {
            Container.Register(
                Component.For<ISessionFactory>().ImplementedBy<SessionFactory>().LifestyleSingleton(),
                Component.For<DbContext>().ImplementedBy<MyBitlyContext>().LifestyleTransient(),
                Component.For<EfUnitOfWorkInterceptor>().LifestyleTransient(),
                Component.For<IUrlRepository>().ImplementedBy<UrlRepository>()
                    .Interceptors<EfUnitOfWorkInterceptor>().LifestyleTransient()
                );

            Factory = Container.Resolve<ISessionFactory>();
        }

        public virtual void SetUp()
        {
            Container = new WindsorContainer();
            DefaultContainer.AddChildContainer(Container);
            Container.Register(
                Component.For<IWindsorContainer>().Instance(Container));

            _scope = new TransactionScope();
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (_scope != null) _scope.Dispose();
            DefaultContainer.RemoveChildContainer(Container);
        }

        protected static MyBitlyException InvokeAndAssertException(TestDelegate invokeMethod, string exceptionMessage)
        {
            var ex = Assert.Throws<MyBitlyException>(invokeMethod);
            Assert.That(ex.Message, Does.Contain(exceptionMessage));
            return ex;
        }
    }
}