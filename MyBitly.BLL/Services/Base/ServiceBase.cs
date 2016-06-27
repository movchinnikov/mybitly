namespace MyBitly.BLL.Services
{
    using Castle.Windsor;

    public abstract class ServiceBase
    {
        protected IWindsorContainer Container { get; private set; }

        protected ServiceBase(IWindsorContainer container)
        {
            Container = container;
        }
    }
}