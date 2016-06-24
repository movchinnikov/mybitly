namespace MyBitly.Tests.Base
{
    using System.Transactions;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using NUnit.Framework;
    using BLL.Exceptions;

    public class TestBase
    {
        protected IWindsorContainer Container;

        protected IWindsorContainer DefaultContainer;

        private TransactionScope _scope;

        public TestBase()
        {
            this.DefaultContainer = new WindsorContainer();
        }

        ~TestBase()
        {
            this.DefaultContainer.Dispose();
        }

        public virtual void SetUp()
        {
            this.Container = new WindsorContainer();
            this.DefaultContainer.AddChildContainer(this.Container);
            this.Container.Register(
                Component.For<IWindsorContainer>().Instance(this.Container));

            this._scope = new TransactionScope();
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (this._scope != null) this._scope.Dispose();
            this.DefaultContainer.RemoveChildContainer(this.Container);
        }

        protected static MyBitlyException InvokeAndAssertException(TestDelegate invokeMethod, string exceptionMessage)
        {
            var ex = Assert.Throws<MyBitlyException>(invokeMethod);
            Assert.That(ex.Message, Does.Contain(exceptionMessage));
            return ex;
        }
    }
}