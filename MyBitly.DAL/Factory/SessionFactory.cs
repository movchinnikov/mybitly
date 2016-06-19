namespace MyBitly.DAL.Factory
{
    using System.Data.Entity;
    using Castle.Windsor;

    public class SessionFactory : ISessionFactory
    {
        private readonly IWindsorContainer _container;

        public SessionFactory(IWindsorContainer container)
        {
            this._container = container;
        }

        public DbContext OpenSession()
        {
            return this._container.Resolve<DbContext>();
        }
    }
}