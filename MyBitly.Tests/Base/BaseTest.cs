namespace MyBitly.Tests.Base
{
    using System;
    using System.Transactions;
    using BLL.Exceptions;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using NUnit.Framework;

    public class BaseTest
    {
        public IWindsorContainer Container;
        protected IWindsorContainer DefaultContainer;
        private TransactionScope _scope;

        public BaseTest()
        {
            this.DefaultContainer = new WindsorContainer();
        }

        ~BaseTest()
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