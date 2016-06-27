namespace MyBitly.DAL.Factory
{
    using System.Data.Entity;
    using Castle.Windsor;

    public class SessionFactory : ISessionFactory
    {
        private readonly IWindsorContainer _container;

        public SessionFactory(IWindsorContainer container)
        {
            _container = container;
        }

        public DbContext OpenSession()
        {
            return _container.Resolve<DbContext>();
        }
    }
}