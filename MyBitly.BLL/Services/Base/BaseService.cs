namespace MyBitly.BLL.Services
{
    using Castle.Windsor;

    public class BaseService
    {
        protected IWindsorContainer Container { get; private set; }

        public BaseService(IWindsorContainer container)
        {
            this.Container = container;
        }
    }
}